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

        public static DialogResult ShowDialogForm(this Form form, IWin32Window owner = null)
        {
            return form.ShowDialog(owner);
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
