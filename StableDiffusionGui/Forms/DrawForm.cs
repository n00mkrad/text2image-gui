using Dasync.Collections;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using StableDiffusionGui.Ui.DrawForm;

namespace StableDiffusionGui.Forms
{
    public partial class DrawForm : CustomForm
    {
        public Image BackgroundImg;
        public Image Mask;

        public Bitmap RawMask;

        public List<Bitmap> History = new List<Bitmap>();
        public int HistoryLimit = 200;

        public DrawForm(Image background, Image mask = null)
        {
            FormControls.F = this;
            FormUtils.F = this;

            FormUtils.Reset();

            Opacity = 0;
            BackgroundImg = background;

            if (mask != null)
                RawMask = mask as Bitmap;
            else
                RawMask = new Bitmap(BackgroundImg.Width, BackgroundImg.Height);

            FormUtils.HistorySave();
            InitializeComponent();
        }

        private void DrawForm_Load(object sender, EventArgs e)
        {
            AllowEscClose = true;
        }

        private async void DrawForm_Shown(object sender, EventArgs e)
        {
            Size = new Size((Program.MainForm.Size.Width * 0.6f).RoundToInt(), (Program.MainForm.Size.Height * 1.0f).RoundToInt());
            MinimumSize = new Size((Program.MainForm.Size.Width * 0.25f).RoundToInt(), (Program.MainForm.Size.Height * 0.5f).RoundToInt());
            CenterToScreen();

            tableLayoutPanelImg.ColumnStyles.Cast<ColumnStyle>().ToList().ForEach(s => s.SizeType = SizeType.Absolute);
            tableLayoutPanelImg.RowStyles.Cast<RowStyle>().ToList().ForEach(s => s.SizeType = SizeType.Absolute);

            if (Inpainting.CurrentBlurValue >= 0)
                sliderBlur.Value = Inpainting.CurrentBlurValue;
            else
                Inpainting.CurrentBlurValue = sliderBlur.Value;

            pictBox.BackgroundImage = BackgroundImg;
            FormControls.SetPictureBoxPadding();
            FormUtils.Apply();
            await Task.Delay(1);
            Opacity = 1;
        }

        private void pictBox_Click(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Right)
                FormControls.ShowContextMenu();
        }

        private void pictBox_MouseDown(object sender, MouseEventArgs e)
        {
            FormUtils.DrawStart(e.Location);
        }

        private void pictBox_MouseMove(object sender, MouseEventArgs e)
        {
            FormUtils.Draw(e);
        }

        private void pictBox_MouseUp(object sender, MouseEventArgs e)
        {
            FormUtils.DrawEnd();
        }

        public void sliderBlur_Scroll(object sender, ScrollEventArgs e)
        {
            Inpainting.CurrentBlurValue = sliderBlur.Value;
            FormUtils.Apply();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Mask = pictBox.Image;
            Close();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.I)) // Hotkey: Invert
                FormUtils.InvertMask();

            if (keyData == (Keys.Control | Keys.V)) // Hotkey: Paste mask
                FormUtils.PasteMask();

            if (keyData == (Keys.Control | Keys.S)) // Hotkey: Save
                FormUtils.SaveMask();

            if (keyData == (Keys.Control | Keys.O)) // Hotkey: Load
                FormUtils.LoadMask();

            if (keyData == (Keys.Control | Keys.Z)) // Hotkey: Undo
                FormUtils.HistoryUndo();

            if (keyData == Keys.Return) // Hotkey: OK
                btnOk_Click(null, null);

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void DrawForm_SizeChanged(object sender, EventArgs e)
        {
            FormControls.SetPictureBoxPadding();
        }

        #region Context Menu

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormUtils.ClearMask();
        }

        private void invertMaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormUtils.InvertMask();
        }

        private void pasteMaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormUtils.PasteMask();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormUtils.SaveMask();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormUtils.LoadMask();
        }

        #endregion

        #region Buttons Bottom Left

        private void btnClear_Click(object sender, EventArgs e)
        {
            FormUtils.ClearMask();
        }

        private void btnMaskSave_Click(object sender, EventArgs e)
        {
            FormUtils.SaveMask();
        }

        private void btnMaskLoad_Click(object sender, EventArgs e)
        {
            FormUtils.LoadMask();
        }

        #endregion

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormUtils.HistoryUndo();
        }
    }
}
