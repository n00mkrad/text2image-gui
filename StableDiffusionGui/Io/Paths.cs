using StableDiffusionGui.Data;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZetaLongPaths;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Io
{
    internal class Paths
    {
        public static string SessionTimestamp;
        public static long SessionClipboardIndex = 0;

        public static void Init()
        {
            var n = DateTime.Now;
            SessionTimestamp = $"{n.Year}-{n.Month.ToString().PadLeft(2, '0')}-{n.Day.ToString().PadLeft(2, '0')}-{n.Hour.ToString().PadLeft(2, '0')}-{n.Minute.ToString().PadLeft(2, '0')}-{n.Second.ToString().PadLeft(2, '0')}";
        }

        public static string GetExe()
        {
            return System.Reflection.Assembly.GetEntryAssembly().GetName().CodeBase.Replace("file:///", "");
        }

        public static string GetExeDir()
        {
            return new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).FullName;
        }

        public static string GetDataPath()
        {
            string path = Path.Combine(GetExeDir(), "Data");
            Directory.CreateDirectory(path);
            return path;
        }

        public static string GetSessionsPath()
        {
            string path = Path.Combine(GetDataPath(), "sessions");
            Directory.CreateDirectory(path);
            return path;
        }

        public static string GetLogPath(bool noSession = false)
        {
            string path = Path.Combine(GetDataPath(), "logs", (noSession ? "" : SessionTimestamp));
            Directory.CreateDirectory(path);
            return path;
        }

        public static string GetModelsPath()
        {
            string path = Path.Combine(GetDataPath(), Constants.Dirs.Models.Root);
            return Directory.CreateDirectory(path).FullName;
        }

        public static string GetSessionDataPath()
        {
            string path = Path.Combine(GetSessionsPath(), SessionTimestamp);
            Directory.CreateDirectory(path);
            return path;
        }

        public static string GetBinPath()
        {
            string path = Path.Combine(GetDataPath(), Constants.Dirs.Bins);
            Directory.CreateDirectory(path);
            return path;
        }

        public static string GetClipboardFilename(string extension)
        {
            string filename = Path.ChangeExtension($"clipboard{SessionClipboardIndex}", extension);
            SessionClipboardIndex++;
            return filename;
        }

        public static string GetClipboardPath(string extension)
        {
            return Path.Combine(GetSessionDataPath(), GetClipboardFilename(extension));
        }
    }
}
