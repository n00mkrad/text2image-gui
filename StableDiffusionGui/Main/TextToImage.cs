using StableDiffusionGui.Data;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Forms;
using StableDiffusionGui.Implementations;
using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Main
{
    internal class TextToImage
    {
        public static TtiTaskInfo CurrentTask { get; set; } = null;
        public static TtiSettings CurrentTaskSettings { get; set; } = null;
        public static Implementation LastImplementation { get; set; } = (Implementation)(-1);
        public static IImplementation LastInstance { get; set; } = null;
        public static long PreviousSeed = -1;
        public static bool Canceled = false;
        public static bool Canceling = false;
        public static bool IsRunningQueue = false;
        public static List<TtiTaskInfo> CompletedTasks = new List<TtiTaskInfo>();

        public static async Task RunTti(TtiSettings settings = null)
        {
            Program.SetState(Program.BusyState.ImageGeneration);
            CompletedTasks.Clear();
            ConfigInstance config = Config.Instance.Clone();
            bool fromQueue = settings == null;
            int iteration = 0;

            if (config.AutoDeleteImgs)
                ImageViewer.DeleteAll(false);

            do
            {
                IsRunningQueue = fromQueue;
                TtiSettings s = null;

                if (fromQueue) // Pull from queue
                    MainUi.Queue.TryDequeue(out s);
                else
                    s = settings; // Use settings

                Application.OpenForms.OfType<PromptListForm>().ToList().Where(f => f.PromptListMode == PromptListForm.ListMode.Queue).ToList().ForEach(f => f.LoadQueue());

                if (s == null)
                    continue;

                CurrentTask = new TtiTaskInfo
                {
                    StartTime = DateTime.Now,
                    OutDir = config.OutPath,
                    TargetImgCount = s.GetTargetImgCount(),
                    Config = config,
                };

                if (config.FolderPerSession)
                    CurrentTask.OutDir = Path.Combine(CurrentTask.OutDir, Paths.SessionTimestamp);

                if (!ValidateSettings(s))
                    continue;

                TtiUtils.ShowPromptWarnings(s.Prompts.ToList());

                Inpainting.PrepareInpaintingIfEnabled(s);

                if (Canceled)
                    continue;

                iteration++;
                PromptHistory.Add(s);

                if (fromQueue)
                    Logger.Log($"Running queue task {iteration} with {s.Prompts.Length} prompt{(s.Prompts.Length != 1 ? "s" : "")}, {MainUi.Queue.Count} remaining...");

                string tempOutDir = Path.Combine(Paths.GetSessionDataPath(), "out");
                IoUtils.TryDeleteIfExists(tempOutDir);
                Directory.CreateDirectory(tempOutDir);
                Directory.CreateDirectory(CurrentTask.OutDir);

                List<Task> tasks = new List<Task>();

                if (LastImplementation != s.Implementation)
                    TtiProcess.KillAll();

                LastImplementation = s.Implementation;

                switch (s.Implementation)
                {
                    case Implementation.InvokeAi: LastInstance = (IImplementation)new InvokeAi(); break;
                    case Implementation.DiffusersOnnx: LastInstance = (IImplementation)new SdOnnx(); break;
                    case Implementation.InstructPixToPix: LastInstance = (IImplementation)new InstructPixToPix(); break;
                    case Implementation.Comfy: LastInstance = new Comfy(); break;
                }

                tasks.Add(LastInstance.Run(s, tempOutDir));
                ImageExport.Init(!fromQueue || (fromQueue && iteration == 1));

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

                CompletedTasks.Add(CurrentTask);
            } while (fromQueue && MainUi.Queue.Any());

            Done();
        }

        private static bool ValidateSettings(TtiSettings s)
        {
            if (s == null)
                return false;

            CurrentTaskSettings = s;
            s.Prompts = s.Prompts.Where(x => x.IsNotEmpty()).ToArray();

            if (!s.Prompts.Any())
            {
                Logger.Log($"No valid prompts to run!");
                return false;
            }

            var invalidInitImgs = s.InitImgs.Where(i => !File.Exists(i)).ToList();

            if (invalidInitImgs.Any())
            {
                Logger.Log($"Missing base images:\n{string.Join("\n", invalidInitImgs.Select(i => Path.GetFileName(i)))}");
                return false;
            }

            if (new[] { ImgMode.ImageMask, ImgMode.InitializationImage }.Contains(s.ImgMode) && !s.Model.Lower().Contains("inpainting"))
                Logger.Log($"Warning: Inpainting/Outpainting is enabled, but '{s.Model}' does not appear to be an inpainting model. Quality will be degraded.");

            if (s.Seed >= 0)
                PreviousSeed = s.Seed;

            return true;
        }

        public enum NotifyMode { None, Ping, Notification, Both }

        public static void Done()
        {
            IsRunningQueue = false;
            TimeSpan timeTaken = DateTime.Now - CurrentTask.StartTime;

            if (CurrentTask.ImgCount > 0)
                Logger.Log($"Done! Generated {CurrentTask.ImgCount} images in {FormatUtils.Time(timeTaken, false)}.");
            else
                Logger.Log($"No images generated.");

            Program.SetState(Program.BusyState.Standby);
            Program.MainForm.runBtn.SetEnabled(true);

            NotifyMode notifyMode = (NotifyMode)Config.Instance.NotifyModeIdx;

            if (notifyMode == NotifyMode.Both || notifyMode == NotifyMode.Ping)
                OsUtils.PlayPingSound(true);

            if (notifyMode == NotifyMode.Both || notifyMode == NotifyMode.Notification)
                OsUtils.ShowNotification($"Image generation has finished.\nGenerated {CurrentTask.ImgCount} images in {FormatUtils.Time(timeTaken, false)}.", true);

            if (Config.Instance.UnloadModel)
                TtiProcess.KillAll();
        }

        public static void CancelManually()
        {
            Cancel("Canceled manually.", false);
        }

        public enum CancelMode { DoNotKill, SoftKill, ForceKill }

        public static async void Cancel(string reason, bool showMsgBox, CancelMode cancelMode = CancelMode.SoftKill)
        {
            if (Canceled || Canceling)
                return;

            Canceling = true;
            Program.MainForm.runBtn.SetEnabled(false);

            try
            {
                bool manual = reason.Lower().Contains("manually.");
                bool forceKill = manual && InputUtils.IsHoldingShift; // Shift force-kills the process

                Logger.Log($"Canceling. Reason: {(string.IsNullOrWhiteSpace(reason) ? "None" : reason)} - Implementation: {(CurrentTaskSettings != null ? CurrentTaskSettings.Implementation.ToString() : "None")} - Force Kill: {forceKill}", true);

                if (cancelMode == CancelMode.ForceKill)
                    forceKill = true;

                if (cancelMode != CancelMode.DoNotKill)
                {
                    if (!forceKill && TtiProcess.IsAiProcessRunning)
                    {
                        await LastInstance.Cancel();
                    }
                    else
                    {
                        TtiProcess.KillAll();
                    }
                }

                Logger.LogIfLastLineDoesNotContainMsg(showMsgBox || manual ? "Canceled." : $"Canceled: {reason.Replace("\n", " ").Trunc(200)}");

                if (!string.IsNullOrWhiteSpace(reason) && showMsgBox)
                    Task.Run(() => UiUtils.ShowMessageBox($"Canceled:\n\n{reason}"));

                if (Program.State == Program.BusyState.PostProcessing)
                    Program.SetState(Program.BusyState.Standby);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            Canceling = false;
            Canceled = true;
        }

        public static T GetInstance<T>() where T : class
        {
            if (LastInstance is T)
                return LastInstance as T;

            return null;
        }
    }
}
