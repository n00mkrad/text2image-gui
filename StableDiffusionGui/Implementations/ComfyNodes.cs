using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using static StableDiffusionGui.Implementations.ComfyNodes;
using static StableDiffusionGui.Implementations.ComfyWorkflow;

namespace StableDiffusionGui.Implementations
{
    public class ComfyNodes
    {
        public enum ConnectionType { Model, Vae, Clip, Conditioning, Latent, Image, Mask, Upscale_Model }

        public interface INode
        {
            string Id { get; set; }
            string Title { get; set; }
            // List<ConnectionType> InputTypes { get; set; }
            // List<ConnectionType> OutputTypes { get; set; }
            // Dictionary<string, object> Inputs { get; set; }
            // string ClassType { get; set; }
            NodeInfo GetNodeInfo();
        }

        public static int GetOutIndex(INode nodeClass, ConnectionType outType)
        {
            if (outType == ConnectionType.Vae)
            {
                if (nodeClass is NmkdCheckpointLoader) return 2;
                if (nodeClass is Conv2dSettings) return 1;
            }
            else if (outType == ConnectionType.Clip)
            {
                if (nodeClass is NmkdCheckpointLoader) return 1;
                if (nodeClass is NmkdMultiLoraLoader) return 1;
                if (nodeClass is Conv2dSettings) return 2;
            }
            else if (outType == ConnectionType.Mask)
            {
                if (nodeClass is NmkdImageLoader) return 1;
            }

            return 0;
        }

        public class ComfyInput
        {
            object Value = null;
            INode Node = null;
            int NodeOutIndex = -1;
            ConnectionType OutType = (ConnectionType)(-1);

            public ComfyInput(object value)
            {
                Value = value;
            }

            public ComfyInput(INode node, ConnectionType outType = (ConnectionType)(-1))
            {
                int outIndex = outType == (ConnectionType)(-1) ? 0 : GetOutIndex(node, outType);
                Node = node;
                NodeOutIndex = outIndex;
            }

            public ComfyInput(INode node, int outIndex = 0)
            {
                Node = node;
                NodeOutIndex = outIndex;
            }

            public object Get ()
            {
                if(Value != null)
                    return Value;

                if (Node == null)
                    return "";

                if(OutType != (ConnectionType)(-1))
                {
                    int outIndex = GetOutIndex(Node, OutType);
                    return new object[] { Node.Id, outIndex };
                }
                else if (NodeOutIndex >= 0)
                {
                    return new object[] { Node.Id, NodeOutIndex };
                }

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
                    Inputs = new Dictionary<string, object> { ["vae_name"] = VaeName },
                    OutputTypes = new List<ConnectionType> { ConnectionType.Vae },
                    ClassType = nameof(VAELoader)
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
            public ComfyInput StartStep = new ComfyInput(0);
            public ComfyInput EndStep = new ComfyInput(10000);
            public ComfyInput Cfg;

            public string DebugString = "";

            public NodeInfo GetNodeInfo()
            {
                // if (DebugString.IsEmpty())
                //     DebugString = Title;

                return new NodeInfo()
                {
                    Inputs = {
                        ["add_noise"] = AddNoise ? "enable" : "disable",
                        ["noise_seed"] = Seed.Get(),
                        ["control_after_generate"] = "fixed",
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
                        // ["debug_string"] = DebugString
                    },
                    InputTypes = new Dictionary<string, ConnectionType>
                    {
                        ["model"] = ConnectionType.Model,
                        ["positive"] = ConnectionType.Conditioning,
                        ["negative"] = ConnectionType.Conditioning,
                        ["latent_image"] = ConnectionType.Latent,
                    },
                    OutputTypes = new List<ConnectionType>
                    {
                        ConnectionType.Latent,
                    },
                    ClassType = nameof(NmkdKSampler),
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }


        public class NmkdHybridSampler : Node, INode
        {
            public bool AddNoise = true;
            public long Seed;
            public int Steps;
            public int BaseSteps;
            public float Cfg;
            public string SamplerName = "dpmpp_2m_sde";
            public string Scheduler = "karras";
            public int StartStep = 0;
            public int EndStep = 10000;
            public bool ReturnLeftoverNoise = false;
            public float Denoise = 1f;

            public ComfyInput Model;
            public ComfyInput ModelRefiner;
            public ComfyInput PositiveCond;
            public ComfyInput NegativeCond;
            public ComfyInput RefinerPositiveCond;
            public ComfyInput RefinerNegativeCond;
            public ComfyInput LatentImage;

            public NodeInfo GetNodeInfo()
            {
                return new NodeInfo()
                {
                    Inputs = {
                        { "add_noise", AddNoise ? "enable" : "disable" },
                        { "noise_seed", Seed },
                        { "steps", Steps },
                        { "refiner_switch_step", BaseSteps + 1 },
                        { "cfg", Cfg },
                        { "sampler_name", SamplerName },
                        { "scheduler", Scheduler },
                        { "start_at_step", StartStep },
                        { "end_at_step", EndStep },
                        { "return_with_leftover_noise", ReturnLeftoverNoise ? "enable" : "disable" },
                        { "denoise", Denoise },
                        { "sharpness", 1.0f },
                        { "model", Model.Get() },
                        { "refiner_model", ModelRefiner.Get() },
                        { "positive", PositiveCond.Get() },
                        { "negative", NegativeCond.Get() },
                        { "refiner_positive", RefinerPositiveCond.Get() },
                        { "refiner_negative", RefinerNegativeCond.Get() },
                        { "latent_image", LatentImage.Get() },
                    },
                    ClassType = nameof(NmkdHybridSampler),
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class DynamicThresholdingSimple : Node, INode
        {
            public float MimicScale = 1.0f;
            public float ThresholdPercentile = 1.0f;
            public ComfyInput Model;

            public NodeInfo GetNodeInfo()
            {
                return new NodeInfo
                {
                    Inputs = {
                        ["mimic_scale"] = MimicScale,
                        ["threshold_percentile"] = ThresholdPercentile,
                        ["model"] = Model.Get(),
                    },
                    OutputTypes = new List<ConnectionType> { ConnectionType.Model },
                    ClassType = nameof(DynamicThresholdingSimple)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class DynamicThresholdingFull : Node, INode
        {
            public float MimicScale = 1.0f;
            public float ThresholdPercentile = 1.0f;
            public float InterpolatePhi = 0.6f;
            public ComfyInput Model;

            public NodeInfo GetNodeInfo()
            {
                return new NodeInfo
                {
                    Inputs = {
                        ["mimic_scale"] = MimicScale,
                        ["threshold_percentile"] = ThresholdPercentile,
                        ["mimic_mode"] = "Sawtooth",
                        ["mimic_scale_min"] = 0,
                        ["cfg_mode"] = "Constant",
                        ["cfg_scale_min"] = 0,
                        ["sched_val"] = 1,
                        ["separate_feature_channels"] = "enable",
                        ["scaling_startpoint"] = "ZERO",
                        ["variability_measure"] = "STD",
                        ["interpolate_phi"] = InterpolatePhi,
                        ["model"] = Model.Get(),
                    },
                    OutputTypes = new List<ConnectionType> { ConnectionType.Model },
                    ClassType = nameof(DynamicThresholdingFull)
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
                    InputTypes = new Dictionary<string, ConnectionType>
                    {
                        ["samples"] = ConnectionType.Latent
                    },
                    OutputTypes = new List<ConnectionType> { ConnectionType.Latent },
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
                    OutputTypes = new List<ConnectionType> { ConnectionType.Clip },
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
                    OutputTypes = new List<ConnectionType> { ConnectionType.Image },
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
                    OutputTypes = new List<ConnectionType> { ConnectionType.Latent },
                    ClassType = nameof(NmkdVaeEncode)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class NmkdSaveImage : Node, INode
        {
            public string Prefix = "nmkd";
            public string OverridePath = "";
            public ComfyInput Image;

            public NodeInfo GetNodeInfo()
            {
                var inputs = new Dictionary<string, object>()
                {
                    { "filename_prefix", Prefix },
                    { "images", Image.Get() }
                };

                if (OverridePath.IsNotEmpty())
                    inputs["override_save_path"] = OverridePath;

                return new NodeInfo
                {
                    Inputs = inputs,
                    ClassType = nameof(NmkdSaveImage)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class NmkdCheckpointLoader : Node, INode
        {
            public string ModelPath = "";
            public int ClipSkip = -1;
            public bool LoadVae = true;
            public string VaePath = "";
            public string EmbeddingsDir = "";

            public NodeInfo GetNodeInfo()
            {
                return new NodeInfo
                {
                    Inputs = new Dictionary<string, object>()
                    {
                        { "mdl_path", ModelPath },
                        { "load_vae", LoadVae ? "enable" : "disable" },
                        { "vae_path", VaePath },
                        { "embeddings_dir", EmbeddingsDir },
                        { "clip_skip", ClipSkip },
                    },
                    OutputTypes = new List<ConnectionType>
                    {
                        ConnectionType.Model,
                        ConnectionType.Clip,
                        ConnectionType.Vae,
                    },
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
                    OutputTypes = new List<ConnectionType> { ConnectionType.Model, ConnectionType.Clip },
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
                    OutputTypes = new List<ConnectionType> { ConnectionType.Image, ConnectionType.Mask },
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
                    OutputTypes = new List<ConnectionType> { ConnectionType.Image },
                    ClassType = nameof(NmkdImageUpscale)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class NmkdUpscaleModelLoader : Node, INode
        {
            public string UpscaleModelPath;

            public NodeInfo GetNodeInfo()
            {
                return new NodeInfo
                {
                    Inputs = new Dictionary<string, object>
                    {
                        { "model_path", UpscaleModelPath },
                    },
                    OutputTypes = new List<ConnectionType> { ConnectionType.Upscale_Model },
                    ClassType = nameof(NmkdUpscaleModelLoader)
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
                    OutputTypes = new List<ConnectionType> { ConnectionType.Clip },
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
                    InputTypes = new Dictionary<string, ConnectionType>
                    {
                        ["image_to"] = ConnectionType.Image,
                        ["image_from"] = ConnectionType.Image,
                    },
                    OutputTypes = new List<ConnectionType> { ConnectionType.Image },
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
                    OutputTypes = new List<ConnectionType> { ConnectionType.Conditioning },
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

                return new NodeInfo
                {
                    Inputs = dict,
                    OutputTypes = new List<ConnectionType> { ConnectionType.Model },
                    ClassType = nameof(NmkdHypernetworkLoader)
                };
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
                var dict = new Dictionary<string, object>
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
                    dict["coarse"] = "disable";
                }

                if (Preprocessor == Enums.StableDiffusion.ImagePreprocessor.LineArtHed)
                {
                    classType = "HEDPreprocessor";
                    dict["version"] = "v1.1";
                    dict["safe"] = "enable";
                }

                if (Preprocessor == Enums.StableDiffusion.ImagePreprocessor.Canny)
                {
                    classType = "Canny";
                    dict["low_threshold"] = 0.1f;
                    dict["high_threshold"] = 0.2f;
                }

                if (Preprocessor == Enums.StableDiffusion.ImagePreprocessor.Blur)
                {
                    classType = "ImageBlur";
                    dict["blur_radius"] = 15;
                    dict["sigma"] = 1f;
                }

                if (Preprocessor == Enums.StableDiffusion.ImagePreprocessor.Pixelate)
                {
                    classType = "NmkdColorPreprocessor";
                    dict["divisor"] = 64;
                }

                if (Preprocessor == Enums.StableDiffusion.ImagePreprocessor.OpenPose)
                {
                    classType = "OpenposePreprocessor";
                    dict["detect_hand"] = "enable";
                    dict["detect_body"] = "enable";
                    dict["detect_face"] = "disable";
                    dict["version"] = "v1.1";
                }

                return new NodeInfo
                {
                    Inputs = dict,
                    OutputTypes = new List<ConnectionType> { ConnectionType.Image },
                    ClassType = classType
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class NmkdDualTextEncode : Node, INode
        {
            public string Text1;
            public string Text2;
            public ComfyInput Clip;
            public float FluxGuidance;

            public NodeInfo GetNodeInfo()
            {
                var dict = new Dictionary<string, object>()
                {
                    { "text1", Text1 },
                    { "text2", Text2 },
                    { "clip", Clip.Get() },
                    { "flux_guidance", FluxGuidance },
                };

                return new NodeInfo
                {
                    Inputs = dict,
                    OutputTypes = new List<ConnectionType> { ConnectionType.Conditioning, ConnectionType.Conditioning },
                    ClassType = nameof(NmkdDualTextEncode)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class NmkdLatentImage : Node, INode
        {
            public int Width;
            public int Height;
            public bool Sd3Mode;

            public NodeInfo GetNodeInfo()
            {
                var dict = new Dictionary<string, object>()
                {
                    { "width", Width },
                    { "height", Height },
                    { "sd3", Sd3Mode ? "enable" : "disable" },
                };

                return new NodeInfo
                {
                    Inputs = dict,
                    OutputTypes = new List<ConnectionType> { ConnectionType.Latent },
                    ClassType = nameof(NmkdLatentImage)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class UltimateSDUpscale : Node, INode
        {
            public ComfyInput Image;
            public ComfyInput Model;
            public ComfyInput CondPositive;
            public ComfyInput CondNegative;
            public ComfyInput Vae;
            public ComfyInput UpscaleModel;
            public float UpscaleFactor = 1.5f;
            public long Seed = 0;
            public int Steps = 20;
            public float Scale = 7f;
            public string Sampler = "euler";
            public string Scheduler = "normal";
            public float Denoise = 0.5f;
            public Size TileSize = new Size(768, 768);
            public bool ForceUniformTiles = false;

            public NodeInfo GetNodeInfo()
            {
                return new NodeInfo
                {
                    Inputs = new Dictionary<string, object>
                    {
                        { "image", Image.Get() },
                        { "model", Model.Get() },
                        { "positive", CondPositive.Get() },
                        { "negative", CondNegative.Get() },
                        { "vae", Vae.Get() },
                        { "upscale_model", UpscaleModel.Get() },
                        { "upscale_by", UpscaleFactor },
                        { "seed", Seed },
                        { "steps", Steps },
                        { "cfg", Scale },
                        { "sampler_name", Sampler },
                        { "scheduler", Scheduler },
                        { "denoise", Denoise },
                        { "mode_type", "Linear" },
                        { "tile_width", TileSize.Width },
                        { "tile_height", TileSize.Height },
                        { "mask_blur", 8 },
                        { "tile_padding", 32 },
                        { "seam_fix_mode", "None" },
                        { "seam_fix_denoise", 1f },
                        { "seam_fix_width", 64 },
                        { "seam_fix_mask_blur", 8 },
                        { "seam_fix_padding", 16 },
                        { "force_uniform_tiles", ForceUniformTiles ? "enable" : "disable" },
                    },
                    InputTypes = new Dictionary<string, ConnectionType>
                    {
                        ["positive"] = ConnectionType.Conditioning,
                        ["negative"] = ConnectionType.Conditioning,
                        ["upscale_model"] = ConnectionType.Upscale_Model,
                    },
                    OutputTypes = new List<ConnectionType> { ConnectionType.Image },
                    ClassType = nameof(UltimateSDUpscale)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class Conv2dSettings : Node, INode
        {
            public ComfyInput Model;
            public ComfyInput Vae;
            public ComfyInput Clip;
            public string PaddingMode = "circular";

            public NodeInfo GetNodeInfo()
            {
                var dict = new Dictionary<string, object>()
                {
                    { "model", Model.Get() },
                    { "vae", Vae.Get() },
                    { "clip", Clip.Get() },
                    { "padding_mode", PaddingMode },
                };

                return new NodeInfo
                {
                    Inputs = dict,
                    OutputTypes = new List<ConnectionType> { ConnectionType.Model, ConnectionType.Vae, ConnectionType.Clip },
                    ClassType = nameof(Conv2dSettings)
                };
            }

            public override string ToString()
            {
                return ToStringNode(this);
            }
        }

        public class SudoLatentUpscale : Node, INode
        {
            public ComfyInput Version = new ComfyInput("DRCT-l_12x6_170k_l1_vaeDecode_l1_fft_xl");
            public ComfyInput Latents;

            public NodeInfo GetNodeInfo()
            {
                return new NodeInfo
                {
                    Inputs = {
                        ["version"] = Version.Get(),
                        ["latent"] = Latents.Get(),
                    },
                    OutputTypes = new List<ConnectionType> { ConnectionType.Latent },
                    ClassType = nameof(SudoLatentUpscale)
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
