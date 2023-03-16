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
            if (combox.AreItemsEqualToList(items.ToArray()))
                return;

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

        private static bool AreItemsEqualToList(this ComboBox combox, object[] newItems)
        {
            if (combox.Items.Count != newItems.Count())
                return false;

            for (int i = 0; i < combox.Items.Count; i++)
            {
                if (combox.Items[i].ToString() != newItems[i].ToString())
                    return false;
            }

            return true;
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

        /// <summary> Sets the visibility of a control while avoiding unnecessary setter calls.  </summary>
        public static void SetVisible(this Control c, bool targetState)
        {
            if (c.Visible != targetState)
                c.Visible = targetState;
        }

        /// <summary> WM_SETREDRAW message is sent to a window to allow changes to be redrawn or to prevent changes from being redrawn. </summary>
        private const int WM_SETREDRAW = 11;

        /// <summary> Suspends painting for the target control. Do not forget to call ResumeRendering! </summary>
        public static void StopRendering(this Control control)
        {
            Message msgSuspendUpdate = Message.Create(control.Handle, WM_SETREDRAW, IntPtr.Zero,
                  IntPtr.Zero);

            NativeWindow window = NativeWindow.FromHandle(control.Handle);
            window.DefWndProc(ref msgSuspendUpdate);
        }

        /// <summary> Resumes painting for the target control. Intended to be called following a call to StopRendering(). </summary>
        public static void ResumeRendering(this Control control)
        {
            IntPtr wparam = new IntPtr(1); // Create a C "true" boolean as an IntPtr
            Message msgResumeUpdate = Message.Create(control.Handle, WM_SETREDRAW, wparam, IntPtr.Zero);
            NativeWindow window = NativeWindow.FromHandle(control.Handle);
            window.DefWndProc(ref msgResumeUpdate);
            control.Invalidate();
            control.Refresh();
        }

        public static void RunWithUiStopped(this Action action, Form form, bool dontPauseInDebugMode = false)
        {
            if (!(dontPauseInDebugMode && Program.Debug))
                form.StopRendering();

            action.RunInTryCatch();

            if (!(dontPauseInDebugMode && Program.Debug))
                form.ResumeRendering();
        }

        public static void PrintThread (this Control c, bool hidden = true)
        {
            Logger.Log($"Currently{(c.InvokeRequired ? " NOT" : "")} on UI thread ({c.Name})", hidden);
        }
    }
}
