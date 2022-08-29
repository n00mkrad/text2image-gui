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
            if (!InstallationStatus.IsInstalled)
            {
                UiUtils.ShowMessageBox("No complete installation of the Stable Diffusion files was found.\n\nThe GUI will now open the installer. Please press \"Install\" in the next window to install all required files.");
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
        }

        private void SaveUiElements()
        {
            ConfigParser.SaveGuiElement(upDownIterations);
            ConfigParser.SaveGuiElement(sliderSteps);
            ConfigParser.SaveGuiElement(sliderScale);
            ConfigParser.SaveGuiElement(sliderResW);
            ConfigParser.SaveGuiElement(sliderResH);
            ConfigParser.SaveComboxIndex(comboxSampler);
        }

        private void installerBtn_Click(object sender, EventArgs e)
        {
            new InstallerForm().ShowDialog();
        }

        public void CleanPrompt()
        {
            textboxPrompt.Text = new Regex(@"[^a-zA-Z0-9 -!,.:()\-]").Replace(textboxPrompt.Text, "");
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
            if (!IsInstalledWithWarning())
                return;

            try
            {
                if (Program.Busy)
                {
                    // cancel...
                }
                else
                {
                    CleanPrompt();

                    List<float> scales = new List<float> { MainUi.CurrentScale };
                    scales.AddRange(textboxExtraScales.Text.Replace(" ", "").Split(",").Select(x => x.GetFloat()).Where(x => x > 0.05f));

                    TextToImage.TtiSettings settings = new TextToImage.TtiSettings
                    {
                        Implementation = TextToImage.Implementation.StableDiffusion,
                        Prompts = new string[] { textboxPrompt.Text },
                        Iterations = (int)upDownIterations.Value,
                        OutPath = Path.Combine(Paths.GetExeDir(), "out"),
                        Params = new Dictionary<string, string>
                        {
                            { "steps", MainUi.CurrentSteps.ToString() },
                            { "scales", String.Join(",", scales.Select(x => x.ToStringDot())) },
                            { "res", $"{MainUi.CurrentResW}x{MainUi.CurrentResH}" },
                            { "seed", upDownSeed.Value < 0 ? (new Random().Next(0, 2000000000)).ToString() : ((int)upDownSeed.Value).ToString() },
                            { "sampler", comboxSampler.Text.Trim() },
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

        private void btnOpenOutFolder_Click(object sender, EventArgs e)
        {
            Process.Start("explorer", Path.Combine(Paths.GetExeDir(), "out"));
        }

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
            Process.Start("https://discord.gg/eJHD2NSJRe");
        }

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
            Clipboard.SetDataObject(imgBoxOutput.Image);
        }

        private void btnImgShare_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ImagePreview.CurrentImagePath) && File.Exists(ImagePreview.CurrentImagePath))
                menuStripOutputImg.Show(Cursor.Position);
        }

        private void cliButton_Click(object sender, EventArgs e)
        {
            if (!IsInstalledWithWarning())
                return;

            TextToImage.RunStableDiffusionCli(Path.Combine(Paths.GetExeDir(), "out"));
        }

        private void copySeedToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(ImagePreview.CurrentImageMetadata.Seed.ToString());
        }

        private void imgBoxOutput_Click(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Right)
                btnImgShare_Click(null, null);
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            MainUi.HandleDroppedFiles((string[])e.Data.GetData(DataFormats.FileDrop));
        }
    }
}
