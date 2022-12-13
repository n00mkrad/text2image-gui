using StableDiffusionGui.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using StableDiffusionGui.Main;
using System.Linq;
using StableDiffusionGui.Os;
using System.Threading.Tasks;
using StableDiffusionGui.Data;

namespace StableDiffusionGui.Io
{
    class Config
    {
        public static bool Ready = false;
        public static string ConfigPath = "";
        private static EasyDict<string, string> _cachedConfig = new EasyDict<string, string>();

        public static void Init()
        {
            ConfigPath = Path.Combine(Paths.GetDataPath(), Constants.Files.Config);
            IoUtils.CreateFileIfNotExists(ConfigPath);
            Reload();
            Ready = true;
        }

        public static async Task Reset(int retries = 3, SettingsForm settingsForm = null)
        {
            try
            {
                if (settingsForm != null)
                    settingsForm.Enabled = false;

                File.Delete(ConfigPath);
                await Task.Delay(100);
                _cachedConfig.Clear();
                await Task.Delay(100);

                if (settingsForm != null)
                    settingsForm.Enabled = true;
            }
            catch (Exception e)
            {
                retries -= 1;
                Logger.Log($"Failed to reset config: {e.Message}. Retrying ({retries} attempts left).", true);
                await Task.Delay(500);
                await Reset(retries, settingsForm);
            }
        }

        public static void Set<T>(string key, T value)
        {
            Reload();
            _cachedConfig[key] = value == null ? "" : value.ToJson();
            WriteConfig();
        }

        private static void WriteConfig()
        {
            SortedDictionary<string, string> cachedValuesSorted = new SortedDictionary<string, string>(_cachedConfig);
            File.WriteAllText(ConfigPath, cachedValuesSorted.ToJson(true));
        }

        private static void Reload()
        {
            try
            {
                EasyDict<string, string> newDict = new EasyDict<string, string>();
                EasyDict<string, string> deserializedConfig = JsonConvert.DeserializeObject<EasyDict<string, string>>(File.ReadAllText(ConfigPath));

                if (deserializedConfig == null)
                    deserializedConfig = new EasyDict<string, string>();

                foreach (KeyValuePair<string, string> entry in deserializedConfig)
                    newDict.Add(entry.Key, entry.Value);

                _cachedConfig = newDict; // Use temp dict and only copy it back if no exception was thrown
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to reload config! {e.Message}", true);
            }
        }

        public static T Get<T>(string key, T defaultValue = default)
        {
            try
            {
                if (!_cachedConfig.ContainsKey(key))
                {
                    if (defaultValue != null && !defaultValue.Equals(default(T)))
                        Set(key, defaultValue);
                    else
                        ApplyDefaults(key);
                }

                return _cachedConfig.GetNoNull(key, default(T).ToJson()).FromJson<T>();
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to get {key.Wrap()} from config! {e.Message}", true);
                return default(T);
            }
        }

        /// <returns> True if a default value was applied, False if not </returns>
        private static bool ApplyDefaults(string key)
        {
            switch (key)
            {
                case Keys.CodeformerFidelity: Set(key, 0.6f); return true;
                case Keys.EnablePromptHistory: Set(key, true); return true;
                case Keys.FaceRestoreStrength: Set(key, 0.8f); return true;
                case Keys.FavsPath: Set(key, Path.Combine(Paths.GetExeDir(), "Images", "Favs")); return true;
                case Keys.FilenameIgnoreWildcards: Set(key, true); return true;
                case Keys.FolderPerPrompt: Set(key, true); return true;
                case Keys.FullPrecision: Set(key, GpuUtils.CachedGpus.Count > 0 && GpuUtils.CachedGpus[0].FullName.Contains(" GTX 16")); return true;
                case Keys.InitStrength: Set(key, 0.5f); return true;
                case Keys.Iterations: Set(key, 5); return true;
                case Keys.MedVramDisablePostProcessing: Set(key, true); return true;
                case Keys.MedVramFreeGpuMem: Set(key, true); return true;
                case Keys.Model: try { Set(key, Paths.GetModels(Enums.StableDiffusion.ModelType.Normal).Select(x => x.Name).First()); } catch { Set(key, ""); } return true;
                case Keys.ModelInFilename: Set(key, true); return true;
                case Keys.ModelVae: try { Set(key, Paths.GetModels(Enums.StableDiffusion.ModelType.Vae).Select(x => x.Name).First()); } catch { Set(key, ""); } return true;
                case Keys.MultiPromptsSameSeed: Set(key, true); return true;
                case Keys.OutPath: Set(key, Path.Combine(Paths.GetExeDir(), "Images")); return true;
                case Keys.PromptInFilename: Set(key, true); return true;
                case Keys.PruneDeleteInput: Set(key, true); return true;
                case Keys.PrunePrecisionIdx: Set(key, true); return true;
                case Keys.ResH: Set(key, 512); return true;
                case Keys.ResW: Set(key, 512); return true;
                case Keys.SamplerInFilename: Set(key, true); return true;
                case Keys.Scale: Set(key, 8.0f); return true;
                case Keys.ScaleInFilename: Set(key, true); return true;
                case Keys.SeedInFilename: Set(key, true); return true;
                case Keys.Steps: Set(key, 25); return true;
                case Keys.UpscaleStrength: Set(key, 1.0f); return true;
                case Keys.AutoSetResForInitImg: Set(key, true); return true;
            }

            return false;
        }

        public class Keys
        {
            public const string CmdDebugMode = "cmdDebugMode";
            public const string MultiPromptsSameSeed = "multiPromptsSameSeed";
            public const string Sampler = "sampler";
            public const string InitStrength = "initStrength";
            public const string Steps = "steps";
            public const string Scale = "scale";
            public const string OutPath = "outPath";
            public const string FavsPath = "favsPath";
            public const string Iterations = "iterations";
            public const string Model = "sdModel";
            public const string ModelVae = "sdModelVae";
            public const string LowMemTurbo = "lowMemTurbo";
            public const string EnablePromptHistory = "enablePromptHistory";
            public const string CodeformerFidelity = "codeformerFidelity";
            public const string ResW = "resW";
            public const string ResH = "resH";
            public const string FullPrecision = "fullPrecision";
            public const string FolderPerPrompt = "folderPerPrompt";
            public const string FolderPerSession = "folderPerSession";
            public const string PromptInFilename = "promptInFilename";
            public const string SeedInFilename = "seedInFilename";
            public const string ScaleInFilename = "scaleInFilename";
            public const string SamplerInFilename = "samplerInFilename";
            public const string ModelInFilename = "modelInFilename";
            public const string FilenameIgnoreWildcards = "filenameIgnoreWildcards";
            public const string FaceRestoreStrength = "faceRestoreStrength";
            public const string FaceRestoreEnable = "faceRestoreEnable";
            public const string FaceRestoreIdx = "faceRestoreIdx";
            public const string UpscaleEnable = "upscaleEnable";
            public const string UpscaleIdx = "upscaleIdx";
            public const string UpscaleStrength = "upscaleStrength";
            public const string ImplementationIdx = "implementation";
            public const string CudaDeviceIdx = "cudaDevice";
            public const string AdvancedUi = "advancedUi";
            public const string NotifyModeIdx = "motifyModeIdx";
            public const string SaveUnprocessedImages = "saveUnprocessedImages";
            public const string UnloadModel = "unloadModel";
            public const string PrunePrecisionIdx = "prunePrecisionIdx";
            public const string PruneDeleteInput = "pruneDeleteInput";
            public const string CustomModelDirsPfx = "customModelDirs";
            public const string MotdShownVersion = "motdShownVersion";
            public const string HideMotd = "hideMotd";
            public const string SafeModels = "safeModels";
            public const string DisablePickleScanner = "disablePickleScanner";
            public const string DisableModelFileValidation = "disableModelFileValidation";
            public const string MedVramFreeGpuMem = "medVramFreeGpuMem";
            public const string MedVramDisablePostProcessing = "medVramDisablePostProcessing";
            public const string DisableModelCaching = "disableModelCaching";
            public const string EnableTokenizationLogging = "enableTokenizationLogging";
            public const string PopupSlideshowEnabledByDefault = "popupSlideshowEnabledByDefault";
            public const string ConvertModelsDeleteInput = "convertModelsDeleteInput";
            public const string AutoSetResForInitImg = "autoSetResForInitImg";
        }
    }
}
