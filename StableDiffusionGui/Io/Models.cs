using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StableDiffusionGui.Data;
using StableDiffusionGui.Implementations;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using ZetaLongPaths;
using static StableDiffusionGui.Main.Enums.Models;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Io
{
    public class Models
    {
        public class ModelSettings
        {
            public ModelArch Arch { get; set; } = ModelArch.Automatic;
            public int ClipSkip { get; set; } = 0;
        }


        public static List<string> GetAllModelDirs(bool includeBuiltin = true)
        {
            List<string> mdlFolders = new List<string>();

            if (includeBuiltin)
                mdlFolders.Add(Paths.GetModelsPath(false));

            mdlFolders = mdlFolders.Concat(Config.Instance.CustomModelDirs).ToList();
            return mdlFolders;
        }

        public static List<Model> GetVaes()
        {
            List<string> dirs = new List<string>(Config.Instance.CustomVaeDirs);
            dirs.Insert(0, Paths.GetVaesPath(false));
            return GetModelsAll(true, dirs, Enums.Models.Type.Vae);
        }

        public static List<Model> GetEmbeddings()
        {
            string path = Config.Instance == null ? Paths.GetEmbeddingsPath() : Config.Instance.EmbeddingsDir;
            var fileList = IoUtils.GetFileInfosSorted(path, false, "*.*pt").Where(f => f.Length < 1024 * 1024);
            return fileList.Select(f => new Model(f, Format.Pytorch, Enums.Models.Type.Embedding)).ToList();
        }

        public static List<Model> GetLoras()
        {
            string path = Config.Instance == null ? Paths.GetLorasPath() : Config.Instance.LorasDir;
            var fileList = IoUtils.GetFileInfosSorted(path, false, "*.safetensors");
            return fileList.Select(f => new Model(f, Format.Safetensors, Enums.Models.Type.Lora)).ToList();
        }

        public static List<Model> GetControlNets()
        {
            string path = Config.Instance == null ? Paths.GetControlNetsPath() : Config.Instance.ControlNetsDir;
            path = IoUtils.EnsureAbsPath(path);
            var fileList = IoUtils.GetFileInfosSorted(path, false, "*.*").ToList();
            var validExts = new[] { ".safetensors", ".pth", ".bin" };
            fileList = fileList.Where(f => validExts.Contains(f.Extension.Lower())).ToList();
            return fileList.Select(f => new Model(f, (Format)(-1), Enums.Models.Type.ControlNet)).ToList();
        }

        public static List<Model> GetUpscalers()
        {
            string path = Config.Instance == null ? Paths.GetUpscalersPath() : Config.Instance.UpscalersDir;
            path = IoUtils.EnsureAbsPath(path);
            var validExts = new[] { ".ckpt", ".pt", ".bin", ".pth", ".safetensors" };
            var fileList = IoUtils.GetFileInfosSorted(path, false, "*.*").Where(f => validExts.Contains(f.Extension.Lower()));
            return fileList.Select(f => new Model(f, (Format)(-1), Enums.Models.Type.Upscaler)).ToList();
        }

        public static List<Model> GetModelsAll(bool removeUnknownModels = true, List<string> overridePaths = null, Enums.Models.Type overrideType = (Enums.Models.Type)(-1))
        {
            List<Model> list = new List<Model>();

            try
            {
                List<string> mdlFolders = overridePaths != null ? overridePaths : GetAllModelDirs();

                var fileList = new List<ZlpFileInfo>();

                foreach (string folderPath in mdlFolders)
                {
                    var dirs = new List<string> { folderPath };

                    foreach (string dir in dirs.Where(d => Directory.Exists(d)))
                        fileList.AddRange(IoUtils.GetFileInfosSorted(dir, false, "*.*").ToList());
                }

                list.AddRange(fileList.Select(f => new Model(f, type: overrideType))); // Add file-based models to final list

                var dirList = new List<ZlpDirectoryInfo>();

                foreach (string folderPath in mdlFolders.Where(dir => Directory.Exists(dir)))
                {
                    var dirs = new List<string> { folderPath };

                    foreach (string dir in dirs.Where(d => Directory.Exists(d)))
                        dirList.AddRange(Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly).Select(x => new ZlpDirectoryInfo(x)).ToList());
                }

                dirList = dirList.Where(d => IsDirDiffusersModel(d.FullName)).ToList();
                list.AddRange(dirList.Select(f => new Model(f, type: overrideType))); // Add folder-based models to final list
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
            models = models.Where(m => supportedFormats.Contains(m.Format));

            if (type != (Enums.Models.Type)(-1))
                models = models.Where(m => m.Type == type);

            List<Model> distinctOrderedList = models.DistinctBy(x => x.Name).OrderBy(x => x.FormatIndependentName).ToList();
            if (Program.Debug) Logger.Log($"GetModels took {sw.ElapsedMilliseconds} ms", true);
            return distinctOrderedList;
        }

        public static Model GetModel(List<Model> cachedModels, string filename, Enums.Models.Type type = Enums.Models.Type.Normal, Implementation imp = Implementation.InvokeAi)
        {
            Format[] supportedFormats = imp.GetInfo().SupportedModelFormats;

            if (type == (Enums.Models.Type)(-1))
                return cachedModels.Where(m => m.Name == filename && supportedFormats.Contains(m.Format)).FirstOrDefault();
            else
                return cachedModels.Where(m => m.Name == filename && m.Type == type && supportedFormats.Contains(m.Format)).FirstOrDefault();
        }

        public static Model GetModel(string filename, Enums.Models.Type type = Enums.Models.Type.Normal, Implementation imp = Implementation.InvokeAi)
        {
            return GetModels(type, imp).Where(x => x.Name == filename).FirstOrDefault();
        }

        public static Model GetModel(List<Model> cachedModels, string filename)
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

                if (parentDirName == Constants.Dirs.Models.Loras)
                    return Enums.Models.Type.Lora;

                if (Path.GetFileName(modelPath).Lower().Contains("refine"))
                    return Enums.Models.Type.Refiner;

                return Enums.Models.Type.Normal;
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to detect model type: {ex.Message} ({modelPath})", !print);
            }

            return (Enums.Models.Type)(-1);
        }

        public static void SetDiffusersClipSkip(Model model, int layersToSkip = 1)
        {
            if (layersToSkip > 0 && model.Format != Format.Diffusers)
            {
                Logger.Log($"Warning: Cannot apply CLIP Skip to this model because it is not a Diffusers model.");
                return;
            }

            if (!Directory.Exists(model.FullName))
            {
                Logger.Log($"Clip Skip Patcher: Not a model directory: {model.FullName}", true);
                return;
            }

            string jsonPath = Path.Combine(model.FullName, "text_encoder", "config.json");
            string srcJsonPath = jsonPath + ".original";

            if (!File.Exists(jsonPath))
            {
                Logger.Log($"Clip Skip Patcher: Can't find config json ({jsonPath})", true);
                return;
            }

            try
            {
                if (!File.Exists(srcJsonPath))
                    File.Copy(jsonPath, srcJsonPath);

                var lines = File.ReadAllLines(srcJsonPath);

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];

                    if (line.Trim().StartsWith("\"num_hidden_layers\": "))
                    {
                        int layers = line.Split(':').Last().GetInt(false);
                        int newLayers = (layers - layersToSkip).Clamp(1, int.MaxValue);

                        string newText = $"{line.Split("\"num_hidden_layers\": ")[0]}\"num_hidden_layers\": {newLayers},";
                        lines[i] = newText;
                        Logger.Log($"Clip Skip Patcher: Using {newLayers} out of {layers} layers (Skipping {layersToSkip})", true);
                    }
                }

                File.WriteAllLines(jsonPath, lines);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        // TODO: Decide to remove this, or could it be useful for other implementations?
        public static void HotswapDiffusersVae(Model mdl, Model vae)
        {
            string originalDir = Path.Combine(mdl.FullName, "vae_original");
            string vaeDir = Path.Combine(mdl.FullName, "vae");

            if (vae == null)
            {
                if (Directory.Exists(originalDir))
                    IoUtils.TryMove(originalDir, vaeDir); // Move original VAE back into place
            }
            else
            {
                if (Directory.Exists(originalDir)) // If backup of the original VAE folder exists...
                    IoUtils.TryDeleteIfExists(vaeDir); // ...delete fake VAE link

                if (Directory.Exists(vaeDir) && !IoUtils.TryMove(vaeDir, originalDir, false)) // Return if VAE folder exists and moving it failed
                    return;

                IoUtils.CreateJunction(vaeDir, InvokeAiUtils.GetConvertedVaePath(vae));
            }
        }

        public static bool HasAnyInpaintingModels(IEnumerable<Model> models = null, Implementation imp = (Implementation)(-1))
        {
            if (models == null)
                models = GetModelsAll();

            if (imp != (Implementation)(-1))
            {
                Format[] supportedFormats = imp.GetInfo().SupportedModelFormats;
                models = models.Where(m => supportedFormats.Contains(m.Format));
            }

            return models.Any(m => m.FormatIndependentName.Lower().EndsWith("inpainting"));
        }

        public class UnetConfig
        {
            public bool UseCheckpoint { get; set; }
            public int ImageSize { get; set; }
            public int OutChannels { get; set; }
            public bool UseSpatialTransformer { get; set; }
            public bool Legacy { get; set; }
            public string NumClasses { get; set; }
            public int AdmInChannels { get; set; }
            public bool UseFp16 { get; set; }
            public int InChannels { get; set; }
            public int ModelChannels { get; set; }
            public int NumResBlocks { get; set; }
            public List<int> AttentionResolutions { get; set; }
            public List<int> TransformerDepth { get; set; }
            public List<int> ChannelMult { get; set; }
            public int TransformerDepthMiddle { get; set; }
            public bool UseLinearInTransformer { get; set; }
            public int ContextDim { get; set; }
            public int NumHeads { get; set; }
            public int NumHeadChannels { get; set; }
        }

        public static ModelArch DetectModelType(string modelType, string unetConfigJson)
        {
            if(modelType == "FLUX")
                return ModelArch.Flux;

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy { ProcessDictionaryKeys = true } },
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            unetConfigJson = unetConfigJson.Replace("True", "true").Replace("False", "false").Replace("None", "-1");

            var config = new UnetConfig();

            try
            {
                config = JsonConvert.DeserializeObject<UnetConfig>(unetConfigJson, settings);
            }
            catch
            {
                return ModelArch.Sd1; // Assume Diffusers, which can currently only load SD1
            }

            if (config.AttentionResolutions != null && config.AttentionResolutions.Count == 2 && config.AdmInChannels > 0) // Must be SD XL
            {
                if (config.ContextDim >= 2048)
                    return ModelArch.SdXlBase;
                else
                    return ModelArch.SdXlRefine;
            }
            else if (modelType == "EPS" && config.ContextDim == 768 && !config.UseLinearInTransformer) // Must be SD 1.x
            {
                if (config.InChannels == 4)
                    return ModelArch.Sd1;
                else if (config.InChannels == 9)
                    return ModelArch.Sd1Inpaint;
            }
            else if (config.ContextDim == 1024 && config.UseLinearInTransformer) // Must be SD 2.x
            {
                if (modelType == "V_PREDICTION")
                    return ModelArch.Sd2V;
                else if (config.InChannels == 4)
                    return ModelArch.Sd2;
                else if (config.InChannels == 9)
                    return ModelArch.Sd2Inpaint;
            }

            return ModelArch.Automatic;
        }

        public static Size GetDefaultRes(ModelArch modelArch)
        {
            switch(modelArch)
            {
                case ModelArch.SdXlBase: return new Size(1024, 1024);
                case ModelArch.SdXlRefine: return new Size(1024, 1024);
                case ModelArch.Sd2V: return new Size(768, 768);
                default: return new Size(512, 512);
            }
        }

        public static ModelArch AssumeModelArch (string modelName, ModelArch fallback = ModelArch.Automatic)
        {
            modelName = modelName.Lower();

            if (modelName.Contains("xl")) // xl in name => assume SDXL
                return modelName.Contains("refine") ? ModelArch.SdXlRefine : ModelArch.SdXlBase; // refine in name => assume SDXL Refiner

            if (modelName.Contains("768-v")) // 768-v in name => assume SD 2 V-Prediction
                return ModelArch.Sd2V;
            else
                return fallback;
        }

        public static ModelArch AssumeControlnetArch(string modelName, ModelArch fallback = (ModelArch)(-1))
        {
            modelName = modelName.Lower();

            if (modelName.Contains("sd15") || modelName.Contains("sd14"))
                return ModelArch.Sd1;

            if (modelName.Contains("xl"))
                return ModelArch.SdXlBase;

            if (modelName.Contains("768-v")) // I don't think SD2V controlnets exist, but who knows
                return ModelArch.Sd2V;

            return fallback;
        }
    }
}
