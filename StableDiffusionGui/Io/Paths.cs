using StableDiffusionGui.Data;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZetaLongPaths;

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

        private static string ReturnDir(string path)
        {
            return Directory.CreateDirectory(path).FullName;
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
            return ReturnDir(Path.Combine(GetExeDir(), Constants.Dirs.Data));
        }

        public static string GetSessionsPath()
        {
            return ReturnDir(Path.Combine(GetDataPath(), "sessions"));
        }

        public static string GetLogPath(bool noSession = false)
        {
            return ReturnDir(Path.Combine(GetDataPath(), "logs", (noSession ? "" : SessionTimestamp)));
        }

        public static string GetModelsPath()
        {
            return ReturnDir(Path.Combine(GetExeDir(), Constants.Dirs.Models.Root, Constants.Dirs.Models.Ckpts));
        }

        public static string GetVaesPath()
        {
            return ReturnDir(Path.Combine(GetExeDir(), Constants.Dirs.Models.Root, Constants.Dirs.Models.Vae));
        }

        public static string GetEmbeddingsPath()
        {
            return ReturnDir(Path.Combine(GetExeDir(), Constants.Dirs.Models.Root, Constants.Dirs.Models.Embeddings));
        }

        public static string GetLorasPath()
        {
            return ReturnDir(Path.Combine(GetExeDir(), Constants.Dirs.Models.Root, Constants.Dirs.Models.Loras));
        }

        public static string GetSessionDataPath()
        {
            return ReturnDir(Path.Combine(GetSessionsPath(), SessionTimestamp));
        }

        public static string GetBinPath()
        {
            return ReturnDir(Path.Combine(GetDataPath(), Constants.Dirs.Bins));
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
