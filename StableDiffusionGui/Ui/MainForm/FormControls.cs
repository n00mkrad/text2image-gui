using StableDiffusionGui.Extensions;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Ui.MainForm
{
    internal class FormControls
    {

        private static StableDiffusionGui.MainForm F { get { return Program.MainForm; } }

        public static void InitializeControls()
        {
            F.comboxSampler.FillFromEnum<Sampler>(Strings.Samplers, 0, EnabledFeatures.DisabledSamplers);
            F.comboxSeamless.FillFromEnum<SeamlessMode>(Strings.SeamlessMode, 0);
            F.comboxInpaintMode.FillFromEnum<InpaintMode>(Strings.InpaintMode, 0, EnabledFeatures.DisabledInpaintModes);

            var resItems = MainUi.Resolutions.Where(x => x <= (Config.GetBool("checkboxAdvancedMode") ? 2048 : 1024)).Select(x => x.ToString());
            F.comboxResW.SetItems(resItems, UiExtensions.SelectMode.Last);
            F.comboxResH.SetItems(resItems, UiExtensions.SelectMode.Last);
        }

        public static void Load()
        {
            ConfigParser.LoadGuiElement(F.upDownIterations);
            ConfigParser.LoadGuiElement(F.sliderSteps);
            ConfigParser.LoadGuiElement(F.sliderScale);
            ConfigParser.LoadGuiElement(F.comboxResH);
            ConfigParser.LoadGuiElement(F.comboxResW);
            ConfigParser.LoadComboxIndex(F.comboxSampler);
            ConfigParser.LoadGuiElement(F.sliderInitStrength);
        }

        public static void Save()
        {
            ConfigParser.SaveGuiElement(F.upDownIterations);
            ConfigParser.SaveGuiElement(F.sliderSteps);
            ConfigParser.SaveGuiElement(F.sliderScale);
            ConfigParser.SaveGuiElement(F.comboxResH);
            ConfigParser.SaveGuiElement(F.comboxResW);
            ConfigParser.SaveComboxIndex(F.comboxSampler);
            ConfigParser.SaveGuiElement(F.sliderInitStrength);
        }

        public static void RefreshUiAfterSettingsChanged()
        {
            var imp = (Implementation)Config.GetInt("comboxImplementation");
            F.panelPromptNeg.Visible = imp != Implementation.OptimizedSd;
            F.btnEmbeddingBrowse.Enabled = imp == Implementation.InvokeAi;
            F.panelSampler.Visible = imp == Implementation.InvokeAi;
            F.panelSeamless.Visible = imp == Implementation.InvokeAi;

            bool adv = Config.GetBool("checkboxAdvancedMode");
            F.upDownIterations.Maximum = !adv ? 10000 : 100000;
            F.sliderSteps.ActualMaximum = !adv ? 120 : 500;
            F.sliderSteps.ValueStep = !adv ? 5 : 1;
            F.sliderScale.ActualMaximum = !adv ? 25 : 50;
            F.comboxResW.SetItems(MainUi.Resolutions.Where(x => x <= (adv ? 2048 : 1024)).Select(x => x.ToString()), UiExtensions.SelectMode.Retain, UiExtensions.SelectMode.Last);
            F.comboxResH.SetItems(MainUi.Resolutions.Where(x => x <= (adv ? 2048 : 1024)).Select(x => x.ToString()), UiExtensions.SelectMode.Retain, UiExtensions.SelectMode.Last);
        }
    }
}
