using System;

namespace StableDiffusionGui.Main
{
    public class Enums
    {
        public class Program
        {
            public enum UpdateChannel { Public, Beta }
        }

        public class Export
        {
            public enum FilenameTimestamp { None, Date, DateTime, UnixEpoch }
        }

        public class Models
        {
            public enum Precision { Fp16, Fp32 }
            public enum Format { Pytorch, Diffusers, DiffusersOnnx, Safetensors }
            public enum Type { Normal, Vae, Embedding, Lora }
            public enum SdArch { Automatic, V1, V2, V2V }

        }

        public class StableDiffusion
        {
            public enum Implementation { InvokeAi, OptimizedSd, DiffusersOnnx, InstructPixToPix }
            public enum Sampler { K_Euler_A, K_Euler, K_Dpmpp_2, K_Lms, Ddim, Plms, K_Heun, K_Dpm_2_A, K_Dpm_2 }
            public enum SeamlessMode { Disabled, SeamlessBoth, SeamlessVert, SeamlessHor }
            public enum SymmetryMode { Disabled, SymVert, SymHor, SymBoth }
            public enum InpaintMode { Disabled, ImageMask, TextMask, Outpaint }
        }

        public class Utils
        {
            public enum FaceTool { Gfpgan, CodeFormer }
        }

        public class Dreambooth
        {
            public enum TrainPreset { VeryHighQuality, HighQuality, MedQuality, LowQuality }
        }

        public class Ai
        {
            public enum Backend { Cuda, DirectMl }
        }

        public class Cuda
        {
            public enum Device { Automatic, Cpu }
        }

        public class Misc
        {
            public enum ImageImportAction { LoadImage, LoadSettings, LoadImageAndSettings, CopyPrompt /* , LoadIntoImgViewer */ }
            public enum ChromaKeyColor { None, Black, White, Green }
        }
    }
}
