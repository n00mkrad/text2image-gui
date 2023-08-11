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
    internal class InstructPixToPix : ImplementationBase, IImplementation
    {
        public List<string> LastMessages { get => _lastMessages; }
        private List<string> _lastMessages = new List<string>();
        private bool _hasErrored = false;

        public async Task Run(TtiSettings s, string outPath)
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
                args["model"] = "";
                args["prompt"] = "";
                args["sampler"] = s.Sampler.ToString().Lower();

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

                Logger.ClearLogBox();
                Logger.Log($"Running Stable Diffusion (InstructPix2Pix) - {s.Res.Width}x{s.Res.Height}, Starting Seed: {startSeed}");

                string initsStr = initImages != null ? $" and {initImages.Count} image{(initImages.Count != 1 ? "s" : "")} using {s.InitStrengths.Length} strength{(s.InitStrengths.Length != 1 ? "s" : "")}" : "";
                Logger.Log(GetImageCountLogString(initImages, s.Prompts.Length, s.Iterations, s.Steps.Length, s.ScalesTxt.Length, s.ScalesImg.Length, argLists.Count));

                if (!TtiProcess.IsAiProcessRunning)
                {
                    Process py = OsUtils.NewProcess(true, logAction: HandleOutput, redirectStdin: true);
                    TextToImage.CurrentTask.Processes.Add(py);

                    py.StartInfo.Arguments = $"/C cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSdCommand()} && {Constants.Files.VenvActivate} && " +
                        $"python \"{Constants.Dirs.SdRepo}/nmkdiff/nmkdiffusers.py\" -p InstructPix2Pix -o {outPath.Wrap(true)}";

                    Logger.Log("cmd.exe " + py.StartInfo.Arguments, true);

                    if (TtiProcess.CurrentProcess != null)
                    {
                        TtiProcess.ProcessExistWasIntentional = true;
                        OsUtils.KillProcessTree(TtiProcess.CurrentProcess.Id);
                    }

                    Logger.Log($"Loading Stable Diffusion (InstructPix2Pix)...");

                    ResetLogger();
                    TtiProcess.CurrentProcess = py;
                    TtiProcess.ProcessExistWasIntentional = false;
                    OsUtils.StartProcess(py, killWithParent: true);

                    Task.Run(() => TtiProcess.CheckStillRunning());
                    TtiProcess.CurrentStdInWriter = new NmkdStreamWriter(py);
                }
                else
                {
                    ResetLogger();
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

        public string GetImageCountLogString(OrderedDictionary initImages, int prompts, int iterations, int steps, int scalesTxt, int scalesImg, int argLists)
        {
            string initsStr = initImages != null ? $" and {initImages.Count} Image{(initImages.Count != 1 ? "s" : "")}" : "";
            string log = $"{prompts} Prompt{(prompts != 1 ? "s" : "")} * {iterations} Image{(iterations != 1 ? "s" : "")} * {steps} Step Value{(steps != 1 ? "s" : "")} * {scalesTxt} (Prompt) * {scalesImg} (Image) Scale Values{initsStr} = {argLists} Images Total";

            if (ConfigParser.UpscaleAndSaveOriginals())
                log += $" ({argLists * 2} With Post-processed Images)";

            return $"{log}.";
        }

        public void HandleOutput(string line)
        {
            if (TextToImage.Canceled || TextToImage.CurrentTaskSettings == null || line == null)
                return;

            Logger.Log(line, true, false, Constants.Lognames.Sd);
            _lastMessages.Insert(0, line);

            bool ellipsis = Program.MainForm.LogText.EndsWith("...");
            bool replace = ellipsis || Logger.LastUiLine.MatchesWildcard("*Image*generated*in*");

            if (line.StartsWith("Model loaded"))
            {
                Logger.Log($"{line}", false, ellipsis);
            }

            if (!TextToImage.Canceled && line.Trim().StartsWith("0%") && line.Contains("[00:00<?, ?it/s]"))
            {
                ImageExport.TimeSinceLastImage.Restart();
            }

            if (!TextToImage.Canceled && line.MatchesWildcard("*%|*| *") && !line.Contains("Fetching ") && !line.Contains("Downloading ") && !line.Contains("Loading pipeline"))
            {
                if (!Logger.LastUiLine.MatchesWildcard("*Generated*image*in*") && !line.Contains("B/s"))
                    Logger.LogIfLastLineDoesNotContainMsg($"Generating...");

                int percent = line.Split("%|")[0].GetInt();

                if (percent > 0 && percent <= 100)
                {
                    Program.MainForm.SetProgressImg(percent);
                }
            }

            TtiProcessOutputHandler.HandleLogGeneric(this, line, _hasErrored);
        }

        public void ResetLogger()
        {
            _hasErrored = false;
            LastMessages.Clear();
        }

        public async Task Cancel()
        {
            await CancelNmkdiffusers();
        }
    }
}
