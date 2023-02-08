﻿using Dasync.Collections;
using Microsoft.VisualBasic.Devices;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tulpep.NotificationWindow;

namespace StableDiffusionGui.Os
{
    internal class OsUtils
    {
        public static string GetProcStdOut(Process process, bool includeStdErr = false, ProcessPriorityClass priority = ProcessPriorityClass.BelowNormal)
        {
            if (includeStdErr)
                process.StartInfo.Arguments += " 2>&1";

            process.Start();
            process.PriorityClass = priority;
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
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
            bool stayOpen = Config.Get<int>(Config.Keys.CmdDebugMode) == 2;

            if (stayOpen)
                return "/K";
            else
                return "/C";
        }

        public static bool ShowHiddenCmd()
        {
            return Config.Get<int>(Config.Keys.CmdDebugMode) > 0;
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

        public static void ShowNotification(string title, string text, bool onlyIfWindowIsInBackground = false, float timeout = 3f)
        {
            if (onlyIfWindowIsInBackground && Program.MainForm.IsInFocus())
                return;

            var popup = new PopupNotifier { TitleText = title, ContentText = text, IsRightToLeft = false };
            popup.BodyColor = System.Drawing.ColorTranslator.FromHtml("#323232");
            popup.ContentColor = System.Drawing.Color.White;
            popup.TitleColor = System.Drawing.Color.LightGray;
            popup.GradientPower = 0;
            popup.AnimationDuration = 250;
            popup.Delay = (timeout * 1000).RoundToInt();
            popup.AnimationInterval = 5;
            popup.Popup();
        }

        public static void PlayPingSound(bool onlyIfWindowIsInBackground = false)
        {
            if (onlyIfWindowIsInBackground && Program.MainForm.IsInFocus())
                return;

            Stream stream = Properties.Resources.notify;
            new System.Media.SoundPlayer(stream).Play();
        }

        public static bool SetClipboard(object data)
        {
            try
            {
                if (data == null)
                    return false;

                Clipboard.SetDataObject(data);
                return true;
            }
            catch (Exception ex)
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
        public static bool GetClipboard(out string text)
        {
            text = null;
            try
            {
                if (!Clipboard.ContainsText()) return false;
                text = Clipboard.GetText();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error getting clipboard text: {ex.Message}");
                return false;
            }
        }

        public static void SendCtrlC(int pid)
        {
            string exePath = Path.Combine(Paths.GetBinPath(), $"{Constants.Bins.WindowsKill}.exe");
            Process p = NewProcess(true, exePath);
            p.StartInfo.Arguments = $"-SIGINT {pid}";
            Logger.Log($"{Path.GetFileName(exePath)} {p.StartInfo.Arguments}", true);
            p.Start();
            p.WaitForExit();
        }

        public static string GetPathVar(IEnumerable<string> additionalPaths)
        {
            var paths = Environment.GetEnvironmentVariable("PATH").Split(';');
            List<string> newPaths = new List<string>();

            newPaths.AddRange(additionalPaths);
            newPaths.AddRange(paths.Where(x => x.Lower().Replace("\\", "/").StartsWith("c:/windows")).ToList());

            return string.Join(";", newPaths.Select(x => x.Replace("\\", "/"))) + ";";
        }

        public static void AttachOrphanHitman(Process p)
        {
            string exePath = Path.Combine(Paths.GetBinPath(), $"{Constants.Bins.OrphanHitman}.exe");
            Process hitmanProc = NewProcess(true, exePath);
            hitmanProc.StartInfo.Arguments = $"-parent-pid={Process.GetCurrentProcess().Id} -child-pid={p.Id}";
            hitmanProc.Start();
        }

        /// <summary> Uses the python package picklescan to check for malware in a pickled file </summary>
        /// <returns> If file is safe </returns>
        public static async Task<bool> ScanPickle(string path)
        {
            Process p = NewProcess(true);
            p.StartInfo.EnvironmentVariables["PATH"] = TtiUtils.GetEnvVarsSd(true, Paths.GetDataPath()).First().Value;
            p.StartInfo.Arguments = $"/C python -m picklescan -p {path.Wrap(true)}";
            string output = await Task.Run(() => GetProcStdOut(p, true));
            bool safe = output.Contains("Infected files: 0") && output.Contains("Dangerous globals: 0");
            Logger.Log($"Pickle Scanner: '{path}' safe: {safe}", true);
            return safe;
        }

        public static async Task<Dictionary<string, bool>> ScanPickles(IEnumerable<string> paths)
        {
            var scanResults = new ConcurrentBag<Tuple<string, bool>>();

            await paths.ParallelForEachAsync(async path =>
            {
                scanResults.Add(new Tuple<string, bool>(path, await ScanPickle(path)));
            }, maxDegreeOfParallelism: Environment.ProcessorCount);

            return scanResults.ToDictionary(x => x.Item1, x => x.Item2);
        }

        /// <summary> Checks if a pip package is installed </summary>
        /// <returns> True if pip package was found, False if not </returns>
        public static async Task<bool> HasPythonPackage(string pkg)
        {
            Process p = NewProcess(true);
            p.StartInfo.EnvironmentVariables["PATH"] = TtiUtils.GetEnvVarsSd(true, Paths.GetDataPath()).First().Value;
            p.StartInfo.Arguments = $"/C python -m pip show {pkg}";
            string output = await Task.Run(() => GetProcStdOut(p, true));
            return output.Contains("Name: ");
        }

        /// <summary> Lists all installed pip packages </summary>
        /// <returns> List of packages </returns>
        public static async Task<List<string>> GetPythonPkgList(bool stripVersion = true)
        {
            var list = new List<string>();
            Process p = NewProcess(true);
            p.StartInfo.EnvironmentVariables["PATH"] = TtiUtils.GetEnvVarsSd(true, Paths.GetDataPath()).First().Value;
            p.StartInfo.Arguments = $"/C python -m pip freeze";
            string output = await Task.Run(() => GetProcStdOut(p, true));
            list = output.SplitIntoLines().Where(l => !string.IsNullOrWhiteSpace(l)).ToList();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Contains("#egg="))
                    list[i] = list[i].Split("#egg=")[1].Split("&subdirectory=")[0];

                if (list[i].Contains(" @ "))
                    list[i] = list[i].Split(" @ ")[0];
            }

            if (stripVersion)
                list = list.Select(s => s.Split("==")[0]).ToList();

            return list;
        }
    }
}
