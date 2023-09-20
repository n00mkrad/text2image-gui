using Dasync.Collections;
using StableDiffusionGui.Forms;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

[assembly: System.Windows.Media.DisableDpiAwareness] // Disable Dpi awareness in the application assembly.

namespace StableDiffusionGui
{
    public static class Program
    {
        public const string Version = "1.13.0b1";
        public const Enums.Program.UpdateChannel ReleaseChannel = Enums.Program.UpdateChannel.Beta;

        public static bool Debug { get { return Debugger.IsAttached || UserArgs.Get("debug").Lower() == true.ToString().Lower(); } }

        public static List<string> Args = new List<string>(); // All args
        public static Dictionary<string, string> UserArgs = new Dictionary<string, string>(); // User args (excludes 1st which is the path) as key/value pairs

        public enum BusyState { Standby, Installation, ImageGeneration, Script, Training, PostProcessing, Other }
        public static BusyState State = BusyState.Standby;
        public static bool Busy { get { return State != BusyState.Standby; } }
        public static MainForm MainForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Logger.Log($"Starting up [{Version}]", true);

            var defaultCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = defaultCulture;
            Thread.CurrentThread.CurrentUICulture = defaultCulture;
            CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
            CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

            HandleArgs(args);
            Config.Init();
            Paths.Init();

            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            IoUtils.Cleanup();

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

            if (UserArgs.Any())
            {
                Logger.Log($"Args:", true);

                foreach (var a in UserArgs)
                    Logger.Log($"-{a.Key}={a.Value}", true);
            }
        }

        public static void SetState(BusyState state)
        {
            Logger.Log($"SetState({state})", true);
            State = state;
            MainForm?.UpdateBusyState();
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            // Ignore dispose stuff
            if (e.Exception.Message.Contains("Dispose() cannot be called while"))
            {
                Logger.Log($"{e.Exception.Message}\n{e.Exception.StackTrace}", true);
                return;
            }

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
