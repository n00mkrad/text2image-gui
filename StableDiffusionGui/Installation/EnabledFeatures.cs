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

        public static List<Implementation> DisabledImplementations { get { return new[] { Implementation.DiffusersOnnx }.ToList();  } }
        public static List<Sampler> DisabledSamplers { get { return new[] { Sampler.K_Dpmpp_2, Sampler.K_Dpmpp_2_A }.ToList();  } }
        public static List<InpaintMode> DisabledInpaintModes { get { return new[] { InpaintMode.ImageMask }.ToList();  } }
    }
}
