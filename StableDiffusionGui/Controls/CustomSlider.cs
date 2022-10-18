using HTAlt.WinForms;
using StableDiffusionGui.Main;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace StableDiffusionGui.Controls
{
    public class CustomSlider : HTSlider
    {
        public TextBox _valueBox;
        public Control ValueBox {
            get {
                return _valueBox;
            }
            set {
                if (!(value is TextBox)) return;
                _valueBox = (TextBox)value;
                value.Leave += new EventHandler(valueBox_Leave);
                value.KeyPress += new KeyPressEventHandler(valueBox_KeyPress);
            } }
        private bool HasValueBox { get { return ValueBox != null && ValueBox is TextBox; } }

        public decimal ActualValue { get { return Value / _conversionFactor; } set { Value = ((double)(value * _conversionFactor)).RoundToInt().Clamp(Minimum, Maximum); } }
        
        [Browsable(false)]
        public int ActualValueInt { get { return (int)ActualValue; } }

        [Browsable(false)]
        public float ActualValueFloat { get { return (float)ActualValue; } }

        private decimal _valueStep = 0.2m;
        public decimal ValueStep { get { return _valueStep; } set { _valueStep = value; UpdateProperties(); } }

        private decimal _actualMinimum = 0.2m;
        public decimal ActualMinimum { get { return _actualMinimum; } set { _actualMinimum = value; UpdateProperties(); } }

        private decimal _actualMaximum = 10.0m;
        public decimal ActualMaximum { get { return _actualMaximum; } set { _actualMaximum = value; UpdateProperties(); } }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            // base.OnMouseWheel(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            UpdateValueBox();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            UpdateValueBox();
        }

        private decimal _conversionFactor { get { return 1m / ValueStep; } }

        private void UpdateProperties ()
        {
            Maximum = ((double)ActualMaximum * (double)_conversionFactor).RoundToInt();
            Minimum = ((double)ActualMinimum * (double)_conversionFactor).RoundToInt();
        }

        private void UpdateValueBox ()
        {
            if (HasValueBox)
                ValueBox.Text = ActualValue.ToString("0.###");
        }

        private void valueBox_Leave(object s, EventArgs e)
        {
            ParseFromValueBox();
        }

        private void valueBox_KeyPress(object s, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                ParseFromValueBox();
        }

        private void ParseFromValueBox ()
        {
            if (!HasValueBox)
                return;

            float value;
            bool parseSuccess = float.TryParse(ValueBox.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out value);

            bool rangeValid = value >= (float)ActualMinimum && value <= (float)ActualMaximum;
            bool stepValid = value % (float)ValueStep == 0;

            if (parseSuccess && rangeValid && stepValid)
                ActualValue = (decimal)value;
            else
                UpdateValueBox();
        }
    }
}
