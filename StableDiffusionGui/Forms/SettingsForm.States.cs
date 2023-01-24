using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Forms
{
    public partial class SettingsForm
    {
        public bool ShouldControlBeVisible(Control control)
        {
            return ShouldControlBeVisible(this, control);
        }

        public static bool ShouldControlBeVisible(SettingsForm instance, Control control)
        {
            if (control == instance.panelFullPrecision)
                return PrecisionAvailable();

            if (control == instance.panelUnloadModel)
                return UnloadModelAvailable();

            if (control == instance.panelCudaDevice)
                return CudaDeviceSelectionAvailable();

            if (control == instance.panelSdModel)
                return ModelSelectionAvailable();

            if (control == instance.panelVae)
                return VaeSelectionAvailable();

            return false;
        }

        private static bool PrecisionAvailable()
        {
            var precisionImps = new List<Implementation> { Implementation.InvokeAi, Implementation.OptimizedSd };
            return precisionImps.Contains(ConfigParser.CurrentImplementation);
        }

        private static bool UnloadModelAvailable()
        {
            var unloadModelImps = new List<Implementation> { Implementation.InvokeAi, Implementation.OptimizedSd };
            return unloadModelImps.Contains(ConfigParser.CurrentImplementation);
        }

        private static bool CudaDeviceSelectionAvailable()
        {
            var unloadModelImps = new List<Implementation> { Implementation.InvokeAi, Implementation.OptimizedSd, Implementation.InstructPixToPix };
            return unloadModelImps.Contains(ConfigParser.CurrentImplementation);
        }

        private static bool ModelSelectionAvailable()
        {
            var singleModelImps = new List<Implementation> { Implementation.InstructPixToPix };
            return !singleModelImps.Contains(ConfigParser.CurrentImplementation);
        }

        private static bool VaeSelectionAvailable()
        {
            var unloadModelImps = new List<Implementation> { Implementation.InvokeAi };
            return unloadModelImps.Contains(ConfigParser.CurrentImplementation);
        }
    }
}
