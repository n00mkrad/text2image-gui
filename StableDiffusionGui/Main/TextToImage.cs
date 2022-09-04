using HTAlt.WinForms;
using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Main
{
    internal class TextToImage
    {
        public static TtiTaskInfo CurrentTask { get; set; } = null;
        public static TtiSettings LastTaskSettings { get; set; } = null;
        public static bool Canceled = false;

        public static async Task RunTti(TtiSettings s)
        {
            if (s == null)
                return;

                LastTaskSettings = s;
            s.Prompts = s.Prompts.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            if (!s.Prompts.Any())
            {
                Logger.Log($"No valid prompts to run!");
                return;
            }

            CurrentTask = new TtiTaskInfo
            {
                StartTime = DateTime.Now,
                OutPath = s.OurDir,
                SubfoldersPerPrompt = Config.GetBool("checkboxFolderPerPrompt"),
            };

            Program.MainForm.SetWorking(true);

            string tempOutDir = Path.Combine(Paths.GetSessionDataPath(), "out");
            Directory.CreateDirectory(tempOutDir);

            List<Task> tasks = new List<Task>();

            if (s.Implementation == Implementation.StableDiffusion)
                tasks.Add(TtiProcess.RunStableDiffusion(s.Prompts, s.Params["initImg"], s.Params["embedding"], s.Params["initStrengths"].Replace(" ", "").Split(",").Select(x => x.GetFloat()).ToArray(),
                    s.Iterations, s.Params["steps"].GetInt(), s.Params["scales"].Replace(" ", "").Split(",").Select(x => x.GetFloat()).ToArray(), s.Params["seed"].GetLong(), s.Params["sampler"], FormatUtils.ParseSize(s.Params["res"]), tempOutDir));

            if (s.Implementation == Implementation.StableDiffusionOptimized)
                tasks.Add(TtiProcess.RunStableDiffusionOptimized(s.Prompts, s.Params["initImg"], s.Params["initStrengths"].Replace(" ", "").Split(",").First().GetFloat(), s.Iterations,
                    s.Params["steps"].GetInt(), s.Params["scales"].Replace(" ", "").Split(",")[0].GetFloat(), s.Params["seed"].GetLong(), FormatUtils.ParseSize(s.Params["res"]), tempOutDir));

            tasks.Add(PostProcess.PostProcLoop(tempOutDir, true));

            await Task.WhenAll(tasks);
            Done();
        }

        public static void Done ()
        {
            int imgCount = CurrentTask.ImgCount; // ImagePreview.SetImages(CurrentTask.OutPath, true, CurrentTask.TargetImgCount);

            if (imgCount > 0)
            {
                Logger.Log($"Done!");
            }
            else
            {
                bool logCopySuccess = OsUtils.SetClipboard(Logger.GetSessionLog("sd"));
                Logger.Log($"No images generated. {(logCopySuccess ? "Log was copied to clipboard." : "")}");
            }

            Program.MainForm.SetWorking(false);
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
