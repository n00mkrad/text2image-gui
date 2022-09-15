using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StableDiffusionGui.Main
{
    internal class TextToImage
    {
        public static TtiTaskInfo CurrentTask { get; set; } = null;
        public static TtiSettings LastTaskSettings { get; set; } = null;
        public static long PreviousSeed = -1;
        public static bool Canceled = false;

        public static async Task RunTti(TtiSettings settings)
        {
            if (settings.Params["inpainting"] == "masked")
                TtiUtils.PrepareInpainting(settings.Params["initImg"], new System.Drawing.Size(settings.Params["res"].Split('x')[0].GetInt(), settings.Params["res"].Split('x')[1].GetInt()));
            else
                IoUtils.TryDeleteIfExists(Path.Combine(Paths.GetSessionDataPath(), "masked.png"));

            if (Canceled)
                return;

            await RunTti(new List<TtiSettings>() { settings });
        }

        public static async Task RunTti(List<TtiSettings> batches)
        {
            if (batches == null || batches.Count < 1)
                return;

            Program.MainForm.SetWorking(true);

            CurrentTask = new TtiTaskInfo
            {
                StartTime = DateTime.Now,
                OutDir = Config.Get(Config.Key.textboxOutPath),
                SubfoldersPerPrompt = Config.GetBool("checkboxFolderPerPrompt"),
                TargetImgCount = batches.Sum(x => x.GetTargetImgCount()),
            };

            foreach (TtiSettings s in batches)
            {
                if (s == null)
                    continue;

                if (s.Params.ContainsKey("seed"))
                    PreviousSeed = s.Params["seed"].GetLong();

                LastTaskSettings = s;
                s.Prompts = s.Prompts.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                if (!s.Prompts.Any())
                {
                    Logger.Log($"No valid prompts to run!");
                    return;
                }

                TtiUtils.WarnIfPromptLong(s.Prompts.ToList());

                PromptHistory.Add(s);

                if(batches.Count > 1)
                    Logger.Log($"Running queue entry with {s.Prompts.Length} prompt{(s.Prompts.Length != 1 ? "s" : "")}...");

                string tempOutDir = Path.Combine(Paths.GetSessionDataPath(), "out");
                IoUtils.TryDeleteIfExists(tempOutDir);
                Directory.CreateDirectory(tempOutDir);
                Directory.CreateDirectory(CurrentTask.OutDir);

                List<Task> tasks = new List<Task>();

                if (s.Implementation == Implementation.StableDiffusion)
                    tasks.Add(TtiProcess.RunStableDiffusion(s.Prompts, s.Params["initImg"], s.Params["embedding"], s.Params["initStrengths"].Replace(" ", "").Split(",").Select(x => x.GetFloat()).ToArray(),
                        s.Iterations, s.Params["steps"].GetInt(), s.Params["scales"].Replace(" ", "").Split(",").Select(x => x.GetFloat()).ToArray(), s.Params["seed"].GetLong(), s.Params["sampler"], FormatUtils.ParseSize(s.Params["res"]), bool.Parse(s.Params["seamless"]), tempOutDir));

                if (s.Implementation == Implementation.StableDiffusionOptimized)
                    tasks.Add(TtiProcess.RunStableDiffusionOptimized(s.Prompts, s.Params["initImg"], s.Params["initStrengths"].Replace(" ", "").Split(",").First().GetFloat(), s.Iterations,
                        s.Params["steps"].GetInt(), s.Params["scales"].Replace(" ", "").Split(",")[0].GetFloat(), s.Params["seed"].GetLong(), FormatUtils.ParseSize(s.Params["res"]), tempOutDir));

                tasks.Add(ImageExport.ExportLoop(tempOutDir, true));

                await Task.WhenAll(tasks);

                MainUi.Queue = MainUi.Queue.Except(new List<TtiSettings> { s }).ToList(); // Remove from queue
            }

            Done();
        }

        public enum NotifyMode { None, Ping, Notification, Both }

        public static void Done()
        {
            TimeSpan timeTaken = DateTime.Now - CurrentTask.StartTime;
            int imgCount = CurrentTask.ImgCount; // ImagePreview.SetImages(CurrentTask.OutPath, true, CurrentTask.TargetImgCount);

            if (imgCount > 0)
            {
                Logger.Log($"Done! Generated {imgCount} images in {FormatUtils.Time(timeTaken, false)}.");
            }
            else
            {
                bool logCopySuccess = OsUtils.SetClipboard(Logger.GetSessionLog("sd"));
                Logger.Log($"No images generated. {(logCopySuccess ? "Log was copied to clipboard." : "")}");
            }

            Program.MainForm.SetWorking(false);

            NotifyMode notifyMode = (NotifyMode)Config.GetInt("comboxNotify");

            if (notifyMode == NotifyMode.Both || notifyMode == NotifyMode.Ping)
                OsUtils.PlayPingSound(true);

            if (notifyMode == NotifyMode.Both || notifyMode == NotifyMode.Notification)
                OsUtils.ShowNotification("Stable Diffusion GUI", $"Image generation has finished.\nGenerated {imgCount} images in {FormatUtils.Time(timeTaken, false)}.", true);
        }

        public static void Cancel(string reason = "", bool showMsgBox = true)
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
