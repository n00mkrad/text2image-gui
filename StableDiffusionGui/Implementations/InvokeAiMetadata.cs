using System.Collections.Generic;
using Newtonsoft.Json;

namespace StableDiffusionGui.Implementations
{
    public class InvokeAiMetadata
    {
        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("model_id")]
        public string ModelId { get; set; }

        // [JsonProperty("model_hash")]
        // public int ModelHash { get; set; }
        // 
        // [JsonProperty("app_id")]
        // public string AppId { get; set; }
        // 
        // [JsonProperty("app_version")]
        // public string AppVersion { get; set; }

        [JsonProperty("image")]
        public Image ImageData { get; set; }

        public class Image
        {
            [JsonProperty("upscale")]
            public string Upscale { get; set; }

            [JsonProperty("facetool")]
            public string Facetool { get; set; }

            [JsonProperty("threshold")]
            public float Threshold { get; set; }

            [JsonProperty("h_symmetry_time_pct")]
            public float HSymmetryTimePct { get; set; }

            [JsonProperty("seed")]
            public long Seed { get; set; }

            [JsonProperty("width")]
            public int Width { get; set; }

            [JsonProperty("height")]
            public int Height { get; set; }

            [JsonProperty("hires_fix")]
            public bool HiResFix { get; set; }

            [JsonProperty("init_mask")]
            public string InitMask { get; set; }

            [JsonProperty("v_symmetry_time_pct")]
            public float VSymmetryTimePct { get; set; }

            [JsonProperty("facetool_strength")]
            public float FacetoolStrength { get; set; }

            [JsonProperty("perlin")]
            public float Perlin { get; set; }

            [JsonProperty("cfg_scale")]
            public float CfgScale { get; set; }

            [JsonProperty("steps")]
            public int Steps { get; set; }

            [JsonProperty("prompt")]
            public List<Prompt> Prompt { get; set; }

            [JsonProperty("postprocessing")]
            public string Postprocessing { get; set; }

            [JsonProperty("sampler")]
            public string Sampler { get; set; }

            [JsonProperty("variations")]
            public List<string> Variations { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("strength_steps")]
            public float StrengthSteps { get; set; }

            [JsonProperty("inpaint_replace")]
            public float InpaintReplace { get; set; }
        }

        public class Prompt
        {
            [JsonProperty("prompt")]
            public string Text { get; set; }

            [JsonProperty("weight")]
            public float Weight { get; set; }
        }
    }
}
