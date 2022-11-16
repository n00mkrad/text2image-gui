using StableDiffusionGui.Main;
using System;

namespace StableDiffusionGui.Installation
{
    internal class EnabledFeatures
    {
        public static bool Onnx { get { return true; } }
        public static bool WildcardAutocomplete { get { return true; } }

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
