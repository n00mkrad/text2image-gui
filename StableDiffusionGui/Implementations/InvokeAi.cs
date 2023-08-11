using StableDiffusionGui.Data;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Implementations
{
    internal class InvokeAi : ImplementationBase, IImplementation
    {
        public List<string> LastMessages { get => _lastMessages; }
        private List<string> _lastMessages = new List<string>();
        private static bool _lastModelCached = false;
        public enum HiresFixStatus { NotUpscaling, WaitingForStart, Upscaling }
        private static HiresFixStatus _hiresFixStatus;
        private bool _hasErrored = false;
        public static int Pid = -1;

        public enum FixAction { Upscale, FaceRestoration }

        public static Dictionary<string, string> PostProcessMovePaths = new Dictionary<string, string>();
        public static EasyDict<string, string> EmbeddingsFilesTriggers = new EasyDict<string, string>(); // Key = Filename without ext, Value = Trigger

        private OperationOrder _opOrder = new OperationOrder();
        private Action<OperationOrder.LoopAction> _loopIterations, _loopPrompts, _loopScales, _loopSteps, _loopInits, _loopInitStrengths, _loopLoraWeights;

        public async Task Run(TtiSettings s, string outPath)
        {
            try
            {
                string vae = s.Vae.NullToEmpty().Replace(Constants.NoneMdl, ""); // VAE model name
                var allModels = Models.GetModelsAll();
                var cachedModels = allModels.Where(m => m.Type == Enums.Models.Type.Normal).ToList();
                var cachedModelsVae = Models.GetVaes();
                Model modelFile = TtiUtils.CheckIfModelExists(s.Model, Enums.StableDiffusion.Implementation.InvokeAi);
                Model vaeFile = Models.GetModel(cachedModelsVae, vae);
                if (TextToImage.Canceled) return;

                cachedModels[cachedModels.IndexOf(cachedModels.Where(m => m.FullName == modelFile.FullName).First())].SetArch(s.ModelArch);

                OrderedDictionary initImages = s.InitImgs != null && s.InitImgs.Length > 0 ? await TtiUtils.CreateResizedInitImagesIfNeeded(s.InitImgs.ToList(), s.Res, s.ResizeGravity) : null;

                long startSeed = s.Seed;
                s.Prompts = s.Prompts.Select(p => InvokeAiUtils.GetCombinedPrompt(p, s.NegativePrompt, s.Loras)).ToArray(); // Apply negative prompt

                var argLists = new List<EasyDict<string, string>>(); // List of all args for each command
                var args = new EasyDict<string, string>(); // List of args for current command
                args["prompt"] = "";
                args["default"] = Args.InvokeAi.GetDefaultArgsCommand();
                args["upscale"] = Args.InvokeAi.GetUpscaleArgs();
                args["facefix"] = Args.InvokeAi.GetFaceRestoreArgs();
                args["seamless"] = Args.InvokeAi.GetSeamlessArg(s.SeamlessMode);
                args["symmetry"] = Args.InvokeAi.GetSymmetryArg(s.SymmetryMode);
                args["hiresFix"] = s.HiresFix ? "--hires_fix" : "";
                args["debug"] = s.AppendArgs;
                args["res"] = $"-W {s.Res.Width} -H {s.Res.Height}";
                args["sampler"] = $"-A {s.Sampler.ToString().Lower()}";

                if (initImages == null || initImages.Count <= 0)
                    _opOrder.LoopOrder.Remove(OperationOrder.LoopAction.InitStrength);

                if (!s.Loras.Any())
                    _opOrder.LoopOrder.Remove(OperationOrder.LoopAction.LoraWeight);

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

                    if (s.Loras != null && s.Loras.Count == 1)
                        foreach (var lora in s.Loras)
                            currPrompt = currPrompt.Replace($"{lora.Key}Weight", currentLoraWeight.ToStringDot("0.0###"));

                    args["prompt"] = currPrompt.Wrap();
                    argLists.Add(new EasyDict<string, string>(args));
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
                        args["seed"] = $"-S {s.Seed}";
                        NextAction(thisAction);
                        currentIteration++;
                    }
                };

                _loopScales = (thisAction) =>
                {
                    foreach (float scale in s.ScalesTxt)
                    {
                        args["scale"] = $"-C {scale.Clamp(0.01f, 1000f).ToStringDot()}";
                        NextAction(thisAction);
                    }
                };

                _loopSteps = (thisAction) =>
                {
                    foreach (int stepCount in s.Steps)
                    {
                        args["steps"] = $"-s {stepCount}";

                        NextAction(thisAction);
                    }
                };

                _loopInits = (thisAction) =>
                {
                    if (initImages == null) // No init image(s)
                    {
                        args["initImg"] = "";
                        args["initStrength"] = "";
                        NextAction(thisAction);
                    }
                    else // With init image(s)
                    {
                        foreach (string initImg in initImages.Values)
                        {
                            args["initImg"] = $"-I {initImg.Wrap()}";
                            NextAction(thisAction);
                        }
                    }
                };

                _loopInitStrengths = (thisAction) =>
                {
                    foreach (float strength in s.InitStrengthsReverse)
                    {
                        args["initStrength"] = s.ImgMode != Enums.StableDiffusion.ImgMode.InitializationImage ? "-f 1.0" : $"-f {strength.ToStringDot("0.###")}"; // Lock to 1.0 when using inpainting

                        if (s.ImgMode == Enums.StableDiffusion.ImgMode.ImageMask)
                            args["inpaintMask"] = $"-M {Inpainting.MaskedImagePath.Wrap()}";
                        else if (s.ImgMode == Enums.StableDiffusion.ImgMode.Outpainting)
                            args["inpaintMask"] = "--force_outpaint";
                        else
                            args["inpaintMask"] = "";

                        NextAction(thisAction);
                    }
                };

                _loopLoraWeights = (thisAction) =>
                {
                    foreach (float weight in s.Loras.First().Value)
                    {
                        currentLoraWeight = weight;
                        NextAction(thisAction);
                    }
                };

                RunLoopAction(_opOrder.LoopOrder.First());

                Logger.ClearLogBox();
                Logger.Log($"Running Stable Diffusion - {s.Res.Width}x{s.Res.Height}, Starting Seed: {startSeed}");

                string modelsChecksumStartup = InvokeAiUtils.GetModelsHash(cachedModels);
                string argsStartup = Args.InvokeAi.GetArgsStartup(cachedModels);
                string newStartupSettings = $"{argsStartup} {modelsChecksumStartup} {Config.Instance.CudaDeviceIdx} {Config.Instance.ClipSkip}"; // Check if startup settings match - If not, we need to restart the process

                Logger.Log(GetImageCountLogString(initImages, s.InitStrengthsReverse, s.Prompts, s.Iterations, s.Steps, s.ScalesTxt, argLists));

                Logger.Clear(Constants.Lognames.Sd);
                bool restartedInvoke = false; // Will be set to true if InvokeAI was not running before

                if (!TtiProcess.IsAiProcessRunning || (TtiProcess.IsAiProcessRunning && TtiProcess.LastStartupSettings != newStartupSettings))
                {
                    InvokeAiUtils.WriteModelsYamlAll(cachedModels, cachedModelsVae, s.ModelArch);
                    await SetModel(modelFile, vaeFile, true);
                    if (TextToImage.Canceled) return;

                    Logger.Log($"(Re)starting InvokeAI. Process running: {TtiProcess.IsAiProcessRunning} - Prev startup string: '{TtiProcess.LastStartupSettings}' - New startup string: '{newStartupSettings}'", true);
                    TtiProcess.LastStartupSettings = newStartupSettings;

                    string invokeExePath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdVenv, "Scripts", "invokeai.exe");
                    string pyExePath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdVenv, "Scripts", "python.exe");
                    Process py = OsUtils.NewProcess(true, pyExePath, logAction: HandleOutput, redirectStdin: true);
                    py.StartInfo.EnvironmentVariables["INVOKEAI_ROOT"] = InvokeAiUtils.HomePath;
                    py.StartInfo.EnvironmentVariables["INVOKE_NMKD_HIRES_MINDIM_MULT"] = Config.IniInstance.HiresFixMinimumDimensionMultiplier.ToStringDot("0.0##");
                    py.StartInfo.WorkingDirectory = Paths.GetDataPath();
                    py.StartInfo.Arguments = $"{invokeExePath.Wrap()} --model {InvokeAiUtils.GetMdlNameForYaml(modelFile, vaeFile)} -o {outPath.Wrap(true)} {argsStartup}";

                    foreach (var pair in TtiUtils.GetEnvVarsSd(false, Paths.GetDataPath()))
                        py.StartInfo.EnvironmentVariables[pair.Key] = pair.Value;

                    TextToImage.CurrentTask.Processes.Add(py);
                    Logger.Log($"{py.StartInfo.FileName} {py.StartInfo.Arguments}", true);

                    if (TtiProcess.CurrentProcess != null)
                    {
                        TtiProcess.ProcessExistWasIntentional = true;
                        OsUtils.KillProcessTree(TtiProcess.CurrentProcess.Id);
                    }

                    ResetLogger();

                    string logMdl = modelFile.FormatIndependentName.Trunc(vae.IsNotEmpty() ? 40 : 80).Wrap();
                    string logVae = vaeFile == null ? "" : vaeFile.FormatIndependentName.Trunc(40).Wrap();
                    Logger.Log($"Loading Stable Diffusion with model {logMdl}{(string.IsNullOrWhiteSpace(logVae) ? "" : $" and VAE {logVae}")}...");

                    TtiProcess.CurrentProcess = py;
                    TtiProcess.ProcessExistWasIntentional = false;

                    restartedInvoke = true;
                    OsUtils.StartProcess(py, killWithParent: true);
                    TtiProcess.CurrentStdInWriter = new NmkdStreamWriter(py);

                    Task.Run(() => TtiProcess.CheckStillRunning());

                    string embeddingMsg = await Logger.WaitForMessageAsync(Constants.LogMsgs.Invoke.TiTriggers, true, true);
                    InvokeAiUtils.LoadEmbeddingTriggerTable(embeddingMsg);
                }
                else
                {
                    ResetLogger();
                    await SetModel(modelFile, vaeFile);
                    TextToImage.CurrentTask.Processes.Add(TtiProcess.CurrentProcess);
                }

                bool noCommandsSent = true;

                foreach (var argList in argLists)
                {
                    if (TextToImage.Canceled)
                        break;

                    if (!InvokeAiUtils.ValidateCommand(argList, s.Res))
                        continue;

                    argList["prompt"] = InvokeAiUtils.FixPromptEmbeddingTriggers(argList["prompt"]);
                    string command = string.Join(" ", argList.Where(entry => entry.Value.IsNotEmpty()).Select(argEntry => argEntry.Value));
                    await TtiProcess.WriteStdIn(command);
                    noCommandsSent = false;
                }

                if (noCommandsSent)
                    TextToImage.Cancel("No valid commands.", false, restartedInvoke ? TextToImage.CancelMode.ForceKill : TextToImage.CancelMode.DoNotKill);
            }
            catch (Exception ex)
            {
                Logger.Log($"Unhandled Stable Diffusion Error: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
            }
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
            if (a == OperationOrder.LoopAction.InitImg) _loopInits(a);
            if (a == OperationOrder.LoopAction.InitStrength) _loopInitStrengths(a);
            if (a == OperationOrder.LoopAction.LoraWeight) _loopLoraWeights(a);
        }

        public string GetImageCountLogString(OrderedDictionary initImages, float[] initStrengths, string[] prompts, int iterations, int[] steps, float[] scales, List<EasyDict<string, string>> argLists)
        {
            string initsStr = initImages != null ? $" and {initImages.Count} Image{(initImages.Count != 1 ? "s" : "")} Using {initStrengths.Length} Strength{(initStrengths.Length != 1 ? "s" : "")}" : "";
            string log = $"{prompts.Length} Prompt{(prompts.Length != 1 ? "s" : "")} * {iterations} Image{(iterations != 1 ? "s" : "")} * {steps.Length} Step Value{(steps.Length != 1 ? "s" : "")} * {scales.Length} Scale{(scales.Length != 1 ? "s" : "")}{initsStr} = {argLists.Count} Images Total";

            if (ConfigParser.UpscaleAndSaveOriginals())
                log += $" ({argLists.Count * 2} With Post-processed Images)";

            return $"{log}.";
        }

        public static void RunCli(string outPath, string vaePath)
        {
            if (Program.Busy)
                return;

            TextToImage.Canceled = false;
            var allModels = Models.GetModelsAll();
            var cachedModels = allModels.Where(m => m.Type == Enums.Models.Type.Normal).ToList();
            var cachedModelsVae = allModels.Where(m => m.Type == Enums.Models.Type.Vae).ToList();
            Model modelFile = TtiUtils.CheckIfModelExists(Config.Instance.Model, Enums.StableDiffusion.Implementation.InvokeAi);
            Model vaeFile = Models.GetModel(cachedModelsVae, Path.GetFileName(vaePath));

            if (modelFile == null)
                return;

            InvokeAiUtils.WriteModelsYamlAll(cachedModels, cachedModelsVae, ModelArch.Automatic, true);
            if (TextToImage.Canceled) return;

            string batPath = Path.Combine(Paths.GetSessionDataPath(), "invoke.bat");

            string batText = $"@echo off\n" +
                $"title Stable Diffusion CLI (InvokeAI)\n" +
                $"cd /D {Paths.GetDataPath().Wrap()}\n" +
                $"{TtiUtils.GetEnvVarsSdCommand()}\n" +
                $"SET \"INVOKEAI_ROOT={InvokeAiUtils.HomePath}\"\n" +
                $"SET \"INVOKE_NMKD_HIRES_MINDIM_MULT={Config.IniInstance.HiresFixMinimumDimensionMultiplier.ToStringDot("0.0##")}\"\n" +
                $"{Constants.Files.VenvActivate} && " +
                $"python {Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdVenv, "Scripts", "invoke.exe").Wrap()} --model {InvokeAiUtils.GetMdlNameForYaml(modelFile, vaeFile)} -o {outPath.Wrap(true)} {Args.InvokeAi.GetArgsStartup(cachedModels)}";

            File.WriteAllText(batPath, batText);
            Process cli = Process.Start(batPath);
            OsUtils.AttachOrphanHitman(cli);
        }

        public static void StartCmdInSdEnv()
        {
            Process.Start("cmd", $"/K title Env: {Constants.Dirs.SdVenv} && cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSdCommand(true, Paths.GetDataPath())} && {Constants.Files.VenvActivate}");
        }

        /// <summary> Run InvokeAI post-processing (!fix) </summary>
        /// <returns> Successful or not </returns>
        public static async Task<bool> RunFix(string imgPath, List<FixAction> actions)
        {
            if (Program.Busy)
            {
                UiUtils.ShowMessageBox("Can't run post-processing while the program is still busy.");
                return false;
            }

            if (!InstallationStatus.HasSdUpscalers())
            {
                UiUtils.ShowMessageBox("Upscalers are not installed. You can install them in the installer window.");
                return false;
            }

            if (TtiProcess.CurrentStdInWriter == null)
            {
                UiUtils.ShowMessageBox("Can't run post-processing when Stable Diffusion is not loaded.");
                return false;
            }

            try
            {
                Program.SetState(Program.BusyState.PostProcessing);

                Logger.Log($"InvokeAI !fix: {string.Join(", ", actions.Select(x => x.ToString()))}", true);

                string tempPath = IoUtils.GetAvailablePath(Path.Combine(Paths.GetSessionDataPath(), $"postproc{FormatUtils.GetUnixTimestamp()}.png"));
                File.Copy(imgPath, tempPath);
                string suffix = $"{(actions.Contains(FixAction.Upscale) ? ".upscale" : "")}{(actions.Contains(FixAction.FaceRestoration) ? ".facefix" : "")}";
                PostProcessMovePaths.Add(Path.GetFileNameWithoutExtension(tempPath), IoUtils.FilenameSuffix(imgPath, suffix));

                List<string> args = new List<string> { "!fix", tempPath.Wrap(true) };

                if (actions.Contains(FixAction.Upscale))
                    args.Add(Args.InvokeAi.GetUpscaleArgs(true));

                if (actions.Contains(FixAction.FaceRestoration))
                    args.Add(Args.InvokeAi.GetFaceRestoreArgs(true));

                bool success = await TtiProcess.WriteStdIn(string.Join(" ", args), 0, true);

                if (!success)
                    throw new Exception("Can't interact with process. Possibly it is not running?");
                else
                    return true;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
                Program.SetState(Program.BusyState.Standby);
                return false;
            }
        }

        public async Task SetModel(Model mdl, Model vae, bool initial = false)
        {
            if (mdl.Format == Enums.Models.Format.Diffusers)
                Models.SetDiffusersClipSkip(mdl, Config.Instance.ClipSkip);

            if (initial) // No need to run !switch when loading InvokeAI for the first time
                return;

            await TtiProcess.WriteStdIn($"!clear");
            await TtiProcess.WriteStdIn($"!switch {InvokeAiUtils.GetMdlNameForYaml(mdl, vae)}");
        }

        public async Task Cancel()
        {
            List<string> lastLogLines = Logger.GetLastLines(Constants.Lognames.Sd, 15);

            if (lastLogLines.Where(x => x.Contains("%|") || x.Contains("error occurred")).Any()) // Only attempt a soft cancel if we've been generating anything
                await WaitForCancel();
            else // This condition should be true if we cancel while it's still initializing, so we can just force kill the process
                TtiProcess.KillAll();
        }

        public void SendCtrlC()
        {
            if (Pid >= 0)
                OsUtils.SendCtrlC(Pid);
        }

        public void HandleOutput(string line)
        {
            if (TextToImage.Canceled || TextToImage.CurrentTaskSettings == null || line == null)
                return;

            Logger.Log(line, true, false, Constants.Lognames.Sd);
            TtiProcessOutputHandler.LastMessages.Insert(0, line);

            bool ellipsis = Program.MainForm.LogText.EndsWith("...");
            string errMsg = "";
            bool replace = ellipsis || Logger.LastUiLine.MatchesWildcard("*Generated*image*in*");

            if (!TextToImage.Canceled && line.StartsWith(">> Retrieving model "))
                _lastModelCached = true;
            else if (!TextToImage.Canceled && line.StartsWith(">> Loading ") && line.Contains(" from "))
                _lastModelCached = false;

            if (!TextToImage.Canceled && _hiresFixStatus == HiresFixStatus.WaitingForStart && line.Remove(" ").MatchesWildcard(@"0%||0/*[00:00<?,?it/s]*"))
            {
                _hiresFixStatus = HiresFixStatus.Upscaling;
            }

            if (!TextToImage.Canceled && line.Remove(" ") == @"Generating:0%||0/1[00:00<?,?it/s]")
            {
                ImageExport.TimeSinceLastImage.Restart();
                _hiresFixStatus = HiresFixStatus.NotUpscaling;
            }

            if (!TextToImage.Canceled && line.StartsWith(">> Interpolating from"))
            {
                _hiresFixStatus = _hiresFixStatus = HiresFixStatus.WaitingForStart;
            }

            if (!TextToImage.Canceled && !TextToImage.Canceling && line.MatchesWildcard("*%|*|*/*") && !line.Lower().Contains("downloading") && !line.Contains("Loading pipeline components"))
            {
                string progStr = line.Split('|')[2].Trim().Split(' ')[0].Trim(); // => e.g. "3/50"

                if (!Logger.LastUiLine.MatchesWildcard("*Generated*image*in*"))
                    Logger.LogIfLastLineDoesNotContainMsg($"Generating...", false, ellipsis);

                try
                {
                    int progressDivisor = TextToImage.CurrentTaskSettings.HiresFix ? 2 : 1;
                    int[] stepsCurrentTarget = progStr.Split('/').Select(x => x.GetInt()).ToArray();
                    int percent = ((((float)stepsCurrentTarget[0] / stepsCurrentTarget[1]) * 100f) / progressDivisor).RoundToInt();

                    if (TextToImage.CurrentTaskSettings.HiresFix && _hiresFixStatus == HiresFixStatus.Upscaling)
                        percent += 50;

                    if (percent > 0 && percent <= 100)
                        Program.MainForm.SetProgressImg(percent);
                }
                catch { }
            }

            if (!TextToImage.Canceled && line.StartsWith("[") && !line.Contains(".png: !fix "))
            {
                string outPath = Path.Combine(Paths.GetSessionDataPath(), "out").Replace('\\', '/');

                if (line.Contains(outPath) && new Regex(@"\[\d+(\.\d+)?\] [A-Z]:\/").Matches(line.Trim()).Count >= 1)
                {
                    string path = outPath + line.Split(outPath)[1].Split(':')[0];
                    ImageExport.Export(path);
                }
            }

            if (line.Contains(".png: !fix "))
            {
                try
                {
                    if (LastMessages.Take(5).Any(x => x.Contains("ESRGAN is disabled.")))
                    {
                        Logger.Log($"Post-Processing is disabled, can't run enhancement.");
                    }
                    else
                    {
                        string pathSource = line.Split(": !fix ")[1].Split(".png ")[0] + ".png";
                        string pathOut = line.Substring(line.IndexOf("] ") + 2).Split(": !fix ")[0];
                        TtiUtils.ExportPostprocessedImage(pathSource, pathOut);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log($"Error parsing !fix log message: {ex.Message}\n{ex.StackTrace}", true);
                }
            }

            if (line.Trim().StartsWith("invoke_pid="))
            {
                Pid = line.Split('=')[1].GetInt();
            }

            if (line.Trim().StartsWith(Constants.LogMsgs.Invoke.TiTriggers))
            {
                Logger.Log($"Model {(_lastModelCached ? " retrieved from RAM cache" : "loaded")}.", false, ellipsis);
            }

            if (line.Trim().StartsWith(">> Preparing tokens for textual inversion"))
            {
                Logger.Log("Loading textual inversion...", false, ellipsis);
            }

            // if (line.Trim().StartsWith(">> Embedding not found:"))
            // {
            //     string emb = line.Split(": ").Last();
            //     Logger.Log($"Warning: No compatible embedding with trigger '{emb}' found!", false, ellipsis);
            // }

            if (line.Trim().StartsWith(">> Converting legacy checkpoint"))
            {
                Logger.Log($"Warning: Model is not in Diffusers format, this makes loading slower due to conversion. For a speedup, convert it to a Diffusers model.", false, ellipsis);
            }

            if (line.Contains("is not a known model name. Cannot change"))
            {
                Logger.Log($"No model with this name and VAE found. Can't change model.", false, ellipsis);
            }

            if (!_hasErrored && line.Contains("An error occurred while processing your prompt"))
            {
                Logger.Log(line);
                _hasErrored = true;
            }

            if (!_hasErrored && line.Trim().EndsWith(" is not a known model name. Please check your models.yaml file"))
            {
                errMsg = $"Failed to switch models.\n\nPossibly you tried to load an incompatible model.";
                _hasErrored = true;
            }

            if (!_hasErrored && line.Trim().StartsWith("** model ") && line.Contains("could not be loaded:"))
            {
                errMsg = $"Failed to load model.";

                if (line.Contains("state_dict"))
                    errMsg += $"\n\nThe model appears to be incompatible.";

                if (line.Contains("pytorch_model.bin"))
                {
                    errMsg += "\n\nCache seems to be corrupted and has been cleared. Please try again.";
                    IoUtils.TryDeleteIfExists(Path.Combine(Environment.ExpandEnvironmentVariables("%USERPROFILE%"), ".cache", "huggingface", "transformers"));
                    IoUtils.TryDeleteIfExists(Path.Combine(Paths.GetDataPath(), Constants.Dirs.Cache.Root, Constants.Dirs.Cache.Hf));
                }

                _hasErrored = true;
            }

            if (line.MatchesWildcard("Added terms: *, *"))
            {
                if (line.MatchesWildcard("*, <*>"))
                    Logger.Log($"Concept keyword: {line.Split("Added terms: *, ").LastOrDefault()}", false, ellipsis);
                else
                    Logger.Log($"Concept keyword: <{line.Split("Added terms: *, ").LastOrDefault()}>", false, ellipsis);
            }

            TtiProcessOutputHandler.HandleLogGeneric(this, line, _hasErrored);
        }

        public void ResetLogger()
        {
            _hasErrored = false;
            LastMessages.Clear();
        }

        private async Task WaitForCancel()
        {
            SendCtrlC();
            SendCtrlC();
            await Task.Delay(500);

            while (true)
            {
                Logger.Entry lastLogEntry = Logger.GetLastEntries(Constants.Lognames.Sd, 1).FirstOrDefault();

                if (lastLogEntry == null)
                    break;

                var msSinceLastMsg = (DateTime.Now - lastLogEntry.TimeDequeue).TotalMilliseconds;

                if (msSinceLastMsg < 200)
                {
                    SendCtrlC();
                    await Task.Delay(250);
                }
                else if (msSinceLastMsg > 2000)
                {
                    break;
                }

                await Task.Delay(200);
            }
        }
    }
}
