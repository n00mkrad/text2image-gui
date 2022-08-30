using StableDiffusionGui.Installation;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class InstallerForm : Form
    {
        public InstallerForm()
        {
            InitializeComponent();
        }

        private void InstallerForm_Load(object sender, EventArgs e)
        {

        }

        private async void installBtn_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            await Setup.Install();
            UpdateStatus();
            this.Enabled = true;
        }

        private void InstallerForm_Shown(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        public void UpdateStatus ()
        {
            for (int i = 0; i < checkedListBoxStatus.Items.Count; i++)
            {
                string text = checkedListBoxStatus.Items[i].ToString().ToLower();

                if (text.Contains("miniconda"))
                    checkedListBoxStatus.SetItemChecked(i, InstallationStatus.HasConda());

                if (text.Contains("repository"))
                    checkedListBoxStatus.SetItemChecked(i, InstallationStatus.HasSdRepo());

                if (text.Contains("env"))
                    checkedListBoxStatus.SetItemChecked(i, InstallationStatus.HasSdEnv());

                if (text.Contains("model"))
                    checkedListBoxStatus.SetItemChecked(i, InstallationStatus.HasSdModel());
            }

            if (checkedListBoxStatus.CheckedItems.Count == checkedListBoxStatus.Items.Count) // all checked
                btnInstall.Text = "Re-Install";
            else
                btnInstall.Text = "Install";

            btnUninstall.Enabled = checkedListBoxStatus.CheckedItems.Count > 1;
        }

        private async void btnUninstall_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            Logger.Log("Uninstalling...");
            await Setup.Cleanup();
            await Setup.RemoveEnv();
            UpdateStatus();
            Logger.Log("Done.");
            this.Enabled = true;
        }

        private void btnPatch_Click(object sender, EventArgs e)
        {
            Setup.Patch();
        }
    }
}
