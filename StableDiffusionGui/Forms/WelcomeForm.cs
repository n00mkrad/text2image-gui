using StableDiffusionGui.Io;
using StableDiffusionGui.Ui;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class WelcomeForm : Form
    {
        public WelcomeForm()
        {
            InitializeComponent();
        }

        private void WelcomeForm_Load(object sender, EventArgs e)
        {
            
        }

        private void WelcomeForm_Shown(object sender, EventArgs e)
        {
            Refresh();

            Task.Run(() => GetWebInfo.LoadNews(newsLabel));
            Task.Run(() => GetWebInfo.LoadPatronListCsv(patronsLabel));

            if(Config.Instance.MotdShownVersion != Program.Version)
            {
                Config.Instance.MotdShownVersion = Program.Version;
            }
            else
            {
                checkboxDoNotShow.Visible = true;
                Config.Instance.HideMotd = false;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void WelcomeForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Return)
                Close();
        }

        private void btnItch_Click(object sender, EventArgs e)
        {
            Process.Start("https://nmkd.itch.io/t2i-gui");
        }

        private void btnGithub_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/n00mkrad/text2image-gui");
        }

        private void WelcomeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Config.Instance.HideMotd = checkboxDoNotShow.Checked;
        }
    }
}
