using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using System;
using System.IO;

namespace StableDiffusionGui.Io
{
    internal class Paths
    {
        public static long SessionTimestampUnix;
        public static string SessionTimestamp;
        private static long _sessionClipboardIndex = 0;
        public static long SessionClipboardIndex { get { long num = _sessionClipboardIndex; _sessionClipboardIndex++;  return num; } }
        private static long _sessionImportIndex = 0;
        public static long SessionImportIndex { get { long num = _sessionImportIndex; _sessionImportIndex++; return num; } }

        public static void Init()
        {
            var n = DateTime.Now;
            SessionTimestampUnix = FormatUtils.GetUnixTime();
            SessionTimestamp = $"{n.Year}-{n.Month.ToString().PadLeft(2, '0')}-{n.Day.ToString().PadLeft(2, '0')}-{n.Hour.ToString().PadLeft(2, '0')}-{n.Minute.ToString().PadLeft(2, '0')}-{n.Second.ToString().PadLeft(2, '0')}";
        }

        public static string ReturnDir(string path, bool create = true, bool expand = false)
        {
            bool relativePath = path[1] != ':'; // If the second char is ':', there is a drive letter, meaning it's an absolute path
            string absPath = path;

            if (relativePath && expand)
                absPath = Path.Combine(GetExeDir(), path);

            if (create)
                Directory.CreateDirectory(absPath);

            if (expand)
                path = absPath;

            return path;
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

        public static string GetModelsPath(bool relative = true, bool create = true)
        {
            string path = Path.Combine(Constants.Dirs.Models.Root, Constants.Dirs.Models.Ckpts);
            return ReturnDir(path, create, !relative);
        }

        public static string GetVaesPath(bool relative = true, bool create = true)
        {
            string path = Path.Combine(Constants.Dirs.Models.Root, Constants.Dirs.Models.Vae);
            return ReturnDir(path, create, !relative);
        }

        public static string GetEmbeddingsPath(bool relative = true, bool create = true)
        {
            string path = Path.Combine(Constants.Dirs.Models.Root, Constants.Dirs.Models.Embeddings);
            return ReturnDir(path, create, !relative);
        }

        public static string GetLorasPath(bool relative = true, bool create = true)
        {
            string path = Path.Combine(Constants.Dirs.Models.Root, Constants.Dirs.Models.Loras);
            return ReturnDir(path, create, !relative);
        }

        public static string GetControlNetsPath(bool relative = true, bool create = true)
        {
            string path = Path.Combine(Constants.Dirs.Models.Root, Constants.Dirs.Models.Controlnets);
            return ReturnDir(path, create, !relative);
        }

        public static string GetUpscalersPath(bool relative = true, bool create = true)
        {
            string path = Path.Combine(Constants.Dirs.Models.Root, Constants.Dirs.Models.Upscalers);
            return ReturnDir(path, create, !relative);
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
            return Path.ChangeExtension($"clipboard{SessionClipboardIndex}", extension);
        }

        public static string GetClipboardPath(string extension)
        {
            return Path.Combine(GetSessionDataPath(), GetClipboardFilename(extension));
        }
    }
}
