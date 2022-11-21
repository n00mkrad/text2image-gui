using StableDiffusionGui.Extensions;
using StableDiffusionGui.Forms;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace StableDiffusionGui.Ui
{
    internal class MainUiHotkeys
    {
        private static bool _anyTextboxFocused { get { return Program.MainForm.GetControls().Where(control => control.Focused && control is TextBox).Any(); } }

        public static void Handle (Keys keys)
        {
            if (keys == (Keys.Control | Keys.Q)) // Hotkey: Quit
                Program.MainForm.Close();

            if (keys == (Keys.Control | Keys.V) && !_anyTextboxFocused) // Hotkey: Paste image
                MainUi.HandlePaste();

            if (keys == (Keys.Control | Keys.G)) // Hotkey: Generate/Cancel
                Program.MainForm.runBtn.PerformClick();

            if (keys == (Keys.Control | Keys.Delete) && !InputUtils.IsKeyPressed(Key.Back)) // Hotkey: Delete generated image
                MainForm.ImageViewer.DeleteCurrent();

            if (keys == (Keys.Control | Keys.Shift | Keys.Delete) && !InputUtils.IsKeyPressed(Key.Back)) // Hotkey: Delete all generated images
                MainForm.ImageViewer.DeleteAll();

            if (keys == (Keys.Control | Keys.Add) || keys == (Keys.Control | Keys.Oemplus)) // Hotkey: Toggle prompt field size
                MainUi.SetPromptFieldSize(MainUi.PromptFieldSizeMode.Toggle, false);

            if (keys == (Keys.Control | Keys.Shift | Keys.Add) || keys == (Keys.Control | Keys.Shift | Keys.Oemplus)) // Hotkey: Toggle negative prompt field size
                MainUi.SetPromptFieldSize(MainUi.PromptFieldSizeMode.Toggle, true);

            if (keys == (Keys.Control | Keys.C) && !_anyTextboxFocused) // Hotkey: Copy current image
                OsUtils.SetClipboard(Program.MainForm.pictBoxImgViewer.Image);

            if (keys == (Keys.Control | Keys.O)) // Hotkey: Open current image
                MainForm.ImageViewer.OpenCurrent();

            if (keys == (Keys.Control | Keys.Shift | Keys.O)) // Hotkey: Open folder of current image
                MainForm.ImageViewer.OpenFolderOfCurrent();

            if (keys == (Keys.Control | Keys.D)) // Hotkey: Copy current image to favs
                MainForm.ImageViewer.CopyCurrentToFavs();

            if (keys == (Keys.Control | Keys.M)) // Hotkey: Model quick switcher
            {
                Program.MainForm.panelSettings.Focus();
                new ModelQuickSelectForm(Enums.StableDiffusion.ModelType.Normal).ShowDialogForm(0f);
            }

            if (keys == (Keys.Control | Keys.Shift | Keys.M)) // Hotkey: VAE quick switcher
            {
                Program.MainForm.panelSettings.Focus();
                new ModelQuickSelectForm(Enums.StableDiffusion.ModelType.Vae).ShowDialogForm(0f);
            }

            if (keys == (Keys.Control | Keys.P)) // Hotkey: Post-process current image
                Program.MainForm.ShowPostProcessMenu();

            if (keys == Keys.F1) // Hotkey: Help
                Process.Start("https://github.com/n00mkrad/text2image-gui/blob/main/README.md");

            if (keys == Keys.F12) // Hotkey: Open settings
                new SettingsForm().ShowDialogForm(0.5f);

            if (keys == Keys.Escape) // Hotkey: Remove focus from focused control
                Program.MainForm.panelSettings.Focus();
        }
    }
}
