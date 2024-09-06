using Newtonsoft.Json;
using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static StableDiffusionGui.Implementations.ComfyWorkflow;

namespace StableDiffusionGui.Implementations
{
    internal class ComfyWorkflowTools
    {
        private static int _workflowCounter = 1;

        public static string ReconstructJson (EasyDict<string, NodeInfo> prompt)
        {
            var sw = new NmkdStopwatch();
            var graph = new ComfyGraph
            {
                Version = 0.4, // TODO: Detect version?
                Extra = new ExtraData() { DisplaySettings = new DisplaySettings { Scale = 1.0, Offset = new List<double> { 0.0, 0.0 } } }
            };

            Dictionary<Node, KeyValuePair<string, NodeInfo>> workflowPromptNodeMap = new Dictionary<Node, KeyValuePair<string, NodeInfo>>();

            for(int i = 0; i < prompt.Count; i++)
            {
                var info = prompt.ElementAt(i).Value;
                var nodeInputs = new List<InConnection>();
                var valueInputs = new List<object>();

                foreach (var input in info.Inputs)
                {
                    if(input.Value.GetType().IsArray)
                    {
                        var inputIndex = prompt.Keys.ToList().IndexOf(((Object[])input.Value)[0].ToString());
                        var inputNodeOutputSlot = ((Object[])input.Value)[1].ToString().GetInt();
                        nodeInputs.Add(new InConnection() { Name = input.Key, Type = GetInputNodeType(info, input.Key), Link = -1, SourceIdAndSlot = new int[] { inputIndex, inputNodeOutputSlot } });
                    }
                    else
                    {
                        valueInputs.Add(input.Value);
                    }
                }

                var outputs = new List<OutConnection>();

                for(int j = 0; j < info.OutputTypes.Count; j++)
                {
                    outputs.Add(new OutConnection { Name = info.OutputTypes[j].ToString().ToUpper(), Type = info.OutputTypes[j].ToString().ToUpper(), Links = (-1).AsList(), SlotIndex = j, Shape = 3 });
                }

                var node = new Node
                {
                    Id = i,
                    Type = info.ClassType,
                    Position = new List<int> { i * 280, 0 },
                    Inputs = nodeInputs,
                    Outputs = outputs,
                    WidgetsValues = valueInputs,
                    Properties = new Dictionary<string, string> { { "Node name for S&R", info.ClassType } },
                };

                // Logger.Log($"{node.Type}\nID {node.Id}\nInputs:\n{string.Join("\n", Prnt(nodeInputs))}\nOutputs:\n{string.Join("\n", Prnt(outputs))}\nWidget Values:\n{string.Join("\n", Prnt(valueInputs))}\n\n\n", hidden: true, filename: "workflow-test");

                graph.Nodes.Add(node);
                workflowPromptNodeMap.Add(node, prompt.ElementAt(i));
            }

            // Reconstruct links: Array of [link ID, source node ID, source node output slot index, target node ID, target node input ID, Connection type]
            for(int i = 0; i < graph.Nodes.Count; i++)
            {
                Node graphNode = graph.Nodes[i];
                var promptNodeName = workflowPromptNodeMap[graphNode].Key;
                NodeInfo promptNodeInfo = workflowPromptNodeMap[graphNode].Value;
            
                foreach(var inp in graphNode.Inputs)
                {
                    int linkId = graph.LastLinkId++;
                    int sourceNodeIndex = inp.SourceIdAndSlot[0];
                    int sourceNodeOutputSlot = inp.SourceIdAndSlot[1];
                    int targetNodeIndex = graphNode.Id;
                    int targetNodeInputId = graphNode.Inputs.IndexOf(inp);

                    var link = new List<object> {
                            linkId, // Link ID
                            sourceNodeIndex, // Source Node ID
                            sourceNodeOutputSlot, // Source Node output slot index
                            targetNodeIndex, // Target Node ID
                            targetNodeInputId, // Target Node input ID
                            inp.Type, // Connection type
                        };

                    // Logger.Log($"Link {link[0]} connects {link[1]}:{link[2]} to {link[3]}:{link[4]} ({link[5]})", hidden: true, filename: "workflow-test");
                    graph.Links.Add(link);

                    // Apply to this input
                    inp.Link = linkId;

                    // Apply to outputs
                    var sourceNode = graph.Nodes.Where(n => n.Id == inp.SourceIdAndSlot[0]).FirstOrDefault();
                    sourceNode.Outputs[sourceNodeOutputSlot].Links[0] = linkId;
                }
            }

            string json = JsonConvert.SerializeObject(graph, Formatting.Indented);
            string jsonPath = Path.Combine(Paths.GetSessionDataPath(), $"comfy_wf_{_workflowCounter}.json");
            _workflowCounter++;
            File.WriteAllText(jsonPath, json);
            Logger.Log($"ComfyUI workflow JSON for current prompt written to disk, reconstruction & saving took {sw.ElapsedMilliseconds} ms", hidden: true);
            return json;
        }

        private static string GetInputNodeType(NodeInfo info, string inputName)
        {
            if(info.InputTypes.ContainsKey(inputName))
                return info.InputTypes[inputName].ToString().ToUpper();

            return inputName.ToUpper().Replace("SAMPLES", "LATENT").Replace("PIXELS", "IMAGE").Replace("IMAGES", "IMAGE");
        }

        public class ComfyGraph
        {
            [JsonProperty("last_node_id")]
            public int LastNodeId { get; set; }

            [JsonProperty("last_link_id")]
            public int LastLinkId { get; set; }

            [JsonProperty("nodes")]
            public List<Node> Nodes { get; set; } = new List<Node>();

            [JsonProperty("links")]
            public List<List<object>> Links { get; set; } = new List<List<object>>();

            [JsonProperty("groups")]
            public List<object> Groups { get; set; } = new List<object>();

            [JsonProperty("config")]
            public Dictionary<string, object> Config { get; set; } = new Dictionary<string, object>();

            [JsonProperty("extra")]
            public ExtraData Extra { get; set; }

            [JsonProperty("version")]
            public double Version { get; set; }
        }

        public class Node
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("pos")]
            public List<int> Position { get; set; }

            [JsonProperty("size")]
            public Dictionary<string, double> Size { get; set; } = new Dictionary<string, double> { { "0", 250.0 }, { "1", 50.0 } };

            [JsonProperty("flags")]
            public Dictionary<string, object> Flags { get; set; } = new Dictionary<string, object>();

            [JsonProperty("order")]
            public int Order { get; set; } = 0;

            [JsonProperty("mode")]
            public int Mode { get; set; } = 0;

            [JsonProperty("inputs")]
            public List<InConnection> Inputs { get; set; }

            [JsonProperty("outputs")]
            public List<OutConnection> Outputs { get; set; }

            [JsonProperty("properties")]
            public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

            [JsonProperty("widgets_values")]
            public List<object> WidgetsValues { get; set; }
        }

        public class InConnection
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("link")]
            public int Link { get; set; } = -1;

            [JsonIgnore]
            public int[] SourceIdAndSlot { get; set; }

            public override string ToString()
            {
                return $"{Name}/{Type}/{Link}";
            }
        }

        public class OutConnection
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("links")]
            public List<int> Links { get; set; } = new List<int>() { -1 };

            [JsonProperty("slot_index")]
            public int? SlotIndex { get; set; }

            [JsonProperty("shape")]
            public int? Shape { get; set; } = 3;

            public override string ToString()
            {
                return $"{Name}/{Type}/{string.Join(", ", Links)}/{SlotIndex}/{Shape}";
            }
        }

        public class ExtraData
        {
            [JsonProperty("ds")]
            public DisplaySettings DisplaySettings { get; set; }
        }

        public class DisplaySettings
        {
            [JsonProperty("scale")]
            public double Scale { get; set; }

            [JsonProperty("offset")]
            public List<double> Offset { get; set; }
        }
    }
}
