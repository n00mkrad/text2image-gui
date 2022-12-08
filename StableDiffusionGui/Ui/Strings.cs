using Microsoft.VisualBasic;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StableDiffusionGui.Forms.PostProcSettingsForm;

namespace StableDiffusionGui.Ui
{
    internal class Strings
    {
        public static Dictionary<string, string> SeamlessMode = new Dictionary<string, string>
        {
            // Seamless Modes
            { Enums.StableDiffusion.SeamlessMode.SeamlessBoth.ToString(), "Seamless on All Sides" },
            { Enums.StableDiffusion.SeamlessMode.SeamlessHor.ToString(), "Seamless on Left/Right Edges" },
            { Enums.StableDiffusion.SeamlessMode.SeamlessVert.ToString(), "Seamless on Top/Bottom Edges" },
        };

        public static Dictionary<string, string> InpaintMode = new Dictionary<string, string>
        {
            { Enums.StableDiffusion.InpaintMode.ImageMask.ToString(), "Image Mask (Draw Mask)" },
            { Enums.StableDiffusion.InpaintMode.TextMask.ToString(), "Text Mask (Describe Objects)" },
        };

        public static Dictionary<string, string> Samplers = new Dictionary<string, string>
        {
            { Enums.StableDiffusion.Sampler.K_Euler_A.ToString(), "Euler Ancestral" },
            { Enums.StableDiffusion.Sampler.K_Euler.ToString(), "Euler" },
            { Enums.StableDiffusion.Sampler.K_Dpmpp_2.ToString(), "DPM++ 2" },
            { Enums.StableDiffusion.Sampler.K_Dpmpp_2_A.ToString(), "DPM++ 2 Ancestral" },
            { Enums.StableDiffusion.Sampler.K_Lms.ToString(), "LMS" },
            { Enums.StableDiffusion.Sampler.Ddim.ToString(), "DDIM" },
            { Enums.StableDiffusion.Sampler.Plms.ToString(), "PLMS" },
            { Enums.StableDiffusion.Sampler.K_Heun.ToString(), "Heun" },
            { Enums.StableDiffusion.Sampler.K_Dpm_2.ToString(), "DPM 2" },
            { Enums.StableDiffusion.Sampler.K_Dpm_2_A.ToString(), "DPM 2 Ancestral" },
        };

        public static Dictionary<string, string> PostProcSettingsUiStrings = new Dictionary<string, string>
        {
            { UpscaleOption.X2.ToString(), "2x" },
            { UpscaleOption.X3.ToString(), "3x" },
            { UpscaleOption.X4.ToString(), "4x" },
            { FaceRestoreOption.Gfpgan.ToString(), "GFPGAN"},
            { FaceRestoreOption.CodeFormer.ToString(), "CodeFormer"}
        };

        public static Dictionary<string, string> Implementation = new Dictionary<string, string>
        {
            { Enums.StableDiffusion.Implementation.InvokeAi.ToString(), "Stable Diffusion (InvokeAI - CUDA - Most Features)" },
            { Enums.StableDiffusion.Implementation.OptimizedSd.ToString(), "Stable Diffusion (OptimizedSD - CUDA - Low Memory Mode)" },
            { Enums.StableDiffusion.Implementation.DiffusersOnnx.ToString(), "Stable Diffusion (ONNX - DirectML - For AMD GPUs)" },
        };

        public static Dictionary<string, string> ImageImportMode = new Dictionary<string, string>
        {
            // Seamless Modes
            { Enums.Misc.ImageImportAction.LoadImage.ToString(), "Load as Initialization Image" },
            { Enums.Misc.ImageImportAction.LoadSettings.ToString(), "Use Settings From Metadata" },
            { Enums.Misc.ImageImportAction.LoadImageAndSettings.ToString(), "Load Image and Use Settings From Metadata" },
            { Enums.Misc.ImageImportAction.CopyPrompt.ToString(), "Copy Prompt" },
        };

        public static Dictionary<string, string> SdPytorchOptions = new Dictionary<string, string>()
        {
            { Enums.Models.Precision.Fp16.ToString(), "Half Precision (FP16 - 2 GB)" },
            { Enums.Models.Precision.Fp32.ToString(), "Full Precision (FP32 - 4 GB)" },
        };

        public static Dictionary<string, string> ModelTypes = new Dictionary<string, string>()
        {
            { Enums.Models.Precision.Fp16.ToString(), "Half Precision (FP16 - 2 GB)" },
            { Enums.Models.Precision.Fp32.ToString(), "Full Precision (FP32 - 4 GB)" },
        };

        public static Dictionary<string, string> ModelFormats = new Dictionary<string, string>()
        {
            { Enums.Models.Format.Pytorch.ToString(), "Pytorch (ckpt/pt Files)" },
            { Enums.Models.Format.Diffusers.ToString(), "Diffusers (Folder)" },
            { Enums.Models.Format.DiffusersOnnx.ToString(), "Diffusers ONNX (Folder)" },
        };
    }
}
