using StableDiffusionGui.Controls;
using System;
using System.Windows.Forms;

namespace StableDiffusionGui.Io
{
    internal class ConfigParser
    {
        public enum StringMode { Any, Int, Float }

        public static bool UpscaleAndSaveOriginals(ConfigInstance instance = null)
        {
            if (instance == null) instance = Config.Instance;
            return instance.SaveUnprocessedImages && (instance.UpscaleEnable || instance.FaceRestoreEnable);
        }

        public static void SaveGuiElement(TextBox textbox, ref string variable)
        {
            variable = textbox.Text;
        }

        public static void SaveGuiElement(ComboBox comboBox, ref string variable)
        {
            variable = comboBox.Text;
        }

        public static void SaveGuiElement(ComboBox comboBox, ref int variable)
        {
            variable = comboBox.Text.GetInt();
        }

        public static void SaveGuiElement(CheckBox checkbox, ref bool variable)
        {
            variable = checkbox.Checked;
        }

        public static void SaveGuiElement(NumericUpDown upDown, ref int variable)
        {
            variable = (int)upDown.Value;
        }

        public static void SaveGuiElement(NumericUpDown upDown, ref float variable)
        {
            variable = (float)upDown.Value;
        }

        public static void SaveGuiElement(HTAlt.WinForms.HTSlider slider, ref float variable)
        {
            float value = slider is CustomSlider ? ((CustomSlider)slider).ActualValueFloat : slider.Value;
            variable = value;
        }

        public static void SaveGuiElement(HTAlt.WinForms.HTSlider slider, ref int variable)
        {
            float value = slider is CustomSlider ? ((CustomSlider)slider).ActualValueFloat : slider.Value;
            variable = value.RoundToInt();
        }

        public static void SaveComboxIndex(ComboBox comboBox, ref int variable)
        {
            variable = comboBox.SelectedIndex;
        }

        public static void SaveComboxIndex<TEnum>(ComboBox comboBox, ref TEnum variable)
        {
            variable = (TEnum)Enum.ToObject(typeof(TEnum), comboBox.SelectedIndex);
        }

        public static void LoadGuiElement(ComboBox comboBox, ref string variable, string suffix = "")
        {
            comboBox.Text = variable + suffix;
        }

        public static void LoadGuiElement(ComboBox comboBox, ref int variable, string suffix = "")
        {
            comboBox.Text = variable + suffix;
        }

        public static void SaveGuiElement(ComboBox comboBox, ref bool variable)
        {
            variable = comboBox.SelectedIndex == 0 ? false : true;
        }

        public static void LoadGuiElement(ComboBox comboBox, ref bool variable, string suffix = "")
        {
            if (comboBox.Items != null && comboBox.Items.Count >= 1)
                comboBox.SelectedIndex = variable ? 1 : 0;
        }

        public static void LoadGuiElement(TextBox textbox, ref string variable, string suffix = "")
        {
            textbox.Text = variable + suffix;
        }

        public static void LoadGuiElement(CheckBox checkbox, ref bool variable)
        {
            checkbox.Checked = variable;
        }

        public static void LoadGuiElement(NumericUpDown upDown, ref float variable)
        {
            upDown.Value = Convert.ToDecimal(variable.Clamp((float)upDown.Minimum, (float)upDown.Maximum));
        }

        public static void LoadGuiElement(NumericUpDown upDown, ref int variable)
        {
            upDown.Value = Convert.ToDecimal(variable.Clamp((int)upDown.Minimum, (int)upDown.Maximum));
        }

        public enum SaveValueAs { Unchanged, Multiplied, Divided }

        public static void LoadGuiElement(HTAlt.WinForms.HTSlider slider, ref float variable)
        {
            if (slider is CustomSlider)
                ((CustomSlider)slider).ActualValue = (decimal)variable.Clamp((float)((CustomSlider)slider).ActualMinimum, (float)((CustomSlider)slider).ActualMaximum);
            else
                slider.Value = variable.RoundToInt().Clamp(slider.Minimum, slider.Maximum);
        }

        public static void LoadGuiElement(HTAlt.WinForms.HTSlider slider, ref int variable)
        {
            if (slider is CustomSlider)
                ((CustomSlider)slider).ActualValue = (decimal)variable.Clamp((int)((CustomSlider)slider).ActualMinimum, (int)((CustomSlider)slider).ActualMaximum);
            else
                slider.Value = variable.Clamp(slider.Minimum, slider.Maximum);
        }

        public static void LoadComboxIndex(ComboBox comboBox, ref int variable)
        {
            if (comboBox.Items.Count == 0)
                return;

            comboBox.SelectedIndex = variable.Clamp(0, comboBox.Items.Count - 1);
        }

        public static void LoadComboxIndex<TEnum>(ComboBox comboBox, ref TEnum variable)
        {
            if (comboBox.Items.Count == 0)
                return;

            comboBox.SelectedIndex = ((int)(object)variable).Clamp(0, comboBox.Items.Count - 1);
        }
    }
}
