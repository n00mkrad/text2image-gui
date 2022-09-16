using Microsoft.VisualBasic.Devices;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tulpep.NotificationWindow;

namespace StableDiffusionGui.Os
{
    internal class OsUtils
    {
        public static string GetProcStdOut(Process proc, bool includeStdErr = false, ProcessPriorityClass priority = ProcessPriorityClass.BelowNormal)
        {
            if (includeStdErr)
                proc.StartInfo.Arguments += " 2>&1";

            proc.Start();
            proc.PriorityClass = priority;
            string output = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
            return output;
        }

        public static bool IsUserAdministrator()
        {
            //bool value to hold our return value
            bool isAdmin;
            WindowsIdentity user = null;
            try
            {
                //get the currently logged in user
                user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (Exception e)
            {
                Logger.Log("IsUserAdministrator() Error: " + e.Message);
                isAdmin = false;
            }
            finally
            {
                if (user != null)
                    user.Dispose();
            }
            return isAdmin;
        }

        public static Process SetStartInfo(Process proc, bool hidden, string filename = "cmd.exe")
        {
            proc.StartInfo.UseShellExecute = !hidden;
            proc.StartInfo.RedirectStandardOutput = hidden;
            proc.StartInfo.RedirectStandardError = hidden;
            proc.StartInfo.CreateNoWindow = hidden;
            proc.StartInfo.FileName = filename;
            return proc;
        }

        public static bool IsProcessHidden(Process proc)
        {
            bool defaultVal = true;

            try
            {
                if (proc == null)
                {
                    Logger.Log($"IsProcessHidden was called but proc is null, defaulting to {defaultVal}", true);
                    return defaultVal;
                }

                if (proc.HasExited)
                {
                    Logger.Log($"IsProcessHidden was called but proc has already exited, defaulting to {defaultVal}", true);
                    return defaultVal;
                }

                ProcessStartInfo si = proc.StartInfo;
                return !si.UseShellExecute && si.CreateNoWindow;
            }
            catch (Exception e)
            {
                Logger.Log($"IsProcessHidden errored, defaulting to {defaultVal}: {e.Message}", true);
                return defaultVal;
            }
        }

        public static Process NewProcess(bool hidden, string filename = "cmd.exe")
        {
            Process proc = new Process();
            return SetStartInfo(proc, hidden, filename);
        }

        public static void KillProcessTree(int pid)
        {
            ManagementObjectSearcher processSearcher = new ManagementObjectSearcher
              ("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection processCollection = processSearcher.Get();

            try
            {
                Process proc = Process.GetProcessById(pid);
                if (!proc.HasExited) proc.Kill();
            }
            catch (ArgumentException)
            {
                // Process already exited.
            }

            if (processCollection != null)
            {
                foreach (ManagementObject mo in processCollection)
                {
                    KillProcessTree(Convert.ToInt32(mo["ProcessID"])); //kill child processes(also kills childrens of childrens etc.)
                }
            }
        }

        public static string GetCmdArg()
        {
            bool stayOpen = Config.GetInt(Config.Key.cmdDebugMode) == 2;

            if (stayOpen)
                return "/K";
            else
                return "/C";
        }

        public static bool ShowHiddenCmd()
        {
            return Config.GetInt(Config.Key.cmdDebugMode) > 0;
        }

        public static bool HasNonAsciiChars(string str)
        {
            return (Encoding.UTF8.GetByteCount(str) != str.Length);
        }

        public static int GetFreeRamMb()
        {
            try
            {
                return (int)(new ComputerInfo().AvailablePhysicalMemory / 1048576);
            }
            catch
            {
                return 1000;
            }
        }

        public static string TryGetOs()
        {
            string info = "";

            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
                {
                    ManagementObjectCollection information = searcher.Get();

                    if (information != null)
                    {
                        foreach (ManagementObject obj in information)
                            info = $"{obj["Caption"]} | {obj["OSArchitecture"]}";
                    }

                    info = info.Replace("NT 5.1.2600", "XP").Replace("NT 5.2.3790", "Server 2003");
                }
            }
            catch (Exception e)
            {
                Logger.Log("TryGetOs Error: " + e.Message, true);
            }

            return info;
        }

        public static IEnumerable<Process> GetChildProcesses(Process process)
        {
            List<Process> children = new List<Process>();
            ManagementObjectSearcher mos = new ManagementObjectSearcher($"Select * From Win32_Process Where ParentProcessID={process.Id}");

            foreach (ManagementObject mo in mos.Get())
                children.Add(Process.GetProcessById(Convert.ToInt32(mo["ProcessID"])));

            return children;
        }

        public static async Task<string> GetOutputAsync(Process process, bool onlyLastLine = false)
        {
            //Logger.Log($"Getting output for {process.StartInfo.FileName} {process.StartInfo.Arguments}", true);
            //NmkdStopwatch sw = new NmkdStopwatch();

            Stopwatch timeSinceLastOutput = new Stopwatch();
            timeSinceLastOutput.Restart();

            string output = "";

            process.OutputDataReceived += (object sender, DataReceivedEventArgs e) => output += $"{e.Data}\n";
            process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) => output += $"{e.Data}\n";
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            while (!process.HasExited) await Task.Delay(50);
            while (timeSinceLastOutput.ElapsedMilliseconds < 100) await Task.Delay(50);
            output = output.Trim('\r', '\n');

            //Logger.Log($"Output (after {sw}):  {output.Replace("\r", " / ").Replace("\n", " / ").Trunc(250)}", true);

            if (onlyLastLine)
                output = output.SplitIntoLines().LastOrDefault();

            return output;
        }

        public static void Shutdown()
        {
            Process proc = NewProcess(true);
            proc.StartInfo.Arguments = "/C shutdown -s -t 0";
            proc.Start();
        }

        public static void Hibernate()
        {
            Application.SetSuspendState(PowerState.Hibernate, true, true);
        }

        public static void Sleep()
        {
            Application.SetSuspendState(PowerState.Suspend, true, true);
        }

        public static void ShowNotification(string title, string text, bool onlyIfWindowIsInBackground = false)
        {
            if (onlyIfWindowIsInBackground && Program.MainForm.IsInFocus())
                return;

            var popupNotifier = new PopupNotifier { TitleText = title, ContentText = text, IsRightToLeft = false };
            popupNotifier.BodyColor = System.Drawing.ColorTranslator.FromHtml("#323232");
            popupNotifier.ContentColor = System.Drawing.Color.White;
            popupNotifier.TitleColor = System.Drawing.Color.LightGray;
            popupNotifier.GradientPower = 0;
            popupNotifier.AnimationInterval = 5;
            popupNotifier.Popup();
        }

        public static void PlayPingSound (bool onlyIfWindowIsInBackground = false)
        {
            if (onlyIfWindowIsInBackground && Program.MainForm.IsInFocus())
                return;

            System.IO.Stream stream = Properties.Resources.notify;
            new System.Media.SoundPlayer(stream).Play();
        }

        public static bool SetClipboard (object data)
        {
            try
            {
                if (data == null)
                    return false;

                Clipboard.SetDataObject(data);
                return true;
            }
            catch(Exception ex)
            {
                Logger.Log($"Error setting clipboard data: {ex.Message}");
                return false;
            }
        }

        public static bool SetClipboard(string text)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(text))
                    return false;

                Clipboard.SetText(text);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error setting clipboard text: {ex.Message}");
                return false;
            }
        }

        public static void SendCtrlC (int pid)
        {
            string exePath = Path.Combine(Paths.GetBinPath(), "windows-kill.exe");
            Process p = NewProcess(true, exePath);
            p.StartInfo.Arguments = $"-SIGINT {pid}";
            Logger.Log($"windows-kill.exe {p.StartInfo.Arguments}", true);
            p.Start();
        }
    }
}
