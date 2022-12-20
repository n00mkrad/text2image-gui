using StableDiffusionGui;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Nmkoder.Forms
{
    public partial class MessageForm : Form
    {
        public enum FontSize { Normal, Big, VeryBig };

        private string _text = "";
        private string _title = "";
        private MessageBoxButtons _btns;

        private bool _dialogResultSet = false;

        public FontSize MsgFontSize = FontSize.Normal;

        private readonly int _fallbackWidth = 1200;
        private readonly int _fallbackHeight = 700;

        private int MainFormWidth { get { return Program.MainForm == null ? _fallbackWidth : Program.MainForm.Width; } }
        private int MainFormHeight { get { return Program.MainForm == null ? _fallbackHeight : Program.MainForm.Height; } }

        public MessageForm(string text, string title, MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            _text = text;
            _title = title;
            _btns = buttons;

            InitializeComponent();
        }

        private void MessageForm_Load(object sender, EventArgs e)
        {
            if (MsgFontSize == FontSize.Big)
                textLabel.Font = new Font(textLabel.Font.Name, (textLabel.Font.SizeInPoints + 2f));
            else if (MsgFontSize == FontSize.VeryBig)
                textLabel.Font = new Font(textLabel.Font.Name, (textLabel.Font.SizeInPoints + 4f));

            Text = _title;
            textLabel.Text = _text;

            if (_btns == MessageBoxButtons.OK)
            {
                SetButtons(true, false, false);
                btn1.Text = "OK";
                // AcceptButton = (IButtonControl)btn1;
            }
            else if (_btns == MessageBoxButtons.YesNo)
            {
                SetButtons(true, true, false);
                btn1.Text = "No";
                btn2.Text = "Yes";
                // AcceptButton = (IButtonControl)btn2;
                // CancelButton = (IButtonControl)btn1;
            }
            else if (_btns == MessageBoxButtons.YesNoCancel)
            {
                SetButtons(true, true, true);
                btn1.Text = "Cancel";
                btn2.Text = "No";
                btn3.Text = "Yes";
                // AcceptButton = (IButtonControl)btn3;
                // CancelButton = (IButtonControl)btn1;
            }

            textLabel.MaximumSize = new Size(MainFormWidth - 30, MainFormHeight - 50);
            Size labelSize = GetLabelSize(textLabel);
            Size = new Size((labelSize.Width + 60).Clamp(360, MainFormWidth), (labelSize.Height + 120).Clamp(200, MainFormHeight));

            CenterToScreen();
        }

        private Size GetLabelSize(Label label)
        {
            return TextRenderer.MeasureText(label.Text, label.Font, label.ClientSize, TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl);
        }

        private void SetButtons(bool b1, bool b2, bool b3)
        {
            btn1.Visible = b1;
            btn2.Visible = b2;
            btn3.Visible = b3;
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            if (_btns == MessageBoxButtons.OK) // OK Button
                DialogResult = DialogResult.OK;
            else if (_btns == MessageBoxButtons.YesNo) // No Button
                DialogResult = DialogResult.No;
            else if (_btns == MessageBoxButtons.YesNoCancel) // Cancel Button
                DialogResult = DialogResult.Cancel;

            _dialogResultSet = true;
            Close();
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            if (_btns == MessageBoxButtons.YesNo) // Yes Button
                DialogResult = DialogResult.Yes;
            else if (_btns == MessageBoxButtons.YesNoCancel) // No Button
                DialogResult = DialogResult.No;

            _dialogResultSet = true;
            Close();
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            if (_btns == MessageBoxButtons.YesNoCancel) // Yes Button
                DialogResult = DialogResult.Yes;

            _dialogResultSet = true;
            Close();
        }

        private void MessageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_btns != MessageBoxButtons.OK && !_dialogResultSet)
                e.Cancel = true;
        }

        private void MessageForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (_btns == MessageBoxButtons.OK)
            {
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
                    btn1_Click(null, null);
            }
            else if (_btns == MessageBoxButtons.YesNo)
            {
                if (e.KeyCode == Keys.Enter)
                    btn2_Click(null, null);
            }
            else if (_btns == MessageBoxButtons.YesNoCancel)
            {
                if (e.KeyCode == Keys.Enter)
                    btn3_Click(null, null);
                else if (e.KeyCode == Keys.Escape)
                    btn1_Click(null, null);
            }
        }

        private void MessageForm_Shown(object sender, EventArgs e)
        {
            Activate();
        }
    }
}
