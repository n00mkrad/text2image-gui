﻿using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

[assembly: System.Windows.Media.DisableDpiAwareness] // Disable Dpi awareness in the application assembly.

namespace StableDiffusionGui
{
    public static class Program
    {
        public const string Version = "1.9.1: ForserX Edition for AMD GPU";
        public const Enums.Program.UpdateChannel ReleaseChannel = Enums.Program.UpdateChannel.Public;

        public static bool Debug { get { return Debugger.IsAttached || UserArgs.Get("debug").Lower() == true.ToString().Lower(); } }

        public static List<string> Args = new List<string>(); // All args
        public static Dictionary<string, string> UserArgs = new Dictionary<string, string>(); // User args (excludes 1st which is the path) as key/value pairs

        public static NmkdStopwatch SwTimeSinceProgramStart = new NmkdStopwatch();

        public enum BusyState { Standby, Installation, ImageGeneration, Script, Dreambooth, PostProcessing, Other }
        public static BusyState State = BusyState.Standby;
        public static bool Busy { get { return State != BusyState.Standby; } }
        public static MainForm MainForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Logger.Log("Starting up.", true);
            HandleArgs(args);
            Config.Init();
            Paths.Init();

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnApplicationExit);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Cleanup();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        static void HandleArgs(string[] args)
        {
            Args = args.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

            foreach (string arg in Args)
            {
                try
                {
                    if (!(arg.StartsWith("-") && arg.Length > 1 && arg[1] != '-')) // Required arg syntax: Starts with hypen, no double hyphen, more than 1 char
                        continue;

                    var split = arg.Substring(1).Split('=').Take(2).ToArray(); // Split into key+value, ignore if more than one '='
                    UserArgs.Add(split[0], split[1]); // Add key+value
                }
                catch { }
            }
        }

        public static void SetState(BusyState state)
        {
            Logger.Log($"SetState({state})", true);
            State = state;
            Ui.MainFormUtils.FormUtils.UpdateBusyState();
        }

        public static void Cleanup()
        {
            int keepLogsDays = 5;
            int keepSessionDataDays = 2;

            try
            {
                foreach (DirectoryInfo dir in new DirectoryInfo(Paths.GetLogPath(true)).GetDirectories())
                {
                    string[] split = dir.Name.Split('-');
                    int daysOld = (DateTime.Now - new DateTime(split[0].GetInt(), split[1].GetInt(), split[2].GetInt())).Days;
                    int fileCount = dir.GetFiles("*", SearchOption.AllDirectories).Length;

                    if (daysOld > keepLogsDays || fileCount < 1) // Delete old logs
                    {
                        Logger.Log($"Cleanup: Log folder {dir.Name} is {daysOld} days old and has {fileCount} files - Will Delete", true);
                        IoUtils.TryDeleteIfExists(dir.FullName);
                    }
                }

                IoUtils.DeleteContentsOfDir(Paths.GetSessionDataPath()); // Clear this session's temp files...

                foreach (DirectoryInfo dir in new DirectoryInfo(Paths.GetSessionsPath()).GetDirectories())
                {
                    string[] split = dir.Name.Split('-');
                    int daysOld = (DateTime.Now - new DateTime(split[0].GetInt(), split[1].GetInt(), split[2].GetInt())).Days;
                    int fileCount = dir.GetFiles("*", SearchOption.AllDirectories).Length;

                    if (daysOld > keepSessionDataDays || fileCount < 1) // Delete old temp files
                    {
                        Logger.Log($"Cleanup: Session folder {dir.Name} is {daysOld} days old and has {fileCount} files - Will Delete", true);
                        IoUtils.TryDeleteIfExists(dir.FullName);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log($"Cleanup Error: {e.Message}\n{e.StackTrace}");
            }
        }

        private static void OnApplicationExit(object sender, EventArgs e)
        {
            // ...
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            ShowUnhandledError($"Unhandled Thread Exception!\n\n{e.Exception.Message}\n\nStack Trace:\n{e.Exception.StackTrace}");
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ShowUnhandledError($"Unhandled Exception!\n\n{((Exception)e.ExceptionObject).Message}\n\nStack Trace:\n{((Exception)e.ExceptionObject).StackTrace}");
        }

        static void ShowUnhandledError(string text)
        {
            Clipboard.SetText(text);
            Logger.Log($"Unhandled Error:\n{text}", true);
            text += "\n\n\nThe error has been copied to the clipboard. Please inform the developer about this.";
            UiUtils.ShowMessageBox(text, UiUtils.MessageType.Error);
        }
    }
}
