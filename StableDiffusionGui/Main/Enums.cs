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
            public enum Format { Diffusers, Pytorch, Safetensors, DiffusersOnnx }
            public enum Type { Normal, Vae, Refiner, Embedding, Lora, ControlNet, Upscaler }
        }

        public class StableDiffusion
        {
            public enum ModelArch { Automatic, Sd1, Sd1Inpaint, Sd2, Sd2Inpaint, Sd2V, SdXlBase, SdXlRefine, Flux }
            public enum Implementation { InvokeAi, DiffusersOnnx, InstructPixToPix, Comfy }
            public enum Sampler { K_Dpmpp_2M, Dpmpp_2M, K_Dpmpp_2M_Sde, Dpmpp_2M_Sde, K_Dpmpp_3M_Sde, Dpmpp_3M_Sde, K_Euler, Euler_A, Euler, Ddim, Lms, Heun, Dpm_2, Dpm_2_A, Uni_Pc }
            public enum SeamlessMode { Disabled, SeamlessBoth, SeamlessVert, SeamlessHor }
            public enum SymmetryMode { Disabled, SymVert, SymHor, SymBoth }
            public enum ImgMode { InitializationImage, ImageMask, Outpainting, Controlnet }
            public enum UpscaleMode { Disabled, LatentsFactor, LatentsTargetRes, UltimeUpsFactor }
            public enum UpscaleMethod { Latent, LatentLegacy, UltimateSd }
            public enum ImagePreprocessor { None, Canny, LineArtHed, LineArt, LineArtAnime, LineArtMangaAnime, DepthMap, Blur, Pixelate, OpenPose }
        }

        public class Comfy
        {
            public enum VramPreset { GpuOnly, HighVram, NormalVram, LowVram, NoVram }
        }

        public class Utils
        {
            public enum FaceTool { Gfpgan, CodeFormer }
        }

        public class Training
        {
            public enum TrainPreset { VeryHighQuality, HighQuality, MedQuality, LowQuality }
            public enum LoraSize { Tiny, Small, Normal, Big }
            public enum CaptionMode { NoCaption, UseSinglePhrase, UseTxtFiles }
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
