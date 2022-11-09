using StableDiffusionGui.Extensions;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class PostProcSettingsForm : CustomForm
    {
        public enum UpscaleOption { X2, X3, X4 }
        public enum FaceRestoreOption { Gfpgan, CodeFormer }

        public PostProcSettingsForm()
        {
            Opacity = 0;
            InitializeComponent();
        }

        private void PostProcSettingsForm_Load(object sender, EventArgs e)
        {

        }

        private async void PostProcSettingsForm_Shown(object sender, EventArgs e)
        {
            Refresh();
            comboxUpscale.FillFromEnum<UpscaleOption>(Strings.PostProcSettingsUiStrings);
            comboxFaceRestoration.FillFromEnum<FaceRestoreOption>(Strings.PostProcSettingsUiStrings);
            LoadSettings();
            UpdateVisibility();
            TabOrderInit(new List<Control>() { checkboxUpscaleEnable, comboxUpscale, checkboxFaceRestorationEnable, comboxFaceRestoration, sliderFaceRestoreStrength, sliderCodeformerFidelity });
            await Task.Delay(1);
            Opacity = 1;

            if (!InstallationStatus.HasSdUpscalers())
            {
                DialogResult dialogResult = UiUtils.ShowMessageBox("Upscalers are not installed.\nDo you want to open the installer to install them (Up to 1 GB of disk space required)?", "Error", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                    new InstallerForm().ShowDialogForm();

                Close();
            }
        }

        private void PostProcSettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        void LoadSettings()
        {
            ConfigParser.LoadGuiElement(checkboxUpscaleEnable);
            ConfigParser.LoadComboxIndex(comboxUpscale);
            ConfigParser.LoadGuiElement(sliderUpscaleStrength);
            ConfigParser.LoadGuiElement(checkboxFaceRestorationEnable);
            ConfigParser.LoadComboxIndex(comboxFaceRestoration);
            ConfigParser.LoadGuiElement(sliderFaceRestoreStrength);
            ConfigParser.LoadGuiElement(sliderCodeformerFidelity);
        }

        void SaveSettings()
        {
            ConfigParser.SaveGuiElement(checkboxUpscaleEnable);
            ConfigParser.SaveComboxIndex(comboxUpscale);
            ConfigParser.SaveGuiElement(sliderUpscaleStrength);
            ConfigParser.SaveGuiElement(checkboxFaceRestorationEnable);
            ConfigParser.SaveComboxIndex(comboxFaceRestoration);
            ConfigParser.SaveGuiElement(sliderFaceRestoreStrength);
            ConfigParser.SaveGuiElement(sliderCodeformerFidelity);
        }

        private void checkboxUpscaleEnable_CheckedChanged(object sender, EventArgs e)
        {
            UpdateVisibility();
        }

        private void checkboxFaceRestorationEnable_CheckedChanged(object sender, EventArgs e)
        {
            UpdateVisibility();
        }

        private void comboxFaceRestoration_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            panelCodeformerFidelity.Visible = (FaceRestoreOption)comboxFaceRestoration.SelectedIndex == FaceRestoreOption.CodeFormer;
        }
    }
}
