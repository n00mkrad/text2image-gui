using StableDiffusionGui.Io;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;
using System.Reflection;
using static StableDiffusionGui.Implementations.ComfyWorkflow;

namespace StableDiffusionGui.Implementations
{
    public class ComfyNodes
    {
        public interface INode
        {
            int Id { get; set; }
            string Title { get; set; }
            NodeInfo GetNodeInfo();
        }

        public class NmkdIntegerConstant : Node, INode
        {
            public long Value;

            public NodeInfo GetNodeInfo()
            {
                return new NodeInfo
                {
                    Inputs = new Dictionary<string, object>
                    {
                        ["value"] = Value
                    },
                    ClassType = nameof(NmkdIntegerConstant)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class NmkdFloatConstant : Node, INode
        {
            public float Value;

            public NodeInfo GetNodeInfo()
            {
                return new NodeInfo
                {
                    Inputs = new Dictionary<string, object>
                    {
                        ["value"] = Value
                    },
                    ClassType = nameof(NmkdFloatConstant)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class NmkdStringConstant : Node, INode
        {
            public string Text;

            public NodeInfo GetNodeInfo()
            {
                return new NodeInfo
                {
                    Inputs = new Dictionary<string, object>
                    {
                        ["value"] = Text
                    },
                    ClassType = nameof(NmkdStringConstant)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class CheckpointLoaderSimple : Node, INode
        {
            public string CkptName = "";

            public NodeInfo GetNodeInfo()
            {
                return new NodeInfo
                {
                    Inputs = new Dictionary<string, object>
                    {
                        ["ckpt_name"] = CkptName
                    },
                    ClassType = nameof(CheckpointLoaderSimple)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class VAELoader : Node, INode
        {
            public string VaeName;

            public NodeInfo GetNodeInfo()
            {
                return new NodeInfo
                {
                    Inputs = { ["vae_name"] = VaeName },
                    ClassType = nameof(VAELoader)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
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
            public int ModelNode;
            public int PosPromptNode;
            public int NegPromptNode;
            public int LatentImageNode;

            public NodeInfo GetNodeInfo()
            {
                return new NodeInfo()
                {
                    Inputs = {
                        ["seed"] = Seed,
                        ["steps"] = Steps,
                        ["cfg"] = Cfg,
                        ["sampler_name"] = SamplerName,
                        ["scheduler"] = Scheduler,
                        ["denoise"] = Denoise,
                        ["model"] = new object[] { ModelNode.ToString(), 0 },
                        ["positive"] = new object[] { PosPromptNode.ToString(), 0 },
                        ["negative"] = new object[] { NegPromptNode.ToString(), 0 },
                        ["latent_image"] = new object[] { LatentImageNode.ToString(), 0 }
                    },
                    ClassType = "KSamplerAdvanced"
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class NmkdKSampler : Node, INode
        {
            public bool AddNoise = true;
            public string SamplerName = "dpmpp_2m";
            public string Scheduler = "normal";
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

            public NodeInfo GetNodeInfo()
            {
                if (DebugString.IsEmpty())
                    DebugString = Title;

                return new NodeInfo()
                {
                    Inputs = {
                        ["add_noise"] = AddNoise ? "enable" : "disable",
                        ["noise_seed"] = new object[] { NodeSeed.Id.ToString(), 0 },
                        ["steps"] = new object[] { NodeSteps.Id.ToString(), 0 },
                        ["cfg"] = new object[] { NodeCfg.Id.ToString(), 0 },
                        ["sampler_name"] = SamplerName,
                        ["scheduler"] = Scheduler,
                        ["start_at_step"] = new object[] { NodeStartStep.Id.ToString(), 0 },
                        ["end_at_step"] = new object[] { NodeEndStep.Id.ToString(), 0 },
                        ["return_with_leftover_noise"] = ReturnLeftoverNoise ? "enable" : "disable",
                        ["denoise"] = Denoise,
                        ["model"] = new object[] { NodeModel.Id.ToString(), 0 },
                        ["positive"] = new object[] { NodePositive.Id.ToString(), 0 },
                        ["negative"] = new object[] { NodeNegative.Id.ToString(), 0 },
                        ["latent_image"] = new object[] { NodeLatentImage.Id.ToString(), 0 },
                        ["debug_string"] = DebugString
                    },
                    ClassType = nameof(NmkdKSampler),
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class EmptyLatentImage : Node, INode
        {
            public int Width;
            public int Height;
            public int BatchSize = 1;

            public NodeInfo GetNodeInfo()
            {
                return new NodeInfo
                {
                    Inputs = {
                        ["width"] = Width,
                        ["height"] = Height,
                        ["batch_size"] = 1
                    },
                    ClassType = nameof(EmptyLatentImage)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class LatentUpscaleBy : Node, INode
        {
            public string UpscaleMethod = "nearest-exact";
            public float ScaleFactor = 1.5f;
            public int IdLatents;

            public NodeInfo GetNodeInfo()
            {
                return new NodeInfo
                {
                    Inputs = new Dictionary<string, object>
                    {
                        ["upscale_method"] = UpscaleMethod,
                        ["scale_by"] = ScaleFactor,
                        ["samples"] = new object[] { IdLatents, 0 }
                    },
                    ClassType = nameof(LatentUpscaleBy)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class LatentUpscale : Node, INode
        {
            public string UpscaleMethod = "nearest-exact";
            public int Width;
            public int Height;
            public INode LatentsNode;

            public NodeInfo GetNodeInfo()
            {
                return new NodeInfo
                {
                    Inputs = new Dictionary<string, object>
                    {
                        { "upscale_method", UpscaleMethod },
                        { "width", Width },
                        { "height", Height },
                        { "crop", "disabled" },
                        { "samples", new object[] { LatentsNode.Id.ToString(), 0 } }
                    },
                    ClassType = nameof(LatentUpscale)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class CLIPTextEncode : Node, INode
        {
            public INode TextNode;
            public INode ClipNode;

            public NodeInfo GetNodeInfo()
            {
                int clipNodeOutIndex = 0;
                if (ClipNode is NmkdCheckpointLoader) clipNodeOutIndex = 1;
                if (ClipNode is NmkdMultiLoraLoader) clipNodeOutIndex = 1;

                return new NodeInfo
                {
                    Inputs = {
                        ["text"] = new object[] { TextNode.Id.ToString(), 0 },
                        ["clip"] = new object[] { ClipNode.Id.ToString(), clipNodeOutIndex }
                    },
                    ClassType = nameof(CLIPTextEncode)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class VAEDecode : Node, INode
        {
            public INode LatentsNode;
            public INode VaeNode;

            public NodeInfo GetNodeInfo()
            {
                int vaeNodeOutIndex = 0;
                if (VaeNode is NmkdCheckpointLoader) vaeNodeOutIndex = 2;

                return new NodeInfo
                {
                    Inputs = new Dictionary<string, object>
                    {
                        ["samples"] = new object[] { LatentsNode.Id.ToString(), 0 },
                        ["vae"] = new object[] { VaeNode.Id.ToString(), vaeNodeOutIndex }
                    },
                    ClassType = nameof(VAEDecode)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class NmkdVaeEncode : Node, INode
        {
            public INode ImageNode;
            public INode VaeNode;
            public bool LoadMask = false;
            public int MaskGrowPixels = 6;

            public NodeInfo GetNodeInfo()
            {
                int vaeNodeOutIndex = 0;
                if (VaeNode is NmkdCheckpointLoader) vaeNodeOutIndex = 2;

                var inputs = new Dictionary<string, object>()
                {
                    { "pixels", new object[] { ImageNode.Id.ToString(), 0 } },
                    { "vae", new object[] { VaeNode.Id.ToString(), vaeNodeOutIndex } },
                    { "grow_mask_by", MaskGrowPixels },
                };

                if (LoadMask)
                {
                    inputs["mask"] = new object[] { ImageNode.Id.ToString(), 1 };
                }

                return new NodeInfo
                {
                    Inputs = inputs,
                    ClassType = nameof(NmkdVaeEncode)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class SaveImage : Node, INode
        {
            public string Prefix = "nmkd";
            public INode ImageNode;

            public NodeInfo GetNodeInfo()
            {
                var inputs = new Dictionary<string, object>()
                {
                    { "filename_prefix", Prefix },
                    { "images", new object[] { ImageNode.Id.ToString(), 0 } }
                };

                return new NodeInfo
                {
                    Inputs = inputs,
                    ClassType = nameof(SaveImage)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class NmkdCheckpointLoader : Node, INode
        {
            public string ModelPath;
            public bool LoadVae;
            public string VaePath;

            public NodeInfo GetNodeInfo()
            {
                var dict = new Dictionary<string, object>()
                {
                    { "mdl_path", ModelPath },
                    { "load_vae", LoadVae ? "enable" : "disable" },
                    { "vae_path", VaePath },
                };

                return new NodeInfo
                {
                    Inputs = dict,
                    ClassType = nameof(NmkdCheckpointLoader)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class NmkdMultiLoraLoader : Node, INode
        {
            public List<string> Loras;
            public List<float> Strengths;
            public INode ModelNode;
            public INode ClipNode;

            public NodeInfo GetNodeInfo()
            {
                int clipNodeIndex = 0;
                if (ClipNode is NmkdCheckpointLoader || ClipNode is CheckpointLoaderSimple)
                    clipNodeIndex = 1;

                var inputsDict = new Dictionary<string, object>()
                {
                    { "lora_paths", string.Join(",", Loras.Select(l => Path.Combine(Paths.GetLorasPath(false), l + ".safetensors"))) },
                    { "lora_strengths", string.Join(",", Strengths.Select(w => w.ToStringDot())) },
                    { "model", new object[] { ModelNode.Id.ToString(), 0 } },
                    { "clip", new object[] { ClipNode.Id.ToString(), clipNodeIndex } },
                };

                return new NodeInfo
                {
                    Inputs = inputsDict,
                    ClassType = nameof(NmkdMultiLoraLoader)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class NmkdImageLoader : Node, INode
        {
            public string ImagePath;

            public NodeInfo GetNodeInfo()
            {
                var dict = new Dictionary<string, object>
                {
                    { "image_path", ImagePath }
                };

                return new NodeInfo
                {
                    Inputs = dict,
                    ClassType = nameof(NmkdImageLoader)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class NmkdImageUpscale : Node, INode
        {
            public string UpscaleModelPath;
            public INode ImageNode;

            public NodeInfo GetNodeInfo()
            {
                return new NodeInfo
                {
                    Inputs = new Dictionary<string, object>
                    {
                        ["model_path"] = UpscaleModelPath,
                        ["image"] = new object[] { ImageNode.Id.ToString(), 0 }
                    },
                    ClassType = nameof(NmkdImageUpscale)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class CLIPSetLastLayer : Node, INode
        {
            public int Skip = -1;
            public INode ClipNode;

            public NodeInfo GetNodeInfo()
            {
                int clipNodeIndex = 0;
                if (ClipNode is NmkdCheckpointLoader || ClipNode is CheckpointLoaderSimple) clipNodeIndex = 1;

                var inputs = new Dictionary<string, object>
                {
                    { "stop_at_clip_layer", Skip },
                    { "clip", new object[] { ClipNode.Id.ToString(), clipNodeIndex } }
                };

                return new NodeInfo
                {
                    Inputs = inputs,
                    ClassType = nameof(CLIPSetLastLayer)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public static string ToStringNode(INode node)
        {
            var baseKeys = new Node().GetPublicVariableValues().Select(kvp => kvp.Key).ToList();

            List<KeyValuePair<string, object>> variables = new List<KeyValuePair<string, object>>();

            var fields = node.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var s = field.FieldType.IsPrimitive ? field.GetValue(node) : (object)(field.GetValue(node) != null ? $"#{node.Id}" : "null");
                variables.Add(new KeyValuePair<string, object>(field.Name, s));
            }

            var properties = node.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead);  // Ensure property can be read
            foreach (var property in properties)
            {
                var s = property.PropertyType.IsPrimitive ? property.GetValue(node) : (object)(property.GetValue(node) != null ? $"#{node.Id}" : "null");
                variables.Add(new KeyValuePair<string, object>(property.Name, s));
            }

            return $"[{node.Id}] {node.GetType().Name} '{node.Title}' - {string.Join(", ", variables.Where(kvp => !baseKeys.Contains(kvp.Key)).Select(v => $"{v.Key}: {v.Value}"))}";
        }
    }
}
