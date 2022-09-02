using StableDiffusionGui.Io;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
            Program.MainForm.RefreshAfterSettingsChanged();
        }

        void LoadSettings()
        {
            ConfigParser.LoadGuiElement(checkboxOptimizedSd);
            ConfigParser.LoadGuiElement(checkboxFullPrecision);
            ConfigParser.LoadGuiElement(checkboxFolderPerPrompt);
            ConfigParser.LoadGuiElement(checkboxAdvancedMode);
            ConfigParser.LoadGuiElement(checkboxPromptInFilename);
        }

        void SaveSettings()
        {
            ConfigParser.SaveGuiElement(checkboxOptimizedSd);
            ConfigParser.SaveGuiElement(checkboxFullPrecision);
            ConfigParser.SaveGuiElement(checkboxFolderPerPrompt);
            ConfigParser.SaveGuiElement(checkboxAdvancedMode);
            ConfigParser.SaveGuiElement(checkboxPromptInFilename);
        }

        private void checkboxFolderPerPrompt_CheckedChanged(object sender, EventArgs e)
        {
            panelPromptInFilename.Visible = !checkboxFolderPerPrompt.Checked;
        }
    }
}
