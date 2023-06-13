using Microsoft.WindowsAPICodePack.Dialogs;
using StableDiffusionGui.Data;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Training;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZetaLongPaths;
using static StableDiffusionGui.Main.Enums.Training;

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

            comboxNetworkSize.FillFromEnum<LoraSizes>(Strings.LoraSizes, 2);
            comboxRes.SelectedIndex = 1;
            LoadModels();
        }

        private async void DreamboothForm_Shown(object sender, EventArgs e)
        {
            Refresh();

            TabOrderInit(new List<Control>() {
                comboxBaseModel, comboxNetworkSize, textboxTrainImgsDir, textboxClassName, sliderLr, sliderSteps
            });

            await PerformChecks();
        }

        private async Task PerformChecks()
        {
            try
            {
                bool valid = true;
                var gpus = await GpuUtils.GetCudaGpusCached();

                Text = "LoRA Training";

                if (valid && gpus.Count < 1)
                {
                    UiUtils.ShowMessageBox("No compatible GPU detected.", UiUtils.MessageType.Error);
                    valid = false;
                }

                int cudaDeviceOpt = Config.Instance.CudaDeviceIdx;

                if (valid && cudaDeviceOpt == (int)Enums.Cuda.Device.Cpu)
                {
                    UiUtils.ShowMessageBox("LoRA training is not supported on CPU.", UiUtils.MessageType.Error);
                    valid = false;
                }

                Gpu gpu = null;

                if (gpus != null && gpus.Count > 0 && cudaDeviceOpt != (int)Enums.Cuda.Device.Cpu)
                    gpu = (cudaDeviceOpt == (int)Enums.Cuda.Device.Automatic) ? gpus[0] : gpus[cudaDeviceOpt - 2];

                if (valid && gpu.VramGb < 7.5f)
                {
                    DialogResult result = UiUtils.ShowMessageBox($"Your GPU appears to have {gpu.VramGb} GB VRAM, but 8 GB are currently required for LoRA " +
                        $"training.\n\nContinue anyway?", "VRAM Warning", MessageBoxButtons.YesNo);

                    if (result == DialogResult.No)
                        valid = false;
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
            new ModelFoldersForm(ModelFoldersForm.Folder.Models).ShowDialogForm();
            LoadModels();
        }

        private void LoadModels()
        {
            comboxBaseModel.Items.Clear();
            Models.GetModels().Where(m => m.Type == Enums.Models.Type.Normal).ToList().ForEach(x => comboxBaseModel.Items.Add(x.Name));

            if (comboxBaseModel.SelectedIndex < 0 && comboxBaseModel.Items.Count > 0)
                comboxBaseModel.SelectedIndex = 0;
        }

        private async void btnRun_Click(object sender, EventArgs e)
        {
            if (Program.State == Program.BusyState.Training)
            {
                ProcessManager.FindAndKillOrphans($"*{Constants.Dirs.Dreambooth}*.py*{Paths.SessionTimestamp}*");
                return;
            }

            if (Program.Busy)
            {
                UiUtils.ShowMessageBox("Please wait until the current process has finished.");
                return;
            }

            Model baseModel = Models.GetModel(comboxBaseModel.Text);

            if (baseModel == null)
            {
                UiUtils.ShowMessageBox("Invalid model selection.");
                return;
            }

            if (!Directory.Exists(textboxTrainImgsDir.Text.Trim()))
            {
                UiUtils.ShowMessageBox("Invalid training directory.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textboxProjName.Text.Trim()))
            {
                UiUtils.ShowMessageBox("Please enter a name.");
                return;
            }

            Program.SetState(Program.BusyState.Training);
            btnStart.Text = "Cancel";

            ZlpDirectoryInfo trainImgDir = new ZlpDirectoryInfo(textboxTrainImgsDir.Text.Trim());
            string trigger = string.Join("_", textboxClassName.Text.Trim().Split(Path.GetInvalidFileNameChars())).Trunc(75, false);
            string name = string.Join("_", textboxProjName.Text.Trim().Split(Path.GetInvalidFileNameChars())).Trunc(50, false);
            textboxClassName.Text = trigger;
            //TrainPreset preset = (TrainPreset)comboxNetworkSize.SelectedIndex;
            // float stepsMultiplier = sliderSteps.ActualValueFloat / Training.GetStepsAndLoggerIntervalAndLrMultiplier(preset).Item1;

            // string outPath = await Dreambooth.TrainDreamboothLegacy(baseModel, trainImgDir, className, preset, sliderLr.ActualValueFloat, stepsMultiplier);

            KohyaSettings settings = new KohyaSettings(KohyaSettings.NetworkType.LoHa)
            {
                Steps = sliderSteps.ActualValueInt,
                LearningRate = sliderLr.ActualValueFloat,
                Resolution = comboxRes.Text.Split(" ").First().GetInt(),
            };

            var size = ParseUtils.GetEnum<LoraSizes>(comboxNetworkSize.Text, true, Strings.LoraSizes);
            if (size == LoraSizes.Tiny) { settings.NetworkDim = 2; settings.ConvDim = 1; }
            if (size == LoraSizes.Small) { settings.NetworkDim = 4; settings.ConvDim = 2; }
            if (size == LoraSizes.Normal) { settings.NetworkDim = 8; settings.ConvDim = 4; }
            if (size == LoraSizes.Big) { settings.NetworkDim = 16; settings.ConvDim = 8; }

            string outPath = await Training.KohyaTraining.TrainLora(baseModel, trainImgDir, name, trigger, settings, true);

            Program.SetState(Program.BusyState.Standby);
            btnStart.Text = "Start Training";

            Console.WriteLine($"checking:\n{outPath}");

            if (File.Exists(outPath))
                Logger.Log($"Done. Saved trained model to:\n{outPath.Replace(Paths.GetModelsPath(), "Models")}");
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
            if (Program.State == Program.BusyState.Training)
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
            System.Diagnostics.Process.Start("https://github.com/n00mkrad/text2image-gui/blob/main/docs/DreamBooth.md");
        }
    }
}
