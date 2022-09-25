using StableDiffusionGui.Forms;
using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace StableDiffusionGui.Main
{
    internal class TtiProcess
    {
        public static Process DreamPyParentProcess;
        public static StreamWriter CurrentStdInWriter;
        public static bool IsDreamPyRunning { get { return DreamPyParentProcess != null && !DreamPyParentProcess.HasExited; } }

        public static void Finish()
        {
            return;
        }

        private static string _lastDreamPyStartupSettings;

        public static async Task RunStableDiffusion(string[] prompts, string initImg, string embedding, float[] initStrengths, int iterations, int steps, float[] scales, long seed, string sampler, Size res, bool seamless, string outPath)
        {
            if (!TtiUtils.CheckIfSdModelExists())
                return;

            if (File.Exists(initImg))
                initImg = TtiUtils.ResizeInitImg(initImg, res);

            TtiUtils.WriteModelsYaml(TtiUtils.GetSdModel());

            long startSeed = seed;

            List<string> commands = new List<string>();

            int upscaleSetting = Config.GetInt("comboxUpscale");
            string upscaling = upscaleSetting == 0 ? "" : $"-U {Math.Pow(2, upscaleSetting)}";

            string faceRestore = ArgsDreamPy.GetFaceRestoreArgs();

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
                            commands.Add($"{prompt} {init} -n {1} -s {steps} -C {scale.ToStringDot()} -A {sampler} -W {res.Width} -H {res.Height} -S {seed} {upscaling} {faceRestore} {(seamless ? "--seamless" : "")}");
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

            Logger.Log($"Running Stable Diffusion - {iterations} Iterations, {steps} Steps, Scales {(scales.Length < 4 ? string.Join(", ", scales.Select(x => x.ToStringDot())) : $"{scales.First()}->{scales.Last()}")}, {res.Width}x{res.Height}, Starting Seed: {startSeed}");

            string mdlArg = TtiUtils.GetSdModel();
            string precArg = ArgsDreamPy.GetPrecisionArg();
            string embArg = ArgsDreamPy.GetEmbeddingArg(embedding);

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

                dream.StartInfo.RedirectStandardInput = true;
                dream.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetPathVariableCmd()} && call activate.bat ldo && " +
                    $"python repo/scripts/dream.py --model {TtiUtils.GetSdModel()} -o {outPath.Wrap()} {ArgsDreamPy.GetDefaultArgs()} {precArg} " +
                    $"{embArg} ";

                Logger.Log("cmd.exe " + dream.StartInfo.Arguments, true);

                if (!OsUtils.ShowHiddenCmd())
                {
                    dream.OutputDataReceived += (sender, line) => { TtiProcessOutputHandler.LogOutput(line.Data); };
                    dream.ErrorDataReceived += (sender, line) => { TtiProcessOutputHandler.LogOutput(line.Data, true); };
                }

                ProcessManager.FindAndKillOrphans("*repo*.py*");
                TtiProcessOutputHandler.Start();
                Logger.Log($"Loading Stable Diffusion with model {TtiUtils.GetSdModel().Wrap()}...");
                DreamPyParentProcess = dream;
                dream.Start();
                CurrentStdInWriter = dream.StandardInput;

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
                await WriteStdIn("!reset");
            }

            Logger.Log($"Writing to stdin...\n{string.Join("\n", commands)}", true);

            foreach(string command in commands)
                await WriteStdIn(command);

            Finish();
        }

        public static async Task RunStableDiffusionOpt(string[] prompts, string initImg, string embedding, float[] initStrengths, int iterations, int steps, float[] scales, long seed, string sampler, Size res, bool seamless, string outPath)
        {
            // NOTE: Currently not implemented: Embeddings, Samplers, Seamless Mode

            if (!TtiUtils.CheckIfSdModelExists())
                return;

            if (File.Exists(initImg))
                initImg = TtiUtils.ResizeInitImg(initImg, res);

            long startSeed = seed;

            string promptFilePath = Path.Combine(Paths.GetSessionDataPath(), "prompts-opt.txt");
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

            string mdlArg = TtiUtils.GetSdModel();
            string precArg = $"--precision {(Config.GetBool("checkboxFullPrecision") ? "full" : "autocast")}";

            string newStartupSettings = $"opt{mdlArg}{precArg}"; // Check if startup settings match - If not, we need to restart the process

            string strengths = File.Exists(initImg) ? $" and {initStrengths.Length} strength{(initStrengths.Length != 1 ? "s" : "")}" : "";
            Logger.Log($"{prompts.Length} prompt{(prompts.Length != 1 ? "s" : "")} with {iterations} iteration{(iterations != 1 ? "s" : "")} each and {scales.Length} scale{(scales.Length != 1 ? "s" : "")}{strengths} each = {imgs} images total.");

            if (!IsDreamPyRunning || (IsDreamPyRunning && _lastDreamPyStartupSettings != newStartupSettings))
            {
                _lastDreamPyStartupSettings = newStartupSettings;

                Process dream = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd());
                TextToImage.CurrentTask.Processes.Add(dream);

                dream.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetPathVariableCmd()} && call activate.bat ldo && " +
                    $"python repo/optimizedSD/optimized_txt2img_loop.py --model {mdlArg} --outdir {outPath.Wrap()} --from_file_loop={promptFilePath.Wrap()} {precArg} ";
                Logger.Log("cmd.exe " + dream.StartInfo.Arguments, true);

                if (!OsUtils.ShowHiddenCmd())
                {
                    dream.OutputDataReceived += (sender, line) => { TtiProcessOutputHandler.LogOutput(line.Data); };
                    dream.ErrorDataReceived += (sender, line) => { TtiProcessOutputHandler.LogOutput(line.Data, true); };
                }

                ProcessManager.FindAndKillOrphans("*repo*.py*");
                TtiProcessOutputHandler.Start();
                Logger.Log($"Loading Stable Diffusion with model {TtiUtils.GetSdModel().Wrap()}...");
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

            if (!TtiUtils.CheckIfSdModelExists())
                return;

            TtiUtils.WriteModelsYaml(TtiUtils.GetSdModel());

            string batPath = Path.Combine(Paths.GetSessionDataPath(), "dream.bat");

            string batText = $"@echo off\n" +
                $"title Dream.py CLI\n" +
                $"cd /D {Paths.GetDataPath().Wrap()}\n" +
                $"SET PATH={OsUtils.GetTemporaryPathVariable(new string[] { "./mb", "./mb/Scripts", "./mb/condabin", "./mb/Library/bin" })}\n" +
                $"call activate.bat mb/envs/ldo\n" +
                $"python repo/scripts/dream.py --model {TtiUtils.GetSdModel()} -o {outPath.Wrap()} {ArgsDreamPy.GetPrecisionArg()} {ArgsDreamPy.GetDefaultArgs()}";

            File.WriteAllText(batPath, batText);
            ProcessManager.FindAndKillOrphans("*repo*.py*");
            Process.Start(batPath);
        }

        public static async Task<bool> WriteStdIn(string text, bool submitLine = true)
        {
            try
            {
                if (CurrentStdInWriter == null)
                    return false;

                if (submitLine)
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
