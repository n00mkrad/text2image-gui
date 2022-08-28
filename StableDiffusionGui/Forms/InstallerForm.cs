using StableDiffusionGui.Installation;
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
            installBtn.Enabled = false;
            await Setup.Install();
            installBtn.Enabled = true;
        }
    }
}
