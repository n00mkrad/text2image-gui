using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using System;
using System.IO;
using System.Windows.Forms;

namespace StableDiffusionGui.Forms
{
    public partial class ImageLoadForm : Form
    {
        public enum ImageAction { None, InitImage, LoadSettings, CopyPrompt }
        public ImageAction Action = ImageAction.None;

        public ImageMetadata CurrentMetadata;

        private string _path;

        public ImageLoadForm(string path)
        {
            _path = path;
            InitializeComponent();
        }

        private void ImageLoadForm_Load(object sender, EventArgs e)
        {
            string filename = Path.GetFileName(_path);
            bool isClipboardImage = filename.Lower() == "clipboard.png" && _path.GetParentDirOfFile() == Paths.GetSessionDataPath();
            Text = isClipboardImage ? "Clipboard Image" :  filename.Trunc(120);
        }

        private void btnCopyPrompt_Click(object sender, EventArgs e)
        {
            Action = ImageAction.CopyPrompt;
            Close();
        }

        private void btnLoadSettings_Click(object sender, EventArgs e)
        {
            Action = ImageAction.LoadSettings;
            Close();
        }

        private void btnInitImage_Click(object sender, EventArgs e)
        {
            Action = ImageAction.InitImage;
            Close();
        }

        private void ImageLoadForm_Shown(object sender, EventArgs e)
        {
            Refresh();
            pictBox.Image = IoUtils.GetImage(_path);
            CurrentMetadata = IoUtils.GetImageMetadata(_path);

            string n = Environment.NewLine;

            if (!string.IsNullOrWhiteSpace(CurrentMetadata.ParsedText))
            {
                textboxInfo.Text += $"Found Metadata in Image:{n}{CurrentMetadata.ParsedText}{n}";

                if (!string.IsNullOrWhiteSpace(CurrentMetadata.Prompt))
                {
                    btnLoadSettings.Enabled = true;
                    btnCopyPrompt.Enabled = true;
                    btnLoadSettings.BackColor = btnInitImage.BackColor;
                    btnCopyPrompt.BackColor = btnInitImage.BackColor;

                    textboxInfo.Text += $"{n}Prompt:{n}{CurrentMetadata.Prompt}{n}";
                    textboxInfo.Text += $"{n}Negative Prompt:{n}{CurrentMetadata.NegativePrompt}{n}";
                    textboxInfo.Text += $"{n}Steps:{n}{CurrentMetadata.Steps}{n}";
                    textboxInfo.Text += $"{n}Scale:{n}{CurrentMetadata.Scale.ToStringDot("0.00")}{n}";
                    textboxInfo.Text += $"{n}Seed:{n}{CurrentMetadata.Seed}{n}";
                    textboxInfo.Text += $"{n}Generated Resolution:{n}{CurrentMetadata.GeneratedResolution.Width}x{CurrentMetadata.GeneratedResolution.Height}{n}";
                    textboxInfo.Text += $"{n}Sampler:{n}{CurrentMetadata.Sampler}{n}";

                    if (!string.IsNullOrWhiteSpace(CurrentMetadata.InitImgName))
                    {
                        textboxInfo.Text += $"{n}Init Image:{n}{CurrentMetadata.InitImgName}{n}";
                        textboxInfo.Text += $"{n}Init Strength:{n}{CurrentMetadata.InitStrength}{n}";
                    }
                }
            }
            else
            {
                textboxInfo.Text += $"No Metadata Found in Image.";
            }
                
        }

        private void ImageLoadForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
    }
}
