using StableDiffusionGui.Io;
using StableDiffusionGui.Os;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace StableDiffusionGui.Main
{
    internal class TtiProcess
    {
        public static Process CurrentProcess;
        public static NmkdStreamWriter CurrentStdInWriter;
        public static bool ProcessExistWasIntentional = false;
        public static bool IsAiProcessRunning { get { return CurrentProcess != null && !CurrentProcess.HasExited; } }

        public static void Finish()
        {
            return;
        }

        public static string LastStartupSettings;

        /// <summary> Writes text to a CLI using stdin </summary>
        /// <returns> True if successful, False if not </returns>
        public static async Task<bool> WriteStdIn(string text, bool ignoreCanceled = false, bool newLine = true)
        {
            try
            {
                if (!ignoreCanceled && TextToImage.Canceled)
                    return false;

                if (CurrentStdInWriter == null || !CurrentStdInWriter.IsRunning)
                    return false;

                Logger.Log($"=> {text}", true);

                if (newLine)
                    await CurrentStdInWriter.Writer.WriteLineAsync(text);
                else
                    await CurrentStdInWriter.Writer.WriteAsync(text);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void Kill()
        {
            Logger.Log($"Killing current task's processes.", true);

            if (TextToImage.CurrentTask != null)
            {
                foreach (var process in TextToImage.CurrentTask.Processes)
                {
                    try
                    {
                        if (process != null && !process.HasExited)
                        {
                            if (process == CurrentProcess)
                                ProcessExistWasIntentional = true;

                            OsUtils.KillProcessTree(process.Id);
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Log($"Failed to kill process tree: {e.Message}", true);
                    }
                }
            }
        }

        public static async Task CheckStillRunning()
        {
            while (CurrentProcess != null && !CurrentProcess.HasExited)
                await Task.Delay(1);

            if (TextToImage.Canceled)
                return;

            if (ProcessExistWasIntentional)
            {
                ProcessExistWasIntentional = false;
            }
            else
            {
                string log = "...\n" + string.Join("\n", Logger.GetLastLines(Constants.Lognames.Sd, 8));
                TextToImage.Cancel($"Process has exited unexpectedly.\n\nOutput:\n{log}", true);
            }
        }
    }
}
