using HTAlt;
using StableDiffusionGui.Io;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StableDiffusionGui.Implementations.ComfyNodes;
using static StableDiffusionGui.Implementations.ComfyWorkflow;

namespace StableDiffusionGui.Implementations
{
    public class ComfyNodes
    {
        public interface INode
        {
            int Id { get; set; }
            string Title { get; set; }
            string GetString();
        }

        public class NmkdIntegerConstant : Node, INode
        {
            public long Value;

            public string GetString()
            {
                return $"\"inputs\":{{\"value\":{Value}}},\"class_type\":\"NmkdIntegerConstant\"";
            }
        }

        public class NmkdFloatConstant : Node, INode
        {
            public float Value;

            public string GetString()
            {
                return $"\"inputs\":{{\"value\":{Value.ToStringDot("0.#####")}}},\"class_type\":\"NmkdFloatConstant\"";
            }
        }

        public class NmkdStringConstant : Node, INode
        {
            public string Text;

            public string GetString()
            {
                return $"\"inputs\":{{\"value\":{Text.Wrap()}}},\"class_type\":\"NmkdStringConstant\"";
            }
        }

        public class CheckpointLoaderSimple : Node, INode
        {
            public string CkptName = "";

            public string GetString()
            {
                return $"\"inputs\":{{\"ckpt_name\":{CkptName.Wrap()}}},\"class_type\":\"CheckpointLoaderSimple\"";
            }
        }

        public class VAELoader : Node, INode
        {
            public string VaeName;

            public string GetString()
            {
                return $"\"inputs\":{{\"vae_name\":{VaeName.Wrap()}}},\"class_type\":\"VAELoader\"";
            }
        }

        public class KSampler : Node, INode
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

        public class NmkdKSampler : Node, INode
        {
            public bool AddNoise = true;
            // public long Seed;
            // public int Steps;
            // public float Cfg;
            public string SamplerName = "dpmpp_2m";
            public string Scheduler = "normal";
            // public int StartStep;
            // public int EndStep;
            public bool ReturnLeftoverNoise;
            public float Denoise = 1f;

            public INode NodeModel;
            public INode NodePositive;
            public INode NodeNegative;
            public INode NodeLatentImage;
            public INode NodeSeed;
            public INode NodeSteps;
            public INode NodeStartStep;
            public INode NodeEndStep;
            public INode NodeCfg;

            public string DebugString = "";

            public string GetString()
            {
                if (DebugString.IsEmpty())
                    DebugString = Title;

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

        public class EmptyLatentImage : Node, INode
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

        public class LatentUpscaleBy : Node, INode
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

        public class LatentUpscale : Node, INode
        {
            public string UpscaleMethod = "nearest-exact";
            public int Width;
            public int Height;
            public INode LatentsNode;

            public string GetString()
            {
                var dict = new Dictionary<string, string>()
                {
                    { "upscale_method", UpscaleMethod.Wrap() },
                    { "width", Width.ToString() },
                    { "height", Height.ToString() },
                    { "crop", "disabled".Wrap() },
                    { "samples", $"[{LatentsNode.Id.ToString().Wrap()},0]" },
                };

                return GetPropertiesString(dict, "LatentUpscale");
            }
        }

        public class CRLatentInputSwitch : Node, INode
        {
            public int Selection;
            public int IdLatents1;
            public int IdLatents2;

            public string GetString()
            {
                return $"\"inputs\": {{\"Input\": {Selection + 1}, \"latent1\": [\"{IdLatents1}\", 0], \"latent2\": [\"{IdLatents2}\", 0]}}, \"class_type\": \"CR Latent Input Switch\"";
            }
        }

        public class CLIPTextEncode : Node, INode
        {
            public INode TextNode;
            public INode ClipNode;

            public string GetString()
            {
                int clipNodeOutIndex = 0;
                if (ClipNode is NmkdCheckpointLoader) clipNodeOutIndex = 1;
                if (ClipNode is NmkdMultiLoraLoader) clipNodeOutIndex = 1;

                return $"\"inputs\":{{" +
                    $"\"text\":[\"{TextNode.Id}\",0]," +
                    $"\"clip\":[\"{ClipNode.Id}\",{clipNodeOutIndex}]" +
                    $"}},\"class_type\":\"CLIPTextEncode\"";
            }

            public override string ToString()
            {
                return $"CLIPTextEncode - '{Title}' - Text Node: '{TextNode}' - CLIP Node: '{ClipNode}'";
            }
        }

        public class VAEDecode : Node, INode
        {
            public INode LatentsNode;
            public INode VaeNode;

            public string GetString()
            {
                int vaeNodeOutIndex = 0;
                if (VaeNode is NmkdCheckpointLoader) vaeNodeOutIndex = 2;

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

        public class VAEEncode : Node, INode
        {
            public INode ImageNode;
            public INode VaeNode;

            public string GetString()
            {
                int vaeNodeOutIndex = 0;
                if (VaeNode is NmkdCheckpointLoader) vaeNodeOutIndex = 2;

                var dict = new Dictionary<string, string>()
                {
                    { "pixels", $"[{ImageNode.Id.ToString().Wrap()},0]" },
                    { "vae", $"[{VaeNode.Id.ToString().Wrap()},{vaeNodeOutIndex}]" },
                };

                return GetPropertiesString(dict, "VAEEncode");
            }
        }

        public class SaveImage : Node, INode
        {
            public string Prefix = "nmkd";
            public INode ImageNode;

            public string GetString()
            {
                var dict = new Dictionary<string, string>()
                {
                    { "filename_prefix", Prefix.Wrap() },
                    { "images", $"[{ImageNode.Id.ToString().Wrap()},0]" },
                };

                return GetPropertiesString(dict, "SaveImage");
            }
        }

        public class NmkdCheckpointLoader : Node, INode
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

        public class NmkdMultiLoraLoader : Node, INode
        {
            public List<string> Loras;
            public List<float> Weights;
            public INode ModelNode;
            public INode ClipNode;

            public string GetString()
            {
                int clipNodeIndex = 0;
                if (ClipNode is NmkdCheckpointLoader || ClipNode is CheckpointLoaderSimple) clipNodeIndex = 1;

                var dict = new Dictionary<string, string>()
                {
                    { "model", $"[{ModelNode.Id.ToString().Wrap()},0]" },
                    { "clip", $"[{ClipNode.Id.ToString().Wrap()},{clipNodeIndex}]" },
                    { "lora_paths", string.Join(",", Loras.Select(l => Path.Combine(Paths.GetLorasPath(false), l + ".safetensors"))).Replace(@"\", @"\\").Wrap() },
                    { "lora_strengths", string.Join(",", Weights.Select(w => w.ToStringDot("0.#####"))).Wrap() },
                };

                return GetPropertiesString(dict, "NmkdMultiLoraLoader");
            }

            public override string ToString()
            {
                return $"NmkdMultiLoraLoader - {Loras.Count} LoRAs - ModelNode: '{ModelNode}' - ClipNode: '{ClipNode}'";
            }
        }

        public class NmkdImageLoader : Node, INode
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

        public class NmkdImageUpscale : Node, INode
        {
            public string UpscaleModelPath;
            public INode ImageNode;

            public string GetString()
            {
                var dict = new Dictionary<string, string>()
                {
                    { "model_path", UpscaleModelPath.Wrap(true) },
                    { "image", $"[{ImageNode.Id.ToString().Wrap()},0]" },
                };

                return GetPropertiesString(dict, "NmkdImageUpscale");
            }
        }

        public class CLIPSetLastLayer : Node, INode
        {
            public int Skip = -1;
            public INode ClipNode;

            public string GetString()
            {
                int clipNodeIndex = 0;
                if (ClipNode is NmkdCheckpointLoader || ClipNode is CheckpointLoaderSimple) clipNodeIndex = 1;

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
