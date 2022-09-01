using StableDiffusionGui.Io;
using StableDiffusionGui.Ui;
using System;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class PostProcSettingsForm : Form
    {
        public PostProcSettingsForm()
        {
            InitializeComponent();
        }

        private void PostProcSettingsForm_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void PostProcSettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        void LoadSettings()
        {
            ConfigParser.LoadComboxIndex(comboxUpscale);
            ConfigParser.LoadGuiElement(sliderGfpgan); sliderGfpgan_Scroll(null, null);
        }

        void SaveSettings()
        {
            ConfigParser.SaveComboxIndex(comboxUpscale);
            ConfigParser.SaveGuiElement(sliderGfpgan);
        }

        private void sliderGfpgan_Scroll(object sender, ScrollEventArgs e)
        {
            float strength = sliderGfpgan.Value / 20f;
            PostProcUi.CurrentGfpganStrength = strength;
            labelGfpStrength.Text = strength.ToString();
        }
    }
}
