using StableDiffusionGui.Extensions;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class RealtimeLoggerForm : Form
    {
        private float _defaultFontSize;
        private int _maxEntriesLoad = -1;
        //private int _maxEntriesDisplay = -1;
        private int _displayDelay = 1;
        private List<long> _previousEntries = new List<long>();

        public RealtimeLoggerForm()
        {
            InitializeComponent();
        }

        private void RealtimeLoggerForm_Load(object sender, EventArgs e)
        {
            _defaultFontSize = logBox.Font.Size;
            Logger.TextboxDebug = logBox;
            logBox.MouseWheel += logBox_MouseWheel;
        }

        private void RealtimeLoggerForm_Shown(object sender, EventArgs e)
        {
            Refresh();
            _previousEntries = Logger.GetLastEntries(-1).Select(entry => entry.Id).ToList();
            DisplayLoop();
        }

        private void logBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!InputUtils.IsHoldingCtrl) return;
            int sizeChange = e.Delta > 0 ? 1 : -1;
            logBox.Font = new Font(logBox.Font.Name, (logBox.Font.Size + sizeChange).Clamp(_defaultFontSize, _defaultFontSize * 2f), logBox.Font.Style, logBox.Font.Unit);
        }

        private void RealtimeLoggerForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Escape))
                BeginInvoke(new MethodInvoker(Close));
        }

        private async Task DisplayLoop ()
        {
            logBox.AppendText($"This window displays all messages that are logged while it's open.{Environment.NewLine}");

            while (true)
            {
                var entries = Logger.GetLastEntries(_maxEntriesLoad).Where(e => !_previousEntries.Contains(e.Id)).OrderBy(e => e.Id).ToList();
                _previousEntries.AddRange(entries.Select(e => e.Id));

                if (entries.Count > 0)
                {
                    foreach (var entry in entries)
                    {
                        logBox.AppendText(entry.ToString(false, true, true) + Environment.NewLine);
                        await Task.Delay(1);
                    }
                }

                await Task.Delay(_displayDelay);
            }
        }
    }
}
