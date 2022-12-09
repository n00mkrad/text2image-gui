using Microsoft.WindowsAPICodePack.Dialogs;
using StableDiffusionGui.Data;
using StableDiffusionGui.Extensions;
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
using ZetaLongPaths;
using static StableDiffusionGui.Main.Enums.Dreambooth;

namespace StableDiffusionGui.Forms
{
    public partial class DreamboothForm : CustomForm
    {
        private Dictionary<string, string> _uiStrings = new Dictionary<string, string>();

        public DreamboothForm()
        {
            InitializeComponent();
            AllowTextboxTab = false;
        }

        private void DreamboothForm_Load(object sender, EventArgs e)
        {
            bool is4090 = GpuUtils.CachedGpus.Where(x => x.FullName.Contains("RTX 4090")).Any();
            _uiStrings.Add(TrainPreset.VeryHighQuality.ToString(), $"Very High Quality ({(is4090 ? "50 minutes on RTX 4090" : "80 minutes on RTX 3090")})");
            _uiStrings.Add(TrainPreset.HighQuality.ToString(), $"High Quality ({(is4090 ? "25 minutes on RTX 4090" : "40 minutes on RTX 3090")})");
            _uiStrings.Add(TrainPreset.MedQuality.ToString(), $"Medium Quality ({(is4090 ? "12 minutes on RTX 4090" : "20 minutes on RTX 3090")})");
            _uiStrings.Add(TrainPreset.LowQuality.ToString(), $"Low Quality, for Testing ({(is4090 ? "4 minutes on RTX 4090" : "6 minutes on RTX 3090")})");

            comboxTrainPreset.FillFromEnum<TrainPreset>(_uiStrings, 0);
            LoadModels();
        }

        private async void DreamboothForm_Shown(object sender, EventArgs e)
        {
            Refresh();

            TabOrderInit(new List<Control>() {
                comboxBaseModel, comboxTrainPreset, textboxTrainImgsDir, textboxClassName, sliderLrMultiplier, sliderSteps
            });

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

                int cudaDeviceOpt = Config.Get<int>(Config.Keys.CudaDeviceIdx);

                if (valid && cudaDeviceOpt == (int)Enums.Cuda.Device.Cpu)
                {
                    UiUtils.ShowMessageBox("DreamBooth training is not supported on CPU.", UiUtils.MessageType.Error);
                    valid = false;
                }

                Gpu gpu = null;
                
                if(gpus != null && gpus.Count > 0 && cudaDeviceOpt != (int)Enums.Cuda.Device.Cpu)
                    gpu = (cudaDeviceOpt == (int)Enums.Cuda.Device.Automatic) ? gpus[0] : gpus[cudaDeviceOpt - 2];

                if (valid && gpu.VramGb < 23f)
                {
                    DialogResult result = UiUtils.ShowMessageBox($"Your GPU appears to have {gpu.VramGb} GB VRAM, but 24 GB are currently required for DreamBooth " +
                        $"training.\n\nContinue anyway?", "VRAM Warning", MessageBoxButtons.YesNo);
                    
                    if(result == DialogResult.No)
                        valid = false;
                }
                else if (valid && gpu.VramGb < 25f)
                {
                    UiUtils.ShowMessageBox($"Your GPU appears to have {gpu.VramGb} GB VRAM.\nThis is enough to train DreamBooth, but you should make sure that no " +
                        $"other VRAM-consuming applications are running (Games, Browsers, Game Launchers, AI/ML).", UiUtils.MessageType.Message);
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

        private void btnOpenModelFolder_Click(object sender, EventArgs e)
        {
            new ModelFoldersForm(Enums.StableDiffusion.ModelType.Normal).ShowDialogForm();
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

            Model baseModel = Paths.GetModel(comboxBaseModel.Text);

            if (baseModel == null)
            {
                UiUtils.ShowMessageBox("Invalid model selection.");
                return;
            }

            Program.SetState(Program.BusyState.Dreambooth);
            btnStart.Text = "Cancel";

            ZlpDirectoryInfo trainImgDir = new ZlpDirectoryInfo(textboxTrainImgsDir.Text.Trim());
            string className = string.Join("_", textboxClassName.Text.Trim().Split(Path.GetInvalidFileNameChars())).Trunc(50, false);
            textboxClassName.Text = className;
            TrainPreset preset = (TrainPreset)comboxTrainPreset.SelectedIndex;
            float stepsMultiplier = sliderSteps.ActualValueFloat / Dreambooth.GetStepsAndLoggerIntervalAndLrMultiplier(preset).Item1;

            string outPath = await Dreambooth.RunTraining(baseModel, trainImgDir, className, preset, sliderLrMultiplier.ActualValueFloat, stepsMultiplier);

            Program.SetState(Program.BusyState.Standby);
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

        private void comboxTrainPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            TrainPreset preset = (TrainPreset)comboxTrainPreset.SelectedIndex;
            var presetValues = Dreambooth.GetStepsAndLoggerIntervalAndLrMultiplier(preset);
            int steps = presetValues.Item1;

            sliderSteps.ActualMinimum = 0;
            sliderSteps.ActualMaximum = steps * 2;
            sliderSteps.ActualMinimum = steps / 2;
            sliderSteps.ActualValue = steps;
        }
    }
}
