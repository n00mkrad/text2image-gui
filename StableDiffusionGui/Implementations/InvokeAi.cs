using StableDiffusionGui.Data;
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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Implementations
{
    internal class InvokeAi
    {
        public enum FixAction { Upscale, FaceRestoration }

        public static Dictionary<string, string> PostProcessMovePaths = new Dictionary<string, string>();

        public static async Task Run(string[] prompts, string negPrompt, int iterations, Dictionary<string, string> parameters, string outPath)
        {
            try
            {
                string[] initImgs = parameters.FromJson<string[]>("initImgs"); // List of init images
                float[] initStrengths = parameters.FromJson<float[]>("initStrengths").Select(n => 1f - n).ToArray(); ; // List of init strength values to run
                int[] steps = parameters.FromJson<int[]>("steps"); // List of diffusion step counts
                float[] scales = parameters.FromJson<float[]>("scales"); // List of CFG scale values to run
                long seed = parameters.FromJson<long>("seed"); // Initial seed
                string sampler = parameters.FromJson<string>("sampler"); // Sampler
                Size res = parameters.FromJson<Size>("res"); // Image resolution
                var seamless = parameters.FromJson<SeamlessMode>("seamless"); // Seamless generation mode
                string model = parameters.FromJson<string>("model"); // Model name
                bool hiresFix = parameters.FromJson<bool>("hiresFix"); // Enable high-resolution fix
                bool lockSeed = parameters.FromJson<bool>("lockSeed"); // Lock seed (disable auto-increment)
                string vae = parameters.FromJson<string>("vae").NullToEmpty().Replace("None", ""); // VAE model name
                float perlin = parameters.FromJson<float>("perlin"); // Perlin noise blend value
                int threshold = parameters.FromJson<int>("threshold"); // Threshold value
                InpaintMode inpaint = parameters.FromJson<InpaintMode>("inpainting"); // Inpainting mode
                string clipSegMask = parameters.FromJson<string>("clipSegMask"); // ClipSeg text-based masking prompt
                ImageMagick.Gravity resizeGravity = parameters.FromJson<ImageMagick.Gravity>("resizeGravity", (ImageMagick.Gravity)(1)); // Inpainting mode

                var allModels = Models.GetModelsAll();
                var cachedModels = allModels.Where(m => m.Type == Enums.Models.Type.Normal).ToList();
                var cachedModelsVae = allModels.Where(m => m.Type == Enums.Models.Type.Vae).ToList();
                Model modelFile = TtiUtils.CheckIfCurrentSdModelExists();
                Model vaeFile = Models.GetModel(cachedModelsVae, vae, false, Enums.Models.Type.Vae);
                if (TextToImage.Canceled) return;

                OrderedDictionary initImages = initImgs != null && initImgs.Length > 0 ? await TtiUtils.CreateResizedInitImagesIfNeeded(initImgs.ToList(), res, resizeGravity) : null;

                long startSeed = seed;
                prompts = prompts.Select(p => FormatUtils.GetCombinedPrompt(p, negPrompt)).ToArray(); // Apply negative prompt

                List<Dictionary<string, string>> argLists = new List<Dictionary<string, string>>(); // List of all args for each command
                Dictionary<string, string> args = new Dictionary<string, string>(); // List of args for current command
                args["prompt"] = "";
                args["default"] = Args.InvokeAi.GetDefaultArgsCommand();
                args["upscale"] = Args.InvokeAi.GetUpscaleArgs();
                args["facefix"] = Args.InvokeAi.GetFaceRestoreArgs();
                args["seamless"] = Args.InvokeAi.GetSeamlessArg(seamless);
                args["hiresFix"] = hiresFix ? "--hires_fix" : "";

                foreach (string prompt in prompts)
                {
                    List<string> processedPrompts = PromptWildcardUtils.ApplyWildcardsAll(prompt, iterations, false);
                    TextToImage.CurrentTaskSettings.ProcessedAndRawPrompts = new EasyDict<string, string>(processedPrompts.Distinct().ToDictionary(x => x, x => prompt));

                    for (int i = 0; i < iterations; i++)
                    {
                        args.Remove("initImg");
                        args.Remove("initStrength");
                        args.Remove("inpaintMask");
                        args["prompt"] = processedPrompts[i].Wrap();
                        args["res"] = $"-W {res.Width} -H {res.Height}";
                        args["sampler"] = $"-A {sampler}";
                        args["seed"] = $"-S {seed}";
                        args["perlin"] = perlin > 0f ? $"--perlin {perlin.ToStringDot()}" : "";
                        args["threshold"] = threshold > 0 ? $"--threshold {threshold}" : "";
                        args["clipSegMask"] = (inpaint == InpaintMode.TextMask && !string.IsNullOrWhiteSpace(clipSegMask)) ? $"-tm {clipSegMask.Wrap()}" : "";
                        args["debug"] = parameters.FromJson<string>("appendArgs");

                        foreach (float scale in scales)
                        {
                            args["scale"] = $"-C {scale.Clamp(0.01f, 1000f).ToStringDot()}";

                            foreach (int stepCount in steps)
                            {
                                args["steps"] = $"-s {stepCount}";

                                if (initImages == null) // No init image(s)
                                {
                                    argLists.Add(new Dictionary<string, string>(args));
                                }
                                else // With init image(s)
                                {
                                    foreach (string initImg in initImages.Values)
                                    {
                                        foreach (float strength in initStrengths)
                                        {
                                            args["initImg"] = $"-I {initImg.Wrap()}";
                                            args["initStrength"] = inpaint != InpaintMode.Disabled ? "-f 1.0" : $"-f {strength.ToStringDot("0.###")}"; // Lock to 1.0 when using inpainting

                                            if (inpaint == InpaintMode.ImageMask)
                                                args["inpaintMask"] = $"-M {Inpainting.MaskedImagePath.Wrap()}";

                                            if (inpaint == InpaintMode.Outpaint)
                                                args["inpaintMask"] = "--force_outpaint";

                                            argLists.Add(new Dictionary<string, string>(args));
                                        }
                                    }
                                }
                            }
                        }

                        if (!lockSeed)
                            seed++;
                    }

                    if (Config.Get<bool>(Config.Keys.MultiPromptsSameSeed))
                        seed = startSeed;
                }

                List<string> cmds = argLists.Select(argList => string.Join(" ", argList.Where(argEntry => argEntry.Value.IsNotEmpty()).Select(argEntry => argEntry.Value))).ToList();

                Logger.Log($"Running Stable Diffusion - {iterations} Iterations, {steps.Length} Steps, Scales {(scales.Length < 4 ? string.Join(", ", scales.Select(x => x.ToStringDot())) : $"{scales.First()}->{scales.Last()}")}, {res.Width}x{res.Height}, Starting Seed: {startSeed}", false, Logger.LastUiLine.EndsWith("..."));

                string modelsChecksumStartup = InvokeAiUtils.GetModelsYamlHash();
                string argsStartup = Args.InvokeAi.GetArgsStartup();
                string newStartupSettings = $"{argsStartup} {modelsChecksumStartup} {Config.Get<int>(Config.Keys.CudaDeviceIdx)}"; // Check if startup settings match - If not, we need to restart the process

                string initsStr = initImages != null ? $" and {initImages.Count} image{(initImages.Count != 1 ? "s" : "")} using {initStrengths.Length} strength{(initStrengths.Length != 1 ? "s" : "")}" : "";
                Logger.Log($"{prompts.Length} prompt{(prompts.Length != 1 ? "s" : "")} * {iterations} image{(iterations != 1 ? "s" : "")} * {steps.Length} step value{(steps.Length != 1 ? "s" : "")} * {scales.Length} scale{(scales.Length != 1 ? "s" : "")}{initsStr} = {cmds.Count} images total.");

                Logger.Clear(Constants.Lognames.Sd);

                if (!TtiProcess.IsAiProcessRunning || (TtiProcess.IsAiProcessRunning && TtiProcess.LastStartupSettings != newStartupSettings))
                {
                    await InvokeAiUtils.WriteModelsYamlAll(modelFile, vaeFile, cachedModels, cachedModelsVae);
                    if (TextToImage.Canceled) return;

                    Logger.Log($"(Re)starting InvokeAI. Process running: {TtiProcess.IsAiProcessRunning} - Prev startup string: '{TtiProcess.LastStartupSettings}' - New startup string: '{newStartupSettings}'", true);

                    TtiProcess.LastStartupSettings = newStartupSettings;

                    Process py = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd(), Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdVenv, "Scripts", "python.exe"));
                    py.StartInfo.RedirectStandardInput = true;
                    py.StartInfo.WorkingDirectory = Paths.GetDataPath();
                    py.StartInfo.Arguments = $"\"{Constants.Dirs.SdRepo}/invoke/scripts/invoke.py\" --model {InvokeAiUtils.GetMdlNameForYaml(modelFile, vaeFile)} -o {outPath.Wrap(true)} {argsStartup}";

                    foreach (var pair in TtiUtils.GetEnvVarsSd(false, Paths.GetDataPath()))
                        py.StartInfo.EnvironmentVariables[pair.Key] = pair.Value;

                    TextToImage.CurrentTask.Processes.Add(py);
                    Logger.Log($"{py.StartInfo.FileName} {py.StartInfo.Arguments}", true);

                    if (!OsUtils.ShowHiddenCmd())
                    {
                        py.OutputDataReceived += (sender, line) => { TtiProcessOutputHandler.LogOutput(line.Data); };
                        py.ErrorDataReceived += (sender, line) => { TtiProcessOutputHandler.LogOutput(line.Data, true); };
                    }

                    if (TtiProcess.CurrentProcess != null)
                    {
                        TtiProcess.ProcessExistWasIntentional = true;
                        OsUtils.KillProcessTree(TtiProcess.CurrentProcess.Id);
                    }

                    TtiProcessOutputHandler.Reset();

                    string logMdl = Path.ChangeExtension(model, null).Trunc(!string.IsNullOrWhiteSpace(vae) ? 35 : 80).Wrap();
                    string logVae = Path.GetFileNameWithoutExtension(vae).Trunc(35).Wrap();
                    Logger.Log($"Loading Stable Diffusion with model {logMdl}{(string.IsNullOrWhiteSpace(vae) ? "" : $" and VAE {logVae}")}...");

                    TtiProcess.CurrentProcess = py;
                    TtiProcess.ProcessExistWasIntentional = false;

                    py.Start();
                    OsUtils.AttachOrphanHitman(py);
                    TtiProcess.CurrentStdInWriter = new NmkdStreamWriter(py);

                    if (!OsUtils.ShowHiddenCmd())
                    {
                        py.BeginOutputReadLine();
                        py.BeginErrorReadLine();
                    }

                    Task.Run(() => TtiProcess.CheckStillRunning());
                }
                else
                {
                    TtiProcessOutputHandler.Reset();
                    await SwitchModel(InvokeAiUtils.GetMdlNameForYaml(modelFile, vaeFile));
                    TextToImage.CurrentTask.Processes.Add(TtiProcess.CurrentProcess);
                }

                foreach (string command in cmds)
                    await TtiProcess.WriteStdIn(command);
            }
            catch (Exception ex)
            {
                Logger.Log($"Unhandled Stable Diffusion Error: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
            }
        }

        public static async Task RunCli(string outPath, string vaePath)
        {
            if (Program.Busy)
                return;

            TextToImage.Canceled = false;
            var cachedModels = Models.GetModels(Enums.Models.Type.Normal);
            var cachedModelsVae = Models.GetModels(Enums.Models.Type.Vae);
            Model modelFile = TtiUtils.CheckIfCurrentSdModelExists();
            Model vaeFile = Models.GetModel(cachedModelsVae, Path.GetFileName(vaePath), false, Enums.Models.Type.Vae);

            if (modelFile == null)
                return;

            await InvokeAiUtils.WriteModelsYamlAll(modelFile, vaeFile, cachedModels, cachedModelsVae, true);
            if (TextToImage.Canceled) return;

            string batPath = Path.Combine(Paths.GetSessionDataPath(), "invoke.bat");

            string batText = $"@echo off\n" +
                $"title Stable Diffusion CLI (InvokeAI)\n" +
                $"cd /D {Paths.GetDataPath().Wrap()}\n" +
                $"{TtiUtils.GetEnvVarsSdCommand()}\n" +
                $"python {Constants.Dirs.SdRepo}/invoke/scripts/invoke.py --model {InvokeAiUtils.GetMdlNameForYaml(modelFile, vaeFile)} -o {outPath.Wrap(true)} {Args.InvokeAi.GetArgsStartup()}";

            File.WriteAllText(batPath, batText);
            Process cli = Process.Start(batPath);
            OsUtils.AttachOrphanHitman(cli);
        }

        public static void StartCmdInSdEnv()
        {
            Process.Start("cmd", $"/K title Env: {Constants.Dirs.SdVenv} && cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSdCommand(true, Paths.GetDataPath())}");
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

                string tempPath = IoUtils.GetAvailableFilePath(Path.Combine(Paths.GetSessionDataPath(), $"postproc{FormatUtils.GetUnixTimestamp()}.png"));
                File.Copy(imgPath, tempPath);
                string suffix = $"{(actions.Contains(FixAction.Upscale) ? ".upscale" : "")}{(actions.Contains(FixAction.FaceRestoration) ? ".facefix" : "")}";
                PostProcessMovePaths.Add(Path.GetFileNameWithoutExtension(tempPath), IoUtils.FilenameSuffix(imgPath, suffix));

                List<string> args = new List<string> { "!fix", tempPath.Wrap(true) };

                if (actions.Contains(FixAction.Upscale))
                    args.Add(Args.InvokeAi.GetUpscaleArgs(true));

                if (actions.Contains(FixAction.FaceRestoration))
                    args.Add(Args.InvokeAi.GetFaceRestoreArgs(true));

                bool success = await TtiProcess.WriteStdIn(string.Join(" ", args), true);

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

        public static async Task SwitchModel (string modelNameInYaml)
        {
            //NmkdStopwatch timeoutSw = new NmkdStopwatch();
            await TtiProcess.WriteStdIn($"!switch {modelNameInYaml}");

            // Logger.Log("SwitchModel waiting...", true);
            // 
            // while (true)
            // {
            //     await Task.Delay(1);
            //     var last2 = Logger.GetLastLines(Constants.Lognames.Sd, 2).Select(l => l.Split("invoke> ").Last().Trim());
            // 
            //     if (last2.Where(l => l.StartsWith($"New model is current model")).Any())
            //         return;
            // 
            //     if (last2.Where(l => l.StartsWith($"Changing model")).Any())
            //         break;
            // 
            //     if(timeoutSw.ElapsedMs > 10000)
            //     {
            //         Logger.Log($"Error switching model: Timed out. (1)");
            //         return;
            //     }
            // }
            // 
            // Logger.Log("Loading model...");
            // 
            // while (true)
            // {
            //     await Task.Delay(10);
            // 
            //     if (Logger.GetLastLines(Constants.Lognames.Sd, 15).Where(l => l.Trim().EndsWith($" is not a known model name. Please check your models.yaml file")).Any())
            //         break;
            // 
            //     if (Logger.GetLastLines(Constants.Lognames.Sd, 15).Where(l => l.Contains($" {modelNameInYaml} from ")).Any())
            //         break;
            // 
            //     if (timeoutSw.ElapsedMs > 60000)
            //     {
            //         Logger.Log($"Error switching model: Timed out. (2)");
            //         return;
            //     }
            // }
            // 
            // while (true)
            // {
            //     await Task.Delay(10);
            // 
            //     if (Logger.GetLastLines(Constants.Lognames.Sd, 5).Where(l => l.StartsWith(">> Setting Sampler to ")).Any())
            //         break;
            // 
            //     if (timeoutSw.ElapsedMs > 60000)
            //     {
            //         Logger.Log($"Error switching model: Timed out. (3)");
            //         return;
            //     }
            // }
        }

        public static async Task Cancel ()
        {
            List<string> lastLogLines = Logger.GetLastLines(Constants.Lognames.Sd, 15);

            if (lastLogLines.Where(x => x.Contains("%|") || x.Contains("error occurred")).Any()) // Only attempt a soft cancel if we've been generating anything
                await WaitForCancel();
            else // This condition should be true if we cancel while it's still initializing, so we can just force kill the process
                TtiProcess.Kill();
        }

        private static async Task WaitForCancel()
        {
            Program.MainForm.runBtn.Enabled = false;
            DateTime cancelTime = DateTime.Now;
            TtiUtils.SoftCancelInvokeAi();
            await Task.Delay(100);

            KeyValuePair<string, TimeSpan> previousLastLine = new KeyValuePair<string, TimeSpan>();

            while (true)
            {
                var entries = Logger.GetLastEntries(Constants.Lognames.Sd, 5);
                Dictionary<string, TimeSpan> linesWithAge = new Dictionary<string, TimeSpan>();

                foreach (Logger.Entry entry in entries)
                    linesWithAge[entry.Message] = DateTime.Now - entry.TimeDequeue;

                linesWithAge = linesWithAge.Where(x => x.Value.TotalMilliseconds >= 0).ToDictionary(p => p.Key, p => p.Value);

                if (linesWithAge.Count > 0)
                {
                    var lastLine = linesWithAge.Last();

                    if (lastLine.Value.TotalMilliseconds > 2000)
                        break;

                    bool linesChanged = !string.IsNullOrWhiteSpace(previousLastLine.Key) && lastLine.Key != previousLastLine.Key && lastLine.Value.TotalMilliseconds < 500;

                    if (linesChanged && !lastLine.Key.Contains("skipped")) // If lines changed (= still outputting), send ctrl+c again
                        TtiUtils.SoftCancelInvokeAi();

                    previousLastLine = lastLine;
                }

                await Task.Delay(100);
            }

            Program.MainForm.runBtn.Enabled = true;
        }
    }
}
