using Dasync.Collections;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class DrawForm : CustomForm
    {
        public Image BackgroundImg;
        public Image Mask;

        public Bitmap RawMask;

        public List<Bitmap> History = new List<Bitmap>();
        private readonly int _historyLimit = 200; // Reference value for 512x512
        public int HistoryLimitNormalized; // Adjusted for resolution to avoid higher than expected RAM usage
        public bool DisableBlurOption;

        public DrawForm(Image background, Image mask = null, bool disableBlurOption = false)
        {
            Reset();
            DialogResult = DialogResult.None;

            Opacity = 0;
            BackgroundImg = background;
            DisableBlurOption = disableBlurOption;

            if (mask != null)
                RawMask = mask as Bitmap;
            else
                RawMask = new Bitmap(BackgroundImg.Width, BackgroundImg.Height);

            float pixelCountFactor = (512 * 512) / (float)(BackgroundImg.Width * BackgroundImg.Height);
            HistoryLimitNormalized = (_historyLimit * pixelCountFactor).RoundToInt().Clamp(10, _historyLimit * 2);

            HistorySave();
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

            if (DisableBlurOption)
            {
                sliderBlur.Visible = false;
                Inpainting.CurrentBlurValue = 0;
            }
            else
            {
                if (Inpainting.CurrentBlurValue >= 0)
                    sliderBlur.Value = Inpainting.CurrentBlurValue;
                else
                    Inpainting.CurrentBlurValue = sliderBlur.Value;
            }

            pictBox.BackgroundImage = BackgroundImg;
            SetPictureBoxPadding();
            Apply();
            await Task.Delay(1);
            Opacity = 1;
        }

        private void pictBox_Click(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Right)
                ShowContextMenu();
        }

        private void pictBox_MouseDown(object sender, MouseEventArgs e)
        {
            DrawStart(e.Location);
        }

        private void pictBox_MouseMove(object sender, MouseEventArgs e)
        {
            Draw(e);
        }

        private void pictBox_MouseUp(object sender, MouseEventArgs e)
        {
            DrawEnd();
        }

        public void sliderBlur_Scroll(object sender, ScrollEventArgs e)
        {
            Inpainting.CurrentBlurValue = sliderBlur.Value;
            Apply();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Mask = pictBox.Image;
            Close();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.I)) // Hotkey: Invert
                InvertMask();

            if (keyData == (Keys.Control | Keys.V)) // Hotkey: Paste mask
                PasteMask();

            if (keyData == (Keys.Control | Keys.S)) // Hotkey: Save
                SaveMask();

            if (keyData == (Keys.Control | Keys.O)) // Hotkey: Load
                LoadMask();

            if (keyData == (Keys.Control | Keys.Z)) // Hotkey: Undo
                HistoryUndo();

            if (keyData == Keys.Return) // Hotkey: OK
                btnOk_Click(null, null);

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void DrawForm_SizeChanged(object sender, EventArgs e)
        {
            SetPictureBoxPadding();
        }

        #region Context Menu

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearMask();
        }

        private void invertMaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InvertMask();
        }

        private void pasteMaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteMask();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveMask();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadMask();
        }

        #endregion

        #region Buttons Bottom Left

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearMask();
        }

        private void btnMaskSave_Click(object sender, EventArgs e)
        {
            SaveMask();
        }

        private void btnMaskLoad_Click(object sender, EventArgs e)
        {
            LoadMask();
        }

        #endregion

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HistoryUndo();
        }

        private void DrawForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
                Inpainting.CurrentRawMask = RawMask;
        }
    }
}
