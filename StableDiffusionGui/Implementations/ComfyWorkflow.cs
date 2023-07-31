using StableDiffusionGui.Data;
using StableDiffusionGui.Implementations;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using System.Collections.Generic;
using System.Linq;

namespace StableDiffusionGui.Implementations
{
    public class ComfyWorkflow
    {
        public enum NodeType { CheckpointLoaderSimple, VAELoader, CLIPTextEncode, KSampler, KSamplerAdvanced, VAEDecode, EmptyLatentImage, SaveImage, PrimitiveNode, LatentUpscaleBy, CRLatentInputSwitch, NmkdIntegerConstant, NmkdFloatConstant, NmkdStringConstant }

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
                return $"{Type} ({Id}){(Title.IsNotEmpty() ? $" {Title}" : "")}{(Class != null ? $" {Class}" : "")}";
            }
        }

        public static string BuildPrompt(Comfy.GenerationInfo g, List<Node> nodes)
        {
            string prompt = "";
            var nodeCounts = new EasyDict<NodeType, int>();
            var nodesDict = new EasyDict<string, Node>();
            bool refine = g.RefinerStrength > 0.001f;
            int baseSteps = (g.Steps * (1f - g.RefinerStrength)).RoundToInt();

            foreach (var node in nodes)
                nodesDict[node.Title.Remove(" ")] = node;

            var modelNodes = nodes.Where(n => n.Type == NodeType.CheckpointLoaderSimple).ToList();
            var promptNodes = nodes.Where(n => n.Type == NodeType.NmkdStringConstant && n.Title.Lower().StartsWith("prompt")).ToList();

            bool useExternalVae = true;

            foreach (var node in nodes)
            {
                if (node == null)
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
                    int val = 0;
                    if (node.Title == "Seed") val = (int)g.Seed; // TODO: USE LONG
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
                    string ckptName = "";
                    if (node.Title == "Model1") ckptName = g.Model.Name;
                    if (node.Title == "Model2") ckptName = g.ModelRefiner != null ? g.ModelRefiner.Name : g.Model.Name;
                    node.Class = new ComfyNodes.CheckpointLoaderSimple() { CkptName = ckptName };
                }
                else if (node.Type == NodeType.VAELoader)
                {
                    node.Class = new ComfyNodes.VAELoader() { VaeName = "sdxl_vae.safetensors" };
                }
                else if (node.Type == NodeType.KSampler)
                {
                    node.Class = new ComfyNodes.KSampler()
                    {
                        Seed = g.Seed,
                        Steps = g.Steps,
                        Cfg = g.Scale,
                        IdModel = nodesDict["Model1"].Id,
                        IdPositivePrompt = nodesDict["PromptPos"].Id,
                        IdNegativePrompt = nodesDict["PromptNeg"].Id,
                        IdLatentImage = nodesDict["EmptyLatentImage"].Id
                    };
                }
                else if (node.Type == NodeType.KSamplerAdvanced)
                {
                    bool baseSampler = !node.Title.Lower().Contains("refine");
                    var latentsNode = nodesDict["EmptyLatentImage"];
                    if (node.Title == "SamplerHires") latentsNode = nodesDict["UpscaleLatent"];
                    if (node.Title == "SamplerRefiner") latentsNode = g.LatentUpscale ? nodesDict["SamplerHires"] : nodesDict["SamplerBase"];

                    node.Class = new ComfyNodes.KSamplerAdvanced()
                    {
                        AddNoise = baseSampler,
                        SamplerName = baseSampler ? "dpmpp_2m" : "euler",
                        Scheduler = node.Title.Contains("Hires") ? "simple" : "normal",
                        IdSeed = nodesDict["Seed"].Id,
                        IdSteps = nodesDict["Steps"].Id,
                        IdCfg = nodesDict["Cfg"].Id,
                        IdStartStep = baseSampler ? nodesDict["StepsSkip"].Id : nodesDict["StepsBase"].Id,
                        IdEndStep = baseSampler ? nodesDict["StepsBase"].Id : nodesDict["StepsMax"].Id,
                        ReturnLeftoverNoise = baseSampler & refine,
                        IdModel = baseSampler ? nodesDict["Model1"].Id : nodesDict["Model2"].Id,
                        IdPositive = baseSampler ? nodesDict["EncodePosPromptBase"].Id : nodesDict["EncodePosPromptRefiner"].Id,
                        IdNegative = baseSampler ? nodesDict["EncodeNegPromptBase"].Id : nodesDict["EncodeNegPromptRefiner"].Id,
                        IdLatentImage = latentsNode.Id,
                    };
                }
                else if (node.Type == NodeType.CLIPTextEncode)
                {
                    int idText = node.Title.Contains("Pos") ? nodesDict["PromptPos"].Id : nodesDict["PromptNeg"].Id;
                    int idClip = node.Title.Contains("Refine") ? nodesDict["Model2"].Id : nodesDict["Model1"].Id;
                    node.Class = new ComfyNodes.CLIPTextEncode() { IdText = idText, IdClip = idClip };
                }
                else if (node.Type == NodeType.EmptyLatentImage)
                {
                    node.Class = new ComfyNodes.EmptyLatentImage() { Width = g.Width, Height = g.Height, BatchSize = 1 };
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
                    int idVae = useExternalVae ? nodesDict["LoadExtVae"].Id : nodesDict["Model1"].Id;
                    int vaeIndex = useExternalVae ? 0 : 2;
                    node.Class = new ComfyNodes.VAEDecode() { IdLatents = nodesDict["SamplerRefiner"].Id, IdVae = idVae, IdVaeIndex = vaeIndex };
                }
                else if (node.Type == NodeType.SaveImage)
                {
                    node.Class = new ComfyNodes.SaveImage() { IdImages = nodesDict["VAEDecode"].Id };
                }
                else
                {
                    Logger.Log($"Warning: No node class found for node with node.Type '{node.Type}' and title '{node.Title}'", true);
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
    }
}
