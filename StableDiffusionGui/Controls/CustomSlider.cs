using HTAlt.WinForms;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Controls
{
    public class CustomSlider : HTSlider
    {
        public TextBox _valueBox;
        public Control ValueBox
        {
            get
            {
                return _valueBox;
            }
            set
            {
                if (!(value is TextBox)) return;
                _valueBox = (TextBox)value;
                value.Leave += new EventHandler(valueBox_Leave);
                value.KeyPress += new KeyPressEventHandler(valueBox_KeyPress);
                UpdateValueBox();
            }
        }
        private bool HasValueBox { get { return ValueBox != null && ValueBox is TextBox; } }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public decimal ActualValue
        {
            get
            {
                return Value / _conversionFactor;
            }
            set
            {
                UpdateMinMax();
                Value = ((double)(value * _conversionFactor)).RoundToInt().Clamp(Minimum, Maximum);
                UpdateValueBox();
            }
        }

        [Browsable(false)]
        public int ActualValueInt { get { return (int)ActualValue; } }

        [Browsable(false)]
        public float ActualValueFloat { get { return (float)ActualValue; } }

        private decimal _valueStep = 0.2m;
        public decimal ValueStep { get { return _valueStep; } set { _valueStep = value; } }

        private decimal _actualMinimum = 0.2m;
        public decimal ActualMinimum { get { return _actualMinimum; } set { _actualMinimum = value; UpdateMinMax(); } }

        private decimal _actualMaximum = 10.0m;
        public decimal ActualMaximum { get { return _actualMaximum; } set { _actualMaximum = value; UpdateMinMax(); } }

        private decimal _initValue = -1m;
        public decimal InitValue { get { return _initValue; } set { _initValue = value; } }

        public CustomSlider()
        {
            Init();
        }

        private async void Init()
        {
            if (Process.GetCurrentProcess().ProcessName == "devenv")
                return;

            await Task.Delay(1);

            if (InitValue >= 0m)
                ActualValue = InitValue;
        }

        public void ChangeStep(decimal newStep)
        {
            var oldVal = ActualValue;
            ValueStep = newStep;
            UpdateMinMax();
            ActualValue = oldVal;
        }

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

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            UpdateValueBox();
        }

        private decimal _conversionFactor { get { return 1m / ValueStep; } }

        private void UpdateMinMax()
        {
            Maximum = ((double)ActualMaximum * (double)_conversionFactor).RoundToInt();
            Minimum = ((double)ActualMinimum * (double)_conversionFactor).RoundToInt();
            UpdateValueBox();
        }

        private void UpdateValueBox()
        {
            if (HasValueBox && Visible && ValueBox.Visible)
                ValueBox.Text = ActualValue.ToString("0.######");
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

        private void ParseFromValueBox()
        {
            if (!HasValueBox)
                return;

            float value;
            bool parseSuccess = float.TryParse(ValueBox.Text.Trim().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out value);

            bool rangeValid = value >= (float)ActualMinimum && value <= (float)ActualMaximum;
            bool stepValid = value % (float)ValueStep == 0;

            if (!stepValid)
                value = (value / (float)ValueStep).RoundToInt() * (float)ValueStep; // Lock into steps

            if (parseSuccess && rangeValid)
                ActualValue = (decimal)value;
            else
                UpdateValueBox();
        }
    }
}
