using MS.WindowsAPICodePack.Internal;
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

        public static DialogResult ShowDialogForm(this Form form, IWin32Window owner, float darken = 0.0f)
        {
            return ShowDialogForm(form, darken, owner);
        }

        public static DialogResult ShowDialogForm(this Form form, float darken = 0.0f, IWin32Window owner = null)
        {
            if (darken < 0.01f || Program.MainForm.Controls.ContainsKey("overlayPanel"))
                return form.ShowDialog(owner);

            Bitmap screen = new Bitmap(Program.MainForm.Width, Program.MainForm.Height);

            Point clientAreaStartingPoint = Program.MainForm.PointToClient(Program.MainForm.Location);
            Program.MainForm.DrawToBitmap(screen, new Rectangle(0, 0, Program.MainForm.Width, Program.MainForm.Height));
            screen = screen.Clone(new Rectangle(-clientAreaStartingPoint.X, -clientAreaStartingPoint.Y, Program.MainForm.ClientRectangle.Width, Program.MainForm.ClientRectangle.Height), screen.PixelFormat);

            using (Graphics gfx = Graphics.FromImage(screen))
                using (Brush brush = new SolidBrush(Color.FromArgb((255 * darken).RoundToInt(), Color.Black)))
                    gfx.FillRectangle(brush, Program.MainForm.ClientRectangle);

            using (Panel overlay = new Panel() { Name = "overlayPanel", Size = Program.MainForm.ClientRectangle.Size, BackgroundImage = screen, BackgroundImageLayout = ImageLayout.Stretch } )
            {
                overlay.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                Program.MainForm.Controls.Add(overlay);
                overlay.BringToFront();

                Program.MainForm.Refresh();

                return form.ShowDialog(owner);
            }
        }

        public static List<Control> GetControls(this Control control)
        {
            List<Control> list = new List<Control>();
            var controls = control.Controls.Cast<Control>().ToList();
            list.AddRange(controls);
            controls.ForEach(c => list.AddRange(c.GetControls()));
            return list;
        }
    }
}
