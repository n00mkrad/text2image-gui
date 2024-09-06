using StableDiffusionGui.Data;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
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
                return (implementation.Supports(Feature.SeamlessMode) && implementation.Supports(Feature.SymmetricMode)) && (!comboxInpaintMode.Visible || ParseUtils.GetEnum<ImgMode>(comboxInpaintMode.Text, stringMap: Strings.InpaintMode) == ImgMode.InitializationImage);

            if (control == panelEmbeddings)
                return implementation.Supports(Feature.Embeddings) && comboxEmbeddingList.Items.Count > 0;

            if (control == textboxClipsegMask)
                return false; // return ParseUtils.GetEnum<ImgMode>(comboxInpaintMode.Text, stringMap: Strings.InpaintMode) == ImgMode.TextMask;

            if (control == panelResizeGravity || control == labelResChange)
                return comboxInpaintMode.Visible && ParseUtils.GetEnum<ImgMode>(comboxInpaintMode.Text, stringMap: Strings.InpaintMode) == ImgMode.Outpainting;

            if (control == btnResetRes)
                return labelResChange.Visible && labelResChange.Text.IsNotEmpty();

            if (control == checkboxShowInitImg)
                return AnyInits;

            if (control == panelModel)
                return implementation.Supports(Feature.CustomModels);

            if (control == panelLoras)
                return implementation.Supports(Feature.Lora) && gridLoras.Rows.Count > 0;

            if (control == panelModel2)
                return Model2Available(implementation);

            if (control == panelGuidance)
                return GuidanceAvailable(implementation);

            if (control == panelRefineStart)
                return implementation == Implementation.Comfy && ShouldControlBeVisible(panelModel2);

            if (control == panelUpscaling)
                return implementation == Implementation.Comfy && !AnyInits;

            if (control == panelControlnet)
                return ControlnetAvailable(implementation) && ParseUtils.GetEnum<ImgMode>(comboxInpaintMode.Text, stringMap: Strings.InpaintMode) == ImgMode.Controlnet && comboxControlnet.Items.Count > 0;

            if (control == panelModelSettings)
                return new[] { Implementation.InvokeAi, Implementation.Comfy }.Contains(implementation);

            if (control == comboxControlnetSlot)
                return comboxInpaintMode.Visible && ParseUtils.GetEnum<ImgMode>(comboxInpaintMode.Text, stringMap: Strings.InpaintMode) == ImgMode.Controlnet;

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

            if (ParseUtils.GetEnum<ImgMode>(comboxInpaintMode.Text, stringMap: Strings.InpaintMode) != ImgMode.InitializationImage)
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

        private static bool ControlnetAvailable(Implementation imp)
        {
            bool available = false;

            bool img2img = MainUi.CurrentInitImgPaths.Any();
            bool inpaintCompat = imp.Supports(Feature.Controlnet);

            if (img2img && inpaintCompat)
                available = true;

            return available;
        }

        private bool HiresFixAvailable(Implementation imp)
        {
            if (imp == Implementation.Comfy)
                return false;

            bool compatible = imp.Supports(Feature.HiresFix);
            bool biggerThan512 = comboxResW.GetInt() > 512 || comboxResH.GetInt() > 512;
            return compatible && biggerThan512 && !AnyInits;
        }

        private bool Model2Available(Implementation imp)
        {
            if (imp != Implementation.Comfy)
                return false;

            var arch = ParseUtils.GetEnum<ModelArch>(comboxModelArch.Text, stringMap: Strings.ModelArch);
            return new[] { ModelArch.SdXlBase }.Contains(arch);
        }

        private bool GuidanceAvailable(Implementation imp)
        {
            if (imp != Implementation.Comfy)
                return false;

            var arch = ParseUtils.GetEnum<ModelArch>(comboxModelArch.Text, stringMap: Strings.ModelArch);
            return new[] { ModelArch.Flux }.Contains(arch);
        }
    }
}
