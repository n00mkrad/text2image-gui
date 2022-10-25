using StableDiffusionGui.Controls;
using System;
using System.Windows.Forms;

namespace StableDiffusionGui.Io
{
    internal class ConfigParser
    {

        public enum StringMode { Any, Int, Float }

        public static void SaveGuiElement(TextBox textbox, StringMode stringMode = StringMode.Any)
        {
            switch (stringMode)
            {
                case StringMode.Any: Config.Set(textbox.Name, textbox.Text); break;
                case StringMode.Int: Config.Set(textbox.Name, textbox.Text.GetInt().ToString()); break;
                case StringMode.Float: Config.Set(textbox.Name, textbox.Text.GetFloat().ToString()); break;
            }
        }

        public static void SaveGuiElement(ComboBox comboBox, StringMode stringMode = StringMode.Any)
        {
            switch (stringMode)
            {
                case StringMode.Any: Config.Set(comboBox.Name, comboBox.Text); break;
                case StringMode.Int: Config.Set(comboBox.Name, comboBox.Text.GetInt().ToString()); break;
                case StringMode.Float: Config.Set(comboBox.Name, comboBox.Text.GetFloat().ToStringDot()); break;
            }
        }

        public static void SaveGuiElement(CheckBox checkbox)
        {
            Config.Set(checkbox.Name, checkbox.Checked.ToString());
        }

        public static void SaveGuiElement(NumericUpDown upDown, StringMode stringMode = StringMode.Any)
        {
            switch (stringMode)
            {
                case StringMode.Any: Config.Set(upDown.Name, ((float)upDown.Value).ToStringDot()); break;
                case StringMode.Int: Config.Set(upDown.Name, ((int)upDown.Value).ToString()); break;
                case StringMode.Float: Config.Set(upDown.Name, ((float)upDown.Value).ToStringDot()); break;
            }
        }

        public static void SaveGuiElement(HTAlt.WinForms.HTSlider slider, SaveValueAs convertMode = SaveValueAs.Unchanged, float convertValue = 1f)
        {
            float value = slider is CustomSlider ? ((CustomSlider)slider).ActualValueFloat : slider.Value;

            if (convertMode == SaveValueAs.Multiplied)
                value = value * convertValue;

            if (convertMode == SaveValueAs.Divided)
                value = value / convertValue;

            Config.Set(slider.Name, value.ToStringDot());
        }

        public static void SaveComboxIndex(ComboBox comboBox)
        {
            Config.Set(comboBox.Name, comboBox.SelectedIndex.ToString());
        }

        public static void LoadGuiElement(ComboBox comboBox, string suffix = "")
        {
            comboBox.Text = Config.Get(comboBox.Name) + suffix;
        }

        public static void LoadGuiElement(TextBox textbox, string suffix = "")
        {
            textbox.Text = Config.Get(textbox.Name) + suffix; ;
        }

        public static void LoadGuiElement(CheckBox checkbox)
        {
            checkbox.Checked = Config.GetBool(checkbox.Name);
        }

        public static void LoadGuiElement(NumericUpDown upDown)
        {
            upDown.Value = Convert.ToDecimal(Config.GetFloat(upDown.Name).Clamp((float)upDown.Minimum, (float)upDown.Maximum));
        }

        public enum SaveValueAs { Unchanged, Multiplied, Divided }

        public static void LoadGuiElement(HTAlt.WinForms.HTSlider slider, SaveValueAs convertMode = SaveValueAs.Unchanged, float convertValue = 1f)
        {
            var value = Config.GetFloat(slider.Name);

            if (convertMode == SaveValueAs.Multiplied)
                value = value / convertValue;

            if (convertMode == SaveValueAs.Divided)
                value = value * convertValue;

            if (slider is CustomSlider)
                ((CustomSlider)slider).ActualValue = (decimal)value.Clamp((float)((CustomSlider)slider).ActualMinimum, (float)((CustomSlider)slider).ActualMaximum);
            else
                slider.Value = value.RoundToInt().Clamp(slider.Minimum, slider.Maximum);
        }

        public static void LoadComboxIndex(ComboBox comboBox)
        {
            if (comboBox.Items.Count == 0)
                return;

            comboBox.SelectedIndex = Config.GetInt(comboBox.Name).Clamp(0, comboBox.Items.Count - 1);
        }
    }
}
