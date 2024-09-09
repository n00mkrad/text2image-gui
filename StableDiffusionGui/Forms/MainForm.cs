using Dasync.Collections;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Implementations;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Main.Utils;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class MainForm : CustomForm
    {
        [Flags]
        public enum EXECUTION_STATE : uint { ES_AWAYMODE_REQUIRED = 0x00000040, ES_CONTINUOUS = 0x80000000, ES_DISPLAY_REQUIRED = 0x00000002, ES_SYSTEM_REQUIRED = 0x00000001 }
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags); // This should prevent Windows from going to sleep

        public bool IsInFocus() { return (ActiveForm == this); }

        public MainForm()
        {
            InitializeComponent();
            AllowTextboxTab = false;
            Program.MainForm = this;
            Opacity = 0;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = Program.Debug;
            AllowEscClose = false;
            Task.Run(() => Logger.QueueLoopOuter());
            MinimumSize = Size;
            Text = $"Stable Diffusion GUI {Program.Version}";
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveControls();

            if (Program.Busy)
            {
                DialogResult dialogResult = UiUtils.ShowMessageBox($"The program is still busy. Are you sure you want to quit?", UiUtils.MessageType.Warning.ToString(), MessageBoxButtons.YesNo);

                if (dialogResult != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            if (Config.Instance.AutoDeleteImgs)
                ImageViewer.DeleteAll(false);
        }

        private async void MainForm_Shown(object sender, EventArgs e)
        {
            Refresh();
            await Initialize();
        }

        private async Task Initialize()
        {
            InitializeControls();
            PromptHistory.Load();
            Setup.PatchFiles();
            PatchUtils.PatchAllPkgs();

            textboxPrompt.MaxLength = 0;
            textboxPromptNeg.MaxLength = 0;
            pictBoxImgViewer.MouseWheel += (s, e) => { ImageViewer.Move(e.Delta > 0); }; // Scroll on MouseWheel

            if (Program.UserArgs.Get(Constants.Args.Install).GetBool() != true) // Don't check for GPU if auto-install is passed (installer runs the check later)
                Task.Run(() => MainUi.GetCudaGpus());

            Task.Run(() => MainUi.PrintVersion());
            upDownSeed.Text = "";

            TabOrderInit(new List<Control>() {
                textboxPrompt, textboxPromptNeg, comboxEmbeddingList,
                sliderInitStrength, textboxSliderInitStrength,
                comboxInpaintMode, comboxResizeGravity, textboxClipsegMask,
                upDownIterations,
                sliderSteps, textboxSliderSteps,
                sliderScale, textboxSliderScale,
                upDownSeed, checkboxLockSeed,
                comboxResW, comboxResH, checkboxHiresFix,
                updownUpscaleFactor, updownUpscaleResultW, updownUpscaleResultH,
                comboxSampler,
                comboxSeamless,
                comboxSymmetry,
                runBtn
            }, -1);

            await Task.Delay(1); // Don't ask. Just keep it here
            TryRefreshUiState(false);
            LoadControls();
            Opacity = 1.0;
            MainUi.DoStartupChecks();

            if (!Program.Debug && !(Config.Instance.HideMotd && Config.Instance.MotdShownVersion == Program.Version))
                new WelcomeForm().ShowDialogForm();
        }

        private void installerBtn_Click(object sender, EventArgs e)
        {
            menuStripInstall.Show(Cursor.Position);
        }

        public void CleanPrompt()
        {
            textboxPrompt.Text = string.Join(Environment.NewLine, textboxPrompt.Text.SplitIntoLines().Select(x => FormatUtils.SanitizePrompt(x)));
            textboxPromptNeg.Text = string.Join(Environment.NewLine, textboxPromptNeg.Text.SplitIntoLines().Select(x => FormatUtils.SanitizePrompt(x)));

            if (upDownSeed.Text == "")
                SetSeed();
        }

        public void SetSeed(long seed = -1)
        {
            upDownSeed.Value = seed;

            if (seed < 0)
                upDownSeed.Text = "";
        }

        public async void runBtn_Click(object sender, EventArgs e)
        {
            panelSettings.Focus();
            await TryRun();

            if (checkboxLoopback.Checked)
            {
                TryUseCurrentImgAsInitImg(true);
                runBtn_Click(null, null);
            }
        }

        private void btnPrevImg_Click(object sender, EventArgs e)
        {
            ImageViewer.Move(true);
        }

        private void btnNextImg_Click(object sender, EventArgs e)
        {
            ImageViewer.Move(false);
        }

        private void btnOpenOutFolder_Click(object sender, EventArgs e)
        {
            menuStripOpenFolder.Show(Cursor.Position);
        }

        #region Link Buttons

        private void paypalBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.paypal.com/paypalme/nmkd");
        }

        private void patreonBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://patreon.com/n00mkrad");
        }

        private void discordBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/fZwWSnV5WA");
        }

        #endregion

        #region Output Image Menu Strip

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageViewer.OpenCurrent();
        }

        private void openOutputFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageViewer.OpenFolderOfCurrent();
        }

        private void copyImageToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OsUtils.SetClipboard(pictBoxImgViewer.GetImageSafe());
        }

        private void copySeedToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OsUtils.SetClipboard(ImageViewer.CurrentImageMetadata.Seed.ToString());
        }

        private void useAsInitImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TryUseCurrentImgAsInitImg();
        }

        private void copyToFavoritesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageViewer.CopyCurrentToFavs();
        }

        private void postProcessImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPostProcessMenu();
        }

        #endregion

        private void cliButton_Click(object sender, EventArgs e)
        {
            menuStripDevTools.Show(Cursor.Position);
        }

        private void pictBoxImgViewer_Click(object sender, EventArgs e)
        {
            HandleImageViewerClick(((MouseEventArgs)e).Button == MouseButtons.Right);
        }

        #region Drag N Drop

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                MainUi.HandleDroppedFiles((string[])e.Data.GetData(DataFormats.FileDrop));
        }

        #endregion

        #region Init Img and Embedding

        private void btnInitImgBrowse_Click(object sender, EventArgs e)
        {
            BrowseInitImage();
        }

        #endregion

        private void btnDebug_Click(object sender, EventArgs e)
        {
            OpenLogsMenu();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            new SettingsForm().ShowDialogForm();
        }

        private void btnPostProc_Click(object sender, EventArgs e)
        {
            TryOpenPostProcessingSettings();
        }

        public void btnExpandPromptField_Click(object sender, EventArgs e)
        {
            MainUi.SetPanelSize(panelPrompt, btnExpandPromptField, MainUi.PanelSizeMode.Toggle);
        }

        public void btnExpandPromptNegField_Click(object sender, EventArgs e)
        {
            MainUi.SetPanelSize(panelPromptNeg, btnExpandPromptNegField, MainUi.PanelSizeMode.Toggle);
        }

        private void btnSeedUsePrevious_Click(object sender, EventArgs e)
        {
            SetSeed(TextToImage.PreviousSeed);
        }

        private void btnSeedResetToRandom_Click(object sender, EventArgs e)
        {
            SetSeed();
        }

        private void btnPromptHistory_Click(object sender, EventArgs e)
        {
            new PromptListForm(PromptListForm.ListMode.History).ShowDialogForm();
        }

        private void btnQueue_Click(object sender, EventArgs e)
        {
            new PromptListForm(PromptListForm.ListMode.Queue).ShowDialogForm();
        }

        private async void generateCurrentPromptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Run();
        }

        private async void generateAllQueuedPromptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Run(true);
        }

        private void btnResetMask_Click(object sender, EventArgs e)
        {
            Inpainting.ClearMask();
        }

        private void addCurrentSettingsToQueueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var settings = GetCurrentTtiSettings();

            if (settings.Prompts.Where(x => !string.IsNullOrWhiteSpace(x)).Any())
                MainUi.Queue.Enqueue(settings);
        }

        private void btnQueue_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                menuStripAddToQueue.Show(Cursor.Position);
        }

        private async void reGenerateImageWithCurrentSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await RegenerateImageWithCurrentSettings();
        }

        private void btnDeleteBatch_Click(object sender, EventArgs e)
        {
            menuStripDeleteImages.Show(Cursor.Position);
        }

        private void deleteThisImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageViewer.DeleteCurrent();
        }

        private void deleteAllCurrentImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageViewer.DeleteAll();
        }

        private void openDreampyCLIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.Busy || !MainUi.IsInstalledWithWarning())
                return;

            InvokeAi.RunCli(Config.Instance.OutPath, Config.Instance.ModelVae);
        }

        private void openModelMergeToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UiUtils.ShowMessageBox("Model merging code is currently slightly outdated and only works with CKPT models.\n" +
                "Compatibility will be improved in the future.", UiUtils.MessageType.Warning);
            new MergeModelsForm().ShowDialogForm();
        }

        private void openModelPruningTrimmingToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new PruneModelsForm().ShowDialogForm();
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            panelSettings.Focus();
        }

        private void viewLogInRealtimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenLogViewerWindow();
        }

        private void fitWindowSizeToImageSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainUi.FitWindowSizeToImageSize();
            CenterToScreen();
        }

        private void labelImgPrompt_Click(object sender, EventArgs e)
        {
            OsUtils.SetClipboard(ImageViewer.CurrentImageMetadata.Prompt);
        }

        private void labelImgPromptNeg_Click(object sender, EventArgs e)
        {
            OsUtils.SetClipboard(ImageViewer.CurrentImageMetadata.NegativePrompt);
        }

        private void openCmdInPythonEnvironmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InvokeAi.StartCmdInSdEnv();
        }

        #region Post-Processing

        public void ShowPostProcessMenu()
        {
            menuStripPostProcess.Show(Cursor.Position);
        }

        private async void upscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Config.Instance.Implementation == Enums.StableDiffusion.Implementation.InvokeAi)
            await InvokeAi.RunFix(ImageViewer.CurrentImagePath, InvokeAi.FixAction.Upscale.AsList());

            if (Config.Instance.Implementation == Enums.StableDiffusion.Implementation.Comfy)
                await Comfy.Upscale(ImageViewer.CurrentImagePath);
        }

        private async void applyFaceRestorationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await InvokeAi.RunFix(ImageViewer.CurrentImagePath, InvokeAi.FixAction.FaceRestoration.AsList());
        }

        private async void applyAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await InvokeAi.RunFix(ImageViewer.CurrentImagePath, new[] { InvokeAi.FixAction.Upscale, InvokeAi.FixAction.FaceRestoration }.ToList());
        }

        #endregion

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Hotkeys.HandleMainForm(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void panelSettings_SizeChanged(object sender, EventArgs e)
        {
            var newPadding = panelSettings.Padding;
            newPadding.Right = panelSettings.VerticalScroll.Visible ? 6 : 0;

            if (panelSettings.Padding.Right != newPadding.Right)
                panelSettings.Padding = newPadding;
        }

        private void btnDreambooth_Click(object sender, EventArgs e)
        {
            new DreamboothForm().ShowDialogForm();
            TryRefreshUiState();
        }

        private void comboxInpaintMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            TryRefreshUiState();
        }

        private void labelCurrentImage_MouseEnter(object sender, EventArgs e)
        {
            Application.OpenForms.Cast<Form>().Where(f => f is ImageHoverForm).ToList().ForEach(f => f.Close());
            MainUi.CurrentInitImgPaths?.Take(1).ToList().ForEach(img => new ImageHoverForm(IoUtils.GetImage(img)).Show());
        }

        private void labelCurrentImage_MouseLeave(object sender, EventArgs e)
        {
            Application.OpenForms.Cast<Form>().Where(f => f is ImageHoverForm).ToList().ForEach(f => f.Close());
            Program.MainForm.toolTip.Active = true;
        }

        private void btnEditMask_Click(object sender, EventArgs e)
        {
            EditMask();
        }

        private void convertModelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ConvertModelForm().ShowDialog();
        }

        private void checkboxShowInitImg_CheckedChanged(object sender, EventArgs e)
        {
            tableLayoutPanelImgViewers.ColumnStyles[0].Width = checkboxShowInitImg.Checked ? 50 : 0;
            tableLayoutPanelImgViewers.ColumnStyles[1].Width = checkboxShowInitImg.Checked ? 50 : 100;

            if (WindowState != FormWindowState.Maximized)
            {
                MainUi.FitWindowSizeToImageSize();
                CenterToScreen();
            }
        }

        private void copySidebySideComparisonImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OsUtils.SetClipboard(ImageViewer.GetCurrentImageComparison());
        }

        private void manageInstallationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new InstallerForm().ShowDialogForm();
        }

        private void installUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new UpdaterForm().ShowDialogForm();
        }

        #region Embeddings

        private void comboxEmbeddingList_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool vis = comboxEmbeddingList.Visible && comboxEmbeddingList.SelectedIndex > 0;
            btnEmbeddingCopy.SetVisible(vis);
            btnEmbeddingAppend.SetVisible(vis);
        }

        private void btnEmbeddingCopy_Click(object sender, EventArgs e)
        {
            OsUtils.SetClipboard(comboxEmbeddingList.Text);
        }

        private void btnEmbeddingAppend_Click(object sender, EventArgs e)
        {
            TextBox textbox = InputUtils.IsHoldingShift ? textboxPromptNeg : textboxPrompt;
            textbox.Focus();
            string format = FormatUtils.GetEmbeddingFormat(Config.Instance.Implementation);
            textbox.Text = textbox.Text.Append(string.Format(format, comboxEmbeddingList.Text), true);
            btnEmbeddingAppend.Focus();
        }

        #endregion

        private void btnResetRes_Click(object sender, EventArgs e)
        {
            MainUi.SetResolutionForInitImage(MainUi.CurrentInitImgPaths.FirstOrDefault());
        }

        private void downloadHuggingfaceModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModelDownloadPrompt();
        }

        private void btnExpandLoras_Click(object sender, EventArgs e)
        {
            MainUi.SetPanelSize(panelLoras, btnExpandLoras, MainUi.PanelSizeMode.Toggle, heightMultiplier: 3);
        }

        private void openOutputFolderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string dir = Directory.CreateDirectory(Config.Instance.OutPath).FullName;
            Process.Start("explorer", dir.Replace("/", @"\").Wrap());
        }

        private void openFavoritesFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dir = Directory.CreateDirectory(Config.Instance.FavsPath).FullName;
            Process.Start("explorer", dir.Replace("/", @"\").Wrap());
        }

        private void btnSaveToFavs_Click(object sender, EventArgs e)
        {
            ImageViewer.CopyCurrentToFavs();
        }

        private void btnSaveMode_Click(object sender, EventArgs e)
        {
            ToggleSaveMode();
        }

        private void sD15ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "https://huggingface.co/nmkd/sd1-safetensors/resolve/main/sd_1.5_fp16.safetensors";
            DownloadModels.DownloadFile(url, Path.Combine(Paths.GetModelsPath(), "sd_xl_base_1.0_fixvae.safetensors"));
        }

        private void sD15ONNXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DownloadModels.DownloadModel("nmkd/stable-diffusion-1.5-onnx");
        }

        private void sDXL10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "https://huggingface.co/bdsqlsz/stable-diffusion-xl-base-1.0_fixvae_fp16/resolve/main/sd_xl_base_1.0_fixvae_V2_fp16.safetensors";
            DownloadModels.DownloadFile(url, Path.Combine(Paths.GetModelsPath(), "sd_xl_base_1.0_fixvae.safetensors"));
        }

        private void sDXL10RefinerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "https://huggingface.co/stabilityai/stable-diffusion-xl-refiner-1.0/resolve/main/sd_xl_refiner_1.0.safetensors";
            DownloadModels.DownloadFile(url, Path.Combine(Paths.GetModelsPath(), "sd_xl_refiner_1.0.safetensors"));
        }

        private void checkboxPreview_CheckedChanged(object sender, EventArgs e)
        {
            pictBoxPreview.Visible = checkboxPreview.Checked;
        }
    }
}