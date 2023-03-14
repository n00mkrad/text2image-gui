using StableDiffusionGui.Data;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZetaLongPaths;
using static StableDiffusionGui.Main.Enums.Models;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Io
{
    internal class Models
    {

        public static List<string> GetAllModelDirs(bool includeBuiltin = true)
        {
            List<string> mdlFolders = new List<string>();

            if (includeBuiltin)
                mdlFolders.Add(Paths.GetModelsPath());

            foreach (Enums.Models.Type type in Enum.GetValues(typeof(Enums.Models.Type)).Cast<Enums.Models.Type>())
            {
                List<string> customModelDirsList = Config.Get<List<string>>($"{Config.Keys.CustomModelDirsPfx}{type}");

                if (customModelDirsList != null)
                    mdlFolders.AddRange(customModelDirsList, out mdlFolders);
            }

            return mdlFolders;
        }

        public static List<Model> GetModelsAll(bool removeUnknownModels = true)
        {
            List<Model> list = new List<Model>();

            try
            {
                var subDirs = new List<string>() { Constants.Dirs.Models.Vae, Constants.Dirs.Models.Embeddings };
                List<string> mdlFolders = GetAllModelDirs();
                var fileList = new List<ZlpFileInfo>();

                foreach (string folderPath in mdlFolders)
                {
                    var dirs = new List<string> { folderPath };
                    subDirs.ForEach(d => dirs.Add(Path.Combine(folderPath, d)));

                    foreach (string dir in dirs.Where(d => Directory.Exists(d)))
                        fileList.AddRange(IoUtils.GetFileInfosSorted(dir, false, "*.*").ToList());
                }

                list.AddRange(fileList.Select(f => new Model(f))); // Add file-based models to final list

                var dirList = new List<ZlpDirectoryInfo>();

                foreach (string folderPath in mdlFolders.Where(dir => Directory.Exists(dir)))
                {
                    var dirs = new List<string> { folderPath };
                    subDirs.ForEach(d => dirs.Add(Path.Combine(folderPath, d)));

                    foreach (string dir in dirs.Where(d => Directory.Exists(d)))
                        dirList.AddRange(Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly).Select(x => new ZlpDirectoryInfo(x)).ToList());
                }

                dirList = dirList.Where(d => IsDirDiffusersModel(d.FullName)).ToList();
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

        public static List<Model> GetModels(Enums.Models.Type type = Enums.Models.Type.Normal, Implementation implementation = Implementation.InvokeAi)
        {
            var sw = Program.Debug ? new NmkdStopwatch() : null;
            IEnumerable<Model> models = GetModelsAll();
            Format[] supportedFormats = implementation.GetInfo().SupportedModelFormats;
            models = models.Where(m => m.Type == type && supportedFormats.Contains(m.Format));
            List<Model> distinctOrderedList = models.DistinctBy(x => x.Name).OrderBy(x => x.FormatIndependentName).ToList();
            if (Program.Debug) Logger.Log($"GetModels took {sw.ElapsedMilliseconds} ms", true);
            return distinctOrderedList;
        }

        public static Model GetModel(string filename, bool anyExtension = false, Enums.Models.Type type = Enums.Models.Type.Normal, Implementation imp = Implementation.InvokeAi)
        {
            return GetModels(type, imp).Where(x => x.Name == filename).FirstOrDefault();
        }

        public static Model GetModel(List<Model> cachedModels, string filename, bool anyExtension = false, Enums.Models.Type type = Enums.Models.Type.Normal, Implementation imp = Implementation.InvokeAi)
        {
            return cachedModels.Where(x => x.Name == filename).FirstOrDefault();
        }

        public static bool IsDirDiffusersModel(string path)
        {
            var jsons = IoUtils.GetFileInfosSorted(path, false, "*.json");

            foreach (var json in jsons.Take(20))
            {
                if (IoUtils.ReadLines(json.FullName).Any(l => l.Contains("_diffusers_version")))
                    return true;
            }

            return false;
        }

        public static Format DetectModelFormat(string modelPath, bool print = true)
        {
            try
            {
                if (File.Exists(modelPath)) // Is file
                {
                    var file = new ZlpFileInfo(modelPath);

                    if (file.Length < 16 * 1024 * 1024) // Assume that a <16 MB file is not a valid model
                        return (Format)(-1);

                    if (file.FullName.Lower().EndsWith(".ckpt") || file.FullName.Lower().EndsWith(".pt"))
                        return Format.Pytorch;

                    if (file.FullName.Lower().EndsWith(".safetensors"))
                        return Format.Safetensors;
                }
                else if (Directory.Exists(modelPath)) // Is directory
                {
                    var dir = new ZlpDirectoryInfo(modelPath);

                    // List<string> subDirs = dir.GetDirectories().Select(d => d.Name).ToList();
                    // 
                    // bool diffusersStructureValid = new[] { "text_encoder", "tokenizer", "unet" }.All(d => subDirs.Contains(d));
                    // var unetDir = new ZlpDirectoryInfo(Path.Combine(dir.FullName, "unet"));
                    // bool unetValid = unetDir.Exists && IoUtils.GetDirSize(unetDir.FullName, false) >= 64 * 1024 * 1024; // Assume that a <64 MB unet file is not valid
                    string indexJsonPath = IoUtils.GetFileInfosSorted(dir.FullName, false, "*.json").OrderByDescending(f => f.Length).FirstOrDefault()?.FullName;

                    if (File.Exists(indexJsonPath))
                    {
                        var lines = File.ReadAllLines(indexJsonPath);

                        if (lines.Any(l => l.Contains("_diffusers_version")))
                        {
                            if (File.ReadAllLines(indexJsonPath).Any(l => l.Contains(@"""_class_name"": ""Onnx")))
                                return Format.DiffusersOnnx;
                            if (File.ReadAllLines(indexJsonPath).Any(l => l.Contains(@"""_class_name"":")))
                                return Format.Diffusers;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to detect model format: {ex.Message} ({modelPath})", !print);
            }

            return (Format)(-1);
        }

        public static Enums.Models.Type GetModelType(string modelPath, bool print = true)
        {
            try
            {
                string parentDirName = "";

                if (File.Exists(modelPath))
                {
                    parentDirName = new ZlpFileInfo(modelPath).Directory.Name;
                }
                else if (Directory.Exists(modelPath))
                {
                    parentDirName = new ZlpDirectoryInfo(modelPath).Parent.Name;
                }

                if (parentDirName == Constants.Dirs.Models.Vae)
                    return Enums.Models.Type.Vae;

                if (parentDirName == Constants.Dirs.Models.Embeddings)
                    return Enums.Models.Type.Embedding;

                return Enums.Models.Type.Normal;
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to detect model type: {ex.Message} ({modelPath})", !print);
            }

            return (Enums.Models.Type)(-1);
        }
    }
}
