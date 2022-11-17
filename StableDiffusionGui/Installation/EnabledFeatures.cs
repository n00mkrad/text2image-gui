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
    }
}
