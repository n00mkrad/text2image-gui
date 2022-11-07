using Microsoft.WindowsAPICodePack.Dialogs;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class SettingsForm : CustomForm
    {
        private bool _ready = false;

        public SettingsForm()
        {
            InitializeComponent();
            Opacity = 0;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            _ready = false;

            MinimumSize = Size;
            MaximumSize = new System.Drawing.Size(Size.Width, (Size.Height * 1.25f).RoundToInt());
        }

        private void SettingsForm_Shown(object sender, EventArgs e)
        {
            Refresh();
            LoadModels(false, Enums.StableDiffusion.ModelType.Normal);
            LoadModels(false, Enums.StableDiffusion.ModelType.Vae);
            LoadSettings();

            TabOrderInit(new List<Control>() {
                checkboxOptimizedSd,
                checkboxFullPrecision,
                checkboxUnloadModel,
                comboxSdModel, btnRefreshModelsDropdown, btnOpenModelsFolder,
                comboxSdModelVae, btnRefreshModelsDropdownVae, btnOpenModelsFolderVae,
                comboxCudaDevice,
                textboxOutPath, btnOutPathBrowse,
                checkboxFolderPerPrompt, checkboxOutputIgnoreWildcards, checkboxFolderPerSession,
                checkboxPromptInFilename, checkboxSeedInFilename, checkboxScaleInFilename, checkboxSamplerInFilename, checkboxModelInFilename,
                textboxFavsPath, btnFavsPathBrowse,
                checkboxMultiPromptsSameSeed,
                checkboxSaveUnprocessedImages,
                checkboxAdvancedMode,
                comboxNotify
            });

            Task.Run(() => LoadGpus());
            Opacity = 1;
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_ready)
            {
                e.Cancel = true;
                return;
            }

            SaveSettings();
            Program.MainForm.RefreshAfterSettingsChanged();
        }

        private void LoadModels(bool loadCombox, Enums.StableDiffusion.ModelType type)
        {
            var combox = type == Enums.StableDiffusion.ModelType.Normal ? comboxSdModel : comboxSdModelVae;

            combox.Items.Clear();

            if (type == Enums.StableDiffusion.ModelType.Vae)
                combox.Items.Add("None");

            Paths.GetModels(type).ForEach(x => combox.Items.Add(x.Name));

            if (loadCombox)
                ConfigParser.LoadGuiElement(combox);
        }

        private async Task LoadGpus()
        {
            comboxCudaDevice.Items.Clear();
            comboxCudaDevice.Items.Add("Loading...");
            comboxCudaDevice.SelectedIndex = 0;

            var gpus = await GpuUtils.GetCudaGpusCached();

            comboxCudaDevice.Items.Clear();
            comboxCudaDevice.Items.Add("Automatic");
            comboxCudaDevice.Items.Add("CPU (Experimental, may not work at all)");

            foreach (var g in gpus)
                comboxCudaDevice.Items.Add($"GPU {g.CudaDeviceId} ({g.FullName} - {g.VramGb} GB)");

            ConfigParser.LoadComboxIndex(comboxCudaDevice);
            _ready = true;
        }

        void LoadSettings()
        {
            ConfigParser.LoadGuiElement(checkboxOptimizedSd);
            ConfigParser.LoadGuiElement(checkboxFullPrecision);
            ConfigParser.LoadGuiElement(checkboxFolderPerPrompt);
            ConfigParser.LoadGuiElement(checkboxOutputIgnoreWildcards);
            ConfigParser.LoadGuiElement(checkboxFolderPerSession);
            ConfigParser.LoadGuiElement(checkboxAdvancedMode);
            ConfigParser.LoadGuiElement(checkboxMultiPromptsSameSeed);
            ConfigParser.LoadGuiElement(checkboxPromptInFilename);
            ConfigParser.LoadGuiElement(checkboxSeedInFilename);
            ConfigParser.LoadGuiElement(checkboxScaleInFilename);
            ConfigParser.LoadGuiElement(checkboxSamplerInFilename);
            ConfigParser.LoadGuiElement(checkboxModelInFilename);
            ConfigParser.LoadGuiElement(textboxOutPath);
            ConfigParser.LoadGuiElement(textboxFavsPath);
            ConfigParser.LoadGuiElement(comboxSdModel);
            ConfigParser.LoadGuiElement(comboxSdModelVae);
            // ConfigParser.LoadComboxIndex(comboxCudaDevice);
            ConfigParser.LoadComboxIndex(comboxNotify);
            ConfigParser.LoadGuiElement(checkboxSaveUnprocessedImages);
            ConfigParser.LoadGuiElement(checkboxUnloadModel);
        }

        void SaveSettings()
        {
            ConfigParser.SaveGuiElement(checkboxOptimizedSd);
            ConfigParser.SaveGuiElement(checkboxFullPrecision);
            ConfigParser.SaveGuiElement(checkboxFolderPerPrompt);
            ConfigParser.SaveGuiElement(checkboxOutputIgnoreWildcards);
            ConfigParser.SaveGuiElement(checkboxFolderPerSession);
            ConfigParser.SaveGuiElement(checkboxAdvancedMode);
            ConfigParser.SaveGuiElement(checkboxMultiPromptsSameSeed);
            ConfigParser.SaveGuiElement(checkboxPromptInFilename);
            ConfigParser.SaveGuiElement(checkboxSeedInFilename);
            ConfigParser.SaveGuiElement(checkboxScaleInFilename);
            ConfigParser.SaveGuiElement(checkboxSamplerInFilename);
            ConfigParser.SaveGuiElement(checkboxModelInFilename);
            ConfigParser.SaveGuiElement(textboxOutPath);
            ConfigParser.SaveGuiElement(textboxFavsPath);
            ConfigParser.SaveGuiElement(comboxSdModel);
            ConfigParser.SaveGuiElement(comboxSdModelVae);
            ConfigParser.SaveComboxIndex(comboxCudaDevice);
            ConfigParser.SaveComboxIndex(comboxNotify);
            ConfigParser.SaveGuiElement(checkboxSaveUnprocessedImages);
            ConfigParser.SaveGuiElement(checkboxUnloadModel);
        }

        private void btnOutPathBrowse_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { InitialDirectory = textboxOutPath.Text, IsFolderPicker = true };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                textboxOutPath.Text = dialog.FileName;
        }

        private void btnOpenModelsFolder_Click(object sender, EventArgs e)
        {
            new ModelFoldersForm(Enums.StableDiffusion.ModelType.Normal).ShowDialogForm();
            LoadModels(true, Enums.StableDiffusion.ModelType.Normal);
        }

        private void btnOpenModelsFolderVae_Click(object sender, EventArgs e)
        {
            new ModelFoldersForm(Enums.StableDiffusion.ModelType.Vae).ShowDialogForm();
            LoadModels(true, Enums.StableDiffusion.ModelType.Vae);
        }

        private void checkboxOptimizedSd_CheckedChanged(object sender, EventArgs e)
        {
            if (_ready && checkboxOptimizedSd.Checked)
                UiUtils.ShowMessageBox($"Warning: Low Memory Mode disables several features, such as custom samplers or seamless mode.\n" +
            $"Only keep this option enabled if your GPU has less than 6 GB of memory.");
        }

        private void btnRefreshModelsDropdown_Click(object sender, EventArgs e)
        {
            LoadModels(true, Enums.StableDiffusion.ModelType.Normal);
        }

        private void btnRefreshModelsDropdownVae_Click(object sender, EventArgs e)
        {
            LoadModels(true, Enums.StableDiffusion.ModelType.Vae);
        }

        private void btnFavsPathBrowse_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { InitialDirectory = textboxFavsPath.Text, IsFolderPicker = true };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                textboxFavsPath.Text = dialog.FileName;
        }
    }
}
