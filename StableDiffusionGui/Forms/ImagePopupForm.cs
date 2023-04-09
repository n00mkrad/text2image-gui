using StableDiffusionGui.Extensions;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class ImagePopupForm : Form
    {
        public enum SizeMode { Percent100, Percent200, Maximized }

        private Image _currentImage;
        public Image CurrentImage { get => _currentImage; set { _currentImage = value; picBox.Image = _currentImage; } }
        public bool SlideshowMode;

        private SizeMode _sizeMode;
        private int _currentTiling = 1;
        private int _currentZoom = 100;

        private int _zoomMax = 100;
        private readonly int _zoomStep = 25;

        private DateTime _lastInfoLabelShowTime;
        private readonly int _infoLabelShowTimeMs = 1500;

        public ImagePopupForm(Image img, SizeMode initSizeMode = SizeMode.Percent100)
        {
            _currentImage = img;
            _sizeMode = initSizeMode;
            Anchor = AnchorStyles.None;
            SetMaxZoom();
            InitializeComponent();
        }

        private void ImagePopupForm_Load(object sender, EventArgs e)
        {
            picBox.MouseWheel += pictBoxImgViewer_MouseWheel;
            SetSize(_sizeMode);
            picBox.Image = CurrentImage;
            Task.Run(() => InfoLabelVisLoop());
            SetInfoLabel("Press ESC to close, right-click for more options.");
        }

        private void ImagePopupForm_Shown(object sender, EventArgs e)
        {
            FixLabelPosition();
            SlideshowMode = Config.Instance.PopupSlideshowEnabledByDefault;
            enableToolStripMenuItem.Checked = SlideshowMode;
            enabledByDefaultToolStripMenuItem.Checked = SlideshowMode;
        }

        private void SetMaxZoom()
        {
            Screen smallestScreen = Screen.AllScreens.OrderBy(x => x.Bounds.Height).First();

            while (true)
            {
                float zoomFactor = _zoomMax / 100f;
                Size zoomed = new Size((CurrentImage.Width * zoomFactor).RoundToInt(), (CurrentImage.Height * zoomFactor).RoundToInt());

                if (zoomed.Width >= smallestScreen.Bounds.Width || zoomed.Height >= smallestScreen.Bounds.Height)
                    break;

                _zoomMax += _zoomStep;
            }
        }

        public void SetSize(SizeMode sizeMode)
        {
            WindowState = sizeMode == SizeMode.Maximized ? FormWindowState.Maximized : FormWindowState.Normal;

            if (sizeMode == SizeMode.Percent100)
                SetZoom(100);
            else if (sizeMode == SizeMode.Percent200)
                SetZoom(200);
        }

        private void SetZoom(int newZoomPercent = 0) // Default = 0 => Do not change zoom, just apply the current value
        {
            Size oldSize = Size;

            if (newZoomPercent > 0)
                _currentZoom = newZoomPercent;

            _currentZoom = _currentZoom.Clamp(_zoomStep, _zoomMax);
            float zoomFactor = _currentZoom / 100f;
            Size = new Size((CurrentImage.Width * zoomFactor).RoundToInt(), (CurrentImage.Height * zoomFactor).RoundToInt());

            // Keep centered after zooming:
            Size sizeDifference = Size - oldSize;
            Location = new Point(Location.X - (sizeDifference.Width / 2f).RoundToInt(), Location.Y - (sizeDifference.Height / 2f).RoundToInt());
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

        private void ImagePopupForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void picBox_Click(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Right)
                menuStripOptions.Show(Cursor.Position);
        }

        public void CycleTiling()
        {
            _currentTiling = _currentTiling == 3 ? 1 : _currentTiling += 1;
            SetImage(CurrentImage, _currentTiling);
            SetInfoLabel($"Tiling set to {_currentTiling}x{_currentTiling}");
        }

        private void copyImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OsUtils.SetClipboard(picBox.GetImageSafe());
        }

        private void ImagePopupForm_KeyDown(object sender, KeyEventArgs e)
        {
            Hotkeys.HandleImageViewer(e.KeyData, this, SlideshowMode);
        }

        private void closeESCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region Move Window

        private bool _mouseDown;
        private Point _lastLocation;

        private void picBox_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseDown = true;
            _lastLocation = e.Location;
        }

        private void picBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDown)
            {
                Location = new Point((Location.X - _lastLocation.X) + e.X, (Location.Y - _lastLocation.Y) + e.Y);
                Update();
            }
        }

        private void picBox_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseDown = false;
        }

        #endregion

        private void picBox_DoubleClick(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                SetSize(SizeMode.Percent100);
            else
                WindowState = FormWindowState.Maximized;

            CenterToScreen();
        }

        private void pixelPerfectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSize(SizeMode.Percent100);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SetSize(SizeMode.Percent200);
        }

        private void fullscreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSize(SizeMode.Maximized);
        }

        private void x1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _currentTiling = 1;
            SetImage(CurrentImage, _currentTiling);
        }

        private void x2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _currentTiling = 2;
            SetImage(CurrentImage, _currentTiling);
        }

        private void x3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _currentTiling = 3;
            SetImage(CurrentImage, _currentTiling);
        }

        private void pictBoxImgViewer_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                SetZoom(_currentZoom += _zoomStep);
            else
                SetZoom(_currentZoom -= _zoomStep);
        }

        private void alwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TopMost = alwaysOnTopToolStripMenuItem.Checked;
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UiUtils.ShowMessageBox("ESC: Close\n\nT: Cycle through tiling modes\n\nClick and drag: Move window around\n\nDouble-Click: Toggle Fullscreen\n\nMouse Wheel: Zoom In/Out\n\n" +
                "Slideshow Mode: When enabled, the pop-up viewer behaves the same way as the regular image viewer. Use arrow keys for previous/next image.\n\n", "Pop-Up Viewer Help");
        }

        #region Info Label

        private void ImagePopupForm_SizeChanged(object sender, EventArgs e)
        {
            FixLabelPosition();
            SetZoomText();
        }

        private void ImagePopupForm_LocationChanged(object sender, EventArgs e)
        {
            FixLabelPosition();
        }

        private void FixLabelPosition()
        {
            infoLabel.Left = (Width - infoLabel.Width) / 2;
            infoLabel.Top = (Height - infoLabel.Height) / 2;
        }

        private void SetZoomText()
        {
            if (_currentImage == null)
                return;

            bool letterboxed = Width == Screen.FromControl(this).Bounds.Width; // True => Letterboxed - False => Pillarboxed
            int zoomPercent = (letterboxed ? (Height / (float)_currentImage.Height * 100f) : (Width / (float)_currentImage.Width * 100f)).RoundToInt();
            SetInfoLabel(WindowState == FormWindowState.Maximized ? $"Zoom: Fullscreen ({zoomPercent}%)" : $"Zoom: {zoomPercent}%");
        }

        private void SetInfoLabel(string text)
        {
            infoLabel.Text = text;
            _lastInfoLabelShowTime = DateTime.Now;
        }

        private async Task InfoLabelVisLoop()
        {
            while (true)
            {
                infoLabel.SetVisible((DateTime.Now - _lastInfoLabelShowTime).TotalMilliseconds <= _infoLabelShowTimeMs);
                await Task.Delay(1);
            }
        }

        #endregion

        private void enableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SlideshowMode = enableToolStripMenuItem.Checked;
        }

        private void enabledByDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Config.Instance.PopupSlideshowEnabledByDefault = enabledByDefaultToolStripMenuItem.Checked;
        }
    }
}
