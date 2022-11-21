using StableDiffusionGui.Main;
using System;

namespace StableDiffusionGui.Installation
{
    internal class EnabledFeatures
    {
        public static bool InvokeAiModelCaching { get { return false; } }
        public static bool WildcardAutocomplete { get { return false; } }

        public static bool Implementation (Enums.StableDiffusion.Implementation implementation)
        {
            switch (implementation)
            {
                case Enums.StableDiffusion.Implementation.InvokeAi: return true;
                case Enums.StableDiffusion.Implementation.OptimizedSd: return true;
                case Enums.StableDiffusion.Implementation.DiffusersOnnx: return false;
                default: return true; 
            }
        }

        public static bool Sampler(Enums.StableDiffusion.Sampler sampler)
        {
            switch (sampler)
            {
                case Enums.StableDiffusion.Sampler.K_Dpmpp_2: return false;
                case Enums.StableDiffusion.Sampler.K_Dpmpp_2_A: return false;
                default: return true;
            }
        }
    }
}
