using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class ImageHoverForm : Form
    {
        private Image _image;

        public ImageHoverForm(Image image)
        {
            InitializeComponent();
            DoubleBuffered = true;
            _image = image;

            if (ImgMaths.IsBiggerThanFrame(image.Width, image.Height, Program.MainForm.Width, Program.MainForm.Height))
                Size = ImgMaths.FitIntoFrame(image.Size, Program.MainForm.Size);
            else
                Size = image.Size;
        }

        #region Show form topmost without stealing focus

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        private const int WS_EX_TOPMOST = 0x00000008;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= WS_EX_TOPMOST;
                return createParams;
            }
        }

        #endregion

        private void ImageHoverForm_Load(object sender, EventArgs e)
        {
            BackgroundImage = _image;
        }

        private async void ImageHoverForm_Shown(object sender, EventArgs e)
        {
            Refresh();
            await Task.Delay(10);

            if (Disposing || IsDisposed || !InputUtils.IsHoldingShift)
                return;

            Opacity = 1;

            long delayTicks = (1000d / 60d * 10000d).RoundToLong(); // Ticks for 60 FPS
            TimeSpan delay = new TimeSpan(delayTicks);

            Program.MainForm.toolTip.Active = false;

            while (true)
            {
                Location = new Point(Cursor.Position.X + 15, Cursor.Position.Y - (Height / 3f).RoundToInt());
                await Task.Delay(delay);
            }
        }
    }
}
