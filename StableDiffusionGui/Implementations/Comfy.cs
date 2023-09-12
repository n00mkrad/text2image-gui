using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static StableDiffusionGui.Implementations.ComfyData;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Implementations
{
    public class Comfy : ImplementationBase, IImplementation
    {
        public List<string> LastMessages { get => _lastMessages; }
        private List<string> _lastMessages = new List<string>();
        private static bool _hasErrored = false;
        private TtiSettings _lastSettings = null;

        public static readonly int ComfyPort = 8189;
        private static readonly HttpClient _webClient = new HttpClient();

        private OperationOrder _opOrder = new OperationOrder(new List<OperationOrder.LoopAction> {
            OperationOrder.LoopAction.Prompt,
            OperationOrder.LoopAction.InitImg,
            OperationOrder.LoopAction.InitStrength,
            OperationOrder.LoopAction.Iteration, 
            /* OperationOrder.LoopAction.LoraWeight, */
            OperationOrder.LoopAction.Scale,
            OperationOrder.LoopAction.RefineStrength,
            OperationOrder.LoopAction.Step,
        });
        private Action<OperationOrder.LoopAction> _loopIterations, _loopPrompts, _loopScales, _loopSteps, _loopRefinerStrengths, _loopInits, _loopInitStrengths/*, _loopLoraWeights*/;

        public async Task Run(TtiSettings s, string outPath)
        {
            _lastSettings = s;
            float[] initStrengths = s.InitStrengths.Select(n => 1f - n).ToArray();
            var cachedModels = Models.GetModels((Enums.Models.Type)(-1), Implementation.Comfy);
            var baseModels = cachedModels.Where(m => m.Type == Enums.Models.Type.Normal).ToList();
            var refinerModels = cachedModels.Where(m => m.Type == Enums.Models.Type.Refiner).ToList();
            Model model = TtiUtils.CheckIfModelExists(s.Model, Implementation.Comfy, baseModels);
            string vaeName = s.Vae.NullToEmpty().Replace(Constants.NoneMdl, ""); // VAE model name
            Model vae = Models.GetModel(Models.GetVaes(), vaeName);
            List<Model> controlnetMdls = Models.GetControlNets();

            if (model == null)
                return;

            OrderedDictionary initImages = s.InitImgs != null && s.InitImgs.Length > 0 ? await TtiUtils.CreateResizedInitImagesIfNeeded(s.InitImgs.ToList(), s.Res, s.ResizeGravity, true) : null;
            long startSeed = s.Seed;
            bool refine = s.RefinerStrengths.Any(rs => rs >= 0.05f);
            string mode = NmkdiffUtils.GetGenerationMode(s, model);

            string missingRefinerMsg = "No Refiner model file has been set.\nPlease set one or disable image refining.";
            Model refineModel = refine ? TtiUtils.CheckIfModelExists(s.ModelAux, Implementation.Comfy, refinerModels, missingRefinerMsg) : null;

            if (refine && refineModel == null)
                return;

            var generations = new List<GenerationInfo>() { new GenerationInfo() };

            var currentGeneration = new GenerationInfo
            {
                BaseResolution = s.Res,
                TargetResolution = s.UpscaleTargetRes,
                UpscaleMethod = s.UpscaleMethod,
                NegativePrompt = s.NegativePrompt,
                Model = model.FullName,
                ModelRefiner = refineModel == null ? "" : refineModel.FullName,
                Vae = vae == null ? "" : vae.FullName,
                Sampler = s.Sampler,
                Upscaler = Config.Instance.UpscaleEnable ? Models.GetUpscalers().Where(m => m.Name == Config.Instance.EsrganModel).FirstOrDefault().FullName : "",
                ClipSkip = (Config.Instance.ModelSettings.Get(model.Name, new Models.ModelSettings()).ClipSkip * -1) - 1,
                SaveOriginalAndUpscale = Config.Instance.SaveUnprocessedImages,
            };

            foreach (ControlnetInfo cnet in s.Controlnets.Where(cn => cn != null && cn.Strength > 0.001f && cn.Model != Constants.NoneMdl))
            {
                var cnetModel = controlnetMdls.Where(m => m.FormatIndependentName == cnet.Model).FirstOrDefault();
                if (cnetModel == null) continue;
                currentGeneration.Controlnets.Add(new ControlnetInfo { Model = cnetModel.FullName, Preprocessor = cnet.Preprocessor, Strength = cnet.Strength });
            }

            currentGeneration.Loras = s.Loras.Select(lora => new KeyValuePair<string, float>(lora.Key, lora.Value.First())).ToList();

            List<string> processedPrompts = null;
            TextToImage.CurrentTaskSettings.ProcessedAndRawPrompts = new EasyDict<string, string>();

            int currentIteration = 0;
            float currentLoraWeight = 1.0f;

            void NextAction(OperationOrder.LoopAction thisAction)
            {
                if (_opOrder.SeedResetActions.Contains(thisAction))
                    s.Seed = startSeed;

                // if (_opOrder.SeedIncrementActions.Contains(thisAction) && !s.LockSeed)
                //     s.Seed++;

                RunNextLoopAction(thisAction, () => FinalAction());
            }

            void FinalAction()
            {
                string currPrompt = processedPrompts[currentIteration];

                // if (s.Loras != null && s.Loras.Count == 1)
                //     foreach (var lora in s.Loras)
                //         currPrompt = currPrompt.Replace($"{lora.Key}Weight", currentLoraWeight.ToStringDot("0.0###"));

                currentGeneration.Prompt = currPrompt;
                generations.Add(currentGeneration.ToJson().FromJson<GenerationInfo>());
            }

            _loopPrompts = (thisAction) =>
            {
                foreach (string prompt in s.Prompts)
                {
                    processedPrompts = PromptWildcardUtils.ApplyWildcardsAll(prompt, s.Iterations, false);
                    TextToImage.CurrentTaskSettings.ProcessedAndRawPrompts = new EasyDict<string, string>(processedPrompts.Distinct().ToDictionary(x => x, x => prompt));
                    NextAction(thisAction);
                }
            };

            _loopIterations = (thisAction) =>
            {
                currentIteration = 0;

                for (int i = 0; i < s.Iterations; i++)
                {
                    currentGeneration.Seed = s.Seed;
                    NextAction(thisAction);
                    currentIteration++;

                    if (_opOrder.SeedIncrementActions.Contains(thisAction) && s.Iterations > 1 && !s.LockSeed)
                        s.Seed++;
                }
            };

            _loopScales = (thisAction) =>
            {
                foreach (float scale in s.ScalesTxt)
                {
                    currentGeneration.Scale = scale.Clamp(0.01f, 1000f);
                    NextAction(thisAction);
                }
            };

            _loopSteps = (thisAction) =>
            {
                foreach (int stepCount in s.Steps)
                {
                    currentGeneration.Steps = stepCount;
                    NextAction(thisAction);
                }
            };

            _loopRefinerStrengths = (thisAction) =>
            {
                foreach (float strength in s.RefinerStrengths)
                {
                    currentGeneration.RefinerStrength = strength;
                    NextAction(thisAction);
                }
            };

            _loopInits = (thisAction) =>
            {
                if (initImages == null) // No init image(s)
                {
                    NextAction(thisAction);
                }
                else // With init image(s)
                {
                    foreach (string initImg in initImages.Values)
                    {
                        currentGeneration.InitImg = initImg;
                        NextAction(thisAction);
                    }
                }
            };

            _loopInitStrengths = (thisAction) =>
            {
                foreach (float strength in s.InitStrengthsReverse)
                {
                    currentGeneration.InitStrength = s.ImgMode != Enums.StableDiffusion.ImgMode.InitializationImage ? 1f : strength; // Lock to 1.0 when using inpainting (OBSOLETE WITH COMFY?)

                    if (s.ImgMode == ImgMode.ImageMask)
                        currentGeneration.MaskPath = Inpainting.MaskedImagePath;
                    else if (s.ImgMode == ImgMode.Outpainting)
                        currentGeneration.MaskPath = currentGeneration.InitImg;
                    // else if (s.ImgMode == Enums.StableDiffusion.ImgMode.Outpainting)
                    //     args["inpaintMask"] = "--force_outpaint";
                    // else
                    //     args["inpaintMask"] = "";

                    NextAction(thisAction);
                }
            };
            // 
            // _loopLoraWeights = (thisAction) =>
            // {
            //     foreach (float weight in s.Loras.First().Value)
            //     {
            //         currentLoraWeight = weight;
            //         NextAction(thisAction);
            //     }
            // };

            RunLoopAction(_opOrder.LoopOrder.First());

            Logger.ClearLogBox();
            Logger.Log($"Running Stable Diffusion - {s.Res.Width}x{s.Res.Height}, Starting Seed: {startSeed}");

            string initsStr = initImages != null ? $" and {initImages.Count} image{(initImages.Count != 1 ? "s" : "")} using {initStrengths.Length} strength{(initStrengths.Length != 1 ? "s" : "")}" : "";
            Logger.Log($"{s.Prompts.Length} prompt{(s.Prompts.Length != 1 ? "s" : "")} * {s.Iterations} image(s) * {s.Steps.Length} step value(s) * {s.ScalesTxt.Length} scale value(s) * {s.RefinerStrengths.Length} refine value(s){initsStr} = {generations.Count - 1} image(s) total.");

            var scriptArgs = new List<string>
            {
                $"--port {ComfyPort}",
                $"--output-directory {outPath.Wrap(true)}",
                $"--preview-method none",
                $"--disable-xformers", // Obsolete since Pytorch 2.0
                $"--cuda-malloc",
                $"--{ComfyUtils.GetVramArg()}",
            };

            if (Config.Instance.FullPrecision)
                scriptArgs.Add("--force-fp32");

            string newStartupSettings = $"{string.Join("", scriptArgs)}{Config.Instance.CudaDeviceIdx}";

            if (!TtiProcess.IsAiProcessRunning || (TtiProcess.IsAiProcessRunning && TtiProcess.LastStartupSettings != newStartupSettings))
            {
                if (TextToImage.Canceled) return;

                PatchUtils.PatchDiffusers();
                Logger.Log($"(Re)starting Comfy. Process running: {TtiProcess.IsAiProcessRunning} - Prev startup string: '{TtiProcess.LastStartupSettings}' - New startup string: '{newStartupSettings}'", true);
                TtiProcess.LastStartupSettings = newStartupSettings;

                Process py = OsUtils.NewProcess(true, logAction: HandleOutput);
                TextToImage.CurrentTask.Processes.Add(py);

                py.StartInfo.Arguments = $"/C cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSdCommand()} && {Constants.Files.VenvActivate} && python repo/comfyui/main.py {string.Join(" ", scriptArgs)}";
                Logger.Log("cmd.exe " + py.StartInfo.Arguments, true);

                if (TtiProcess.CurrentProcess != null)
                {
                    TtiProcess.ProcessExistWasIntentional = true;
                    OsUtils.KillProcessTree(TtiProcess.CurrentProcess.Id);
                }

                ResetLogger();

                Logger.Log($"Loading Stable Diffusion with model {s.Model.Trunc(80).Wrap()}{(refineModel != null ? $" and {s.ModelAux.Trunc(80).Wrap()}" : "")}...");

                var sw = new NmkdStopwatch();
                TtiProcess.ProcessExistWasIntentional = false;
                OsUtils.StartProcess(py, killWithParent: true);
                TtiProcess.CurrentProcess = py;

                Task.Run(() => TtiProcess.CheckStillRunning());

                await Logger.WaitForMessageAsync("To see the GUI go to:", contains: true);
                Logger.Log($"Comfy startup time: {sw.ElapsedMs} ms", true);
            }
            else
            {
                ResetLogger();
                TextToImage.CurrentTask.Processes.Add(TtiProcess.CurrentProcess);
            }

            foreach (var genInfo in generations.Where(g => g.Model.IsNotEmpty()))
            {
                var prompt = ComfyPrompts.GetMainWorkflow(genInfo);
                EasyDict<string, object> meta = new EasyDict<string, object> { { "GenerationInfoJson", genInfo.GetSerializeClone() } };
                string response = await SendPrompt(prompt, meta);

                if (response.IsEmpty())
                    break;
            }
        }

        private enum ComfyEndpoint { Prompt, Queue, Interrupt }

        private static async Task<string> SendPrompt(EasyDict<string, ComfyWorkflow.NodeInfo> prompt, EasyDict<string, object> pngMetadata = null)
        {
            try
            {
                var req = new ComfyWorkflow.PromptRequest() { ClientId = Paths.SessionTimestampUnix.ToString(), Prompt = prompt };

                foreach (var pair in (pngMetadata == null ? new EasyDict<string, object>() : pngMetadata))
                    req.ExtraData.ExtraPnginfo[pair.Key] = pair.Value;

                if (Program.Debug)
                    File.WriteAllText(IoUtils.GetAvailablePath(Path.Combine(Paths.GetLogPath(), "req.json")), req.Serialize(true));

                string resp = await ApiPost(req.ToString());
                Logger.Log($"[<-] {resp}", true, filename: Constants.Lognames.Api);
                return resp;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error creating request: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
                return "";
            }
        }

        private async static Task<string> ApiPost(string data = "", ComfyEndpoint endpoint = ComfyEndpoint.Prompt)
        {
            try
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                var stringContent = data.IsEmpty() ? null : new StringContent(data, Encoding.UTF8, "application/json");
                string baseUrl = $"http://127.0.0.1:{ComfyPort}/{endpoint.ToString().Lower()}";
                Logger.Log($"[->] {baseUrl} - {data.Trunc(150)}", true, filename: Constants.Lognames.Api);
                var response = await _webClient.PostAsync(baseUrl, stringContent);
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Logger.Log($"API Error: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
                TextToImage.Cancel("API Error.", false, TextToImage.CancelMode.SoftKill);
            }

            return "";
        }

        void RunNextLoopAction(OperationOrder.LoopAction previousAction, Action doneAction)
        {
            int actionIndex = _opOrder.LoopOrder.IndexOf(previousAction);

            if (actionIndex + 1 < _opOrder.LoopOrder.Count)
                RunLoopAction(_opOrder.LoopOrder.ElementAt(actionIndex + 1));
            else
                doneAction(); // End
        }

        void RunLoopAction(OperationOrder.LoopAction a)
        {
            if (a == OperationOrder.LoopAction.Prompt) _loopPrompts(a);
            if (a == OperationOrder.LoopAction.Iteration) _loopIterations(a);
            if (a == OperationOrder.LoopAction.Scale) _loopScales(a);
            if (a == OperationOrder.LoopAction.Step) _loopSteps(a);
            if (a == OperationOrder.LoopAction.RefineStrength) _loopRefinerStrengths(a);
            if (a == OperationOrder.LoopAction.InitImg) _loopInits(a);
            if (a == OperationOrder.LoopAction.InitStrength) _loopInitStrengths(a);
            // if (a == OperationOrder.LoopAction.LoraWeight) _loopLoraWeights(a);
        }

        public async Task Cancel()
        {
            List<string> lastLogLines = Logger.GetLastLines(Constants.Lognames.Sd, 20);
            bool startingUp = LastMessages.Any(s => s.Contains("Total VRAM")) && LastMessages.Any(s => !s.Contains("To see the GUI go to:"));

            if (startingUp)
            {
                TtiProcess.KillAll(); // Kill process if initialization was not done yet
            }
            else
            {
                var queueArgs = new Dictionary<string, bool> { { "clear", true } };
                await ApiPost(queueArgs.ToJson(), ComfyEndpoint.Queue);
                await ApiPost(endpoint: ComfyEndpoint.Interrupt);
            }
        }

        public static async Task Upscale(string imagePath)
        {
            Program.SetState(Program.BusyState.PostProcessing);

            var gi = new GenerationInfo
            {
                InitImg = imagePath,
                Upscaler = Models.GetUpscalers().Where(m => m.Name == Config.Instance.EsrganModel).FirstOrDefault().FullName,
            };

            var metadata = new EasyDict<string, object>();
            var meta = IoUtils.GetImageMetadata(imagePath);

            if (meta != null)
            {
                string giKey = ImageMetadata.MetadataType.GenerationInfoJson.ToString();

                GenerationInfo metaGi = ImageMetadata.DeserializeGenInfo(meta.AllEntries.Where(e => e.Key == giKey).FirstOrDefault().Value);
                
                if (metaGi != null)
                    metadata[giKey] = metaGi.GetSerializeClone();
            }

            string savePath = IoUtils.FilenameSuffix(imagePath, ".upscale");
            savePath = IoUtils.GetAvailablePath(savePath, "{0}");
            savePath = FormatUtils.NormalizePath(savePath);
            await SendPrompt(ComfyPrompts.GetUpscaleWorkflow(gi, savePath), metadata);
        }

        public void HandleOutput(string line)
        {
            if (TextToImage.CurrentTaskSettings == null || line == null)
                return;

            if (line.Contains("Setting up MemoryEfficientCrossAttention"))
                return;

            if (line.Trim().StartsWith("left over keys:"))
                line = "left over keys: dict_keys([...])";

            if (line.StartsWith("PREVIEW_JPEG:b'")) // Decode base64 encoded JPEG preview
            {
                byte[] imageBytes = Convert.FromBase64String(line.Split('\'')[1]);
                Console.WriteLine($"Received preview {(imageBytes.Length / 1024f).RoundToInt()}k");

                using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    Image image = Image.FromStream(ms, true);
                    Program.MainForm.pictBoxPreview.Image = image;
                }

                return;
            }

            if (line.StartsWith("PREVIEW_WEBP:b'")) // Decode base64 encoded WEBP preview
            {
                Console.WriteLine($"Received WEBP preview {((line.Length * sizeof(Char)) / 1024f).RoundToInt()}k");
                var magick = ImgUtils.GetMagickImage(line.Split('\'')[1]);

                if (magick != null)
                    Program.MainForm.pictBoxImgViewer.Image = ImgUtils.ToBitmap(magick);

                return;
            }

            Logger.Log(line, true, false, Constants.Lognames.Sd);
            TtiProcessOutputHandler.LastMessages.Insert(0, line);

            if (TextToImage.Canceled)
                return;

            bool ellipsis = Program.MainForm.LogText.EndsWith("...");
            string errMsg = "";
            bool replace = ellipsis || Logger.LastUiLine.MatchesWildcard("*Generated*image*in*");
            bool lastLineGeneratedText = Logger.LastUiLine.MatchesWildcard("*Generated*image*in*");
            TextToImage.CancelMode cancelMode = TextToImage.CancelMode.SoftKill;

            if (!TextToImage.Canceled && line.Trim() == "got prompt")
            {
                // if (!lastLineGeneratedText)
                //     Logger.LogIfLastLineDoesNotContainMsg($"Generating...", replaceLastLine: ellipsis);

                ImageExport.TimeSinceLastImage.Restart();
            }

            if (!TextToImage.Canceled && line.MatchesWildcard("*%|*| *") && !line.Contains("B/s"))
            {
                if (!lastLineGeneratedText)
                    Logger.LogIfLastLineDoesNotContainMsg($"Generating...", replaceLastLine: ellipsis);

                int percent = line.Split("%|")[0].GetInt();

                if (percent >= 0 && percent <= 100)
                    Program.MainForm.SetProgressImg(percent);
            }

            // Info: Model type
            if (!TextToImage.Canceled && line.Trim().StartsWith("MODEL INFO:"))
            {
                var split = line.Split('|');
                string filename = split[0].Split("MODEL INFO:")[1].Trim();
                string modelType = split[1].Trim();
                string unetConfigJson = split[2].Trim();
                var mdlArch = Models.DetectModelType(modelType, unetConfigJson);
                Config.Instance.ModelSettings.GetPopulate(filename, new Models.ModelSettings()).Arch = mdlArch;
                Logger.Log($"Loaded '{filename.Trunc(100)}' - {Strings.ModelArch.Get(mdlArch.ToString(), true)}", false, Logger.LastUiLine.Contains(filename));

                string controlnetError = ComfyUtils.ControlnetCompatCheck(_lastSettings.Controlnets, mdlArch);

                if (controlnetError.IsNotEmpty())
                {
                    errMsg = controlnetError;
                    _hasErrored = true;
                    cancelMode = TextToImage.CancelMode.ForceKill;
                }

                if (Program.MainForm.comboxModel.GetTextSafe() == filename && mdlArch != ModelArch.SdXlRefine)
                {
                    Program.MainForm.comboxModelArch.SetWithText(Config.Instance.ModelSettings[filename].Arch.ToString(), false, Strings.ModelArch);
                    Size res = Models.GetDefaultRes(mdlArch);

                    if (_lastSettings.Res.Width < res.Width && _lastSettings.Res.Height < res.Height)
                        Logger.Log($"Warning: The resolution {_lastSettings.Res.Width}x{_lastSettings.Res.Height} might lead to low quality results, the native resolution of this model is {res.AsString()}.");
                }
            }

            // Info: Image saved
            if (!TextToImage.Canceled && line.Trim().StartsWith("Saved image"))
            {
                string pfx = line.Split('\'')[1];
                string path = string.Join(":", line.Split(':').Skip(1)).Trim();

                if (pfx == "upscale")
                {
                    ImageViewer.AppendImage(path, ImageViewer.ImgShowMode.ShowLast, false);
                    Program.SetState(Program.BusyState.Standby);
                }
            }

            // Warning: Missing embedding
            if (!TextToImage.Canceled && line.Trim().StartsWith("warning, embedding:") && line.Trim().EndsWith("does not exist, ignoring"))
            {
                string embName = line.Split("embedding:")[1].Split(' ')[0];
                Logger.Log($"Warning: Embedding '{embName}' not found!");
            }

            // Warning: Incompatible embedding
            if (!TextToImage.Canceled && line.Trim().StartsWith("WARNING: shape mismatch when trying to apply embedding"))
            {
                Logger.Log($"Warning: One or more embeddings were ignored because they are not compatible with the selected model.");
            }

            // Warning: Upscaling failure
            if (!TextToImage.Canceled && line.Contains("Upscaling failed!"))
            {
                Logger.Log($"Warning: Image upscaling failed. Maybe your model is incompatible?");
            }

            // Error: Port in use
            if (!_hasErrored && !TextToImage.Canceled && line.Trim().StartsWith("OSError: [Errno 10048]"))
            {
                errMsg = $"Port is already in use. Are you running ComfyUI on port {ComfyPort}?";
                _hasErrored = true;
                cancelMode = TextToImage.CancelMode.ForceKill;
            }

            // Error: Incompatible model
            if (!_hasErrored && !TextToImage.Canceled && line.StartsWith("Failed to load model:"))
            {
                errMsg = $"{line}\n\nIt might be incompatible.";
                _hasErrored = true;
            }

            // Error: Shapes
            if (!_hasErrored && !TextToImage.Canceled && line.Contains("shapes cannot be multiplied"))
            {
                errMsg = $"{line}\n\nThis most likely means that certain models (e.g. LoRAs) are not compatible with the selected Stable Diffusion model.";
                _hasErrored = true;
            }

            TtiProcessOutputHandler.HandleLogGeneric(this, line, _hasErrored, cancelMode, errMsg, true);
        }

        public void ResetLogger()
        {
            _hasErrored = false;
            LastMessages.Clear();
        }

        public override string GetEmbeddingStringFormat()
        {
            return "embedding:{0}";
        }
    }
}
