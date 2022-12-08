using System;

namespace StableDiffusionGui.Main
{
    public class Enums
    {
        public class Models
        {
            public enum Precision { Fp16, Fp32 }
            public enum Format { Pytorch, Diffusers, DiffusersOnnx, Safetensors }
        }

        public class StableDiffusion
        {
            public enum Implementation { InvokeAi, OptimizedSd, DiffusersOnnx }
            public enum ModelType { Normal, Vae, Embedding }
            public enum Sampler { K_Euler_A, K_Euler, K_Dpmpp_2_A, K_Dpmpp_2, K_Lms, Ddim, Plms, K_Heun, K_Dpm_2_A, K_Dpm_2 }
            public enum SeamlessMode { Disabled, SeamlessBoth, SeamlessHor, SeamlessVert }
            public enum InpaintMode { Disabled, ImageMask, TextMask }
        }

        public class Dreambooth
        {
            public enum TrainPreset { VeryHighQuality, HighQuality, MedQuality, LowQuality }
        }

        public class Cuda
        {
            public enum Device { Automatic, Cpu }
        }

        public class Misc
        {
            public enum ImageImportAction { LoadImage, LoadSettings, LoadImageAndSettings, CopyPrompt /* , LoadIntoImgViewer */ }
        }
    }
}
