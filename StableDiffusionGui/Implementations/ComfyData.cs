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
            public float RefinerStrength;
            public string InitImg;
            public float InitStrength;
            public string MaskPath;
            public Size BaseResolution;
            public Size TargetResolution;
            public Sampler Sampler;
            public int ClipSkip = -1;

            public GenerationInfo GetSerializeClone ()
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
                    RefinerStrength = RefinerStrength,
                    InitImg = InitImg,
                    InitStrength = InitStrength,
                    MaskPath = MaskPath,
                    BaseResolution = BaseResolution,
                    TargetResolution = TargetResolution,
                    Sampler = Sampler,
                    ClipSkip = ClipSkip,
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
    }
}
