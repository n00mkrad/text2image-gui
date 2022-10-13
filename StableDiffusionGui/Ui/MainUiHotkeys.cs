using StableDiffusionGui.Forms;
using StableDiffusionGui.MiscUtils;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Input;

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

            if (keys == (Keys.Control | Keys.Delete) && !InputUtils.IsKeyPressed(Key.Back)) // Hotkey: Delete generated image
                ImagePreview.DeleteCurrent();

            if (keys == (Keys.Control | Keys.Shift | Keys.Delete) && !InputUtils.IsKeyPressed(Key.Back)) // Hotkey: Delete all generated images
                ImagePreview.DeleteAll();

            if (keys == (Keys.Control | Keys.Add) || keys == (Keys.Control | Keys.Oemplus)) // Hotkey: Toggle prompt field size
                MainUi.SetPromptFieldSize(MainUi.PromptFieldSizeMode.Toggle);

            if (keys == (Keys.Control | Keys.O)) // Hotkey: Open current image
                ImagePreview.OpenCurrent();

            if (keys == (Keys.Control | Keys.Shift | Keys.O)) // Hotkey: Open folder of current image
                ImagePreview.OpenFolderOfCurrent();

            if (keys == (Keys.Control | Keys.M)) // Hotkey: Model quick switcher
            {
                Program.MainForm.PanelBg.Focus();
                new ModelQuickSelectForm().ShowDialog();
            }

            if (keys == (Keys.F1)) // Hotkey: Help
                Process.Start("https://github.com/n00mkrad/text2image-gui/blob/main/README.md");

            if (keys == (Keys.F12)) // Hotkey: Open settings
                new SettingsForm().ShowDialog();
        }
    }
}
