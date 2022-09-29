using StableDiffusionGui.Forms;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Taskbar;
using Microsoft.WindowsAPICodePack.Dialogs;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Data;
using StableDiffusionGui.Os;
using StableDiffusionGui.Properties;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Paths = StableDiffusionGui.Io.Paths;
using System.Globalization;
using HTAlt.WinForms;

namespace StableDiffusionGui
{
    public partial class MainForm : Form
    {
        [Flags]
        public enum EXECUTION_STATE : uint { ES_AWAYMODE_REQUIRED = 0x00000040, ES_CONTINUOUS = 0x80000000, ES_DISPLAY_REQUIRED = 0x00000002, ES_SYSTEM_REQUIRED = 0x00000001 }
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags); // This should prevent Windows from going to sleep

        #region References
        public Button RunBtn { get { return runBtn; } }
        public TextBox TextboxPrompt { get { return textboxPrompt; } }
        public PictureBox PictBoxImgViewer { get { return pictBoxImgViewer; } }
        public Label OutputImgLabel { get { return outputImgLabel; } }
        public Button BtnExpandPromptField { get { return btnExpandPromptField; } }
        #endregion

        public bool IsInFocus() { return (ActiveForm == this); }

        private float _defaultPromptFontSize;

        public MainForm()
        {
            InitializeComponent();
            Program.MainForm = this;
            pictBoxImgViewer.MouseWheel += pictBoxImgViewer_MouseWheel;
            textboxPrompt.MouseWheel += textboxPrompt_MouseWheel;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            _defaultPromptFontSize = textboxPrompt.Font.Size;
            Logger.Textbox = logBox;
            MinimumSize = Size;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveUiElements();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            LoadUiElements();
            PromptHistory.Load();
            Setup.FixHardcodedPaths();
            Task.Run(() => SetGpusInWindowTitle());
            upDownSeed.Text = "";
            MainUi.DoStartupChecks();
            RefreshAfterSettingsChanged();
            UpdateInitImgAndEmbeddingUi();

            if (!Debugger.IsAttached)
                new WelcomeForm().ShowDialog();

            textboxCliTest.Visible = Debugger.IsAttached;
        }

        private async Task SetGpusInWindowTitle()
        {
            var gpus = await GpuUtils.GetCudaGpus();
            List<string> gpuNames = gpus.Select(x => x.FullName).ToList();
            int maxGpusToListInTitle = 2;

            if (gpuNames.Count < 1)
                Text = $"{Text} - No CUDA GPUs available.";
            else if (gpuNames.Count <= maxGpusToListInTitle)
                Text = $"{Text} - CUDA GPU{(gpuNames.Count != 1 ? "s" : "")}: {string.Join(", ", gpuNames)}";
            else
                Text = $"{Text} - CUDA GPUs: {string.Join(", ", gpuNames.Take(maxGpusToListInTitle))} (+{gpuNames.Count - maxGpusToListInTitle})";

            Logger.Log($"Detected {gpus.Count} CUDA-capable GPU{(gpus.Count != 1 ? "s" : "")}.");
        }

        private void LoadUiElements()
        {
            ConfigParser.LoadGuiElement(upDownIterations);
            ConfigParser.LoadGuiElement(sliderSteps, ConfigParser.SaveValueAs.Multiplied, 5); sliderSteps_Scroll(null, null);
            ConfigParser.LoadGuiElement(sliderScale, ConfigParser.SaveValueAs.Divided, 2f); sliderScale_Scroll(null, null);
            ConfigParser.LoadGuiElement(sliderResW, ConfigParser.SaveValueAs.Multiplied, 64); sliderResW_Scroll(null, null);
            ConfigParser.LoadGuiElement(sliderResH, ConfigParser.SaveValueAs.Multiplied, 64); sliderResH_Scroll(null, null);
            ConfigParser.LoadComboxIndex(comboxSampler);
            ConfigParser.LoadGuiElement(sliderInitStrength, ConfigParser.SaveValueAs.Divided, 40f); sliderInitStrength_Scroll(null, null);
        }

        private void SaveUiElements()
        {
            ConfigParser.SaveGuiElement(upDownIterations);
            ConfigParser.SaveGuiElement(sliderSteps, ConfigParser.SaveValueAs.Multiplied, 5);
            ConfigParser.SaveGuiElement(sliderScale, ConfigParser.SaveValueAs.Divided, 2f);
            ConfigParser.SaveGuiElement(sliderResW, ConfigParser.SaveValueAs.Multiplied, 64);
            ConfigParser.SaveGuiElement(sliderResH, ConfigParser.SaveValueAs.Multiplied, 64);
            ConfigParser.SaveComboxIndex(comboxSampler);
            ConfigParser.SaveGuiElement(sliderInitStrength, ConfigParser.SaveValueAs.Divided, 40f);
        }

        public void RefreshAfterSettingsChanged()
        {
            bool opt = Config.GetBool("checkboxOptimizedSd");

            btnEmbeddingBrowse.Visible = !opt; // Disable embedding browse btn when using optimizedSD
            panelSampler.Visible = !(File.Exists(MainUi.CurrentInitImgPath) || opt); // Disable sampler selection if using optimized mode or using img2img
            panelSeamless.Visible = !opt; // Disable seamless option when using optimizedSD

            bool adv = Config.GetBool("checkboxAdvancedMode");

            upDownIterations.Maximum = !adv ? 1000 : 10000;
            sliderSteps.Maximum = !adv ? 24 : 100;
            sliderScale.Maximum = !adv ? 50 : 100;
            sliderResW.Maximum = !adv ? 16 : 32;
            sliderResH.Maximum = !adv ? 16 : 32;
        }

        private void installerBtn_Click(object sender, EventArgs e)
        {
            new InstallerForm().ShowDialog();
        }

        public void CleanPrompt()
        {
            if (File.Exists(MainUi.CurrentEmbeddingPath) && Path.GetExtension(MainUi.CurrentEmbeddingPath).ToLower() == ".bin")
            {
                string conceptName = Path.GetFileNameWithoutExtension(MainUi.CurrentEmbeddingPath);
                textboxPrompt.Text = textboxPrompt.Text.Replace("*", $"<{conceptName.Trim()}>");
            }

            var lines = textboxPrompt.Text.SplitIntoLines();
            textboxPrompt.Text = string.Join(Environment.NewLine, lines.Select(x => MainUi.SanitizePrompt(x)));

            if (upDownSeed.Text == "")
                SetSeed();
        }

        public void SetSeed(long seed = -1)
        {
            upDownSeed.Value = seed;

            if(seed < 0)
                upDownSeed.Text = "";
        }

        public void LoadMetadataIntoUi (ImageMetadata meta)
        {
            textboxPrompt.Text = meta.Prompt;
            sliderSteps.Value = meta.Steps / 5; sliderSteps_Scroll(null, null);
            sliderScale.Value = (meta.Scale * 2f).RoundToInt(); sliderScale_Scroll(null, null);
            sliderResW.Value = meta.GeneratedResolution.Width / 64; sliderResW_Scroll(null, null);
            sliderResH.Value = meta.GeneratedResolution.Height / 64; sliderResH_Scroll(null, null);
            upDownSeed.Value = meta.Seed;
            comboxSampler.Text = meta.Sampler;
            MainUi.CurrentInitImgPath = meta.InitImgName;

            if (meta.InitStrength > 0f)
                sliderInitStrength.Value = (meta.InitStrength * 40f).RoundToInt().Clamp(sliderInitStrength.Minimum, sliderInitStrength.Maximum); sliderInitStrength_Scroll(null, null);

            UpdateInitImgAndEmbeddingUi();
        } 

        public void LoadTtiSettingsIntoUi(string[] prompts)
        {
            textboxPrompt.Text = string.Join(Environment.NewLine, prompts);
        }

        public void LoadTtiSettingsIntoUi(TtiSettings s)
        {
            textboxPrompt.Text = string.Join(Environment.NewLine, s.Prompts);
            upDownIterations.Value = s.Iterations;
            sliderSteps.Value = s.Params["steps"].GetInt() / 5; sliderSteps_Scroll(null, null);
            sliderScale.Value = (s.Params["scales"].Split(",")[0].GetFloat() * 2f).RoundToInt(); sliderScale_Scroll(null, null);
            sliderResW.Value = s.Params["res"].Split('x')[0].GetInt() / 64; sliderResW_Scroll(null, null);
            sliderResH.Value = s.Params["res"].Split('x')[1].GetInt() / 64; sliderResH_Scroll(null, null);
            upDownSeed.Value = s.Params["seed"].GetLong();
            comboxSampler.Text = s.Params["sampler"];
            MainUi.CurrentInitImgPath = s.Params["initImg"];
            sliderInitStrength.Value = (s.Params["initStrengths"].Split(",")[0].GetFloat() * 40f).RoundToInt(); sliderInitStrength_Scroll(null, null);
            MainUi.CurrentEmbeddingPath = s.Params["embedding"];
            checkboxSeamless.Checked = s.Params["seamless"] == true.ToString();
            checkboxInpainting.Checked = s.Params["inpainting"] == "masked";

            UpdateInitImgAndEmbeddingUi();
        }

        public TtiSettings GetCurrentTtiSettings()
        {
            TtiSettings settings = new TtiSettings
            {
                Implementation = Config.GetBool("checkboxOptimizedSd") ? Implementation.StableDiffusionOptimized : Implementation.StableDiffusion,
                Prompts = textboxPrompt.Text.SplitIntoLines(),
                Iterations = (int)upDownIterations.Value,
                Params = new Dictionary<string, string>
                        {
                            { "steps", MainUi.CurrentSteps.ToString() },
                            { "scales", string.Join(",", MainUi.GetScales(textboxExtraScales.Text).Select(x => x.ToStringDot("0.0000"))) },
                            { "res", $"{MainUi.CurrentResW}x{MainUi.CurrentResH}" },
                            { "seed", upDownSeed.Value < 0 ? (new Random().Next(0, Int32.MaxValue)).ToString() : ((long)upDownSeed.Value).ToString() },
                            { "sampler", comboxSampler.Text.Trim() },
                            { "initImg", MainUi.CurrentInitImgPath },
                            { "initStrengths", string.Join(",", MainUi.GetInitStrengths(textboxExtraInitStrengths.Text).Select(x => x.ToStringDot("0.0000"))) },
                            { "embedding", MainUi.CurrentEmbeddingPath },
                            { "seamless", checkboxSeamless.Checked.ToString() },
                            { "inpainting", checkboxInpainting.Checked ? "masked" : "" },
                            { "model", Config.Get(Config.Key.comboxSdModel) },
                        },
            };

            return settings;
        }

        private void runBtn_Click(object sender, EventArgs e)
        {
            if (Program.Busy)
            {
                TextToImage.CancelManually();
                return;
            }

            if (MainUi.Queue.Count > 0)
            {
                generateAllQueuedPromptsToolStripMenuItem.Text = $"Generate Queued Prompts ({MainUi.Queue.Count})";
                menuStripRunQueue.Show(Cursor.Position);
            }
            else
            {
                Run();
            }
        }

        public void Run(bool fromQueue = false)
        {
            try
            {
                if (Program.Busy)
                {
                    TextToImage.Cancel();
                    return;
                }
                else
                {
                    TextToImage.Canceled = false;

                    if (!MainUi.IsInstalledWithWarning())
                        return;

                    Logger.ClearLogBox();
                    CleanPrompt();
                    UpdateInitImgAndEmbeddingUi();
                    InpaintingUtils.DeleteMaskedImage();

                    if (fromQueue)
                    {
                        if (MainUi.Queue.Where(x => x != null).Count() < 0)
                        {
                            TextToImage.Cancel("Queue is empty.");
                            return;
                        }

                        TextToImage.RunTti(MainUi.Queue.AsEnumerable().Reverse().ToList()); // Reverse list to use top entries first
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(textboxPrompt.Text))
                        {
                            TextToImage.Cancel("No prompt was entered.");
                            return;
                        }

                        TextToImage.RunTti(GetCurrentTtiSettings());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"{ex.Message}\n{ex.StackTrace}");
            }
        }

        public void SetWorking(bool state, bool allowCancel = true)
        {
            Logger.Log($"SetWorking({state})", true);
            SetProgress(-1);
            runBtn.Text = state ? "Cancel" : "Generate!";
            runBtn.ForeColor = state ? Color.IndianRed : Color.White;
            Control[] controlsToDisable = new Control[] { };
            Control[] controlsToHide = new Control[] { };
            progressCircle.Visible = state;

            foreach (Control c in controlsToDisable)
                c.Enabled = !state;

            foreach (Control c in controlsToHide)
                c.Visible = !state;

            if (!state)
                SetProgressImg(0);

            progressBarImg.Visible = state;

            Program.Busy = state;
        }

        public void SetProgress(int percent, bool taskbarProgress = true)
        {
            percent = percent.Clamp(0, 100);
            progressBar.Value = percent;
            progressBar.Refresh();

            if (taskbarProgress)
                TaskbarManager.Instance.SetProgressValue(percent, 100);
        }

        public void SetProgressImg(int percent, bool taskbarProgress = false)
        {
            percent = percent.Clamp(0, 100);
            progressBarImg.Value = percent;
            progressBarImg.Refresh();

            if (taskbarProgress)
                TaskbarManager.Instance.SetProgressValue(percent, 100);
        }

        private void btnPrevImg_Click(object sender, EventArgs e)
        {
            ImagePreview.Move(true);
        }

        private void btnNextImg_Click(object sender, EventArgs e)
        {
            ImagePreview.Move(false);
        }

        #region Sliders

        private void sliderSteps_Scroll(object sender, ScrollEventArgs e)
        {
            int steps = sliderSteps.Value * 5;
            MainUi.CurrentSteps = steps;
            iterLabel.Text = steps.ToString();
        }

        private void sliderScale_Scroll(object sender, ScrollEventArgs e)
        {
            float scale = sliderScale.Value / 2f;
            MainUi.CurrentScale = scale;
            scaleLabel.Text = scale.ToString();
        }

        private void sliderResW_Scroll(object sender, ScrollEventArgs e)
        {
            int px = sliderResW.Value * 64;
            MainUi.CurrentResW = px;
            labelResW.Text = px.ToString();
        }

        private void sliderResH_Scroll(object sender, ScrollEventArgs e)
        {
            int px = sliderResH.Value * 64;
            MainUi.CurrentResH = px;
            labelResH.Text = px.ToString();
        }

        private void sliderInitStrength_Scroll(object sender, ScrollEventArgs e)
        {
            float strength = sliderInitStrength.Value / 40f;
            MainUi.CurrentInitImgStrength = strength;
            labelInitStrength.Text = strength.ToString("0.000");
        }

        #endregion

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
            Process.Start(ImagePreview.CurrentImagePath);
        }

        private void openOutputFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer", $@"/select, {ImagePreview.CurrentImagePath.Wrap()}");
        }

        private void copyImageToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OsUtils.SetClipboard(pictBoxImgViewer.Image);
        }

        private void copySeedToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OsUtils.SetClipboard(ImagePreview.CurrentImageMetadata.Seed.ToString());
        }

        private void useAsInitImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.Busy)
            {
                UiUtils.ShowMessageBox("Please wait until the generation has finished.");
                return;
            }

            MainUi.HandleDroppedFiles(new string[] { ImagePreview.CurrentImagePath });
        }

        #endregion

        private void cliButton_Click(object sender, EventArgs e)
        {
            menuStripDevTools.Show(Cursor.Position);
        }

        private void pictBoxImgViewer_Click(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Right)
            {
                if (!string.IsNullOrWhiteSpace(ImagePreview.CurrentImagePath) && File.Exists(ImagePreview.CurrentImagePath))
                    menuStripOutputImg.Show(Cursor.Position);
            }
            else
            {
                if (pictBoxImgViewer.Image != null)
                    ImagePopup.Show(pictBoxImgViewer.Image, ImagePopupForm.SizeMode.Percent100);
            }
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
            if (Program.Busy)
                return;

            if (!string.IsNullOrWhiteSpace(MainUi.CurrentInitImgPath))
            {
                MainUi.CurrentInitImgPath = "";
            }
            else
            {
                CommonOpenFileDialog dialog = new CommonOpenFileDialog { InitialDirectory = MainUi.CurrentInitImgPath.GetParentDirOfFile(), IsFolderPicker = false };

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    if (MainUi.ValidInitImgExtensions.Contains(Path.GetExtension(dialog.FileName).ToLower()))
                        MainUi.CurrentInitImgPath = dialog.FileName;
                    else
                        UiUtils.ShowMessageBox("Invalid file type.");
                }
            }

            UpdateInitImgAndEmbeddingUi();
        }

        public void UpdateInitImgAndEmbeddingUi()
        {
            if (!string.IsNullOrWhiteSpace(MainUi.CurrentInitImgPath) && !File.Exists(MainUi.CurrentInitImgPath))
            {
                MainUi.CurrentInitImgPath = "";
                Logger.Log($"Initialization image was cleared because the file no longer exists.");
            }

            if (!string.IsNullOrWhiteSpace(MainUi.CurrentEmbeddingPath) && !File.Exists(MainUi.CurrentEmbeddingPath))
            {
                MainUi.CurrentEmbeddingPath = "";
                Logger.Log($"Concept was cleared because the file no longer exists.");
            }

            bool imgExists = File.Exists(MainUi.CurrentInitImgPath);
            panelInpainting.Visible = imgExists;
            panelInitImgStrength.Visible = imgExists;
            btnInitImgBrowse.Text = imgExists ? "Clear Image" : "Load Image";

            bool embeddingExists = File.Exists(MainUi.CurrentEmbeddingPath);
            btnEmbeddingBrowse.Text = embeddingExists ? "Clear Concept" : "Load Concept";

            labelCurrentImage.Text = string.IsNullOrWhiteSpace(MainUi.CurrentInitImgPath) ? "No initialization image loaded." : $"Currently using {Path.GetFileName(MainUi.CurrentInitImgPath).Trunc(30)}";
            labelCurrentConcept.Text = string.IsNullOrWhiteSpace(MainUi.CurrentEmbeddingPath) ? "No trained concept loaded." : $"Currently using {Path.GetFileName(MainUi.CurrentEmbeddingPath).Trunc(30)}";

            RefreshAfterSettingsChanged();
        }

        private void btnEmbeddingBrowse_Click(object sender, EventArgs e)
        {
            if (Program.Busy)
                return;

            if (Config.GetBool("checkboxOptimizedSd"))
            {
                Logger.Log("Not supported in Low Memory Mode.");
                return;
            }

            if (!string.IsNullOrWhiteSpace(MainUi.CurrentEmbeddingPath))
            {
                MainUi.CurrentEmbeddingPath = "";
            }
            else
            {
                string initDir = File.Exists(MainUi.CurrentEmbeddingPath) ? MainUi.CurrentEmbeddingPath.GetParentDirOfFile() : Path.Combine(Paths.GetExeDir(), "ExampleConcepts");

                Logger.Log(initDir);

                CommonOpenFileDialog dialog = new CommonOpenFileDialog { InitialDirectory = initDir, IsFolderPicker = false };

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    if (MainUi.ValidInitEmbeddingExtensions.Contains(Path.GetExtension(dialog.FileName.ToLower())))
                        MainUi.CurrentEmbeddingPath = dialog.FileName;
                    else
                        UiUtils.ShowMessageBox("Invalid file type.");
                }
            }

            UpdateInitImgAndEmbeddingUi();
        }

        #endregion

        private void btnDebug_Click(object sender, EventArgs e)
        {
            menuStripLogs.Items.Clear();
            var openLogs = menuStripLogs.Items.Add($"Open Logs Folder");
            openLogs.Click += (s, ea) => { Process.Start("explorer", Paths.GetLogPath().Wrap()); };

            foreach (var log in Logger.SessionLogs)
            {
                ToolStripItem newItem = menuStripLogs.Items.Add($"Copy {log.Key}");
                newItem.Click += (s, ea) => { OsUtils.SetClipboard(Logger.SessionLogs[log.Key]); };
            }

            menuStripLogs.Show(Cursor.Position);
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            new SettingsForm().ShowDialog();
        }

        private void btnPostProc_Click(object sender, EventArgs e)
        {
            if (Config.GetBool("checkboxOptimizedSd"))
            {
                UiUtils.ShowMessageBox("Post-processing is not available when using Low Memory Mode.");
                return;
            }

            new PostProcSettingsForm().ShowDialog();
        }

        private void btnExpandPromptField_Click(object sender, EventArgs e)
        {
            MainUi.SetPromptFieldSize(MainUi.PromptFieldSizeMode.Toggle);
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
            new PromptListForm(PromptListForm.ListMode.History).ShowDialog();
        }

        private void btnQueue_Click(object sender, EventArgs e)
        {
            if (Program.Busy)
                return;

            new PromptListForm(PromptListForm.ListMode.Queue).ShowDialog();
        }

        private void generateCurrentPromptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Run();
        }

        private void generateAllQueuedPromptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Run(true);
        }

        public void UpdateInpaintUi()
        {
            btnResetMask.Visible = InpaintingUtils.CurrentMask != null;
        }

        private void btnResetMask_Click(object sender, EventArgs e)
        {
            InpaintingUtils.CurrentMask = null;
        }

        private void pictBoxImgViewer_MouseWheel(object sender, MouseEventArgs e)
        {
            ImagePreview.Move(e.Delta > 0);
        }

        private void textboxCliTest_DoubleClick(object sender, EventArgs e)
        {
            TtiProcess.WriteStdIn(textboxCliTest.Text);
            textboxCliTest.Text = "";
        }

        private void addCurrentSettingsToQueueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var settings = GetCurrentTtiSettings();

            if (settings.Prompts.Where(x => !string.IsNullOrWhiteSpace(x)).Any())
                MainUi.Queue.Add(settings);
        }

        private void btnQueue_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                menuStripAddToQueue.Show(Cursor.Position);
        }

        private void reGenerateImageWithCurrentSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.Busy)
            {
                UiUtils.ShowMessageBox("Please wait until the current process has finished.");
                return;
            }

            var prevSeedVal = upDownSeed.Value;
            var prevIterVal = upDownIterations.Value;
            upDownSeed.Value = ImagePreview.CurrentImageMetadata.Seed;
            upDownIterations.Value = 1;
            runBtn_Click(null, null);
            SetSeed((long)prevSeedVal);
            upDownIterations.Value = prevIterVal;
        }

        private void textboxPrompt_MouseWheel(object sender, MouseEventArgs e)
        {
            int sizeChange = e.Delta > 0 ? 1 : -1;
            textboxPrompt.Font = new Font(textboxPrompt.Font.Name, (textboxPrompt.Font.Size + sizeChange).Clamp(_defaultPromptFontSize, _defaultPromptFontSize * 2f), textboxPrompt.Font.Style, textboxPrompt.Font.Unit);
        }

        private void MainForm_ResizeBegin(object sender, EventArgs e)
        {
            MainUi.SetPromptFieldSize(MainUi.PromptFieldSizeMode.Collapse);
        }

        private void btnDeleteBatch_Click(object sender, EventArgs e)
        {
            menuStripDeleteImages.Show(Cursor.Position);
        }

        private void deleteThisImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImagePreview.DeleteCurrent();
        }

        private void deleteAllCurrentImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImagePreview.DeleteAll();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.V))
            {
                try
                {
                    Image clipboardImg = Clipboard.GetImage();

                    if (clipboardImg == null)
                        return;

                    string savePath = Path.Combine(Paths.GetSessionDataPath(), "clipboard.png");
                    clipboardImg.Save(savePath);
                    MainUi.HandleDroppedFiles(new string[] { savePath });
                }
                catch (Exception ex)
                {
                    Logger.Log($"Failed to paste image from clipboard: {ex.Message}\n{ex.StackTrace}", true);
                }
            }
        }

        private void openDreampyCLIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.Busy || !MainUi.IsInstalledWithWarning())
                return;

            TtiProcess.RunStableDiffusionCli(Config.Get(Config.Key.textboxOutPath));
        }

        private void openModelMergeToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new MergeModelsForm().ShowDialog();
        }
    }
}
