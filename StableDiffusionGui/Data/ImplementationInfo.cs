using StableDiffusionGui.Main;
using System.Collections.Generic;
using static StableDiffusionGui.Main.Enums.Models;

namespace StableDiffusionGui.Data
{
    public class ImplementationInfo
    {
        public enum Feature { InteractiveCli, CustomModels, CustomVae, HalfPrecisionToggle, NegPrompts, Img2Img, NativeInpainting, DeviceSelection, MultipleSamplers, Embeddings, SeamlessMode, SymmetricMode, HiresFix, Lora, Controlnet }
        public List<Feature> SupportedFeatures = new List<Feature>();
        public Enums.Ai.Backend Backend { get; set; } = Enums.Ai.Backend.Cuda;
        public string[] ValidModelExts { get; set; } = new string[0];
        public string[] ValidModelExtsVae { get; set; } = new string[0];
        public Format[] SupportedModelFormats { get; set; } = new Format[0];

        public ImplementationInfo() { }

        public ImplementationInfo(Enums.StableDiffusion.Implementation imp)
        {
            if (imp == Enums.StableDiffusion.Implementation.InvokeAi)
            {
                Backend = Enums.Ai.Backend.Cuda;
                SupportedModelFormats = new Format[] { Format.Pytorch, Format.Safetensors, Format.Diffusers };
                ValidModelExts = new string[] { ".ckpt", ".safetensors" };
                ValidModelExtsVae = new string[] { ".ckpt", ".pt" };
                SupportedFeatures = new List<Feature> { Feature.InteractiveCli, Feature.CustomModels, Feature.CustomVae, Feature.HalfPrecisionToggle, Feature.NegPrompts, Feature.Img2Img, Feature.NativeInpainting, Feature.DeviceSelection,
                    Feature.MultipleSamplers, Feature.Embeddings, Feature.SeamlessMode, Feature.SymmetricMode, Feature.HiresFix, Feature.Lora };
            }
            else if (imp == Enums.StableDiffusion.Implementation.DiffusersOnnx)
            {
                Backend = Enums.Ai.Backend.DirectMl;
                SupportedModelFormats = new Format[] { Format.DiffusersOnnx };
                SupportedFeatures = new List<Feature> { Feature.InteractiveCli, Feature.CustomModels, Feature.HalfPrecisionToggle, Feature.NegPrompts, Feature.MultipleSamplers, Feature.Img2Img };
            }
            else if (imp == Enums.StableDiffusion.Implementation.InstructPixToPix)
            {
                Backend = Enums.Ai.Backend.Cuda;
                SupportedFeatures = new List<Feature> { Feature.InteractiveCli, Feature.NegPrompts, Feature.Img2Img, Feature.MultipleSamplers };
            }
            else if (imp == Enums.StableDiffusion.Implementation.Comfy)
            {
                Backend = Enums.Ai.Backend.Cuda;
                SupportedModelFormats = new Format[] { Format.Safetensors, Format.Pytorch, Format.Diffusers };
                SupportedFeatures = new List<Feature> { Feature.InteractiveCli, Feature.CustomModels, Feature.NegPrompts, Feature.MultipleSamplers, Feature.Img2Img, Feature.HiresFix, Feature.CustomVae, Feature.Lora, Feature.Embeddings, Feature.NativeInpainting, Feature.Controlnet, Feature.HalfPrecisionToggle, Feature.DeviceSelection };
            }
        }
    }
}
