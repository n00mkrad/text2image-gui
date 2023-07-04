using StableDiffusionGui.Installation;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Ui;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class InstallerForm : CustomForm
    {
        private bool _autoInstall = false;
        private bool _updateDeps = false;
        private bool _overrideInstallUpscalers = false;
        private bool _onlyInstallUpscalers = false;

        public InstallerForm()
        {
            InitializeComponent();
        }

        public InstallerForm(bool onlyInstallUpscalers)
        {
            _autoInstall = false;
            _updateDeps = false;
            _onlyInstallUpscalers = onlyInstallUpscalers;
            InitializeComponent();
        }

        public InstallerForm(bool autoInstall, bool updateDeps, bool overrideInstallUpscalers)
        {
            _autoInstall = autoInstall;
            _updateDeps = updateDeps;
            _overrideInstallUpscalers = overrideInstallUpscalers;
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
                await Setup.Install(true, InstallationStatus.HasSdUpscalers());
            }
            else
            {
                bool installUpscalers = _autoInstall ? _overrideInstallUpscalers : AskInstallUpscalers();
                bool updateDeps = _autoInstall && _updateDeps;
                await Setup.Install(false, updateDeps, installUpscalers);
            }

            if (_autoInstall)
            {
                Task.Run(() => MainUi.GetCudaGpus());
                Close();
            }

            BringToFront();
            UpdateStatus();
            Enabled = true;
        }

        private async void InstallerForm_Shown(object sender, EventArgs e)
        {
            Refresh();
            UpdateStatus();
            Enabled = true;

            if (_onlyInstallUpscalers)
            {
                await InstallUpscalers();

                if (InstallationStatus.HasSdUpscalers())
                    Close();
            }
            else if (_autoInstall)
            {
                installBtn_Click(null, null);
            }
        }

        public void UpdateStatus()
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
            Logger.ClearLogBox();

            if (InputUtils.IsHoldingShift)
            {
                var form = new PromptForm("Clone Specific Commit", "Enter a commit hash (SHA) to clone.", "");
                form.ShowDialog();
                commit = form.EnteredText.Trim();
            }

            Enabled = false;
            Program.SetState(Program.BusyState.Installation);
            await Setup.InstallRepo(commit, InputUtils.IsHoldingShift);
            Setup.RepoCleanup();
            UpdateStatus();
            Program.SetState(Program.BusyState.Standby);
            Enabled = true;
        }

        private async void btnInstallUpscalers_Click(object sender, EventArgs e)
        {
            await InstallUpscalers();
        }

        private async Task InstallUpscalers()
        {
            Enabled = false;
            await Setup.InstallUpscalers();
            UpdateStatus();
            Enabled = true;
        }

        private bool AskInstallUpscalers()
        {
            DialogResult res = UiUtils.ShowMessageBox("Do you want to pre-download the upscaling and face restoration models? (1 GB)", "Setup", MessageBoxButtons.YesNo);
            return res == DialogResult.Yes;
        }

        private void InstallerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.MainForm.TryRefreshUiState();
            Program.MainForm.BringToFront();
        }
    }
}
