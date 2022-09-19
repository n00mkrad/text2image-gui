using HTAlt.WinForms;
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
using TextBox = System.Windows.Forms.TextBox;
using StableDiffusionGui.Os;
using Microsoft.VisualBasic.Logging;
using System.Reflection;
using StableDiffusionGui.Properties;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ImageMagick;
using Paths = StableDiffusionGui.Io.Paths;
using StableDiffusionGui.MiscUtils;

namespace StableDiffusionGui
{
    public partial class MainForm : Form
    {
        [Flags]
        public enum EXECUTION_STATE : uint { ES_AWAYMODE_REQUIRED = 0x00000040, ES_CONTINUOUS = 0x80000000, ES_DISPLAY_REQUIRED = 0x00000002, ES_SYSTEM_REQUIRED = 0x00000001 }
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags); // This should prevent Windows from going to sleep

        public PictureBox PictBoxImgViewer { get { return pictBoxImgViewer; } }
        public Label OutputImgLabel { get { return outputImgLabel; } }

        public bool IsInFocus() { return (ActiveForm == this); }

        public MainForm()
        {
            InitializeComponent();
            Program.MainForm = this;
            pictBoxImgViewer.MouseWheel += PictBoxImgViewer_MouseWheel;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Logger.Textbox = logBox;
            LoadUiElements();
            PromptHistory.Load();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveUiElements();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Setup.FixHardcodedPaths();
            Task.Run(() => SetGpusInWindowTitle());
            upDownSeed.Text = "";
            MainUi.DoStartupChecks();
            RefreshAfterSettingsChanged();

            if (!Debugger.IsAttached)
                new WelcomeForm().ShowDialog();
        }

        private async Task SetGpusInWindowTitle()
        {
            var gpus = await GpuUtils.GetCudaGpus();
            List<string> gpuNames = gpus.Select(x => x.FullName).ToList();
            int maxGpusToListInTitle = 2;

            if(gpuNames.Count < 1)
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
            ConfigParser.LoadGuiElement(sliderSteps); sliderSteps_Scroll(null, null);
            ConfigParser.LoadGuiElement(sliderScale); sliderScale_Scroll(null, null);
            ConfigParser.LoadGuiElement(sliderResW); sliderResW_Scroll(null, null);
            ConfigParser.LoadGuiElement(sliderResH); sliderResH_Scroll(null, null);
            ConfigParser.LoadComboxIndex(comboxSampler);
            ConfigParser.LoadGuiElement(sliderInitStrength); sliderInitStrength_Scroll(null, null);
        }

        private void SaveUiElements()
        {
            ConfigParser.SaveGuiElement(upDownIterations);
            ConfigParser.SaveGuiElement(sliderSteps);
            ConfigParser.SaveGuiElement(sliderScale);
            ConfigParser.SaveGuiElement(sliderResW);
            ConfigParser.SaveGuiElement(sliderResH);
            ConfigParser.SaveComboxIndex(comboxSampler);
            ConfigParser.SaveGuiElement(sliderInitStrength);
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
            sliderResW.Maximum = !adv ? 16 : 24;
            sliderResH.Maximum = !adv ? 16 : 24;
        }

        private void installerBtn_Click(object sender, EventArgs e)
        {
            new InstallerForm().ShowDialog();
        }

        public void CleanPrompt()
        {
            if (File.Exists(MainUi.CurrentEmbeddingPath))
            {
                string conceptName = Path.GetFileNameWithoutExtension(MainUi.CurrentEmbeddingPath);
                textboxPrompt.Text = textboxPrompt.Text.Replace("*", $"<{conceptName.Trim()}>");
            }

            var lines = textboxPrompt.Text.SplitIntoLines();
            textboxPrompt.Text = string.Join(Environment.NewLine, lines.Select(x => MainUi.SanitizePrompt(x)).Where(x => !string.IsNullOrWhiteSpace(x)));

            if (upDownSeed.Text == "")
            {
                upDownSeed.Value = -1;
                upDownSeed.Text = "";
            }
        }

        public void LoadTtiSettingsIntoUi(string[] prompts)
        {
            textboxPrompt.Text = String.Join(Environment.NewLine, prompts);
        }

        public void LoadTtiSettingsIntoUi(TtiSettings s)
        {
            textboxPrompt.Text = String.Join(Environment.NewLine, s.Prompts);
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
                            { "scales", String.Join(",", MainUi.GetScales(textboxExtraScales.Text).Select(x => x.ToStringDot("0.0000"))) },
                            { "res", $"{MainUi.CurrentResW}x{MainUi.CurrentResH}" },
                            { "seed", upDownSeed.Value < 0 ? (new Random().Next(0, Int32.MaxValue)).ToString() : ((long)upDownSeed.Value).ToString() },
                            { "sampler", comboxSampler.Text.Trim() },
                            { "initImg", MainUi.CurrentInitImgPath },
                            { "initStrengths", String.Join(",", MainUi.GetInitStrengths(textboxExtraInitStrengths.Text).Select(x => x.ToStringDot("0.0000"))) },
                            { "embedding", MainUi.CurrentEmbeddingPath },
                            { "seamless", checkboxSeamless.Checked.ToString() },
                            { "inpainting", checkboxInpainting.Checked ? "masked" : "" },
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
            if (Program.Busy || !MainUi.IsInstalledWithWarning())
                return;

            TtiProcess.RunStableDiffusionCli(Config.Get(Config.Key.textboxOutPath));
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
                    new BigPreviewForm(pictBoxImgViewer.Image, true, checkboxSeamless.Checked).Show();
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
                Logger.Log($"Init image was cleared because the file no longer exists.");
            }

            if (!string.IsNullOrWhiteSpace(MainUi.CurrentEmbeddingPath) && !File.Exists(MainUi.CurrentEmbeddingPath))
            {
                MainUi.CurrentEmbeddingPath = "";
                Logger.Log($"Embedding was cleared because the file no longer exists.");
            }

            bool imgExists = File.Exists(MainUi.CurrentInitImgPath);
            panelInpainting.Visible = imgExists;
            panelInitImgStrength.Visible = imgExists;
            btnInitImgBrowse.Text = imgExists ? "Clear Image" : "Load Image";

            bool embeddingExists = File.Exists(MainUi.CurrentEmbeddingPath);
            btnEmbeddingBrowse.Text = embeddingExists ? "Clear Concept" : "Load Concept";

            if (!string.IsNullOrWhiteSpace(MainUi.CurrentInitImgPath) && !string.IsNullOrWhiteSpace(MainUi.CurrentEmbeddingPath))
            {
                labelPromptInfo.Text = $"With {Path.GetFileName(MainUi.CurrentInitImgPath).Trunc(28)}\nWith {Path.GetFileName(MainUi.CurrentEmbeddingPath).Trunc(28)}";
            }
            else if (!string.IsNullOrWhiteSpace(MainUi.CurrentInitImgPath))
            {
                labelPromptInfo.Text = $"With {Path.GetFileName(MainUi.CurrentInitImgPath).Trunc(28)}";
            }
            else if (!string.IsNullOrWhiteSpace(MainUi.CurrentEmbeddingPath))
            {
                labelPromptInfo.Text = $"With {Path.GetFileName(MainUi.CurrentEmbeddingPath).Trunc(28)}";
            }
            else
            {
                labelPromptInfo.Text = "";
            }

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
            new PostProcSettingsForm().ShowDialog();
        }

        private void btnExpandPromptField_Click(object sender, EventArgs e)
        {
            int smallHeight = 65;

            if (panelPrompt.Height == smallHeight)
            {
                btnExpandPromptField.BackgroundImage = Resources.upArrowIcon;
                panelPrompt.Height = 130;
            }
            else
            {
                btnExpandPromptField.BackgroundImage = Resources.downArrowIcon;
                panelPrompt.Height = smallHeight;
            }
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
            if (Program.Busy)
                return;

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

        private void PictBoxImgViewer_MouseWheel(object sender, MouseEventArgs e)
        {
            ImagePreview.Move(e.Delta > 0);
        }
    }
}
