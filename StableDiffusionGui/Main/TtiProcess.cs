using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Main
{
    internal class TtiProcess
    {
        public static Process CurrentProcess;
        public static StreamWriter CurrentStdInWriter;
        public static bool ProcessExistWasIntentional = false;
        public static bool IsAiProcessRunning { get { return CurrentProcess != null && !CurrentProcess.HasExited; } }

        public static void Finish()
        {
            return;
        }

        private static string _lastInvokeStartupSettings;

        public static async Task RunStableDiffusion(string[] prompts, string negPrompt, int iterations, Dictionary<string, string> parameters, string outPath)
        {
            try
            {
                string[] initImgs = parameters.FromJson<string[]>("initImgs"); // List of init images
                string embedding = parameters.FromJson<string>("embedding"); // Textual Inversion embedding file
                float[] initStrengths = parameters.FromJson<float[]>("initStrengths"); // List of init strength values to run
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

                var cachedModels = Paths.GetModels(ModelType.Normal);
                var cachedModelsVae = Paths.GetModels(ModelType.Vae);
                Model modelFile = TtiUtils.CheckIfCurrentSdModelExists();
                Model vaeFile = Paths.GetModel(cachedModelsVae, vae, false, ModelType.Vae);

                if (modelFile == null)
                    return;

                InvokeAiUtils.WriteModelsYamlAll(modelFile, vaeFile, cachedModels, cachedModelsVae);

                Dictionary<string, string> initImages = initImgs != null && initImgs.Length > 0 ? await TtiUtils.CreateResizedInitImagesIfNeeded(initImgs.ToList(), res) : null;

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
                    TextToImage.CurrentTaskSettings.ProcessedAndRawPrompts = processedPrompts.Distinct().ToDictionary(x => x, x => prompt);

                    for (int i = 0; i < iterations; i++)
                    {
                        args.Remove("initImg");
                        args.Remove("initStrength");
                        args["prompt"] = processedPrompts[i].Wrap();
                        args["res"] = $"-W {res.Width} -H {res.Height}";
                        args["sampler"] = $"-A {sampler}";
                        args["seed"] = $"-S {seed}";
                        args["perlin"] = $"--perlin {perlin.ToStringDot()}";
                        args["threshold"] = $"--threshold {threshold}";
                        args["clipSegMask"] = (inpaint == InpaintMode.TextMask && !string.IsNullOrWhiteSpace(clipSegMask)) ? $"-tm {clipSegMask.Wrap()}" : "";

                        foreach (float scale in scales)
                        {
                            args["scale"] = $"-C {scale.ToStringDot()}";

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
                                            args["initImg"] = $"--init_img {initImg.Wrap()}";
                                            args["initStrength"] = $"--strength {strength.ToStringDot("0.###")}";
                                            argLists.Add(new Dictionary<string, string>(args));
                                        }
                                    }
                                }
                            }
                        }

                        if (!lockSeed)
                            seed++;
                    }

                    if (Config.GetBool(Config.Key.checkboxMultiPromptsSameSeed))
                        seed = startSeed;
                }

                List<string> cmds = argLists.Select(argList => string.Join(" ", argList.Where(argEntry => !string.IsNullOrWhiteSpace(argEntry.Value)).Select(argEntry => argEntry.Value))).ToList();

                Logger.Log($"Running Stable Diffusion - {iterations} Iterations, {steps} Steps, Scales {(scales.Length < 4 ? string.Join(", ", scales.Select(x => x.ToStringDot())) : $"{scales.First()}->{scales.Last()}")}, {res.Width}x{res.Height}, Starting Seed: {startSeed}");

                string modelsChecksumStartup = InvokeAiUtils.GetModelsYamlHash();
                string argsStartup = Args.InvokeAi.GetArgsStartup(embedding);

                string newStartupSettings = $"{argsStartup} {modelsChecksumStartup} {Config.GetInt("comboxCudaDevice")}"; // Check if startup settings match - If not, we need to restart the process

                string initsStr = initImages != null ? $" and {initImages.Count} image{(initImages.Count != 1 ? "s" : "")} using {initStrengths.Length} strength{(initStrengths.Length != 1 ? "s" : "")}" : "";
                Logger.Log($"{prompts.Length} prompt{(prompts.Length != 1 ? "s" : "")} with {iterations} iteration{(iterations != 1 ? "s" : "")} each and {scales.Length} scale{(scales.Length != 1 ? "s" : "")}{initsStr} each = {cmds.Count} images total.");

                if (!IsAiProcessRunning || (IsAiProcessRunning && _lastInvokeStartupSettings != newStartupSettings))
                {
                    Logger.Log($"(Re)starting InvokeAI. Process running: {IsAiProcessRunning} - Prev startup string: '{_lastInvokeStartupSettings}' - New startup string: '{newStartupSettings}'", true);

                    _lastInvokeStartupSettings = newStartupSettings;

                    Process py = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd(), Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdVenv, "Scripts", "python.exe"));
                    py.StartInfo.RedirectStandardInput = true;
                    py.StartInfo.WorkingDirectory = Paths.GetDataPath();
                    py.StartInfo.Arguments = $"\"{Constants.Dirs.SdRepo}/scripts/invoke.py\" -o {outPath.Wrap(true)} {argsStartup}";

                    foreach (var pair in TtiUtils.GetEnvVarsSd(false, Paths.GetDataPath()))
                        py.StartInfo.EnvironmentVariables[pair.Key] = pair.Value;

                    TextToImage.CurrentTask.Processes.Add(py);
                    Logger.Log($"{py.StartInfo.FileName} {py.StartInfo.Arguments}", true);

                    if (!OsUtils.ShowHiddenCmd())
                    {
                        py.OutputDataReceived += (sender, line) => { TtiProcessOutputHandler.LogOutput(line.Data); };
                        py.ErrorDataReceived += (sender, line) => { TtiProcessOutputHandler.LogOutput(line.Data, true); };
                    }

                    if (CurrentProcess != null)
                    {
                        ProcessExistWasIntentional = true;
                        OsUtils.KillProcessTree(CurrentProcess.Id);
                    }

                    TtiProcessOutputHandler.Reset();

                    string logMdl = Path.ChangeExtension(model, null).Trunc(!string.IsNullOrWhiteSpace(vae) ? 35 : 80).Wrap();
                    string logVae = Path.GetFileNameWithoutExtension(vae).Trunc(35).Wrap();
                    Logger.Log($"Loading Stable Diffusion with model {logMdl}{(string.IsNullOrWhiteSpace(vae) ? "" : $" and VAE {logVae}")}...");

                    CurrentProcess = py;
                    ProcessExistWasIntentional = false;

                    py.Start();
                    OsUtils.AttachOrphanHitman(py);
                    CurrentStdInWriter = py.StandardInput;

                    if (!OsUtils.ShowHiddenCmd())
                    {
                        py.BeginOutputReadLine();
                        py.BeginErrorReadLine();
                    }

                    Task.Run(() => CheckStillRunning());
                }
                else
                {
                    TtiProcessOutputHandler.Reset();
                    await InvokeAi.SwitchModel(InvokeAiUtils.GetMdlNameForYaml(modelFile, vaeFile));
                    TextToImage.CurrentTask.Processes.Add(CurrentProcess);
                }

                Logger.Log($"Writing to stdin...", true);

                await WriteStdIn("!reset");

                foreach (string command in cmds)
                    await WriteStdIn(command);
            }
            catch (Exception ex)
            {
                Logger.Log($"Unhandled Stable Diffusion Error: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
            }
        }

        public static async Task RunStableDiffusionOpt(string[] prompts, int iterations, Dictionary<string, string> parameters, string outPath)
        {
            // NOTE: Currently not implemented: Embeddings, Samplers, Seamless Mode, ...
            string[] initImgs = parameters.Get("initImgs").FromJson<string[]>();
            float[] initStrengths = parameters.Get("initStrengths").FromJson<float[]>();
            int[] steps = parameters.FromJson<int[]>("steps");
            float[] scales = parameters.Get("scales").FromJson<float[]>();
            long seed = parameters.Get("seed").FromJson<long>();
            Size res = parameters.Get("res").FromJson<Size>();
            string model = parameters.Get("model").FromJson<string>();
            string modelNoExt = Path.ChangeExtension(model, null);
            bool lockSeed = parameters.Get("lockSeed").FromJson<bool>();

            Model modelFile = TtiUtils.CheckIfCurrentSdModelExists();

            if (modelFile == null)
                return;

            Dictionary<string, string> initImages = initImgs != null && initImgs.Length > 0 ? await TtiUtils.CreateResizedInitImagesIfNeeded(initImgs.ToList(), res) : null;

            long startSeed = seed;

            List<Dictionary<string, string>> argLists = new List<Dictionary<string, string>>();

            foreach (string prompt in prompts)
            {
                for (int i = 0; i < iterations; i++)
                {
                    Dictionary<string, string> args = new Dictionary<string, string>();
                    args.Remove("init_img");
                    args.Remove("strength");
                    args["prompt"] = prompt.Wrap();
                    args["W"] = res.Width.ToString();
                    args["H"] = res.Height.ToString();
                    args["seed"] = seed.ToString();

                    foreach (float scale in scales)
                    {
                        args["scale"] = scale.ToStringDot();

                        foreach (int stepCount in steps)
                        {
                            args["ddim_steps"] = stepCount.ToString();

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
                                        args["init_img"] = initImg.Wrap();
                                        args["strength"] = strength.ToStringDot("0.###");
                                        argLists.Add(new Dictionary<string, string>(args));
                                    }
                                }
                            }
                        }
                    }

                    if (!lockSeed)
                        seed++;
                }

                if (Config.GetBool(Config.Key.checkboxMultiPromptsSameSeed))
                    seed = startSeed;
            }

            List<string> promptFileLines = argLists.Select(argList => string.Join(" ", argList.Where(arg => !string.IsNullOrWhiteSpace(arg.Value)).Select(arg => $"--{arg.Key} {arg.Value}"))).ToList();
            string promptFilePath = Path.Combine(Paths.GetSessionDataPath(), "prompts.txt");

            IoUtils.TryDeleteIfExists(promptFilePath); // idk if this is needed, but the line below MIGHT append something so better make sure the previous prompts are deleted
            File.WriteAllLines(promptFilePath, promptFileLines);

            Logger.Log($"Running Stable Diffusion - {iterations} Iterations, {steps} Steps, Scales {(scales.Length < 4 ? string.Join(", ", scales.Select(x => x.ToStringDot())) : $"{scales.First()}->{scales.Last()}")}, {res.Width}x{res.Height}, Starting Seed: {startSeed}");

            string argsStartup = Args.OptimizedSd.GetDefaultArgsStartup();
            string newStartupSettings = $"opt {modelNoExt} {argsStartup} {Config.GetInt("comboxCudaDevice")}"; // Check if startup settings match - If not, we need to restart the process

            string initsStr = initImages != null ? $" and {initImages.Count} image{(initImages.Count != 1 ? "s" : "")} using {initStrengths.Length} strength{(initStrengths.Length != 1 ? "s" : "")}" : "";
            Logger.Log($"{prompts.Length} prompt{(prompts.Length != 1 ? "s" : "")} with {iterations} iteration{(iterations != 1 ? "s" : "")} each and {scales.Length} scale{(scales.Length != 1 ? "s" : "")}{initsStr} each = {promptFileLines.Count} images total.");

            if (!IsAiProcessRunning || (IsAiProcessRunning && _lastInvokeStartupSettings != newStartupSettings))
            {
                _lastInvokeStartupSettings = newStartupSettings;

                Process py = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd(), Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdVenv, "Scripts", "python.exe"));
                py.StartInfo.RedirectStandardInput = true;
                py.StartInfo.WorkingDirectory = Paths.GetDataPath();
                py.StartInfo.Arguments = $"\"{Constants.Dirs.SdRepo}/optimizedSD/optimized_txt2img_loop.py\" --model {modelFile.FullName.Wrap(true)} --outdir {outPath.Wrap(true)} --from_file_loop={promptFilePath.Wrap(true)} {argsStartup}";

                foreach (var pair in TtiUtils.GetEnvVarsSd(false, Paths.GetDataPath()))
                    py.StartInfo.EnvironmentVariables[pair.Key] = pair.Value;

                TextToImage.CurrentTask.Processes.Add(py);
                Logger.Log($"{py.StartInfo.FileName} {py.StartInfo.Arguments}", true);

                if (!OsUtils.ShowHiddenCmd())
                {
                    py.OutputDataReceived += (sender, line) => { TtiProcessOutputHandler.LogOutput(line.Data); };
                    py.ErrorDataReceived += (sender, line) => { TtiProcessOutputHandler.LogOutput(line.Data, true); };
                }

                if (CurrentProcess != null)
                {
                    ProcessExistWasIntentional = true;
                    OsUtils.KillProcessTree(CurrentProcess.Id);
                }

                TtiProcessOutputHandler.Reset();
                Logger.Log($"Loading Stable Diffusion with model {modelNoExt.Wrap()}...");
                CurrentProcess = py;
                ProcessExistWasIntentional = false;
                py.Start();
                OsUtils.AttachOrphanHitman(py);

                if (!OsUtils.ShowHiddenCmd())
                {
                    py.BeginOutputReadLine();
                    py.BeginErrorReadLine();
                }

                Task.Run(() => CheckStillRunning());
            }
            else
            {
                TextToImage.CurrentTask.Processes.Add(CurrentProcess);
            }
        }

        public static async Task RunStableDiffusionCli(string outPath, string vaePath)
        {
            if (Program.Busy)
                return;

            var cachedModels = Paths.GetModels(ModelType.Normal);
            var cachedModelsVae = Paths.GetModels(ModelType.Vae);
            Model modelFile = TtiUtils.CheckIfCurrentSdModelExists();
            Model vaeFile = Paths.GetModel(cachedModelsVae, Path.GetFileName(vaePath), false, ModelType.Vae);

            if (modelFile == null)
                return;

            InvokeAiUtils.WriteModelsYamlAll(modelFile, vaeFile, cachedModels, cachedModelsVae);

            string batPath = Path.Combine(Paths.GetSessionDataPath(), "invoke.bat");

            string batText = $"@echo off\n" +
                $"title Stable Diffusion CLI (InvokeAI)\n" +
                $"cd /D {Paths.GetDataPath().Wrap()}\n" +
                $"{TtiUtils.GetEnvVarsSdCommand()}\n" +
                $"python {Constants.Dirs.SdRepo}/scripts/invoke.py -o {outPath.Wrap(true)} {Args.InvokeAi.GetArgsStartup()}";

            File.WriteAllText(batPath, batText);
            ProcessManager.FindAndKillOrphans($"*invoke.py*{outPath}*");
            Process cli = Process.Start(batPath);
            OsUtils.AttachOrphanHitman(cli);
        }

        public static void StartCmdInSdEnv(bool conda)
        {
            if (conda)
            {
                Process.Start("cmd", $"/K title Env: {Constants.Dirs.SdEnv} && cd /D {Paths.GetDataPath().Wrap()} && " +
                    $"{TtiUtils.GetEnvVarsSdCommand(true, Paths.GetDataPath())} && call activate.bat {Constants.Dirs.Conda}/envs/{Constants.Dirs.SdEnv}");
            }
            else
            {
                Process.Start("cmd", $"/K title Env: {Constants.Dirs.SdVenv} && cd /D {Paths.GetDataPath().Wrap()} && " +
                    $"{TtiUtils.GetEnvVarsSdCommand(true, Paths.GetDataPath())}");
            }
        }

        public static async Task<bool> WriteStdIn(string text, bool ignoreCanceled = false, bool newLine = true)
        {
            try
            {
                if ((!ignoreCanceled && TextToImage.Canceled) || CurrentStdInWriter == null)
                    return false;

                Logger.Log($"=> {text}", true);

                if (newLine)
                    await CurrentStdInWriter.WriteLineAsync(text);
                else
                    await CurrentStdInWriter.WriteAsync(text);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void Kill()
        {
            Logger.Log($"Killing current task's processes.", true);

            if (TextToImage.CurrentTask != null)
            {
                foreach (var process in TextToImage.CurrentTask.Processes)
                {
                    try
                    {
                        if (process != null && !process.HasExited)
                        {
                            if (process == CurrentProcess)
                                ProcessExistWasIntentional = true;

                            OsUtils.KillProcessTree(process.Id);
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Log($"Failed to kill process tree: {e.Message}", true);
                    }
                }
            }
        }

        public static async Task CheckStillRunning()
        {
            while (CurrentProcess != null && !CurrentProcess.HasExited)
                await Task.Delay(1);

            if (TextToImage.Canceled)
                return;

            if (ProcessExistWasIntentional)
            {
                ProcessExistWasIntentional = false;
            }
            else
            {
                string log = "...\n" + string.Join("\n", Logger.GetLastLines(Constants.Lognames.Sd, 8));
                TextToImage.Cancel($"Process has exited unexpectedly.\n\nOutput:\n{log}");
            }
        }
    }
}
