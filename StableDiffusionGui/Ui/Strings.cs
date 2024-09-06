using StableDiffusionGui.Main;
using StableDiffusionGui.Training;
using System.Collections.Generic;
using static StableDiffusionGui.Forms.PostProcSettingsForm;

namespace StableDiffusionGui.Ui
{
    internal class Strings
    {
        public static Dictionary<string, string> SeamlessMode = new Dictionary<string, string>
        {
            { Enums.StableDiffusion.SeamlessMode.Disabled.ToString(), "Not Seamless" },
            { Enums.StableDiffusion.SeamlessMode.SeamlessBoth.ToString(), "Seamless on All Sides" },
            { Enums.StableDiffusion.SeamlessMode.SeamlessHor.ToString(), "Seamless on Left/Right Edges" },
            { Enums.StableDiffusion.SeamlessMode.SeamlessVert.ToString(), "Seamless on Top/Bottom Edges" },
        };

        public static Dictionary<string, string> SymmetryMode = new Dictionary<string, string>
        {
            { Enums.StableDiffusion.SymmetryMode.Disabled.ToString(), "Symmetry Disabled" },
            { Enums.StableDiffusion.SymmetryMode.SymVert.ToString(), "Symmetrical on Vertical Axis" },
            { Enums.StableDiffusion.SymmetryMode.SymHor.ToString(), "Symmetrical on Horizontal Axis" },
            { Enums.StableDiffusion.SymmetryMode.SymBoth.ToString(), "Symmetrical on Both Axes (Cross)" },
        };

        public static Dictionary<string, string> InpaintMode = new Dictionary<string, string>
        {
            { Enums.StableDiffusion.ImgMode.InitializationImage.ToString(), "Base Image (Img2Img)" },
            { Enums.StableDiffusion.ImgMode.ImageMask.ToString(), "Inpainting with Mask" },
            { Enums.StableDiffusion.ImgMode.Outpainting.ToString(), "Outpainting / Fill Transparency" },
            { Enums.StableDiffusion.ImgMode.Controlnet.ToString(), "ControlNet" },
            // { Enums.StableDiffusion.ImgMode.TextMask.ToString(), "Inpainting with Text Description" },
        };

        public static Dictionary<string, string> Samplers = new Dictionary<string, string>
        {
            { Enums.StableDiffusion.Sampler.Euler.ToString(), "Euler" },
            { Enums.StableDiffusion.Sampler.Euler_A.ToString(), "Euler Ancestral" },
            { Enums.StableDiffusion.Sampler.K_Euler.ToString(), "Euler Karras" },
            { Enums.StableDiffusion.Sampler.Dpmpp_2M.ToString(), "DPM++ 2M" },
            { Enums.StableDiffusion.Sampler.K_Dpmpp_2M.ToString(), "DPM++ 2M Karras" },
            { Enums.StableDiffusion.Sampler.Dpmpp_2M_Sde.ToString(), "DPM++ 2M SDE" },
            { Enums.StableDiffusion.Sampler.K_Dpmpp_2M_Sde.ToString(), "DPM++ 2M SDE Karras" },
            { Enums.StableDiffusion.Sampler.Dpmpp_3M_Sde.ToString(), "DPM++ 3M SDE" },
            { Enums.StableDiffusion.Sampler.K_Dpmpp_3M_Sde.ToString(), "DPM++ 3M SDE Karras" },
            { Enums.StableDiffusion.Sampler.Lms.ToString(), "LMS" },
            { Enums.StableDiffusion.Sampler.Ddim.ToString(), "DDIM" },
            // { Enums.StableDiffusion.Sampler.Plms.ToString(), "PLMS" },
            { Enums.StableDiffusion.Sampler.Heun.ToString(), "Heun" },
            { Enums.StableDiffusion.Sampler.Dpm_2.ToString(), "DPM 2" },
            { Enums.StableDiffusion.Sampler.Dpm_2_A.ToString(), "DPM 2 Ancestral" },
            { Enums.StableDiffusion.Sampler.Uni_Pc.ToString(), "UniPC" },
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
            { Enums.StableDiffusion.Implementation.DiffusersOnnx.ToString(), "Stable Diffusion (ONNX - DirectML - For AMD GPUs)" },
            { Enums.StableDiffusion.Implementation.InstructPixToPix.ToString(), "InstructPix2Pix (Diffusers - CUDA)" },
            { Enums.StableDiffusion.Implementation.Comfy.ToString(), "Stable Diffusion (CUDA / DirectML)" },
        };

        public static Dictionary<string, string> ImageImportMode = new Dictionary<string, string>
        {
            { Enums.Misc.ImageImportAction.LoadImage.ToString(), "Load Image" },
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
            { Enums.Models.Format.Pytorch.ToString(), "Pytorch (CKPT/PT File)" },
            { Enums.Models.Format.Diffusers.ToString(), "Diffusers (Folder)" },
            { Enums.Models.Format.DiffusersOnnx.ToString(), "Diffusers ONNX (Folder)" },
            { Enums.Models.Format.Safetensors.ToString(), "Safetensors (Safetensors File)" },
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

        public static Dictionary<string, string> LoraSizes = new Dictionary<string, string>
        {
            { Enums.Training.LoraSize.Tiny.ToString(), "Tiny (7.5 MB File)" },
            { Enums.Training.LoraSize.Small.ToString(), "Small (15 MB File)" },
            { Enums.Training.LoraSize.Normal.ToString(), "Normal (30 MB File)" },
            { Enums.Training.LoraSize.Big.ToString(), "Big (60 MB File)" },
        };

        public static Dictionary<string, string> CaptionModes = new Dictionary<string, string>
        {
            { Enums.Training.CaptionMode.NoCaption.ToString(), "No Caption (LoRA will always influence image)" },
            { Enums.Training.CaptionMode.UseSinglePhrase.ToString(), "Use a Single Word or Phrase" },
            { Enums.Training.CaptionMode.UseTxtFiles.ToString(), "Read Captions From TXT Files in Training Folder" },
        };

        public static Dictionary<string, string> LoraNetworkTypes = new Dictionary<string, string>
        {
            { KohyaSettings.NetworkType.LoCon.ToString(), "LoRA (Vanilla)" },
            { KohyaSettings.NetworkType.LoHa.ToString(), "LoHa (LyCORIS)" },
        };

        public static Dictionary<string, string> ComfyVramPresets = new Dictionary<string, string>
        {
            { Enums.Comfy.VramPreset.GpuOnly.ToString(), "Max (Keep All Data on GPU - Not Recommended!)" },
            { Enums.Comfy.VramPreset.HighVram.ToString(), "High (Don’t Offload Models to CPU)" },
            { Enums.Comfy.VramPreset.NormalVram.ToString(), "Normal (Dynamically Offload Models to CPU)" },
            { Enums.Comfy.VramPreset.LowVram.ToString(), "Low (Slower)" },
            { Enums.Comfy.VramPreset.NoVram.ToString(), "Very Low (Slowest)" },
        };

        public static Dictionary<string, string> UpscaleModes = new Dictionary<string, string>
        {
            { Enums.StableDiffusion.UpscaleMode.LatentsFactor.ToString(), "Latent Upscale Factor" },
            { Enums.StableDiffusion.UpscaleMode.LatentsTargetRes.ToString(), "Latent Upscale Resolution" },
            { Enums.StableDiffusion.UpscaleMode.UltimeUpsFactor.ToString(), "Ultimate SD Upscale Factor" },
        };

        public static Dictionary<string, string> UpscaleMethods = new Dictionary<string, string>
        {
            { Enums.StableDiffusion.UpscaleMethod.Latent.ToString(), "Latent Upscaling" },
            { Enums.StableDiffusion.UpscaleMethod.UltimateSd.ToString(), "Ultimate SD Upscale" },
        };

        public static Dictionary<string, string> ImagePreprocessors = new Dictionary<string, string>()
        {
             { Enums.StableDiffusion.ImagePreprocessor.Canny.ToString(), "Edge Detection (Fine - Canny)" },
             { Enums.StableDiffusion.ImagePreprocessor.LineArtHed.ToString(), "Edge Detection (Soft - HED)" },
             // { Enums.StableDiffusion.ImagePreprocessor.Scribble.ToString(), "Scribble Art" },
             { Enums.StableDiffusion.ImagePreprocessor.LineArt.ToString(), "Line Art" },
             { Enums.StableDiffusion.ImagePreprocessor.LineArtAnime.ToString(), "Line Art (Anime)" },
             { Enums.StableDiffusion.ImagePreprocessor.LineArtMangaAnime.ToString(), "Line Art (Anime, Softer)" },
             { Enums.StableDiffusion.ImagePreprocessor.DepthMap.ToString(), "Depth Map (3D Effect)" },
             { Enums.StableDiffusion.ImagePreprocessor.Blur.ToString(), "Blur (For Tile Model)" },
             { Enums.StableDiffusion.ImagePreprocessor.Pixelate.ToString(), "Pixelate (For Color Model)" },
             { Enums.StableDiffusion.ImagePreprocessor.OpenPose.ToString(), "Detect Pose (OpenPose)" },
        };

        public static Dictionary<string, string> ModelArch = new Dictionary<string, string>
        {
            { Enums.StableDiffusion.ModelArch.Sd1.ToString(), "SD 1.X (512px)" },
            { Enums.StableDiffusion.ModelArch.Sd1Inpaint.ToString(), "SD 1.X Inpainting (512px)" },
            { Enums.StableDiffusion.ModelArch.Sd2.ToString(), "SD 2.0 (512px)" },
            { Enums.StableDiffusion.ModelArch.Sd2Inpaint.ToString(), "SD 2.0 Inpainting (512px)" },
            { Enums.StableDiffusion.ModelArch.Sd2V.ToString(), "SD 2.X V (768px)" },
            { Enums.StableDiffusion.ModelArch.SdXlBase.ToString(), "SD XL Base (1024px)" },
            { Enums.StableDiffusion.ModelArch.SdXlRefine.ToString(), "SD XL Refiner (1024px)" },
            { Enums.StableDiffusion.ModelArch.Flux.ToString(), "FLUX" },
        };
    }
}
