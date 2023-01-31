using StableDiffusionGui.Data;
using StableDiffusionGui.Implementations;
using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using StableDiffusionGui.Ui.MainFormUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

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
            Inpainting.PrepareInpaintingIfEnabled(settings);

            if (Canceled)
                return;

            await RunTti(new List<TtiSettings>() { settings });
        }

        public static async Task RunTti(List<TtiSettings> batches, bool IsPilot = false)
        {
            if (batches == null || batches.Count < 1)
                return;

            Program.SetState(Program.BusyState.ImageGeneration);

            CurrentTask = new TtiTaskInfo
            {
                StartTime = DateTime.Now,
                OutDir = Config.Get<string>(Config.Keys.OutPath),
                SubfoldersPerPrompt = Config.Get<bool>(Config.Keys.FolderPerPrompt),
                IgnoreWildcardsForFilenames = Config.Get<bool>(Config.Keys.FilenameIgnoreWildcards),
                TargetImgCount = batches.Sum(x => x.GetTargetImgCount()),
            };

            if (Config.Get<bool>(Config.Keys.FolderPerSession))
                CurrentTask.OutDir = Path.Combine(CurrentTask.OutDir, Paths.SessionTimestamp);

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

                var invalidInitImgs = s.Params.FromJson<string[]>("initImgs", new string[0]).Where(i => !File.Exists(i)).ToList();

                if (invalidInitImgs.Any())
                {
                    Logger.Log($"Missing initialization images:\n{string.Join("\n", invalidInitImgs.Select(i => Path.GetFileName(i)))}");
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

                switch (s.Implementation)
                {
                    case Implementation.InvokeAi: tasks.Add(InvokeAi.Run(s.Prompts, s.NegativePrompt, s.Iterations, s.Params, tempOutDir)); break;
                    case Implementation.OptimizedSd: tasks.Add(OptimizedSd.Run(s.Prompts, s.Iterations, s.Params, tempOutDir)); break;
                    case Implementation.DiffusersOnnx: tasks.Add(SdOnnx.Run(s.Prompts, s.NegativePrompt, s.Iterations, s.Params, tempOutDir)); break;
                    case Implementation.InstructPixToPix: tasks.Add(InstructPixToPix.Run(s.Prompts, s.NegativePrompt, s.Iterations, s.Params, tempOutDir)); break;
                }

                tasks.Add(ImageExport.ExportLoop(tempOutDir, CurrentTask.ImgCount, s.GetTargetImgCount(), true, IsPilot));

                await Task.WhenAll(tasks);

                MainUi.Queue = MainUi.Queue.Except(new List<TtiSettings> { s }).ToList(); // Remove from queue
            }

            Done();
        }

        public enum NotifyMode { None, Ping, Notification, Both }

        private static void PostProcessingVulkan()
        {
            Logger.Log("Postprocessing...");

            List<string> imagePaths = new List<string>();

            if (Config.Get<bool>(Config.Keys.UpscaleEnable))
            {
                for (int Iter = 0; CurrentTask.ImgCount > Iter; Iter++)
                {
                    string SType = Config.Get<string>(Config.Keys.UpscaleType);
                    string File = ImageViewer._currentImages[Iter];

                    Process process = new System.Diagnostics.Process();
                    ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

                    string DopCmd = Config.Get<string>(Config.Keys.UpscaleIdx).Substring(0, 1);
                    DopCmd = " -s " + (DopCmd.GetInt() + 2).ToString();

                    string cmd = Paths.GetExeDir() + @"\tools";

                    switch (SType)
                    {
                        case "Real-ESRGAN (Vulkan)":
                            {
                                cmd += @"\realesrgan\realesrgan-ncnn-vulkan.exe";
                                startInfo.FileName = cmd;
                                DopCmd += " -n realesrgan-x4plus";
                                break;
                            }

                        case "Real-ESRGAN: Anime (Vulkan)":
                            {
                                cmd += @"\realesrgan\realesrgan-ncnn-vulkan.exe";
                                startInfo.FileName = cmd;

                                DopCmd += " -n realesrgan-x4plus-anime";
                                break;
                            }
                        case "Real-SR (Vulkan)":
                            {
                                cmd += @"\realsr.exe\realsr-ncnn-vulkan.exe";
                                startInfo.FileName = cmd;
                                break;
                            }
                        case "SRMD (Vulkan)":
                            {
                                cmd += @"\srmd\srmd-ncnn-vulkan.exe";
                                startInfo.FileName = cmd;
                                break;
                            }
                        default:
                            return;
                    }

                    string OutFile = File.Substring(0, File.Length - 4) + "_upscale.png";
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.Arguments = "-i " + File + " -o " + OutFile + DopCmd;
                    process.StartInfo = startInfo;
                    process.Start();

                    imagePaths.Add(OutFile);
                    process.WaitForExit();
                }
                ImageViewer.Clear();
                ImageViewer.SetImages(imagePaths, ImageViewer.ImgShowMode.ShowLast);
                ImageViewer.Show();
            }

        }
        public static void Done()
        {
            TimeSpan timeTaken = DateTime.Now - CurrentTask.StartTime;
            Logger.Log($"Done! Generated {CurrentTask.ImgCount} images in {FormatUtils.Time(timeTaken, false)}.");

            if (CurrentTask.ImgCount == 0)
                Logger.Log($"No images generated.");

            PostProcessingVulkan();

            Program.SetState(Program.BusyState.Standby);

            NotifyMode notifyMode = (NotifyMode)Config.Get<int>(Config.Keys.NotifyModeIdx);

            if (notifyMode == NotifyMode.Both || notifyMode == NotifyMode.Ping)
                OsUtils.PlayPingSound(true);

            if (notifyMode == NotifyMode.Both || notifyMode == NotifyMode.Notification)
                OsUtils.ShowNotification("Stable Diffusion GUI", $"Image generation has finished.\nGenerated {CurrentTask.ImgCount} images in {FormatUtils.Time(timeTaken, false)}.", true);

            if (Config.Get<bool>(Config.Keys.UnloadModel))
                TtiProcess.Kill();
        }

        public static void CancelManually()
        {
            Cancel("Canceled manually.", false);
        }

        public static async void Cancel(string reason, bool showMsgBox)
        {
            if (Canceled)
                return;

            Canceled = true;

            bool manual = reason.Lower().Contains("manually.");
            bool forceKill = manual && InputUtils.IsHoldingShift; // Shift force-kills the process

            Logger.Log($"Canceling. Reason: {(string.IsNullOrWhiteSpace(reason) ? "None" : reason)} - Implementation: {(CurrentTaskSettings != null ? CurrentTaskSettings.Implementation.ToString() : "None")} - Force Kill: {forceKill}", true);

            if (CurrentTaskSettings != null && CurrentTaskSettings.Implementation != Implementation.InvokeAi)
                forceKill = true;

            if (!forceKill && TtiProcess.IsAiProcessRunning)
            {
                if (CurrentTaskSettings.Implementation == Implementation.InvokeAi)
                {
                    List<string> lastLogLines = Logger.GetLastLines(Constants.Lognames.Sd, 15);

                    if (lastLogLines.Where(x => x.MatchesWildcard("*step */*") || x.Contains("error occurred")).Any()) // Only attempt a soft cancel if we've been generating anything
                        await WaitForInvokeAiCancel();
                    else // This condition should be true if we cancel while it's still initializing, so we can just force kill the process
                        TtiProcess.Kill();
                }
            }
            else
            {
                TtiProcess.Kill();
            }

            Logger.LogIfLastLineDoesNotContainMsg(showMsgBox || manual ? "Canceled." : $"Canceled: {reason.Replace("\n", " ").Trunc(200)}");

            if (!string.IsNullOrWhiteSpace(reason) && showMsgBox)
                Task.Run(() => UiUtils.ShowMessageBox($"Canceled:\n\n{reason}"));

            if (Program.State == Program.BusyState.PostProcessing)
                Program.SetState(Program.BusyState.Standby);
        }

        public static async Task WaitForInvokeAiCancel()
        {
            Program.MainForm.runBtn.Enabled = false;
            DateTime cancelTime = DateTime.Now;
            TtiUtils.SoftCancelInvokeAi();
            await Task.Delay(100);

            KeyValuePair<string, TimeSpan> previousLastLine = new KeyValuePair<string, TimeSpan>();

            while (true)
            {
                var entries = Logger.GetLastEntries(Constants.Lognames.Sd, 5);
                Dictionary<string, TimeSpan> linesWithAge = new Dictionary<string, TimeSpan>();

                foreach (Logger.Entry entry in entries)
                    linesWithAge[entry.Message] = DateTime.Now - entry.TimeDequeue;

                linesWithAge = linesWithAge.Where(x => x.Value.TotalMilliseconds >= 0).ToDictionary(p => p.Key, p => p.Value);

                if (linesWithAge.Count > 0)
                {
                    var lastLine = linesWithAge.Last();

                    if (lastLine.Value.TotalMilliseconds > 2000)
                        break;

                    bool linesChanged = !string.IsNullOrWhiteSpace(previousLastLine.Key) && lastLine.Key != previousLastLine.Key && lastLine.Value.TotalMilliseconds < 500;

                    if (linesChanged && !lastLine.Key.Contains("skipped")) // If lines changed (= still outputting), send ctrl+c again
                        TtiUtils.SoftCancelInvokeAi();

                    previousLastLine = lastLine;
                }

                await Task.Delay(100);
            }

            await TtiProcess.WriteStdIn("!reset", true);
            Program.MainForm.runBtn.Enabled = true;
        }
    }
}
