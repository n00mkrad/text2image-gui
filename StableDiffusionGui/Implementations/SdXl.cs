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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Implementations
{
    internal class SdXl : IImplementation
    {
        public List<string> LastMessages { get => _lastMessages; }
        private List<string> _lastMessages = new List<string>();
        private bool _hasErrored = false;

        public async Task Run(TtiSettings s, string outPath)
        {
            try
            {
                float[] initStrengths = s.InitStrengths.Select(n => 1f - n).ToArray();
                var cachedModels = Models.GetModels(Enums.Models.Type.Normal, Implementation.SdXl);
                Model model = TtiUtils.CheckIfModelExists(s.Model, Implementation.SdXl, cachedModels);

                if (model == null)
                    return;

                OrderedDictionary initImages = s.InitImgs != null && s.InitImgs.Length > 0 ? await TtiUtils.CreateResizedInitImagesIfNeeded(s.InitImgs.ToList(), s.Res) : null;
                long startSeed = s.Seed;
                bool refine = s.RefinerStrengths.All(rs => rs >= 0.05f);

                string mode = "txt2img";
                bool inpaintingMdl = model.Name.EndsWith(Constants.SuffixesPrefixes.InpaintingMdlSuf);

                if (s.InitImgs != null && s.InitImgs.Length > 0)
                {
                    mode = "img2img";

                    if (inpaintingMdl && s.ImgMode != ImgMode.InitializationImage)
                        mode = "inpaint";
                }

                var argLists = new List<Dictionary<string, string>>(); // List of all args for each command
                var args = new Dictionary<string, string>(); // List of args for current command
                args["mode"] = mode;
                args["model"] = model.FullName;
                args["modelRefiner"] = GetRefineModelPath(refine, model);
                args["prompt"] = "";

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
                        args["sampler"] = s.Sampler.ToString().Lower();

                        foreach (float scale in s.ScalesTxt)
                        {
                            args["scaleTxt"] = $"{scale.ToStringDot()}";

                            foreach (float refinerStrength in s.RefinerStrengths)
                            {
                                args["refineFrac"] = $"{(1f - refinerStrength).ToStringDot()}";

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

                var scriptArgs = new List<string>
                {
                    $"-p SdXl",
                    $"-o {outPath.Wrap(true)}"
                };

                if (Config.Instance.SdXlOptimize)
                {
                    scriptArgs.Add("--sdxl_optimize");
                }

                string newStartupSettings = string.Join(" ", scriptArgs).Remove(" ");

                if (!TtiProcess.IsAiProcessRunning || (TtiProcess.IsAiProcessRunning && TtiProcess.LastStartupSettings != newStartupSettings))
                {
                    if (TextToImage.Canceled) return;

                    Logger.Log($"(Re)starting Nmkdiffusers. Process running: {TtiProcess.IsAiProcessRunning} - Prev startup string: '{TtiProcess.LastStartupSettings}' - New startup string: '{newStartupSettings}'", true);
                    TtiProcess.LastStartupSettings = newStartupSettings;

                    Process py = OsUtils.NewProcess(true, logAction: HandleOutput, redirectStdin: true);
                    TextToImage.CurrentTask.Processes.Add(py);

                    py.StartInfo.Arguments = $"{OsUtils.GetCmdArg()} cd /D {Paths.GetDataPath().Wrap()} && {TtiUtils.GetEnvVarsSdCommand()} && {Constants.Files.VenvActivate} && python {Constants.Dirs.SdRepo}/nmkdiff/nmkdiffusers.py {string.Join(" ", scriptArgs)}";
                    Logger.Log("cmd.exe " + py.StartInfo.Arguments, true);

                    if (TtiProcess.CurrentProcess != null)
                    {
                        TtiProcess.ProcessExistWasIntentional = true;
                        OsUtils.KillProcessTree(TtiProcess.CurrentProcess.Id);
                    }

                    ResetLogger();
                    _genState = GenerationState.Base;

                    string modelStr = refine ? "(Base+Refiner)" : "(Base)";
                    Logger.Log($"Loading Stable Diffusion XL with model {s.Model.Trunc(80).Wrap()} {modelStr}...");

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

        private string GetRefineModelPath (bool doRefine, Model model)
        {
            if (!doRefine)
                return "";

            string refineModelName = model.Name.Replace("base", "refiner");
            string refinePath = Path.Combine(model.Directory.FullName, refineModelName);

            if (refinePath != model.FullName && IoUtils.IsPathValid(refinePath))
            {
                Logger.Log($"Using Refiner Model '{refineModelName}'.");
                return refinePath;
            }
            else
            {
                var refinersAll = Models.GetModels(Enums.Models.Type.Refiner, Implementation.SdXl);
                var refinersDiffFirst = refinersAll.Where(m => m.Format == Enums.Models.Format.Diffusers).ToList();
                refinersDiffFirst.AddRange(refinersAll.Where(m => m.Format == Enums.Models.Format.Safetensors).ToList());

                if (refinersDiffFirst.Any())
                {
                    string refineName = refineModelName != model.Name ? $"'{refineModelName}' " : "";
                    Logger.Log($"No corresponding refiner model {refineName}found, using '{refinersDiffFirst.First().Name}' instead.");
                    return refinersDiffFirst.First().FullName;
                }
                else
                {
                    Logger.Log("Warning: No refiner model found.");
                }
            }

            return "";
        }

        public enum GenerationState { Base, Refiner }
        private GenerationState _genState = GenerationState.Base;
        private float _refineFrac = 0.7f;

        public void HandleOutput(string line)
        {
            if (TextToImage.Canceled || TextToImage.CurrentTaskSettings == null || line == null)
                return;

            Logger.Log(line, true, false, Constants.Lognames.Sd);
            TtiProcessOutputHandler.LastMessages.Insert(0, line);

            bool ellipsis = Program.MainForm.LogText.EndsWith("...");
            // bool replace = ellipsis || Logger.LastUiLine.MatchesWildcard("*Image*generated*in*");

            if (line.Contains("Loading base model"))
            {
                Logger.Log($"Loading base model...", replaceLastLine: ellipsis);
            }

            if (line.Contains("Loading refiner model"))
            {
                Logger.Log($"Loading refiner model...", replaceLastLine: ellipsis);
            }

            if (line.Contains("refine_frac = "))
            {
                _refineFrac = line.Split("refine_frac =")[1].GetFloat();
            }

            if (line.Contains("Running base model"))
            {
                _genState = GenerationState.Base;
            }

            if (line.Contains("Running refine model"))
            {
                _genState = GenerationState.Refiner;
            }

            if (line.MatchesWildcard("*%|*| *") && !line.Contains("Loading"))
            {
                if (!Logger.LastUiLine.MatchesWildcard("*Generated*image*in*"))
                    Logger.LogIfLastLineDoesNotContainMsg($"Generating...", replaceLastLine: ellipsis);

                int percent;
                int prog = line.Split("%|")[0].GetInt();

                if (_genState == GenerationState.Base)
                    percent = (prog * _refineFrac).RoundToInt();
                else
                    percent = (prog * (1.0f - _refineFrac)).RoundToInt() + (100 * _refineFrac).RoundToInt();

                if (percent >= 0 && percent < 100)
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
            Program.MainForm.runBtn.Enabled = false;

            await TtiProcess.WriteStdIn("stop", 0, true);

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
