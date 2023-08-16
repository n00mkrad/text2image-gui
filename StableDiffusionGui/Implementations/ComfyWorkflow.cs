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
                return ToStringAdvanced();
            }

            public string ToStringAdvanced(bool indent = false)
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

            var encodePosPromptBase = AddNode<CLIPTextEncode>(); // CLIP Encode Positive Prompt Base
            encodePosPromptBase.Text = new ComfyInput(g.Prompt);
            encodePosPromptBase.Clip = new ComfyInput(finalModelNode, OutType.Clip);

            var encodeNegPromptBase = AddNode<CLIPTextEncode>(); // CLIP Encode Negative Prompt Base
            encodeNegPromptBase.Text = new ComfyInput(g.NegativePrompt);
            encodeNegPromptBase.Clip = new ComfyInput(finalModelNode, OutType.Clip);

            CLIPTextEncode encodePosPromptRefiner = null;
            CLIPTextEncode encodeNegPromptRefiner = null;

            if (refine)
            {
                encodePosPromptRefiner = AddNode<CLIPTextEncode>(); // CLIP Encode Positive Prompt Refiner
                encodePosPromptRefiner.Text = new ComfyInput(g.Prompt);
                encodePosPromptRefiner.Clip = new ComfyInput(model2, OutType.Clip);

                encodeNegPromptRefiner = AddNode<CLIPTextEncode>(); // CLIP Encode Negative Prompt Refiner
                encodeNegPromptRefiner.Text = new ComfyInput(g.NegativePrompt);
                encodeNegPromptRefiner.Clip = new ComfyInput(model2, OutType.Clip);
            }

            if (g.InitImg.IsEmpty())
            {
                var emptyLatentImg = AddNode<EmptyLatentImage>(); // Empty Latent Image (T2I)
                emptyLatentImg.Width = baseWidth;
                emptyLatentImg.Height = baseHeight;
            }
            else
            {
                var loadInitImg = AddNode<NmkdImageLoader>(); // Load Init Image
                loadInitImg.ImagePath = g.MaskPath.IsNotEmpty() ? g.MaskPath : g.InitImg;

                var encodeInitImg = AddNode<NmkdVaeEncode>(); // Encode Init Image (I2I)
                encodeInitImg.Image = new ComfyInput(loadInitImg, OutType.Image);
                encodeInitImg.Mask = new ComfyInput(loadInitImg, OutType.Mask);
                encodeInitImg.Vae = new ComfyInput(model1, OutType.Vae);
                encodeInitImg.UseMask = g.MaskPath.IsNotEmpty();
            }

            INode initImageNode = nodes.Last(); // Keep track of the init image to use
            INode finalConditioningNode = encodePosPromptBase; // Keep track of the final conditioning node for sampling

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

            var samplerBase = AddNode<NmkdKSampler>("SamplerBase"); // Sampler Base
            samplerBase.AddNoise = true;
            samplerBase.Model = new ComfyInput(finalModelNode, OutType.Model);
            samplerBase.PositiveCond = new ComfyInput(finalConditioningNode, OutType.Conditioning);
            samplerBase.NegativeCond = new ComfyInput(encodeNegPromptBase, OutType.Conditioning);
            samplerBase.LatentImage = new ComfyInput(initImageNode, OutType.Latents);
            samplerBase.SamplerName = GetComfySampler(g.Sampler);
            samplerBase.Scheduler = GetComfyScheduler(g);
            samplerBase.Seed = new ComfyInput(g.Seed);
            samplerBase.StepsTotal = new ComfyInput(g.Steps);
            samplerBase.Cfg = new ComfyInput(g.Scale);
            samplerBase.StartStep = new ComfyInput(0);
            samplerBase.EndStep = new ComfyInput(baseSteps);
            samplerBase.ReturnLeftoverNoise = refine;
            samplerBase.Denoise = img2img ? g.InitStrength : 1.0f;
            INode finalLatents = samplerBase;

            if (upscale)
            {
                var latentUpscale = AddNode<LatentUpscale>(); // Latent Upscale
                latentUpscale.Width = g.TargetResolution.Width;
                latentUpscale.Height = g.TargetResolution.Height;
                latentUpscale.Latents = new ComfyInput(samplerBase, OutType.Latents);

                var samplerHires = AddNode<NmkdKSampler>("SamplerHires"); // Sampler Hi-Res
                samplerHires.AddNoise = true;
                samplerHires.Model = new ComfyInput(finalModelNode, OutType.Model);
                samplerHires.PositiveCond = new ComfyInput(finalConditioningNode, OutType.Conditioning);
                samplerHires.NegativeCond = new ComfyInput(encodeNegPromptBase, OutType.Conditioning);
                samplerHires.LatentImage = new ComfyInput(latentUpscale, OutType.Latents);
                samplerHires.SamplerName = GetComfySampler(g.Sampler);
                samplerHires.Scheduler = GetComfyScheduler(g);
                samplerHires.Seed = new ComfyInput(g.Seed);
                samplerHires.StepsTotal = new ComfyInput(g.Steps);
                samplerHires.Cfg = new ComfyInput(g.Scale);
                samplerHires.StartStep = new ComfyInput(0);
                samplerHires.EndStep = new ComfyInput(baseSteps);
                samplerHires.ReturnLeftoverNoise = refine;
                finalLatents = samplerHires;
            }

            if (refine)
            {
                var samplerRefiner = AddNode<NmkdKSampler>("SamplerRefiner"); // Sampler Refiner
                samplerRefiner.AddNoise = false;
                samplerRefiner.Model = new ComfyInput(model2, OutType.Model);
                samplerRefiner.PositiveCond = new ComfyInput(encodePosPromptRefiner, OutType.Conditioning);
                samplerRefiner.NegativeCond = new ComfyInput(encodeNegPromptRefiner, OutType.Conditioning);
                samplerRefiner.LatentImage = new ComfyInput(finalLatents, OutType.Latents);
                samplerRefiner.SamplerName = GetComfySampler(g.Sampler);
                samplerRefiner.Scheduler = GetComfyScheduler(g);
                samplerRefiner.Seed = new ComfyInput(g.Seed);
                samplerRefiner.StepsTotal = new ComfyInput(g.Steps);
                samplerRefiner.Cfg = new ComfyInput(g.Scale);
                samplerRefiner.StartStep = new ComfyInput(baseSteps);
                samplerRefiner.EndStep = new ComfyInput(1000);
                samplerRefiner.ReturnLeftoverNoise = false;
                finalLatents = samplerRefiner;
            }

            var vaeDecode = AddNode<VAEDecode>(); // Final VAE Decode
            vaeDecode.Vae = new ComfyInput(model1, OutType.Vae);
            vaeDecode.Latents = new ComfyInput(finalLatents, OutType.Latents);

            INode finalImageNode = vaeDecode; // Keep track of the final image output node to use

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
