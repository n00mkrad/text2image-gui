using StableDiffusionGui.Main;
using System;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class RealtimeLoggerForm : Form
    {
        public RealtimeLoggerForm()
        {
            InitializeComponent();
        }

        private void RealtimeLoggerForm_Load(object sender, EventArgs e)
        {
            Logger.TextboxDebug = logBox;
        }
    }
}
