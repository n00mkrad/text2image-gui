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
            comboxImplementation.FillFromEnum<Implementation>(Strings.Implementation, -1, EnabledFeatures.DisabledImplementations);

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
                ConfigParser.LoadGuiElement(combox);

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

            ConfigParser.LoadComboxIndex(comboxCudaDevice);
            _ready = true;
        }

        void LoadSettings()
        {
            ConfigParser.LoadComboxIndex(comboxImplementation);
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
            ConfigParser.SaveComboxIndex(comboxImplementation);
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
            new ModelFoldersForm(ModelType.Normal).ShowDialogForm();
            LoadModels(true, ModelType.Normal);
        }

        private void btnOpenModelsFolderVae_Click(object sender, EventArgs e)
        {
            new ModelFoldersForm(ModelType.Vae).ShowDialogForm();
            LoadModels(true, ModelType.Vae);
        }

        private void checkboxOptimizedSd_CheckedChanged(object sender, EventArgs e)
        {
            if (_ready && CurrImplementation == Implementation.OptimizedSd)
                UiUtils.ShowMessageBox($"Warning: Low Memory Mode disables several features, such as custom samplers or seamless mode.\n" +
            $"Only keep this option enabled if your GPU has less than 6 GB of memory.");
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
        }
    }
}
