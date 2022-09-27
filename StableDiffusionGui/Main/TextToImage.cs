using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

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
            InpaintingUtils.PrepareInpaintingIfEnabled(settings);

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
                    continue;
                }

                TtiUtils.WarnIfPromptLong(s.Prompts.ToList());

                PromptHistory.Add(s);

                if (batches.Count > 1)
                    Logger.Log($"Running queue entry with {s.Prompts.Length} prompt{(s.Prompts.Length != 1 ? "s" : "")}...");

                string tempOutDir = Path.Combine(Paths.GetSessionDataPath(), "out");
                IoUtils.TryDeleteIfExists(tempOutDir);
                Directory.CreateDirectory(tempOutDir);
                Directory.CreateDirectory(CurrentTask.OutDir);

                List<Task> tasks = new List<Task>();

                if (s.Implementation == Implementation.StableDiffusion)
                    tasks.Add(TtiProcess.RunStableDiffusion(s.Prompts, s.Iterations, s.Params, tempOutDir));

                if (s.Implementation == Implementation.StableDiffusionOptimized)
                    tasks.Add(TtiProcess.RunStableDiffusionOpt(s.Prompts, s.Iterations, s.Params, tempOutDir));

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
                bool logCopySuccess = OsUtils.SetClipboard(Logger.GetSessionLog(Constants.SdLogFilename));
                Logger.Log($"No images generated. {(logCopySuccess ? "Log was copied to clipboard." : "")}");
            }

            Program.MainForm.SetWorking(false);

            NotifyMode notifyMode = (NotifyMode)Config.GetInt("comboxNotify");

            if (notifyMode == NotifyMode.Both || notifyMode == NotifyMode.Ping)
                OsUtils.PlayPingSound(true);

            if (notifyMode == NotifyMode.Both || notifyMode == NotifyMode.Notification)
                OsUtils.ShowNotification("Stable Diffusion GUI", $"Image generation has finished.\nGenerated {imgCount} images in {FormatUtils.Time(timeTaken, false)}.", true);
        }

        public static void CancelManually()
        {
            Cancel("Canceled manually.", false);
        }

        public static async void Cancel(string reason = "", bool showMsgBox = true)
        {
            Canceled = true;

            bool forceKill = reason.ToLower().Contains("manually") && Keyboard.Modifiers == ModifierKeys.Shift; // Shift force-kills the process

            if (!forceKill && TtiProcess.IsDreamPyRunning)
            {
                if (LastTaskSettings.Implementation == Implementation.StableDiffusionOptimized)
                {
                    IoUtils.TryDeleteIfExists(Path.Combine(Paths.GetSessionDataPath(), "prompts.txt"));
                    TtiUtils.SoftCancelDreamPy();
                }

                if (LastTaskSettings.Implementation == Implementation.StableDiffusion)
                {
                    if (Logger.GetSessionLogLastLines(Constants.SdLogFilename, 10).Where(x => x.MatchesWildcard("step */*")).Any()) // Only attempt a soft cancel if we've been generating anything
                        await WaitForDreamPyCancel();
                    else // This condition should be true if we cancel while it's still initializing, so we can just force kill the process
                        TtiProcess.Kill();
                }
            }
            else
            {
                TtiProcess.Kill();
            }

            Logger.LogIfLastLineDoesNotContainMsg("Canceled.");

            if (!string.IsNullOrWhiteSpace(reason) && showMsgBox)
                UiUtils.ShowMessageBox($"Canceled:\n\n{reason}");
        }

        public static async Task WaitForDreamPyCancel()
        {
            Program.MainForm.RunBtn.Enabled = false;
            DateTime cancelTime = DateTime.Now;
            TtiUtils.SoftCancelDreamPy();
            await Task.Delay(100);

            KeyValuePair<string, TimeSpan> previousLastLine = new KeyValuePair<string, TimeSpan>();

            while (true)
            {
                var lines = Logger.GetSessionLogLastLines(Constants.SdLogFilename, 5);
                lines = lines.Where(x => x.MatchesRegex(@"\[(?:(?!\]\s+\[)(?:.|\n))*\]\s+\[(?:(?!\]\:)(?:.|\n))*\]\:")).ToList();
                Dictionary<string, TimeSpan> linesWithAge = new Dictionary<string, TimeSpan>();

                lines.ToList().ForEach(x => linesWithAge.Add(x, (DateTime.Now - DateTime.ParseExact(x.Split('[')[2].Split(']')[0], "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture))));
                linesWithAge = linesWithAge.Where(x => x.Value.TotalMilliseconds >= 0).ToDictionary(p => p.Key, p => p.Value);

                if (linesWithAge.Where(x => linesWithAge.Last().Value.TotalMilliseconds >= 0 && x.Key.Contains("canceling")).Any())
                    break;

                if (linesWithAge.Count > 0)
                {
                    var lastLine = linesWithAge.Last();

                    if (lastLine.Value.TotalMilliseconds > 2000)
                        break;

                    if (!string.IsNullOrWhiteSpace(previousLastLine.Key) && lastLine.Key != previousLastLine.Key && lastLine.Value.TotalMilliseconds < 500) // If lines changed (= still outputting), send ctrl+c again
                        TtiUtils.SoftCancelDreamPy();

                    previousLastLine = lastLine;
                }

                await Task.Delay(100);
            }

            Program.MainForm.RunBtn.Enabled = true;
        }
    }
}
