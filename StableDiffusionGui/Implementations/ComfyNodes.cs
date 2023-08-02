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

        public class NmkdKSampler : IComfyNode
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
            public float Denoise = 1f;

            public ComfyWorkflow.Node NodeModel;
            public ComfyWorkflow.Node NodePositive;
            public ComfyWorkflow.Node NodeNegative;
            public ComfyWorkflow.Node NodeLatentImage;
            public ComfyWorkflow.Node NodeSeed;
            public ComfyWorkflow.Node NodeSteps;
            public ComfyWorkflow.Node NodeStartStep;
            public ComfyWorkflow.Node NodeEndStep;
            public ComfyWorkflow.Node NodeCfg;

            public string DebugString = "";

            public string GetString()
            {
                return $"\"inputs\":{{" +
                    $"\"add_noise\":{(AddNoise ? "enable" : "disable").Wrap()}," +
                    $"\"noise_seed\":[\"{NodeSeed.Id}\",0]," +
                    $"\"steps\":[\"{NodeSteps.Id}\",0]," +
                    $"\"cfg\":[\"{NodeCfg.Id}\",0]," +
                    $"\"sampler_name\":\"{SamplerName}\"," +
                    $"\"scheduler\":\"{Scheduler}\"," +
                    $"\"start_at_step\":[\"{NodeStartStep.Id}\",0]," +
                    $"\"end_at_step\":[\"{NodeEndStep.Id}\",0]," +
                    $"\"return_with_leftover_noise\":{(ReturnLeftoverNoise ? "enable" : "disable").Wrap()}," +
                    $"\"denoise\":{Denoise.ToStringDot("0.######")}," +
                    $"\"model\":[\"{NodeModel.Id}\",0]," +
                    $"\"positive\":[\"{NodePositive.Id}\",0]," +
                    $"\"negative\":[\"{NodeNegative.Id}\",0]," +
                    $"\"latent_image\":[\"{NodeLatentImage.Id}\",0]," +
                    $"\"debug_string\":\"{DebugString}\"" +
                    $"}},\"class_type\":\"NmkdKSampler\"";
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
            public ComfyWorkflow.Node TextNode;
            public ComfyWorkflow.Node ClipNode;

            public string GetString()
            {
                int clipNodeOutIndex = 0;
                if (ClipNode.Type == ComfyWorkflow.NodeType.NmkdCheckpointLoader) clipNodeOutIndex = 1;

                return $"\"inputs\":{{" +
                    $"\"text\":[\"{TextNode.Id}\",0]," +
                    $"\"clip\":[\"{ClipNode.Id}\",{clipNodeOutIndex}]" +
                    $"}},\"class_type\":\"CLIPTextEncode\"";
            }

            public override string ToString()
            {
                return $"CLIPTextEncode - Text Node: '{TextNode}' - CLIP Node: '{ClipNode}'";
            }
        }

        public class VAEDecode : IComfyNode
        {
            public ComfyWorkflow.Node LatentsNode;
            public ComfyWorkflow.Node VaeNode;

            public string GetString()
            {
                int vaeNodeOutIndex = 0;
                if (VaeNode.Type == ComfyWorkflow.NodeType.NmkdCheckpointLoader) vaeNodeOutIndex = 2;

                return $"\"inputs\":{{" +
                    $"\"samples\":[\"{LatentsNode.Id}\",0]," +
                    $"\"vae\":[\"{VaeNode.Id}\",{vaeNodeOutIndex}]}}," +
                    $"\"class_type\":\"VAEDecode\"";
            }

            public override string ToString()
            {
                return $"VAEDecode - Latents Node: '{LatentsNode}' - VAE Node: '{VaeNode}'";
            }
        }

        public class VAEEncode : IComfyNode
        {
            public ComfyWorkflow.Node ImageNode;
            public ComfyWorkflow.Node VaeNode;

            public string GetString()
            {
                int vaeNodeOutIndex = 0;
                if (VaeNode.Type == ComfyWorkflow.NodeType.NmkdCheckpointLoader) vaeNodeOutIndex = 2;

                var dict = new Dictionary<string, string>()
                {
                    { "pixels", $"[{ImageNode.Id.ToString().Wrap()},0]" },
                    { "vae", $"[{VaeNode.Id.ToString().Wrap()},{vaeNodeOutIndex}]" },
                };

                return GetPropertiesString(dict, "VAEEncode");
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

            public override string ToString()
            {
                return $"NmkdCheckpointLoader - Model: {ModelPath} - Load VAE: {LoadVae} - VAE: {VaePath}";
            }
        }

        public class NmkdImageLoader : IComfyNode
        {
            public string ImagePath;

            public string GetString()
            {
                var dict = new Dictionary<string, string>()
                {
                    { "image_path", ImagePath.Wrap(true) },
                };

                return GetPropertiesString(dict, "NmkdImageLoader");
            }
        }

        public class CLIPSetLastLayer : IComfyNode
        {
            public int Skip = -1;
            public ComfyWorkflow.Node ClipNode;

            public string GetString()
            {
                int clipNodeIndex = 0;

                if (ClipNode.Type == ComfyWorkflow.NodeType.NmkdCheckpointLoader || ClipNode.Type == ComfyWorkflow.NodeType.CheckpointLoaderSimple) clipNodeIndex = 1;

                var dict = new Dictionary<string, string>()
                {
                    { "stop_at_clip_layer", Skip.ToString() },
                    { "clip", $"[{ClipNode.Id.ToString().Wrap()},{clipNodeIndex}]" },
                };

                return GetPropertiesString(dict, "CLIPSetLastLayer");
            }
        }

        private static string GetPropertiesString(Dictionary<string, string> dict, string classType)
        {
            return $"\"inputs\":{{{string.Join(",", dict.Select(pair => $"{pair.Key.Wrap()}:{pair.Value}"))}}},\"class_type\":\"{classType}\"";
        }

    }
}
