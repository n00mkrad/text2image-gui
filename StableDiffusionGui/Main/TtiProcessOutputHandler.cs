using StableDiffusionGui.Implementations;
using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StableDiffusionGui.Main
{
    internal class TtiProcessOutputHandler
    {
        private static bool _hasErrored = false;
        private static bool _invokeAiLastModelCached = false;

        public static void Reset()
        {
            _hasErrored = false;
        }

        public static void LogOutput(string line, bool stdErr = false)
        {
            if (string.IsNullOrWhiteSpace(line))
                return;

            //var noLogWildcards = new string[] { "step */*" };
            //
            //if (noLogWildcards.Where(w => !line.MatchesWildcard(w)).Any())
            Logger.Log(line, true, false, Constants.Lognames.Sd);

            bool ellipsis = Logger.LastUiLine.Contains("...");
            string errMsg = "";

            if (TextToImage.CurrentTaskSettings != null && TextToImage.CurrentTaskSettings.Implementation == Enums.StableDiffusion.Implementation.InvokeAi)
            {
                bool replace = ellipsis || Logger.LastUiLine.MatchesWildcard("*Generated*image*in*");

                if (!TextToImage.Canceled && line.StartsWith(">> Retrieving model "))
                {
                    _invokeAiLastModelCached = true;
                }

                if (!TextToImage.Canceled && line.StartsWith(">> Loading ") && line.Contains(" from "))
                {
                    _invokeAiLastModelCached = false;
                }

                if (!TextToImage.Canceled && line.StartsWith(">> Setting Sampler to "))
                {
                    Logger.Log($"Model {(_invokeAiLastModelCached ? " retrieved from RAM cache" : "loaded")}.", false, ellipsis);
                }

                if (!TextToImage.Canceled && line.MatchesWildcard("step */*"))
                {
                    if (!Logger.LastUiLine.MatchesWildcard("*Generated*image*in*"))
                        Logger.LogIfLastLineDoesNotContainMsg($"Generating...");

                    int[] stepsCurrentTarget = line.Split("step ")[1].Split('/').Select(x => x.GetInt()).ToArray();
                    int percent = (((float)stepsCurrentTarget[0] / stepsCurrentTarget[1]) * 100f).RoundToInt();

                    if (percent > 0 && percent <= 100)
                        Program.MainForm.SetProgressImg(percent);
                }

                if (!TextToImage.Canceled && line.Contains("image(s) generated in "))
                {
                    var split = line.Split("image(s) generated in ");
                    TextToImage.CurrentTask.ImgCount += split[0].GetInt();
                    Program.MainForm.SetProgress((int)Math.Round(((float)TextToImage.CurrentTask.ImgCount / TextToImage.CurrentTask.TargetImgCount) * 100f));

                    int lastMsPerImg = $"{split[1].Remove(".").Remove("s")}0".GetInt();
                    int remainingMs = (TextToImage.CurrentTask.TargetImgCount - TextToImage.CurrentTask.ImgCount) * lastMsPerImg;

                    Logger.Log($"Generated {split[0].GetInt()} image in {split[1]} ({TextToImage.CurrentTask.ImgCount}/{TextToImage.CurrentTask.TargetImgCount})" +
                        $"{(TextToImage.CurrentTask.ImgCount > 1 && remainingMs > 1000 ? $" - ETA: {FormatUtils.Time(remainingMs, false)}" : "")}", false, replace || Logger.LastUiLine.MatchesWildcard("*Generated*image*in*"));
                }

                if (line.Contains(".png: !fix "))
                {
                    try
                    {
                        string pathSource = line.Split(": !fix ")[1].Split(".png ")[0] + ".png";
                        string pathOut = line.Substring(line.IndexOf("] ") + 2).Split(": !fix ")[0];
                        TtiUtils.ExportPostprocessedImage(pathSource, pathOut);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Error parsing !fix log message: {ex.Message}\n{ex.StackTrace}", true);
                    }
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

                    if(line.Contains("state_dict"))
                        errMsg += $"\n\nThe model appears to be incompatible.";

                    if (line.Contains("pytorch_model.bin"))
                    {
                        errMsg += "\n\nCache seems to be corrupted and has been cleared. Please try again.";
                        IoUtils.TryDeleteIfExists(Path.Combine(Environment.ExpandEnvironmentVariables("%USERPROFILE%"), ".cache", "huggingface", "transformers"));
                        IoUtils.TryDeleteIfExists(Path.Combine(Paths.GetDataPath(), Constants.Dirs.Cache.Root, Constants.Dirs.Cache.Transformers));
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

                if (!TextToImage.Canceled && line.MatchesWildcard("*%|*| *") && !line.Contains("Fetching "))
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
                    TextToImage.CurrentTask.ImgCount += 1;
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

                if (!TextToImage.Canceled && line.MatchesWildcard("*%|*| *") && !line.Contains("Fetching "))
                {
                    if (!Logger.LastUiLine.MatchesWildcard("*Generated*image*in*") && !line.Contains("B/s"))
                        Logger.LogIfLastLineDoesNotContainMsg($"Generating...");

                    int percent = line.Split("%|")[0].GetInt();

                    if (percent > 0 && percent <= 100)
                    {
                        if(line.Contains("Downloading "))
                            Program.MainForm.SetProgress(percent);
                        else
                            Program.MainForm.SetProgressImg(percent);
                    }
                }

                if (!TextToImage.Canceled && line.Contains("Image generated in "))
                {
                    var split = line.Split("Image generated in ");
                    TextToImage.CurrentTask.ImgCount += 1;
                    Program.MainForm.SetProgress((int)Math.Round(((float)TextToImage.CurrentTask.ImgCount / TextToImage.CurrentTask.TargetImgCount) * 100f));

                    int lastMsPerImg = $"{split[1].Remove(".").Remove("s")}0".GetInt();
                    int remainingMs = (TextToImage.CurrentTask.TargetImgCount - TextToImage.CurrentTask.ImgCount) * lastMsPerImg;

                    Logger.Log($"Generated 1 image in {split[1]} ({TextToImage.CurrentTask.ImgCount}/{TextToImage.CurrentTask.TargetImgCount})" +
                        $"{(TextToImage.CurrentTask.ImgCount > 1 && remainingMs > 1000 ? $" - ETA: {FormatUtils.Time(remainingMs, false)}" : "")}", false, replace || Logger.LastUiLine.MatchesWildcard("*Generated*image*in*"));
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
            }

            if (_hasErrored)
            {
                TextToImage.Cancel($"Process has errored: {errMsg}", false);

                if (!string.IsNullOrWhiteSpace(errMsg))
                    Task.Run(() => UiUtils.ShowMessageBox(errMsg, UiUtils.MessageType.Error));
            }
        }
    }
}
