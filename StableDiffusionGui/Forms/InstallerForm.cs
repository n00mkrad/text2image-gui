using StableDiffusionGui.Installation;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Ui;
using System;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class InstallerForm : CustomForm
    {
        public InstallerForm()
        {
            InitializeComponent();
        }

        private void InstallerForm_Load(object sender, EventArgs e)
        {
            Enabled = false;
        }

        private async void installBtn_Click(object sender, EventArgs e)
        {
            Enabled = false;

            if (InstallationStatus.IsInstalledAll) // Re-install
            {
                await Setup.Install(true, InstallationStatus.HasOnnx(), InstallationStatus.HasSdUpscalers());
            }
            else
            {
                bool installOnnxDml = AskInstallOnnxDml();
                bool installUpscalers = AskInstallUpscalers();
                await Setup.Install(false, installOnnxDml, installUpscalers);
            }
            
            BringToFront();
            UpdateStatus();
            Enabled = true;
        }

        private void InstallerForm_Shown(object sender, EventArgs e)
        {
            Refresh();
            UpdateStatus();
            Enabled = true;
        }

        public void UpdateStatus ()
        {
            for (int i = 0; i < checkedListBoxStatus.Items.Count; i++)
            {
                string text = checkedListBoxStatus.Items[i].ToString().Lower();

                if (text.Contains("binaries"))
                    checkedListBoxStatus.SetItemChecked(i, InstallationStatus.HasBins());

                if (text.Contains("env"))
                    checkedListBoxStatus.SetItemChecked(i, InstallationStatus.HasSdEnv());

                if (text.Contains("code"))
                    checkedListBoxStatus.SetItemChecked(i, InstallationStatus.HasSdRepo());

                //if (text.Contains("model"))
                //    checkedListBoxStatus.SetItemChecked(i, InstallationStatus.HasSdModel());

                if (text.Contains("upscalers"))
                    checkedListBoxStatus.SetItemChecked(i, InstallationStatus.HasSdUpscalers());
            }

            if (InstallationStatus.IsInstalledAll)
                btnInstall.Text = "Re-Install";
            else
                btnInstall.Text = "Install";

            btnUninstall.Enabled = checkedListBoxStatus.CheckedItems.Count > 1;
        }

        private async void btnUninstall_Click(object sender, EventArgs e)
        {
            Enabled = false;
            Logger.Log("Uninstalling...");
            await Setup.RemoveRepo();
            await Setup.RemoveEnv();
            UpdateStatus();
            Logger.Log("Done.");
            Enabled = true;
        }

        private async void btnClone_Click(object sender, EventArgs e)
        {
            string commit = "";

            if (InputUtils.IsHoldingShift)
            {
                var form = new PromptForm("Clone Specific Commit", "Enter a commit hash (SHA) to clone.", "");
                form.ShowDialog();
                commit = form.EnteredText.Trim();
            }

            bool installOnnxDml = AskInstallOnnxDml();
            Enabled = false;
            Program.SetState(Program.BusyState.Installation);
            await Setup.InstallRepo(installOnnxDml, commit, false);
            Setup.RepoCleanup();
            UpdateStatus();
            Program.SetState(Program.BusyState.Standby);
            Enabled = true;
        }

        private async void btnInstallUpscalers_Click(object sender, EventArgs e)
        {
            Enabled = false;
            await Setup.InstallUpscalers();
            UpdateStatus();
            Enabled = true;
        }

        private bool AskInstallOnnxDml ()
        {
            DialogResult res = UiUtils.ShowMessageBox("Do you want to download the Stable Diffusion ONNX/DirectML files?\n\nThey are only needed if you have an AMD GPU.", "Setup", MessageBoxButtons.YesNo);
            return res == DialogResult.Yes;
        }

        private bool AskInstallUpscalers()
        {
            DialogResult res = UiUtils.ShowMessageBox("Do you want to pre-download the upscaling and face restoration models? (800 MB)", "Setup", MessageBoxButtons.YesNo);
            return res == DialogResult.Yes;
        }

        private void InstallerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Ui.MainFormUtils.FormControls.RefreshUiAfterSettingsChanged();
        }
    }
}
