using StableDiffusionGui.Extensions;
using StableDiffusionGui.Io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Ui.MainForm
{
    internal class Settings
    {

        private static StableDiffusionGui.MainForm F { get { return Program.MainForm; } }

        private void InitializeControls()
        {
            F.comboxSampler.FillFromEnum<Sampler>(Strings.MainUiStrings);
            F.comboxSeamless.FillFromEnum<SeamlessMode>(Strings.MainUiStrings, 0);
            F.comboxInpaintMode.FillFromEnum<InpaintMode>(Strings.MainUiStrings, 0);

            var resItems = MainUi.Resolutions.Where(x => x <= (Config.GetBool("checkboxAdvancedMode") ? 2048 : 1024)).Select(x => x.ToString());
            F.comboxResW.SetItems(resItems, UiExtensions.SelectMode.Last);
            F.comboxResH.SetItems(resItems, UiExtensions.SelectMode.Last);
        }
    }
}
