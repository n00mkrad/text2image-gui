using HTAlt.WinForms;
using StableDiffusionGui.Forms;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Taskbar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text.RegularExpressions;
using Microsoft.WindowsAPICodePack.Dialogs;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Data;
using TextBox = System.Windows.Forms.TextBox;
using StableDiffusionGui.Os;

namespace StableDiffusionGui
{
    public partial class MainForm : Form
    {
        public Cyotek.Windows.Forms.ImageBox ImgBoxOutput { get { return imgBoxOutput; } }
        public Label OutputImgLabel { get { return outputImgLabel; } }
        public System.Windows.Forms.Button BtnImgShare { get { return btnImgShare; } }

        public bool IsInFocus() { return (ActiveForm == this); }

        public MainForm()
        {
            InitializeComponent();
            Program.MainForm = this;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Logger.Textbox = logBox;
            LoadUiElements();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveUiElements();
            Program.Cleanup();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (!Debugger.IsAttached)
            {
                int pathLength = Paths.GetExeDir().Length;

                if (pathLength > 70)
                    UiUtils.ShowMessageBox($"You are running the program from this path:\n\n{Paths.GetExeDir()}\n\nIt's very long ({pathLength} characters), this can cause problems.\n" +
                        $"Please move the program to a shorter path or continue at your own risk.", UiUtils.MessageType.Warning, Nmkoder.Forms.MessageForm.FontSize.Big);

                UiUtils.ShowMessageBox("READ THIS FIRST!\n\nThis software is still in development and may contain bugs.\n\nImportant:\n" +
                "- You MUST have a recent (GTX 10 series or newer) Nvidia graphics card to use this.\n" +
                "- You need as much VRAM (graphics card memory) as possible. IF YOU HAVE LESS THAN 8 GB, use this at your own risk, it might not work at all!!\n" +
                "- The resolution settings is very VRAM-heavy. I do not recommend going above 512x512 unless you have 12+ GB VRAM.\n\n" +
                "Last but not least, this GUI includes tooltips, so if you're not sure what a button or other control does, hover over it with your cursor and an info message will pop up.\n\nHave fun!", UiUtils.MessageType.Warning, Nmkoder.Forms.MessageForm.FontSize.Big);
            }
            else
            {
                Logger.Log("Debugger is attached.");
            }
            
            if (!InstallationStatus.IsInstalled)
            {
                UiUtils.ShowMessageBox("No complete installation of the Stable Diffusion files was found.\n\nThe GUI will now open the installer.\nPlease press \"Install\" in the next window to install all required files.");
                installerBtn_Click(null, null);
            }
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

        private void installerBtn_Click(object sender, EventArgs e)
        {
            new InstallerForm().ShowDialog();
        }

        public void CleanPrompt()
        {
            textboxPrompt.Text = MainUi.SanitizePrompt(textboxPrompt.Text);

            if (upDownSeed.Text == "")
                upDownSeed.Value = -1;
        }


        public bool IsInstalledWithWarning(bool showInstaller = true)
        {
            if (!InstallationStatus.IsInstalled)
            {
                UiUtils.ShowMessageBox("A valid installation is required.");

                if (showInstaller)
                    installerBtn_Click(null, null);

                return false;
            }

            return true;
        }

        private void runBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (Program.Busy)
                {
                    TextToImage.Cancel();
                }
                else
                {
                    TextToImage.Canceled = false;

                    if (!IsInstalledWithWarning())
                        return;

                    if (string.IsNullOrWhiteSpace(textboxPrompt.Text))
                        TextToImage.Cancel("No prompt was entered.");

                    if (TextToImage.Canceled)
                        return;

                    Logger.ClearLogBox();
                    CleanPrompt();

                    TtiSettings settings = new TtiSettings
                    {
                        Implementation = Implementation.StableDiffusion,
                        Prompts = new string[] { textboxPrompt.Text },
                        Iterations = (int)upDownIterations.Value,
                        OutPath = Path.Combine(Paths.GetExeDir(), "out"),
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
                        },
                    };

                    TextToImage.RunTti(settings);
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

            Program.Busy = state;
        }

        public void SetProgress(int percent)
        {
            percent = percent.Clamp(0, 100);
            TaskbarManager.Instance.SetProgressValue(percent, 100);
            progressBar.Value = percent;
            progressBar.Refresh();
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
            Process.Start("explorer", Path.Combine(Paths.GetExeDir(), "out"));
        }

        #region Link Buttons

        private void paypalBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.paypal.com/paypalme/nmkd/10");
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
            OsUtils.SetClipboard(imgBoxOutput.Image);
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

        private void btnImgShare_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ImagePreview.CurrentImagePath) && File.Exists(ImagePreview.CurrentImagePath))
                menuStripOutputImg.Show(Cursor.Position);
        }

        private void cliButton_Click(object sender, EventArgs e)
        {
            if (!IsInstalledWithWarning())
                return;

            TtiProcess.RunStableDiffusionCli(Path.Combine(Paths.GetExeDir(), "out"));
        }

        private void imgBoxOutput_Click(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Right)
                btnImgShare_Click(null, null);
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
            if (!string.IsNullOrWhiteSpace(MainUi.CurrentInitImgPath))
            {
                MainUi.CurrentInitImgPath = "";
            }
            else
            {
                CommonOpenFileDialog dialog = new CommonOpenFileDialog { InitialDirectory = MainUi.CurrentInitImgPath.GetParentDirOfFile(), IsFolderPicker = false };

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    MainUi.CurrentInitImgPath = dialog.FileName;
            }

            UpdateInitImgAndEmbeddingUi();
        }

        public void UpdateInitImgAndEmbeddingUi ()
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
            panelInitImgStrength.Visible = imgExists;
            btnInitImgBrowse.Text = imgExists ? "Clear Image" : "Load Image";

            bool embeddingExists = File.Exists(MainUi.CurrentEmbeddingPath);
            btnEmbeddingBrowse.Text = embeddingExists ? "Clear Embedding" : "Load Embedding";

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
        }

        private void textboxInitImgPath_TextChanged(object sender, EventArgs e)
        {
            UpdateInitImgAndEmbeddingUi();
        }

        private void btnEmbeddingBrowse_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(MainUi.CurrentEmbeddingPath))
            {
                MainUi.CurrentEmbeddingPath = "";
            }
            else
            {
                CommonOpenFileDialog dialog = new CommonOpenFileDialog { InitialDirectory = MainUi.CurrentEmbeddingPath.GetParentDirOfFile(), IsFolderPicker = false };

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    MainUi.CurrentEmbeddingPath = dialog.FileName;
            }

            UpdateInitImgAndEmbeddingUi();
        }

        #endregion

        private void btnDebug_Click(object sender, EventArgs e)
        {
            Process.Start("explorer", Paths.GetLogPath().Wrap());
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            new SettingsForm().ShowDialog();
        }
    }
}
