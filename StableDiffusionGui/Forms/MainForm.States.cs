using StableDiffusionGui.Data;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Io;
using StableDiffusionGui.Ui;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static StableDiffusionGui.Data.ImplementationInfo;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Forms
{
    public partial class MainForm
    {
        public void SetVisibility(IEnumerable<Control> controls, Implementation currImplementation = (Implementation)(-1))
        {
            foreach (Control c in controls)
                c.SetVisible(ShouldControlBeVisible(c, currImplementation));
        }

        public void SetVisibility(Control control, Implementation currImplementation = (Implementation)(-1))
        {
            control.SetVisible(ShouldControlBeVisible(control, currImplementation));
        }

        public bool ShouldControlBeVisible(Control control, Implementation currImplementation = (Implementation)(-1))
        {
            if (currImplementation.IsUnset())
                currImplementation = ConfigParser.CurrentImplementation;

            if (control == panelRes)
                return ResolutionAdjustAvailable(currImplementation);

            if (control == panelScaleImg)
                return currImplementation == Implementation.InstructPixToPix;

            if (control == panelInitImgStrength)
                return InitImgStrengthAvailable(currImplementation);

            if (control == panelInpainting)
                return InpaintingAvailable(currImplementation);

            if (control == checkboxHiresFix)
                return HiresFixAvailable(currImplementation);

            if (control == panelPromptNeg)
                return currImplementation.Supports(Feature.NegPrompts);

            if (control == panelSampler)
                return currImplementation.Supports(Feature.MultipleSamplers);

            if (control == panelSeamless)
                return currImplementation.Supports(Feature.SeamlessMode) && (!comboxInpaintMode.Visible || (InpaintMode)comboxInpaintMode.SelectedIndex == InpaintMode.Disabled);

            if (control == panelSymmetry)
                return currImplementation.Supports(Feature.SymmetricMode) && (!comboxInpaintMode.Visible || (InpaintMode)comboxInpaintMode.SelectedIndex == InpaintMode.Disabled);

            if (control == panelEmbeddings)
                return currImplementation.Supports(Feature.Embeddings);

            if (control == textboxClipsegMask)
                return (InpaintMode)comboxInpaintMode.SelectedIndex == InpaintMode.TextMask;

            if (control == panelResizeGravity || control == labelResChange)
                return comboxInpaintMode.Visible && (InpaintMode)comboxInpaintMode.SelectedIndex == InpaintMode.Outpaint;

            if (control == btnResetRes)
                return labelResChange.Visible && labelResChange.Text.IsNotEmpty();

            if (control == checkboxShowInitImg)
                return AnyInits;

            return false;
        }

        private static bool ResolutionAdjustAvailable(Implementation imp)
        {
            bool available = true;

            if (imp == Implementation.InstructPixToPix)
                return MainUi.CurrentInitImgPaths.Any(); // Only visible if image is loaded

            return available;
        }

        private bool InitImgStrengthAvailable(Implementation imp)
        {
            bool available = false;

            if (imp == Implementation.InstructPixToPix)
                return false;

            if ((InpaintMode)comboxInpaintMode.SelectedIndex != InpaintMode.Disabled)
                return false;

            bool img2img = MainUi.CurrentInitImgPaths.Any();

            if (img2img)
                available = true;

            return available;
        }

        private static bool InpaintingAvailable(Implementation imp)
        {
            bool available = false;

            bool img2img = MainUi.CurrentInitImgPaths.Any();
            bool inpaintCompat = imp.Supports(ImplementationInfo.Feature.NativeInpainting);

            if (img2img && inpaintCompat)
                available = true;

            return available;
        }

        private bool HiresFixAvailable(Implementation imp)
        {
            bool compatible = imp.Supports(Feature.HiresFix);
            bool biggerThan512 = comboxResW.GetInt() > 512 || comboxResH.GetInt() > 512;
            return compatible && biggerThan512 && !AnyInits;
        }
    }
}
