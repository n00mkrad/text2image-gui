using HTAlt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StableDiffusionGui.Implementations.InvokeAiMetadata;

namespace StableDiffusionGui.Implementations
{
    public class ComfyNodes
    {
        public interface IComfyNode
        {
            string GetString();
        }

        public class NmkdIntegerConstant : IComfyNode
        {
            public long Value;

            public string GetString()
            {
                return $"\"inputs\":{{\"value\":{Value}}},\"class_type\":\"NmkdIntegerConstant\"";
            }
        }

        public class NmkdFloatConstant : IComfyNode
        {
            public float Value;

            public string GetString()
            {
                return $"\"inputs\":{{\"value\":{Value.ToStringDot("0.#####")}}},\"class_type\":\"NmkdFloatConstant\"";
            }
        }

        public class NmkdStringConstant : IComfyNode
        {
            public string Text;

            public string GetString()
            {
                return $"\"inputs\":{{\"value\":{Text.Wrap()}}},\"class_type\":\"NmkdStringConstant\"";
            }
        }

        public class CheckpointLoaderSimple : IComfyNode
        {
            public string CkptName = "";

            public string GetString()
            {
                return $"\"inputs\":{{\"ckpt_name\":{CkptName.Wrap()}}},\"class_type\":\"CheckpointLoaderSimple\"";
            }
        }

        public class VAELoader : IComfyNode
        {
            public string VaeName;

            public string GetString()
            {
                return $"\"inputs\":{{\"vae_name\":{VaeName.Wrap()}}},\"class_type\":\"VAELoader\"";
            }
        }

        public class KSampler : IComfyNode
        {
            public long Seed;
            public int Steps;
            public float Cfg;
            public string SamplerName = "dpmpp_2m";
            public string Scheduler = "normal";
            public float Denoise = 1.0f;
            public int IdModel;
            public int IdPositivePrompt;
            public int IdNegativePrompt;
            public int IdLatentImage;

            public string GetString()
            {
                return $"\"inputs\":{{" +
                    $"\"seed\":{Seed}," +
                    $"\"steps\":{Steps}," +
                    $"\"cfg\":{Cfg.ToStringDot("0.####")}," +
                    $"\"sampler_name\":\"{SamplerName}\"," +
                    $"\"scheduler\":\"{Scheduler}\"," +
                    $"\"denoise\":{Denoise}," +
                    $"\"model\":[\"{IdModel}\",0]," +
                    $"\"positive\":[\"{IdPositivePrompt}\",0]," +
                    $"\"negative\":[\"{IdNegativePrompt}\",0]," +
                    $"\"latent_image\":[\"{IdLatentImage}\",0]" +
                    $"}},\"class_type\":\"KSamplerAdvanced\"";
            }
        }

        public class KSamplerAdvanced : IComfyNode
        {
            public bool AddNoise;
            // public long Seed;
            // public int Steps;
            // public float Cfg;
            public string SamplerName = "dpmpp_2m";
            public string Scheduler = "normal";
            // public int StartStep;
            // public int EndStep;
            public bool ReturnLeftoverNoise;

            public int IdModel;
            public int IdPositive;
            public int IdNegative;
            public int IdLatentImage;
            public int IdSeed;
            public int IdSteps;
            public int IdStartStep;
            public int IdEndStep;
            public int IdCfg;

            public string GetString()
            {
                return $"\"inputs\":{{" +
                    $"\"add_noise\":{(AddNoise ? "enable" : "disable").Wrap()}," +
                    $"\"noise_seed\":[\"{IdSeed}\",0]," +
                    $"\"steps\":[\"{IdSteps}\",0]," +
                    $"\"cfg\":[\"{IdCfg}\",0]," +
                    $"\"sampler_name\":\"{SamplerName}\"," +
                    $"\"scheduler\":\"{Scheduler}\"," +
                    $"\"start_at_step\":[\"{IdStartStep}\",0]," +
                    $"\"end_at_step\":[\"{IdEndStep}\",0]," +
                    $"\"return_with_leftover_noise\":{(ReturnLeftoverNoise ? "enable" : "disable").Wrap()}," +
                    $"\"model\":[\"{IdModel}\",0]," +
                    $"\"positive\":[\"{IdPositive}\",0]," +
                    $"\"negative\":[\"{IdNegative}\",0]," +
                    $"\"latent_image\":[\"{IdLatentImage}\",0]" +
                    $"}},\"class_type\":\"KSamplerAdvanced\"";
            }
        }

        public class EmptyLatentImage : IComfyNode
        {
            public int Width;
            public int Height;
            public int BatchSize = 1;

            public string GetString()
            {
                return $"\"inputs\":{{" +
                    $"\"width\":{Width}," +
                    $"\"height\":{Height}," +
                    $"\"batch_size\":1" +
                    $"}},\"class_type\":\"EmptyLatentImage\"";
            }
        }

        public class LatentUpscaleBy : IComfyNode
        {
            public string UpscaleMethod = "nearest-exact";
            public float ScaleFactor = 1.5f;
            public int IdLatents;

            public string GetString()
            {
                return $"\"inputs\":{{" +
                    $"\"upscale_method\":\"{UpscaleMethod}\"," +
                    $"\"scale_by\":{ScaleFactor.ToStringDot("0.######")}," +
                    $"\"samples\":[\"{IdLatents}\",0]" +
                    $"}},\"class_type\":\"LatentUpscaleBy\"";
            }
        }

        public class LatentUpscale : IComfyNode
        {
            public string UpscaleMethod = "nearest-exact";
            public int Width;
            public int Height;
            public int IdLatents;

            public string GetString()
            {
                var dict = new Dictionary<string, string>()
                {
                    { "upscale_method", UpscaleMethod.Wrap() },
                    { "width", Width.ToString() },
                    { "height", Height.ToString() },
                    { "crop", "disabled".Wrap() },
                    { "samples", $"[{IdLatents.ToString().Wrap()},0]" },
                };

                return GetPropertiesString(dict, "LatentUpscale");
            }
        }

        public class CRLatentInputSwitch : IComfyNode
        {
            public int Selection;
            public int IdLatents1;
            public int IdLatents2;

            public string GetString()
            {
                return $"\"inputs\": {{\"Input\": {Selection + 1}, \"latent1\": [\"{IdLatents1}\", 0], \"latent2\": [\"{IdLatents2}\", 0]}}, \"class_type\": \"CR Latent Input Switch\"";
            }
        }

        public class CLIPTextEncode : IComfyNode
        {
            public int IdText;
            public int IdClip;

            public string GetString()
            {
                return $"\"inputs\":{{" +
                    $"\"text\":[\"{IdText}\",0]," +
                    $"\"clip\":[\"{IdClip}\",1]" +
                    $"}},\"class_type\":\"CLIPTextEncode\"";
            }
        }

        public class VAEDecode : IComfyNode
        {
            public int IdLatents;
            public int IdVae;
            public int IdVaeIndex;

            public string GetString()
            {
                return $"\"inputs\":{{" +
                    $"\"samples\":[\"{IdLatents}\",0]," +
                    $"\"vae\":[\"{IdVae}\",{IdVaeIndex}]}}," +
                    $"\"class_type\":\"VAEDecode\"";
            }
        }

        public class SaveImage : IComfyNode
        {
            public string Prefix = "nmkd";
            public int IdImages;

            public string GetString()
            {
                return $"\"inputs\":{{" +
                    $"\"filename_prefix\":\"{Prefix}\"," +
                    $"\"images\":[\"{IdImages}\",0]}}," +
                    $"\"class_type\":\"SaveImage\"";
            }
        }

        public class NmkdCheckpointLoader : IComfyNode
        {
            public string ModelPath;
            public bool LoadVae;
            public string VaePath;

            public string GetString()
            {
                var dict = new Dictionary<string, string>()
                {
                    { "mdl_path", ModelPath.Wrap(true) },
                    { "load_vae", (LoadVae ? "enable" : "disable").Wrap() },
                    { "vae_path", VaePath.Wrap(true) },
                };

                return GetPropertiesString(dict, "NmkdCheckpointLoader");
            }
        }


        private static string GetPropertiesString(Dictionary<string, string> dict, string classType)
        {
            return $"\"inputs\":{{{string.Join(",", dict.Select(pair => $"{pair.Key.Wrap()}:{pair.Value}"))}}},\"class_type\":\"{classType}\"";
        }
    }
}
