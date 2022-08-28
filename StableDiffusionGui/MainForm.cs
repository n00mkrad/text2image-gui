using StableDiffusionGui.Forms;
using StableDiffusionGui.Io;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void installerBtn_Click(object sender, EventArgs e)
        {
            new InstallerForm().ShowDialog();
        }

        private void runBtn_Click(object sender, EventArgs e)
        {
            string args = $"/C cd /D {Paths.GetDataPath().Wrap()} && call \"{Paths.GetDataPath()}\\mc\\Scripts\\activate.bat\" ldo && python \"{Paths.GetDataPath()}/repo/scripts/dream.py\" -o \"{Paths.GetExeDir()}/out\"";
            Process.Start("cmd", args);
        }
    }
}
