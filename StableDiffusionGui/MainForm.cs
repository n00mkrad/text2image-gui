using Dasync.Collections;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Forms;
using StableDiffusionGui.Implementations;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using StableDiffusionGui.Ui.MainForm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui
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
            CheckForIllegalCrossThreadCalls = false;
            AllowEscClose = false;
            Logger.Textbox = logBox;
            Task.Run(() => Logger.QueueLoop());
            MinimumSize = Size;
            Text = $"Stable Diffusion GUI {Program.Version}";
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormControls.Save();

            if (Program.Busy)
            {
                DialogResult dialogResult = UiUtils.ShowMessageBox($"The program is still busy. Are you sure you want to quit?", UiUtils.MessageType.Warning.ToString(), MessageBoxButtons.YesNo);
                e.Cancel = dialogResult != DialogResult.Yes;
            }
        }

        private async void MainForm_Shown(object sender, EventArgs e)
        {
            Refresh();
            await Initialize();
        }

        private async Task Initialize()
        {
            MainUi.SetSettingsVertScrollbar();
            FormControls.InitializeControls();
            FormControls.RefreshUiAfterSettingsChanged();
            FormControls.Load();
            PromptHistory.Load();
            Setup.PatchFiles();

            pictBoxImgViewer.MouseWheel += (s, e) => { ImageViewer.Move(e.Delta > 0); }; // Scroll on MouseWheel
            comboxResW.SelectedIndexChanged += (s, e) => { FormControls.SetHiresFixVisible(); }; // Show/Hide HiRes Fix depending on chosen res
            comboxResH.SelectedIndexChanged += (s, e) => { FormControls.SetHiresFixVisible(); }; // Show/Hide HiRes Fix depending on chosen res

            MainUi.LoadAutocompleteData(promptAutocomplete, new[] { textboxPrompt, textboxPromptNeg });
            Task.Run(() => MainUi.SetGpusInWindowTitle());
            upDownSeed.Text = "";
            MainUi.DoStartupChecks();
            FormControls.UpdateInitImgAndEmbeddingUi();

            TabOrderInit(new List<Control>() {
                textboxPrompt, textboxPromptNeg,
                sliderInitStrength, textboxSliderInitStrength,
                comboxInpaintMode, textboxClipsegMask,
                upDownIterations,
                sliderSteps, textboxSliderSteps,
                sliderScale, textboxSliderScale,
                upDownSeed, checkboxLockSeed,
                comboxResW, comboxResH, checkboxHiresFix,
                comboxSampler,
                comboxSeamless,
                runBtn
            }, false);

            await Task.Delay(1); // Don't ask. Just keep it here
            Opacity = 1.0;

            if (!Program.Debug)
                new WelcomeForm().ShowDialogForm(0f);

            panelDebugSendStdin.Visible = Program.Debug;
            panelDebugPerlinThresh.Visible = Program.Debug;
        }

        private void installerBtn_Click(object sender, EventArgs e)
        {
            new InstallerForm().ShowDialogForm();
        }

        public void CleanPrompt()
        {
            if (File.Exists(MainUi.CurrentEmbeddingPath) && Path.GetExtension(MainUi.CurrentEmbeddingPath).Lower() == ".bin")
            {
                string conceptName = Path.GetFileNameWithoutExtension(MainUi.CurrentEmbeddingPath);
                textboxPrompt.Text = textboxPrompt.Text.Replace("*", $"<{conceptName.Trim()}>");
            }

            textboxPrompt.Text = string.Join(Environment.NewLine, textboxPrompt.Text.SplitIntoLines().Select(x => MainUi.SanitizePrompt(x)));
            textboxPromptNeg.Text = MainUi.SanitizePrompt(textboxPromptNeg.Text);

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
            await FormUtils.TryRun();
        }

        public void SetProgress(int percent, bool taskbarProgress = true)
        {
            FormControls.SetProgress(percent, taskbarProgress, progressBar);
        }

        public void SetProgressImg(int percent, bool taskbarProgress = false)
        {
            FormControls.SetProgress(percent, taskbarProgress, progressBarImg);
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
            Process.Start("explorer", Config.Get(Config.Key.textboxOutPath));
        }

        #region Link Buttons

        private void paypalBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.paypal.com/paypalme/nmkd/8");
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
            OsUtils.SetClipboard(pictBoxImgViewer.Image);
        }

        private void copySeedToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OsUtils.SetClipboard(ImageViewer.CurrentImageMetadata.Seed.ToString());
        }

        private void useAsInitImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormUtils.TryUseCurrentImgAsInitImg();
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
            FormControls.HandleImageViewerClick(((MouseEventArgs)e).Button == MouseButtons.Right);
        }

        #region Drag N Drop

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            MainUi.HandleDroppedFiles((string[])e.Data.GetData(DataFormats.FileDrop));
        }

        #endregion

        #region Init Img and Embedding

        private void btnInitImgBrowse_Click(object sender, EventArgs e)
        {
            FormUtils.BrowseInitImage();
        }

        private void btnEmbeddingBrowse_Click(object sender, EventArgs e)
        {
            FormUtils.BrowseEmbedding();
        }

        #endregion

        private void btnDebug_Click(object sender, EventArgs e)
        {
            FormControls.OpenLogsMenu();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            new SettingsForm().ShowDialogForm(0.5f);
        }

        private void btnPostProc_Click(object sender, EventArgs e)
        {
            FormUtils.TryOpenPostProcessingSettings();
        }

        private void btnExpandPromptField_Click(object sender, EventArgs e)
        {
            MainUi.SetPromptFieldSize(MainUi.PromptFieldSizeMode.Toggle, false);
        }

        private void btnExpandPromptNegField_Click(object sender, EventArgs e)
        {
            MainUi.SetPromptFieldSize(MainUi.PromptFieldSizeMode.Toggle, true);
        }

        private void btnSeedUsePrevious_Click(object sender, EventArgs e)
        {
            upDownSeed.Value = TextToImage.PreviousSeed;
        }

        private void btnSeedResetToRandom_Click(object sender, EventArgs e)
        {
            upDownSeed.Value = -1;
            upDownSeed.Text = "";
        }

        private void btnPromptHistory_Click(object sender, EventArgs e)
        {
            new PromptListForm(PromptListForm.ListMode.History).ShowDialogForm();
        }

        private void btnQueue_Click(object sender, EventArgs e)
        {
            if (Program.Busy)
                return;

            new PromptListForm(PromptListForm.ListMode.Queue).ShowDialogForm();
        }

        private async void generateCurrentPromptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await FormUtils.Run();
        }

        private async void generateAllQueuedPromptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await FormUtils.Run(true);
        }

        public void UpdateInpaintUi()
        {
            btnResetMask.Visible = InpaintingUtils.CurrentMask != null;
        }

        private void btnResetMask_Click(object sender, EventArgs e)
        {
            InpaintingUtils.CurrentMask = null;
        }

        private void textboxCliTest_DoubleClick(object sender, EventArgs e)
        {
            TtiProcess.WriteStdIn(textboxCliTest.Text);
            textboxCliTest.Text = "";
        }

        private void addCurrentSettingsToQueueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var settings = FormParsing.GetCurrentTtiSettings();

            if (settings.Prompts.Where(x => !string.IsNullOrWhiteSpace(x)).Any())
                MainUi.Queue.Add(settings);
        }

        private void btnQueue_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                menuStripAddToQueue.Show(Cursor.Position);
        }

        private async void reGenerateImageWithCurrentSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await FormUtils.RegenerateImageWithCurrentSettings();
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

            InvokeAi.RunCli(Config.Get(Config.Key.textboxOutPath), Config.Get(Config.Key.comboxSdModelVae));
        }

        private void openModelMergeToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
            new RealtimeLoggerForm().Show();
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
            InvokeAi.StartCmdInSdEnv(false);
        }

        #region Post-Processing

        public void ShowPostProcessMenu()
        {
            menuStripPostProcess.Show(Cursor.Position);
        }

        private async void upscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await InvokeAi.RunFix(ImageViewer.CurrentImagePath, new[] { InvokeAi.FixAction.Upscale }.ToList());
        }

        private async void applyFaceRestorationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await InvokeAi.RunFix(ImageViewer.CurrentImagePath, new[] { InvokeAi.FixAction.FaceRestoration }.ToList());
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

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            MainUi.SetSettingsVertScrollbar();
        }

        private void btnDreambooth_Click(object sender, EventArgs e)
        {
            new DreamboothForm().ShowDialogForm();
        }

        private void comboxInpaintMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            textboxClipsegMask.Visible = (InpaintMode)comboxInpaintMode.SelectedIndex == InpaintMode.TextMask;
        }
    }
}