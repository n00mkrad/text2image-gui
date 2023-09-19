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
            InitEvents();
        }

        private void InitEvents()
        {
            btnOutPathBrowse.Click += (s, e) => { BrowsePath(textboxOutPath); };
            btnEmbeddingsDirBrowse.Click += (s, e) => { BrowsePath(textboxEmbeddingsDir); };
            btnLorasDirBrowse.Click += (s, e) => { BrowsePath(textboxLorasDir); };
            btnOutPathBrowse.Click += (s, e) => { BrowsePath(textboxOutPath); };
            btnFavsPathBrowse.Click += (s, e) => { BrowsePath(textboxFavsPath); };
        }

        private void SettingsForm_Shown(object sender, EventArgs e)
        {
            Refresh();
            comboxTimestampInFilename.FillFromEnum<Enums.Export.FilenameTimestamp>(Strings.TimestampModes);
            comboxComfyVram.FillFromEnum<Enums.Comfy.VramPreset>(Strings.ComfyVramPresets);
            LoadSettings();

            TabOrderInit(new List<Control>() {
                comboxImplementation,
                checkboxFullPrecision,
                checkboxUnloadModel,
                comboxSdModel, btnRefreshModelsDropdown, btnOpenModelsFolder,
                comboxSdModelVae, btnRefreshModelsDropdownVae, btnOpenModelsFolderVae,
                textboxEmbeddingsDir,
                textboxLorasDir,
                comboxComfyVram,
                comboxClipSkip,
                comboxCudaDevice,
                textboxOutPath, btnOutPathBrowse,
                checkboxFolderPerPrompt, checkboxOutputIgnoreWildcards, checkboxFolderPerSession,
                checkboxPromptInFilename, checkboxSeedInFilename, checkboxScaleInFilename, checkboxSamplerInFilename, checkboxModelInFilename,
                textboxFavsPath, btnFavsPathBrowse,
                checkboxSaveUnprocessedImages,
                checkboxAutoSetResForInitImg,
                checkboxInitImageRetainAspectRatio,
                checkboxAdvancedMode,
                comboxNotify
            }, -1);

            Task.Run(() => LoadGpus());
            LoadImplementations();
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

            ((Action)(() =>
            {
                comboxSdModel.Items.Clear();
                var models = Models.GetModelsAll().Where(m => CurrImplementation.GetInfo().SupportedModelFormats.Contains(m.Format)).ToList();
                models.ForEach(m => comboxSdModel.Items.Add(m.Name));
                ConfigParser.LoadGuiElement(comboxSdModel, ref Config.Instance.Model);
                comboxSdModel.InitCombox();

                comboxUsduModel.Items.Clear();
                GetValidUsduModels(models).ToList().ForEach(m => comboxUsduModel.Items.Add(m.Name));
                ConfigParser.LoadGuiElement(comboxUsduModel, ref Config.Instance.SdUpscaleModel);
                comboxUsduModel.InitCombox();

                UpdateComboxStates();
            })).RunWithUiStopped(this, showErrors: true);
        }

        private IEnumerable<Model> GetValidUsduModels(IEnumerable<Model> models)
        {
            models = models.Where(m => new[] { Enums.Models.Format.Safetensors, Enums.Models.Format.Pytorch }.Contains(m.Format)); // Only ST/CKPT Models
            models = models.Where(m => !(m.Size > 5L * 1024 * 1024 * 1024 && m.Name.Lower().Contains("xl"))); // Roughly filter out XL models based on size/name
            models = models.Where(m => !(m.Size > 4.5 * 1024 * 1024 * 1024 && m.Name.Lower().Contains("768"))); // Roughly filter out SD2V models based on size/name
            models = models.Where(m => !(new[] { "inpaint", "refiner" }.Any(s => m.Name.Lower().Contains(s)))); // Filter out inpainting and refiner models based on name
            return models;
        }

        private void LoadVaes()
        {
            if (CurrImplementation < 0)
                return;

            ((Action)(() =>
            {
                comboxSdModelVae.Items.Clear();
                comboxSdModelVae.Items.Add("None");
                Models.GetVaes().ForEach(m => comboxSdModelVae.Items.Add(m.Name));
                ConfigParser.LoadGuiElement(comboxSdModelVae, ref Config.Instance.ModelVae);
                comboxSdModelVae.InitCombox();
                UpdateComboxStates();
            })).RunWithUiStopped(this, showErrors: true);
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

        private void LoadImplementations()
        {
            if (this.RequiresInvoke(new Action(LoadImplementations)))
                return;

            comboxImplementation.Items.Clear();
            comboxImplementation.FillFromEnum<Implementation>(Strings.Implementation, -1);
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
            ConfigParser.LoadComboxIndex(comboxTimestampInFilename, ref Config.Instance.FilenameTimestampMode);
            ConfigParser.LoadGuiElement(checkboxPromptInFilename, ref Config.Instance.PromptInFilename);
            ConfigParser.LoadGuiElement(checkboxSeedInFilename, ref Config.Instance.SeedInFilename);
            ConfigParser.LoadGuiElement(checkboxScaleInFilename, ref Config.Instance.ScaleInFilename);
            ConfigParser.LoadGuiElement(checkboxSamplerInFilename, ref Config.Instance.SamplerInFilename);
            ConfigParser.LoadGuiElement(checkboxModelInFilename, ref Config.Instance.ModelInFilename);
            ConfigParser.LoadGuiElement(comboxImgSaveMode, ref Config.Instance.AutoDeleteImgs);
            ConfigParser.LoadGuiElement(textboxOutPath, ref Config.Instance.OutPath);
            ConfigParser.LoadGuiElement(textboxFavsPath, ref Config.Instance.FavsPath);
            ConfigParser.LoadGuiElement(textboxEmbeddingsDir, ref Config.Instance.EmbeddingsDir);
            ConfigParser.LoadGuiElement(textboxLorasDir, ref Config.Instance.LorasDir);
            //ConfigParser.LoadGuiElement(comboxSdModel, ref Config.Instance.Model);
            ConfigParser.LoadGuiElement(comboxSdModelVae, ref Config.Instance.ModelVae);
            // ConfigParser.LoadComboxIndex(comboxCudaDevice);
            ConfigParser.LoadGuiElement(checkboxModelCaching, ref Config.Instance.InvokeAllowModelCaching);
            ConfigParser.LoadComboxIndex(comboxClipSkip, ref Config.Instance.ClipSkip);
            ConfigParser.LoadComboxIndex(comboxNotify, ref Config.Instance.NotifyModeIdx);
            ConfigParser.LoadGuiElement(checkboxSaveUnprocessedImages, ref Config.Instance.SaveUnprocessedImages);
            ConfigParser.LoadGuiElement(checkboxUnloadModel, ref Config.Instance.UnloadModel);
            ConfigParser.LoadGuiElement(checkboxAutoSetResForInitImg, ref Config.Instance.AutoSetResForInitImg);
            ConfigParser.LoadGuiElement(checkboxInitImageRetainAspectRatio, ref Config.Instance.InitImageRetainAspectRatio);
            ConfigParser.LoadComboxIndex(comboxComfyVram, ref Config.Instance.ComfyVramPreset);
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
            ConfigParser.SaveComboxIndex(comboxTimestampInFilename, ref Config.Instance.FilenameTimestampMode);
            ConfigParser.SaveGuiElement(checkboxPromptInFilename, ref Config.Instance.PromptInFilename);
            ConfigParser.SaveGuiElement(checkboxSeedInFilename, ref Config.Instance.SeedInFilename);
            ConfigParser.SaveGuiElement(checkboxScaleInFilename, ref Config.Instance.ScaleInFilename);
            ConfigParser.SaveGuiElement(checkboxSamplerInFilename, ref Config.Instance.SamplerInFilename);
            ConfigParser.SaveGuiElement(checkboxModelInFilename, ref Config.Instance.ModelInFilename);
            ConfigParser.SaveGuiElement(comboxImgSaveMode, ref Config.Instance.AutoDeleteImgs);
            ConfigParser.SaveGuiElement(textboxOutPath, ref Config.Instance.OutPath);
            ConfigParser.SaveGuiElement(textboxFavsPath, ref Config.Instance.FavsPath);
            ConfigParser.SaveGuiElement(textboxEmbeddingsDir, ref Config.Instance.EmbeddingsDir);
            ConfigParser.SaveGuiElement(textboxLorasDir, ref Config.Instance.LorasDir);
            if (!string.IsNullOrWhiteSpace(comboxSdModel.Text)) ConfigParser.SaveGuiElement(comboxSdModel, ref Config.Instance.Model);
            if (!string.IsNullOrWhiteSpace(comboxSdModelVae.Text)) ConfigParser.SaveGuiElement(comboxSdModelVae, ref Config.Instance.ModelVae);
            if (!string.IsNullOrWhiteSpace(comboxUsduModel.Text)) ConfigParser.SaveGuiElement(comboxUsduModel, ref Config.Instance.SdUpscaleModel);
            if (!comboxCudaDevice.Text.StartsWith("Loading")) ConfigParser.SaveComboxIndex(comboxCudaDevice, ref Config.Instance.CudaDeviceIdx);
            ConfigParser.SaveGuiElement(checkboxModelCaching, ref Config.Instance.InvokeAllowModelCaching);
            ConfigParser.SaveComboxIndex(comboxClipSkip, ref Config.Instance.ClipSkip);
            ConfigParser.SaveComboxIndex(comboxNotify, ref Config.Instance.NotifyModeIdx);
            ConfigParser.SaveGuiElement(checkboxSaveUnprocessedImages, ref Config.Instance.SaveUnprocessedImages);
            ConfigParser.SaveGuiElement(checkboxUnloadModel, ref Config.Instance.UnloadModel);
            ConfigParser.SaveGuiElement(checkboxAutoSetResForInitImg, ref Config.Instance.AutoSetResForInitImg);
            ConfigParser.SaveGuiElement(checkboxInitImageRetainAspectRatio, ref Config.Instance.InitImageRetainAspectRatio);
            ConfigParser.SaveComboxIndex(comboxComfyVram, ref Config.Instance.ComfyVramPreset);

            Config.Save();
        }

        private void BrowsePath(TextBox textbox)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { InitialDirectory = textbox.Text, IsFolderPicker = true };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                textbox.Text = dialog.FileName;
        }

        private void btnOpenModelsFolder_Click(object sender, EventArgs e)
        {
            SetupModelDirs(ModelFoldersForm.Folder.Models);
        }

        private void btnOpenModelsFolderVae_Click(object sender, EventArgs e)
        {
            SetupModelDirs(ModelFoldersForm.Folder.Vaes);
        }

        private void SetupModelDirs(ModelFoldersForm.Folder folderType)
        {
            new ModelFoldersForm(folderType).ShowDialogForm();

            if (folderType == ModelFoldersForm.Folder.Vaes)
                LoadVaes();
            else
                LoadModels();
        }

        private void btnRefreshModelsDropdown_Click(object sender, EventArgs e)
        {
            LoadModels();
        }

        private void btnRefreshModelsDropdownVae_Click(object sender, EventArgs e)
        {
            LoadVaes();
        }

        private void comboxImplementation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrImplementation < 0)
                return;

            ((Action)(() =>
            {
                Config.Instance.Implementation = CurrImplementation;
                panelFullPrecision.SetVisible(CurrImplementation.Supports(ImplementationInfo.Feature.HalfPrecisionToggle));
                panelUnloadModel.SetVisible(CurrImplementation == Implementation.InvokeAi);
                panelCudaDevice.SetVisible(CurrImplementation.Supports(ImplementationInfo.Feature.DeviceSelection));
                panelSdModel.SetVisible(CurrImplementation.Supports(ImplementationInfo.Feature.CustomModels));
                panelUsduModel.SetVisible(CurrImplementation == Implementation.Comfy);
                panelEmbeddingsPath.SetVisible(CurrImplementation.Supports(ImplementationInfo.Feature.Embeddings));
                panelLoras.SetVisible(CurrImplementation.Supports(ImplementationInfo.Feature.Lora));
                panelVae.SetVisible(CurrImplementation.Supports(ImplementationInfo.Feature.CustomVae));
                panelAdvancedOptsInvoke.SetVisible(CurrImplementation == Implementation.InvokeAi);
                panelModelCaching.SetVisible(CurrImplementation == Implementation.InvokeAi);
                panelComfyVram.SetVisible(CurrImplementation == Implementation.Comfy);

                LoadModels();
                LoadVaes();
            })).RunWithUiStopped(this, showErrors: true);
        }

        private void UpdateComboxStates()
        {
            comboxSdModel.Enabled = comboxSdModel.Items.Count > 0;
            comboxSdModelVae.Enabled = comboxSdModelVae.Items.Count > 0;
        }

        private void parentPanel_SizeChanged(object sender, EventArgs e)
        {
            var newPadding = parentPanel.Padding;
            newPadding.Right = parentPanel.VerticalScroll.Visible ? 6 : 0;

            if (parentPanel.Padding.Right != newPadding.Right)
                parentPanel.Padding = newPadding;
        }

        private void btnRefreshUsduModelsDropdown_Click(object sender, EventArgs e)
        {
            LoadModels();
        }
    }
}
