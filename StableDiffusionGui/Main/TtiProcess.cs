using StableDiffusionGui.Forms;
using StableDiffusionGui.Io;
using StableDiffusionGui.Os;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StableDiffusionGui.Main
{
    internal class TtiProcess
    {
        public static Process DreamPyParentProcess;
        public static bool IsDreamPyRunning { get { return DreamPyParentProcess != null && !DreamPyParentProcess.HasExited; } }

        public static void Finish()
        {
            return;
        }

        private static string _lastDreamPyStartupSettings;

        public static async Task RunStableDiffusion(string[] prompts, string initImg, string embedding, float[] initStrengths, int iterations, int steps, float[] scales, long seed, string sampler, Size res, bool seamless, string outPath)
        {
            if (!CheckIfSdModelExists())
                return;

            if (File.Exists(initImg))
                initImg = TtiUtils.ResizeInitImg(initImg, res);

            TtiUtils.WriteModelsYaml(GetSdModel());

            long startSeed = seed;

            string promptFilePath = Path.Combine(Paths.GetSessionDataPath(), "prompts.txt");
            List<string> promptFileLines = new List<string>();

            int upscaleSetting = Config.GetInt("comboxUpscale");
            string upscaling = upscaleSetting == 0 ? "" : $"-U {Math.Pow(2, upscaleSetting)}";

            float gfpganSetting = Config.GetFloat("sliderGfpgan");
            string gfpgan = gfpganSetting > 0.01f ? $"-G {gfpganSetting.ToStringDot("0.00")}" : "";

            int imgs = 0;

            foreach (string prompt in prompts)
            {
                for (int i = 0; i < iterations; i++)
                {
                    foreach (float scale in scales)
                    {
                        foreach (float strength in initStrengths)
                        {
                            bool initImgExists = File.Exists(initImg);
                            string init = initImgExists ? $"--init_img {initImg.Wrap()} --strength {strength.ToStringDot("0.0000")}" : "";
                            promptFileLines.Add($"{prompt} {init} -n {1} -s {steps} -C {scale.ToStringDot()} -A {sampler} -W {res.Width} -H {res.Height} -S {seed} {upscaling} {gfpgan} {(seamless ? "--seamless" : "")}");
                            imgs++;

                            if (!initImgExists)
                                break;
                        }
                    }

                    seed++;
                }

                if (Config.GetBool(Config.Key.checkboxMultiPromptsSameSeed))
                    seed = startSeed;
            }

            File.WriteAllLines(promptFilePath, promptFileLines);

            Logger.Log($"Running Stable Diffusion - {iterations} Iterations, {steps} Steps, Scales {(scales.Length < 4 ? string.Join(", ", scales.Select(x => x.ToStringDot())) : $"{scales.First()}->{scales.Last()}")}, {res.Width}x{res.Height}, Starting Seed: {startSeed}");

            string mdlArg = GetSdModel();
            string precArg = $"{(Config.GetBool("checkboxFullPrecision") ? "-F" : "")}";
            string embArg = !string.IsNullOrWhiteSpace(embedding) ? $"--embedding_path {embedding.Wrap()}" : "";

            string newStartupSettings = $"{mdlArg}{precArg}{embArg}"; // Check if startup settings match - If not, we need to restart the process

            string strengths = File.Exists(initImg) ? $" and {initStrengths.Length} strength{(initStrengths.Length != 1 ? "s" : "")}" : "";
            Logger.Log($"{prompts.Length} prompt{(prompts.Length != 1 ? "s" : "")} with {iterations} iteration{(iterations != 1 ? "s" : "")} each and {scales.Length} scale{(scales.Length != 1 ? "s" : "")}{strengths} each = {imgs} images total.");

            if (!IsDreamPyRunning || (IsDreamPyRunning && _lastDreamPyStartupSettings != newStartupSettings))
            {
                _lastDreamPyStartupSettings = newStartupSettings;

                if (!string.IsNullOrWhiteSpace(embedding))
                {
                    if (!File.Exists(embedding))
                        embedding = "";
                    else
                        Logger.Log($"Using learned concept: {Path.GetFileName(embedding)}");
                }

                Process dream = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd());
                TextToImage.CurrentTask.Processes.Add(dream);

                dream.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && call \"{Paths.GetDataPath()}\\mb\\Scripts\\activate.bat\" ldo && " +
                    $"python \"{Paths.GetDataPath()}/repo/scripts/dream.py\" --model {GetSdModel()} -o {outPath.Wrap()} --from_file_loop={promptFilePath.Wrap()} {precArg} " +
                    $"{embArg} --print_steps ";

                Logger.Log("cmd.exe " + dream.StartInfo.Arguments, true);

                if (!OsUtils.ShowHiddenCmd())
                {
                    dream.OutputDataReceived += (sender, line) => { TtiProcessOutputHandler.LogOutput(line.Data); };
                    dream.ErrorDataReceived += (sender, line) => { TtiProcessOutputHandler.LogOutput(line.Data, true); };
                }

                ProcessManager.FindAndKillOrphans("dream.py");
                TtiProcessOutputHandler.Start();
                Logger.Log("Loading Stable Diffusion...");
                DreamPyParentProcess = dream;
                dream.Start();

                if (!OsUtils.ShowHiddenCmd())
                {
                    dream.BeginOutputReadLine();
                    dream.BeginErrorReadLine();
                }

                //while (!dream.HasExited) await Task.Delay(1); // We don't wait for it to quit since it keeps running in background.
            }
            else
            {
                TextToImage.CurrentTask.Processes.Add(DreamPyParentProcess);
            }

            Finish();
        }

        public static async Task RunStableDiffusionOptimized(string[] prompts, string initImg, float initStrength, int iterations, int steps, float scale, long seed, string sampler, Size res, string outPath)
        {
            if (!CheckIfSdModelExists())
                return;

            if (File.Exists(initImg))
                initImg = TtiUtils.ResizeInitImg(initImg, res);

            string promptFilePath = Path.Combine(Paths.GetSessionDataPath(), "prompts.txt");
            File.WriteAllLines(promptFilePath, prompts);

            Logger.Log($"Preparing to run Optimized Stable Diffusion - {iterations} Iterations, {steps} Steps, Scale {scale}, {res.Width}x{res.Height}, Starting Seed: {seed}");

            Logger.Log($"{prompts.Length} prompt{(prompts.Length != 1 ? "s" : "")} with {iterations} iteration{(iterations != 1 ? "s" : "")} each = {iterations * prompts.Length} images total.");

            Process dream = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd());
            TextToImage.CurrentTask.Processes.Add(dream);

            string prec = $"{(Config.GetBool("checkboxFullPrecision") ? "full" : "autocast")}";
            sampler = "ddim"; // OVERRIDE FOR NOW! //if (sampler.StartsWith("k_")) sampler = sampler.Remove(0, 2); // This script does not use the k_ prefix for k-diffusion samplers, so we remove it

            bool initImgExists = File.Exists(initImg);

            dream.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && call \"{Paths.GetDataPath()}\\mb\\Scripts\\activate.bat\" ldo && " +
                $"python \"{Paths.GetDataPath()}/repo/optimizedSD/optimized_{(initImgExists ? "img" : "txt")}2img.py\" --model {GetSdModel()} --outdir {outPath.Wrap()} --from-file {promptFilePath.Wrap()} " +
                $"--n_iter {iterations} --ddim_steps {steps} --W {res.Width} --H {res.Height} --scale {scale.ToStringDot("0.0000")} --seed {seed} --sampler {sampler} --precision {prec} " +
                $"{(initImgExists ? $"--init-img {initImg.Wrap()} --strength {initStrength.ToStringDot("0.0000")}" : "")} {(Config.GetBool(Config.Key.lowMemTurbo) ? "--turbo" : "")} ";

            Logger.Log("cmd.exe " + dream.StartInfo.Arguments, true);

            if (!OsUtils.ShowHiddenCmd())
            {
                dream.OutputDataReceived += (sender, line) => { TtiProcessOutputHandler.LogOutput(line.Data); };
                dream.ErrorDataReceived += (sender, line) => { TtiProcessOutputHandler.LogOutput(line.Data, true); };
            }

            ProcessManager.FindAndKillOrphans("dream.py");
            TtiProcessOutputHandler.Start();
            Logger.Log("Loading Stable Diffusion...");
            dream.Start();

            if (!OsUtils.ShowHiddenCmd())
            {
                dream.BeginOutputReadLine();
                dream.BeginErrorReadLine();
            }

            while (!dream.HasExited) await Task.Delay(1);

            IoUtils.TryDeleteIfExists(Path.Combine(Paths.GetSessionDataPath(), "prompts.txt"));

            Finish();
        }

        public static async Task RunStableDiffusionOpt(string[] prompts, string initImg, string embedding, float[] initStrengths, int iterations, int steps, float[] scales, long seed, string sampler, Size res, bool seamless, string outPath)
        {
            // NOTE: Currently not implemented: Embeddings, Samplers, Seamless Mode

            if (!CheckIfSdModelExists())
                return;

            if (File.Exists(initImg))
                initImg = TtiUtils.ResizeInitImg(initImg, res);

            long startSeed = seed;

            string promptFilePath = Path.Combine(Paths.GetSessionDataPath(), "prompts.txt");
            List<string> promptFileLines = new List<string>();

            int upscaleSetting = Config.GetInt("comboxUpscale");
            string upscaling = upscaleSetting == 0 ? "" : $"-U {Math.Pow(2, upscaleSetting)}";

            float gfpganSetting = Config.GetFloat("sliderGfpgan");
            string gfpgan = gfpganSetting > 0.01f ? $"-G {gfpganSetting.ToStringDot("0.00")}" : "";

            int imgs = 0;

            foreach (string prompt in prompts)
            {
                for (int i = 0; i < iterations; i++)
                {
                    foreach (float scale in scales)
                    {
                        foreach (float strength in initStrengths)
                        {
                            bool initImgExists = File.Exists(initImg);
                            string init = initImgExists ? $"--init_img {initImg.Wrap()} --strength {strength.ToStringDot("0.0000")}" : "";
                            promptFileLines.Add($"--prompt {prompt.Wrap()} {init} --ddim_steps {steps} --scale {scale.ToStringDot()} --W {res.Width} --H {res.Height} --seed {seed} {upscaling} {gfpgan}");
                            imgs++;

                            if (!initImgExists)
                                break;
                        }
                    }

                    seed++;
                }

                if (Config.GetBool(Config.Key.checkboxMultiPromptsSameSeed))
                    seed = startSeed;
            }

            File.WriteAllLines(promptFilePath, promptFileLines);

            Logger.Log($"Running Stable Diffusion - {iterations} Iterations, {steps} Steps, Scales {(scales.Length < 4 ? string.Join(", ", scales.Select(x => x.ToStringDot())) : $"{scales.First()}->{scales.Last()}")}, {res.Width}x{res.Height}, Starting Seed: {startSeed}");

            string mdlArg = GetSdModel();
            string precArg = $"--precision {(Config.GetBool("checkboxFullPrecision") ? "full" : "autocast")}";

            string newStartupSettings = $"opt{mdlArg}{precArg}"; // Check if startup settings match - If not, we need to restart the process

            string strengths = File.Exists(initImg) ? $" and {initStrengths.Length} strength{(initStrengths.Length != 1 ? "s" : "")}" : "";
            Logger.Log($"{prompts.Length} prompt{(prompts.Length != 1 ? "s" : "")} with {iterations} iteration{(iterations != 1 ? "s" : "")} each and {scales.Length} scale{(scales.Length != 1 ? "s" : "")}{strengths} each = {imgs} images total.");

            if (!IsDreamPyRunning || (IsDreamPyRunning && _lastDreamPyStartupSettings != newStartupSettings))
            {
                _lastDreamPyStartupSettings = newStartupSettings;

                Process dream = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd());
                TextToImage.CurrentTask.Processes.Add(dream);

                dream.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && call \"{Paths.GetDataPath()}\\mb\\Scripts\\activate.bat\" ldo && " +
                    $"python \"{Paths.GetDataPath()}/repo/optimizedSD/optimized_txt2img_loop.py\" --model {mdlArg} --outdir {outPath.Wrap()} --from_file_loop={promptFilePath.Wrap()} {precArg} ";
                Logger.Log("cmd.exe " + dream.StartInfo.Arguments, true);

                if (!OsUtils.ShowHiddenCmd())
                {
                    dream.OutputDataReceived += (sender, line) => { TtiProcessOutputHandler.LogOutput(line.Data); };
                    dream.ErrorDataReceived += (sender, line) => { TtiProcessOutputHandler.LogOutput(line.Data, true); };
                }

                ProcessManager.FindAndKillOrphans("optimized_txt2img_loop.py");
                TtiProcessOutputHandler.Start();
                Logger.Log("Loading Stable Diffusion...");
                DreamPyParentProcess = dream;
                dream.Start();

                if (!OsUtils.ShowHiddenCmd())
                {
                    dream.BeginOutputReadLine();
                    dream.BeginErrorReadLine();
                }

                //while (!dream.HasExited) await Task.Delay(1); // We don't wait for it to quit since it keeps running in background.
            }
            else
            {
                TextToImage.CurrentTask.Processes.Add(DreamPyParentProcess);
            }

            Finish();
        }

        public static async Task RunStableDiffusionCli(string outPath)
        {
            if (Program.Busy)
                return;

            if (!CheckIfSdModelExists())
                return;

            TtiUtils.WriteModelsYaml(GetSdModel());

            string batPath = Path.Combine(Paths.GetSessionDataPath(), "dream.bat");

            string batText = $"@echo off\n title Dream.py CLI && cd /D {Paths.GetDataPath().Wrap()} && call \"mb\\Scripts\\activate.bat\" \"mb/envs/ldo\" && " +
                $"python \"repo/scripts/dream.py\" --model {GetSdModel()} -o {outPath.Wrap()} {(Config.GetBool("checkboxFullPrecision") ? "--full_precision" : "")} ";

            File.WriteAllText(batPath, batText);
            ProcessManager.FindAndKillOrphans("dream.py");
            Process.Start(batPath);
        }

        private static bool CheckIfSdModelExists()
        {
            if (!File.Exists(Path.Combine(Paths.GetModelsPath(), GetSdModel(true))))
            {
                string savedModelFileName = Config.Get(Config.Key.comboxSdModel);

                if (string.IsNullOrWhiteSpace(savedModelFileName))
                {
                    TextToImage.Cancel($"No Stable Diffusion model file has been set.\nPlease set one in the settings.");
                    new SettingsForm().ShowDialog();
                }
                else
                {
                    TextToImage.Cancel($"Stable Diffusion model file {savedModelFileName.Wrap()} not found.\nPossibly it was moved, renamed, or deleted.");
                }

                return false;
            }

            return true;
        }

        private static string GetSdModel(bool withExtension = false)
        {
            string filename = Config.Get(Config.Key.comboxSdModel);
            return withExtension ? filename : Path.GetFileNameWithoutExtension(filename);
        }

        public static void Kill()
        {
            if (TextToImage.CurrentTask != null)
            {
                foreach (var process in TextToImage.CurrentTask.Processes)
                {
                    try
                    {
                        if (process != null && !process.HasExited)
                            OsUtils.KillProcessTree(process.Id);
                    }
                    catch (Exception e)
                    {
                        Logger.Log($"Failed to kill process tree: {e.Message}", true);
                    }
                }
            }
        }
    }
}
