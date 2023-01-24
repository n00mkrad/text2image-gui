using StableDiffusionGui.Controls;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using System;
using System.Windows.Forms;

namespace StableDiffusionGui.Io
{
    internal class ConfigParser
    {

        public enum StringMode { Any, Int, Float }

        public static Enums.StableDiffusion.Implementation CurrentImplementation { get { return ParseUtils.GetEnum<Enums.StableDiffusion.Implementation>(Config.Get<string>(Config.Keys.ImplementationName)); } }

        public static void SaveGuiElement(TextBox textbox, string key, StringMode stringMode = StringMode.Any)
        {
            switch (stringMode)
            {
                case StringMode.Any: Config.Set(key, textbox.Text); break;
                case StringMode.Int: Config.Set(key, textbox.Text.GetInt()); break;
                case StringMode.Float: Config.Set(key, textbox.Text.GetFloat()); break;
            }
        }

        public static void SaveGuiElement(ComboBox comboBox, string key, StringMode stringMode = StringMode.Any)
        {
            switch (stringMode)
            {
                case StringMode.Any: Config.Set(key, comboBox.Text); break;
                case StringMode.Int: Config.Set(key, comboBox.Text.GetInt()); break;
                case StringMode.Float: Config.Set(key, comboBox.Text.GetFloat()); break;
            }
        }

        public static void SaveGuiElement(CheckBox checkbox, string key)
        {
            Config.Set(key, checkbox.Checked);
        }

        public static void SaveGuiElement(NumericUpDown upDown, string key, StringMode stringMode = StringMode.Any)
        {
            switch (stringMode)
            {
                case StringMode.Any: Config.Set(key, ((float)upDown.Value)); break;
                case StringMode.Int: Config.Set(key, ((int)upDown.Value)); break;
                case StringMode.Float: Config.Set(key, ((float)upDown.Value)); break;
            }
        }

        public static void SaveGuiElement(HTAlt.WinForms.HTSlider slider, string key)
        {
            float value = slider is CustomSlider ? ((CustomSlider)slider).ActualValueFloat : slider.Value;
            Config.Set(key, value);
        }

        public static void SaveComboxIndex(ComboBox comboBox, string key)
        {
            Config.Set(key, comboBox.SelectedIndex);
        }

        public static void LoadGuiElement(ComboBox comboBox, string key, string suffix = "")
        {
            comboBox.Text = Config.Get<string>(key) + suffix;
        }

        public static void LoadGuiElement(TextBox textbox, string key, string suffix = "")
        {
            textbox.Text = Config.Get<string>(key) + suffix; ;
        }

        public static void LoadGuiElement(CheckBox checkbox, string key)
        {
            checkbox.Checked = Config.Get<bool>(key);
        }

        public static void LoadGuiElement(NumericUpDown upDown, string key)
        {
            upDown.Value = Convert.ToDecimal(Config.Get<float>(key).Clamp((float)upDown.Minimum, (float)upDown.Maximum));
        }

        public enum SaveValueAs { Unchanged, Multiplied, Divided }

        public static void LoadGuiElement(HTAlt.WinForms.HTSlider slider, string key)
        {
            var value = Config.Get<float>(key);

            if (slider is CustomSlider)
                ((CustomSlider)slider).ActualValue = (decimal)value.Clamp((float)((CustomSlider)slider).ActualMinimum, (float)((CustomSlider)slider).ActualMaximum);
            else
                slider.Value = value.RoundToInt().Clamp(slider.Minimum, slider.Maximum);
        }

        public static void LoadComboxIndex(ComboBox comboBox, string key)
        {
            if (comboBox.Items.Count == 0)
                return;

            comboBox.SelectedIndex = Config.Get<int>(key).Clamp(0, comboBox.Items.Count - 1);
        }
    }
}
