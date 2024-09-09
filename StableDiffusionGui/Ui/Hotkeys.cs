using StableDiffusionGui.Extensions;
using StableDiffusionGui.Forms;
using StableDiffusionGui.Implementations;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace StableDiffusionGui.Ui
{
    internal class Hotkeys
    {
        private static bool _anyTextboxFocused { get { return Program.MainForm.GetControls().Where(control => control.Focused && control is TextBox).Any(); } }

        public static void HandleMainForm (Keys keys)
        {
            if (keys == (Keys.Control | Keys.Q)) // Hotkey: Quit
            {
                Program.MainForm.Close();
                return;
            }

            if (keys == (Keys.Control | Keys.V) && !_anyTextboxFocused) // Hotkey: Paste image
            {
                MainUi.HandlePaste();
                return;
            }

            if (keys == (Keys.Control | Keys.G)) // Hotkey: Generate/Cancel
            {
                Program.MainForm.runBtn.PerformClick();
                return;
            }

            if (keys == (Keys.Control | Keys.Delete) && !InputUtils.IsKeyPressed(Key.Back)) // Hotkey: Delete generated image
            {
                ImageViewer.DeleteCurrent();
                return;
            }

            if (keys == (Keys.Control | Keys.Shift | Keys.Delete) && !InputUtils.IsKeyPressed(Key.Back)) // Hotkey: Delete all generated images
            {
                ImageViewer.DeleteAll();
                return;
            }

            if (keys == (Keys.Control | Keys.Add) || keys == (Keys.Control | Keys.Oemplus)) // Hotkey: Toggle prompt field size
            {
                Program.MainForm.InvokeIfNeeded(() => Program.MainForm.btnExpandPromptField_Click(null, null));
                return;
            }

            if (keys == (Keys.Control | Keys.Shift | Keys.Add) || keys == (Keys.Control | Keys.Shift | Keys.Oemplus)) // Hotkey: Toggle negative prompt field size
            {
                Program.MainForm.InvokeIfNeeded(() => Program.MainForm.btnExpandPromptNegField_Click(null, null));
                return;
            }

            if (keys == (Keys.Control | Keys.C) && !_anyTextboxFocused) // Hotkey: Copy current image
            {
                OsUtils.SetClipboard(Program.MainForm.pictBoxImgViewer.GetImageSafe());
                return;
            }

            if (keys == (Keys.Control | Keys.O)) // Hotkey: Open current image
            {
                ImageViewer.OpenCurrent();
                return;
            }

            if (keys == (Keys.Control | Keys.Shift | Keys.O)) // Hotkey: Open folder of current image
            {
                ImageViewer.OpenFolderOfCurrent();
                return;
            }

            if (keys == (Keys.Control | Keys.S)) // Hotkey: Copy current image to favs
            {
                ImageViewer.CopyCurrentToFavs();
                return;
            }

            if (keys == (Keys.Control | Keys.M)) // Hotkey: Model quick switcher
            {
                Program.MainForm.panelSettings.Focus();
                new ModelQuickSelectForm(Enums.Models.Type.Normal).ShowDialogForm();
                return;
            }

            if (keys == (Keys.Control | Keys.Shift | Keys.M)) // Hotkey: VAE quick switcher
            {
                Program.MainForm.panelSettings.Focus();
                new ModelQuickSelectForm(Enums.Models.Type.Vae).ShowDialogForm();
                return;
            }

            if (keys == (Keys.Control | Keys.P)) // Hotkey: Post-process current image
            {
                Program.MainForm.ShowPostProcessMenu();
                return;
            }

            if (keys == (Keys.Control | Keys.I)) // Hotkey: Toggle input image preview
            {
                if(Program.MainForm.checkboxShowInitImg.Visible)
                    Program.MainForm.checkboxShowInitImg.Checked = !Program.MainForm.checkboxShowInitImg.Checked;

                return;
            }

            if (keys == Keys.F1) // Hotkey: Help
            {
                Process.Start("https://github.com/n00mkrad/text2image-gui/blob/main/README.md");
                return;
            }

            if (keys == Keys.F4 && Program.Debug)
            {
                IInvokePatcher patcher = new InvokePatcher235();
                patcher.Run();
                return;
            }

            if (keys == Keys.F5)
            {
                Program.MainForm.TryRefreshUiState();
                return;
            }

            if (keys == Keys.F8 && Program.Debug)
            {
                new ComfyPatcher().Run();
                return;
            }

            // if (keys == Keys.F10 && Program.Debug) // Hotkey: UI TEST
            // {
            //     UiConstruction.CreateMainWindowOption();
            //     return;
            // }

            if (keys == Keys.F11) // Hotkey: Open Log Viewer
            {
                Program.MainForm.OpenLogViewerWindow();
                return;
            }

            if (keys == Keys.F12) // Hotkey: Open settings
            {
                new SettingsForm().ShowDialogForm();
                return;
            }

            if (keys == Keys.Escape) // Hotkey: Remove focus from focused control
            {
                Program.MainForm.panelSettings.Focus();
                return;
            }

            if (keys == (Keys.Control | Keys.Right) && !_anyTextboxFocused) // Hotkey: Next image
            {
                ImageViewer.Move(false);
                return;
            }

            if (keys == (Keys.Control | Keys.Left) && !_anyTextboxFocused) // Hotkey: Previous image
            {
                ImageViewer.Move(true);
                return;
            }
        }

        public static void HandleImageViewer(Keys keys, ImagePopupForm form, bool slideshowMode)
        {
            if (keys == Keys.Escape || keys == Keys.Q)
            {
                form.Close();
                return;
            }

            if (keys == (Keys.Control | Keys.C) && !_anyTextboxFocused) // Hotkey: Copy current image
            {
                OsUtils.SetClipboard(Program.MainForm.pictBoxImgViewer.GetImageSafe());
                return;
            }

            if (keys == (Keys.Control | Keys.O)) // Hotkey: Open current image
            {
                ImageViewer.OpenCurrent();
                return;
            }

            if (keys == (Keys.Control | Keys.Shift | Keys.O)) // Hotkey: Open folder of current image
            {
                ImageViewer.OpenFolderOfCurrent();
                return;
            }

            if (keys == (Keys.Control | Keys.D)) // Hotkey: Copy current image to favs
            {
                ImageViewer.CopyCurrentToFavs();
                return;
            }

            if (keys == Keys.Right && form.SlideshowMode) // Hotkey: Next image
            {
                ImageViewer.Move(false);
                return;
            }

            if (keys == Keys.Left && form.SlideshowMode) // Hotkey: Previous image
            {
                ImageViewer.Move(true);
                return;
            }

            if (keys == Keys.T)
            {
                form.CycleTiling();
                return;
            }
        }
    }
}
