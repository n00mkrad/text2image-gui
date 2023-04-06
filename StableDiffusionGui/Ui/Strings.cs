using StableDiffusionGui.Main;
using System.Collections.Generic;
using static StableDiffusionGui.Forms.PostProcSettingsForm;

namespace StableDiffusionGui.Ui
{
    internal class Strings
    {
        public static Dictionary<string, string> SeamlessMode = new Dictionary<string, string>
        {
            { Enums.StableDiffusion.SeamlessMode.SeamlessBoth.ToString(), "Seamless on All Sides" },
            { Enums.StableDiffusion.SeamlessMode.SeamlessHor.ToString(), "Seamless on Left/Right Edges" },
            { Enums.StableDiffusion.SeamlessMode.SeamlessVert.ToString(), "Seamless on Top/Bottom Edges" },
        };

        public static Dictionary<string, string> SymmetryMode = new Dictionary<string, string>
        {
            { Enums.StableDiffusion.SymmetryMode.SymVert.ToString(), "Symmetrical on Vertical Axis" },
            { Enums.StableDiffusion.SymmetryMode.SymHor.ToString(), "Symmetrical on Horizontal Axis" },
            { Enums.StableDiffusion.SymmetryMode.SymBoth.ToString(), "Symmetrical on Both Axes (Cross)" },
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
            // { Enums.StableDiffusion.Sampler.K_Dpmpp_2_A.ToString(), "DPM++ 2 Ancestral" },
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
            { Enums.Utils.FaceTool.Gfpgan.ToString(), "GFPGAN"},
            { Enums.Utils.FaceTool.CodeFormer.ToString(), "CodeFormer"}
        };

        public static Dictionary<string, string> Implementation = new Dictionary<string, string>
        {
            { Enums.StableDiffusion.Implementation.InvokeAi.ToString(), "Stable Diffusion (InvokeAI - CUDA - Most Features)" },
            { Enums.StableDiffusion.Implementation.OptimizedSd.ToString(), "Stable Diffusion (OptimizedSD - CUDA - Low Memory Mode)" },
            { Enums.StableDiffusion.Implementation.DiffusersOnnx.ToString(), "Stable Diffusion (ONNX - DirectML - For AMD GPUs)" },
            { Enums.StableDiffusion.Implementation.InstructPixToPix.ToString(), "InstructPix2Pix (Diffusers - CUDA)" },
        };

        public static Dictionary<string, string> ImageImportMode = new Dictionary<string, string>
        {
            { Enums.Misc.ImageImportAction.LoadImage.ToString(), "Load as Initialization Image" },
            { Enums.Misc.ImageImportAction.LoadSettings.ToString(), "Use Settings From Metadata" },
            { Enums.Misc.ImageImportAction.LoadImageAndSettings.ToString(), "Load Image and Use Settings From Metadata" },
            { Enums.Misc.ImageImportAction.CopyPrompt.ToString(), "Copy Prompt" },
        };

        public static Dictionary<string, string> ChromaKeyMode = new Dictionary<string, string>
        {
            { Enums.Misc.ChromaKeyColor.None.ToString(), "Disabled" },
            { Enums.Misc.ChromaKeyColor.Black.ToString(), "Make Black Pixels Transparent" },
            { Enums.Misc.ChromaKeyColor.White.ToString(), "Make White Pixels Transparent" },
            { Enums.Misc.ChromaKeyColor.Green.ToString(), "Make Green Pixels Transparent" },
        };

        public static Dictionary<string, string> SdPrecision = new Dictionary<string, string>()
        {
            { Enums.Models.Precision.Fp16.ToString(), "Half Precision (FP16 - 2 GB)" },
            { Enums.Models.Precision.Fp32.ToString(), "Full Precision (FP32 - 4 GB)" },
        };

        public static Dictionary<string, string> ModelFormats = new Dictionary<string, string>()
        {
            { Enums.Models.Format.Pytorch.ToString(), "Pytorch (ckpt/pt File)" },
            { Enums.Models.Format.Diffusers.ToString(), "Diffusers (Folder)" },
            { Enums.Models.Format.DiffusersOnnx.ToString(), "Diffusers ONNX (Folder)" },
            { Enums.Models.Format.Safetensors.ToString(), "Safetensors (safetensors File)" },
        };

        public static Dictionary<string, string> TimestampModes = new Dictionary<string, string>()
        {
            { Enums.Export.FilenameTimestamp.None.ToString(), "No Timestamp" },
            { Enums.Export.FilenameTimestamp.Date.ToString(), "Date" },
            { Enums.Export.FilenameTimestamp.DateTime.ToString(), "Date and Time" },
            { Enums.Export.FilenameTimestamp.UnixEpoch.ToString(), "Unix Epoch" },
        };

        public static Dictionary<string, string> ImageGravity = new Dictionary<string, string>
        {
            { ImageMagick.Gravity.West.ToString(), "Center Left" },
            { ImageMagick.Gravity.Center.ToString(), "Center" },
            { ImageMagick.Gravity.East.ToString(), "Center Right" },
            { ImageMagick.Gravity.Northwest.ToString(), "Top Left" },
            { ImageMagick.Gravity.North.ToString(), "Top Middle" },
            { ImageMagick.Gravity.Northeast.ToString(), "Top Right" },
            { ImageMagick.Gravity.Southwest.ToString(), "Bottom Left" },
            { ImageMagick.Gravity.South.ToString(), "Bottom Middle" },
            { ImageMagick.Gravity.Southeast.ToString(), "Bottom Right" },
        };

        public static Dictionary<string, string> MainUiCategories = new Dictionary<string, string>()
        {
            { "btnCollapsePrompt", "Prompt Settings" },
            { "btnCollapseImplementation", "Implementation Settings" },
            { "btnCollapseRendering", "Rendering Settings" },
            { "btnCollapseGeneration", "Generation Settings" },
            { "btnCollapseSymmetry", "Symmetry Settings" },
            { "btnCollapseDebug", "Debug Settings" },
        };

        public static Dictionary<string, string> SdModelArch = new Dictionary<string, string>()
        {
            { Enums.Models.SdArch.V1.ToString(), "SD 1.x" },
            { Enums.Models.SdArch.V2.ToString(), "SD 2.x (512 px)" },
            { Enums.Models.SdArch.V2V.ToString(), "SD 2.x (768 px)" },
        };
    }
}
