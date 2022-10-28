using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;

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

        public static async Task RunStableDiffusion(string[] prompts, int iterations, Dictionary<string, string> paramsDict, string outPath)
        {
            string[] initImgs = paramsDict.Get("initImgs").FromJson<string[]>();
            string embedding = paramsDict.Get("embedding").FromJson<string>();
            float[] initStrengths = paramsDict.Get("initStrengths").FromJson<float[]>();
            int steps = paramsDict.Get("steps").FromJson<int>();
            float[] scales = paramsDict.Get("scales").FromJson<float[]>();
            long seed = paramsDict.Get("seed").FromJson<long>();
            string sampler = paramsDict.Get("sampler").FromJson<string>();
            Size res = paramsDict.Get("res").FromJson<Size>();
            bool seamless = paramsDict.Get("seamless").FromJson<bool>();
            string model = paramsDict.Get("model").FromJson<string>();
            bool hiresFix = paramsDict.Get("hiresFix").FromJson<bool>();
            bool lockSeed = paramsDict.Get("lockSeed").FromJson<bool>();
            string vae = paramsDict.Get("vae").FromJson<string>().Replace("None", "");

            if (!TtiUtils.CheckIfSdModelExists())
                return;

            Dictionary<string, string> initImages = initImgs != null && initImgs.Length > 0 ? await TtiUtils.CreateResizedInitImagesIfNeeded(initImgs.ToList(), res) : null;

            TtiUtils.WriteModelsYaml(model, vae);

            long startSeed = seed;

            List<string> cmds = new List<string>();

            int imgs = 0;

            foreach (string prompt in prompts)
            {
                PromptWildcardUtils.Reset();

                for (int i = 0; i < iterations; i++)
                {
                    foreach (float scale in scales)
                    {
                        if (initImages == null) // No init image(s)
                        {
                            List<string> args = new List<string>();
                            string p = PromptWildcardUtils.ApplyWildcards(prompt, iterations);
                            TextToImage.CurrentTaskSettings.ProcessedAndRawPrompts[FormatUtils.ConvertTextEncoding(p)] = prompt; // Save the prompt we stored plus the processed (wildcards etc.) one for later reference
                            args.Add(p.Wrap());
                            args.Add($"-n 1");
                            args.Add($"-s {steps}");
                            args.Add($"-C {scale.ToStringDot()}");
                            args.Add($"-A {sampler}");
                            args.Add($"-W {res.Width} -H {res.Height}");
                            args.Add($"-S {seed}");
                            args.Add(ArgsInvoke.GetUpscaleArgs());
                            args.Add(ArgsInvoke.GetFaceRestoreArgs());
                            args.Add(seamless ? "--seamless" : "");
                            args.Add(hiresFix ? "--hires_fix" : "");
                            args.Add(ArgsInvoke.GetDefaultArgsCommand());

                            cmds.Add(string.Join(" ", args.Where(x => !string.IsNullOrWhiteSpace(x))));

                            imgs++;
                        }
                        else // With init image(s)
                        {
                            foreach (string initImg in initImages.Values)
                            {
                                foreach (float strength in initStrengths)
                                {
                                    List<string> args = new List<string>();
                                    string p = PromptWildcardUtils.ApplyWildcards(prompt, iterations);
                                    TextToImage.CurrentTaskSettings.ProcessedAndRawPrompts[FormatUtils.ConvertTextEncoding(p)] = prompt; // Save the prompt we stored plus the processed (wildcards etc.) one for later reference
                                    args.Add(p.Wrap());
                                    args.Add($"--init_img {initImg.Wrap()} --strength {strength.ToStringDot("0.###")}");
                                    args.Add($"-n 1");
                                    args.Add($"-s {steps}");
                                    args.Add($"-C {scale.ToStringDot()}");
                                    args.Add($"-A ddim");
                                    args.Add($"-W {res.Width} -H {res.Height}");
                                    args.Add($"-S {seed}");
                                    args.Add(ArgsInvoke.GetUpscaleArgs());
                                    args.Add(ArgsInvoke.GetFaceRestoreArgs());
                                    args.Add(seamless ? "--seamless" : "");
                                    args.Add(hiresFix ? "--hires_fix" : "");
                                    args.Add(ArgsInvoke.GetDefaultArgsCommand());

                                    cmds.Add(string.Join(" ", args.Where(x => !string.IsNullOrWhiteSpace(x))));

                                    imgs++;
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

            Logger.Log($"Running Stable Diffusion - {iterations} Iterations, {steps} Steps, Scales {(scales.Length < 4 ? string.Join(", ", scales.Select(x => x.ToStringDot())) : $"{scales.First()}->{scales.Last()}")}, {res.Width}x{res.Height}, Starting Seed: {startSeed}");

            string precArg = ArgsInvoke.GetPrecisionArg();
            string embArg = ArgsInvoke.GetEmbeddingArg(embedding);

            string newStartupSettings = $"{model}{vae}{precArg}{embArg}{Config.GetInt("comboxCudaDevice")}"; // Check if startup settings match - If not, we need to restart the process

            string initsStr = initImages != null ? $" and {initImages.Count} image{(initImages.Count != 1 ? "s" : "")} using {initStrengths.Length} strength{(initStrengths.Length != 1 ? "s" : "")}" : "";
            Logger.Log($"{prompts.Length} prompt{(prompts.Length != 1 ? "s" : "")} with {iterations} iteration{(iterations != 1 ? "s" : "")} each and {scales.Length} scale{(scales.Length != 1 ? "s" : "")}{initsStr} each = {imgs} images total.");

            if (!IsAiProcessRunning || (IsAiProcessRunning && _lastInvokeStartupSettings != newStartupSettings))
            {
                _lastInvokeStartupSettings = newStartupSettings;

                if (!string.IsNullOrWhiteSpace(embedding))
                {
                    if (!File.Exists(embedding))
                        embedding = "";
                    else
                        Logger.Log($"Using learned concept: {Path.GetFileName(embedding)}");
                }

                Process py = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd());
                TextToImage.CurrentTask.Processes.Add(py);

                py.StartInfo.RedirectStandardInput = true;
                py.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSd()} && call activate.bat {Constants.Dirs.SdEnv} && " +
                    $"python {Constants.Dirs.RepoSd}/scripts/invoke.py --model default -o {outPath.Wrap(true)} {ArgsInvoke.GetDefaultArgsStartup()} {precArg} " +
                    $"{embArg} ";

                Logger.Log("cmd.exe " + py.StartInfo.Arguments, true);

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

                TtiProcessOutputHandler.Start();

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
                TextToImage.CurrentTask.Processes.Add(CurrentProcess);
            }

            Logger.Log($"Writing to stdin...", true);

            await WriteStdIn("!reset");

            foreach (string command in cmds)
                await WriteStdIn(command);

            Finish();
        }

        public enum FixAction { Upscale, FaceRestoration }

        public static void InvokeAiFix(string imgPath, List<FixAction> actions)
        {
            if (Program.Busy)
            {
                UiUtils.ShowMessageBox("Can't run post-processing while the program is still busy.");
                return;
            }

            if (CurrentStdInWriter == null)
            {
                UiUtils.ShowMessageBox("Can't run post-processing when Stable Diffusion is not loaded.");
                return;
            }

            Program.MainForm.SetWorking(Program.BusyState.PostProcessing, false);

            Logger.Log($"InvokeAI Fix: {string.Join(", ", actions.Select(x => x.ToString()))}", true);

            List<string> args = new List<string> { "!fix", imgPath.Wrap(true) };

            if (actions.Contains(FixAction.Upscale))
                args.Add(ArgsInvoke.GetUpscaleArgs(true));

            if (actions.Contains(FixAction.FaceRestoration))
                args.Add(ArgsInvoke.GetFaceRestoreArgs(true));

            WriteStdIn(string.Join(" ", args), true);
        }

        public static async Task RunStableDiffusionOpt(string[] prompts, int iterations, Dictionary<string, string> paramsDict, string outPath)
        {
            // NOTE: Currently not implemented: Embeddings, Samplers, Seamless Mode
            string[] initImgs = paramsDict.Get("initImgs").FromJson<string[]>();
            string embedding = paramsDict.Get("embedding").FromJson<string>();
            float[] initStrengths = paramsDict.Get("initStrengths").FromJson<float[]>();
            int steps = paramsDict.Get("steps").FromJson<int>();
            float[] scales = paramsDict.Get("scales").FromJson<float[]>();
            long seed = paramsDict.Get("seed").FromJson<long>();
            string sampler = paramsDict.Get("sampler").FromJson<string>();
            Size res = paramsDict.Get("res").FromJson<Size>();
            bool seamless = paramsDict.Get("seamless").FromJson<bool>();
            string model = paramsDict.Get("model").FromJson<string>();
            string modelNoExt = Path.ChangeExtension(model, null);
            bool lockSeed = paramsDict.Get("lockSeed").FromJson<bool>();

            if (!TtiUtils.CheckIfSdModelExists())
                return;

            Dictionary<string, string> initImages = initImgs != null && initImgs.Length > 0 ? await TtiUtils.CreateResizedInitImagesIfNeeded(initImgs.ToList(), res) : null;

            long startSeed = seed;

            string promptFilePath = Path.Combine(Paths.GetSessionDataPath(), "prompts.txt");
            List<string> promptFileLines = new List<string>();

            // int upscaleSetting = Config.GetInt("comboxUpscale");
            // string upscaling = upscaleSetting == 0 ? "" : $"-U {Math.Pow(2, upscaleSetting)}";
            // 
            // float gfpganSetting = Config.GetFloat("sliderGfpgan");
            // string gfpgan = gfpganSetting > 0.01f ? $"-G {gfpganSetting.ToStringDot("0.00")}" : "";

            int imgs = 0;

            foreach (string prompt in prompts)
            {
                for (int i = 0; i < iterations; i++)
                {
                    foreach (float scale in scales)
                    {
                        if (initImages == null) // No init image(s)
                        {
                            promptFileLines.Add($"--prompt {prompt.Wrap()} --ddim_steps {steps} --scale {scale.ToStringDot()} --W {res.Width} --H {res.Height} --seed {seed}");
                            imgs++;
                        }
                        else // With init image(s)
                        {
                            foreach (string initImg in initImages.Values)
                            {
                                foreach (float strength in initStrengths)
                                {
                                    string init = $"--init_img {initImg.Wrap()} --strength {strength.ToStringDot("0.###")}";
                                    promptFileLines.Add($"--prompt {prompt.Wrap()} {init} --ddim_steps {steps} --scale {scale.ToStringDot()} --W {res.Width} --H {res.Height} --seed {seed}");
                                    imgs++;
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

            IoUtils.TryDeleteIfExists(promptFilePath); // idk if this is needed, but the line below MIGHT append something so better make sure the previous prompts are deleted
            File.WriteAllLines(promptFilePath, promptFileLines);

            Logger.Log($"Running Stable Diffusion - {iterations} Iterations, {steps} Steps, Scales {(scales.Length < 4 ? string.Join(", ", scales.Select(x => x.ToStringDot())) : $"{scales.First()}->{scales.Last()}")}, {res.Width}x{res.Height}, Starting Seed: {startSeed}");

            string precArg = $"--precision {(Config.GetBool("checkboxFullPrecision") ? "full" : "autocast")}";

            string newStartupSettings = $"opt{modelNoExt}{precArg}{Config.GetInt("comboxCudaDevice")}"; // Check if startup settings match - If not, we need to restart the process

            string initsStr = initImages != null ? $" and {initImages.Count} image{(initImages.Count != 1 ? "s" : "")} using {initStrengths.Length} strength{(initStrengths.Length != 1 ? "s" : "")}" : "";
            Logger.Log($"{prompts.Length} prompt{(prompts.Length != 1 ? "s" : "")} with {iterations} iteration{(iterations != 1 ? "s" : "")} each and {scales.Length} scale{(scales.Length != 1 ? "s" : "")}{initsStr} each = {imgs} images total.");

            if (!IsAiProcessRunning || (IsAiProcessRunning && _lastInvokeStartupSettings != newStartupSettings))
            {
                _lastInvokeStartupSettings = newStartupSettings;

                Process py = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd());
                TextToImage.CurrentTask.Processes.Add(py);

                py.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSd()} && call activate.bat {Constants.Dirs.SdEnv} && " +
                    $"python {Constants.Dirs.RepoSd}/optimizedSD/optimized_txt2img_loop.py --model {modelNoExt.Wrap()} --outdir {outPath.Wrap(true)} --from_file_loop={promptFilePath.Wrap()} {precArg} ";
                Logger.Log("cmd.exe " + py.StartInfo.Arguments, true);

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

                TtiProcessOutputHandler.Start();
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

            Finish();
        }

        public static async Task RunStableDiffusionCli(string outPath, string vaePath)
        {
            if (Program.Busy)
                return;

            if (!TtiUtils.CheckIfSdModelExists())
                return;

            TtiUtils.WriteModelsYaml(Config.Get(Config.Key.comboxSdModel), vaePath);

            string batPath = Path.Combine(Paths.GetSessionDataPath(), "invoke.bat");

            string batText = $"@echo off\n" +
                $"title Stable Diffusion CLI (InvokeAI)\n" +
                $"cd /D {Paths.GetDataPath().Wrap()}\n" +
                $"SET PATH={OsUtils.GetTemporaryPathVariable(new string[] { $"./{Constants.Dirs.Conda}", $"./{Constants.Dirs.Conda}/Scripts", $"./{Constants.Dirs.Conda}/condabin", $"./{Constants.Dirs.Conda}/Library/bin" })}\n" +
                $"call activate.bat {Constants.Dirs.Conda}/envs/{Constants.Dirs.SdEnv}\n" +
                $"python {Constants.Dirs.RepoSd}/scripts/invoke.py --model default -o {outPath.Wrap(true)} {ArgsInvoke.GetPrecisionArg()} {ArgsInvoke.GetDefaultArgsStartup()}";

            File.WriteAllText(batPath, batText);
            ProcessManager.FindAndKillOrphans($"*invoke.py*{outPath}*");
            Process cli = Process.Start(batPath);
            OsUtils.AttachOrphanHitman(cli);
        }

        public static void StartCmdInSdCondaEnv()
        {
            string c = Constants.Dirs.Conda;
            string pathVar = OsUtils.GetTemporaryPathVariable(new string[] { $"./{c}", $"./{c}/Scripts", $"./{c}/condabin", $"./{c}/Library/bin" });
            Process.Start("cmd", $"/K title Environment: {Constants.Dirs.SdEnv} && cd /D {Paths.GetDataPath().Wrap()} && SET PATH={pathVar} && call activate.bat {c}/envs/{Constants.Dirs.SdEnv}");
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
                string log = "...\n" + string.Join("\n", Logger.GetSessionLogLastLines(Constants.Lognames.Sd, 8));
                TextToImage.Cancel($"Process has exited unexpectedly.\n\nOutput:\n{log}");
            }
        }
    }
}
