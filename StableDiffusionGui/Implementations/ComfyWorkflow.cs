using ImageMagick;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using StableDiffusionGui.Data;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static StableDiffusionGui.Implementations.ComfyNodes;
using System.IO;
using StableDiffusionGui.Io;
using Newtonsoft.Json.Converters;

namespace StableDiffusionGui.Implementations
{
    public class ComfyWorkflow
    {
        public class Node
        {
            public string Id { get; set; }
            public string Title { get; set; } = "";

            public Node() { }

            public Node(string id, string title = "")
            {
                Id = id;
                Title = title;
            }

            // public override string ToString()
            // {
            //     return $"#{Id} {(Title.IsNotEmpty() ? $" - {Title}" : "")}";
            // }
        }

        public class PromptRequest
        {
            public string ClientId { get; set; }
            public EasyDict<string, NodeInfo> Prompt { get; set; } = new EasyDict<string, NodeInfo>();
            public ExtraDataClass ExtraData { get; set; } = new ExtraDataClass();

            public class ExtraDataClass
            {
                public EasyDict<string, object> ExtraPnginfo { get; set; } = new EasyDict<string, object>();
            }

            public override string ToString()
            {
                return Serialize();
            }

            public string Serialize(bool indent = false)
            {
                var format = indent ? Formatting.Indented : Formatting.None;
                var settings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() } };
                settings.Converters.Add(new StringEnumConverter());
                return JsonConvert.SerializeObject(this, format, settings);
            }
        }

        public class NodeInfo
        {
            public Dictionary<string, object> Inputs { get; set; } = new Dictionary<string, object>();
            public string ClassType { get; set; }
        }

        private static List<INode> _currentNodes = new List<INode>();

        public static EasyDict<string, NodeInfo> GetPromptInfos(ComfyData.GenerationInfo g, List<INode> nodes = null)
        {
            if (nodes == null)
                nodes = new List<INode>();

            _currentNodes = nodes;
            var nodeInfos = new EasyDict<string, NodeInfo>();
            bool refine = g.RefinerStrength > 0.001f;
            bool upscale = !g.TargetResolution.IsEmpty && g.TargetResolution != g.BaseResolution;
            bool inpaint = g.MaskPath.IsNotEmpty();
            bool controlnet = g.Controlnets.Any(c => c.Model.IsNotEmpty() && c.Strength > 0.001f);
            bool img2img = g.InitImg.IsNotEmpty() && !controlnet;
            int baseSteps = (g.Steps * (1f - g.RefinerStrength)).RoundToInt();
            int baseWidth = g.BaseResolution.Width;
            int baseHeight = g.BaseResolution.Height;

            var model1 = AddNode<NmkdCheckpointLoader>(); // Base Model Loader
            model1.ModelPath = g.Model;
            model1.ClipSkip = g.ClipSkip;
            model1.LoadVae = true;
            model1.VaePath = g.Vae ?? "";
            model1.EmbeddingsDir = Io.Paths.ReturnDir(Config.Instance.EmbeddingsDir, true, true);

            INode finalModelNode = model1; // Keep track of the final model node to use

            if (g.Loras.Any(l => l.Value > 0.001f)) // Add LoRAs
            {
                var loraLoader = AddNode<NmkdMultiLoraLoader>(); // LoRA Loader
                loraLoader.Loras = g.Loras;
                loraLoader.Model = new ComfyInput(finalModelNode, OutType.Model);
                loraLoader.Clip = new ComfyInput(model1, OutType.Clip);
                finalModelNode = loraLoader;
            }

            foreach (var hypernetModelStrength in g.Hypernetworks) // Add hypernetworks
            {
                var hypernetLoader = AddNode<NmkdHypernetworkLoader>(); // Hypernetwork Loader
                hypernetLoader.Model = new ComfyInput(finalModelNode, OutType.Model);
                hypernetLoader.ModelPath = hypernetModelStrength.Key;
                hypernetLoader.Strength = hypernetModelStrength.Value;
                finalModelNode = hypernetLoader;
            }

            NmkdCheckpointLoader model2 = null;

            if (g.ModelRefiner.IsNotEmpty())
            {
                model2 = AddNode<NmkdCheckpointLoader>(); // Refiner Model Loader
                model2.ModelPath = g.ModelRefiner == null ? g.Model : g.ModelRefiner;
                model2.ClipSkip = g.ClipSkip;
                model2.LoadVae = false;
                model2.EmbeddingsDir = Io.Paths.ReturnDir(Config.Instance.EmbeddingsDir, true, true);
            }

            var encodePromptBase = AddNode<NmkdDualTextEncode>(); // CLIP Encode Prompt Base
            encodePromptBase.Text1 = g.Prompt;
            encodePromptBase.Text2 = g.NegativePrompt;
            encodePromptBase.Clip = new ComfyInput(finalModelNode, OutType.Clip);

            NmkdDualTextEncode encodePromptRefiner = null;

            if (refine)
            {
                encodePromptRefiner = AddNode<NmkdDualTextEncode>(); // CLIP Encode Prompt Refiner
                encodePromptRefiner.Text1 = g.Prompt;
                encodePromptRefiner.Text2 = g.NegativePrompt;
                encodePromptRefiner.Clip = new ComfyInput(model2, OutType.Clip);
            }

            INode initImageNode = null; // Keep track of the init image to use
            INode finalLatentsNode = null;

            if (g.InitImg.IsNotEmpty())
            {
                var loadInitImg = AddNode<NmkdImageLoader>(); // Load Init Image
                loadInitImg.ImagePath = g.MaskPath.IsNotEmpty() ? g.MaskPath : g.InitImg;
                initImageNode = loadInitImg;
            }

            if (!img2img)
            {
                var emptyLatentImg = AddNode<EmptyLatentImage>(); // Empty Latent Image (T2I)
                emptyLatentImg.Width = baseWidth;
                emptyLatentImg.Height = baseHeight;
                finalLatentsNode = emptyLatentImg;
            }
            else
            {
                var encodeInitImg = AddNode<NmkdVaeEncode>(); // Encode Init Image (I2I)
                encodeInitImg.Image = new ComfyInput(initImageNode, OutType.Image);
                encodeInitImg.Mask = new ComfyInput(initImageNode, OutType.Mask);
                encodeInitImg.Vae = new ComfyInput(model1, OutType.Vae);
                encodeInitImg.UseMask = g.MaskPath.IsNotEmpty();
                finalLatentsNode = encodeInitImg;
            }

            INode finalConditioningNode = encodePromptBase; // Keep track of the final conditioning node for sampling

            for (int i = 0; i < g.Controlnets.Count; i++) // Apply ControlNets
            {
                ComfyData.ControlnetInfo ci = g.Controlnets[i];

                if (ci.Preprocessor != Enums.StableDiffusion.ImagePreprocessor.None)
                {
                    nodes.Add(new GenericImagePreprocessor
                    {
                        Image = new ComfyInput(initImageNode, OutType.Image),
                        Preprocessor = ci.Preprocessor,
                        Id = GetNewId("Preproc")
                    });
                }

                nodes.Add(new NmkdControlNet
                {
                    ModelPath = ci.Model,
                    Strength = ci.Strength,
                    Conditioning = new ComfyInput(finalConditioningNode, OutType.Conditioning),
                    Image = new ComfyInput(ci.Preprocessor != Enums.StableDiffusion.ImagePreprocessor.None ? nodes.Last() : initImageNode, OutType.Image),
                    Model = new ComfyInput(model1, OutType.Model),
                    Id = GetNewId("ControlNet"),
                });

                finalConditioningNode = nodes.Last();
            }

            INode samplerNode = null;

            if (refine)
            {
                var samplerBase = AddNode<NmkdHybridSampler>("SamplerHybrid"); // Sampler Hybrid
                samplerBase.AddNoise = true;
                samplerBase.Seed = g.Seed;
                samplerBase.Steps = g.Steps;
                samplerBase.BaseSteps = baseSteps;
                samplerBase.Cfg = g.Scale;
                samplerBase.SamplerName = GetComfySampler(g.Sampler);
                samplerBase.Scheduler = GetComfyScheduler(g);
                samplerBase.Model = new ComfyInput(finalModelNode, OutType.Model);
                samplerBase.ModelRefiner = new ComfyInput(model2, OutType.Model);
                samplerBase.PositiveCond = new ComfyInput(finalConditioningNode, 0);
                samplerBase.NegativeCond = new ComfyInput(encodePromptBase, 1);
                samplerBase.RefinerPositiveCond = new ComfyInput(encodePromptRefiner, 0);
                samplerBase.RefinerNegativeCond = new ComfyInput(encodePromptRefiner, 1);
                samplerBase.LatentImage = new ComfyInput(finalLatentsNode, OutType.Latents);
                // samplerBase.Denoise = img2img ? g.InitStrength : 1.0f;
                finalLatentsNode = samplerBase;
                samplerNode = samplerBase;
            }
            else
            {
                var samplerBase = AddNode<NmkdKSampler>("SamplerBase"); // Sampler Base
                samplerBase.AddNoise = true;
                samplerBase.Model = new ComfyInput(finalModelNode, OutType.Model);
                samplerBase.PositiveCond = new ComfyInput(finalConditioningNode, OutType.Conditioning);
                samplerBase.NegativeCond = new ComfyInput(encodePromptBase, 1);
                samplerBase.LatentImage = new ComfyInput(finalLatentsNode, OutType.Latents);
                samplerBase.SamplerName = GetComfySampler(g.Sampler);
                samplerBase.Scheduler = GetComfyScheduler(g);
                samplerBase.Seed = new ComfyInput(g.Seed);
                samplerBase.StepsTotal = new ComfyInput(g.Steps);
                samplerBase.Cfg = new ComfyInput(g.Scale);
                samplerBase.StartStep = new ComfyInput(0);
                samplerBase.EndStep = new ComfyInput(baseSteps);
                samplerBase.ReturnLeftoverNoise = refine;
                samplerBase.Denoise = img2img ? g.InitStrength : 1.0f;
                finalLatentsNode = samplerBase;
                samplerNode = samplerBase;
            }

            if (g.UpscaleMethod == Enums.StableDiffusion.UpscaleMethod.Latent) // Latent upscaling
            {
                var latentUpscale = AddNode<LatentUpscale>(); // Latent Upscale
                latentUpscale.Width = g.TargetResolution.Width;
                latentUpscale.Height = g.TargetResolution.Height;
                latentUpscale.Latents = new ComfyInput(samplerNode, OutType.Latents);
                
                var samplerHires = AddNode<NmkdKSampler>("SamplerHires"); // Sampler Hi-Res
                samplerHires.AddNoise = true;
                samplerHires.Model = new ComfyInput(finalModelNode, OutType.Model);
                samplerHires.PositiveCond = new ComfyInput(finalConditioningNode, OutType.Conditioning);
                samplerHires.NegativeCond = new ComfyInput(encodePromptBase, 1);
                samplerHires.LatentImage = new ComfyInput(latentUpscale, OutType.Latents);
                samplerHires.SamplerName = GetComfySampler(g.Sampler);
                samplerHires.Scheduler = "simple"; // GetComfyScheduler(g);
                samplerHires.Seed = new ComfyInput(g.Seed);
                samplerHires.StepsTotal = new ComfyInput((g.Steps * 0.5f).RoundToInt());
                samplerHires.Cfg = new ComfyInput(g.Scale);
                finalLatentsNode = samplerHires;
            }

            var vaeDecode = AddNode<VAEDecode>(); // Final VAE Decode
            vaeDecode.Vae = new ComfyInput(model1, OutType.Vae);
            vaeDecode.Latents = new ComfyInput(finalLatentsNode, OutType.Latents);

            INode finalImageNode = vaeDecode; // Keep track of the final image output node to use

            if(g.UpscaleMethod == Enums.StableDiffusion.UpscaleMethod.UltimateSd) // Ultimate SD Upscaler
            {
                var upscalerModelNode = finalModelNode;
                var conditioningNode = encodePromptBase;
                bool xl = Config.Instance.ModelSettings.Get(Path.GetFileName(g.Model), new Models.ModelSettings()).Arch == Enums.StableDiffusion.ModelArch.SdXlBase;

                if (xl) // Load SD1 model as we can't use XL for USDU yet
                {
                    upscalerModelNode = AddNode<NmkdCheckpointLoader>("UpscalingModelSD"); // Base Model Loader
                    ((NmkdCheckpointLoader)upscalerModelNode).ModelPath = "M:\\Weights\\SD\\SafetensorsNew\\dreamshaper_8.safetensors"; // TODO: Don't hardcode
                    ((NmkdCheckpointLoader)upscalerModelNode).ClipSkip = Config.Instance.ModelSettings.Get(Path.GetFileName(((NmkdCheckpointLoader)upscalerModelNode).ModelPath), new Models.ModelSettings()).ClipSkip;
                    ((NmkdCheckpointLoader)upscalerModelNode).LoadVae = true;
                    ((NmkdCheckpointLoader)upscalerModelNode).EmbeddingsDir = Io.Paths.ReturnDir(Config.Instance.EmbeddingsDir, true, true);

                    conditioningNode = AddNode<NmkdDualTextEncode>(); // CLIP Encode Prompt Base
                    conditioningNode.Text1 = g.Prompt;
                    conditioningNode.Text2 = g.NegativePrompt;
                    conditioningNode.Clip = new ComfyInput(upscalerModelNode, OutType.Clip);
                }

                var upscaleMdl = AddNode<NmkdUpscaleModelLoader>(); // Latent Upscale
                upscaleMdl.UpscaleModelPath = "D:\\AI\\ComfyUI\\ComfyUI\\models\\upscale_models\\realesr-general-x4v3.pth"; // TODO: Don't hardcode

                var tileControlnet = AddNode<NmkdControlNet>("TileControlnetUpscale"); // Tile ControlNet
                tileControlnet.ModelPath = "M:\\Weights\\SD\\ControlNet\\control_v11f1e_sd15_tile_fp16.safetensors"; // TODO: Don't hardcode
                tileControlnet.Conditioning = new ComfyInput(conditioningNode, OutType.Conditioning);
                tileControlnet.Image = new ComfyInput(finalImageNode, OutType.Image);
                tileControlnet.Model = new ComfyInput(upscalerModelNode, OutType.Model);

                var upscaler = AddNode<UltimateSDUpscale>(); // Ultimate SD Upscale
                upscaler.Image = new ComfyInput(finalImageNode, OutType.Image);
                upscaler.Model = new ComfyInput(upscalerModelNode, OutType.Model);
                upscaler.CondPositive = new ComfyInput(tileControlnet, OutType.Conditioning);
                upscaler.CondNegative = new ComfyInput(conditioningNode, 1);
                upscaler.Vae = new ComfyInput(xl ? upscalerModelNode : model1, OutType.Vae);
                upscaler.UpscaleModel = new ComfyInput(upscaleMdl, 0);
                upscaler.UpscaleFactor = (float)g.TargetResolution.Width / g.BaseResolution.Width;
                upscaler.Seed = g.Seed;
                upscaler.Steps = (g.Steps * 0.5f).RoundToInt();
                upscaler.Scale = g.Scale;
                upscaler.Sampler = GetComfySampler(g.Sampler);
                upscaler.Scheduler = GetComfyScheduler(g);

                finalImageNode = upscaler;
            }

            if (inpaint)
            {
                INode initImgNode = nodes.Where(n => n is NmkdImageLoader).First();
                var compositeImgs = AddNode<NmkdImageMaskComposite>(); // Composite with source image
                compositeImgs.ImageTo = new ComfyInput(vaeDecode, OutType.Image);
                compositeImgs.ImageFrom = new ComfyInput(initImageNode, OutType.Image);
                compositeImgs.Mask = new ComfyInput(initImgNode, OutType.Mask);
                finalImageNode = compositeImgs;
            }

            if (g.Upscaler.IsNotEmpty() && g.SaveOriginalAndUpscale)
            {
                var saveImageUpscaled = AddNode<SaveImage>();
                saveImageUpscaled.Prefix = $"nmkd_src";
                saveImageUpscaled.Image = new ComfyInput(finalImageNode, OutType.Image);
            }

            if (g.Upscaler.IsNotEmpty())
            {
                var upscaler = AddNode<NmkdImageUpscale>(); // Upscale Image
                upscaler.UpscaleModelPath = g.Upscaler;
                upscaler.Image = new ComfyInput(finalImageNode, OutType.Image);
                finalImageNode = upscaler;
            }

            var saveImage = AddNode<SaveImage>();
            saveImage.Prefix = $"nmkd";
            saveImage.Image = new ComfyInput(finalImageNode, OutType.Image);

            foreach (INode node in _currentNodes.Where(n => n != null))
                nodeInfos.Add(node.Id, node.GetNodeInfo());

            return nodeInfos;
        }

        private static T AddNode<T>(string preferredName = "", List<INode> nodes = null) where T : INode, new()
        {
            if (nodes == null)
                nodes = _currentNodes;

            string name = preferredName.IsNotEmpty() ? preferredName : typeof(T).ToString().Split('+').Last();
            T newNode = new T() { Id = GetNewId(name, nodes) };
            nodes.Add(newNode);
            return newNode;
        }

        private static INode AddNode(INode obj, string preferredName = "", List<INode> nodes = null)
        {
            if (nodes == null)
                nodes = _currentNodes;

            string name = preferredName.IsNotEmpty() ? preferredName : obj.GetType().Name;
            obj.Id = GetNewId(name, nodes);
            nodes.Add(obj);
            return obj;
        }

        private static string GetNewId(string preferred, List<INode> nodes = null)
        {
            if (nodes == null)
                nodes = _currentNodes;

            List<string> existingIds = nodes.Select(n => n.Id).ToList();
            string unique = preferred;
            int counter = 1;

            do
            {
                unique = $"{preferred}{counter}";
                counter++;
            } while (existingIds.Contains(unique));

            return unique;
        }

        public static string GetComfySampler(Enums.StableDiffusion.Sampler s)
        {
            switch (s)
            {
                case Enums.StableDiffusion.Sampler.Dpmpp_2M: return "dpmpp_2m";
                case Enums.StableDiffusion.Sampler.K_Dpmpp_2M: return "dpmpp_2m";
                case Enums.StableDiffusion.Sampler.Dpmpp_2M_Sde: return "dpmpp_2m_sde";
                case Enums.StableDiffusion.Sampler.K_Dpmpp_2M_Sde: return "dpmpp_2m_sde";
                case Enums.StableDiffusion.Sampler.Dpmpp_3M_Sde: return "dpmpp_3m_sde";
                case Enums.StableDiffusion.Sampler.K_Dpmpp_3M_Sde: return "dpmpp_3m_sde";
                case Enums.StableDiffusion.Sampler.Euler_A: return "euler_ancestral";
                case Enums.StableDiffusion.Sampler.Euler: return "euler";
                case Enums.StableDiffusion.Sampler.K_Euler: return "euler";
                case Enums.StableDiffusion.Sampler.Ddim: return "ddim";
                case Enums.StableDiffusion.Sampler.Lms: return "lms";
                case Enums.StableDiffusion.Sampler.Heun: return "heun";
                case Enums.StableDiffusion.Sampler.Dpm_2: return "dpm_2";
                case Enums.StableDiffusion.Sampler.Dpm_2_A: return "dpm_2_ancestral";
                case Enums.StableDiffusion.Sampler.Uni_Pc: return "uni_pc";
                default: return "dpmpp_2m_sde";
            }
        }

        public static string GetComfyScheduler(ComfyData.GenerationInfo g)
        {
            return g.Sampler.ToString().Lower().StartsWith("k_") ? "karras" : "normal";
        }
    }
}
