using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static StableDiffusionGui.Main.TextToImage;

namespace StableDiffusionGui.Data
{
    public enum Implementation { StableDiffusion, StableDiffusionOptimized }

    public class TtiSettings
    {
        public Implementation Implementation { get; set; } = Implementation.StableDiffusion;
        public string[] Prompts { get; set; } = new string[] { "" };
        public int Iterations { get; set; } = 1;
        public string OutDir { get; set; } = "";
        public Dictionary<string, string> Params { get; set; } = new Dictionary<string, string>();
    }
}
