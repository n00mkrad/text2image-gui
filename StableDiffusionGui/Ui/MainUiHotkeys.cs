using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Ui
{
    internal class MainUiHotkeys
    {
        public static void Handle (Keys keys)
        {
            if (keys == (Keys.Control | Keys.Q)) // Hotkey: Quit
                Program.MainForm.Close();

            if (keys == (Keys.Control | Keys.V)) // Hotkey: Paste image
                MainUi.HandlePaste();

            if (keys == (Keys.Control | Keys.G)) // Hotkey: Generate/Cancel
                Program.MainForm.RunBtn.PerformClick();

            if (keys == (Keys.Control | Keys.Delete)) // Hotkey: Delete generated image
                ImagePreview.DeleteCurrent();

            if (keys == (Keys.Control | Keys.Shift | Keys.Delete)) // Hotkey: Delete all generated images
                ImagePreview.DeleteAll();

            if (keys == (Keys.Control | Keys.Add) || keys == (Keys.Control | Keys.Oemplus)) // Hotkey: Toggle prompt field size
                MainUi.SetPromptFieldSize(MainUi.PromptFieldSizeMode.Toggle);
        }
    }
}
