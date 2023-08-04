using StableDiffusionGui.Data;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace StableDiffusionGui.Implementations
{
    public class ComfyWorkflow
    {
        public enum NodeType
        {
            CheckpointLoaderSimple, VAELoader, CLIPTextEncode, KSampler, NmkdKSampler, VAEDecode, VAEEncode, EmptyLatentImage, SaveImage, PrimitiveNode, LatentUpscale, LatentUpscaleBy, CRLatentInputSwitch, Reroute,
            NmkdIntegerConstant, NmkdFloatConstant, NmkdStringConstant, NmkdCheckpointLoader, CLIPSetLastLayer, NmkdImageLoader, NmkdImageUpscale, NmkdMultiLoraLoader
        }

        public class Node
        {
            public int Id;
            public NodeType Type;
            public string Title = "";
            public ComfyNodes.IComfyNode Class;

            public Node() { }

            public Node(int id, NodeType type, string title = "")
            {
                Id = id;
                Type = type;
                Title = title;
            }

            public override string ToString()
            {
                return $"#{Id} - {Type}{(Title.IsNotEmpty() ? $" - {Title}" : "")}";
            }
        }

        private enum Axis { Width, Height }

        public static string BuildPrompt(Comfy.GenerationInfo g, List<Node> nodes)
        {
            string prompt = "";
            var nodeCounts = new EasyDict<NodeType, int>();
            var nodesDict = new EasyDict<string, Node>();
            bool refine = g.RefinerStrength > 0.001f;
            bool upscale = !g.TargetResolution.IsEmpty && g.TargetResolution != g.BaseResolution;
            int baseSteps = (g.Steps * (1f - g.RefinerStrength)).RoundToInt();
            int baseWidth = g.BaseResolution.Width;
            int baseHeight = g.BaseResolution.Height;

            if (upscale)
            {
                // Axis shorterAxis = g.Width < g.Height ? Axis.Width : Axis.Height;
                // int oldShorterAxisLength = shorterAxis == Axis.Width ? g.Width : g.Height;
                // int newShorterAxisLength = ((shorterAxis == Axis.Width ? g.Width : g.Height) * 0.5f).Clamp(512f, 2048f).RoundToInt();
                // float factor = (float)newShorterAxisLength / oldShorterAxisLength;
                // baseWidth = (g.Width * factor).RoundToInt().RoundMod(8);
                // baseHeight = (g.Height * factor).RoundToInt().RoundMod(8);
                // 
                // if(baseWidth < 1024 && baseHeight < 1024)
                // {
                //     Size s = ImgMaths.FitIntoFrame(new Size(baseWidth, baseHeight), new Size(1024, 1024));
                //     baseWidth = s.Width;
                //     baseHeight = s.Height;
                // }
                // 
                // baseWidth = baseWidth.RoundMod(16);
                // baseHeight = baseHeight.RoundMod(16);
                // Logger.Log($"Final latent upscaling size: {baseWidth}x{baseHeight}", true);
            }

            nodes = nodes.OrderBy(n => n.Id).ToList();

            foreach (var node in nodes)
                nodesDict[node.Title.Remove(" ")] = node;

            foreach (var node in nodes)
            {
                if (node == null)
                {
                    continue;
                }
                else if (node.Type == NodeType.Reroute)
                {
                    continue;
                }
                else if (node.Type == NodeType.NmkdStringConstant)
                {
                    string str = "";
                    if (node.Title == "PromptPos") str = g.Prompt;
                    if (node.Title == "PromptNeg") str = g.NegativePrompt;
                    node.Class = new ComfyNodes.NmkdStringConstant() { Text = str };
                }
                else if (node.Type == NodeType.NmkdIntegerConstant)
                {
                    long val = 0;
                    if (node.Title == "Seed") val = g.Seed;
                    if (node.Title == "Steps") val = g.Steps;
                    if (node.Title == "StepsBase") val = baseSteps;
                    if (node.Title == "StepsSkip") val = 0;
                    if (node.Title == "StepsMax") val = 1000;
                    node.Class = new ComfyNodes.NmkdIntegerConstant() { Value = val };
                }
                else if (node.Type == NodeType.NmkdFloatConstant)
                {
                    float val = 0f;
                    if (node.Title == "Cfg") val = g.Scale;
                    node.Class = new ComfyNodes.NmkdFloatConstant() { Value = val };
                }
                else if (node.Type == NodeType.CheckpointLoaderSimple)
                {
                    node.Class = new ComfyNodes.CheckpointLoaderSimple();
                }
                else if (node.Type == NodeType.NmkdCheckpointLoader)
                {
                    if (node.Title == "Model1")
                        node.Class = new ComfyNodes.NmkdCheckpointLoader() { ModelPath = g.Model, LoadVae = true, VaePath = g.Vae != null ? g.Vae : "" };
                    if (node.Title == "Model2")
                        node.Class = new ComfyNodes.NmkdCheckpointLoader() { ModelPath = g.ModelRefiner == null ? g.Model : g.ModelRefiner, LoadVae = false };
                }
                else if (node.Type == NodeType.NmkdMultiLoraLoader)
                {
                    node.Class = new ComfyNodes.NmkdMultiLoraLoader() { Loras = g.Loras.Keys.ToList(), Weights = g.Loras.Values.ToList(), ModelNode = nodesDict["Model1"], ClipNode = nodesDict["ClipSkipModel1"] };
                }
                else if (node.Type == NodeType.NmkdImageLoader)
                {
                    node.Class = new ComfyNodes.NmkdImageLoader() { ImagePath = g.InitImg };
                }
                else if (node.Type == NodeType.NmkdImageUpscale)
                {
                    node.Class = new ComfyNodes.NmkdImageUpscale() { UpscaleModelPath = g.Upscaler, ImageNode = nodesDict["VAEDecode"] };
                }
                else if (node.Type == NodeType.VAELoader)
                {
                    node.Class = new ComfyNodes.VAELoader();
                }
                else if (node.Type == NodeType.KSampler)
                {
                    continue;
                }
                else if (node.Type == NodeType.NmkdKSampler)
                {
                    bool baseSampler = !node.Title.Lower().Contains("refine") || !refine;
                    var latentsNode = nodesDict[g.InitImg.IsNotEmpty() ? "InitImgEncode" : "EmptyLatentImage"];
                    if (node.Title == "SamplerHires" && !upscale) continue;
                    if (node.Title == "SamplerHires") latentsNode = nodesDict["UpscaleLatent"];
                    if (node.Title == "SamplerRefiner") latentsNode = upscale ? nodesDict["SamplerHires"] : nodesDict["SamplerBase"];

                    node.Class = new ComfyNodes.NmkdKSampler()
                    {
                        AddNoise = baseSampler,
                        SamplerName = baseSampler ? GetComfySampler(g.Sampler) : "euler",
                        Scheduler = baseSampler ? (node.Title.Contains("Hires") ? "simple" : GetComfyScheduler(g)) : "normal", // Base = From Sampler, Hires = Simple, Refiner = Normal
                        NodeSeed = nodesDict["Seed"],
                        NodeSteps = nodesDict["Steps"],
                        NodeCfg = nodesDict["Cfg"],
                        NodeStartStep = baseSampler ? nodesDict["StepsSkip"] : nodesDict["StepsBase"],
                        NodeEndStep = baseSampler ? nodesDict["StepsBase"] : nodesDict["StepsMax"],
                        ReturnLeftoverNoise = baseSampler & refine,
                        Denoise = node.Title == "SamplerBase" && g.InitImg.IsNotEmpty() ? g.InitStrength : 1f,
                        NodeModel = baseSampler ? nodesDict["LoraLoader1"] : nodesDict["Model2"],
                        NodePositive = baseSampler ? nodesDict["EncodePosPromptBase"] : nodesDict["EncodePosPromptRefiner"],
                        NodeNegative = baseSampler ? nodesDict["EncodeNegPromptBase"] : nodesDict["EncodeNegPromptRefiner"],
                        NodeLatentImage = latentsNode,
                        DebugString = node.Title,
                    };
                }
                else if (node.Type == NodeType.CLIPTextEncode)
                {
                    var textNode = nodesDict[node.Title.Contains("Pos") ? "PromptPos" : "PromptNeg"];
                    var clipSkipNode = nodesDict[node.Title.Contains("Refine") ? "ClipSkipModel2" : "LoraLoader1"];
                    node.Class = new ComfyNodes.CLIPTextEncode() { TextNode = textNode, ClipNode = clipSkipNode };
                }
                else if (node.Type == NodeType.CLIPSetLastLayer)
                {
                    var clipNode = (node.Title.Contains("Model1") || !refine) ? nodesDict["Model1"] : nodesDict["Model2"];
                    node.Class = new ComfyNodes.CLIPSetLastLayer() { Skip = g.ClipSkip, ClipNode = clipNode };
                }
                else if (node.Type == NodeType.EmptyLatentImage)
                {
                    node.Class = new ComfyNodes.EmptyLatentImage() { Width = baseWidth, Height = baseHeight, BatchSize = 1 };
                }
                else if (node.Type == NodeType.LatentUpscale)
                {
                    node.Class = new ComfyNodes.LatentUpscale() { Width = g.TargetResolution.Width, Height = g.TargetResolution.Height, IdLatents = nodesDict["SamplerBase"].Id };
                }
                else if (node.Type == NodeType.LatentUpscaleBy)
                {
                    node.Class = new ComfyNodes.LatentUpscaleBy() { ScaleFactor = 1.5f, IdLatents = nodesDict["SamplerBase"].Id };
                }
                else if (node.Type == NodeType.CRLatentInputSwitch)
                {
                    if (node.Title == "HiResSwitch")
                        node.Class = new ComfyNodes.CRLatentInputSwitch() { IdLatents1 = nodesDict["SamplerBase"].Id, IdLatents2 = nodesDict["SamplerHires"].Id };
                }
                else if (node.Type == NodeType.VAEDecode)
                {
                    string latentsNode = "SamplerBase";
                    if (refine) latentsNode = "SamplerRefiner";
                    if (!refine && upscale) latentsNode = "SamplerHires";
                    node.Class = new ComfyNodes.VAEDecode() { LatentsNode = nodesDict[latentsNode], VaeNode = nodesDict["Model1"] };
                }
                else if (node.Type == NodeType.VAEEncode)
                {
                    node.Class = new ComfyNodes.VAEEncode() { ImageNode = nodesDict["InitImg"], VaeNode = nodesDict["Model1"] };
                }
                else if (node.Type == NodeType.SaveImage)
                {
                    node.Class = new ComfyNodes.SaveImage() { Prefix = $"nmkd{FormatUtils.GetUnixTimestamp()}", ImageNode = nodesDict["VAEDecode"] };
                }
                else
                {
                    Logger.Log($"Warning: No node class found for node with type '{node.Type}' and title '{node.Title}'", true);
                    continue;
                }

                nodeCounts.GetPopulate(node.Type, 0);
                nodeCounts[node.Type] = nodeCounts[node.Type] + 1;
            }

            prompt += string.Join(",", nodes.Where(n => n.Class != null).Select(n => $"\"{n.Id}\":{{{n.Class.GetString()}}}"));
            return prompt;
        }

        public static List<Node> GetNodes(string comfyPrompt)
        {
            var nodesList = new List<Node>();
            comfyPrompt = "\"nodes\": [" + comfyPrompt.Split("\"nodes\": [")[1]; // Cut off everything before nodes
            comfyPrompt = comfyPrompt.Split("\n  ],")[0];

            var lines = comfyPrompt.SplitIntoLines().Where(l => l.StartsWith("      \"id\":") || l.StartsWith("      \"type\":") || l.StartsWith("      \"title\":")).ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith("      \"id\":"))
                {
                    int id = lines[i].GetInt();
                    string type = lines[i + 1].Split("\"type\":")[1].Trim().Trim(',').Trim('\"');
                    string title = (i + 2 < lines.Count) && lines[i + 2].Trim().StartsWith("\"title\":") ? lines[i + 2].Split("\"title\":")[1].Trim().Trim(',').Trim('\"') : "";
                    var enumType = ParseUtils.GetEnum<NodeType>(type.Remove(" "), false);
                    nodesList.Add(new Node() { Id = id, Type = enumType, Title = title });
                }
            }

            return nodesList;
        }

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

            if (noThreeStage && g.Sampler.ToString().Contains("2M"))
            {
                sched = "simple";
                Logger.Log($"Warning: Scheduler was overwritten to '{sched}' due to Comfy issues when using 2M samplers in final denoising stage.", true);
            }

            return sched;
        }
    }
}
