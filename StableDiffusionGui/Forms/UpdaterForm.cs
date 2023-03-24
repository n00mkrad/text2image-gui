using HTAlt;
using StableDiffusionGui.Data;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Main;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class UpdaterForm : CustomForm
    {
        public UpdaterForm()
        {
            InitializeComponent();
        }

        private void UpdaterForm_Load(object sender, EventArgs e)
        {

        }

        private async void UpdaterForm_Shown(object sender, EventArgs e)
        {
            Refresh();
            labelCurrVersion.Text = $"Version {Program.Version} (Repo Commit {Setup.GitCommit.Trunc(8, false)})";
            TabOrderInit(new List<Control>() { comboxVersion, btnInstall }, -1);
            await Task.Delay(1);
            Opacity = 1;
            await Task.Delay(1);
            LoadAvailableVersions();
        }

        private async Task LoadAvailableVersions()
        {
            List<MdlRelease> releases = await GetWebInfo.LoadReleases();
            comboxVersion.Items.Clear();

            foreach (MdlRelease r in releases)
                comboxVersion.Items.Add(r);

            comboxVersion.Items.Cast<MdlRelease>().Where(r => r.Version == Program.Version).ToList().ForEach(r => comboxVersion.Text = r.ToString());

            if (comboxVersion.Text.IsEmpty() && comboxVersion.Items.Count > 0)
                comboxVersion.SelectedIndex = 0;
        }

        private async void btnInstall_Click(object sender, EventArgs e)
        {
            if (Program.Busy)
            {
                UiUtils.ShowMessageBox("Please wait until the current process has finished.");
                return;
            }

            if (string.IsNullOrWhiteSpace(comboxVersion.Text))
            {
                UiUtils.ShowMessageBox("Invalid version selection.");
                return;
            }

            MdlRelease selectedRelease = (MdlRelease)comboxVersion.SelectedItem;

            if (selectedRelease.Version == Program.Version)
            {
                DialogResult dialogResult = UiUtils.ShowMessageBox($"The selected version is the installed one. Do you want to install it anyway?", "Re-install?", MessageBoxButtons.YesNo);

                if (dialogResult != DialogResult.Yes)
                    return;
            }

            Enabled = false;
            Program.SetState(Program.BusyState.Installation);
            btnInstall.Text = "Installing...";
            await Updater.Install(selectedRelease, checkboxKeepImages.Checked, checkboxKeepModels.Checked, checkboxKeepSettings.Checked);
            btnInstall.Text = "Install Selected Release";
            Enabled = true;
            Program.SetState(Program.BusyState.Standby);
        }
    }
}
