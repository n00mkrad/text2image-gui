using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StableDiffusionGui.Main.Enums.Models;

namespace StableDiffusionGui.Data
{
    public class ImplementationInfo
    {
        public Enums.Ai.Backend Backend { get; set; } = Enums.Ai.Backend.Cuda;
        public bool SupportsDeviceSelection { get; set; } = false;
        public bool IsInteractive { get; set; } = false;
        public bool HasPrecisionOpt { get; set; } = false;
        public bool SupportsCustomModels { get; set; } = true;
        public bool SupportsCustomVaeModels { get; set; } = false;
        public bool SupportsNativeInpainting { get; set; } = false;
        public bool SupportsNegativePrompt { get; set; } = false;
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
                SupportsDeviceSelection = true;
                IsInteractive = true;
                HasPrecisionOpt = true;
                SupportsCustomModels = true;
                SupportsCustomVaeModels = true;
                SupportsNativeInpainting = true;
                SupportsNegativePrompt = true;
            }
            else if (imp == Enums.StableDiffusion.Implementation.OptimizedSd)
            {
                Backend = Enums.Ai.Backend.Cuda;
                SupportedModelFormats = new Format[] { Format.Pytorch };
                ValidModelExts = new string[] { ".ckpt" };
                SupportsDeviceSelection = true;
                IsInteractive = false;
                HasPrecisionOpt = true;
                SupportsCustomModels = true;
                SupportsCustomVaeModels = false;
            }
            else if (imp == Enums.StableDiffusion.Implementation.DiffusersOnnx)
            {
                Backend = Enums.Ai.Backend.DirectMl;
                SupportedModelFormats = new Format[] { Format.DiffusersOnnx };
                SupportsDeviceSelection = false;
                IsInteractive = true;
                HasPrecisionOpt = true;
                SupportsCustomModels = true;
                SupportsCustomVaeModels = false;
                SupportsNativeInpainting = true;
                SupportsNegativePrompt = true;
            }
            else if (imp == Enums.StableDiffusion.Implementation.InstructPixToPix)
            {
                Backend = Enums.Ai.Backend.Cuda;
                SupportsDeviceSelection = false;
                IsInteractive = true;
                HasPrecisionOpt = false;
                SupportsCustomModels = false;
                SupportsCustomVaeModels = false;
                SupportsNegativePrompt = true;
            }
        }
    }
}
