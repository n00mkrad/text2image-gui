using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
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
        public static int Pid = -1;

        public static readonly int ComfyPort = 8188;
        private static readonly HttpClient _webClient = new HttpClient();

        private OperationOrder _opOrder = new OperationOrder(new List<OperationOrder.LoopAction> { OperationOrder.LoopAction.Prompt, OperationOrder.LoopAction.Iteration, OperationOrder.LoopAction.Scale, OperationOrder.LoopAction.Step });
        private Action<OperationOrder.LoopAction> _loopIterations, _loopPrompts, _loopScales, _loopSteps/*, _loopInits, _loopInitStrengths, _loopLoraWeights*/;

        public async Task Run(TtiSettings s, string outPath)
        {
            float[] initStrengths = s.InitStrengths.Select(n => 1f - n).ToArray();
            var cachedModels = Models.GetModels((Enums.Models.Type)(-1), Implementation.SdXl);
            var baseModels = cachedModels.Where(m => m.Type == Enums.Models.Type.Normal).ToList();
            var refinerModels = cachedModels.Where(m => m.Type == Enums.Models.Type.Refiner).ToList();
            Model model = TtiUtils.CheckIfModelExists(s.Model, Implementation.SdXl, baseModels);

            if (model == null)
                return;

            // if (s.Res.Width < 1024 && s.Res.Height < 1024)
            //     Logger.Log($"Warning: The resolution {s.Res.Width}x{s.Res.Height} might lead to low quality results, as the default resolution of SDXL is 1024x1024.");

            OrderedDictionary initImages = s.InitImgs != null && s.InitImgs.Length > 0 ? await TtiUtils.CreateResizedInitImagesIfNeeded(s.InitImgs.ToList(), s.Res) : null;
            long startSeed = s.Seed;
            bool refine = s.RefinerStrengths.Any(rs => rs >= 0.05f);
            string mode = NmkdiffUtils.GetGenerationMode(s, model);

            string missingRefinerMsg = "No Refiner model file has been set.\nPlease set one or disable image refining.";
            Model refineModel = refine ? TtiUtils.CheckIfModelExists(s.ModelAux, Implementation.SdXl, refinerModels, missingRefinerMsg) : null;

            if (refine && refineModel == null)
                return;


            Logger.ClearLogBox();
            Logger.Log($"Running Stable Diffusion - {s.Res.Width}x{s.Res.Height}, Starting Seed: {startSeed}");

            string initsStr = initImages != null ? $" and {initImages.Count} image{(initImages.Count != 1 ? "s" : "")} using {initStrengths.Length} strength{(initStrengths.Length != 1 ? "s" : "")}" : "";
            Logger.Log($"{s.Prompts.Length} prompt{(s.Prompts.Length != 1 ? "s" : "")} * {s.Iterations} image(s) * {s.Steps.Length} step value(s) * {s.ScalesTxt.Length} scale value(s) * {s.RefinerStrengths.Length} refine value(s){initsStr} = {999} images total.");

            var scriptArgs = new List<string>
            {
                $"--normalvram",
                $"--output-directory {outPath.Wrap(true)}",
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
            }
            else
            {
                ResetLogger();
                TextToImage.CurrentTask.Processes.Add(TtiProcess.CurrentProcess);
            }

            //foreach (var argList in argLists)
            //    await TtiProcess.WriteStdIn($"generate {argList.ToJson()}", 200, true);

            var generations = new List<GenerationInfo>() { new GenerationInfo() };

            List<string> processedPrompts = null;
            TextToImage.CurrentTaskSettings.ProcessedAndRawPrompts = new EasyDict<string, string>();

            int currentIteration = 0;
            float currentLoraWeight = 1.0f;

            void NextAction(OperationOrder.LoopAction thisAction)
            {
                if (_opOrder.SeedResetActions.Contains(thisAction))
                    s.Seed = startSeed;

                if (_opOrder.SeedIncrementActions.Contains(thisAction) && !s.LockSeed)
                    s.Seed++;

                RunNextLoopAction(thisAction, () => FinalAction());
            }

            void FinalAction()
            {
                string currPrompt = processedPrompts[currentIteration];

                // if (s.Loras != null && s.Loras.Count == 1)
                //     foreach (var lora in s.Loras)
                //         currPrompt = currPrompt.Replace($"{lora.Key}Weight", currentLoraWeight.ToStringDot("0.0###"));

                generations.Last().Width = s.Res.Width;
                generations.Last().Height = s.Res.Height;
                generations.Last().Prompt = currPrompt;
                generations.Last().NegativePrompt = s.NegativePrompt;
                generations.Last().Model = model;
                generations.Last().ModelRefiner = refineModel;
                generations.Add(new GenerationInfo());
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
                    generations.Last().Seed = s.Seed;
                    NextAction(thisAction);
                    currentIteration++;
                }
            };

            _loopScales = (thisAction) =>
            {
                foreach (float scale in s.ScalesTxt)
                {
                    generations.Last().Scale = scale.Clamp(0.01f, 1000f);
                    NextAction(thisAction);
                }
            };

            _loopSteps = (thisAction) =>
            {
                foreach (int stepCount in s.Steps)
                {
                    generations.Last().Steps = stepCount;
                    NextAction(thisAction);
                }
            };

            // _loopInits = (thisAction) =>
            // {
            //     if (initImages == null) // No init image(s)
            //     {
            //         args["initImg"] = "";
            //         args["initStrength"] = "";
            //         NextAction(thisAction);
            //     }
            //     else // With init image(s)
            //     {
            //         foreach (string initImg in initImages.Values)
            //         {
            //             args["initImg"] = $"-I {initImg.Wrap()}";
            //             NextAction(thisAction);
            //         }
            //     }
            // };
            // 
            // _loopInitStrengths = (thisAction) =>
            // {
            //     foreach (float strength in s.InitStrengthsReverse)
            //     {
            //         args["initStrength"] = s.ImgMode != Enums.StableDiffusion.ImgMode.InitializationImage ? "-f 1.0" : $"-f {strength.ToStringDot("0.###")}"; // Lock to 1.0 when using inpainting
            // 
            //         if (s.ImgMode == Enums.StableDiffusion.ImgMode.ImageMask)
            //             args["inpaintMask"] = $"-M {Inpainting.MaskedImagePath.Wrap()}";
            //         else if (s.ImgMode == Enums.StableDiffusion.ImgMode.Outpainting)
            //             args["inpaintMask"] = "--force_outpaint";
            //         else
            //             args["inpaintMask"] = "";
            // 
            //         NextAction(thisAction);
            //     }
            // };
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

            string wf = File.ReadAllText(Path.Combine(Paths.GetDataPath(), "comfy", "wf", "workflow.json"));
            wf = wf.Trim().TrimStart('{').TrimEnd('}');
            var nodes = ComfyWorkflow.GetNodes(wf).OrderBy(n => n.Type).ThenBy(n => n.Title).ToList().ToList();
            // GenerationInfo test = new GenerationInfo { Model = model, ModelRefiner = refineModel, Prompt = s.Prompts[0], NegativePrompt = s.NegativePrompt, Steps = s.Steps[0], Seed = s.Seed, Scale = s.ScalesTxt[0], RefinerStrength = s.RefinerStrengths[0], Width = s.Res.Width, Height = s.Res.Height };
            
            foreach(var genInfo in generations.Where(g => g.Model != null))
            {
                string req = $"{{\"client_id\":\"{Paths.SessionTimestampUnix}\",\"prompt\":{{{ComfyWorkflow.BuildPrompt(genInfo, nodes)}}},\"extra_data\":{{\"extra_pnginfo\":{{\"workflow\":{{{wf}}}}}}}}}";
                await ApiPost(req);
            }
        }

        private enum ComfyEndpoint { Prompt, Queue, Interrupt }

        private async Task<string> ApiPost(string data = "", ComfyEndpoint endpoint = ComfyEndpoint.Prompt)
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            var stringContent = data.IsEmpty() ? null : new StringContent(data, Encoding.UTF8, "application/json");
            string baseUrl = $"http://127.0.0.1:8188/{endpoint.ToString().Lower()}";
            var response = await _webClient.PostAsync(baseUrl, stringContent);
            Logger.Log($"{baseUrl} - {stringContent}", true, filename: "api");
            return await response.Content.ReadAsStringAsync();
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
            // if (a == OperationOrder.LoopAction.InitImg) _loopInits(a);
            // if (a == OperationOrder.LoopAction.InitStrengths) _loopInitStrengths(a);
            // if (a == OperationOrder.LoopAction.LoraWeight) _loopLoraWeights(a);
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
                ImageExport.TimeSinceLastImage.Restart();
            }

            if (!TextToImage.Canceled && line.MatchesWildcard("*%|*| *"))
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


        public class GenerationInfo
        {
            public Model Model;
            public Model ModelRefiner;
            public string Prompt;
            public string NegativePrompt;
            public int Steps;
            public long Seed;
            public float Scale;
            public float RefinerStrength;
            public string InitImg;
            public float InitStrength;
            public int Width;
            public int Height;
            public bool LatentUpscale = true;
        }
    }
}
