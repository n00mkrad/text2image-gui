using System;

namespace StableDiffusionGui.Main
{
    public class Enums
    {
        public class Models
        {
            public enum Format { Fp16, Fp32 }
        }

        public class StableDiffusion
        {
            public enum Implementation { InvokeAi, OptimizedSd, DiffusersOnnx }
            public enum ModelType { Normal, Vae, Embedding }
            public enum Sampler { K_Euler_A, K_Euler, K_Lms, Ddim, Plms, K_Heun, K_Dpm_2, K_Dpm_2_A }
            public enum SeamlessMode { Disabled, SeamlessBoth, SeamlessHor, SeamlessVert }
        }

        public class Dreambooth
        {
            public enum TrainPreset { VeryHighQuality, HighQuality, MedQuality, LowQuality }
        }

        public class Cuda
        {
            public enum Device { Automatic, Cpu }
        }
    }
}
