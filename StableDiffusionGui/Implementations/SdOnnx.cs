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
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Implementations
{
    internal class SdOnnx : ImplementationBase, IImplementation
    {
        public List<string> LastMessages { get => _lastMessages; set => _lastMessages = value; }
        private List<string> _lastMessages = new List<string>();
        private bool _hasErrored = false;

        public async Task Run(TtiSettings s, string outPath)
        {
            try
            {
                float[] initStrengths = s.InitStrengths.Select(n => 1f - n).ToArray();
                var cachedModels = Models.GetModels(Enums.Models.Type.Normal, Implementation.DiffusersOnnx);
                Model model = TtiUtils.CheckIfModelExists(s.Model, Implementation.DiffusersOnnx);

                if (model == null)
                    return;

                OrderedDictionary initImages = s.InitImgs != null && s.InitImgs.Length > 0 ? await TtiUtils.CreateResizedInitImagesIfNeeded(s.InitImgs.ToList(), s.Res) : null;
                long startSeed = s.Seed;
                string mode = NmkdiffUtils.GetGenerationMode(s, model);

                List<Dictionary<string, string>> argLists = new List<Dictionary<string, string>>(); // List of all args for each command
                Dictionary<string, string> args = new Dictionary<string, string>(); // List of args for current command
                args["mode"] = mode;
                args["model"] = model.FullName;
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
                        args["inpaintMask"] = "";
                        args["prompt"] = processedPrompts[i];
                        args["promptNeg"] = s.NegativePrompt;
                        args["w"] = $"{s.Res.Width}";
                        args["h"] = $"{s.Res.Height}";
                        args["seed"] = $"{s.Seed}";

                        foreach (float scale in s.ScalesTxt)
                        {
                            args["scaleTxt"] = $"{scale.ToStringDot()}";

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

                Logger.ClearLogBox();
                Logger.Log($"Running Stable Diffusion - {s.Res.Width}x{s.Res.Height}, Starting Seed: {startSeed}");

                string initsStr = initImages != null ? $" and {initImages.Count} image{(initImages.Count != 1 ? "s" : "")} using {initStrengths.Length} strength{(initStrengths.Length != 1 ? "s" : "")}" : "";
                Logger.Log($"{s.Prompts.Length} prompt{(s.Prompts.Length != 1 ? "s" : "")} * {s.Iterations} image{(s.Iterations != 1 ? "s" : "")} * {s.Steps.Length} step value{(s.Steps.Length != 1 ? "s" : "")} * {s.ScalesTxt.Length} scale{(s.ScalesTxt.Length != 1 ? "s" : "")}{initsStr} = {argLists.Count} images total.");

                if (!TtiProcess.IsAiProcessRunning)
                {
                    Process py = OsUtils.NewProcess(true, logAction: HandleOutput, redirectStdin: true);
                    TextToImage.CurrentTask.Processes.Add(py);

                    py.StartInfo.Arguments = $"/C cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSdCommand()} && {Constants.Files.VenvActivate} && " +
                        $"python \"{Constants.Dirs.SdRepo}/nmkdiff/nmkdiffusers.py\" -p SdOnnx -g {mode} -m {model.FullName.Wrap(true)} -o {outPath.Wrap(true)}";

                    Logger.Log("cmd.exe " + py.StartInfo.Arguments, true);

                    if (TtiProcess.CurrentProcess != null)
                    {
                        TtiProcess.ProcessExistWasIntentional = true;
                        OsUtils.KillProcessTree(TtiProcess.CurrentProcess.Id);
                    }

                    ResetLogger();

                    Logger.Log($"Loading Stable Diffusion (ONNX) with model {s.Model.Trunc(80).Wrap()}...");

                    TtiProcess.ProcessExistWasIntentional = false;
                    OsUtils.StartProcess(py, killWithParent: true);
                    TtiProcess.CurrentProcess = py;

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

            if (!TextToImage.Canceled && line.MatchesWildcard("*%|*| *") && !line.Contains("Fetching ") && !line.Contains("Loading pipeline components"))
            {
                if (!Logger.LastUiLine.MatchesWildcard("*Generated*image*in*"))
                    Logger.LogIfLastLineDoesNotContainMsg($"Generating...");

                int percent = line.Split("%|")[0].GetInt();

                if (percent > 0 && percent <= 100)
                    Program.MainForm.SetProgressImg(percent);
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
