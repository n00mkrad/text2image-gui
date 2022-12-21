using StableDiffusionGui.Installation;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
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
            await Setup.Install(InstallationStatus.IsInstalledAll);
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

                if (text.Contains("model"))
                    checkedListBoxStatus.SetItemChecked(i, InstallationStatus.HasSdModel());

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

            Enabled = false;
            Program.SetState(Program.BusyState.Installation);
            await Setup.CloneSdRepo(commit);
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
    }
}
