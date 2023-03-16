using StableDiffusionGui.Extensions;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Ui;
using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class RealtimeLoggerForm : Form
    {
        private float _defaultFontSize;

        public RealtimeLoggerForm()
        {
            InitializeComponent();
        }

        private void RealtimeLoggerForm_Load(object sender, EventArgs e)
        {
            _defaultFontSize = logBox.Font.Size;
            Logger.RealtimeLoggerForm = this;
            logBox.MouseWheel += logBox_MouseWheel;
        }

        private async void RealtimeLoggerForm_Shown(object sender, EventArgs e)
        {
            Refresh();
            LogAppend($"This window displays all messages that are logged while it's open.{Environment.NewLine}");
            Opacity = 1f;

            var font = Fonts.GetFontOnDemand("Cascadia Mono SemiBold", Path.Combine(Paths.GetDataPath(), "fonts", "CascadiaMono.ttf"), true);
            logBox.Font = logBox.Font.ChangeFontFamily(font);
        }

        private void logBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!InputUtils.IsHoldingCtrl) return;
            int sizeChange = e.Delta > 0 ? 1 : -1;
            logBox.Font = logBox.Font.ChangeSize((logBox.Font.Size + sizeChange).Clamp(_defaultFontSize, _defaultFontSize * 2f));
        }

        private void RealtimeLoggerForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Escape))
                BeginInvoke(new MethodInvoker(Close));
        }

        private void RealtimeLoggerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logger.RealtimeLoggerForm = null;
        }
    }
}
