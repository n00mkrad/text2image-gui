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
        public DreamboothForm()
        {
            InitializeComponent();
        }

        private void DreamboothForm_Load(object sender, EventArgs e)
        {
            comboxNetworkSize.FillFromEnum<LoraSize>(Strings.LoraSizes, 2);
            comboxTrainMethod.FillFromEnum<KohyaSettings.NetworkType>(Strings.LoraNetworkTypes, 0, KohyaSettings.NetworkType.LoCon.AsList());
            comboxCaptions.FillFromEnum<CaptionMode>(Strings.CaptionModes, 1);
            comboxRes.SelectedIndex = 1;
            comboxSaveFormat.SelectedIndex = 0;
            comboxTrainFormat.SelectedIndex = 0;
            LoadModels();

            if (Config.Instance.LastTrainingBaseModel.IsEmpty())
            {
                var matchingModels = comboxBaseModel.Items.Cast<object>().Select(o => o.ToString()).Where(m => m.Lower().StartsWith("animefull"));

                if (matchingModels.Any())
                    comboxBaseModel.Text = matchingModels.First();
            }
        }

        private async void DreamboothForm_Shown(object sender, EventArgs e)
        {
            Refresh();

            TabOrderInit(new List<Control>() {
                comboxBaseModel,
                comboxNetworkSize, upDownClipSkip,
                textboxProjName,
                textboxTrainImgsDir,
                textboxTrainImgsDir, btnTrainImgsBrowse,
                textboxClassName,
                comboxRes, checkboxAspectBuckets,
                sliderLr,
                sliderSteps
            });

            AllowTextboxTab = false;
            sliderSteps.ActualMaximum = Config.IniInstance.LoraMaxSteps;
            LoadControls();
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

                if (valid && gpu.VramGb < 7f)
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
            var models = Models.GetModels().Where(m => m.Type == Enums.Models.Type.Normal).Select(m => m.Name);
            comboxBaseModel.SetItems(models, UiExtensions.SelectMode.Retain, UiExtensions.SelectMode.First);

            if (comboxBaseModel.SelectedIndex < 0 && comboxBaseModel.Items.Count > 0)
                comboxBaseModel.SelectedIndex = 0;
        }

        private async void btnRun_Click(object sender, EventArgs e)
        {
            SaveControls();
            KohyaTraining.Cleanup();

            if (Program.State == Program.BusyState.Training)
            {
                TtiProcess.KillAll();
                btnStart.DisableFor(1000);
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

            TtiProcess.KillAll();
            Program.SetState(Program.BusyState.Training);
            btnStart.DisableFor(2000);
            btnStart.Text = "Cancel";

            ZlpDirectoryInfo trainImgDir = new ZlpDirectoryInfo(textboxTrainImgsDir.Text.Trim());
            textboxProjName.Text = FormatUtils.SanitizePromptFilename(textboxProjName.Text);
            string name = string.Join("_", textboxProjName.Text.Trim().Split(Path.GetInvalidFileNameChars())).Trunc(50, false);

            var trainMethod = ParseUtils.GetEnum<KohyaSettings.NetworkType>(comboxTrainMethod.Text, true, Strings.LoraNetworkTypes);

            KohyaSettings settings = new KohyaSettings(trainMethod)
            {
                BatchSize = comboxBatchSize.GetInt(),
                Steps = sliderSteps.ActualValueInt * (4 / comboxBatchSize.GetInt()), // Compensate for sub-4 batch sizes
                LearningRate = sliderLr.ActualValueFloat / (comboxBatchSize.GetInt() / 4f),
                Resolution = comboxRes.Text.Split(" ").First().GetInt(),
                ClipSkip = (int)upDownClipSkip.Value,
                UseAspectBuckets = checkboxAspectBuckets.Checked,
                GradientCheckpointing = checkboxGradCkpt.Checked,
                TrainMixedPrec = comboxTrainFormat.Text.Split(':').Last().Trim().Lower().Replace("fp32", "no"),
                SaveMixedPrec = comboxSaveFormat.Text.Split(':').Last().Trim().Lower().Replace("fp32", "no"),
                AugmentFlip = checkboxAugFlip.Checked,
                AgumentColor = checkboxAugColor.Checked,
                ShuffleCaption = checkboxShuffleTags.Checked,
                Seed = textboxSeed.GetInt(),
            };

            var size = ParseUtils.GetEnum<LoraSize>(comboxNetworkSize.Text, true, Strings.LoraSizes);
            KohyaTraining.ApplyPreset(ref settings, size);

            var captionMode = ParseUtils.GetEnum<CaptionMode>(comboxCaptions.Text, true, Strings.CaptionModes);
            string trigger = captionMode == CaptionMode.UseTxtFiles ? null : textboxClassName.Visible ? textboxClassName.Text.Trim() : "";
            string outPath = await KohyaTraining.TrainLora(baseModel, trainImgDir, name, trigger, settings, checkboxDebugOpts.Checked);

            Program.SetState(Program.BusyState.Standby);
            btnStart.Text = "Start Training";

            if (File.Exists(outPath))
                Logger.Log($"Done. Saved LoRA model to:\n{outPath.Replace(Paths.GetLorasPath(false), Constants.Dirs.Models.Loras)}");
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
            SaveControls();

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

        private void SaveControls()
        {
            Config.Instance.LastTrainingBaseModel = comboxBaseModel.Text;
            ConfigParser.SaveGuiElement(upDownClipSkip, ref Config.Instance.LastLoraClipSkip);
            ConfigParser.SaveGuiElement(textboxTrainImgsDir, ref Config.Instance.LastLoraDataDir);
            ConfigParser.SaveGuiElement(textboxProjName, ref Config.Instance.LastLoraTrainName);
            ConfigParser.SaveGuiElement(textboxClassName, ref Config.Instance.LastLoraTrainTrigger);
        }

        private void LoadControls()
        {
            if (comboxBaseModel.Items.Cast<object>().Any(item => item.ToString() == Config.Instance.LastTrainingBaseModel))
                comboxBaseModel.Text = Config.Instance.LastTrainingBaseModel;

            ConfigParser.LoadGuiElement(upDownClipSkip, ref Config.Instance.LastLoraClipSkip);
            ConfigParser.LoadGuiElement(textboxProjName, ref Config.Instance.LastLoraTrainName);
            ConfigParser.LoadGuiElement(textboxClassName, ref Config.Instance.LastLoraTrainTrigger);

            if (Directory.Exists(Config.Instance.LastLoraDataDir))
            {
                textboxTrainImgsDir.Text = Config.Instance.LastLoraDataDir;

                if (IoUtils.GetFilesSorted(Config.Instance.LastLoraDataDir, false, "*.txt").Any())
                    comboxCaptions.SetWithEnum(CaptionMode.UseTxtFiles, true, Strings.CaptionModes);
            }
        }

        private void checkboxDebugOpts_CheckedChanged(object sender, EventArgs e)
        {
            flowLayoutPanelDebugOpts.Visible = checkboxDebugOpts.Checked;
        }

        private void comboxCaptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((Action)(() =>
            {
                var selectedMode = ParseUtils.GetEnum<CaptionMode>(comboxCaptions.Text, true, Strings.CaptionModes);
                textboxClassName.Visible = selectedMode == CaptionMode.UseSinglePhrase;
                comboxCaptions.Size = new System.Drawing.Size(selectedMode == CaptionMode.UseSinglePhrase ? 200 : 456, textboxClassName.Size.Height);
            })).RunWithUiStopped(this);
        }
    }
}
