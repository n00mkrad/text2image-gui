using StableDiffusionGui.Ui;
using System;
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
            Task.Run(() => GetWebInfo.LoadNews(newsLabel));
            Task.Run(() => GetWebInfo.LoadPatronListCsv(patronsLabel));
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
    }
}
