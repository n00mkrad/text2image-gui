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

            return true;
        }

        private static bool ResolutionAdjustAvailable()
        {
            bool available = true;

            if (FormControls.CurrImpl == Implementation.InstructPixToPix)
                available = false;

            return available;
        }

        private static bool EmbeddingsAvailable()
        {
            bool available = true;

            if (FormControls.CurrImpl != Implementation.InvokeAi)
                available = false;

            return available;
        }

        private static bool ImgScaleAvailable()
        {
            bool available = true;

            if (FormControls.CurrImpl != Implementation.InstructPixToPix)
                available = false;

            return available;
        }

        private static bool InitImgStrengthAvailable()
        {
            bool available = false;

            if (FormControls.CurrImpl == Implementation.InstructPixToPix)
                return false;

            bool img2img = MainUi.CurrentInitImgPaths != null;

            if (img2img && !FormControls.IsUsingInpaintingModel)
                available = true;

            return available;
        }
    }
}
