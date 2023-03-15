using StableDiffusionGui.Extensions;
using System;
using System.Linq;

namespace StableDiffusionGui.Forms
{
    public partial class RealtimeLoggerForm
    {
        public string LogText
        {
            get { return logBox.InvokeRequired ? (string)logBox.Invoke(new Func<string>(() => logBox.Text)) : logBox.Text; }
            set { if (logBox.InvokeRequired) logBox.Invoke(new Action<string>((text) => { logBox.Text = text; }), value); else logBox.Text = value; }
        }

        public void LogAppend(string s, bool replaceLastLine = false)
        {
            if (this.IsDisposed || logBox.IsDisposed || this.Disposing || logBox.Disposing)
                return;

            if (logBox.InvokeRequired)
            {
                Invoke(new Action<string, bool>(LogAppend), s, replaceLastLine);
                return;
            }

            if (replaceLastLine)
            {
                logBox.Suspend();
                string[] lines = LogText.SplitIntoLines();
                LogText = string.Join(Environment.NewLine, lines.Take(lines.Length - 1));
            }

            if (LogText.IsNotEmpty())
                s = Environment.NewLine + s;

            if (s.IsNotEmpty())
                logBox.AppendText(s);

            if (replaceLastLine)
                logBox.Resume();
        }
    }
}
