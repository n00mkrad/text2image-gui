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
            sessionTimestamp = $"{n.Year}-{n.Month}-{n.Day}-{n.Hour}-{n.Minute}-{n.Second}-{n.Millisecond}";
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
    }
}
