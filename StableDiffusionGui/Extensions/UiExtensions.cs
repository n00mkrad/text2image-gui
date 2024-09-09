using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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

        /// <summary> Sets ComboBox items, but only if the list has actually changed </summary>
        /// <returns> true if the list was changed, false if the new list is identical with the existing one </returns>
        public static bool SetItems(this ComboBox combox, IEnumerable<object> items, SelectMode select = SelectMode.Retain, SelectMode fallback = SelectMode.First)
        {
            var newItems = items as object[] ?? items.ToArray();

            if (combox.AreItemsEqualToList(newItems))
                return false;

            string prevText = combox.Text;
            combox.Items.Clear();
            combox.Items.AddRange(newItems);

            if (select == SelectMode.Retain && combox.Items.Cast<object>().Any(o => o.ToString() == prevText))
            {
                combox.Text = prevText;
            }
            else if ((select == SelectMode.Retain && fallback == SelectMode.First) || select == SelectMode.First)
            {
                combox.SelectedItem = combox.Items.Cast<object>().FirstOrDefault();
            }
            else if ((select == SelectMode.Retain && fallback == SelectMode.Last) || select == SelectMode.Last)
            {
                combox.SelectedItem = combox.Items.Cast<object>().LastOrDefault();
            }

            return true;
        }

        public static void SetItems(this ComboBox combox, IEnumerable<object> items, int selectIndex = -1)
        {
            if (combox.AreItemsEqualToList(items.ToArray()))
                return;

            combox.Items.Clear();
            combox.Items.AddRange(items.ToArray());

            if (selectIndex >= 0 && combox.Items.Count > 0)
                combox.SelectedIndex = selectIndex;
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

        public static DialogResult ShowDialogForm(this Form form)
        {
            return form.ShowDialogSafe();
        }

        public static List<Control> GetControls(this Control control)
        {
            List<Control> list = new List<Control>();
            var controls = control.Controls.Cast<Control>().ToList();
            list.AddRange(controls);
            controls.ForEach(c => list.AddRange(c.GetControls()));
            return list;
        }

        /// <summary> Sets the visibility of controls while avoiding unnecessary setter calls.  </summary>
        public static void SetVisible(this IEnumerable<Control> c, bool targetState)
        {
            foreach (var control in c)
                control.SetVisible(targetState);
        }

        /// <summary> Sets the visibility of a control while avoiding unnecessary setter calls.  </summary>
        public static void SetVisible(this Control c, bool targetState)
        {
            c.InvokeIfNeeded(() =>
            {
                if (c.Visible != targetState)
                    c.Visible = targetState;
            });
        }

        private static HashSet<Control> _nonRenderingControls = new HashSet<Control>();

        /// <summary> WM_SETREDRAW message is sent to a window to allow changes to be redrawn or to prevent changes from being redrawn. </summary>
        private const int WM_SETREDRAW = 11;

        /// <summary> Suspends painting for the target control. Do not forget to call ResumeRendering! </summary>
        public static void StopRendering(this Control control)
        {
            Message msgSuspendUpdate = Message.Create(control.Handle, WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
            NativeWindow window = NativeWindow.FromHandle(control.Handle);
            window.DefWndProc(ref msgSuspendUpdate);
            _nonRenderingControls.Add(control);
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
            _nonRenderingControls.Remove(control);
        }

        public static bool IsRendering(this Control control)
        {
            return !_nonRenderingControls.Contains(control);
        }

        public static void RunWithUiStopped(this Action action, Form form, string errorPrefix = "", bool showErrors = false, bool dontResumeIfAlreadyStopped = true, bool dontPauseInDebugMode = false)
        {
            if (form.RequiresInvoke(() => action.RunWithUiStopped(form, errorPrefix, showErrors, dontResumeIfAlreadyStopped, dontPauseInDebugMode)))
                return;

            if (dontResumeIfAlreadyStopped && !form.IsRendering())
            {
                action.RunInTryCatch(errorPrefix, showErrors ? errorPrefix : "");
                return;
            }

            if (!(dontPauseInDebugMode && Program.Debug))
                form.StopRendering();

            action.RunInTryCatch(errorPrefix, showErrors ? errorPrefix : "");

            if (!(dontPauseInDebugMode && Program.Debug))
                form.ResumeRendering();
        }

        public static void PrintThread(this Control c, bool hidden = true)
        {
            Logger.Log($"Currently{(c.InvokeRequired ? " NOT" : "")} on UI thread ({c.Name})", hidden);
        }

        public static Font ChangeFontFamily(this Font f, FontFamily newFontFamily)
        {
            if (newFontFamily == null)
                return f;

            return new Font(newFontFamily, f.Size, f.Style, f.Unit);
        }

        public static Font ChangeSize(this Font f, float newSize)
        {
            return new Font(f.FontFamily, newSize, f.Style, f.Unit);
        }

        public static bool RequiresInvoke(this Control c, Delegate method, params object[] args)
        {
            if (c.InvokeRequired)
            {
                if (c.Disposing || c.IsDisposed)
                    return true;

                c.Invoke(method, args);
                return true;
            }

            return false;
        }

        public static bool RequiresInvoke(this Control c, Action action)
        {
            if (c.InvokeRequired)
            {
                if (c.Disposing || c.IsDisposed)
                    return true;

                c.Invoke(action);
                return true;
            }

            return false;
        }

        public static TResult InvokeIfNeeded<TResult>(this Control c, Func<TResult> method)
        {
            if (c.InvokeRequired)
            {
                if (c.Disposing || c.IsDisposed)
                    return default;

                return (TResult)c.Invoke(method);
            }

            return method();
        }

        public static void InvokeIfNeeded(this Control c, Action method)
        {
            if (c.InvokeRequired)
            {
                try
                {
                    if (c.Disposing || c.IsDisposed)
                        return;

                    c.Invoke(method);
                }
                catch { }

                return;
            }

            method();
        }

        public static Image GetImageSafe(this PictureBox pictureBox)
        {
            if (pictureBox.InvokeRequired)
                return (Image)pictureBox.Invoke(new Func<Image>(() => pictureBox.GetImageSafe()));

            try
            {
                if (pictureBox.Image.IsDisposed())
                    pictureBox.Image = null;

                var img = pictureBox.Image == null ? null : (Image)pictureBox.Image.Clone();
                return img;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static void SetValue(this NumericUpDown upDown, decimal value)
        {
            upDown.InvokeIfNeeded(() =>
            {
                if (value <= upDown.Maximum && value >= upDown.Minimum)
                {
                    upDown.Value = value;
                }
            });
        }

        public static void SetTextSafe(this Control control, string text)
        {
            control.InvokeIfNeeded(() => control.Text = text);
        }

        public static void SetTooltipSafe(this ToolTip tooltip, Control control, string text)
        {
            control.InvokeIfNeeded(() => tooltip.SetToolTip(control, text));
        }

        public static void SetEnabled(this Control control, bool targetState)
        {
            control.InvokeIfNeeded(() =>
            {
                if (control.Enabled != targetState)
                    control.Enabled = targetState;
            });
        }

        public static DialogResult ShowDialogSafe(this Form form, IWin32Window owner = null)
        {
            return form.InvokeIfNeeded(() => form.ShowDialog(owner));
        }

        public static void DisableFor(this Control control, int milliseconds)
        {
            control.Enabled = false;

            var timer = new System.Threading.Timer((obj) =>
            {
                if (control.IsDisposed)
                    return;

                if (control.InvokeRequired)
                {
                    control.Invoke(new Action(() =>
                    {
                        control.Enabled = true;
                        ((System.Threading.Timer)control.Tag).Dispose();
                        control.Tag = null;
                    }));
                }
                else
                {
                    control.Enabled = true;
                    ((System.Threading.Timer)control.Tag).Dispose();
                    control.Tag = null;
                }

            }, null, milliseconds, System.Threading.Timeout.Infinite);
            control.Tag = timer; // keep a reference to the timer
        }

        /// <summary> Sets ComboBox index to <paramref name="index"/> if it has any items and current index is -1 </summary>
        public static void InitCombox (this ComboBox c, int index = 0)
        {
            if (c.Items.Count > 0 && c.SelectedIndex < 0)
                c.SelectedIndex = index;
        }

        public static void SetRows(this DataGridView grid, List<DataGridViewRow> rows, bool validate = true)
        {
            if(grid == null || rows.Count == 0)
                return;

            if (validate)
            {
                string rowStrOld = string.Join("", grid.GetRows().Select(row => string.Join("", row.Cells.Cast<DataGridViewCell>().Select(cell => $"{cell.Value}"))));
                string rowStrNew = string.Join("", rows.Select(row => string.Join("", row.Cells.Cast<DataGridViewCell>().Select(cell => $"{cell.Value}"))));

                if (rowStrOld == rowStrNew)
                    return;
            }

            grid.Rows.Clear();
            grid.Rows.AddRange(rows.ToArray());
        }

        public static IEnumerable<DataGridViewRow> GetRows(this DataGridView grid)
        {
            return grid.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow);
        }
    }
}
