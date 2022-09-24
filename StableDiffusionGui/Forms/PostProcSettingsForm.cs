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
        public enum UpscaleOption { Disabled, X2, X4 }
        public enum FaceRestoreOption { Disabled, CodeFormer, Gfpgan }

        public Dictionary<string, string> UiStrings = new Dictionary<string, string>();

        private bool _loaded = false;

        public PostProcSettingsForm()
        {
            UiStrings.Add(UpscaleOption.X2.ToString(), "2x");
            UiStrings.Add(UpscaleOption.X4.ToString(), "4x");
            UiStrings.Add(FaceRestoreOption.CodeFormer.ToString(), "CodeFormer (2022)");
            UiStrings.Add(FaceRestoreOption.Gfpgan.ToString(), "GFPGAN (2021)");

            InitializeComponent();
        }

        private void PostProcSettingsForm_Load(object sender, EventArgs e)
        {
            if (!InstallationStatus.HasSdUpscalers())
            {
                DialogResult dialogResult = UiUtils.ShowMessageBox("Upscalers are not installed.\nDo you want to open the installer to install them (Up to 1 GB of disk space required)?", "Error", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                    new InstallerForm().ShowDialog();

                Close();
            }
        }

        private void PostProcSettingsForm_Shown(object sender, EventArgs e)
        {
            comboxUpscale.FillFromEnum<UpscaleOption>(UiStrings);
            comboxFaceRestoration.FillFromEnum<FaceRestoreOption>(UiStrings);

            LoadSettings();
        }

        private void PostProcSettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        void LoadSettings()
        {
            ConfigParser.LoadComboxIndex(comboxUpscale);
            ConfigParser.LoadComboxIndex(comboxFaceRestoration);
            ConfigParser.LoadGuiElement(sliderFaceRestoreStrength); sliderGfpgan_Scroll(null, null);

            _loaded = true;
        }

        void SaveSettings()
        {
            if (!_loaded)
                return;

            ConfigParser.SaveComboxIndex(comboxUpscale);
            ConfigParser.SaveComboxIndex(comboxFaceRestoration);
            ConfigParser.SaveGuiElement(sliderFaceRestoreStrength);
            Config.Set(Config.Key.faceRestoreStrength, PostProcUi.CurrentGfpganStrength.ToStringDot());
        }

        private void sliderGfpgan_Scroll(object sender, ScrollEventArgs e)
        {
            float strength = sliderFaceRestoreStrength.Value / 20f;
            PostProcUi.CurrentGfpganStrength = strength;
            labelFaceRestoreStrength.Text = strength.ToString();
        }

        private void comboxFaceRestoration_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelFaceRestorationStrength.Visible = (FaceRestoreOption)comboxFaceRestoration.SelectedIndex != FaceRestoreOption.Disabled;
        }
    }
}
