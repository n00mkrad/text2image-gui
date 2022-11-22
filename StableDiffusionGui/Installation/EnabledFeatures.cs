using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Installation
{
    internal class EnabledFeatures
    {
        public static bool InvokeAiModelCaching { get { return false; } }
        public static bool WildcardAutocomplete { get { return false; } }

        public static List<Implementation> DisabledImplementations { get { return new List<Implementation>() { Implementation.DiffusersOnnx }; } }
        // public static List<Sampler> DisabledSamplers { get { return new List<Sampler>() { Sampler.K_Dpmpp_2, Sampler.K_Dpmpp_2_A }; } }
        public static List<InpaintMode> DisabledInpaintModes { get { return new List<InpaintMode>() { InpaintMode.TextMask }; } }
    }
}
