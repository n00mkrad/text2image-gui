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
        public void SetVisibility(IEnumerable<Control> controls, Implementation implementation = (Implementation)(-1))
        {
            foreach (Control c in controls)
                c.SetVisible(ShouldControlBeVisible(c, implementation));
        }

        public void SetVisibility(Control control, Implementation implementation = (Implementation)(-1))
        {
            control.SetVisible(ShouldControlBeVisible(control, implementation));
        }

        public bool ShouldControlBeVisible(Control control, Implementation implementation = (Implementation)(-1))
        {
            if (implementation.IsUnset())
                implementation = Config.Instance.Implementation;

            if (control == panelBaseImg)
                return implementation.Supports(Feature.Img2Img);

            if (control == panelRes)
                return ResolutionAdjustAvailable(implementation);

            if (control == panelScaleImg)
                return implementation == Implementation.InstructPixToPix;

            if (control == panelInitImgStrength)
                return InitImgStrengthAvailable(implementation);

            if (control == panelInpainting)
                return InpaintingAvailable(implementation);

            if (control == checkboxHiresFix)
                return HiresFixAvailable(implementation);

            if (control == panelPromptNeg)
                return implementation.Supports(Feature.NegPrompts);

            if (control == panelSampler)
                return implementation.Supports(Feature.MultipleSamplers);

            if (control == panelSeamless)
                return implementation.Supports(Feature.SeamlessMode) && (!comboxInpaintMode.Visible || (ImgMode)comboxInpaintMode.SelectedIndex == ImgMode.InitializationImage);

            if (control == panelSymmetry)
                return implementation.Supports(Feature.SymmetricMode) && (!comboxInpaintMode.Visible || (ImgMode)comboxInpaintMode.SelectedIndex == ImgMode.InitializationImage);

            if (control == panelEmbeddings)
                return implementation.Supports(Feature.Embeddings) && comboxEmbeddingList.Items.Count > 0;

            if (control == textboxClipsegMask)
                return false; // return (ImgMode)comboxInpaintMode.SelectedIndex == ImgMode.TextMask;

            if (control == panelResizeGravity || control == labelResChange)
                return comboxInpaintMode.Visible && (ImgMode)comboxInpaintMode.SelectedIndex == ImgMode.Outpainting;

            if (control == btnResetRes)
                return labelResChange.Visible && labelResChange.Text.IsNotEmpty();

            if (control == checkboxShowInitImg)
                return AnyInits;

            if(control == panelModel)
                return implementation.Supports(Feature.CustomModels);

            if (control == panelLoras)
                return implementation.Supports(Feature.Lora) && gridLoras.Rows.Count > 0;

            if (control == panelRefineStart)
                return implementation == Implementation.SdXl;

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

            if ((ImgMode)comboxInpaintMode.SelectedIndex != ImgMode.InitializationImage)
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
            bool inpaintCompat = imp.Supports(Feature.NativeInpainting);

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
