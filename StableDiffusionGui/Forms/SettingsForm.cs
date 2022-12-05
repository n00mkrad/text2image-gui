using Microsoft.WindowsAPICodePack.Dialogs;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Forms
{
    public partial class SettingsForm : CustomForm
    {
        private Implementation CurrImplementation { get { return (Implementation)comboxImplementation.SelectedIndex; } }

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
            comboxImplementation.FillFromEnum<Implementation>(Strings.Implementation, -1);

            LoadSettings();

            TabOrderInit(new List<Control>() {
                comboxImplementation,
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
            Ui.MainForm.FormControls.RefreshUiAfterSettingsChanged();
        }

        private void LoadModels(bool loadCombox, ModelType type)
        {
            var combox = type == ModelType.Normal ? comboxSdModel : comboxSdModelVae;

            combox.Items.Clear();

            if (type == ModelType.Vae)
                combox.Items.Add("None");

            Paths.GetModels(type, CurrImplementation).ForEach(x => combox.Items.Add(x.Name));

            if (loadCombox)
            {
                ConfigParser.LoadGuiElement(combox, type == ModelType.Normal ? Config.Keys.Model : Config.Keys.ModelVae);

                if (combox.Items.Count > 0 && combox.SelectedIndex == -1)
                    combox.SelectedIndex = 0;
            }
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

            ConfigParser.LoadComboxIndex(comboxCudaDevice, Config.Keys.CudaDeviceIdx);
            _ready = true;
        }

        void LoadSettings()
        {
            ConfigParser.LoadComboxIndex(comboxImplementation, Config.Keys.ImplementationIdx);
            ConfigParser.LoadGuiElement(checkboxFullPrecision, Config.Keys.FullPrecision);
            ConfigParser.LoadGuiElement(checkboxFolderPerPrompt, Config.Keys.FolderPerPrompt);
            ConfigParser.LoadGuiElement(checkboxOutputIgnoreWildcards, Config.Keys.FilenameIgnoreWildcards);
            ConfigParser.LoadGuiElement(checkboxFolderPerSession, Config.Keys.FolderPerSession);
            ConfigParser.LoadGuiElement(checkboxAdvancedMode, Config.Keys.AdvancedUi);
            ConfigParser.LoadGuiElement(checkboxMultiPromptsSameSeed, Config.Keys.MultiPromptsSameSeed);
            ConfigParser.LoadGuiElement(checkboxPromptInFilename, Config.Keys.PromptInFilename);
            ConfigParser.LoadGuiElement(checkboxSeedInFilename, Config.Keys.SeedInFilename);
            ConfigParser.LoadGuiElement(checkboxScaleInFilename, Config.Keys.ScaleInFilename);
            ConfigParser.LoadGuiElement(checkboxSamplerInFilename, Config.Keys.SamplerInFilename);
            ConfigParser.LoadGuiElement(checkboxModelInFilename, Config.Keys.ModelInFilename);
            ConfigParser.LoadGuiElement(textboxOutPath, Config.Keys.OutPath);
            ConfigParser.LoadGuiElement(textboxFavsPath, Config.Keys.FavsPath);
            ConfigParser.LoadGuiElement(comboxSdModel, Config.Keys.Model);
            ConfigParser.LoadGuiElement(comboxSdModelVae, Config.Keys.ModelVae);
            // ConfigParser.LoadComboxIndex(comboxCudaDevice);
            ConfigParser.LoadComboxIndex(comboxNotify, Config.Keys.NotifyModeIdx);
            ConfigParser.LoadGuiElement(checkboxSaveUnprocessedImages, Config.Keys.SaveUnprocessedImages);
            ConfigParser.LoadGuiElement(checkboxUnloadModel, Config.Keys.UnloadModel);
        }

        void SaveSettings()
        {
            ConfigParser.SaveComboxIndex(comboxImplementation, Config.Keys.ImplementationIdx);
            ConfigParser.SaveGuiElement(checkboxFullPrecision, Config.Keys.FullPrecision);
            ConfigParser.SaveGuiElement(checkboxFolderPerPrompt, Config.Keys.FolderPerPrompt);
            ConfigParser.SaveGuiElement(checkboxOutputIgnoreWildcards, Config.Keys.FilenameIgnoreWildcards);
            ConfigParser.SaveGuiElement(checkboxFolderPerSession, Config.Keys.FolderPerSession);
            ConfigParser.SaveGuiElement(checkboxAdvancedMode, Config.Keys.AdvancedUi);
            ConfigParser.SaveGuiElement(checkboxMultiPromptsSameSeed, Config.Keys.MultiPromptsSameSeed);
            ConfigParser.SaveGuiElement(checkboxPromptInFilename, Config.Keys.PromptInFilename);
            ConfigParser.SaveGuiElement(checkboxSeedInFilename, Config.Keys.SeedInFilename);
            ConfigParser.SaveGuiElement(checkboxScaleInFilename, Config.Keys.ScaleInFilename);
            ConfigParser.SaveGuiElement(checkboxSamplerInFilename, Config.Keys.SamplerInFilename);
            ConfigParser.SaveGuiElement(checkboxModelInFilename, Config.Keys.ModelInFilename);
            ConfigParser.SaveGuiElement(textboxOutPath, Config.Keys.OutPath);
            ConfigParser.SaveGuiElement(textboxFavsPath, Config.Keys.FavsPath);
            ConfigParser.SaveGuiElement(comboxSdModel, Config.Keys.Model);
            ConfigParser.SaveGuiElement(comboxSdModelVae, Config.Keys.ModelVae);
            ConfigParser.SaveComboxIndex(comboxCudaDevice, Config.Keys.CudaDeviceIdx);
            ConfigParser.SaveComboxIndex(comboxNotify, Config.Keys.NotifyModeIdx);
            ConfigParser.SaveGuiElement(checkboxSaveUnprocessedImages, Config.Keys.SaveUnprocessedImages);
            ConfigParser.SaveGuiElement(checkboxUnloadModel, Config.Keys.UnloadModel);
        }

        private void btnOutPathBrowse_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { InitialDirectory = textboxOutPath.Text, IsFolderPicker = true };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                textboxOutPath.Text = dialog.FileName;
        }

        private void btnOpenModelsFolder_Click(object sender, EventArgs e)
        {
            new ModelFoldersForm(ModelType.Normal).ShowDialogForm();
            LoadModels(true, ModelType.Normal);
        }

        private void btnOpenModelsFolderVae_Click(object sender, EventArgs e)
        {
            new ModelFoldersForm(ModelType.Vae).ShowDialogForm();
            LoadModels(true, ModelType.Vae);
        }

        private void btnRefreshModelsDropdown_Click(object sender, EventArgs e)
        {
            LoadModels(true, ModelType.Normal);
        }

        private void btnRefreshModelsDropdownVae_Click(object sender, EventArgs e)
        {
            LoadModels(true, ModelType.Vae);
        }

        private void btnFavsPathBrowse_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { InitialDirectory = textboxFavsPath.Text, IsFolderPicker = true };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                textboxFavsPath.Text = dialog.FileName;
        }

        private void comboxImplementation_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelFullPrecision.Visible = CurrImplementation != Implementation.DiffusersOnnx;
            panelUnloadModel.Visible = CurrImplementation != Implementation.DiffusersOnnx;
            panelCudaDevice.Visible = CurrImplementation != Implementation.DiffusersOnnx;
            panelVae.Visible = CurrImplementation == Implementation.InvokeAi;

            LoadModels(true, ModelType.Normal);
            LoadModels(true, ModelType.Vae);

            if (_ready && CurrImplementation != Implementation.InvokeAi)
                UiUtils.ShowMessageBox($"Warning: This implementation disables several features.\n" +
            $"Only use it if you need it due to compatibility or hardware limitations.");
        }
    }
}
