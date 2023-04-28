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
using System.Linq;
using System.Threading.Tasks;

namespace StableDiffusionGui.Implementations
{
    internal class InstructPixToPix
    {
        public static async Task Run(TtiSettings s, string outPath)
        {
            try
            {
                OrderedDictionary initImages = s.InitImgs != null && s.InitImgs.Length > 0 ? await TtiUtils.CreateResizedInitImagesIfNeeded(s.InitImgs.ToList(), s.Res) : null;

                if (initImages == null || initImages.Count < 1)
                {
                    UiUtils.ShowMessageBox("Please load an image first.", "No image loaded!");
                    return;
                }

                long startSeed = s.Seed;

                List<Dictionary<string, string>> argLists = new List<Dictionary<string, string>>(); // List of all args for each command
                Dictionary<string, string> args = new Dictionary<string, string>(); // List of args for current command
                args["prompt"] = "";

                foreach (string prompt in s.Prompts)
                {
                    List<string> processedPrompts = PromptWildcardUtils.ApplyWildcardsAll(prompt, s.Iterations, false);
                    TextToImage.CurrentTaskSettings.ProcessedAndRawPrompts = new EasyDict<string, string>(processedPrompts.Distinct().ToDictionary(x => x, x => prompt));

                    for (int i = 0; i < s.Iterations; i++)
                    {
                        args["initImg"] = "";
                        args["initStrength"] = "0";
                        args["prompt"] = processedPrompts[i];
                        args["promptNeg"] = s.NegativePrompt;
                        args["seed"] = $"{s.Seed}";

                        foreach (float scale in s.ScalesTxt)
                        {
                            args["scaleTxt"] = $"{scale.ToStringDot()}";

                            foreach (float scaleImg in s.ScalesImg)
                            {
                                args["scaleImg"] = $"{scaleImg.ToStringDot()}";

                                foreach (int stepCount in s.Steps)
                                {
                                    args["steps"] = $"{stepCount}";

                                    foreach (string initImg in initImages.Values)
                                    {
                                        foreach (float strength in s.InitStrengths)
                                        {
                                            args["initImg"] = initImg;
                                            args["initStrength"] = strength.ToStringDot("0.###");

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

                Logger.Log($"Running Stable Diffusion - {s.Iterations} Iterations, {s.Steps.Length} Steps, Scales {(s.ScalesTxt.Length < 4 ? string.Join(", ", s.ScalesTxt.Select(x => x.ToStringDot())) : $"{s.ScalesTxt.First()}->{s.ScalesTxt.Last()}")}, Starting Seed: {startSeed}");

                string initsStr = initImages != null ? $" and {initImages.Count} image{(initImages.Count != 1 ? "s" : "")} using {s.InitStrengths.Length} strength{(s.InitStrengths.Length != 1 ? "s" : "")}" : "";
                Logger.Log(GetImageCountLogString(initImages, s.Prompts.Length, s.Iterations, s.Steps.Length, s.ScalesTxt.Length, s.ScalesImg.Length, argLists.Count));

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
                    await TtiProcess.WriteStdIn($"generate {argList.ToJson()}", 200, true);
            }
            catch (Exception ex)
            {
                Logger.Log($"Unhandled Stable Diffusion Error: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
            }
        }

        public static string GetImageCountLogString(OrderedDictionary initImages, int prompts, int iterations, int steps, int scalesTxt, int scalesImg, int argLists)
        {
            string initsStr = initImages != null ? $" and {initImages.Count} Image{(initImages.Count != 1 ? "s" : "")}" : "";
            string log = $"{prompts} Prompt{(prompts != 1 ? "s" : "")} * {iterations} Image{(iterations != 1 ? "s" : "")} * {steps} Step Value{(steps != 1 ? "s" : "")} * {scalesTxt} (Prompt) * {scalesImg} (Image) Scale Values{initsStr} = {argLists} Images Total";

            if (ConfigParser.UpscaleAndSaveOriginals)
                log += $" ({argLists * 2} With Post-processed Images)";

            return $"{log}.";
        }

        public static async Task Cancel()
        {
            Program.MainForm.runBtn.Enabled = false;

            await TtiProcess.WriteStdIn($"stop", 0, true);

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
