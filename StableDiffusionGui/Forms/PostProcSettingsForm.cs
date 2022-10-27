using StableDiffusionGui.Extensions;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class PostProcSettingsForm : Form
    {
        public enum UpscaleOption { X2, X3, X4 }
        public enum FaceRestoreOption { Gfpgan, CodeFormer }

        public Dictionary<string, string> UiStrings = new Dictionary<string, string>();

        public PostProcSettingsForm()
        {
            Opacity = 0;

            UiStrings.Add(UpscaleOption.X2.ToString(), "2x");
            UiStrings.Add(UpscaleOption.X3.ToString(), "3x");
            UiStrings.Add(UpscaleOption.X4.ToString(), "4x");
            UiStrings.Add(FaceRestoreOption.Gfpgan.ToString(), "GFPGAN");
            UiStrings.Add(FaceRestoreOption.CodeFormer.ToString(), "CodeFormer");

            InitializeComponent();
        }

        private void PostProcSettingsForm_Load(object sender, EventArgs e)
        {

        }

        private void PostProcSettingsForm_Shown(object sender, EventArgs e)
        {
            Refresh();
            comboxUpscale.FillFromEnum<UpscaleOption>(UiStrings);
            comboxFaceRestoration.FillFromEnum<FaceRestoreOption>(UiStrings);
            LoadSettings();
            UpdateVisibility();
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
            ConfigParser.LoadGuiElement(checkboxFaceRestorationEnable);
            ConfigParser.LoadComboxIndex(comboxFaceRestoration);
            ConfigParser.LoadGuiElement(sliderFaceRestoreStrength, ConfigParser.SaveValueAs.Divided, 20f); sliderFaceRestoreStrength_Scroll(null, null);
            ConfigParser.LoadGuiElement(sliderCodeformerFidelity, ConfigParser.SaveValueAs.Divided, 20f); sliderCodeformerFidelity_Scroll(null, null);
        }

        void SaveSettings()
        {
            ConfigParser.SaveGuiElement(checkboxUpscaleEnable);
            ConfigParser.SaveComboxIndex(comboxUpscale);
            ConfigParser.SaveGuiElement(checkboxFaceRestorationEnable);
            ConfigParser.SaveComboxIndex(comboxFaceRestoration);
            ConfigParser.SaveGuiElement(sliderFaceRestoreStrength, ConfigParser.SaveValueAs.Divided, 20f);
            ConfigParser.SaveGuiElement(sliderCodeformerFidelity, ConfigParser.SaveValueAs.Divided, 20f);
        }

        private void sliderFaceRestoreStrength_Scroll(object sender, ScrollEventArgs e)
        {
            float strength = sliderFaceRestoreStrength.Value / 20f;
            PostProcUi.CurrentGfpganStrength = strength;
            labelFaceRestoreStrength.Text = strength.ToString();
        }

        private void sliderCodeformerFidelity_Scroll(object sender, ScrollEventArgs e)
        {
            float strength = sliderCodeformerFidelity.Value / 20f;
            PostProcUi.CurrentCfFidelity = strength;
            labelCodeformerFidelity.Text = strength.ToString();
        }

        private void checkboxUpscaleEnable_CheckedChanged(object sender, EventArgs e)
        {
            UpdateVisibility();
        }

        private void checkboxFaceRestorationEnable_CheckedChanged(object sender, EventArgs e)
        {
            UpdateVisibility();
        }

        private void UpdateVisibility ()
        {
            panelCodeformerFidelity.Visible = (FaceRestoreOption)comboxFaceRestoration.SelectedIndex == FaceRestoreOption.CodeFormer;
        }
    }
}
