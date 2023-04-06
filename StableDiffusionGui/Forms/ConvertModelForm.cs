using StableDiffusionGui.Data;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Main.Utils;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class ConvertModelForm : CustomForm
    {
        private Enums.Models.Format _currentInFormat;
        private Enums.Models.Format _currentOutFormat;

        public ConvertModelForm()
        {
            InitializeComponent();
        }

        private void ConvertModelForm_Load(object sender, EventArgs e)
        {

        }

        private void btnReloadModels_Click(object sender, EventArgs e)
        {
            LoadModels();
        }

        private void btnOpenModelFolder_Click(object sender, EventArgs e)
        {
            new ModelFoldersForm().ShowDialogForm();
            LoadModels();
        }

        private void UpdateVisibility()
        {
            ((Action)(() =>
            {
                panelDiffSafetensors.SetVisible(_currentOutFormat == Enums.Models.Format.Diffusers);
                panelFp16.SetVisible(!(_currentOutFormat == Enums.Models.Format.DiffusersOnnx && GpuUtils.CachedGpus.Count <= 0)); // ONNX FP16 conversion currently requires CUDA
                panelModelArch.SetVisible(_currentInFormat == Enums.Models.Format.Pytorch || _currentInFormat == Enums.Models.Format.Safetensors); // Not needed for Diffusers models
            })).RunWithUiStopped(this);
        }

        private void LoadModels()
        {
            ((Action)(() =>
            {
                comboxModel.Items.Clear();
                Models.GetModelsAll().Where(m => m.Format == _currentInFormat && m.Type == Enums.Models.Type.Normal).ToList().ForEach(x => comboxModel.Items.Add(x));

                if (comboxModel.SelectedIndex < 0 && comboxModel.Items.Count > 0)
                    comboxModel.SelectedIndex = 0;

                comboxModel.Enabled = comboxModel.Items.Count >= 1;
            })).RunWithUiStopped(this);
        }

        private async void btnRun_Click(object sender, EventArgs e)
        {
            if (Program.Busy)
            {
                UiUtils.ShowMessageBox("Please wait until the current process has finished.");
                return;
            }

            if (string.IsNullOrWhiteSpace(comboxModel.Text))
            {
                UiUtils.ShowMessageBox("Invalid model selection.");
                return;
            }

            SaveConfig();

            Program.SetState(Program.BusyState.Script);
            Enabled = false;
            btnRun.Text = "Converting...";

            var model = Models.GetModelsAll().Where(m => m.Name == comboxModel.Text).FirstOrDefault();
            bool fp16 = checkboxFp16.Visible && checkboxFp16.Checked;
            bool safeDiffusers = checkboxDiffSafetensors.Visible && checkboxDiffSafetensors.Checked;
            Model outModel = await ConvertModels.Convert(_currentInFormat, _currentOutFormat, model, fp16, safeDiffusers);

            Program.SetState(Program.BusyState.Standby);
            LoadModels();
            Enabled = true;
            btnRun.Text = "Convert!";
        }

        private void ConvertModelForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfig();
        }

        private void SaveConfig()
        {
            ConfigParser.SaveGuiElement(checkboxDeleteInput, ref Config.Instance.ConvertModelsDeleteInput);

            if (Config.Instance != null && comboxModel.SelectedIndex >= 0)
                Config.Instance.ModelArchs[((Model)comboxModel.SelectedItem).FullName] = ParseUtils.GetEnum<Enums.Models.SdArch>(comboxModelArch.Text, true, Strings.SdModelArch);

            Config.Save();
        }

        private void comboxInFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentInFormat = ParseUtils.GetEnum<Enums.Models.Format>(comboxInFormat.Text, true, Strings.ModelFormats);
            comboxOutFormat.FillFromEnum<Enums.Models.Format>(Strings.ModelFormats, 0, new[] { _currentInFormat }.ToList());
            LoadModels();
            UpdateVisibility();
        }

        private void comboxOutFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentOutFormat = ParseUtils.GetEnum<Enums.Models.Format>(comboxOutFormat.Text, true, Strings.ModelFormats);
            UpdateVisibility();
        }

        private async void ConvertModelForm_Shown(object sender, EventArgs e)
        {
            Refresh();
            comboxInFormat.FillFromEnum<Enums.Models.Format>(Strings.ModelFormats, 0, new[] { Enums.Models.Format.DiffusersOnnx }.ToList());
            ConfigParser.LoadGuiElement(checkboxDeleteInput, ref Config.Instance.ConvertModelsDeleteInput);
            comboxModelArch.FillFromEnum<Enums.Models.SdArch>(Strings.SdModelArch, 0);
            TabOrderInit(new List<Control>() { comboxInFormat, comboxModel, comboxOutFormat, checkboxDeleteInput, btnRun }, 0);
            await Task.Delay(1);
            Opacity = 1;
        }

        private void comboxModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            Model mdl = (Model)comboxModel.SelectedItem;

            if (mdl != null && Config.Instance.ModelArchs.ContainsKey(mdl.FullName))
                comboxModelArch.SetIfTextMatches(Config.Instance.ModelArchs[mdl.FullName].ToString(), false, Strings.SdModelArch);
            else if (comboxModelArch.Items.Count > 0)
                comboxModelArch.SelectedIndex = 0;
        }
    }
}
