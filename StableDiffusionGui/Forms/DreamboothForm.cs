using Microsoft.WindowsAPICodePack.Dialogs;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class DreamboothForm : Form
    {
        private Dictionary<string, string> _uiStrings = new Dictionary<string, string>()
        {
            { Enums.Dreambooth.TrainPreset.VeryHighQuality.ToString(), "Very High Quality (80 minutes on RTX 3090)" },
            { Enums.Dreambooth.TrainPreset.HighQuality.ToString(), "High Quality (40 Minutes on RTX 3090)" },
            { Enums.Dreambooth.TrainPreset.MedQuality.ToString(), "Medium Quality (20 Minutes on RTX 3090)" },
            { Enums.Dreambooth.TrainPreset.LowQuality.ToString(), "Low Quality, for Testing (6 Minutes on RTX 3090)" },
        };

        public DreamboothForm()
        {
            InitializeComponent();
        }

        private void DreamboothForm_Load(object sender, EventArgs e)
        {
            comboxTrainPreset.FillFromEnum<Enums.Dreambooth.TrainPreset>(_uiStrings, 0);
            LoadModels();
        }

        private async void DreamboothForm_Shown(object sender, EventArgs e)
        {
            await PerformChecks();
        }

        private async Task PerformChecks()
        {
            try
            {
                bool valid = true;
                var gpus = await GpuUtils.GetCudaGpusCached();

                Text = "DreamBooth Training";

                if (valid && gpus.Count < 1)
                {
                    UiUtils.ShowMessageBox("No compatible GPU detected.", UiUtils.MessageType.Error);
                    valid = false;
                }

                int cudaDeviceOpt = Config.GetInt("comboxCudaDevice");
                Data.Gpu gpu = gpus[cudaDeviceOpt - 2];

                if (valid && gpu.VramGb < 23f)
                {
                    UiUtils.ShowMessageBox($"Your GPU has {gpu.VramGb} GB VRAM, but 24 GB are currently required for DreamBooth training.", UiUtils.MessageType.Error);
                    valid = false;
                }
                else if (valid && gpu.VramGb < 25f)
                {
                    UiUtils.ShowMessageBox($"Your GPU has {gpu.VramGb} GB VRAM.\nThis is enough to train DreamBooth, but you should make sure that no other VRAM-consuming applications are running (Games, Browsers, Game Launchers, AI/ML).", UiUtils.MessageType.Message);
                }

                if (valid)
                    btnStart.Enabled = true;
                else
                    Close();
            }
            catch(Exception ex)
            {
                Close();
                Logger.Log($"Error: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
            }
        }

        private void btnReloadModels_Click(object sender, EventArgs e)
        {
            LoadModels();
        }

        private void btnOpenModelFolder_Click(object sender, EventArgs e)
        {
            new ModelFoldersForm().ShowDialog();
            LoadModels();
        }

        private void LoadModels()
        {
            comboxBaseModel.Items.Clear();
            Paths.GetModels().ForEach(x => comboxBaseModel.Items.Add(x.Name));

            if (comboxBaseModel.SelectedIndex < 0 && comboxBaseModel.Items.Count > 0)
                comboxBaseModel.SelectedIndex = 0;
        }


        private async void btnRun_Click(object sender, EventArgs e)
        {
            if (Program.Busy)
            {
                UiUtils.ShowMessageBox("Please wait until the current process has finished.");
                return;
            }

            // if (string.IsNullOrWhiteSpace(comboxBaseModel.Text) || string.IsNullOrWhiteSpace(comboxTrainPreset.Text) || comboxBaseModel.Text == comboxTrainPreset.Text)
            // {
            //     UiUtils.ShowMessageBox("Invalid model selection.");
            //     return;
            // }

            Program.MainForm.SetWorking(true);
            Enabled = false;
            //btnStart.Text = "Merging...";

            FileInfo baseModel = Paths.GetModel(comboxBaseModel.Text);
            DirectoryInfo trainImgDir = new DirectoryInfo(textboxTrainImgsDir.Text.Trim());
            string className = textboxClassName.Text.Trim().Trunc(50, false);
            Enums.Dreambooth.TrainPreset preset = (Enums.Dreambooth.TrainPreset)comboxTrainPreset.SelectedIndex;

            string outPath = await Dreambooth.RunTraining(baseModel, trainImgDir, className, preset);

            Program.MainForm.SetWorking(false);
            Enabled = true;
            //btnStart.Text = "Merge!";

            //if (File.Exists(outPath))
            //    Logger.Log($"Done. Saved merged model to:\n{outPath.Replace(Paths.GetDataPath(), "Data")}");
            //else
            //    Logger.Log($"Failed to merge models.");

            //if (File.Exists(outPath))
            //    UiUtils.ShowMessageBox($"Done.\n\nSaved merged model to:\n{outPath}");
            //else
            //    UiUtils.ShowMessageBox($"Failed to merge models.");
        }

        private void btnTrainImgsBrowse_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { InitialDirectory = textboxTrainImgsDir.Text, IsFolderPicker = true };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok && Directory.Exists(dialog.FileName))
                textboxTrainImgsDir.Text = dialog.FileName;
        }
    }
}
