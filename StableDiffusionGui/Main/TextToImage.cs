using StableDiffusionGui.Data;
using StableDiffusionGui.Implementations;
using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Main
{
    internal class TextToImage
    {
        public static TtiTaskInfo CurrentTask { get; set; } = null;
        public static TtiSettings CurrentTaskSettings { get; set; } = null;
        public static Implementation LastImplementation { get; set; } = (Implementation)(-1);
        public static long PreviousSeed = -1;
        public static bool Canceled = false;

        public static async Task RunTti(TtiSettings settings)
        {
            Inpainting.PrepareInpaintingIfEnabled(settings);

            if (Canceled)
                return;

            await RunTti(new List<TtiSettings>() { settings });
        }

        public static async Task RunTti(List<TtiSettings> batches)
        {
            if (batches == null || batches.Count < 1)
                return;

            Program.SetState(Program.BusyState.ImageGeneration);

            CurrentTask = new TtiTaskInfo
            {
                StartTime = DateTime.Now,
                OutDir = Config.Instance.OutPath,
                SubfoldersPerPrompt = Config.Instance.FolderPerPrompt,
                IgnoreWildcardsForFilenames = Config.Instance.FilenameIgnoreWildcards,
                TargetImgCount = batches.Sum(x => x.GetTargetImgCount()),
            };

            if (Config.Instance.FolderPerSession)
                CurrentTask.OutDir = Path.Combine(CurrentTask.OutDir, Paths.SessionTimestamp);

            foreach (TtiSettings s in batches)
            {
                if (!ValidateSettings(s))
                    continue;

                TtiUtils.ShowPromptWarnings(s.Prompts.ToList());

                PromptHistory.Add(s);

                if (batches.Count > 1)
                    Logger.Log($"Running queue entry with {s.Prompts.Length} prompt{(s.Prompts.Length != 1 ? "s" : "")}...");

                string tempOutDir = Path.Combine(Paths.GetSessionDataPath(), "out");
                IoUtils.TryDeleteIfExists(tempOutDir);
                Directory.CreateDirectory(tempOutDir);
                Directory.CreateDirectory(CurrentTask.OutDir);

                List<Task> tasks = new List<Task>();

                if (LastImplementation != s.Implementation)
                    TtiProcess.Kill();

                LastImplementation = s.Implementation;

                switch (s.Implementation)
                {
                    case Implementation.InvokeAi: tasks.Add(InvokeAi.Run(s.Prompts, s.NegativePrompt, s.Iterations, s.Params, tempOutDir)); break;
                    case Implementation.OptimizedSd: tasks.Add(OptimizedSd.Run(s.Prompts, s.Iterations, s.Params, tempOutDir)); break;
                    case Implementation.DiffusersOnnx: tasks.Add(SdOnnx.Run(s.Prompts, s.NegativePrompt, s.Iterations, s.Params, tempOutDir)); break;
                    case Implementation.InstructPixToPix: tasks.Add(InstructPixToPix.Run(s.Prompts, s.NegativePrompt, s.Iterations, s.Params, tempOutDir)); break;
                }

                ImageExport.Init();

                if (s.Implementation != Implementation.InvokeAi)
                {
                    tasks.Add(ImageExport.ExportLoop(tempOutDir, CurrentTask.ImgCount, s.GetTargetImgCount()));
                    await Task.WhenAll(tasks);
                }
                else
                {
                    await Task.WhenAll(tasks);
                    int targetImgCount = s.GetTargetImgCount();

                    while (!Canceled && CurrentTask.ImgCount < targetImgCount)
                        await Task.Delay(100);
                }
               
                MainUi.Queue = MainUi.Queue.Except(new List<TtiSettings> { s }).ToList(); // Remove from queue
            }

            Done();
        }

        private static bool ValidateSettings(TtiSettings s)
        {
            if (s == null)
                return false;

            if (s.Params.ContainsKey("seed"))
                PreviousSeed = s.Params["seed"].FromJson<long>();

            CurrentTaskSettings = s;
            s.Prompts = s.Prompts.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            if (!s.Prompts.Any())
            {
                Logger.Log($"No valid prompts to run!");
                return false;
            }

            var invalidInitImgs = s.Params.FromJson<string[]>("initImgs", new string[0]).Where(i => !File.Exists(i)).ToList();

            if (invalidInitImgs.Any())
            {
                Logger.Log($"Missing initialization images:\n{string.Join("\n", invalidInitImgs.Select(i => Path.GetFileName(i)))}");
                return false;
            }

            string mdl = s.Params.FromJson<string>("model");

            if (s.Params.FromJson<InpaintMode>("inpainting") != InpaintMode.Disabled && !mdl.Contains("inpainting"))
                Logger.Log($"Warning: Inpainting is enabled, but '{mdl}' does not appear to be an inpainting model. Quality will be degraded.");

            return true;
        }

        public enum NotifyMode { None, Ping, Notification, Both }

        public static void Done()
        {
            TimeSpan timeTaken = DateTime.Now - CurrentTask.StartTime;

            if (CurrentTask.ImgCount > 0)
                Logger.Log($"Done! Generated {CurrentTask.ImgCount} images in {FormatUtils.Time(timeTaken, false)}.");
            else
                Logger.Log($"No images generated.");

            Program.SetState(Program.BusyState.Standby);

            NotifyMode notifyMode = (NotifyMode)Config.Instance.NotifyModeIdx;

            if (notifyMode == NotifyMode.Both || notifyMode == NotifyMode.Ping)
                OsUtils.PlayPingSound(true);

            if (notifyMode == NotifyMode.Both || notifyMode == NotifyMode.Notification)
                OsUtils.ShowNotification("Stable Diffusion GUI", $"Image generation has finished.\nGenerated {CurrentTask.ImgCount} images in {FormatUtils.Time(timeTaken, false)}.", true);

            if (Config.Instance.UnloadModel)
                TtiProcess.Kill();
        }

        public static void CancelManually()
        {
            Cancel("Canceled manually.", false);
        }

        public enum CancelMode { DoNotKill, SoftKill, ForceKill }

        public static async void Cancel(string reason, bool showMsgBox, CancelMode cancelMode = CancelMode.SoftKill)
        {
            if (Canceled)
                return;

            Canceled = true;

            bool manual = reason.Lower().Contains("manually.");
            bool forceKill = manual && InputUtils.IsHoldingShift; // Shift force-kills the process

            Logger.Log($"Canceling. Reason: {(string.IsNullOrWhiteSpace(reason) ? "None" : reason)} - Implementation: {(CurrentTaskSettings != null ? CurrentTaskSettings.Implementation.ToString() : "None")} - Force Kill: {forceKill}", true);

            if (cancelMode == CancelMode.ForceKill || (CurrentTaskSettings != null && !CurrentTaskSettings.Implementation.Supports(ImplementationInfo.Feature.InteractiveCli)))
                forceKill = true;

            if (cancelMode != CancelMode.DoNotKill)
            {
                if (!forceKill && TtiProcess.IsAiProcessRunning)
                {
                    if (CurrentTaskSettings.Implementation == Implementation.InvokeAi)
                        await InvokeAi.Cancel(); // TODO: Make an interface IImplementation to avoid duplicate lines for each implementation

                    if (CurrentTaskSettings.Implementation == Implementation.InstructPixToPix)
                        await InstructPixToPix.Cancel(); // TODO: Make an interface IImplementation to avoid duplicate lines for each implementation
                }
                else
                {
                    TtiProcess.Kill();
                }
            }

            Logger.LogIfLastLineDoesNotContainMsg(showMsgBox || manual ? "Canceled." : $"Canceled: {reason.Replace("\n", " ").Trunc(200)}");

            if (!string.IsNullOrWhiteSpace(reason) && showMsgBox)
                Task.Run(() => UiUtils.ShowMessageBox($"Canceled:\n\n{reason}"));

            if (Program.State == Program.BusyState.PostProcessing)
                Program.SetState(Program.BusyState.Standby);
        }
    }
}
