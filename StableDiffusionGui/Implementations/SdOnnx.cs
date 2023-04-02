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
using System.Text;
using System.Threading.Tasks;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Implementations
{
    internal class SdOnnx
    {
        public static async Task Run(string[] prompts, string promptNeg, int iterations, Dictionary<string, string> parameters, string outPath)
        {
            try
            {
                string[] initImgs = parameters.FromJson<string[]>("initImgs");
                float[] initStrengths = parameters.FromJson<float[]>("initStrengths").Select(n => 1f - n).ToArray();
                int[] steps = parameters.FromJson<int[]>("steps");
                float[] scales = parameters.FromJson<float[]>("scales");
                long seed = parameters.FromJson<long>("seed");
                string sampler = parameters.FromJson<string>("sampler");
                Size res = parameters.FromJson<Size>("res");
                var seamless = parameters.FromJson<SeamlessMode>("seamless");
                string model = parameters.FromJson<string>("model");
                bool lockSeed = parameters.FromJson<bool>("lockSeed");
                InpaintMode inpaint = parameters.FromJson<InpaintMode>("inpainting");

                var cachedModels = Models.GetModels(Enums.Models.Type.Normal, Implementation.DiffusersOnnx);
                Model modelDir = TtiUtils.CheckIfCurrentSdModelExists();

                if (modelDir == null)
                    return;

                OrderedDictionary initImages = initImgs != null && initImgs.Length > 0 ? await TtiUtils.CreateResizedInitImagesIfNeeded(initImgs.ToList(), res) : null;

                long startSeed = seed;

                List<Dictionary<string, string>> argLists = new List<Dictionary<string, string>>(); // List of all args for each command
                Dictionary<string, string> args = new Dictionary<string, string>(); // List of args for current command
                args["prompt"] = "";
                args["default"] = "";

                foreach (string prompt in prompts)
                {
                    List<string> processedPrompts = PromptWildcardUtils.ApplyWildcardsAll(prompt, iterations, false);
                    TextToImage.CurrentTaskSettings.ProcessedAndRawPrompts = new EasyDict<string, string>(processedPrompts.Distinct().ToDictionary(x => x, x => prompt));

                    for (int i = 0; i < iterations; i++)
                    {
                        args["initImg"] = "";
                        args["initStrength"] = "0";
                        args["inpaintMask"] = "";
                        args["prompt"] = processedPrompts[i];
                        args["promptNeg"] = promptNeg;
                        args["w"] = $"{res.Width}";
                        args["h"] = $"{res.Height}";
                        args["seed"] = $"{seed}";

                        foreach (float scale in scales)
                        {
                            args["scale"] = $"{scale.ToStringDot()}";

                            foreach (int stepCount in steps)
                            {
                                args["steps"] = $"{stepCount}";

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
                                            args["initImg"] = initImg;
                                            args["initStrength"] = strength.ToStringDot("0.###");

                                            if (inpaint == InpaintMode.ImageMask)
                                                args["inpaintMask"] = Inpainting.MaskImagePathDiffusers;

                                            argLists.Add(new Dictionary<string, string>(args));
                                        }
                                    }
                                }
                            }
                        }

                        if (!lockSeed)
                            seed++;
                    }

                    if (Config.Instance.MultiPromptsSameSeed)
                        seed = startSeed;
                }

                Logger.Log($"Running Stable Diffusion - {iterations} Iterations, {steps.Length} Steps, Scales {(scales.Length < 4 ? string.Join(", ", scales.Select(x => x.ToStringDot())) : $"{scales.First()}->{scales.Last()}")}, {res.Width}x{res.Height}, Starting Seed: {startSeed}");

                string initsStr = initImages != null ? $" and {initImages.Count} image{(initImages.Count != 1 ? "s" : "")} using {initStrengths.Length} strength{(initStrengths.Length != 1 ? "s" : "")}" : "";
                Logger.Log($"{prompts.Length} prompt{(prompts.Length != 1 ? "s" : "")} * {iterations} image{(iterations != 1 ? "s" : "")} * {steps.Length} step value{(steps.Length != 1 ? "s" : "")} * {scales.Length} scale{(scales.Length != 1 ? "s" : "")}{initsStr} = {argLists.Count} images total.");

                if (!TtiProcess.IsAiProcessRunning)
                {
                    PatchDiffusersIfNeeded();

                    Process py = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd());
                    TextToImage.CurrentTask.Processes.Add(py);

                    string mode = "txt2img";
                    bool inpaintingMdl = modelDir.Name.EndsWith(Constants.SuffixesPrefixes.InpaintingMdlSuf);

                    if (initImgs != null && initImgs.Length > 0)
                    {
                        mode = "img2img";

                        if (inpaintingMdl && inpaint != InpaintMode.Disabled)
                            mode = "inpaint";
                    }

                    py.StartInfo.RedirectStandardInput = true;
                    py.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSdCommand()} && " +
                        $"python \"{Constants.Dirs.SdRepo}/nmkdiff/nmkdiffusers.py\" -p SdOnnx -g {mode} -m {modelDir.FullName.Wrap(true)} -o {outPath.Wrap(true)}";

                    Logger.Log("cmd.exe " + py.StartInfo.Arguments, true);

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

                    Logger.Log($"Loading Stable Diffusion (ONNX) with model {model.Trunc(80).Wrap()}...");

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
                    TtiProcess.CurrentStdInWriter = new NmkdStreamWriter(py);
                }
                else
                {
                    TtiProcessOutputHandler.Reset();
                    TextToImage.CurrentTask.Processes.Add(TtiProcess.CurrentProcess);
                }

                foreach (var argList in argLists)
                    await TtiProcess.WriteStdIn($"generate {argList.ToJson()}", 200, true);
            }
            catch (Exception ex)
            {
                Logger.Log($"Unhandled Stable Diffusion Error: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
            }
        }

        private static void PatchDiffusersIfNeeded()
        {
            return;

            string marker = "# PATCHED BY NMKD SD GUI";

            string diffusersPath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdVenv, "Lib", "site-packages", "diffusers");

            string pipelinePath = Path.Combine(diffusersPath, "pipelines", "stable_diffusion", "pipeline_onnx_stable_diffusion.py");
            string text = File.ReadAllText(pipelinePath);

            if (text.SplitIntoLines()[0].Trim() != marker)
            {
                text = text.Replace("    safety_checker: OnnxRuntimeModel", "    # safety_checker: OnnxRuntimeModel");
                text = text.Replace("    safety_checker=safety_checker", "    # safety_checker=safety_checker");
                text = text.Replace("    feature_extractor: CLIPFeatureExtractor", "    # feature_extractor: CLIPFeatureExtractor");
                text = text.Replace("    feature_extractor=feature_extractor", "    # feature_extractor=feature_extractor");
                text = text.Replace("    if self.safety_checker is not None:", "    if False:");
                text = text.Replace("    if safety_checker is None and requires_safety_checker:", "    if False:");
                text = text.Replace("    if safety_checker is not None and feature_extractor is None:", "    if False:");
                File.WriteAllText(pipelinePath, $"{marker}{Environment.NewLine}{text}");
                Logger.Log($"Patched {Path.GetFileName(pipelinePath)}", true);
            }

            pipelinePath = Path.Combine(diffusersPath, "pipelines", "stable_diffusion", "pipeline_onnx_stable_diffusion_img2img.py");
            text = File.ReadAllText(pipelinePath);

            if (text.SplitIntoLines()[0].Trim() != marker)
            {
                text = text.Replace("    safety_checker: OnnxRuntimeModel", "    # safety_checker: OnnxRuntimeModel");
                text = text.Replace("    safety_checker=safety_checker", "    # safety_checker=safety_checker");
                text = text.Replace("    if self.safety_checker is not None", "    if False");
                text = text.Replace("    if safety_checker is None", "    if False");
                text = text.Replace("    feature_extractor: CLIPFeatureExtractor", "    # feature_extractor: CLIPFeatureExtractor");
                text = text.Replace("    feature_extractor=feature_extractor,", "    # feature_extractor=feature_extractor,");
                File.WriteAllText(pipelinePath, $"{marker}{Environment.NewLine}{text}");
                Logger.Log($"Patched {Path.GetFileName(pipelinePath)}", true);
            }

            pipelinePath = Path.Combine(diffusersPath, "pipelines", "stable_diffusion", "pipeline_stable_diffusion.py");
            text = File.ReadAllText(pipelinePath);

            if (text.SplitIntoLines()[0].Trim() != marker)
            {
                text = text.Replace("    safety_checker: StableDiffusionSafetyChecker", "    # safety_checker: StableDiffusionSafetyChecker");
                text = text.Replace("    feature_extractor: CLIPFeatureExtractor,", "    # feature_extractor: CLIPFeatureExtractor,");
                text = text.Replace("    feature_extractor=feature_extractor", "    # feature_extractor=feature_extractor");
                text = text.Replace("    safety_checker=safety_checker", "    # safety_checker=safety_checker");
                text = text.Replace("    if self.safety_checker is not None", "    if False");
                text = text.Replace("    if safety_checker is None", "    if False");
                File.WriteAllText(pipelinePath, $"{marker}{Environment.NewLine}{text}");
                Logger.Log($"Patched {Path.GetFileName(pipelinePath)}", true);
            }
        }
    }
}
