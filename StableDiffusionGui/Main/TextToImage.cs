using HTAlt.WinForms;
using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StableDiffusionGui.Main
{
    internal class TextToImage
    {
        public static TtiTaskInfo CurrentTask { get; set; } = null;
        public static bool Canceled = false;

        public static async Task RunTti(TtiSettings s)
        {
            if (s.Implementation == Implementation.StableDiffusion)
                await TtiProcess.RunStableDiffusion(s.Prompts, s.Iterations, s.Params["steps"].GetInt(), s.Params["scales"].Replace(" ", "").Split(",").Select(x => x.GetFloat()).ToArray(), s.Params["seed"].GetLong(), s.Params["sampler"], FormatUtils.ParseSize(s.Params["res"]), s.OutPath);
        }

        public static void Cancel (string reason = "", bool showMsgBox = true)
        {
            Canceled = true;
            Program.MainForm.SetProgress(0);
            Program.MainForm.SetWorking(false);

            TtiProcess.Kill();

            Logger.LogIfLastLineDoesNotContainMsg("Canceled.");

            if (!string.IsNullOrWhiteSpace(reason) && showMsgBox)
                UiUtils.ShowMessageBox($"Canceled:\n\n{reason}");
        }
    }
}
