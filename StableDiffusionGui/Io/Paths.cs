﻿using StableDiffusionGui.Data;
using StableDiffusionGui.Forms;
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
            return System.IO.Directory.GetCurrentDirectory();
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

        public static string GetModelsPath(ModelType type = ModelType.Normal, bool bCP = false)
        {
            string path = "";

            if (type == ModelType.Normal)
            {
                if (!bCP && SettingsForm.IsONNX())
                    path = Path.Combine(GetDataPath(), @"models\onnx");
                else
                    path = Path.Combine(GetDataPath(), @"models\stable-diffusion");
            }
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

        public static List<Model> GetModelsAll(bool removeUnknownModels = true, bool bCP = false)
        {
            List<Model> list = new List<Model>();

            try
            {
                List<string> mdlFolders = new List<string>() { GetModelsPath(ModelType.Normal, bCP) };

                foreach (ModelType type in Enum.GetValues(typeof(ModelType)).Cast<ModelType>())
                {
                    List<string> customModelDirsList = Config.Get<List<string>>($"{Config.Keys.CustomModelDirsPfx}{type}");

                    if (customModelDirsList != null)
                        mdlFolders.AddRange(customModelDirsList, out mdlFolders);
                }

                var fileList = new List<ZlpFileInfo>();

                foreach (string folderPath in mdlFolders)
                    fileList.AddRange(IoUtils.GetFileInfosSorted(folderPath, false, $"*.*").ToList());

                // foreach (ModelType type in Enum.GetValues(typeof(ModelType)).Cast<ModelType>())
                // {
                //     if (!Config.Get<bool>(Config.Keys.DisableModelFileValidation))
                //     {
                //         fileList = fileList.Where(f => f.).Where(f => TtiUtils.ModelFilesizeValid(f.Length, type)).ToList();
                // 
                //         // if (type == ModelType.Normal)
                //         //     fileList = fileList.Where(f => Constants.FileExts.ValidSdModels.Contains(f.Extension)).ToList();
                //         // else if (type == ModelType.Vae)
                //         //     fileList = fileList.Where(f => Constants.FileExts.ValidSdVaeModels.Contains(f.Extension)).ToList();
                //     }   
                // }

                // if (!Config.Get<bool>(Config.Keys.DisableModelFileValidation))
                //     fileList = fileList.Where(f => TtiUtils.ModelFilesizeValid(f.Length, ModelType.Normal)).ToList();

                list.AddRange(fileList.Select(f => new Model(f))); // Add file-based models to final list

                var dirList = new List<ZlpDirectoryInfo>();

                foreach (string folderPath in mdlFolders)
                    dirList.AddRange(Directory.GetDirectories(folderPath, "*", SearchOption.TopDirectoryOnly).Select(x => new ZlpDirectoryInfo(x)).ToList());

                list.AddRange(dirList.Select(f => new Model(f))); // Add folder-based models to final list
            }
            catch (Exception ex)
            {
                Logger.Log($"Error getting models: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
            }

            if (removeUnknownModels)
                list = list.Where(m => m.Format != (Enums.Models.Format)(-1)).ToList();

            return list.DistinctBy(x => x.Name).OrderBy(x => x.Name).ToList();
        }

        public static List<Model> GetModels(ModelType type = ModelType.Normal, Implementation implementation = Implementation.InvokeAi)
        {
            List<Model> list = new List<Model>();

            try
            {
                bool CudaUse = implementation == Implementation.InvokeAi || implementation == Implementation.OptimizedSd;
                List<string> mdlFolders = new List<string>() { GetModelsPath(type, CudaUse) };

                List<string> customModelDirsList = Config.Get<List<string>>($"{Config.Keys.CustomModelDirsPfx}{type}");

                if (customModelDirsList != null)
                    mdlFolders.AddRange(customModelDirsList, out mdlFolders);

                if (CudaUse)
                {
                    var fileList = new List<ZlpFileInfo>();

                    foreach (string folderPath in mdlFolders)
                        fileList.AddRange(IoUtils.GetFileInfosSorted(folderPath, false, $"*.*").ToList());

                    if (!Config.Get<bool>(Config.Keys.DisableModelFileValidation))
                    {
                        fileList = fileList.Where(f => TtiUtils.ModelFilesizeValid(f.Length, type)).ToList();

                        if (type == ModelType.Normal)
                            fileList = fileList.Where(f => Constants.FileExts.ValidSdModels.Contains(f.Extension)).ToList();
                        else if (type == ModelType.Vae)
                            fileList = fileList.Where(f => Constants.FileExts.ValidSdVaeModels.Contains(f.Extension)).ToList();
                    }

                    list = fileList.Select(f => new Model(f, new[] { Implementation.InvokeAi, Implementation.OptimizedSd })).ToList();
                }
                else if (implementation == Implementation.DiffusersOnnx)
                {
                    var dirList = new List<ZlpDirectoryInfo>();

                    foreach (string folderPath in mdlFolders)
                        dirList.AddRange(Directory.GetDirectories(folderPath, "*", SearchOption.TopDirectoryOnly).Select(x => new ZlpDirectoryInfo(x)).ToList());

                    foreach (ZlpDirectoryInfo dir in new List<ZlpDirectoryInfo>(dirList)) // Filter for valid model folders
                    {
                        List<string> subDirs = dir.GetDirectories().Select(d => d.Name).ToList();

                        if (!(new[] { "unet", "text_encoder", "vae_decoder", "vae_encoder", "tokenizer" }.All(d => subDirs.Contains(d))))
                            dirList.Remove(dir);
                    }

                    list = dirList.Select(f => new Model(f, new[] { Implementation.DiffusersOnnx })).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Error getting models: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
            }

            return list.DistinctBy(x => x.Name).OrderBy(x => x.Name).ToList();
        }


        public static Model GetModel(string filename, bool anyExtension = false, ModelType type = ModelType.Normal, Implementation imp = Implementation.InvokeAi)
        {
            return GetModels(type, imp).Where(x => x.Name == filename).FirstOrDefault();
        }

        public static Model GetModel(List<Model> cachedModels, string filename, bool anyExtension = false, ModelType type = ModelType.Normal, Implementation imp = Implementation.InvokeAi)
        {
            return cachedModels.Where(x => x.Name == filename).FirstOrDefault();
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
