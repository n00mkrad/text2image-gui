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
        public static Dictionary<string, string> MainUiStrings = new Dictionary<string, string>
        {
            // Samplers
            { Enums.StableDiffusion.Sampler.K_Euler_A.ToString(), "Euler Ancestral" },
            { Enums.StableDiffusion.Sampler.K_Euler.ToString(), "Euler" },
            { Enums.StableDiffusion.Sampler.K_Lms.ToString(), "LMS" },
            { Enums.StableDiffusion.Sampler.Ddim.ToString(), "DDIM" },
            { Enums.StableDiffusion.Sampler.Plms.ToString(), "PLMS" },
            { Enums.StableDiffusion.Sampler.K_Heun.ToString(), "Heun" },
            { Enums.StableDiffusion.Sampler.K_Dpm_2.ToString(), "DPM 2" },
            { Enums.StableDiffusion.Sampler.K_Dpm_2_A.ToString(), "DPM 2 Ancestral" },
            // Seamless Modes
            { Enums.StableDiffusion.SeamlessMode.SeamlessBoth.ToString(), "Seamless on All Sides" },
            { Enums.StableDiffusion.SeamlessMode.SeamlessHor.ToString(), "Seamless on Left/Right Edges" },
            { Enums.StableDiffusion.SeamlessMode.SeamlessVert.ToString(), "Seamless on Top/Bottom Edges" },
            // Inpaint Modes
            { Enums.StableDiffusion.InpaintMode.ImageMask.ToString(), "Image Mask (Draw Mask)" },
            { Enums.StableDiffusion.InpaintMode.TextMask.ToString(), "Text Mask (Describe what you want to replace)" },
        };

        public static Dictionary<string, string> PostProcSettingsUiStrings = new Dictionary<string, string>
        {
            { UpscaleOption.X2.ToString(), "2x" },
            { UpscaleOption.X3.ToString(), "3x" },
            { UpscaleOption.X4.ToString(), "4x" },
            { FaceRestoreOption.Gfpgan.ToString(), "GFPGAN"},
            { FaceRestoreOption.CodeFormer.ToString(), "CodeFormer"}
        };

        public static Dictionary<string, string> SettingsUiStrings = new Dictionary<string, string>
        {
            { Enums.StableDiffusion.Implementation.InvokeAi.ToString(), "Stable Diffusion (InvokeAI - CUDA)" },
            { Enums.StableDiffusion.Implementation.OptimizedSd.ToString(), "Stable Diffusion (OptimizedSD - CUDA)" },
            { Enums.StableDiffusion.Implementation.DiffusersOnnx.ToString(), "Stable Diffusion (ONNX - DirectML)" },
        };
    }
}
