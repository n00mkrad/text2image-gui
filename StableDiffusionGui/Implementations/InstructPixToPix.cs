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
using System.Threading.Tasks;
using static StableDiffusionGui.Main.Constants;
using static StableDiffusionGui.Main.Enums.StableDiffusion;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace StableDiffusionGui.Implementations
{
    internal class InstructPixToPix
    {
        public static async Task Run(string[] prompts, string negPrompt, int iterations, Dictionary<string, string> parameters, string outPath)
        {
            try
            {
                string[] initImgs = parameters.FromJson<string[]>("initImgs");
                // string embedding = parameters.FromJson<string>("embedding");
                float[] initStrengths = parameters.FromJson<float[]>("initStrengths").Select(n => 1f - n).ToArray();
                int[] steps = parameters.FromJson<int[]>("steps");
                float[] scalesTxt = parameters.FromJson<float[]>("scales");
                float[] scalesImg = parameters.FromJson<float[]>("scalesImg");
                long seed = parameters.FromJson<long>("seed");
                bool lockSeed = parameters.FromJson<bool>("lockSeed"); // Lock seed (disable auto-increment)
                // string sampler = parameters.FromJson<string>("sampler");
                Size res = parameters.FromJson<Size>("res");
                // string model = parameters.FromJson<string>("model");

                // var cachedModels = Paths.GetModels(ModelType.Normal, Implementation.DiffusersOnnx);
                // Model modelDir = TtiUtils.CheckIfCurrentSdModelExists();
                // 
                // if (modelDir == null)
                //     return;

                OrderedDictionary initImages = initImgs != null && initImgs.Length > 0 ? await TtiUtils.CreateResizedInitImagesIfNeeded(initImgs.ToList(), res) : null;

                if (initImages == null || initImages.Count < 1)
                {
                    UiUtils.ShowMessageBox("Please load an image first.", "No image loaded!");
                    return;
                }

                long startSeed = seed;

                List<Dictionary<string, string>> argLists = new List<Dictionary<string, string>>(); // List of all args for each command
                Dictionary<string, string> args = new Dictionary<string, string>(); // List of args for current command
                args["prompt"] = "";

                foreach (string prompt in prompts)
                {
                    List<string> processedPrompts = PromptWildcardUtils.ApplyWildcardsAll(prompt, iterations, false);
                    TextToImage.CurrentTaskSettings.ProcessedAndRawPrompts = processedPrompts.Distinct().ToDictionary(x => x, x => prompt);

                    for (int i = 0; i < iterations; i++)
                    {
                        args["initImg"] = "";
                        args["initStrength"] = "0";
                        args["prompt"] = processedPrompts[i];
                        args["promptNeg"] = negPrompt;
                        args["seed"] = $"{seed}";

                        foreach (float scale in scalesTxt)
                        {
                            args["scaleTxt"] = $"{scale.ToStringDot()}";

                            foreach (float scaleImg in scalesImg)
                            {
                                args["scaleImg"] = $"{scaleImg.ToStringDot()}";

                                foreach (int stepCount in steps)
                                {
                                    args["steps"] = $"{stepCount}";

                                    foreach (string initImg in initImages.Values)
                                    {
                                        foreach (float strength in initStrengths)
                                        {
                                            args["initImg"] = initImg;
                                            args["initStrength"] = strength.ToStringDot("0.###");

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

                Logger.Log($"Running Stable Diffusion - {iterations} Iterations, {steps.Length} Steps, Scales {(scalesTxt.Length < 4 ? string.Join(", ", scalesTxt.Select(x => x.ToStringDot())) : $"{scalesTxt.First()}->{scalesTxt.Last()}")}, Starting Seed: {startSeed}");

                string initsStr = initImages != null ? $" and {initImages.Count} image{(initImages.Count != 1 ? "s" : "")} using {initStrengths.Length} strength{(initStrengths.Length != 1 ? "s" : "")}" : "";
                Logger.Log($"{prompts.Length} prompt{(prompts.Length != 1 ? "s" : "")} * {iterations} image{(iterations != 1 ? "s" : "")} * {steps.Length} step value{(steps.Length != 1 ? "s" : "")} * {scalesTxt.Length} scale{(scalesTxt.Length != 1 ? "s" : "")}{initsStr} = {argLists.Count} images total.");

                if (!TtiProcess.IsAiProcessRunning)
                {
                    Process py = OsUtils.NewProcess(!OsUtils.ShowHiddenCmd());
                    TextToImage.CurrentTask.Processes.Add(py);

                    py.StartInfo.RedirectStandardInput = true;
                    py.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSdCommand()} && " +
                        $"python \"{Constants.Dirs.SdRepo}/nmkdiff/nmkdiffusers.py\" -p InstructPix2Pix -o {outPath.Wrap(true)}";

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

                    Logger.Log($"Loading Stable Diffusion (InstructPix2Pix)...");

                    TtiProcessOutputHandler.Reset();
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
                    TtiProcess.CurrentStdInWriter = new NmkdStreamWriter(py);
                }
                else
                {
                    TtiProcessOutputHandler.Reset();
                    TextToImage.CurrentTask.Processes.Add(TtiProcess.CurrentProcess);
                }

                foreach (var argList in argLists)
                    await TtiProcess.WriteStdIn($"generate {argList.ToJson()}", true);
            }
            catch (Exception ex)
            {
                Logger.Log($"Unhandled Stable Diffusion Error: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
            }
        }

        public static async Task Cancel()
        {
            Program.MainForm.runBtn.Enabled = false;

            await TtiProcess.WriteStdIn($"stop", true);

            await Task.Delay(100);

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
                }

                await Task.Delay(100);
            }

            Program.MainForm.runBtn.Enabled = true;
        }
    }
}
