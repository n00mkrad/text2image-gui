using StableDiffusionGui.Implementations;
using StableDiffusionGui.Ui;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StableDiffusionGui.Main
{
    internal class TtiProcessOutputHandler
    {
        public static List<string> LastMessages { get { return TextToImage.LastInstance == null ? new List<string>() : TextToImage.LastInstance.LastMessages; } }
        private static List<string> _forceKillErrors = new List<string> { "RuntimeError", "ImportError", "OSError", "KeyError", "ModuleNotFoundError", "NameError" };

        public static void HandleLogGeneric(IImplementation implementation, string line, bool hasErrored = false, TextToImage.CancelMode cancelMode = TextToImage.CancelMode.SoftKill, string errMsg = "", bool forceKillOnPyErr = true)
        {
            bool ellipsis = Program.MainForm.LogText.EndsWith("...");

            if (line.MatchesWildcard("*%|*/*[*B/s]*") && !line.Lower().Contains("it/s") && !line.Lower().Contains("s/it"))
            {
                Logger.Log($"Downloading required files - {line.Trunc(80)}...", false, ellipsis);
            }

            bool trace = line.Contains("|") && (line.Contains("raise ") || line.Contains("except ")); // Log line is part of a printed stack trace

            if (!hasErrored && line.Contains("CUDA out of memory"))
            {
                hasErrored = true;
                errMsg = $"Your GPU ran out of VRAM! Try a lower resolution.\n\n{line.Split("If reserved memory is").FirstOrDefault()}";
            }

            if (!hasErrored && (line.Contains("PytorchStreamReader failed reading zip archive") || line.Contains("UnpicklingError")))
            {
                hasErrored = true;
                errMsg = $"Your model file seems to be damaged or incomplete!\n\n{line}";
            }

            if (!hasErrored && line.StartsWith("usage: "))
            {
                hasErrored = true;
                errMsg = $"Invalid CLI syntax.";
            }

            if (!hasErrored && line.Lower().Contains("illegal memory access"))
            {
                hasErrored = true;
                errMsg = $"Your GPU appears to be unstable! If you have an overclock enabled, please disable it!\n\n{line}";
            }

            if (!hasErrored && !trace && _forceKillErrors.Any(e => line.Contains(e)))
            {
                hasErrored = true;
                errMsg = $"Python Error:\n\n{line}";

                if (forceKillOnPyErr)
                    cancelMode = TextToImage.CancelMode.ForceKill;
            }

            if (hasErrored)
            {
                TextToImage.Cancel($"Error: {errMsg}", false, cancelMode);

                if (!string.IsNullOrWhiteSpace(errMsg))
                    Task.Run(() => UiUtils.ShowMessageBox(errMsg, UiUtils.MessageType.Error));
            }
        }
    }
}
