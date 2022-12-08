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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class ConvertModelForm : Form
    {
        private Enums.Models.Format _currentInFormat;
        private Enums.Models.Format _currentOutFormat;

        public ConvertModelForm()
        {
            InitializeComponent();
        }

        private void ConvertModelForm_Load(object sender, EventArgs e)
        {
            // LoadModels();
            comboxInFormat.FillFromEnum<Enums.Models.Format>(Strings.ModelFormats);
            comboxOutFormat.FillFromEnum<Enums.Models.Format>(Strings.ModelFormats);

            // ConfigParser.LoadComboxIndex(comboxOutFormat, Config.Keys.PrunePrecisionIdx);
            ConfigParser.LoadGuiElement(checkboxDeleteInput, Config.Keys.ConvertModelsDeleteInput);
        }

        private void btnReloadModels_Click(object sender, EventArgs e)
        {
            LoadModels();
        }

        private void btnOpenModelFolder_Click(object sender, EventArgs e)
        {
            new ModelFoldersForm(Enums.StableDiffusion.ModelType.Normal).ShowDialogForm();
            LoadModels();
        }

        private void LoadModels()
        {
            comboxModel.Items.Clear();
            Paths.GetModels().ForEach(x => comboxModel.Items.Add(x.Name));

            if (comboxModel.SelectedIndex < 0 && comboxModel.Items.Count > 0)
                comboxModel.SelectedIndex = 0;
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

            var model = Paths.GetModel(comboxModel.Text, false);
            Model outModel = await ConvertModels.Convert(_currentInFormat, _currentOutFormat, model, true);

            Program.SetState(Program.BusyState.Standby);
            Enabled = true;
            btnRun.Text = "Convert!";
            
        }

        private void ConvertModelForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfig();
        }

        private void SaveConfig ()
        {
            // ConfigParser.SaveComboxIndex(comboxOutFormat, Config.Keys.PrunePrecisionIdx);
            ConfigParser.SaveGuiElement(checkboxDeleteInput, Config.Keys.ConvertModelsDeleteInput);
        }

        private void comboxInFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentInFormat = ParseUtils.GetEnum<Enums.Models.Format>(comboxInFormat.Text, true, Strings.ModelFormats);
            LoadModels();
        }
    }
}
