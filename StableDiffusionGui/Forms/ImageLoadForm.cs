using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using static StableDiffusionGui.Main.Enums.Misc;

namespace StableDiffusionGui.Forms
{
    public partial class ImageLoadForm : CustomForm
    {
        public ImageImportAction Action = (ImageImportAction)(-1);
        public ImageMetadata CurrentMetadata;

        private string _path;
        private bool _ready;

        public ImageLoadForm(string path)
        {
            AllowTextboxTab = false;
            _path = path;
            InitializeComponent();
        }

        private void ImageLoadForm_Load(object sender, EventArgs e)
        {
            string filename = Path.GetFileName(_path);
            bool isClipboardImage = filename.StartsWith("clipboard") && _path.GetParentDirOfFile() == Paths.GetSessionDataPath();
            Text = isClipboardImage ? "Clipboard Image" :  filename.Trunc(120);
        }

        private async void ImageLoadForm_Shown(object sender, EventArgs e)
        {
            await Task.Delay(1);
            Refresh();

            try
            {
                pictBox.Image = IoUtils.GetImage(_path);
                CurrentMetadata = IoUtils.GetImageMetadata(_path);

                var disabledActions = new List<ImageImportAction>();

                if (CurrentMetadata == null || string.IsNullOrWhiteSpace(CurrentMetadata.ParsedText))
                {
                    disabledActions.Add(ImageImportAction.LoadSettings);
                    disabledActions.Add(ImageImportAction.LoadImageAndSettings);
                    disabledActions.Add(ImageImportAction.CopyPrompt);
                }

                comboxImportAction.FillFromEnum<ImageImportAction>(Strings.ImageImportMode, 0, disabledActions);

                string n = Environment.NewLine;
                textboxInfo.Text += $"Resolution: {pictBox.Image.Size.AsString()}{n}{n}";

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
                        textboxInfo.Text += $"{n}Sampler:{n}{Strings.Samplers.Get(CurrentMetadata.Sampler, true, true)}{n}";

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

                panelOk.Visible = true;
                TabOrderInit(new List<Control>() { textboxInfo, comboxImportAction }, 1);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString(), true);
            }

            _ready = true;
        }

        private void btnCopyPrompt_Click(object sender, EventArgs e)
        {
            Action = ImageImportAction.CopyPrompt;
            Close();
        }

        private void btnLoadSettings_Click(object sender, EventArgs e)
        {
            Action = ImageImportAction.LoadSettings;
            Close();
        }

        private void btnInitImage_Click(object sender, EventArgs e)
        {
            Action = ImageImportAction.LoadImage;
            Close();
        }

        private void ImageLoadForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && _ready)
                Close();

            if (e.KeyCode == Keys.Return && _ready)
                btnOk_Click(null, null);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Action = ParseUtils.GetEnum<ImageImportAction>(comboxImportAction.Text, true, Strings.ImageImportMode);
            Close();
        }
    }
}
