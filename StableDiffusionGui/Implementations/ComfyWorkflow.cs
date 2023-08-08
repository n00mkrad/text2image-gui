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
                return JsonConvert.SerializeObject(this, format, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() } });
            }
        }

        public class NodeInfo
        {
            public Dictionary<string, object> Inputs { get; set; } = new Dictionary<string, object>();
            public string ClassType { get; set; }
        }

        private enum Axis { Width, Height }

        public static EasyDict<string, NodeInfo> GetPromptInfos(Comfy.GenerationInfo g, List<INode> nodes)
        {
            var nodeInfos = new EasyDict<string, NodeInfo>();
            var nodeCounts = new EasyDict<string, int>();
            var nodesDict = new EasyDict<string, INode>();
            bool refine = g.RefinerStrength > 0.001f;
            bool upscale = !g.TargetResolution.IsEmpty && g.TargetResolution != g.BaseResolution;
            bool inpaint = g.MaskPath.IsNotEmpty();
            bool controlnet = g.ControlnetModel.IsNotEmpty() && g.ControlnetStrength > 0.001f;
            bool img2img = g.InitImg.IsNotEmpty() && !controlnet;
            int baseSteps = (g.Steps * (1f - g.RefinerStrength)).RoundToInt();
            int baseWidth = g.BaseResolution.Width;
            int baseHeight = g.BaseResolution.Height;

            nodes = nodes.OrderBy(n => n.Id).ToList();

            foreach (var node in nodes)
                nodesDict[node.Title.Remove(" ")] = node;

            var promptPos = (NmkdStringConstant)nodesDict["PromptPos"]; // Positive Prompt
            promptPos.Text = g.Prompt;

            var promptNeg = (NmkdStringConstant)nodesDict["PromptNeg"]; // Negative Prompt
            promptNeg.Text = g.NegativePrompt;

            var seed = (NmkdIntegerConstant)nodesDict["Seed"]; // Seed
            seed.Value = g.Seed;

            var steps = (NmkdIntegerConstant)nodesDict["Steps"]; // Seed
            steps.Value = g.Steps;

            var stepsBase = (NmkdIntegerConstant)nodesDict["StepsBase"]; // Base Model Steps
            stepsBase.Value = baseSteps;

            var stepsSkip = (NmkdIntegerConstant)nodesDict["StepsSkip"]; // Skip Steps (Start at)
            stepsSkip.Value = 0;

            var stepsMax = (NmkdIntegerConstant)nodesDict["StepsMax"]; // Max Steps
            stepsMax.Value = 1000;

            var cfg = (NmkdFloatConstant)nodesDict["Cfg"]; // CFG Scale
            cfg.Value = g.Scale;

            var model1 = (NmkdCheckpointLoader)nodesDict["Model1"]; // Base Model
            model1.ModelPath = g.Model;
            model1.LoadVae = true;
            model1.VaePath = g.Vae ?? "";

            var model2 = (NmkdCheckpointLoader)nodesDict["Model2"]; // Aux Model (Refiner etc)
            model2.ModelPath = g.ModelRefiner == null ? g.Model : g.ModelRefiner;
            model2.LoadVae = false;

            var clipSkipModel1 = (CLIPSetLastLayer)nodesDict["ClipSkipModel1"]; // CLIP Skip Model 1
            var clipSkipModel2 = (CLIPSetLastLayer)nodesDict["ClipSkipModel2"]; // CLIP Skip Model 2
            clipSkipModel1.Skip = g.ClipSkip;
            clipSkipModel1.ClipNode = model1;
            clipSkipModel2.Skip = g.ClipSkip;
            clipSkipModel2.ClipNode = model2;

            var loraLoader = (NmkdMultiLoraLoader)nodesDict["LoraLoader"]; // LoRA Loader
            loraLoader.Loras = g.Loras.Keys.ToList();
            loraLoader.Strengths = g.Loras.Values.ToList();
            loraLoader.ModelNode = model1;
            loraLoader.ClipNode = model1;

            var encodePosPromptBase = (CLIPTextEncode)nodesDict["EncodePosPromptBase"]; // CLIP Encode Positive Prompt Base
            var encodeNegPromptBase = (CLIPTextEncode)nodesDict["EncodeNegPromptBase"]; // CLIP Encode Negative Prompt Base
            var encodePosPromptRefiner = (CLIPTextEncode)nodesDict["EncodePosPromptRefiner"]; // CLIP Encode Positive Prompt Refiner
            var encodeNegPromptRefiner = (CLIPTextEncode)nodesDict["EncodeNegPromptRefiner"]; // CLIP Encode Negative Prompt Refiner
            encodePosPromptBase.TextNode = nodesDict["PromptPos"];
            encodeNegPromptBase.TextNode = nodesDict["PromptNeg"];
            encodePosPromptRefiner.TextNode = nodesDict["PromptPos"];
            encodeNegPromptRefiner.TextNode = nodesDict["PromptNeg"];
            encodePosPromptBase.ClipNode = loraLoader;
            encodeNegPromptBase.ClipNode = loraLoader;
            encodePosPromptRefiner.ClipNode = clipSkipModel2;
            encodeNegPromptRefiner.ClipNode = clipSkipModel2;

            var emptyLatentImg = (EmptyLatentImage)nodesDict["EmptyLatentImage"]; // Empty Latent Image (T2I)
            emptyLatentImg.Width = baseWidth;
            emptyLatentImg.Height = baseHeight;

            var loadInitImg = (NmkdImageLoader)nodesDict["InitImg"]; // Load Init Image
            loadInitImg.ImagePath = g.MaskPath.IsNotEmpty() ? g.MaskPath : g.InitImg;

            var encodeInitImg = (NmkdVaeEncode)nodesDict["InitImgEncode"]; // Encode Init Image (I2I)
            encodeInitImg.ImageNode = loadInitImg;
            encodeInitImg.VaeNode = model1;
            encodeInitImg.LoadMask = g.MaskPath.IsNotEmpty();

            INode preProcessor = null;

            if (g.ImagePreprocessor != Enums.StableDiffusion.ImagePreprocessor.None)
                preProcessor = new GenericImagePreprocessor { ImageNode = loadInitImg, Preprocessor = g.ImagePreprocessor };

            if (preProcessor != null)
            {
                preProcessor.Id = g.ImagePreprocessor.ToString();
                preProcessor.Title = "ImgPreprocessor";
                nodes.Add(preProcessor);
            }

            var controlNet = (NmkdControlNet)nodesDict["ControlNet"]; // ControlNet
            controlNet.ModelPath = g.ControlnetModel;
            controlNet.Strength = g.ControlnetStrength;
            controlNet.ConditioningNode = encodePosPromptBase;
            controlNet.ImageNode = preProcessor ?? loadInitImg;

            var samplerBase = (NmkdKSampler)nodesDict["SamplerBase"]; // Sampler Base
            samplerBase.AddNoise = true;
            samplerBase.NodeModel = loraLoader;
            samplerBase.NodePositive = controlnet ? (INode)controlNet : (INode)encodePosPromptBase;
            samplerBase.NodeNegative = encodeNegPromptBase;
            samplerBase.NodeLatentImage = img2img ? (INode)encodeInitImg : (INode)emptyLatentImg;
            samplerBase.SamplerName = GetComfySampler(g.Sampler);
            samplerBase.Scheduler = GetComfyScheduler(g);
            samplerBase.NodeSeed = seed;
            samplerBase.NodeSteps = steps;
            samplerBase.NodeCfg = cfg;
            samplerBase.NodeStartStep = stepsSkip;
            samplerBase.NodeEndStep = stepsBase;
            samplerBase.ReturnLeftoverNoise = refine;
            samplerBase.Denoise = img2img ? g.InitStrength : 1.0f;

            var latentUpscale = (LatentUpscale)nodesDict["UpscaleLatent"]; // Latent Upscale
            latentUpscale.Width = g.TargetResolution.Width;
            latentUpscale.Height = g.TargetResolution.Height;
            latentUpscale.LatentsNode = samplerBase;

            var samplerHires = (NmkdKSampler)nodesDict["SamplerHires"]; // Sampler Hi-Res
            samplerHires.AddNoise = true;
            samplerHires.NodeModel = loraLoader;
            samplerHires.NodePositive = controlnet ? (INode)controlNet : (INode)encodePosPromptBase;
            samplerHires.NodeNegative = encodeNegPromptBase;
            samplerHires.NodeLatentImage = latentUpscale;
            samplerHires.SamplerName = GetComfySampler(g.Sampler);
            samplerHires.Scheduler = "simple"; // TODO: CHECK IN UI IF THIS IS NEEDED OR NOT
            samplerHires.NodeSeed = seed;
            samplerHires.NodeSteps = steps;
            samplerHires.NodeCfg = cfg;
            samplerHires.NodeStartStep = stepsSkip;
            samplerHires.NodeEndStep = stepsBase;
            samplerHires.ReturnLeftoverNoise = refine;

            var samplerRefiner = (NmkdKSampler)nodesDict["SamplerRefiner"]; // Sampler Refiner
            samplerRefiner.AddNoise = false;
            samplerRefiner.NodeModel = model2;
            samplerRefiner.NodePositive = encodePosPromptRefiner;
            samplerRefiner.NodeNegative = encodeNegPromptRefiner;
            samplerRefiner.NodeLatentImage = upscale ? samplerHires : samplerBase;
            samplerRefiner.SamplerName = "euler";
            samplerRefiner.Scheduler = "normal";
            samplerRefiner.NodeSeed = seed;
            samplerRefiner.NodeSteps = steps;
            samplerRefiner.NodeCfg = cfg;
            samplerRefiner.NodeStartStep = stepsBase;
            samplerRefiner.NodeEndStep = stepsMax;
            samplerRefiner.ReturnLeftoverNoise = false;

            var vaeDecode = (VAEDecode)nodesDict["VAEDecode"]; // Final VAE Decode
            vaeDecode.VaeNode = model1;
            vaeDecode.LatentsNode = samplerBase;
            if (refine) vaeDecode.LatentsNode = samplerRefiner;
            if (!refine && upscale) vaeDecode.LatentsNode = samplerHires;

            var upscaler = (NmkdImageUpscale)nodesDict["ImageUpscale"]; // Upscale Image
            upscaler.UpscaleModelPath = g.Upscaler;
            upscaler.ImageNode = vaeDecode;

            var compositeImgs = (NmkdImageMaskComposite)nodesDict["InpaintImageComposite"]; // Composite Images
            compositeImgs.ImageToNode = upscaler;
            compositeImgs.ImageFromNode = loadInitImg;
            compositeImgs.MaskNode = loadInitImg;

            var saveImage = (SaveImage)nodesDict["Save"]; // Save Image
            saveImage.Prefix = $"nmkd{FormatUtils.GetUnixTimestamp()}";
            saveImage.ImageNode = inpaint ? (INode)compositeImgs : (INode)upscaler;

            foreach (INode node in nodes.Where(n => n != null))
            {
                try
                {
                    nodeInfos[node.Id.ToString()] = node.GetNodeInfo();
                }
                catch
                {
                    Logger.Log($"Warning: Failed to parse node {node.Id} {node.Title}");
                }
            }

            return nodeInfos;
        }

        public static List<INode> GetNodes(string comfyPrompt)
        {
            var nodesList = new List<INode>();

            comfyPrompt = "\"nodes\": [" + comfyPrompt.Split("\"nodes\": [")[1]; // Cut off everything before nodes
            comfyPrompt = comfyPrompt.Split("\n  ],")[0];

            var lines = comfyPrompt.SplitIntoLines().Where(l => l.StartsWith("      \"id\":") || l.StartsWith("      \"type\":") || l.StartsWith("      \"title\":")).ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith("      \"id\":"))
                {
                    string type = lines[i + 1].Split("\"type\":")[1].Trim().Trim(',').Trim('\"');
                    string title = (i + 2 < lines.Count) && lines[i + 2].Trim().StartsWith("\"title\":") ? lines[i + 2].Split("\"title\":")[1].Trim().Trim(',').Trim('\"') : "";
                    string id = $"{type}{lines[i].GetInt()}";

                    INode newNode = null;
                    if (type == "Reroute") continue;
                    else if (type == "CheckpointLoaderSimple") newNode = new CheckpointLoaderSimple();
                    else if (type == "VAELoader") newNode = new VAELoader();
                    else if (type == "CLIPTextEncode") newNode = new CLIPTextEncode();
                    else if (type == "KSampler") newNode = new KSampler();
                    else if (type == "NmkdKSampler") newNode = new NmkdKSampler();
                    else if (type == "VAEDecode") newNode = new VAEDecode();
                    else if (type == "NmkdVaeEncode") newNode = new NmkdVaeEncode();
                    else if (type == "EmptyLatentImage") newNode = new EmptyLatentImage();
                    else if (type == "SaveImage") newNode = new SaveImage();
                    else if (type == "LatentUpscale") newNode = new LatentUpscale();
                    else if (type == "LatentUpscaleBy") newNode = new LatentUpscaleBy();
                    else if (type == "NmkdIntegerConstant") newNode = new NmkdIntegerConstant();
                    else if (type == "NmkdFloatConstant") newNode = new NmkdFloatConstant();
                    else if (type == "NmkdStringConstant") newNode = new NmkdStringConstant();
                    else if (type == "NmkdCheckpointLoader") newNode = new NmkdCheckpointLoader();
                    else if (type == "CLIPSetLastLayer") newNode = new CLIPSetLastLayer();
                    else if (type == "NmkdImageLoader") newNode = new NmkdImageLoader() { Id = FormatUtils.GetUnixTimestamp() };
                    else if (type == "NmkdImageUpscale") newNode = new NmkdImageUpscale();
                    else if (type == "NmkdMultiLoraLoader") newNode = new NmkdMultiLoraLoader();
                    else if (type == "NmkdImageMaskComposite") newNode = new NmkdImageMaskComposite();
                    else if (type == "NmkdControlNet") newNode = new NmkdControlNet();
                    else Logger.Log($"Comfy Nodes Parser: No class found for {type}.", true);

                    if (newNode == null)
                        continue;

                    if (newNode.Id.IsEmpty())
                        newNode.Id = id + newNode.Id;

                    newNode.Title = title;
                    nodesList.Add(newNode);
                }
            }
            return nodesList;
        }

        // private static int GetAvailableId(List<INode> nodes)
        // {
        //     List<int> ids = nodes.Select(n => n.Id.GetInt()).ToList();
        //     int id = (int)FormatUtils.GetUnixTime();
        // 
        //     while (ids.Contains(id))
        //         id++;
        // 
        //     return id;
        // }

        public static string GetComfySampler(Enums.StableDiffusion.Sampler s)
        {
            switch (s)
            {
                case Enums.StableDiffusion.Sampler.Dpmpp_2M: return "dpmpp_2m";
                case Enums.StableDiffusion.Sampler.K_Dpmpp_2M: return "dpmpp_2m";
                case Enums.StableDiffusion.Sampler.Dpmpp_2M_Sde: return "dpmpp_2m_sde";
                case Enums.StableDiffusion.Sampler.K_Dpmpp_2M_Sde: return "dpmpp_2m_sde";
                case Enums.StableDiffusion.Sampler.Euler_A: return "euler_ancestral";
                case Enums.StableDiffusion.Sampler.Euler: return "euler";
                case Enums.StableDiffusion.Sampler.K_Euler: return "euler";
                case Enums.StableDiffusion.Sampler.Ddim: return "ddim";
                case Enums.StableDiffusion.Sampler.Lms: return "lms";
                case Enums.StableDiffusion.Sampler.Heun: return "heun";
                case Enums.StableDiffusion.Sampler.Dpm_2: return "dpm_2";
                case Enums.StableDiffusion.Sampler.Dpm_2_A: return "dpm_2_ancestral";
                case Enums.StableDiffusion.Sampler.UniPc: return "uni_pc";
                default: return "dpmpp_2m";
            }
        }

        public static string GetComfyScheduler(Comfy.GenerationInfo g)
        {
            string sched = g.Sampler.ToString().Lower().StartsWith("k_") ? "karras" : "normal";
            bool upscale = !g.TargetResolution.IsEmpty && g.TargetResolution != g.BaseResolution;
            bool noThreeStage = !(upscale && g.RefinerStrength > 0f);

            // if (noThreeStage && g.Sampler.ToString().Contains("2M"))
            // {
            //     sched = "simple";
            //     Logger.Log($"Warning: Scheduler was overwritten to '{sched}' due to Comfy issues when using 2M samplers in final denoising stage.", true);
            // }

            return sched;
        }
    }
}
