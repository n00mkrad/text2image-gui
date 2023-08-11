using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Ui;
using System.Linq;

namespace StableDiffusionGui.Main
{
    internal class DreamboothOutputHandler
    {
        private static bool _hasErrored = false;

        public static void Start()
        {
            _hasErrored = false;
        }

        public static void Log(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return;

            Logger.Log(line, true, false, Constants.Lognames.Training);

            if (!Program.Busy)
                return;

            bool ellipsis = Logger.LastUiLine.EndsWith("...");

            bool replace = ellipsis;

            if (line.Contains("Validation sanity check"))
                Logger.Log("Validation sanity check...", false, replace);

            if (line.Contains("Training:") && line.Contains("?it/s"))
                Logger.Log($"Starting training...", false, replace);

            string lastLogLines = string.Join("\n", Logger.GetLastLines(Constants.Lognames.Training, 6));

            if (line.Contains("global_step=") && !lastLogLines.Contains("Saving"))
            {
                int step = line.Split("global_step=").LastOrDefault().Split('.').First().GetInt(true);
                int percent = (((float)step / Dreambooth.CurrentTargetSteps) * 100f).RoundToInt();

                if (percent > 0 && percent <= 100)
                    Program.MainForm.SetProgress(percent);

                string speed = line.Split(", loss=").First().Split(' ').Last();
                int remainingMs = (Dreambooth.CurrentTargetSteps - step) * FormatUtils.IterationsToMsPerIteration(speed);

                if ((Dreambooth.CurrentTargetSteps - step) > 1)
                    Logger.Log($"Training (Step {step}/{Dreambooth.CurrentTargetSteps} - {percent}%{(step >= 5 && remainingMs > 1000 ? $" - ETA: {FormatUtils.Time(remainingMs, false)}" : "")})...", false, replace);
            }

            if (line.Contains("Saving"))
                Logger.Log($"Saving checkpoint...", false, replace);

            if (line.Contains("Pruning..."))
                Logger.Log($"Pruning model...", false, replace);

            if (line.MatchesWildcard("*%|*/*[*B/s]*") && !line.Lower().Contains("it/s") && !line.Lower().Contains("s/it"))
            {
                Logger.Log($"Downloading required files... {line.Trunc(80)}", false, ellipsis);
            }

            if (!_hasErrored && line.Contains("CUDA out of memory"))
            {
                _hasErrored = true;
                UiUtils.ShowMessageBox($"Your GPU ran out of VRAM!\n\n{line.Split("If reserved memory is").FirstOrDefault()}", UiUtils.MessageType.Error);
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

            //if (_hasErrored)
            //    TextToImage.Cancel();
        }
    }
}
