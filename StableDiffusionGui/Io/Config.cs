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
using System.Reflection;

namespace StableDiffusionGui.Io
{
    class Config
    {
        public static bool Ready = false;
        public static string ConfigPath = "";
        private static EasyDict<string, string> _cachedConfig = new EasyDict<string, string>();

        public static ConfigInstance Instance = null;

        public static void Init()
        {
            ConfigPath = Path.Combine(Paths.GetDataPath(), Constants.Files.Config);
            IoUtils.CreateFileIfNotExists(ConfigPath);
            Load();
            // Reload();
            Ready = true;
            // DumpKeys();
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

        // public static void Set<T>(string key, T value)
        // {
        //     Reload();
        //     _cachedConfig[key] = value == null ? "" : value.ToJson();
        //     WriteConfig();
        // }

        public static void Load ()
        {
            try
            {
                string path = Path.Combine(Paths.GetDataPath(), "conf.json");
                Instance = File.ReadAllText(path).FromJson<ConfigInstance>();
                Console.WriteLine("Loaded config.");
            }
            catch(Exception ex)
            {
                Logger.LogException(ex);
            }

            if(Instance == null)
                Instance = new ConfigInstance();

            Console.WriteLine("=> INSTANCE LOADED");
        }

        public static void Save ()
        {
            try
            {
                string path = Path.Combine(Paths.GetDataPath(), "conf.json");
                Instance.Clean();
                File.WriteAllText(path, Instance.ToJson(true, true));
                Console.WriteLine("Saved config.");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        /// <summary> Gets value from config by looking for <paramref name="key"/>. Use <paramref name="defaultValue"/> as fallback, optionally
        /// with <paramref name="writeToFileIfNotExists"/> to write it to the config file. </summary>
        /// <returns> Saved value if one was found, otherwise <paramref name="defaultValue"/> </returns>
        // public static T Get<T>(string key, T defaultValue = default, bool writeToFileIfNotExists = true)
        // {
        //     try
        //     {
        //         if (!_cachedConfig.ContainsKey(key))
        //         {
        //             if (!writeToFileIfNotExists)
        //                 return defaultValue;
        // 
        //             Set(key, defaultValue);
        //         }
        // 
        //         return _cachedConfig.GetNoNull(key, default(T).ToJson()).FromJson<T>();
        //     }
        //     catch (Exception e)
        //     {
        //         Logger.Log($"Failed to get {key.Wrap()} from config! {e.Message}", true);
        //         return default(T);
        //     }
        // }

        /// <summary> Gets value from config by looking for <paramref name="key"/>. If it does not exist, try applying defaults first. </summary>
        /// <returns> Saved value if one was found </returns>
        // public static T Get<T>(string key)
        // {
        //     try
        //     {
        //         if (!_cachedConfig.ContainsKey(key))
        //             ApplyDefaults(key);
        // 
        //         return _cachedConfig.GetNoNull(key, default(T).ToJson()).FromJson<T>();
        //     }
        //     catch (Exception e)
        //     {
        //         Logger.Log($"Failed to get {key.Wrap()} from config! {e.Message}", true);
        //         return default(T);
        //     }
        // }

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
            public const string ImplementationName = "implementationName";
            public const string CudaDeviceIdx = "cudaDevice";
            public const string AdvancedUi = "advancedUi";
            public const string NotifyModeIdx = "motifyModeIdx";
            public const string SaveUnprocessedImages = "saveUnprocessedImages";
            public const string UnloadModel = "unloadModel";
            public const string PrunePrecisionIdx = "prunePrecisionIdx";
            public const string PruneDeleteInput = "pruneDeleteInput";
            public const string CustomModelDirs = "customModelDirs";
            public const string MotdShownVersion = "motdShownVersion";
            public const string HideMotd = "hideMotd";
            public const string SafeModels = "safeModels";
            public const string DisablePickleScanner = "disablePickleScanner";
            public const string DisableModelFileValidation = "disableModelFileValidation";
            public const string MedVramFreeGpuMem = "medVramFreeGpuMem";
            public const string MedVramDisablePostProcessing = "medVramDisablePostProcessing";
            public const string InvokeAllowModelCaching = "invokeAllowModelCaching";
            public const string EnableTokenizationLogging = "enableTokenizationLogging";
            public const string PopupSlideshowEnabledByDefault = "popupSlideshowEnabledByDefault";
            public const string ConvertModelsDeleteInput = "convertModelsDeleteInput";
            public const string AutoSetResForInitImg = "autoSetResForInitImg";
            public const string InitImageRetainAspectRatio = "initImageRetainAspectRatio";
            public const string HiresFix = "hiresFix";
            public const string FilenameTimestampMode = "filenameTimestampMode";
            public const string WildcardAllowEmptyEntries = "wildcardAllowEmptyEntries";
            public const string LogStdin = "logStdin";
            public const string OfflineMode = "offlineMode";
            public const string SymmetryTimepoint = "symmetryTimepoint";
            public const string ClipSkip = "clipSkip";
        }
    }
}
