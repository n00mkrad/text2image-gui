using StableDiffusionGui.Data;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Io;
using StableDiffusionGui.Ui;
using System.Linq;
using System.Windows.Forms;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Forms
{
    public partial class MainForm
    {
        public void SetVisibility(Control control)
        {
            control.SetVisible(ShouldControlBeVisible(control));
        }

        public bool ShouldControlBeVisible(Control control)
        {
            if (control == panelRes)
                return ResolutionAdjustAvailable();

            if (control == panelScaleImg)
                return ImgScaleAvailable();

            if (control == panelInitImgStrength)
                return InitImgStrengthAvailable();

            if (control == panelInpainting)
                return InpaintingAvailable();

            if (control == checkboxHiresFix)
                return HiresFixAvailable();

            return false;
        }

        private static bool ResolutionAdjustAvailable()
        {
            bool available = true;

            if (ConfigParser.CurrentImplementation == Implementation.InstructPixToPix)
                return MainUi.CurrentInitImgPaths.Any(); // Only visible if image is loaded

            return available;
        }

        private static bool ImgScaleAvailable()
        {
            bool available = true;

            if (ConfigParser.CurrentImplementation != Implementation.InstructPixToPix)
                available = false;

            return available;
        }

        private bool InitImgStrengthAvailable()
        {
            bool available = false;

            if (ConfigParser.CurrentImplementation == Implementation.InstructPixToPix)
                return false;

            if ((InpaintMode)comboxInpaintMode.SelectedIndex != InpaintMode.Disabled)
                return false;

            bool img2img = MainUi.CurrentInitImgPaths.Any();

            if (img2img)
                available = true;

            return available;
        }

        private static bool InpaintingAvailable()
        {
            bool available = false;

            bool img2img = MainUi.CurrentInitImgPaths.Any();
            bool inpaintCompat = ConfigParser.CurrentImplementation.Supports(ImplementationInfo.Feature.NativeInpainting);

            if (img2img && inpaintCompat)
                available = true;

            return available;
        }

        private bool HiresFixAvailable ()
        {
            bool compatible = ConfigParser.CurrentImplementation == Implementation.InvokeAi;
            return compatible && (comboxResW.GetInt() > 512 || comboxResH.GetInt() > 512) && !AnyInits;
        }
    }
}
