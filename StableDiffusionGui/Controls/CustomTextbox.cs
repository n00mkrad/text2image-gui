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
    internal class CustomTextbox : TextBox
    {
        [Category("Custom")]
        public string Placeholder { get; set; } = "";
        [Category("Custom")]
        public Color PlaceholderTextColor { get; set; } = System.Drawing.Color.Silver;

        [Browsable(false)]
        private bool _init;
        [Browsable(false)]
        private Color _originalTextColor;
        [Browsable(false)]
        private float _defaultPromptFontSize;

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

        protected override void OnGotFocus(EventArgs e)
        {
            UpdatePlaceholderState();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            UpdatePlaceholderState();
            base.OnLostFocus(e);
        }

        private void UpdatePlaceholderState()
        {
            if (DesignMode)
                return;

            if (!string.IsNullOrWhiteSpace(Placeholder))
            {
                if (Text == Placeholder && Focused)
                    Text = "";
                else if (Text == "")
                    Text = Placeholder;
            
                ForeColor = Text == Placeholder ? PlaceholderTextColor : _originalTextColor;
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (Focused && InputUtils.IsHoldingCtrl)
            {
                int sizeChange = e.Delta > 0 ? 1 : -1;
                Font = new Font(Font.Name, (Font.Size + sizeChange).Clamp(_defaultPromptFontSize, _defaultPromptFontSize * 2f), Font.Style, Font.Unit);
            }

            base.OnMouseWheel(e);
        }
    }
}
