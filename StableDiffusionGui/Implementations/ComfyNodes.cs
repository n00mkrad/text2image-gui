using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static StableDiffusionGui.Implementations.ComfyWorkflow;

namespace StableDiffusionGui.Implementations
{
    public class ComfyNodes
    {
        public enum OutType { Model, Vae, Clip, Conditioning, Latents, Image, Mask }

        public interface INode
        {
            string Id { get; set; }
            string Title { get; set; }
            NodeInfo GetNodeInfo();
        }

        public static int GetOutIndex(INode nodeClass, OutType outType)
        {
            if (outType == OutType.Vae)
            {
                if (nodeClass is NmkdCheckpointLoader) return 2;
            }
            else if (outType == OutType.Clip)
            {
                if (nodeClass is NmkdCheckpointLoader) return 1;
                if (nodeClass is NmkdMultiLoraLoader) return 1;
            }
            else if (outType == OutType.Mask)
            {
                if (nodeClass is NmkdImageLoader) return 2;
            }

            return 0;
        }

        public class ComfyInput
        {
            object Value = null;
            object[] NodeOutput = new object[0];

            public ComfyInput(object value)
            {
                Value = value;
            }

            public ComfyInput(INode node, OutType outType = (OutType)(-1))
            {
                int outputIndex = outType == (OutType)(-1) ? 0 : GetOutIndex(node, outType);
                NodeOutput = new object[] { node.Id.ToString(), outputIndex };
            }

            public object Get ()
            {
                if (NodeOutput.Length == 2)
                    return NodeOutput;
                else
                    return Value;
            }
        }

        #region Constants

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

        #endregion

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
            public ComfyInput Model;
            public ComfyInput PosPrompt;
            public ComfyInput NegPrompt;
            public ComfyInput LatentImage;

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
                        ["model"] = Model.Get(),
                        ["positive"] = PosPrompt.Get(),
                        ["negative"] = NegPrompt.Get(),
                        ["latent_image"] = LatentImage.Get(),
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

            public ComfyInput Model;
            public ComfyInput PositiveCond;
            public ComfyInput NegativeCond;
            public ComfyInput LatentImage;
            public ComfyInput Seed;
            public ComfyInput StepsTotal;
            public ComfyInput StartStep;
            public ComfyInput EndStep;
            public ComfyInput Cfg;

            public string DebugString = "";

            public NodeInfo GetNodeInfo()
            {
                if (DebugString.IsEmpty())
                    DebugString = Title;

                return new NodeInfo()
                {
                    Inputs = {
                        ["add_noise"] = AddNoise ? "enable" : "disable",
                        ["noise_seed"] = Seed.Get(),
                        ["steps"] = StepsTotal.Get(),
                        ["cfg"] = Cfg.Get(),
                        ["sampler_name"] = SamplerName,
                        ["scheduler"] = Scheduler,
                        ["start_at_step"] = StartStep.Get(),
                        ["end_at_step"] = EndStep.Get(),
                        ["return_with_leftover_noise"] = ReturnLeftoverNoise ? "enable" : "disable",
                        ["denoise"] = Denoise,
                        ["model"] = Model.Get(),
                        ["positive"] = PositiveCond.Get(),
                        ["negative"] = NegativeCond.Get(),
                        ["latent_image"] = LatentImage.Get(),
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
                        ["batch_size"] = BatchSize,
                    },
                    ClassType = nameof(EmptyLatentImage)
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
            public ComfyInput Latents;

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
                        { "samples", Latents.Get() }
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
            public ComfyInput Text;
            public ComfyInput Clip;

            public NodeInfo GetNodeInfo()
            {
                return new NodeInfo
                {
                    Inputs = {
                        ["text"] = Text.Get(),
                        ["clip"] = Clip.Get(),
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
            public ComfyInput Latents;
            public ComfyInput Vae;

            public NodeInfo GetNodeInfo()
            {
                return new NodeInfo
                {
                    Inputs = new Dictionary<string, object>
                    {
                        ["samples"] = Latents.Get(),
                        ["vae"] = Vae.Get(),
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
            public ComfyInput Image;
            public ComfyInput Mask;
            public ComfyInput Vae;
            public bool UseMask = false;
            public int MaskGrowPixels = 6;

            public NodeInfo GetNodeInfo()
            {
                var inputs = new Dictionary<string, object>()
                {
                    { "grow_mask_by", MaskGrowPixels },
                    { "pixels", Image.Get() },
                    { "vae", Vae.Get() },
                };

                if (UseMask)
                {
                    inputs["mask"] = Mask.Get();
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
            public ComfyInput Image;

            public NodeInfo GetNodeInfo()
            {
                var inputs = new Dictionary<string, object>()
                {
                    { "filename_prefix", Prefix },
                    { "images", Image.Get() }
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
            public string EmbeddingsDir;

            public NodeInfo GetNodeInfo()
            {
                var dict = new Dictionary<string, object>()
                {
                    { "mdl_path", ModelPath },
                    { "load_vae", LoadVae ? "enable" : "disable" },
                    { "vae_path", VaePath },
                    { "embeddings_dir", EmbeddingsDir },
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
            public List<KeyValuePair<string,float>> Loras;
            public ComfyInput Model;
            public ComfyInput Clip;

            public NodeInfo GetNodeInfo()
            {
                var inputsDict = new Dictionary<string, object>()
                {
                    { "lora_paths", string.Join(",", Loras.Select(l => Path.Combine(Paths.GetLorasPath(false), l.Key + ".safetensors"))) },
                    { "lora_strengths", string.Join(",", Loras.Select(w => w.Value.ToStringDot())) },
                    { "model", Model.Get() },
                    { "clip", Clip.Get() },
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
            public ComfyInput Image;

            public NodeInfo GetNodeInfo()
            {
                return new NodeInfo
                {
                    Inputs = new Dictionary<string, object>
                    {
                        { "model_path", UpscaleModelPath },
                        { "image", Image.Get() },
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
            public ComfyInput ClipNode;

            public NodeInfo GetNodeInfo()
            {
                var inputs = new Dictionary<string, object>
                {
                    { "stop_at_clip_layer", Skip },
                    { "clip", ClipNode.Get() }
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

        public class NmkdImageMaskComposite : Node, INode
        {
            public ComfyInput ImageTo;
            public ComfyInput ImageFrom;
            public ComfyInput Mask;

            public NodeInfo GetNodeInfo()
            {
                var inputs = new Dictionary<string, object>
                {
                    { "image_to", ImageTo.Get() },
                    { "image_from", ImageFrom.Get() },
                    { "mask", Mask.Get() },
                };

                return new NodeInfo
                {
                    Inputs = inputs,
                    ClassType = nameof(NmkdImageMaskComposite)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class NmkdControlNet : Node, INode
        {
            public string ModelPath;
            public float Strength = 1.0f;
            public ComfyInput Conditioning;
            public ComfyInput Image;
            public ComfyInput Model;

            public NodeInfo GetNodeInfo()
            {
                var inputs = new Dictionary<string, object>
                {
                    { "controlnet_path", ModelPath },
                    { "strength", Strength },
                    { "conditioning", Conditioning.Get() },
                    { "image", Image.Get() },
                };

                if(Model != null)
                {
                    inputs["model"] = Model.Get();
                }

                return new NodeInfo
                {
                    Inputs = inputs,
                    ClassType = nameof(NmkdControlNet)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class NmkdHypernetworkLoader : Node, INode
        {
            public ComfyInput Model;
            public string ModelPath;
            public float Strength;

            public NodeInfo GetNodeInfo()
            {
                var dict = new Dictionary<string, object>()
                {
                    { "model", Model.Get() },
                    { "hypernetwork_path", ModelPath },
                    { "strength", Strength },
                };

                return new NodeInfo { Inputs = dict, ClassType = nameof(NmkdHypernetworkLoader) };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class GenericImagePreprocessor : Node, INode
        {
            public ComfyInput Image;
            public Enums.StableDiffusion.ImagePreprocessor Preprocessor;

            public NodeInfo GetNodeInfo()
            {
                var inputs = new Dictionary<string, object>
                {
                    { "image", Image.Get() },
                };

                string classType = "";

                // if (Preprocessor == Enums.StableDiffusion.ImagePreprocessor.Scribble)
                //     classType = "ScribblePreprocessor";

                if (Preprocessor == Enums.StableDiffusion.ImagePreprocessor.DepthMap)
                    classType = "Zoe-DepthMapPreprocessor";
                    
                if (Preprocessor == Enums.StableDiffusion.ImagePreprocessor.LineArtAnime)
                    classType = "AnimeLineArtPreprocessor";

                if (Preprocessor == Enums.StableDiffusion.ImagePreprocessor.LineArtMangaAnime)
                    classType = "Manga2Anime-LineArtPreprocessor";

                if (Preprocessor == Enums.StableDiffusion.ImagePreprocessor.LineArt)
                {
                    classType = "LineArtPreprocessor";
                    inputs["coarse"] = "disable";
                }

                if (Preprocessor == Enums.StableDiffusion.ImagePreprocessor.LineArtHed)
                {
                    classType = "HEDPreprocessor";
                    inputs["version"] = "v1.1";
                    inputs["safe"] = "enable";
                }

                if (Preprocessor == Enums.StableDiffusion.ImagePreprocessor.Canny)
                {
                    classType = "Canny";
                    inputs["low_threshold"] = 0.1f;
                    inputs["high_threshold"] = 0.2f;
                }

                if (Preprocessor == Enums.StableDiffusion.ImagePreprocessor.Blur)
                {
                    classType = "ImageBlur";
                    inputs["blur_radius"] = 15;
                    inputs["sigma"] = 1f;
                }

                if (Preprocessor == Enums.StableDiffusion.ImagePreprocessor.Pixelate)
                {
                    classType = "NmkdColorPreprocessor";
                    inputs["divisor"] = 64;
                }

                if (Preprocessor == Enums.StableDiffusion.ImagePreprocessor.OpenPose)
                {
                    classType = "OpenposePreprocessor";
                    inputs["detect_hand"] = "enable";
                    inputs["detect_body"] = "enable";
                    inputs["detect_face"] = "disable";
                    inputs["version"] = "v1.1";
                }

                return new NodeInfo { Inputs = inputs, ClassType = classType };
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
