using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using System.Linq;

namespace StableDiffusionGui.MiscUtils
{
    internal class InputUtils
    {
        public static bool IsHoldingShift { get { return Keyboard.Modifiers == ModifierKeys.Shift; } }
        public static bool IsHoldingCtrl { get { return Keyboard.Modifiers == ModifierKeys.Control; } }
        public static bool IsHoldingAlt { get { return Keyboard.Modifiers == ModifierKeys.Alt; } }
        public static bool IsHoldingWin { get { return Keyboard.Modifiers == ModifierKeys.Windows; } }

        private static List<Key> _keysCached;
        public static List<Key> KeysCached { get { if (_keysCached == null) _keysCached = Enum.GetValues(typeof(Key)).Cast<Key>().ToList(); return _keysCached; } }

        public static List<Key> GetPressedKeys ()
        {
            return KeysCached.Where(key => key != Key.None && Keyboard.IsKeyDown(key)).ToList(); // Should be fairly fast thanks to cached key list
        }

        public static bool IsKeyPressed(Key key)
        {
            return GetPressedKeys().Contains(key);
        }
    }
}
