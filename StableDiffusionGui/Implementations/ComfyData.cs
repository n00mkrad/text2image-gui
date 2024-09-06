using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Implementations
{
    public class ComfyData
    {
        public class ControlnetInfo
        {
            public string Model;
            public ImagePreprocessor Preprocessor;
            public float Strength;
            public ControlnetInfo() { }
        }

        public class GenerationInfo
        {
            public string Model;
            public string ModelRefiner;
            public string Vae;
            public string Upscaler;
            public string Prompt;
            public string NegativePrompt;
            public List<KeyValuePair<string, float>> Loras = new List<KeyValuePair<string, float>>();
            public List<KeyValuePair<string, float>> Hypernetworks = new List<KeyValuePair<string, float>>();
            public List<ControlnetInfo> Controlnets = new List<ControlnetInfo>();
            public int Steps;
            public long Seed;
            public float Scale;
            public float Guidance = 3.0f;
            public float RefinerStrength;
            public string InitImg;
            public float InitStrength;
            public string MaskPath;
            public Size BaseResolution;
            public Size TargetResolution;
            public UpscaleMethod UpscaleMethod = (UpscaleMethod)(-1);
            public Sampler Sampler;
            public int ClipSkip = -1;
            public bool SaveOriginalAndUpscale;
            public UltimateSdUpConfig UltimateSdUpConfig;
            public bool Seamless = false;

            public GenerationInfo() { }

            public GenerationInfo(TtiSettings s, Model model, Model refineModel, Model vae)
            {
                BaseResolution = s.Res;
                TargetResolution = s.UpscaleTargetRes;
                UpscaleMethod = s.UpscaleMethod;
                NegativePrompt = s.NegativePrompt;
                Model = model.FullName;
                ModelRefiner = refineModel == null ? "" : refineModel.FullName;
                Vae = vae == null ? "" : vae.FullName;
                Sampler = s.Sampler;
                Upscaler = Config.Instance.UpscaleEnable ? Models.GetUpscalers().Where(m => m.Name == Config.Instance.EsrganModel).FirstOrDefault().FullName : "";
                ClipSkip = (Config.Instance.ModelSettings.Get(model.Name, new Models.ModelSettings()).ClipSkip * -1) - 1;
                SaveOriginalAndUpscale = Config.Instance.SaveUnprocessedImages;
                Seamless = s.SeamlessMode != SeamlessMode.Disabled;

                if (UpscaleMethod == UpscaleMethod.UltimateSd)
                {
                    var esrganMdl = Models.GetUpscalers().Where(m => m.Name == Config.Instance.EsrganModel).FirstOrDefault();
                    var sdMdl = Models.GetModels(implementation: Implementation.Comfy).Where(m => m.Name == Config.Instance.SdUpscaleModel).FirstOrDefault();
                    var cnetMdl = (Model)null; // Models.GetControlNets().Where(m => m.Name.Contains("_tile_") && m.Name.Contains("sd15")).FirstOrDefault();

                    UltimateSdUpConfig = new UltimateSdUpConfig()
                    {
                        ModelPathEsrgan = esrganMdl == null ? "" : esrganMdl.FullName,
                        ModelPathTileControlnet = cnetMdl == null ? "" : cnetMdl.FullName,
                        ModelPathSd = sdMdl == null ? "" : sdMdl.FullName,
                    };

                    if (UltimateSdUpConfig.GetMissingModels(out List<string> missing) && missing.Any())
                        throw new System.Exception($"Ultimate SD Upscaler is enabled, but the following required models could not be found:\n{string.Join(", ", missing)}");
                }

                foreach (ControlnetInfo cnet in s.Controlnets.Where(cn => cn != null && cn.Strength > 0.001f && cn.Model != Constants.NoneMdl))
                {
                    var cnetModel = Models.GetControlNets().Where(m => m.FormatIndependentName == cnet.Model).FirstOrDefault();
                    if (cnetModel == null) continue;
                    Controlnets.Add(new ControlnetInfo { Model = cnetModel.FullName, Preprocessor = cnet.Preprocessor, Strength = cnet.Strength });
                }

                Loras = s.Loras.Select(lora => new KeyValuePair<string, float>(lora.Key, lora.Value.First())).ToList();
            }

            public GenerationInfo GetSerializeClone()
            {
                return new GenerationInfo()
                {
                    Model = Path.GetFileName(Model),
                    ModelRefiner = Path.GetFileName(ModelRefiner),
                    Vae = Path.GetFileName(Vae),
                    Upscaler = Path.GetFileName(Upscaler),
                    Prompt = Prompt,
                    NegativePrompt = NegativePrompt,
                    Loras = Loras.Select(l => new KeyValuePair<string, float>(Path.GetFileName(l.Key), l.Value)).ToList(),
                    Hypernetworks = Hypernetworks.Select(l => new KeyValuePair<string, float>(Path.GetFileName(l.Key), l.Value)).ToList(),
                    Controlnets = Controlnets.Select(c => new ControlnetInfo { Model = Path.GetFileName(c.Model), Preprocessor = c.Preprocessor, Strength = c.Strength }).ToList(),
                    Steps = Steps,
                    Seed = Seed,
                    Scale = Scale,
                    Guidance = Guidance,
                    RefinerStrength = RefinerStrength,
                    InitImg = InitImg,
                    InitStrength = InitStrength,
                    MaskPath = MaskPath,
                    BaseResolution = BaseResolution,
                    TargetResolution = TargetResolution,
                    UpscaleMethod = UpscaleMethod,
                    Sampler = Sampler,
                    ClipSkip = ClipSkip,
                    SaveOriginalAndUpscale = SaveOriginalAndUpscale,
                    UltimateSdUpConfig = UpscaleMethod != UpscaleMethod.UltimateSd ? null : new UltimateSdUpConfig
                    {
                        ModelPathEsrgan = Path.GetFileName(UltimateSdUpConfig.ModelPathEsrgan),
                        ModelPathSd = Path.GetFileName(UltimateSdUpConfig.ModelPathSd),
                        ModelPathTileControlnet = Path.GetFileName(UltimateSdUpConfig.ModelPathTileControlnet),
                        UseTileControlnet = UltimateSdUpConfig.UseTileControlnet,
                        TileSize = UltimateSdUpConfig.TileSize,
                    },
                    Seamless = Seamless,
                };
            }

            public string Serialize()
            {
                return GetSerializeClone().ToJson();
            }

            public Dictionary<string, dynamic> GetMetadataDict()
            {
                return new Dictionary<string, dynamic>
                {
                    { "model", Path.GetFileName(Model) },
                    { "modelRefiner", Path.GetFileName(ModelRefiner) },
                    { "upscaler", Path.GetFileName(Upscaler) },
                    { "prompt", Prompt },
                    { "promptNeg", NegativePrompt },
                    { "initImg", InitImg },
                    { "initStrength", InitStrength },
                    { "w", BaseResolution.Width },
                    { "h", BaseResolution.Height },
                    { "steps", Steps },
                    { "seed", Seed },
                    { "scaleTxt", Scale },
                    { "guidance", Guidance },
                    { "inpaintMask", MaskPath },
                    { "sampler", Sampler.ToString().Lower() },
                    { "refineFrac", (1f - RefinerStrength) },
                    { "upscaleW", TargetResolution.Width },
                    { "upscaleH", TargetResolution.Height },
                    { "loras", Loras },
                    { "hypernetworks", Hypernetworks },
                    { "controlnets", Controlnets },
                };
            }
        }

        public class UltimateSdUpConfig
        {
            public string ModelPathEsrgan = "";
            public string ModelPathSd = "";
            public string ModelPathTileControlnet = "";
            public int TileSize = 1024;
            public bool UseTileControlnet = false;

            public bool IsValid()
            {
                GetMissingModels(out List<string> missing);
                return !missing.Any();
            }

            public bool GetMissingModels (out List<string> list)
            {
                var missing = new List<string>();

                if (!File.Exists(ModelPathEsrgan))
                    missing.Add("ESRGAN Upscaling Model");

                if (!File.Exists(ModelPathSd))
                    missing.Add("Stable Diffusion 1 Model");

                if (UseTileControlnet && !File.Exists(ModelPathTileControlnet))
                    missing.Add("ControlNet Tile Model");

                list = missing;
                return list.Any();
            }
        }
    }
}
