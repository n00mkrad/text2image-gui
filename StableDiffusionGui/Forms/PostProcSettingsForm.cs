using StableDiffusionGui.Extensions;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class PostProcSettingsForm : CustomForm
    {
        public enum UpscaleOption { X2, X3, X4 }

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
            comboxFaceRestoration.FillFromEnum<Enums.Utils.FaceTool>(Strings.PostProcSettingsUiStrings);
            comboxUpscaler.SetItems(Models.GetUpscalers(), 0);
            LoadSettings();
            UpdateVisibility();
            TabOrderInit(new List<Control>() { checkboxUpscaleEnable, comboxUpscale, checkboxFaceRestorationEnable, comboxFaceRestoration, sliderFaceRestoreStrength, sliderCodeformerFidelity });
            await Task.Delay(1);

            if (!InstallationStatus.HasSdUpscalers())
            {
                DialogResult dialogResult = UiUtils.ShowMessageBox("Upscalers are not installed.\nDo you want to install them (Roughly 1 GB of disk space required)?", "Error", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                    new InstallerForm(true).ShowDialogForm();

                if (!InstallationStatus.HasSdUpscalers())
                {
                    Close();
                    return;
                }
            }

            Opacity = 1;
        }

        private void PostProcSettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        void LoadSettings()
        {
            ConfigParser.LoadGuiElement(checkboxUpscaleEnable, ref Config.Instance.UpscaleEnable);
            ConfigParser.LoadComboxIndex(comboxUpscale, ref Config.Instance.UpscaleIdx);
            ConfigParser.LoadGuiElement(comboxUpscaler, ref Config.Instance.EsrganModel);
            ConfigParser.LoadGuiElement(checkboxFaceRestorationEnable, ref Config.Instance.FaceRestoreEnable);
            ConfigParser.LoadComboxIndex(comboxFaceRestoration, ref Config.Instance.FaceRestoreIdx);
            ConfigParser.LoadGuiElement(sliderFaceRestoreStrength, ref Config.Instance.FaceRestoreStrength);
            ConfigParser.LoadGuiElement(sliderCodeformerFidelity, ref Config.Instance.CodeformerFidelity);
        }

        void SaveSettings()
        {
            ConfigParser.SaveGuiElement(checkboxUpscaleEnable, ref Config.Instance.UpscaleEnable);
            ConfigParser.SaveComboxIndex(comboxUpscale, ref Config.Instance.UpscaleIdx);
            ConfigParser.SaveGuiElement(comboxUpscaler, ref Config.Instance.EsrganModel);
            ConfigParser.SaveGuiElement(checkboxFaceRestorationEnable, ref Config.Instance.FaceRestoreEnable);
            ConfigParser.SaveComboxIndex(comboxFaceRestoration, ref Config.Instance.FaceRestoreIdx);
            ConfigParser.SaveGuiElement(sliderFaceRestoreStrength, ref Config.Instance.FaceRestoreStrength);
            ConfigParser.SaveGuiElement(sliderCodeformerFidelity, ref Config.Instance.CodeformerFidelity);
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
        }
    }
}
