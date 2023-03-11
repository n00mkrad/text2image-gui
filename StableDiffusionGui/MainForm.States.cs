using StableDiffusionGui.Io;
using StableDiffusionGui.Ui;
using StableDiffusionGui.Ui.MainFormUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui
{
    public partial class MainForm
    {
        public bool ShouldControlBeVisible (Control control)
        {
            return ShouldControlBeVisible(this, control);
        }

        public static bool ShouldControlBeVisible(MainForm instance, Control control)
        {
            if (control == instance.panelRes)
                return ResolutionAdjustAvailable();

            if (control == instance.btnEmbeddingBrowse)
                return EmbeddingsAvailable();

            if (control == instance.panelScaleImg)
                return ImgScaleAvailable();

            if (control == instance.panelInitImgStrength)
                return InitImgStrengthAvailable();

            if (control == instance.panelInpainting)
                return InpaintingAvailable();

            return false;
        }

        private static bool ResolutionAdjustAvailable()
        {
            bool available = true;

            if (ConfigParser.CurrentImplementation == Implementation.InstructPixToPix)
                return MainUi.CurrentInitImgPaths.Any(); // Only visible if image is loaded

            return available;
        }

        private static bool EmbeddingsAvailable()
        {
            bool available = true;

            if (ConfigParser.CurrentImplementation != Implementation.InvokeAi)
                available = false;

            return available;
        }

        private static bool ImgScaleAvailable()
        {
            bool available = true;

            if (ConfigParser.CurrentImplementation != Implementation.InstructPixToPix)
                available = false;

            return available;
        }

        private static bool InitImgStrengthAvailable()
        {
            bool available = false;

            if (ConfigParser.CurrentImplementation == Implementation.InstructPixToPix)
                return false;

            bool img2img = MainUi.CurrentInitImgPaths.Any();

            if (img2img && !FormControls.IsUsingInpaintingModel)
                available = true;

            return available;
        }

        private static bool InpaintingAvailable()
        {
            bool available = false;

            bool img2img = MainUi.CurrentInitImgPaths.Any();
            bool inpaintCompat = ConfigParser.CurrentImplementation.GetInfo().SupportsNativeInpainting;

            if (img2img && inpaintCompat)
                available = true;

            return available;
        }
    }
}
