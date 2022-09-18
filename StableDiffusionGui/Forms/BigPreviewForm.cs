using StableDiffusionGui.Main;
using StableDiffusionGui.Os;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class BigPreviewForm : Form
    {
        private Image _img;
        private bool _maximize = true;
        private bool _enableTiling = false;
        private int _currentTiling = 1;

        public BigPreviewForm(Image img, bool maximize = true, bool enableTiling = false)
        {
            _img = img;
            _maximize = maximize;
            _enableTiling = enableTiling;
            InitializeComponent();
        }

        private void BigPreviewForm_Load(object sender, EventArgs e)
        {
            if(_maximize)
                WindowState = FormWindowState.Maximized;

            Text += $" - Right-click for More Options{(_enableTiling ? $" - Left-click To Cycle Tiling Mode" : "")}";
            picBox.Image = _img;
        }

        public void SetImage(Image img, int repeat)
        {
            Bitmap bitmap = new Bitmap(img.Width * repeat, img.Height * repeat);
            Graphics g = Graphics.FromImage(bitmap);

            for (int w = 0; w < repeat; w++)
                for (int h = 0; h < repeat; h++)
                    g.DrawImage(img, new Point(w * img.Width, h * img.Height));

            picBox.Image = bitmap;
        }

        private void BigPreviewForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void picBox_Click(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Right)
            {
                menuStripOptions.Show(Cursor.Position);
            }
            else
            {
                if (_enableTiling)
                    CycleTiling();
            }
        }

        private void CycleTiling()
        {
            if (_currentTiling == 1)
            {
                _currentTiling = 2;
                SetImage(_img, _currentTiling);
                return;
            }
            else if (_currentTiling == 2)
            {
                _currentTiling = 3;
                SetImage(_img, _currentTiling);
                return;
            }
            else if (_currentTiling == 3)
            {
                _currentTiling = 1;
                SetImage(_img, _currentTiling);
                return;
            }
        }

        private void copyImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OsUtils.SetClipboard(picBox.Image);
        }

        private void BigPreviewForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
                Close();
        }
    }
}
