using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class PromptForm : Form
    {
        public string EnteredText { get; set; } = "";

        public PromptForm(string title, string message, string defaultText)
        {
            InitializeComponent();
            Text = title;
            msgLabel.Text = message;
            textBox.Text = defaultText;
            AcceptButton = confirmBtn;
        }

        private void PromptForm_Load(object sender, EventArgs e)
        {

        }

        private void confirmBtn_Click(object sender, EventArgs e)
        {
            EnteredText = textBox.Text.Trim();
            DialogResult = DialogResult.OK;
            Close();
        }

        private async void PromptForm_Shown(object sender, EventArgs e)
        {
            Refresh();
            await Task.Delay(1);
            Opacity = 1f;
        }
    }
}
