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
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Implementations
{
    public class Comfy : ImplementationBase, IImplementation
    {
        public List<string> LastMessages { get => _lastMessages; }
        private List<string> _lastMessages = new List<string>();
        private bool _hasErrored = false;

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
            float[] initStrengths = s.InitStrengths.Select(n => 1f - n).ToArray();
            var cachedModels = Models.GetModels((Enums.Models.Type)(-1), Implementation.Comfy);
            var baseModels = cachedModels.Where(m => m.Type == Enums.Models.Type.Normal).ToList();
            var refinerModels = cachedModels.Where(m => m.Type == Enums.Models.Type.Refiner).ToList();
            Model model = TtiUtils.CheckIfModelExists(s.Model, Implementation.Comfy, baseModels);
            string vaeName = s.Vae.NullToEmpty().Replace("None", ""); // VAE model name
            Model vae = Models.GetModel(Models.GetVaes(), vaeName);
            var controlnetMdl = Models.GetControlNets().Where(m => m.FormatIndependentName == s.ControlnetModel).FirstOrDefault();

            if (model == null)
                return;

            // if (s.Res.Width < 1024 && s.Res.Height < 1024)
            //     Logger.Log($"Warning: The resolution {s.Res.Width}x{s.Res.Height} might lead to low quality results, as the default resolution of SDXL is 1024x1024.");

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
                NegativePrompt = s.NegativePrompt,
                Model = model.FullName,
                ModelRefiner = refineModel == null ? "" : refineModel.FullName,
                Vae = vae == null ? "" : vae.FullName,
                Sampler = s.Sampler,
                Upscaler = Config.Instance.UpscaleEnable ? Config.Instance.EsrganModel : "",
                ControlnetModel = controlnetMdl == null ? "" : controlnetMdl.FullName,
                ControlnetStrength = s.ControlnetStrength,
            };

            foreach (var lora in s.Loras)
                currentGeneration.Loras.Add(lora.Key, lora.Value.First());

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
            Logger.Log($"{s.Prompts.Length} prompt{(s.Prompts.Length != 1 ? "s" : "")} * {s.Iterations} image(s) * {s.Steps.Length} step value(s) * {s.ScalesTxt.Length} scale value(s) * {s.RefinerStrengths.Length} refine value(s){initsStr} = {generations.Count} image(s) total.");

            var scriptArgs = new List<string>
            {
                GetVramArg(),
                $"--output-directory {outPath.Wrap(true)}",
                $"--preview-method none",
                $"--port {ComfyPort}",
            };

            string newStartupSettings = string.Join(" ", scriptArgs).Remove(" ");

            if (!TtiProcess.IsAiProcessRunning || (TtiProcess.IsAiProcessRunning && TtiProcess.LastStartupSettings != newStartupSettings))
            {
                if (TextToImage.Canceled) return;

                PatchUtils.PatchDiffusers();
                Logger.Log($"(Re)starting Comfy. Process running: {TtiProcess.IsAiProcessRunning} - Prev startup string: '{TtiProcess.LastStartupSettings}' - New startup string: '{newStartupSettings}'", true);
                TtiProcess.LastStartupSettings = newStartupSettings;

                Process py = OsUtils.NewProcess(true, logAction: HandleOutput);
                TextToImage.CurrentTask.Processes.Add(py);

                string comfyPath = "D:\\AI\\ComfyUI\\";
                py.StartInfo.Arguments = $"/C cd /D {comfyPath} && .\\python_embeded\\python.exe -s ComfyUI\\main.py {string.Join(" ", scriptArgs)}";
                // py.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSdCommand()} && {Constants.Files.VenvActivate} && python {Constants.Dirs.SdRepo}/nmkdiff/nmkdiffusers.py {string.Join(" ", scriptArgs)}";
                Logger.Log("cmd.exe " + py.StartInfo.Arguments, true);

                if (TtiProcess.CurrentProcess != null)
                {
                    TtiProcess.ProcessExistWasIntentional = true;
                    OsUtils.KillProcessTree(TtiProcess.CurrentProcess.Id);
                }

                ResetLogger();

                Logger.Log($"Loading Stable Diffusion with model {s.Model.Trunc(80).Wrap()}...");

                var sw = new NmkdStopwatch();
                TtiProcess.ProcessExistWasIntentional = false;
                py.Start();
                TtiProcess.CurrentProcess = py;
                OsUtils.AttachOrphanHitman(py);

                if (!OsUtils.ShowHiddenCmd())
                {
                    py.BeginOutputReadLine();
                    py.BeginErrorReadLine();
                }

                Task.Run(() => TtiProcess.CheckStillRunning());

                await Logger.WaitForMessageAsync("To see the GUI go to:", contains: true);
                Logger.Log($"Comfy startup time: {sw.ElapsedMs} ms", true);
            }
            else
            {
                ResetLogger();
                TextToImage.CurrentTask.Processes.Add(TtiProcess.CurrentProcess);
            }

            string wf = File.ReadAllText(Path.Combine(Paths.GetDataPath(), "comfy", "wf", "workflow.json"));
            var nodes = ComfyWorkflow.GetNodes(wf).OrderBy(n => nameof(n)).ThenBy(n => n.Title).ToList().ToList();

            foreach (var genInfo in generations.Where(g => g.Model.IsNotEmpty()))
            {
                string reqString = "";

                try
                {
                    var promptItems = ComfyWorkflow.GetPromptInfos(genInfo, nodes);
                    var req = new ComfyWorkflow.PromptRequest() { ClientId = Paths.SessionTimestampUnix.ToString(), Prompt = promptItems };
                    req.ExtraData.ExtraPnginfo["GenerationInfo"] = genInfo.GetMetadataDict();
                    reqString = req.ToString();

                    if (Program.Debug)
                        File.WriteAllText(IoUtils.GetAvailablePath(Path.Combine(Paths.GetLogPath(), "req.json")), req.ToStringAdvanced(true));
                }
                catch (Exception ex)
                {
                    Logger.Log($"Error creating request: {ex.Message}");
                    Logger.Log(ex.StackTrace, true);
                }

                if (reqString.IsEmpty())
                    continue;

                string resp = await ApiPost(reqString);
                Logger.Log($"[<-] {resp}", true, filename: Constants.Lognames.Api);
            }
        }

        private enum ComfyEndpoint { Prompt, Queue, Interrupt }

        private async Task<string> ApiPost(string data = "", ComfyEndpoint endpoint = ComfyEndpoint.Prompt)
        {
            try
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                var stringContent = data.IsEmpty() ? null : new StringContent(data, Encoding.UTF8, "application/json");
                string baseUrl = $"http://127.0.0.1:{ComfyPort}/{endpoint.ToString().Lower()}";
                var response = await _webClient.PostAsync(baseUrl, stringContent);
                Logger.Log($"[->] {baseUrl} - {data.Trunc(150)}", true, filename: Constants.Lognames.Api);
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Logger.Log($"API Error: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
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

        private string GetVramArg()
        {
            var preset = ParseUtils.GetEnum<Enums.Comfy.VramPreset>(Config.Instance.ComfyVramPreset.ToString(), true, Strings.ComfyVramPresets);
            if (preset == Enums.Comfy.VramPreset.GpuOnly) return "--gpu-only";
            if (preset == Enums.Comfy.VramPreset.HighVram) return "--highvram";
            if (preset == Enums.Comfy.VramPreset.NormalVram) return "--normalvram";
            if (preset == Enums.Comfy.VramPreset.LowVram) return "--lowvram";
            if (preset == Enums.Comfy.VramPreset.NoVram) return "--novram";
            return "";
        }

        public async Task Cancel()
        {
            List<string> lastLogLines = Logger.GetLastLines(Constants.Lognames.Sd, 15);

            await ApiPost("{\"clear\":true}", ComfyEndpoint.Queue);
            await ApiPost("", ComfyEndpoint.Interrupt);

            // if (lastLogLines.Where(x => x.Contains("%|") || x.Contains("error occurred")).Any()) // Only attempt a soft cancel if we've been generating anything
            //     await WaitForCancel();
            // else // This condition should be true if we cancel while it's still initializing, so we can just force kill the process
            //     TtiProcess.KillAll();
        }

        public void HandleOutput(string line)
        {
            if (TextToImage.CurrentTaskSettings == null || line == null)
                return;

            if (line.Contains("Setting up MemoryEfficientCrossAttention"))
                return;

            if (line.Trim().StartsWith("left over keys:"))
                line = "left over keys: dict_keys([...])";

            if (line.StartsWith("PREVIEW:b'")) // Decode base64 encoded JPEG preview
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

            Logger.Log(line, true, false, Constants.Lognames.Sd);
            TtiProcessOutputHandler.LastMessages.Insert(0, line);

            if (TextToImage.Canceled)
                return;


            bool ellipsis = Program.MainForm.LogText.EndsWith("...");
            string errMsg = "";
            bool replace = ellipsis || Logger.LastUiLine.MatchesWildcard("*Generated*image*in*");
            bool lastLineGeneratedText = Logger.LastUiLine.MatchesWildcard("*Generated*image*in*");

            if (!TextToImage.Canceled && line.Trim() == "got prompt")
            {
                if (!lastLineGeneratedText)
                    Logger.LogIfLastLineDoesNotContainMsg($"Generating...", replaceLastLine: ellipsis);

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

            TtiProcessOutputHandler.HandleLogGeneric(this, line, _hasErrored);
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

        public class GenerationInfo
        {
            public string Model;
            public string ModelRefiner;
            public string Vae;
            public string Upscaler;
            public string Prompt;
            public string NegativePrompt;
            public EasyDict<string, float> Loras = new EasyDict<string, float>();
            public string ControlnetModel;
            public float ControlnetStrength = 1f;
            public int Steps;
            public long Seed;
            public float Scale;
            public float RefinerStrength;
            public string InitImg;
            public float InitStrength;
            public string MaskPath;
            public Size BaseResolution;
            public Size TargetResolution;
            public Sampler Sampler;
            public int ClipSkip = -1;

            public Dictionary<string, dynamic> GetMetadataDict()
            {
                return new Dictionary<string, dynamic>
                {
                    { "model", Path.GetFileName(Model) },
                    { "modelRefiner", Path.GetFileName(ModelRefiner) },
                    { "upscaler", Path.GetFileName(Upscaler) },
                    { "prompt", Prompt },
                    { "promptNeg", NegativePrompt },
                    { "initImg", InitImg },
                    { "initStrength", InitStrength },
                    { "w", BaseResolution.Width },
                    { "h", BaseResolution.Height },
                    { "steps", Steps },
                    { "seed", Seed },
                    { "scaleTxt", Scale },
                    { "inpaintMask", MaskPath },
                    { "sampler", Sampler.ToString().Lower() },
                    { "refineFrac", (1f - RefinerStrength) },
                    { "upscaleW", TargetResolution.Width },
                    { "upscaleH", TargetResolution.Height },
                };
            }
        }
    }
}
