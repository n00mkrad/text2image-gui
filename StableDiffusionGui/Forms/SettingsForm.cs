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
                comboxClipSkip,
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
                ConfigParser.LoadGuiElement(combox, ref type == Enums.Models.Type.Normal ? ref Config.Instance.Model : ref Config.Instance.ModelVae);

                if (combox.Items.Count > 0 && combox.SelectedIndex == -1)
                    combox.SelectedIndex = 0;
            }

            UpdateComboxStates();
        }

        private async Task LoadGpus()
        {
            if (this.RequiresInvoke(new Func<Task>(LoadGpus)))
                return;

            comboxCudaDevice.Items.Clear();
            comboxCudaDevice.Items.Add("Loading CUDA devices...");
            comboxCudaDevice.SelectedIndex = 0;

            var gpus = await GpuUtils.GetCudaGpusCached();

            comboxCudaDevice.Items.Clear();
            comboxCudaDevice.Items.Add("Automatic");
            comboxCudaDevice.Items.Add("CPU (Experimental, may not work at all)");

            foreach (var g in gpus)
                comboxCudaDevice.Items.Add($"GPU {g.CudaDeviceId} ({g.FullName} - {g.VramGb} GB)");

            ConfigParser.LoadComboxIndex(comboxCudaDevice, ref Config.Instance.CudaDeviceIdx);
        }

        private async Task LoadImplementations()
        {
            if(this.RequiresInvoke(new Func<Task>(LoadImplementations)))
                return;

            comboxImplementation.Items.Clear();
            comboxImplementation.Items.Add("Loading available implementations...");
            comboxImplementation.SelectedIndex = 0;

            var disabledImplementations = new List<Implementation>();

            if (!(await InstallationStatus.HasOnnxAsync(true)))
                disabledImplementations.Add(Implementation.DiffusersOnnx);

            comboxImplementation.FillFromEnum<Implementation>(Strings.Implementation, -1, disabledImplementations);
            comboxImplementation.Text = Strings.Implementation.Get(Config.Instance.Implementation.ToString());
            _initialImplementationLoad = false;
        }

        void LoadSettings()
        {
            ConfigParser.LoadGuiElement(checkboxFullPrecision, ref Config.Instance.FullPrecision);
            ConfigParser.LoadGuiElement(checkboxFolderPerPrompt, ref Config.Instance.FolderPerPrompt);
            ConfigParser.LoadGuiElement(checkboxOutputIgnoreWildcards, ref Config.Instance.FilenameIgnoreWildcards);
            ConfigParser.LoadGuiElement(checkboxFolderPerSession, ref Config.Instance.FolderPerSession);
            ConfigParser.LoadGuiElement(checkboxAdvancedMode, ref Config.Instance.AdvancedUi);
            ConfigParser.LoadGuiElement(checkboxMultiPromptsSameSeed, ref Config.Instance.MultiPromptsSameSeed);
            ConfigParser.LoadComboxIndex(comboxTimestampInFilename, ref Config.Instance.FilenameTimestampMode);
            ConfigParser.LoadGuiElement(checkboxPromptInFilename, ref Config.Instance.PromptInFilename);
            ConfigParser.LoadGuiElement(checkboxSeedInFilename, ref Config.Instance.SeedInFilename);
            ConfigParser.LoadGuiElement(checkboxScaleInFilename, ref Config.Instance.ScaleInFilename);
            ConfigParser.LoadGuiElement(checkboxSamplerInFilename, ref Config.Instance.SamplerInFilename);
            ConfigParser.LoadGuiElement(checkboxModelInFilename, ref Config.Instance.ModelInFilename);
            ConfigParser.LoadGuiElement(textboxOutPath, ref Config.Instance.OutPath);
            ConfigParser.LoadGuiElement(textboxFavsPath, ref Config.Instance.FavsPath);
            //ConfigParser.LoadGuiElement(comboxSdModel, ref Config.Instance.Model);
            ConfigParser.LoadGuiElement(comboxSdModelVae, ref Config.Instance.ModelVae);
            // ConfigParser.LoadComboxIndex(comboxCudaDevice);
            ConfigParser.LoadComboxIndex(comboxClipSkip, ref Config.Instance.ClipSkip);
            ConfigParser.LoadComboxIndex(comboxNotify, ref Config.Instance.NotifyModeIdx);
            ConfigParser.LoadGuiElement(checkboxSaveUnprocessedImages, ref Config.Instance.SaveUnprocessedImages);
            ConfigParser.LoadGuiElement(checkboxUnloadModel, ref Config.Instance.UnloadModel);
            ConfigParser.LoadGuiElement(checkboxAutoSetResForInitImg, ref Config.Instance.AutoSetResForInitImg);
            ConfigParser.LoadGuiElement(checkboxInitImageRetainAspectRatio, ref Config.Instance.InitImageRetainAspectRatio);
        }

        void SaveSettings()
        {
            textboxOutPath.Text = textboxOutPath.Text.Replace(@"\", "/");
            textboxFavsPath.Text = textboxFavsPath.Text.Replace(@"\", "/");

            if (!comboxImplementation.Text.StartsWith("Loading")) Config.Instance.Implementation = ParseUtils.GetEnum<Implementation>(comboxImplementation.Text, true, Strings.Implementation);
            ConfigParser.SaveGuiElement(checkboxFullPrecision, ref Config.Instance.FullPrecision);
            ConfigParser.SaveGuiElement(checkboxFolderPerPrompt, ref Config.Instance.FolderPerPrompt);
            ConfigParser.SaveGuiElement(checkboxOutputIgnoreWildcards, ref Config.Instance.FilenameIgnoreWildcards);
            ConfigParser.SaveGuiElement(checkboxFolderPerSession, ref Config.Instance.FolderPerSession);
            ConfigParser.SaveGuiElement(checkboxAdvancedMode, ref Config.Instance.AdvancedUi);
            ConfigParser.SaveGuiElement(checkboxMultiPromptsSameSeed, ref Config.Instance.MultiPromptsSameSeed);
            ConfigParser.SaveComboxIndex(comboxTimestampInFilename, ref Config.Instance.FilenameTimestampMode);
            ConfigParser.SaveGuiElement(checkboxPromptInFilename, ref Config.Instance.PromptInFilename);
            ConfigParser.SaveGuiElement(checkboxSeedInFilename, ref Config.Instance.SeedInFilename);
            ConfigParser.SaveGuiElement(checkboxScaleInFilename, ref Config.Instance.ScaleInFilename);
            ConfigParser.SaveGuiElement(checkboxSamplerInFilename, ref Config.Instance.SamplerInFilename);
            ConfigParser.SaveGuiElement(checkboxModelInFilename, ref Config.Instance.ModelInFilename);
            ConfigParser.SaveGuiElement(textboxOutPath, ref Config.Instance.OutPath);
            ConfigParser.SaveGuiElement(textboxFavsPath, ref Config.Instance.FavsPath);
            if (!string.IsNullOrWhiteSpace(comboxSdModel.Text)) ConfigParser.SaveGuiElement(comboxSdModel, ref Config.Instance.Model);
            if (!string.IsNullOrWhiteSpace(comboxSdModelVae.Text)) ConfigParser.SaveGuiElement(comboxSdModelVae, ref Config.Instance.ModelVae);
            if (!comboxCudaDevice.Text.StartsWith("Loading")) ConfigParser.SaveComboxIndex(comboxCudaDevice, ref Config.Instance.CudaDeviceIdx);
            ConfigParser.SaveComboxIndex(comboxClipSkip, ref Config.Instance.ClipSkip);
            ConfigParser.SaveComboxIndex(comboxNotify, ref Config.Instance.NotifyModeIdx);
            ConfigParser.SaveGuiElement(checkboxSaveUnprocessedImages, ref Config.Instance.SaveUnprocessedImages);
            ConfigParser.SaveGuiElement(checkboxUnloadModel, ref Config.Instance.UnloadModel);
            ConfigParser.SaveGuiElement(checkboxAutoSetResForInitImg, ref Config.Instance.AutoSetResForInitImg);
            ConfigParser.SaveGuiElement(checkboxInitImageRetainAspectRatio, ref Config.Instance.InitImageRetainAspectRatio);

            Config.Save();
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
                Config.Instance.Implementation = CurrImplementation;
                panelFullPrecision.SetVisible(CurrImplementation.Supports(ImplementationInfo.Feature.HalfPrecisionToggle));
                panelUnloadModel.SetVisible(CurrImplementation.Supports(ImplementationInfo.Feature.InteractiveCli));
                panelCudaDevice.SetVisible(CurrImplementation.Supports(ImplementationInfo.Feature.DeviceSelection));
                panelSdModel.SetVisible(CurrImplementation.Supports(ImplementationInfo.Feature.CustomModels));
                panelVae.SetVisible(CurrImplementation.Supports(ImplementationInfo.Feature.CustomVae));
                panelAdvancedOptsInvoke.SetVisible(CurrImplementation == Implementation.InvokeAi);

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
