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
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Forms
{
    public partial class PostProcSettingsForm : CustomForm
    {
        public enum UpscaleOption { X2, X3, X4 }

        public PostProcSettingsForm()
        {
            Opacity = 0;
            InitializeComponent();

            var imp = ConfigParser.CurrentImplementation;
            var supportedImps = new List<Implementation> { Implementation.InvokeAi };

            if (!supportedImps.Contains(imp))
            {
                panel2.Visible = false;
                tableLayoutPanel8.Visible = false;
                tableLayoutPanel2.Visible = false;
                tableLayoutPanel5.Visible = false;

                cbUpscaleType.Items.RemoveAt(0);
            }

            cbUpscaleType.SelectedItem = cbUpscaleType.Items[0];
        }

        private void PostProcSettingsForm_Load(object sender, EventArgs e)
        {

        }

        private async void PostProcSettingsForm_Shown(object sender, EventArgs e)
        {
            Refresh();
            TabOrderInit(new List<Control>() { checkboxUpscaleEnable, comboxUpscale, checkboxFaceRestorationEnable, comboxFaceRestoration, sliderFaceRestoreStrength, sliderCodeformerFidelity });

            comboxUpscale.FillFromEnum<UpscaleOption>(Strings.PostProcSettingsUiStrings);
            comboxFaceRestoration.FillFromEnum<Enums.Utils.FaceTool>(Strings.PostProcSettingsUiStrings);
            LoadSettings();
            UpdateVisibility();
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
            ConfigParser.LoadGuiElement(checkboxUpscaleEnable, Config.Keys.UpscaleEnable);
            ConfigParser.LoadComboxIndex(comboxUpscale, Config.Keys.UpscaleIdx);
            ConfigParser.LoadGuiElement(sliderUpscaleStrength, Config.Keys.UpscaleStrength);
            ConfigParser.LoadGuiElement(checkboxFaceRestorationEnable, Config.Keys.FaceRestoreEnable);
            ConfigParser.LoadComboxIndex(comboxFaceRestoration, Config.Keys.FaceRestoreIdx);
            ConfigParser.LoadGuiElement(sliderFaceRestoreStrength, Config.Keys.FaceRestoreStrength);
            ConfigParser.LoadGuiElement(sliderCodeformerFidelity, Config.Keys.CodeformerFidelity);
            ConfigParser.LoadGuiElement(cbUpscaleType, Config.Keys.UpscaleType);
        }

        void SaveSettings()
        {
            ConfigParser.SaveGuiElement(checkboxUpscaleEnable, Config.Keys.UpscaleEnable);
            ConfigParser.SaveComboxIndex(comboxUpscale, Config.Keys.UpscaleIdx);
            ConfigParser.SaveGuiElement(sliderUpscaleStrength, Config.Keys.UpscaleStrength);
            ConfigParser.SaveGuiElement(checkboxFaceRestorationEnable, Config.Keys.FaceRestoreEnable);
            ConfigParser.SaveComboxIndex(comboxFaceRestoration, Config.Keys.FaceRestoreIdx);
            ConfigParser.SaveGuiElement(sliderFaceRestoreStrength, Config.Keys.FaceRestoreStrength);
            ConfigParser.SaveGuiElement(sliderCodeformerFidelity, Config.Keys.CodeformerFidelity);
            ConfigParser.SaveGuiElement(cbUpscaleType, Config.Keys.UpscaleType);
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
            panelCodeformerFidelity.Visible = (Enums.Utils.FaceTool)comboxFaceRestoration.SelectedIndex == Enums.Utils.FaceTool.CodeFormer;


            var imp = ConfigParser.CurrentImplementation;
            var supportedImps = new List<Implementation> { Implementation.InvokeAi };

            if (!supportedImps.Contains(imp))
            {
                panel2.Visible = false;
                tableLayoutPanel8.Visible = false;
                tableLayoutPanel2.Visible = false;
                label5.Visible = false;
                label4.Visible = false;
                label3.Visible = false;
                label6.Visible = false;
                label8.Visible = false;
                checkboxFaceRestorationEnable.Visible = false;
                comboxFaceRestoration.Visible = false;
                sliderCodeformerFidelity.Visible = false;
                sliderFaceRestoreStrength.Visible = false;
                tableLayoutPanel5.Visible = false;
            }
        }
    }
}
