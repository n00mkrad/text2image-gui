using Nmkoder.Forms;
using StableDiffusionGui.Extensions;
using System.Windows.Forms;

namespace StableDiffusionGui.Ui
{
    internal class UiUtils
    {
        public enum MessageType { Message, Warning, Error };

        public static DialogResult ShowMessageBox(string text, MessageType type = MessageType.Message, MessageForm.FontSize fontSize = MessageForm.FontSize.Normal)
        {
            MessageBoxIcon icon = MessageBoxIcon.Information;
            if (type == MessageType.Warning) icon = MessageBoxIcon.Warning;
            else if (type == MessageType.Error) icon = MessageBoxIcon.Error;

            MessageForm form = new MessageForm(text, $"{type}");
            form.MsgFontSize = fontSize;
            form.ShowDialogForm();
            return DialogResult.OK;
        }

        public static DialogResult ShowMessageBox(string text, string title, MessageBoxButtons btns = MessageBoxButtons.OK)
        {
            MessageForm form = new MessageForm(text, title, btns);
            return form.ShowDialogForm();
        }
    }
}
