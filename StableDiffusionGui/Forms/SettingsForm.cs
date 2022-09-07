using Microsoft.WindowsAPICodePack.Dialogs;
using StableDiffusionGui.Io;
using StableDiffusionGui.Ui;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            comboxSdModel.Items.Clear();
            IoUtils.GetFileInfosSorted(Paths.GetModelsPath(), true, "*.ckpt").ToList().ForEach(x => comboxSdModel.Items.Add(x.Name));
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
            ConfigParser.LoadGuiElement(checkboxMultiPromptsSameSeed);
            ConfigParser.LoadGuiElement(checkboxPromptInFilename);
            ConfigParser.LoadGuiElement(textboxOutPath);
            ConfigParser.LoadGuiElement(comboxSdModel);
        }

        void SaveSettings()
        {
            ConfigParser.SaveGuiElement(checkboxOptimizedSd);
            ConfigParser.SaveGuiElement(checkboxFullPrecision);
            ConfigParser.SaveGuiElement(checkboxFolderPerPrompt);
            ConfigParser.SaveGuiElement(checkboxAdvancedMode);
            ConfigParser.SaveGuiElement(checkboxMultiPromptsSameSeed);
            ConfigParser.SaveGuiElement(checkboxPromptInFilename);
            ConfigParser.SaveGuiElement(textboxOutPath);
            ConfigParser.SaveGuiElement(comboxSdModel);
        }

        private void checkboxFolderPerPrompt_CheckedChanged(object sender, EventArgs e)
        {
            panelPromptInFilename.Visible = !checkboxFolderPerPrompt.Checked;
        }

        private void btnOutPathBrowse_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { InitialDirectory = textboxOutPath.Text, IsFolderPicker = true };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                textboxOutPath.Text = dialog.FileName;
        }

        private void btnOpenModelsFolder_Click(object sender, EventArgs e)
        {
            Process.Start("explorer", Paths.GetModelsPath().Wrap());
        }
    }
}
