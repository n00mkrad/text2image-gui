using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Controls
{
    public class CustomTextbox : TextBox
    {
        [Category("Custom")]
        public bool DisableUnfocusedInput { get; set; } = true;
        [Category("Custom")]
        public float MaxTextZoomFactor { get; set; } = 2.0f;
        [Category("Custom")]
        public string Placeholder { get; set; } = "";
        [Category("Custom")]
        public Color PlaceholderTextColor { get; set; } = Color.Silver;

        [Browsable(false)]
        private bool _init;
        [Browsable(false)]
        private Color _originalTextColor;
        [Browsable(false)]
        private float _defaultPromptFontSize;
        [Browsable(false)]
        public string TextNoPlaceholder { get { return base.Text == Placeholder ? "" : base.Text; } }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (!_init && !DesignMode)
            {
                _init = true;
                _originalTextColor = ForeColor;
                _defaultPromptFontSize = Font.Size;
            }

            UpdatePlaceholderState();
            base.OnVisibleChanged(e);
        }

        private int _selectionStart = int.MaxValue;
        private int selectionStart { get { return _selectionStart.Clamp(0, Text.Length); } set { _selectionStart = value; } } 
        private int selectionLength = 0;

        private void SaveCursorPos()
        {
            selectionStart = SelectionStart;
            selectionLength = SelectionLength;
        }

        private void RestoreCursorPos()
        {
            SelectionStart = selectionStart;
            SelectionLength = selectionLength;
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            SaveCursorPos();
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            RestoreCursorPos();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (DisableUnfocusedInput)
                ReadOnly = false;

            UpdatePlaceholderState();
            base.OnGotFocus(e);

            RestoreCursorPos();
            SelectionLength = 0;
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (DisableUnfocusedInput)
                ReadOnly = true;

            UpdatePlaceholderState();
            SaveCursorPos();
            base.OnLostFocus(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (!Focused)
                UpdatePlaceholderState();

            base.OnTextChanged(e);
        }

        private void UpdatePlaceholderState()
        {
            if (DesignMode)
                return;

            if (!string.IsNullOrWhiteSpace(Placeholder))
            {
                if (Text.Trim() == Placeholder && Focused)
                    Text = "";
                else if (Text.Trim() == "")
                    Text = Placeholder;

                ForeColor = Text.Trim() == Placeholder ? PlaceholderTextColor : _originalTextColor;
                Text = Text.Trim();
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (Focused && InputUtils.IsHoldingCtrl)
            {
                int sizeChange = e.Delta > 0 ? 1 : -1;
                float newFontSize = (Font.Size + sizeChange).Clamp(_defaultPromptFontSize, _defaultPromptFontSize * MaxTextZoomFactor);
                Font = new Font(Font.Name, newFontSize, Font.Style, Font.Unit);
            }

            base.OnMouseWheel(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Control && e.KeyCode == Keys.Back)
            {
                e.SuppressKeyPress = true;

                if (SelectionStart > 0)
                    SendKeys.Send("+{LEFT}{DEL}");
            }
        }
    }
}
