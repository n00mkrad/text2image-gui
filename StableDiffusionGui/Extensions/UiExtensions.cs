using Newtonsoft.Json.Linq;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace StableDiffusionGui.Extensions
{
    public static class UiExtensions
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool LockWindowUpdate(IntPtr hWndLock);

        public static void Suspend(this Control control)
        {
            LockWindowUpdate(control.Handle);
        }

        public static void Resume(this Control control)
        {
            LockWindowUpdate(IntPtr.Zero);
        }

        public enum SelectMode { Retain, None, First, Last }

        public static void SetItems(this ComboBox combox, IEnumerable<object> items, SelectMode select = SelectMode.Retain, SelectMode fallback = SelectMode.First)
        {
            string prevText = combox.Text;

            combox.Items.Clear();
            combox.Items.AddRange(items.ToArray());

            if (select == SelectMode.Retain)
            {
                if (combox.Items.Cast<string>().Contains(prevText))
                {
                    combox.Text = prevText;
                }
                else
                {
                    if (fallback == SelectMode.First)
                    {
                        combox.SelectedIndex = 0;
                        return;
                    }
                    else if (fallback == SelectMode.Last)
                    {
                        combox.SelectedIndex = combox.Items.Count - 1;
                        return;
                    }
                }

                return;
            }

            if (select == SelectMode.First)
            {
                combox.SelectedIndex = 0;
                return;
            }

            if (select == SelectMode.Last)
            {
                combox.SelectedIndex = combox.Items.Count - 1;
                return;
            }
        }

        public static DialogResult ShowDialogForm(this Form form, IWin32Window owner, float darken = 0.5f)
        {
            return ShowDialogForm(form, owner, darken);
        }

        public static DialogResult ShowDialogForm(this Form form, float darken = 0.5f, IWin32Window owner = null)
        {
            if(darken < 0.01f || Program.MainForm.Controls.ContainsKey("overlayPanel"))
                return form.ShowDialog(owner);

            Bitmap screenshot = new Bitmap(Program.MainForm.Size.Width, Program.MainForm.Size.Height);

            using (Graphics gfx = Graphics.FromImage(screenshot))
            {
                gfx.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                gfx.CopyFromScreen(Program.MainForm.PointToScreen(new Point()), new Point(), Program.MainForm.Size);

                using (Brush brush = new SolidBrush(Color.FromArgb((255 * darken).RoundToInt(), Color.Black)))
                    gfx.FillRectangle(brush, Program.MainForm.ClientRectangle);
            }

            using (Panel overlay = new Panel() { Name = "overlayPanel", Size = Program.MainForm.Size, BackgroundImage = screenshot, BackgroundImageLayout = ImageLayout.Stretch } )
            {
                overlay.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                Program.MainForm.Controls.Add(overlay);
                overlay.BringToFront();

                Program.MainForm.Refresh();

                return form.ShowDialog(owner);
            }
        }
    }
}
