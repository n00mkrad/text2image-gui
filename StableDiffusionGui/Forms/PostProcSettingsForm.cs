using StableDiffusionGui.Extensions;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private async void PostProcSettingsForm_Shown(object sender, EventArgs e)
        {
            Refresh();
            comboxUpscale.FillFromEnum<UpscaleOption>(UiStrings);
            comboxFaceRestoration.FillFromEnum<FaceRestoreOption>(UiStrings);
            LoadSettings();
            UpdateVisibility();
            titleLabel.Focus();
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

        private void PostProcSettingsForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        void LoadSettings()
        {
            ConfigParser.LoadGuiElement(checkboxUpscaleEnable);
            ConfigParser.LoadComboxIndex(comboxUpscale);
            ConfigParser.LoadGuiElement(checkboxFaceRestorationEnable);
            ConfigParser.LoadComboxIndex(comboxFaceRestoration);
            ConfigParser.LoadGuiElement(sliderFaceRestoreStrength);
            ConfigParser.LoadGuiElement(sliderCodeformerFidelity);
        }

        void SaveSettings()
        {
            ConfigParser.SaveGuiElement(checkboxUpscaleEnable);
            ConfigParser.SaveComboxIndex(comboxUpscale);
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
