using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using StableDiffusionGui.Data;
using StableDiffusionGui.Main;
using System.Collections.Generic;
using System.Linq;
using static StableDiffusionGui.Implementations.ComfyNodes;
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
            public Dictionary<string, ConnectionType> InputTypes { get; set; } = new Dictionary<string, ConnectionType>();
            public List<ConnectionType> OutputTypes { get; set; } = new List<ConnectionType>();
            public string ClassType { get; set; }
        }

        public static T AddNode<T>(List<INode> nodes, string preferredName = "") where T : INode, new()
        {
            string name = preferredName.IsNotEmpty() ? preferredName : typeof(T).ToString().Split('+').Last();
            T newNode = new T() { Id = GetNewId(nodes, name) };
            nodes.Add(newNode);
            return newNode;
        }

        public static INode AddNode(INode obj, List<INode> nodes, string preferredName = "")
        {
            string name = preferredName.IsNotEmpty() ? preferredName : obj.GetType().Name;
            obj.Id = GetNewId(nodes, name);
            nodes.Add(obj);
            return obj;
        }

        public static string GetNewId(List<INode> nodes, string preferred)
        {
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
