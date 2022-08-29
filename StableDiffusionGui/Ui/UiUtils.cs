using Nmkoder.Forms;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Ui
{
    internal class UiUtils
    {
        public enum MessageType { Message, Warning, Error };

        public static DialogResult ShowMessageBox(string text, MessageType type = MessageType.Message)
        {
            MessageBoxIcon icon = MessageBoxIcon.Information;
            if (type == MessageType.Warning) icon = MessageBoxIcon.Warning;
            else if (type == MessageType.Error) icon = MessageBoxIcon.Error;

            MessageForm form = new MessageForm(text, $"SD GUI - {type}");
            form.ShowDialog();
            return DialogResult.OK;
        }

        public static DialogResult ShowMessageBox(string text, string title, MessageBoxButtons btns)
        {
            MessageForm form = new MessageForm(text, title, btns);
            return form.ShowDialog();
        }
    }
}
