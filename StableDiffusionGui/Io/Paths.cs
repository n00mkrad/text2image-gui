using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StableDiffusionGui.Io
{
    internal class Paths
    {
        public static string sessionTimestamp;

        public static void Init()
        {
            var n = DateTime.Now;
            sessionTimestamp = $"{n.Year}-{n.Month.ToString().PadLeft(2, '0')}-{n.Day.ToString().PadLeft(2, '0')}-{n.Hour.ToString().PadLeft(2, '0')}-{n.Minute.ToString().PadLeft(2, '0')}-{n.Second.ToString().PadLeft(2, '0')}";
        }

        public static string GetExe()
        {
            return System.Reflection.Assembly.GetEntryAssembly().GetName().CodeBase.Replace("file:///", "");
        }

        public static string GetExeDir()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
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
            string path = Path.Combine(GetDataPath(), "logs", (noSession ? "" : sessionTimestamp));
            Directory.CreateDirectory(path);
            return path;
        }

        public static string GetModelsPath()
        {
            string path = Path.Combine(GetDataPath(), "models");
            Directory.CreateDirectory(path);
            return path;
        }

        public static string GetSessionDataPath()
        {
            string path = Path.Combine(GetSessionsPath(), sessionTimestamp);
            Directory.CreateDirectory(path);
            return path;
        }

        public static string GetBinPath()
        {
            string path = Path.Combine(GetDataPath(), "bin");
            Directory.CreateDirectory(path);
            return path;
        }
    }
}
