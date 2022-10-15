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
            comboxLrMultiplier.Text = "Normal";
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

                if (valid && cudaDeviceOpt == (int)Enums.Cuda.Device.Cpu)
                {
                    UiUtils.ShowMessageBox("DreamBooth training is not supported on CPU.", UiUtils.MessageType.Error);
                    valid = false;
                }

                Data.Gpu gpu = cudaDeviceOpt == (int)Enums.Cuda.Device.Automatic ? gpus[0] : gpus[cudaDeviceOpt - 2];

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
            catch (Exception ex)
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
            if(Program.State == Program.BusyState.Dreambooth)
            {
                ProcessManager.FindAndKillOrphans($"*{Constants.Dirs.Dreambooth}*.py*{Paths.SessionTimestamp}*");
                return;
            }

            if (Program.Busy)
            {
                UiUtils.ShowMessageBox("Please wait until the current process has finished.");
                return;
            }

            if (!Directory.Exists(textboxTrainImgsDir.Text.Trim()))
            {
                UiUtils.ShowMessageBox("Invalid training directory.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textboxClassName.Text.Trim()))
            {
                UiUtils.ShowMessageBox("Please enter a class name.");
                return;
            }

            FileInfo baseModel = Paths.GetModel(comboxBaseModel.Text);

            if (baseModel == null)
            {
                UiUtils.ShowMessageBox("Invalid model selection.");
                return;
            }

            Program.MainForm.SetWorking(Program.BusyState.Dreambooth);
            btnStart.Text = "Cancel";

            DirectoryInfo trainImgDir = new DirectoryInfo(textboxTrainImgsDir.Text.Trim());
            string className = string.Join("_", textboxClassName.Text.Trim().Split(Path.GetInvalidFileNameChars())).Trunc(50, false);
            textboxClassName.Text = className;
            Enums.Dreambooth.TrainPreset preset = (Enums.Dreambooth.TrainPreset)comboxTrainPreset.SelectedIndex;
            float lrMultiplier = comboxLrMultiplier.Text.EndsWith("x") ? comboxLrMultiplier.Text.GetFloat() : 1f;

            string outPath = await Dreambooth.RunTraining(baseModel, trainImgDir, className, preset, lrMultiplier);

            Program.MainForm.SetWorking(Program.BusyState.Standby);
            btnStart.Text = "Start Training";

            if (File.Exists(outPath))
                Logger.Log($"Done. Saved trained model to:\n{outPath.Replace(Paths.GetDataPath(), "Data")}");
            else
                Logger.Log($"Training failed - model file was not saved.");
        }

        private void btnTrainImgsBrowse_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { InitialDirectory = textboxTrainImgsDir.Text, IsFolderPicker = true };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok && Directory.Exists(dialog.FileName))
                textboxTrainImgsDir.Text = dialog.FileName;
        }

        private void DreamboothForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Program.State == Program.BusyState.Dreambooth)
                e.Cancel = true;
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            OpenGuide();
        }

        private void DreamboothForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
                OpenGuide();
        }

        private void OpenGuide()
        {
            System.Diagnostics.Process.Start("https://github.com/n00mkrad/text2image-gui/blob/main/DreamBooth.md");
        }
    }
}
