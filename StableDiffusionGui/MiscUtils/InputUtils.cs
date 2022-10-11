namespace StableDiffusionGui.MiscUtils
{
    internal class InputUtils
    {
        public static bool IsHoldingShift { get { return System.Windows.Input.Keyboard.Modifiers == System.Windows.Input.ModifierKeys.Shift; } }
        public static bool IsHoldingCtrl { get { return System.Windows.Input.Keyboard.Modifiers == System.Windows.Input.ModifierKeys.Control; } }
        public static bool IsHoldingAlt { get { return System.Windows.Input.Keyboard.Modifiers == System.Windows.Input.ModifierKeys.Alt; } }
        public static bool IsHoldingWin { get { return System.Windows.Input.Keyboard.Modifiers == System.Windows.Input.ModifierKeys.Windows; } }
    }
}
