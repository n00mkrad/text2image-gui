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

namespace StableDiffusionGui
{
    public partial class MainForm : Form
    {
        public Cyotek.Windows.Forms.ImageBox ImgBoxOutput { get { return resultImgBox; } }
        public Label OutputImgLabel { get { return outputImgLabel; } }

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

        private void LoadUiElements()
        {
            ConfigParser.LoadGuiElement(upDownIterations);
            ConfigParser.LoadGuiElement(sliderSteps); sliderSteps_Scroll(null, null);
            ConfigParser.LoadGuiElement(sliderScale); sliderScale_Scroll(null, null);
        }

        private void SaveUiElements()
        {
            ConfigParser.SaveGuiElement(upDownIterations);
            ConfigParser.SaveGuiElement(sliderSteps);
            ConfigParser.SaveGuiElement(sliderScale);
        }

        private void installerBtn_Click(object sender, EventArgs e)
        {
            new InstallerForm().ShowDialog();
        }

        private void runBtn_Click(object sender, EventArgs e)
        {
            TextToImage.RunStableDiffusion(textboxPrompt.Text, (int)upDownIterations.Value, MainUi.CurrentSteps, MainUi.CurrentScale, new Size(512, 512), Path.Combine(Paths.GetExeDir(), "out"));
        }

        public void ShowImages (List<Image> imgs)
        {

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
    }
}
