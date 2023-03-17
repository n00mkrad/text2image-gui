using Microsoft.WindowsAPICodePack.Dialogs;
using StableDiffusionGui.Data;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
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
        private Implementation CurrImplementation { get { return ParseUtils.GetEnum<Implementation>(comboxImplementation.Text, true, Strings.Implementation); } }

        private bool _initialImplementationLoad = true; // If true, suppress the compat warning, this is to avoid showing it when opening the form
        private bool _ready = false; // Wait for loading settings etc. before allowing to close the form & save settings

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
            comboxTimestampInFilename.FillFromEnum<Enums.Export.FilenameTimestamp>(Strings.TimestampModes);
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
                checkboxAutoSetResForInitImg,
                checkboxInitImageRetainAspectRatio,
                checkboxAdvancedMode,
                comboxNotify
            }, -1);

            Task.Run(() => LoadGpus());
            Task.Run(() => LoadImplementations());
            UpdateComboxStates();
            Opacity = 1;
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
            Program.MainForm.TryRefreshUiState();
        }

        private void LoadModels()
        {
            if (CurrImplementation < 0)
                return;

            List<Model> models = Models.GetModelsAll().Where(m => CurrImplementation.GetInfo().SupportedModelFormats.Contains(m.Format)).ToList();
            var types = new List<Enums.Models.Type>() { Enums.Models.Type.Normal, Enums.Models.Type.Vae };

            foreach(var type in types)
            {
                var combox = type == Enums.Models.Type.Normal ? comboxSdModel : comboxSdModelVae;
                combox.Items.Clear();

                if (type == Enums.Models.Type.Vae)
                    combox.Items.Add("None");

                models.Where(m => m.Type == type).ToList().ForEach(m => combox.Items.Add(m.Name));
                ConfigParser.LoadGuiElement(combox, type == Enums.Models.Type.Normal ? Config.Keys.Model : Config.Keys.ModelVae);

                if (combox.Items.Count > 0 && combox.SelectedIndex == -1)
                    combox.SelectedIndex = 0;
            }

            UpdateComboxStates();
        }

        private async Task LoadGpus()
        {
            comboxCudaDevice.Items.Clear();
            comboxCudaDevice.Items.Add("Loading CUDA devices...");
            comboxCudaDevice.SelectedIndex = 0;

            var gpus = await GpuUtils.GetCudaGpusCached();

            comboxCudaDevice.Items.Clear();
            comboxCudaDevice.Items.Add("Automatic");
            comboxCudaDevice.Items.Add("CPU (Experimental, may not work at all)");

            foreach (var g in gpus)
                comboxCudaDevice.Items.Add($"GPU {g.CudaDeviceId} ({g.FullName} - {g.VramGb} GB)");

            ConfigParser.LoadComboxIndex(comboxCudaDevice, Config.Keys.CudaDeviceIdx);
        }

        private async Task LoadImplementations()
        {
            comboxImplementation.Items.Clear();
            comboxImplementation.Items.Add("Loading available implementations...");
            comboxImplementation.SelectedIndex = 0;

            var disabledImplementations = new List<Implementation>();

            if (!(await InstallationStatus.HasOnnxAsync(true)))
                disabledImplementations.Add(Implementation.DiffusersOnnx);

            comboxImplementation.FillFromEnum<Implementation>(Strings.Implementation, -1, disabledImplementations);
            comboxImplementation.Text = Strings.Implementation.Get(Config.Get<string>(Config.Keys.ImplementationName));
            // ConfigParser.LoadComboxIndex(comboxImplementation, Config.Keys.ImplementationIdx);
            _initialImplementationLoad = false;
        }

        void LoadSettings()
        {
            //ConfigParser.LoadComboxIndex(comboxImplementation, Config.Keys.ImplementationIdx);
            ConfigParser.LoadGuiElement(checkboxFullPrecision, Config.Keys.FullPrecision);
            ConfigParser.LoadGuiElement(checkboxFolderPerPrompt, Config.Keys.FolderPerPrompt);
            ConfigParser.LoadGuiElement(checkboxOutputIgnoreWildcards, Config.Keys.FilenameIgnoreWildcards);
            ConfigParser.LoadGuiElement(checkboxFolderPerSession, Config.Keys.FolderPerSession);
            ConfigParser.LoadGuiElement(checkboxAdvancedMode, Config.Keys.AdvancedUi);
            ConfigParser.LoadGuiElement(checkboxMultiPromptsSameSeed, Config.Keys.MultiPromptsSameSeed);
            ConfigParser.LoadComboxIndex(comboxTimestampInFilename, Config.Keys.FilenameTimestampMode);
            ConfigParser.LoadGuiElement(checkboxPromptInFilename, Config.Keys.PromptInFilename);
            ConfigParser.LoadGuiElement(checkboxSeedInFilename, Config.Keys.SeedInFilename);
            ConfigParser.LoadGuiElement(checkboxScaleInFilename, Config.Keys.ScaleInFilename);
            ConfigParser.LoadGuiElement(checkboxSamplerInFilename, Config.Keys.SamplerInFilename);
            ConfigParser.LoadGuiElement(checkboxModelInFilename, Config.Keys.ModelInFilename);
            ConfigParser.LoadGuiElement(textboxOutPath, Config.Keys.OutPath);
            ConfigParser.LoadGuiElement(textboxFavsPath, Config.Keys.FavsPath);
            //ConfigParser.LoadGuiElement(comboxSdModel, Config.Keys.Model);
            ConfigParser.LoadGuiElement(comboxSdModelVae, Config.Keys.ModelVae);
            // ConfigParser.LoadComboxIndex(comboxCudaDevice);
            ConfigParser.LoadComboxIndex(comboxNotify, Config.Keys.NotifyModeIdx);
            ConfigParser.LoadGuiElement(checkboxSaveUnprocessedImages, Config.Keys.SaveUnprocessedImages);
            ConfigParser.LoadGuiElement(checkboxUnloadModel, Config.Keys.UnloadModel);
            ConfigParser.LoadGuiElement(checkboxAutoSetResForInitImg, Config.Keys.AutoSetResForInitImg);
            ConfigParser.LoadGuiElement(checkboxInitImageRetainAspectRatio, Config.Keys.InitImageRetainAspectRatio);
        }

        void SaveSettings()
        {
            textboxOutPath.Text = textboxOutPath.Text.Replace(@"\", "/");
            textboxFavsPath.Text = textboxFavsPath.Text.Replace(@"\", "/");

            if (!comboxImplementation.Text.StartsWith("Loading")) Config.Set(Config.Keys.ImplementationName, Strings.Implementation.GetReverse(comboxImplementation.Text));
            ConfigParser.SaveGuiElement(checkboxFullPrecision, Config.Keys.FullPrecision);
            ConfigParser.SaveGuiElement(checkboxFolderPerPrompt, Config.Keys.FolderPerPrompt);
            ConfigParser.SaveGuiElement(checkboxOutputIgnoreWildcards, Config.Keys.FilenameIgnoreWildcards);
            ConfigParser.SaveGuiElement(checkboxFolderPerSession, Config.Keys.FolderPerSession);
            ConfigParser.SaveGuiElement(checkboxAdvancedMode, Config.Keys.AdvancedUi);
            ConfigParser.SaveGuiElement(checkboxMultiPromptsSameSeed, Config.Keys.MultiPromptsSameSeed);
            ConfigParser.SaveComboxIndex(comboxTimestampInFilename, Config.Keys.FilenameTimestampMode);
            ConfigParser.SaveGuiElement(checkboxPromptInFilename, Config.Keys.PromptInFilename);
            ConfigParser.SaveGuiElement(checkboxSeedInFilename, Config.Keys.SeedInFilename);
            ConfigParser.SaveGuiElement(checkboxScaleInFilename, Config.Keys.ScaleInFilename);
            ConfigParser.SaveGuiElement(checkboxSamplerInFilename, Config.Keys.SamplerInFilename);
            ConfigParser.SaveGuiElement(checkboxModelInFilename, Config.Keys.ModelInFilename);
            ConfigParser.SaveGuiElement(textboxOutPath, Config.Keys.OutPath);
            ConfigParser.SaveGuiElement(textboxFavsPath, Config.Keys.FavsPath);
            if (!string.IsNullOrWhiteSpace(comboxSdModel.Text)) ConfigParser.SaveGuiElement(comboxSdModel, Config.Keys.Model);
            if (!string.IsNullOrWhiteSpace(comboxSdModelVae.Text)) ConfigParser.SaveGuiElement(comboxSdModelVae, Config.Keys.ModelVae);
            if (!comboxCudaDevice.Text.StartsWith("Loading")) ConfigParser.SaveComboxIndex(comboxCudaDevice, Config.Keys.CudaDeviceIdx);
            ConfigParser.SaveComboxIndex(comboxNotify, Config.Keys.NotifyModeIdx);
            ConfigParser.SaveGuiElement(checkboxSaveUnprocessedImages, Config.Keys.SaveUnprocessedImages);
            ConfigParser.SaveGuiElement(checkboxUnloadModel, Config.Keys.UnloadModel);
            ConfigParser.SaveGuiElement(checkboxAutoSetResForInitImg, Config.Keys.AutoSetResForInitImg);
            ConfigParser.SaveGuiElement(checkboxInitImageRetainAspectRatio, Config.Keys.InitImageRetainAspectRatio);
        }

        private void btnOutPathBrowse_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { InitialDirectory = textboxOutPath.Text, IsFolderPicker = true };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                textboxOutPath.Text = dialog.FileName;
        }

        private void btnOpenModelsFolder_Click(object sender, EventArgs e)
        {
            SetupModelDirs();
        }

        private void btnOpenModelsFolderVae_Click(object sender, EventArgs e)
        {
            SetupModelDirs();
        }

        private void SetupModelDirs ()
        {
            new ModelFoldersForm().ShowDialogForm();
            LoadModels();
        }

        private void btnRefreshModelsDropdown_Click(object sender, EventArgs e)
        {
            LoadModels();
        }

        private void btnRefreshModelsDropdownVae_Click(object sender, EventArgs e)
        {
            LoadModels();
        }

        private void btnFavsPathBrowse_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { InitialDirectory = textboxFavsPath.Text, IsFolderPicker = true };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                textboxFavsPath.Text = dialog.FileName;
        }

        private void comboxImplementation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrImplementation < 0)
                return;

            this.StopRendering();

            try
            {
                Config.Set(Config.Keys.ImplementationName, CurrImplementation.ToString());
                panelFullPrecision.SetVisible(CurrImplementation.Supports(ImplementationInfo.Feature.HalfPrecisionToggle));
                panelUnloadModel.SetVisible(CurrImplementation.Supports(ImplementationInfo.Feature.InteractiveCli));
                panelCudaDevice.SetVisible(CurrImplementation.Supports(ImplementationInfo.Feature.DeviceSelection));
                panelSdModel.SetVisible(CurrImplementation.Supports(ImplementationInfo.Feature.CustomModels));
                panelVae.SetVisible(CurrImplementation.Supports(ImplementationInfo.Feature.CustomVae));

                LoadModels();

                if (_ready && CurrImplementation != Implementation.InvokeAi)
                {
                    if (_initialImplementationLoad)
                    {
                        _initialImplementationLoad = false;
                        return; // Supress once, as we only want to show this if the user selects it, not if it's loaded from config
                    }

                    UiUtils.ShowMessageBox($"Warning: This implementation disables several features.\nOnly use it if you need it due to compatibility or hardware limitations.");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            this.ResumeRendering();
        }

        private void UpdateComboxStates()
        {
            comboxSdModel.Enabled = comboxSdModel.Items.Count > 0;
            comboxSdModelVae.Enabled = comboxSdModelVae.Items.Count > 0;
        }
    }
}
