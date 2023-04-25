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
        public static async Task Run(TtiSettings s, string outPath)
        {
            try
            {
                // string[] initImgs = parameters.FromJson<string[]>("initImgs");
                // float[] initStrengths = parameters.FromJson<float[]>("initStrengths").Select(n => 1f - n).ToArray();
                // int[] steps = parameters.FromJson<int[]>("steps");
                // float[] scales = parameters.FromJson<float[]>("scales");
                // long seed = parameters.FromJson<long>("seed");
                // string sampler = parameters.FromJson<string>("sampler");
                // Size res = parameters.FromJson<Size>("res");
                // var seamless = parameters.FromJson<SeamlessMode>("seamless");
                // string model = parameters.FromJson<string>("model");
                // bool lockSeed = parameters.FromJson<bool>("lockSeed");
                // ImgMode inpaint = parameters.FromJson<ImgMode>("inpainting");


                float[] initStrengths = s.InitStrengths.Select(n => 1f - n).ToArray();

                var cachedModels = Models.GetModels(Enums.Models.Type.Normal, Implementation.DiffusersOnnx);
                Model modelDir = TtiUtils.CheckIfCurrentSdModelExists();

                if (modelDir == null)
                    return;

                OrderedDictionary initImages = s.InitImgs != null && s.InitImgs.Length > 0 ? await TtiUtils.CreateResizedInitImagesIfNeeded(s.InitImgs.ToList(), s.Res) : null;

                long startSeed = s.Seed;

                List<Dictionary<string, string>> argLists = new List<Dictionary<string, string>>(); // List of all args for each command
                Dictionary<string, string> args = new Dictionary<string, string>(); // List of args for current command
                args["prompt"] = "";
                args["default"] = "";

                foreach (string prompt in s.Prompts)
                {
                    List<string> processedPrompts = PromptWildcardUtils.ApplyWildcardsAll(prompt, s.Iterations, false);
                    TextToImage.CurrentTaskSettings.ProcessedAndRawPrompts = new EasyDict<string, string>(processedPrompts.Distinct().ToDictionary(x => x, x => prompt));

                    for (int i = 0; i < s.Iterations; i++)
                    {
                        args["initImg"] = "";
                        args["initStrength"] = "0";
                        args["inpaintMask"] = "";
                        args["prompt"] = processedPrompts[i];
                        args["promptNeg"] = s.NegativePrompt;
                        args["w"] = $"{s.Res.Width}";
                        args["h"] = $"{s.Res.Height}";
                        args["seed"] = $"{s.Seed}";

                        foreach (float scale in s.ScalesTxt)
                        {
                            args["scale"] = $"{scale.ToStringDot()}";

                            foreach (int stepCount in s.Steps)
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

                                            if (s.ImgMode == ImgMode.ImageMask)
                                                args["inpaintMask"] = Inpainting.MaskImagePathDiffusers;

                                            argLists.Add(new Dictionary<string, string>(args));
                                        }
                                    }
                                }
                            }
                        }

                        if (!s.LockSeed)
                            s.Seed++;
                    }

                    if (Config.Instance.MultiPromptsSameSeed)
                        s.Seed = startSeed;
                }

                Logger.Log($"Running Stable Diffusion - {s.Iterations} Iterations, {s.Steps.Length} Steps, Scales {(s.ScalesTxt.Length < 4 ? string.Join(", ", s.ScalesTxt.Select(x => x.ToStringDot())) : $"{s.ScalesTxt.First()}->{s.ScalesTxt.Last()}")}, {s.Res.Width}x{s.Res.Height}, Starting Seed: {startSeed}");

                string initsStr = initImages != null ? $" and {initImages.Count} image{(initImages.Count != 1 ? "s" : "")} using {initStrengths.Length} strength{(initStrengths.Length != 1 ? "s" : "")}" : "";
                Logger.Log($"{s.Prompts.Length} prompt{(s.Prompts.Length != 1 ? "s" : "")} * {s.Iterations} image{(s.Iterations != 1 ? "s" : "")} * {s.Steps.Length} step value{(s.Steps.Length != 1 ? "s" : "")} * {s.ScalesTxt.Length} scale{(s.ScalesTxt.Length != 1 ? "s" : "")}{initsStr} = {argLists.Count} images total.");

                if (!TtiProcess.IsAiProcessRunning)
                {
                    Process py = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd());
                    TextToImage.CurrentTask.Processes.Add(py);

                    string mode = "txt2img";
                    bool inpaintingMdl = modelDir.Name.EndsWith(Constants.SuffixesPrefixes.InpaintingMdlSuf);

                    if (s.InitImgs != null && s.InitImgs.Length > 0)
                    {
                        mode = "img2img";

                        if (inpaintingMdl && s.ImgMode != ImgMode.InitializationImage)
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

                    Logger.Log($"Loading Stable Diffusion (ONNX) with model {s.Model.Trunc(80).Wrap()}...");

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
    }
}
