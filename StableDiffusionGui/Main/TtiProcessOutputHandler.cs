using StableDiffusionGui.Implementations;
using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StableDiffusionGui.Main
{
    internal class TtiProcessOutputHandler
    {
        private static bool _hasErrored = false;
        private static bool _invokeAiLastModelCached = false;
        public enum HiresFixStatus { NotUpscaling, WaitingForStart, Upscaling }
        private static HiresFixStatus _hiresFixStatus;

        public static List<string> LastMessages = new List<string>();


        public static void Reset()
        {
            _hasErrored = false;
            _hiresFixStatus = HiresFixStatus.NotUpscaling;
            LastMessages.Clear();
        }

        public static void LogOutput(string line, bool stdErr = false)
        {
            if (string.IsNullOrWhiteSpace(line))
                return;

            Logger.Log(line, true, false, Constants.Lognames.Sd);
            LastMessages.Insert(0, line);

            bool ellipsis = Program.MainForm.LogText.EndsWith("...");
            string l = Program.MainForm.LogText;
            string errMsg = "";

            if (TextToImage.CurrentTaskSettings != null && TextToImage.CurrentTaskSettings.Implementation == Enums.StableDiffusion.Implementation.InvokeAi)
            {
                bool replace = ellipsis || Logger.LastUiLine.MatchesWildcard("*Generated*image*in*");

                if (!TextToImage.Canceled && line.StartsWith(">> Retrieving model "))
                    _invokeAiLastModelCached = true;
                else if (!TextToImage.Canceled && line.StartsWith(">> Loading ") && line.Contains(" from "))
                    _invokeAiLastModelCached = false;

                if (!TextToImage.Canceled && _hiresFixStatus == HiresFixStatus.WaitingForStart && line.Remove(" ").MatchesWildcard(@"0%||0/*[00:00<?,?it/s]*"))
                {
                    _hiresFixStatus = HiresFixStatus.Upscaling;
                }

                if (!TextToImage.Canceled && line.Remove(" ") == @"Generating:0%||0/1[00:00<?,?it/s]")
                {
                    ImageExport.TimeSinceLastImage.Restart();
                    _hiresFixStatus = HiresFixStatus.NotUpscaling;
                }

                if (!TextToImage.Canceled && line.StartsWith(">> Interpolating from"))
                {
                    _hiresFixStatus = _hiresFixStatus = HiresFixStatus.WaitingForStart;
                }

                if (!TextToImage.Canceled && line.MatchesWildcard("*%|*|*/*") && !line.Lower().Contains("downloading") && !line.Contains("Loading pipeline components"))
                {
                    string progStr = line.Split('|')[2].Trim().Split(' ')[0].Trim(); // => e.g. "3/50"

                    if (!Logger.LastUiLine.MatchesWildcard("*Generated*image*in*"))
                        Logger.LogIfLastLineDoesNotContainMsg($"Generating...", false, ellipsis);

                    try
                    {
                        int progressDivisor = TextToImage.CurrentTaskSettings.HiresFix ? 2 : 1;
                        int[] stepsCurrentTarget = progStr.Split('/').Select(x => x.GetInt()).ToArray();
                        int percent = ((((float)stepsCurrentTarget[0] / stepsCurrentTarget[1]) * 100f) / progressDivisor).RoundToInt();

                        if (TextToImage.CurrentTaskSettings.HiresFix && _hiresFixStatus == HiresFixStatus.Upscaling)
                            percent += 50;

                        if (percent > 0 && percent <= 100)
                            Program.MainForm.SetProgressImg(percent);
                    }
                    catch { }
                }

                if (!TextToImage.Canceled && line.StartsWith("[") && !line.Contains(".png: !fix "))
                {
                    string outPath = Path.Combine(Paths.GetSessionDataPath(), "out").Replace('\\', '/');

                    if (line.Contains(outPath) && new Regex(@"\[\d+(\.\d+)?\] [A-Z]:\/").Matches(line.Trim()).Count >= 1)
                    {
                        string path = outPath + line.Split(outPath)[1].Split(':')[0];
                        ImageExport.Export(path);
                    }
                }

                if (line.Contains(".png: !fix "))
                {
                    try
                    {
                        if (LastMessages.Take(5).Any(x => x.Contains("ESRGAN is disabled.")))
                        {
                            Logger.Log($"Post-Processing is disabled, can't run enhancement.");
                        }
                        else
                        {
                            string pathSource = line.Split(": !fix ")[1].Split(".png ")[0] + ".png";
                            string pathOut = line.Substring(line.IndexOf("] ") + 2).Split(": !fix ")[0];
                            TtiUtils.ExportPostprocessedImage(pathSource, pathOut);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Error parsing !fix log message: {ex.Message}\n{ex.StackTrace}", true);
                    }
                }

                if (line.Trim().StartsWith("invoke_pid="))
                {
                    InvokeAi.Pid = line.Split('=')[1].GetInt();
                }

                if (line.Trim().StartsWith(Constants.LogMsgs.Invoke.TiTriggers))
                {
                    Logger.Log($"Model {(_invokeAiLastModelCached ? " retrieved from RAM cache" : "loaded")}.", false, ellipsis);
                }

                if (line.Trim().StartsWith(">> Preparing tokens for textual inversion"))
                {
                    Logger.Log("Loading textual inversion...", false, ellipsis);
                }

                // if (line.Trim().StartsWith(">> Embedding not found:"))
                // {
                //     string emb = line.Split(": ").Last();
                //     Logger.Log($"Warning: No compatible embedding with trigger '{emb}' found!", false, ellipsis);
                // }

                if (line.Trim().StartsWith(">> Converting legacy checkpoint"))
                {
                    Logger.Log($"Warning: Model is not in Diffusers format, this makes loading slower due to conversion. For a speedup, convert it to a Diffusers model.", false, ellipsis);
                }

                if (line.Contains("is not a known model name. Cannot change"))
                {
                    Logger.Log($"No model with this name and VAE found. Can't change model.", false, ellipsis);
                }

                if (!_hasErrored && line.Contains("An error occurred while processing your prompt"))
                {
                    Logger.Log(line);
                    _hasErrored = true;
                }

                if (!_hasErrored && line.Trim().EndsWith(" is not a known model name. Please check your models.yaml file"))
                {
                    errMsg = $"Failed to switch models.\n\nPossibly you tried to load an incompatible model.";
                    _hasErrored = true;
                }

                if (!_hasErrored && line.Trim().StartsWith("** model ") && line.Contains("could not be loaded:"))
                {
                    errMsg = $"Failed to load model.";

                    if (line.Contains("state_dict"))
                        errMsg += $"\n\nThe model appears to be incompatible.";

                    if (line.Contains("pytorch_model.bin"))
                    {
                        errMsg += "\n\nCache seems to be corrupted and has been cleared. Please try again.";
                        IoUtils.TryDeleteIfExists(Path.Combine(Environment.ExpandEnvironmentVariables("%USERPROFILE%"), ".cache", "huggingface", "transformers"));
                        IoUtils.TryDeleteIfExists(Path.Combine(Paths.GetDataPath(), Constants.Dirs.Cache.Root, Constants.Dirs.Cache.Hf));
                    }

                    _hasErrored = true;
                }
            }

            if (TextToImage.CurrentTaskSettings != null && TextToImage.CurrentTaskSettings.Implementation == Enums.StableDiffusion.Implementation.OptimizedSd)
            {
                bool replace = ellipsis || Logger.LastUiLine.MatchesWildcard("*Generated*image*in*");

                if (line.Contains("reading prompts from"))
                    Logger.Log("Generating...");

                if (line.Contains("Decoding image: "))
                {
                    int percent = line.Split("Decoding image: ")[1].Split('#')[0].GetInt();

                    if (percent > 0 && percent <= 100)
                        Program.MainForm.SetProgressImg(percent);
                }

                if (line.MatchesWildcard("*data: 100%*<00:00,*it*]"))
                {
                    TextToImage.CurrentTask.ImgCount += 1;
                    Program.MainForm.SetProgress((int)Math.Round(((float)TextToImage.CurrentTask.ImgCount / TextToImage.CurrentTask.TargetImgCount) * 100f));

                    int lastMsPerImg = line.EndsWith("it/s]") ? (1000000f / (line.Split("00:00, ").Last().Remove(".").Remove("s") + "0").GetInt()).RoundToInt() : (line.Split("00:00, ").Last().Remove(".").Remove("s") + "0").GetInt();
                    int remainingMs = (TextToImage.CurrentTask.TargetImgCount - TextToImage.CurrentTask.ImgCount) * lastMsPerImg;

                    string lastLine = Logger.LastUiLine;

                    Logger.Log($"Generated 1 image in {FormatUtils.Time(lastMsPerImg, false)} ({TextToImage.CurrentTask.ImgCount}/{TextToImage.CurrentTask.TargetImgCount})" +
                        $"{(TextToImage.CurrentTask.ImgCount > 1 && remainingMs > 1000 ? $" - ETA: {FormatUtils.Time(remainingMs, false)}" : "")}", false, replace || Logger.LastUiLine.MatchesWildcard("Generated*image*"));
                }
            }

            if (TextToImage.CurrentTaskSettings != null && TextToImage.CurrentTaskSettings.Implementation == Enums.StableDiffusion.Implementation.DiffusersOnnx)
            {
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
            }

            if (TextToImage.CurrentTaskSettings != null && TextToImage.CurrentTaskSettings.Implementation == Enums.StableDiffusion.Implementation.SdXl)
            {
                bool replace = ellipsis || Logger.LastUiLine.MatchesWildcard("*Image*generated*in*");

                if (line.StartsWith("Model loaded"))
                {
                    Logger.Log($"{line}", false, ellipsis);
                }

                if (!TextToImage.Canceled && line.MatchesWildcard("*%|*| *") && !line.Contains("Fetching") && !line.Contains("Loading checkpoint"))
                {
                    if (!Logger.LastUiLine.MatchesWildcard("*Generated*image*in*"))
                        Logger.LogIfLastLineDoesNotContainMsg($"Generating...");

                    int percent = line.Split("%|")[0].GetInt();

                    if (percent > 0 && percent <= 100)
                        Program.MainForm.SetProgressImg(percent);
                }

                if (!TextToImage.Canceled && line.Contains("Image generated in "))
                {
                    var split = line.Split("Image generated in ");
                    Program.MainForm.SetProgress((int)Math.Round(((float)TextToImage.CurrentTask.ImgCount / TextToImage.CurrentTask.TargetImgCount) * 100f));

                    int lastMsPerImg = $"{split[1].Remove(".").Remove("s")}0".GetInt();
                    int remainingMs = (TextToImage.CurrentTask.TargetImgCount - TextToImage.CurrentTask.ImgCount) * lastMsPerImg;

                    Logger.Log($"Generated 1 image in {split[1]} ({TextToImage.CurrentTask.ImgCount}/{TextToImage.CurrentTask.TargetImgCount})" +
                        $"{(TextToImage.CurrentTask.ImgCount > 1 && remainingMs > 1000 ? $" - ETA: {FormatUtils.Time(remainingMs, false)}" : "")}", false, replace || Logger.LastUiLine.MatchesWildcard("*Generated*image*in*"));
                }
            }


            if (TextToImage.CurrentTaskSettings != null && TextToImage.CurrentTaskSettings.Implementation == Enums.StableDiffusion.Implementation.InstructPixToPix)
            {
                bool replace = ellipsis || Logger.LastUiLine.MatchesWildcard("*Image*generated*in*");

                if (line.StartsWith("Model loaded"))
                {
                    Logger.Log($"{line}", false, ellipsis);
                }

                if (!TextToImage.Canceled && line.Trim().StartsWith("0%") && line.Contains("[00:00<?, ?it/s]"))
                {
                    ImageExport.TimeSinceLastImage.Restart();
                }

                if (!TextToImage.Canceled && line.MatchesWildcard("*%|*| *") && !line.Contains("Fetching "))
                {
                    if (!Logger.LastUiLine.MatchesWildcard("*Generated*image*in*") && !line.Contains("B/s"))
                        Logger.LogIfLastLineDoesNotContainMsg($"Generating...");

                    int percent = line.Split("%|")[0].GetInt();

                    if (percent > 0 && percent <= 100)
                    {
                        if (line.Contains("Downloading "))
                            ; // Program.MainForm.SetProgress(percent);
                        else
                            Program.MainForm.SetProgressImg(percent);
                    }
                }
            }

            if (line.MatchesWildcard("*%|*/*[*B/s]*") && !line.Lower().Contains("it/s") && !line.Lower().Contains("s/it"))
            {
                Logger.Log($"Downloading required files - {line.Trunc(80)}...", false, ellipsis);
            }

            if (line.MatchesWildcard("Added terms: *, *"))
            {
                if (line.MatchesWildcard("*, <*>"))
                    Logger.Log($"Concept keyword: {line.Split("Added terms: *, ").LastOrDefault()}", false, ellipsis);
                else
                    Logger.Log($"Concept keyword: <{line.Split("Added terms: *, ").LastOrDefault()}>", false, ellipsis);
            }

            TextToImage.CancelMode cancelMode = TextToImage.CancelMode.SoftKill;
            string lastLogLines = string.Join("\n", Logger.GetLastLines(Constants.Lognames.Sd, 6));

            if (!_hasErrored && line.Contains("CUDA out of memory"))
            {
                _hasErrored = true;
                errMsg = $"Your GPU ran out of VRAM! Try a lower resolution.\n\n{line.Split("If reserved memory is").FirstOrDefault()}";
            }

            if (!_hasErrored && (line.Contains("PytorchStreamReader failed reading zip archive") || line.Contains("UnpicklingError")))
            {
                _hasErrored = true;
                errMsg = $"Your model file seems to be damaged or incomplete!\n\n{line}";
            }

            if (!_hasErrored && line.StartsWith("usage: "))
            {
                _hasErrored = true;
                errMsg = $"Invalid CLI syntax.";
            }

            if (!_hasErrored && line.Lower().Contains("illegal memory access"))
            {
                _hasErrored = true;
                errMsg = $"Your GPU appears to be unstable! If you have an overclock enabled, please disable it!\n\n{line}";
            }

            if (!_hasErrored && (line.Contains("RuntimeError") || line.Contains("ImportError") || line.Contains("OSError") || line.Contains("KeyError") || line.Contains("ModuleNotFoundError")))
            {
                _hasErrored = true;
                errMsg = $"Python Error:\n\n{line}";
                cancelMode = TextToImage.CancelMode.ForceKill;
            }

            if (_hasErrored)
            {
                TextToImage.Cancel($"Process has errored: {errMsg}", false, cancelMode);

                if (!string.IsNullOrWhiteSpace(errMsg))
                    Task.Run(() => UiUtils.ShowMessageBox(errMsg, UiUtils.MessageType.Error));
            }
        }
    }
}
