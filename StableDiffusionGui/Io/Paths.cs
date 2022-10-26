using Newtonsoft.Json;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Io
{
    internal class Paths
    {
        public static string SessionTimestamp;

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
            string path = Path.Combine(GetDataPath(), "logs", (noSession ? "" : SessionTimestamp));
            Directory.CreateDirectory(path);
            return path;
        }

        public static string GetModelsPath(ModelType type = ModelType.Normal)
        {
            string path = "";

            if (type == ModelType.Normal)
                path = Path.Combine(GetDataPath(), "models");
            else if (type == ModelType.Vae)
                path = Path.Combine(GetDataPath(), "models", "vae");

            Directory.CreateDirectory(path);
            return path;
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

        public static List<FileInfo> GetModels(ModelType type = ModelType.Normal, string pattern = null)
        {
            pattern = pattern ?? $"*{Constants.FileExts.SdModel}";  
            List<FileInfo> list = new List<FileInfo>();

            try
            {
                List<string> mdlFolders = new List<string>() { GetModelsPath(type) };

                List<string> customModelDirsList = Config.Get($"CustomModelDirs{type}").FromJson<List<string>>();
                mdlFolders.AddRange(customModelDirsList, out mdlFolders);

                foreach (string folderPath in mdlFolders)
                    list.AddRange(IoUtils.GetFileInfosSorted(folderPath, false, pattern).ToList());

                if (type == ModelType.Normal)
                    list = list.Where(mdl => TtiUtils.ModelFilesizeValid(mdl.FullName, type)).ToList();

                return list.Distinct().OrderBy(x => x.Name).ToList();
            }
            catch (Exception ex)
            {
                Logger.Log($"Error getting models: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
            }

            return list;
        }

        public static FileInfo GetModel(string filename, bool anyExtension = false, ModelType type = ModelType.Normal)
        {
            return GetModels(type).Where(x => x.Name == filename).FirstOrDefault();
        }
    }
}
