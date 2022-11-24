using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections;
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
    internal class OptimizedSd
    {
        public static async Task Run(string[] prompts, int iterations, Dictionary<string, string> parameters, string outPath)
        {
            // NOTE: Currently not implemented: Embeddings, Samplers, Seamless Mode, ...
            string[] initImgs = parameters.Get("initImgs").FromJson<string[]>();
            float[] initStrengths = parameters.Get("initStrengths").FromJson<float[]>().Select(n => 1f - n).ToArray();
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

            await OptimizedSdUtils.RunPickleScan(modelFile);
            if (TextToImage.Canceled) return;

            OrderedDictionary initImages = initImgs != null && initImgs.Length > 0 ? await TtiUtils.CreateResizedInitImagesIfNeeded(initImgs.ToList(), res) : null;

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

            Logger.Log($"Running Stable Diffusion - {iterations} Iterations, {steps.Length} Steps, Scales {(scales.Length < 4 ? string.Join(", ", scales.Select(x => x.ToStringDot())) : $"{scales.First()}->{scales.Last()}")}, {res.Width}x{res.Height}, Starting Seed: {startSeed}");

            string argsStartup = Args.OptimizedSd.GetDefaultArgsStartup();
            string newStartupSettings = $"opt {modelNoExt} {argsStartup} {Config.GetInt("comboxCudaDevice")}"; // Check if startup settings match - If not, we need to restart the process

            string initsStr = initImages != null ? $" and {initImages.Count} image{(initImages.Count != 1 ? "s" : "")} using {initStrengths.Length} strength{(initStrengths.Length != 1 ? "s" : "")}" : "";
            Logger.Log($"{prompts.Length} prompt{(prompts.Length != 1 ? "s" : "")} * {iterations} image{(iterations != 1 ? "s" : "")} * {steps.Length} step count{(steps.Length != 1 ? "s" : "")} * {scales.Length} scale{(scales.Length != 1 ? "s" : "")}{initsStr} = {promptFileLines.Count} images total.");

            if (!TtiProcess.IsAiProcessRunning || (TtiProcess.IsAiProcessRunning && TtiProcess.LastStartupSettings != newStartupSettings))
            {
                TtiProcess.LastStartupSettings = newStartupSettings;

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

                if (TtiProcess.CurrentProcess != null)
                {
                    TtiProcess.ProcessExistWasIntentional = true;
                    OsUtils.KillProcessTree(TtiProcess.CurrentProcess.Id);
                }

                TtiProcessOutputHandler.Reset();
                Logger.Log($"Loading Stable Diffusion with model {modelNoExt.Wrap()}...");
                TtiProcess.CurrentProcess = py;
                TtiProcess.ProcessExistWasIntentional = false;
                py.Start();
                OsUtils.AttachOrphanHitman(py);

                if (!OsUtils.ShowHiddenCmd())
                {
                    py.BeginOutputReadLine();
                    py.BeginErrorReadLine();
                }

                Task.Run(() => TtiProcess.CheckStillRunning());
            }
            else
            {
                TextToImage.CurrentTask.Processes.Add(TtiProcess.CurrentProcess);
            }
        }

    }
}
