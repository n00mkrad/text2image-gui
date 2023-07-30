using StableDiffusionGui.Io;
using StableDiffusionGui.Os;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StableDiffusionGui.Main
{
    internal class TtiProcess
    {
        public static List<Process> AllProcesses = new List<Process>(); // TODO: Track all started processes here
        private static Process _currentProcess;
        public static Process CurrentProcess { get { return _currentProcess; } set { _currentProcess = value; AllProcesses.Add(value); } }
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
        public static async Task<bool> WriteStdIn(string text, int blockTimeMs = 0, bool ignoreCanceled = false, bool newLine = true)
        {
            try
            {
                if (!ignoreCanceled && (TextToImage.Canceled || TextToImage.Canceling))
                    return false;

                if (CurrentStdInWriter == null || !CurrentStdInWriter.IsRunning)
                    return false;

                if (Config.Instance.LogStdin)
                    Logger.Log($"=> {text}", true);

                if (newLine)
                    await CurrentStdInWriter.Writer.WriteLineAsync(text);
                else
                    await CurrentStdInWriter.Writer.WriteAsync(text);

                if (blockTimeMs > 0)
                    await Task.Delay(blockTimeMs);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void KillAll ()
        {
            AllProcesses = AllProcesses.Where(p => p != null && !p.HasExited).ToList();
            Kill(AllProcesses);
        }

        public static void Kill(List<Process> overrideProcessList = null)
        {
            Logger.Log($"Killing processes.", true);
            var procList = new List<Process>();

            if(overrideProcessList != null)
            {
                procList = overrideProcessList;
            }
            else if (TextToImage.CurrentTask != null)
            {
                procList = TextToImage.CurrentTask.Processes;
            }

            foreach (Process process in procList)
            {
                try
                {
                    if (process != null && !process.HasExited)
                    {
                        if (process == CurrentProcess)
                            ProcessExistWasIntentional = true;

                        OsUtils.KillProcessTree(process.Id);
                        Logger.Log($"Killed proc tree with PID {process.Id}", true);
                    }
                }
                catch (Exception e)
                {
                    Logger.Log($"Failed to kill process tree: {e.Message}", true);
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
