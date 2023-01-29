﻿using Dasync.Collections;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Forms;
using StableDiffusionGui.Implementations;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using StableDiffusionGui.Ui.MainFormUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
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
            Task.Run(() => MainUi.PrintVersion());
            upDownSeed.Text = "";
            FormControls.RefreshUiAfterSettingsChanged();

            TabOrderInit(new List<Control>() {
                textboxPrompt, textboxPromptNeg,
                sliderInitStrength, textboxSliderInitStrength,
                comboxInpaintMode, textboxClipsegMask,
                upDownIterations,
                sliderSteps, textboxSliderSteps,
                sliderScale, textboxSliderScale,
                upDownSeed,
                comboxResW, comboxResH, checkboxHiresFix,
                comboxSampler,
                comboxSeamless,
                runBtn
            }, -1);

            await Task.Delay(1); // Don't ask. Just keep it here

            Opacity = 1.0;
            MainUi.DoStartupChecks();

            if (!Program.Debug && !(Config.Get<bool>(Config.Keys.HideMotd) && Config.Get<string>(Config.Keys.MotdShownVersion) == Program.Version))
                new WelcomeForm().ShowDialogForm();

            panelDebugLoopback.Visible = Program.Debug;
            panelDebugPerlinThresh.Visible = Program.Debug;
            panelDebugSendStdin.Visible = Program.Debug;

            LoadModels();
        }

        private void installerBtn_Click(object sender, EventArgs e)
        {
            new InstallerForm().ShowDialogForm();
        }

        string CurrImplText;
        private void LoadImplementations()
        {
            CurrImplText = Strings.Implementation.Get(Config.Get<string>(Config.Keys.ImplementationName));
        }
        private Implementation CurrImplementation { get { return ParseUtils.GetEnum<Implementation>(CurrImplText, true, Strings.Implementation); } }

        private void LoadModels()
        {
            LoadImplementations();

            var combox = comboxSdModel;

            combox.Items.Clear();

            Paths.GetModels(ModelType.Normal, CurrImplementation).ForEach(x => combox.Items.Add(x.Name));

            ConfigParser.LoadGuiElement(combox, Config.Keys.Model);

            if (combox.Items.Count > 0 && combox.SelectedIndex == -1)
                combox.SelectedIndex = 0;
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

            if (checkboxLoopback.Checked)
            {
                FormUtils.TryUseCurrentImgAsInitImg(true);
                runBtn_Click(null, null);
            }
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
            Process.Start("explorer", Config.Get<string>(Config.Keys.OutPath).Replace("/", @"\").Wrap());
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

            checkboxShowInitImg.Visible = true;
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
            new SettingsForm().ShowDialogForm();
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
            btnResetMask.Visible = Inpainting.CurrentMask != null;
            btnEditMask.Visible = Inpainting.CurrentMask != null;
        }

        private void btnResetMask_Click(object sender, EventArgs e)
        {
            Inpainting.ClearMask();
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

            InvokeAi.RunCli(Config.Get<string>(Config.Keys.OutPath), Config.Get<string>(Config.Keys.ModelVae));
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
            Application.OpenForms.Cast<Form>().Where(f => f is RealtimeLoggerForm).ToList().ForEach(f => f.Close());
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
            InvokeAi.StartCmdInSdEnv();
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

        private void btnDreambooth_Click(object sender, EventArgs e)
        {
            new DreamboothForm().ShowDialogForm();
        }

        private void comboxInpaintMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormControls.RefreshUiAfterSettingsChanged();
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
            FormUtils.EditMask();
        }

        private void convertModelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ConvertModelForm().ShowDialog();
        }

        private void checkboxShowInitImg_CheckedChanged(object sender, EventArgs e)
        {
            tableLayoutPanelImgViewers.ColumnStyles[0].Width = checkboxShowInitImg.Checked ? 50 : 0;
            tableLayoutPanelImgViewers.ColumnStyles[1].Width = checkboxShowInitImg.Checked ? 50 : 100;
            MainUi.FitWindowSizeToImageSize();
            CenterToScreen();
        }

        private void comboxResW_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateAspectRatio();
        }

        private void comboxResH_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateAspectRatio();
        }

        private void UpdateAspectRatio ()
        {
            int w = comboxResW.GetInt();
            int h = comboxResH.GetInt();
            int gcd = GCD(w, h);
            int reducedWidth = w / gcd;
            int reducedHeight = h / gcd;
            labelAspectRatio.Text = $"Ratio {reducedWidth}:{reducedHeight}".Replace("8:5", "8:5 (16:10)").Replace("7:3", "7:3 (21:9)");
        }

        private int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        private void copySidebySideComparisonImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OsUtils.SetClipboard(ImageViewer.GetCurrentImageComparison());
        }

        private void htSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            upDownSeed.Value = -1;
            upDownSeed.Text = "";

            upDownSeed.Enabled = !htSwitch1.Checked;
        }

        private void comboxSdModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(comboxSdModel.Text)) ConfigParser.SaveGuiElement(comboxSdModel, Config.Keys.Model);
        }
    }
}