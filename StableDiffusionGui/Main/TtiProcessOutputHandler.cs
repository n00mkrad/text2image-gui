using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StableDiffusionGui.Main
{
    internal class TtiProcessOutputHandler
    {
        private static bool _hasErrored = false;

        public static void Start()
        {
            _hasErrored = false;
        }

        public static void LogOutput(string line, bool stdErr = false)
        {
            if (string.IsNullOrWhiteSpace(line))
                return;

            Logger.Log(line, true, false, Constants.Lognames.Sd);

            if (TextToImage.Canceled)
                return;

            bool ellipsis = Logger.LastUiLine.Contains("...");

            if (TextToImage.CurrentTaskSettings != null && TextToImage.CurrentTaskSettings.Implementation == Data.Implementation.StableDiffusion)
            {
                bool replace = ellipsis || Logger.LastUiLine.MatchesWildcard("*Generated*image*in*");

                if (line.Contains("Setting Sampler"))
                    Logger.Log("Generating...");

                if (line.MatchesWildcard("step */*"))
                {
                    int[] stepsCurrentTarget = line.Split("step ")[1].Split('/').Select(x => x.GetInt()).ToArray();

                    int percent = (((float)stepsCurrentTarget[0] / stepsCurrentTarget[1]) * 100f).RoundToInt();

                    if (percent > 0 && percent <= 100)
                        Program.MainForm.SetProgressImg(percent);
                }

                if (line.Contains("image(s) generated in "))
                {
                    var split = line.Split("image(s) generated in ");
                    TextToImage.CurrentTask.ImgCount += split[0].GetInt();
                    Program.MainForm.SetProgress((int)Math.Round(((float)TextToImage.CurrentTask.ImgCount / TextToImage.CurrentTask.TargetImgCount) * 100f));

                    int lastMsPerImg = $"{split[1].Remove(".").Remove("s")}0".GetInt();
                    int remainingMs = (TextToImage.CurrentTask.TargetImgCount - TextToImage.CurrentTask.ImgCount) * lastMsPerImg;

                    string lastLine = Logger.LastUiLine;

                    Logger.Log($"Generated {split[0].GetInt()} image in {split[1]} ({TextToImage.CurrentTask.ImgCount}/{TextToImage.CurrentTask.TargetImgCount})" +
                        $"{(TextToImage.CurrentTask.ImgCount > 1 && remainingMs > 1000 ? $" - ETA: {FormatUtils.Time(remainingMs, false)}" : "")}", false, replace || Logger.LastUiLine.MatchesWildcard("*Generated*image*in*"));
                }
            }

            if (TextToImage.CurrentTaskSettings != null && TextToImage.CurrentTaskSettings.Implementation == Data.Implementation.StableDiffusionOptimized)
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


            if (line.MatchesWildcard("*%|*/*[*B/s]*") && !line.Lower().Contains("it/s") && !line.Lower().Contains("s/it"))
            {
                Logger.Log($"Downloading required files... {line.Trunc(80)}", false, ellipsis);
            }

            if (line.MatchesWildcard("Added terms: *, *"))
            {
                if (line.MatchesWildcard("*, <*>"))
                    Logger.Log($"Concept keyword: {line.Split("Added terms: *, ").LastOrDefault()}", false, ellipsis);
                else
                    Logger.Log($"Concept keyword: <{line.Split("Added terms: *, ").LastOrDefault()}>", false, ellipsis);
            }

            string lastLogLines = string.Join("\n", Logger.GetSessionLogLastLines(Constants.Lognames.Sd, 6));

            if (!_hasErrored && line.Contains("CUDA out of memory"))
            {
                _hasErrored = true;
                UiUtils.ShowMessageBox($"Your GPU ran out of VRAM! Try a lower resolution.\n\n{line.Split("If reserved memory is").FirstOrDefault()}", UiUtils.MessageType.Error);
            }

            if (!_hasErrored && (line.Contains("PytorchStreamReader failed reading zip archive") || line.Contains("UnpicklingError")))
            {
                _hasErrored = true;
                UiUtils.ShowMessageBox($"Your model file seems to be damaged or incomplete!\n\n{line}", UiUtils.MessageType.Error);
            }

            if (!_hasErrored && line.Contains("usage: "))
            {
                _hasErrored = true;
                UiUtils.ShowMessageBox($"Invalid CLI syntax.", UiUtils.MessageType.Error);
            }

            if (!_hasErrored && line.Lower().Contains("illegal memory access"))
            {
                _hasErrored = true;
                UiUtils.ShowMessageBox($"Your GPU appears to be unstable! If you have an overclock enabled, please disable it!\n\n{line}", UiUtils.MessageType.Error);
            }

            if (!_hasErrored && (line.Contains("RuntimeError") || line.Contains("ImportError") || line.Contains("OSError") || line.Contains("KeyError") || line.Contains("ModuleNotFoundError")))
            {
                _hasErrored = true;
                UiUtils.ShowMessageBox($"Python Error:\n\n{line}", UiUtils.MessageType.Error);
            }

            if (_hasErrored)
                TextToImage.Cancel();
        }
    }
}
