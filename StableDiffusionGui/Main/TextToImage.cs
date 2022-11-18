using StableDiffusionGui.Data;
using StableDiffusionGui.Implementations;
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
        public static TtiSettings CurrentTaskSettings { get; set; } = null;
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

            Program.MainForm.SetWorking(Program.BusyState.ImageGeneration);

            CurrentTask = new TtiTaskInfo
            {
                StartTime = DateTime.Now,
                OutDir = Config.Get(Config.Key.textboxOutPath),
                SubfoldersPerPrompt = Config.GetBool("checkboxFolderPerPrompt"),
                IgnoreWildcardsForFilenames = Config.GetBool("checkboxOutputIgnoreWildcards"),
                TargetImgCount = batches.Sum(x => x.GetTargetImgCount()),
            };

            foreach (TtiSettings s in batches)
            {
                if (s == null)
                    continue;

                if (s.Params.ContainsKey("seed"))
                    PreviousSeed = s.Params["seed"].FromJson<long>();

                CurrentTaskSettings = s;
                s.Prompts = s.Prompts.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                if (!s.Prompts.Any())
                {
                    Logger.Log($"No valid prompts to run!");
                    continue;
                }

                TtiUtils.ShowPromptWarnings(s.Prompts.ToList());

                PromptHistory.Add(s);

                if (batches.Count > 1)
                    Logger.Log($"Running queue entry with {s.Prompts.Length} prompt{(s.Prompts.Length != 1 ? "s" : "")}...");

                string tempOutDir = Path.Combine(Paths.GetSessionDataPath(), "out");
                IoUtils.TryDeleteIfExists(tempOutDir);
                Directory.CreateDirectory(tempOutDir);
                Directory.CreateDirectory(CurrentTask.OutDir);

                List<Task> tasks = new List<Task>();

                if (s.Implementation == Enums.StableDiffusion.Implementation.InvokeAi)
                    tasks.Add(TtiProcess.RunStableDiffusion(s.Prompts, s.NegativePrompt, s.Iterations, s.Params, tempOutDir));

                if (s.Implementation == Enums.StableDiffusion.Implementation.OptimizedSd)
                    tasks.Add(TtiProcess.RunStableDiffusionOpt(s.Prompts, s.Iterations, s.Params, tempOutDir));

                if (s.Implementation == Enums.StableDiffusion.Implementation.DiffusersOnnx)
                    tasks.Add(SdOnnx.Run(s.Prompts, s.NegativePrompt, s.Iterations, s.Params, tempOutDir));

                tasks.Add(ImageExport.ExportLoop(tempOutDir, CurrentTask.ImgCount, s.GetTargetImgCount(), true));

                await Task.WhenAll(tasks);

                MainUi.Queue = MainUi.Queue.Except(new List<TtiSettings> { s }).ToList(); // Remove from queue
            }

            Done();
        }

        public enum NotifyMode { None, Ping, Notification, Both }

        public static void Done()
        {
            TimeSpan timeTaken = DateTime.Now - CurrentTask.StartTime;

            if (CurrentTask.ImgCount > 0)
            {
                Logger.Log($"Done! Generated {CurrentTask.ImgCount} images in {FormatUtils.Time(timeTaken, false)}.");
            }
            else
            {
                bool logCopySuccess = OsUtils.SetClipboard(Logger.GetSessionLog(Constants.Lognames.Sd));
                Logger.Log($"No images generated. {(logCopySuccess ? "Log was copied to clipboard." : "")}");
            }

            Program.MainForm.SetWorking(Program.BusyState.Standby);

            NotifyMode notifyMode = (NotifyMode)Config.GetInt("comboxNotify");

            if (notifyMode == NotifyMode.Both || notifyMode == NotifyMode.Ping)
                OsUtils.PlayPingSound(true);

            if (notifyMode == NotifyMode.Both || notifyMode == NotifyMode.Notification)
                OsUtils.ShowNotification("Stable Diffusion GUI", $"Image generation has finished.\nGenerated {CurrentTask.ImgCount} images in {FormatUtils.Time(timeTaken, false)}.", true);

            if (Config.GetBool("checkboxUnloadModel"))
                TtiProcess.Kill();
        }

        public static void CancelManually()
        {
            Cancel("Canceled manually.", false);
        }

        public static async void Cancel(string reason = "", bool showMsgBox = true)
        {
            if (Canceled)
                return;

            Canceled = true;

            bool manual = reason.Lower().Contains("manually");
            bool forceKill = manual && InputUtils.IsHoldingShift; // Shift force-kills the process

            Logger.Log($"Canceling. Manual: {manual} - Implementation: {(CurrentTaskSettings != null ? CurrentTaskSettings.Implementation.ToString() : "None")} - Force Kill: {forceKill}", true);

            if (CurrentTaskSettings != null && CurrentTaskSettings.Implementation == Enums.StableDiffusion.Implementation.DiffusersOnnx)
                forceKill = true;

            if (!forceKill && TtiProcess.IsAiProcessRunning)
            {
                if (CurrentTaskSettings.Implementation == Enums.StableDiffusion.Implementation.OptimizedSd)
                {
                    IoUtils.TryDeleteIfExists(Path.Combine(Paths.GetSessionDataPath(), "prompts.txt"));
                    // TtiUtils.SoftCancelDreamPy();
                }

                if (CurrentTaskSettings.Implementation == Enums.StableDiffusion.Implementation.InvokeAi)
                {
                    List<string> lastLogLines = Logger.GetLastLines(Constants.Lognames.Sd, 15);

                    if (lastLogLines.Where(x => x.MatchesWildcard("*step */*") || x.Contains("error occurred")).Any()) // Only attempt a soft cancel if we've been generating anything
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
                var lines = Logger.GetLastLines(Constants.Lognames.Sd, 5);
                lines = lines.Where(x => x.MatchesRegex(@"\[(?:(?!\]\s+\[)(?:.|\n))*\]\s+\[(?:(?!\]\:)(?:.|\n))*\]\:")).ToList();
                Dictionary<string, TimeSpan> linesWithAge = new Dictionary<string, TimeSpan>();

                foreach(string line in lines)
                {
                    if (linesWithAge.ContainsKey(line))
                        continue;

                    try
                    {
                        linesWithAge.Add(line, (DateTime.Now - DateTime.ParseExact(line.Split('[')[2].Split(']')[0], "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture)));
                    }
                    catch
                    {
                        string msg = $"Failed to parse date in log line:\n{line}\n\n.Please tell the developer about this message!\nIt has been copied to the clipboard.";
                        OsUtils.SetClipboard(msg);
                        UiUtils.ShowMessageBox(msg, UiUtils.MessageType.Error, Nmkoder.Forms.MessageForm.FontSize.Big);
                    }
                }

                linesWithAge = linesWithAge.Where(x => x.Value.TotalMilliseconds >= 0).ToDictionary(p => p.Key, p => p.Value);

                if (linesWithAge.Count > 0)
                {
                    var lastLine = linesWithAge.Last();

                    if (lastLine.Value.TotalMilliseconds > 2000)
                        break;

                    bool linesChanged = !string.IsNullOrWhiteSpace(previousLastLine.Key) && lastLine.Key != previousLastLine.Key && lastLine.Value.TotalMilliseconds < 500;

                    if (linesChanged && !lastLine.Key.Contains("skipped")) // If lines changed (= still outputting), send ctrl+c again
                        TtiUtils.SoftCancelDreamPy();

                    previousLastLine = lastLine;
                }

                await Task.Delay(100);
            }

            await TtiProcess.WriteStdIn("!reset", true);
            Program.MainForm.RunBtn.Enabled = true;
        }
    }
}
