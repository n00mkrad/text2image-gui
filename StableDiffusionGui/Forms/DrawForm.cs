using Dasync.Collections;
using ImageMagick;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Point = System.Drawing.Point;

namespace StableDiffusionGui.Forms
{
    public partial class DrawForm : CustomForm
    {
        public Image BackgroundImg;
        public Image Mask;

        private Bitmap _raw;

        private List<Bitmap> _history = new List<Bitmap>();
        private int _historyLimit = 200;

        private Point _lastPoint = Point.Empty;
        private bool _mouseDown;

        public DrawForm(Image background, Image mask = null)
        {
            Opacity = 0;
            BackgroundImg = background;

            if (mask != null)
                _raw = mask as Bitmap;
            else
                _raw = new Bitmap(BackgroundImg.Width, BackgroundImg.Height);

            HistorySave();
            InitializeComponent();
        }

        private void DrawForm_Load(object sender, EventArgs e)
        {
            AllowEscClose = true;
        }

        private async void DrawForm_Shown(object sender, EventArgs e)
        {
            Size = new Size((Program.MainForm.Size.Width * 0.8f).RoundToInt(), (Program.MainForm.Size.Height * 0.9f).RoundToInt());
            MinimumSize = new Size((Program.MainForm.Size.Width * 0.4f).RoundToInt(), (Program.MainForm.Size.Height * 0.6f).RoundToInt());
            CenterToScreen();

            tableLayoutPanelImg.ColumnStyles.Cast<ColumnStyle>().ToList().ForEach(s => s.SizeType = SizeType.Absolute);
            tableLayoutPanelImg.RowStyles.Cast<RowStyle>().ToList().ForEach(s => s.SizeType = SizeType.Absolute);

            pasteMaskToolStripMenuItem.Visible = EnabledFeatures.MaskPasting;
            invertMaskToolStripMenuItem.Visible = EnabledFeatures.MaskInversion;

            if (InpaintingUtils.CurrentBlurValue >= 0)
                sliderBlur.Value = InpaintingUtils.CurrentBlurValue;
            else
                InpaintingUtils.CurrentBlurValue = sliderBlur.Value;

            pictBox.BackgroundImage = BackgroundImg;
            SetPictureBoxPadding();
            Blur();
            await Task.Delay(1);
            Opacity = 1;
        }

        private void pictBox_Click(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Right)
                menuStripOptions.Show(Cursor.Position);
        }

        private void pictBox_MouseDown(object sender, MouseEventArgs e)
        {
            _lastPoint = e.Location;
            _mouseDown = true;
        }

        private void pictBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_mouseDown || _lastPoint == null || e.Button != MouseButtons.Left)
                return;

            if (pictBox.Image == null)
                pictBox.Image = new Bitmap(BackgroundImg.Width, BackgroundImg.Height);

            int brushSize = sliderBrushSize.Value;

            using (Graphics g = Graphics.FromImage(_raw))
            {
                float scaleFactor = (float)pictBox.Width / pictBox.Image.Width;
                Point scaledPoint = new Point((_lastPoint.X / scaleFactor).RoundToInt(), (_lastPoint.Y / scaleFactor).RoundToInt());
                g.DrawEllipse(new Pen(Color.Black, brushSize), new RectangleF(scaledPoint, new SizeF(brushSize, brushSize)));
                g.SmoothingMode = SmoothingMode.HighQuality;
            }

            Blur();
            pictBox.Invalidate();
            _lastPoint = e.Location;
        }

        private void pictBox_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseDown = false;
            _lastPoint = Point.Empty;
            HistorySave();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _raw = new Bitmap(BackgroundImg.Width, BackgroundImg.Height);
            pictBox.Image = null;
            Invalidate();
        }

        private void sliderBlur_Scroll(object sender, ScrollEventArgs e)
        {
            InpaintingUtils.CurrentBlurValue = sliderBlur.Value;
            Blur();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Mask = pictBox.Image;
            Close();
        }

        private void invertMaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var magickImg = ImgUtils.GetMagickImage(_raw);
            magickImg = ImgUtils.RemoveTransparency(magickImg, ImgUtils.NoAlphaMode.Fill, MagickColors.White);
            magickImg = ImgUtils.Invert(magickImg);
            magickImg = ImgUtils.ReplaceColorWithTransparency(magickImg, MagickColors.White);
            _raw = magickImg.ToBitmap();
            Blur();
            pictBox.Invalidate();
        }

        private void Blur()
        {
            if (_raw != null)
                pictBox.Image = new GaussianBlur(_raw).Process(InpaintingUtils.CurrentBlurValue);
        }

        // I could not get KeyPreview to work in this Form for whatever reason, so I use this alternative method of processing keys
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.V)) // Hotkey: Paste mask
                PasteImage();

            if (keyData == (Keys.Control | Keys.Z)) // Hotkey: Undo
                HistoryUndo();

            if (keyData == Keys.Return) // Hotkey: OK
                btnOk_Click(null, null);

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void pasteMaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteImage();
        }

        private void PasteImage()
        {
            if (!EnabledFeatures.MaskPasting)
                return;

            Image clipboardImg = Clipboard.GetImage();

            if (clipboardImg != null)
            {
                if (clipboardImg.Size != pictBox.Image.Size)
                {
                    UiUtils.ShowMessageBox($"The pasted mask ({clipboardImg.Width}x{clipboardImg.Height}) needs to have the same dimensions as the initialization image ({pictBox.Image.Width}x{pictBox.Image.Height}).");
                    return;
                }

                var magickImg = ImgUtils.GetMagickImage(clipboardImg);
                Image pastedMask = ImgUtils.ReplaceOtherColorsWithTransparency(magickImg).ToBitmap();
                _raw = (Bitmap)pastedMask;
                sliderBlur.Value = 0;
                sliderBlur_Scroll(null, null);
                pictBox.Invalidate();
                HistorySave();
            }
        }

        private void HistorySave()
        {
            if (_history.Count >= _historyLimit)
                _history = _history.Skip(1).ToList(); // Remove first (oldest) entry if we maxed out the capacity

            _history.Add(new Bitmap(_raw));
        }

        private void HistoryUndo()
        {
            if (_history.Count <= 1)
                return;

            _history.Remove(_history.Last());
            _raw = new Bitmap(_history.Last());

            sliderBlur_Scroll(null, null);
            pictBox.Invalidate();
        }

        private void DrawForm_SizeChanged(object sender, EventArgs e)
        {
            SetPictureBoxPadding();
        }

        private void SetPictureBoxPadding ()
        {
            Size frameSize = tableLayoutPanelImg.Size;
            Size imageSize = BackgroundImg.Size;

            Size targetImgBoxSize = ImgMaths.FitIntoFrame(imageSize, frameSize);

            int padTopBot = ((tableLayoutPanelImg.Size.Height - targetImgBoxSize.Height) / 2f).RoundToInt();
            int padSides = ((tableLayoutPanelImg.Size.Width - targetImgBoxSize.Width) / 2f).RoundToInt();

            tableLayoutPanelImg.ColumnStyles[0].Width = padSides;
            tableLayoutPanelImg.ColumnStyles[1].Width = targetImgBoxSize.Width;
            tableLayoutPanelImg.ColumnStyles[2].Width = padSides;

            tableLayoutPanelImg.RowStyles[0].Height = padTopBot;
            tableLayoutPanelImg.RowStyles[1].Height = targetImgBoxSize.Height;
            tableLayoutPanelImg.RowStyles[2].Height = padTopBot;
        }
    }
}
