using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Ui;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class DrawForm : Form
    {
        public Image BackgroundImg;
        public Image Mask;

        Bitmap _raw;

        public DrawForm(Image background, Image mask = null)
        {
            BackgroundImg = background;

            if(mask != null)
                _raw = mask as Bitmap;
            else
                _raw = new Bitmap(BackgroundImg.Width, BackgroundImg.Height);

            InitializeComponent();
        }

        private void DrawForm_Load(object sender, EventArgs e)
        {
            Width = BackgroundImg.Width + 16;
            Height = BackgroundImg.Height + 144;
            CenterToScreen();

            if (InpaintingUtils.CurrentBlurValue >= 0)
                sliderBlur.Value = InpaintingUtils.CurrentBlurValue;
            else
                InpaintingUtils.CurrentBlurValue = sliderBlur.Value;

            pictBox.BackgroundImage = BackgroundImg;
            Blur();
        }

        Point _lastPoint = Point.Empty;
        bool _mouseDown;

        private void pictBox_Click(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Right)
            {
                menuStripOptions.Show(Cursor.Position);
            }
        }

        private void pictBox_MouseDown(object sender, MouseEventArgs e)
        {
            _lastPoint = e.Location;
            _mouseDown = true;
        }

        private void pictBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_mouseDown || _lastPoint == null || ((MouseEventArgs)e).Button != MouseButtons.Left)
                return;

            if (pictBox.Image == null)
                pictBox.Image = new Bitmap(BackgroundImg.Width, BackgroundImg.Height);

            int brushSize = sliderBrushSize.Value;

            using (Graphics g = Graphics.FromImage(_raw))
            {
                g.DrawEllipse(new Pen(Color.Black, brushSize), new RectangleF(_lastPoint, new SizeF(brushSize, brushSize)));
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

        private void Blur()
        {
            if (_raw != null)
                pictBox.Image = new GaussianBlur(_raw).Process(InpaintingUtils.CurrentBlurValue);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Mask = pictBox.Image;
            Close();
        }
    }
}
