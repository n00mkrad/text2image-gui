using Newtonsoft.Json;
using StableDiffusionGui.Data;
using StableDiffusionGui.Main;
using StableDiffusionGui.Os;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StableDiffusionGui.Io
{
    public class ConfigInstance
    {
        public EasyDict<string, Models.ModelSettings> ModelSettings = new EasyDict<string, Models.ModelSettings>();
        public bool MultiPromptsSameSeed;
        public int SamplerIdx;
        public float InitStrength;
        public int Steps;
        public float Scale;
        public float Guidance;
        public string OutPath = "";
        public string FavsPath = "";
        public int Iterations;
        public string Model = "";
        public string ModelAux = "";
        public string ModelVae = "";
        public string ModelControlnet = "";
        public bool EnablePromptHistory;
        public float CodeformerFidelity;
        public int ResW;
        public int ResH;
        public bool FullPrecision;
        public bool FolderPerPrompt;
        public bool FolderPerSession;
        public bool PromptInFilename;
        public bool SeedInFilename;
        public bool ScaleInFilename;
        public bool SamplerInFilename;
        public bool ModelInFilename;
        public bool FilenameIgnoreWildcards;
        public float FaceRestoreStrength;
        public bool FaceRestoreEnable;
        public int FaceRestoreIdx;
        public bool UpscaleEnable;
        public int UpscaleIdx;
        public float UpscaleStrength;
        public Enums.StableDiffusion.Implementation Implementation = Enums.StableDiffusion.Implementation.InvokeAi;
        public int CudaDeviceIdx;
        public bool AdvancedUi;
        public int NotifyModeIdx;
        public bool SaveUnprocessedImages;
        public bool UnloadModel;
        public List<string> CustomModelDirs = new List<string>();
        public List<string> CustomVaeDirs = new List<string>();
        public string MotdShownVersion = "";
        public bool HideMotd;
        public bool DisableModelFileValidation;
        public bool InvokeSequentialGuidance;
        public bool InvokeFreeGpuMem;
        public bool DisablePostProcessing;
        public bool InvokeAllowModelCaching;
        public bool EnableTokenizationLogging;
        public bool PopupSlideshowEnabledByDefault;
        public bool ConvertModelsDeleteInput;
        public bool AutoSetResForInitImg;
        public bool InitImageRetainAspectRatio;
        public bool HiresFix;
        public Enums.Export.FilenameTimestamp FilenameTimestampMode;
        public bool WildcardAllowEmptyEntries;
        public bool LogStdin;
        public bool OfflineMode;
        public float SymmetryTimepoint;
        public int ClipSkip;
        public string LastInitImageParentPath;
        public bool InvokeAllowMod8;
        public int EsrganTileSize;
        public float EsrganDenoise;
        public string EmbeddingsDir;
        public string LorasDir;
        public string ControlNetsDir;
        public string UpscalersDir;
        public bool AutoDeleteImgs;
        public EasyDict<string, List<float>> LoraWeights = new EasyDict<string, List<float>>();
        public string LastTrainingBaseModel;
        public int LastLoraClipSkip;
        public string LastLoraTrainName;
        public string LastLoraTrainTrigger;
        public string LastLoraDataDir;
        public bool DontClearPipCache;
        public bool AlwaysClearInpaintMask;
        public int ImageCacheMaxSizeMb;
        public bool NmkdiffOffload;
        public bool NmkdiffSdXLSequential;
        public float SdXlRefinerStrength;
        public Enums.Comfy.VramPreset ComfyVramPreset;
        public string EsrganModel;
        public string SdUpscaleModel;
        public bool ComfyFast;

        public ConfigInstance()
        {
            MultiPromptsSameSeed = true;
            InitStrength = 0.4f;
            Steps = 20;
            Scale = 7.0f;
            Guidance = 3.0f;
            OutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "SDGUI");
            FavsPath = Path.Combine(OutPath, Constants.Dirs.ImgFavs);
            Iterations = 5;
            EnablePromptHistory = true;
            CodeformerFidelity = 0.6f;
            ResW = 512;
            ResH = 512;
            FullPrecision = GpuUtils.CachedGpus.Count > 0 && GpuUtils.CachedGpus[0].FullName.Contains(" GTX 16");
            FolderPerPrompt = true;
            PromptInFilename = true;
            SeedInFilename = true;
            ScaleInFilename = true;
            SamplerInFilename = true;
            ModelInFilename = true;
            FilenameIgnoreWildcards = true;
            FaceRestoreStrength = 0.8f;
            UpscaleStrength = 1.0f;
            InvokeAllowModelCaching = true;
            ConvertModelsDeleteInput = true;
            AutoSetResForInitImg = true;
            InitImageRetainAspectRatio = true;
            FilenameTimestampMode = Enums.Export.FilenameTimestamp.DateTime;
            WildcardAllowEmptyEntries = true;
            SymmetryTimepoint = 0.9f;
            EsrganTileSize = 1024;
            EsrganDenoise = 0.0f;
            EmbeddingsDir = Paths.GetEmbeddingsPath();
            LorasDir = Paths.GetLorasPath();
            ControlNetsDir = Paths.GetControlNetsPath();
            UpscalersDir = Paths.GetUpscalersPath();

            float ramGb = HwInfo.GetTotalRamGb;
            ImageCacheMaxSizeMb = 64;
            if (ramGb > 14f) ImageCacheMaxSizeMb = 128;
            if (ramGb > 22f) ImageCacheMaxSizeMb = 256;

            SdXlRefinerStrength = 0.2f;
            ComfyVramPreset = Enums.Comfy.VramPreset.NormalVram;
            ComfyFast = true;
        }

        public ConfigInstance Clone()
        {
            return this.ToJson().FromJson<ConfigInstance>(NullValueHandling.Ignore, DefaultValueHandling.Include, true, true);
        }

        public void Clean()
        {
            var newModelSettings = new EasyDict<string, Models.ModelSettings>();

            foreach (var mdlName in ModelSettings.Keys)
            {
                if (ModelSettings[mdlName].Arch != Enums.StableDiffusion.ModelArch.Automatic || ModelSettings[mdlName].ClipSkip != 0)
                    newModelSettings.Add(mdlName, ModelSettings[mdlName]);
            }

            ModelSettings = newModelSettings;
        }
    }
}
