using StableDiffusionGui.MiscUtils;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class DrawForm : Form
    {
        public Image BackgroundImage;
        public Image Mask;

        public DrawForm(Image img)
        {
            BackgroundImage = img;
            InitializeComponent();
        }

        private void DrawForm_Load(object sender, EventArgs e)
        {
            pictBox.BackgroundImage = BackgroundImage;
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

        Bitmap _raw;

        private void pictBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_mouseDown || _lastPoint == null || ((MouseEventArgs)e).Button != MouseButtons.Left)
                return;

            if(_raw == null)
                _raw = new Bitmap(pictBox.Width, pictBox.Height);

            if (pictBox.Image == null)
                pictBox.Image = new Bitmap(pictBox.Width, pictBox.Height);

            int brushSize = sliderBrushSize.Value;

            using (Graphics g = Graphics.FromImage(_raw))
            {
                g.DrawEllipse(new Pen(Color.Black, brushSize), new RectangleF(_lastPoint, new SizeF(brushSize, brushSize)));
                g.SmoothingMode = SmoothingMode.HighQuality;
            }

            pictBox.Image = new GaussianBlur(_raw).Process(sliderBlur.Value);

            pictBox.Invalidate();
            _lastPoint = e.Location;
        }

        private void pictBox_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseDown = false;
            _lastPoint = Point.Empty;
            Mask = pictBox.Image;
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _raw = null;
            pictBox.Image = null;
            Invalidate();
        }

        private void blurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
    }
}
