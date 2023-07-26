using ImageMagick;
using Microsoft.WindowsAPICodePack.Dialogs;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Ui;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Paths = StableDiffusionGui.Io.Paths;

namespace StableDiffusionGui.Forms
{
    public partial class DrawForm
    {
        public float ScaleFactor = 1f;
        public int LastPointX = 0;
        public int LastPointY = 0;
        public bool MouseIsDown = false;
        public Pen cachedPen = null;

        public void Reset()
        {
            ScaleFactor = 1f;
            LastPointX = 0;
            LastPointY = 0;
            MouseIsDown = false;
        }

        public void DrawStart(Point mouseLocation)
        {
            ScaleFactor = pictBox.Image == null ? 1f : (float)pictBox.Width / pictBox.Image.Width;
            LastPointX = mouseLocation.X;
            LastPointY = mouseLocation.Y;
            MouseIsDown = true;
        }

        public void DrawEnd()
        {
            MouseIsDown = false;
            LastPointX = 0;
            LastPointY = 0;
            Apply();
            HistorySave();
        }

        public void Draw(MouseEventArgs e)
        {
            if (!MouseIsDown || e.Button != MouseButtons.Left)
                return;

            if (pictBox.Image == null)
                pictBox.Image = new Bitmap(BackgroundImg.Width, BackgroundImg.Height);

            int brushSize = sliderBrushSize.Value;

            if (cachedPen == null || cachedPen.Width != brushSize)
                cachedPen = new Pen(Color.Black, brushSize);

            Point currentPoint = new Point((e.Location.X / ScaleFactor).RoundToInt(), (e.Location.Y / ScaleFactor).RoundToInt());

            using (Graphics g = Graphics.FromImage(RawMask))
            {
                g.SmoothingMode = SmoothingMode.None;
                g.DrawEllipse(cachedPen, new RectangleF(currentPoint, new SizeF(brushSize, brushSize)));
            }

            Apply(false); // Disable blur while drawing for performance reasons
            int x = Math.Min(LastPointX, currentPoint.X) - brushSize / 2;
            int y = Math.Min(LastPointY, currentPoint.Y) - brushSize / 2;
            int width = Math.Abs(LastPointX - currentPoint.X) + brushSize;
            int height = Math.Abs(LastPointY - currentPoint.Y) + brushSize;
            pictBox.Invalidate(new Rectangle(x, y, width, height));
            LastPointX = (e.Location.X / ScaleFactor).RoundToInt();
            LastPointY = (e.Location.Y / ScaleFactor).RoundToInt();
        }

        public void Apply(bool blur = true)
        {
            if (RawMask != null)
            {
                if (blur && !DisableBlurOption && Inpainting.CurrentBlurValue > 0)
                    pictBox.Image = new GaussianBlur(RawMask).Process(Inpainting.CurrentBlurValue);
                else
                    pictBox.Image = RawMask;
            }
        }

        public void PasteMask()
        {
            Image clipboardImg = Clipboard.GetImage();

            if (clipboardImg != null)
            {
                if (clipboardImg.Size != pictBox.Image.Size)
                {
                    UiUtils.ShowMessageBox($"The pasted mask ({clipboardImg.Width}x{clipboardImg.Height}) needs to have the same dimensions as the base image ({pictBox.Image.Width}x{pictBox.Image.Height}).");
                    return;
                }

                var magickImg = ImgUtils.GetMagickImage(clipboardImg);
                Image pastedMask = ImgUtils.ReplaceOtherColorsWithTransparency(magickImg).ToBitmap();
                RawMask = (Bitmap)pastedMask;
                sliderBlur.Value = 0;
                sliderBlur_Scroll(null, null);
                pictBox.Invalidate();
                HistorySave();
            }
        }

        #region History

        public void HistorySave()
        {
            if (History.Count >= HistoryLimitNormalized)
                History = History.Skip(1).ToList(); // Remove first (oldest) entry if we maxed out the capacity

            History.Add(new Bitmap(RawMask));
        }

        public void HistoryUndo()
        {
            if (History.Count <= 1)
                return;

            History.Remove(History.Last());
            RawMask = new Bitmap(History.Last());

            sliderBlur_Scroll(null, null);
            pictBox.Invalidate();
        }

        #endregion

        public void ClearMask()
        {
            RawMask = new Bitmap(BackgroundImg.Width, BackgroundImg.Height);
            pictBox.Image = null;
            Invalidate();
        }

        public void InvertMask()
        {
            var magickImg = ImgUtils.GetMagickImage(RawMask);
            magickImg = ImgUtils.RemoveTransparency(magickImg, ImgUtils.NoAlphaMode.Fill, MagickColors.White);
            magickImg = ImgUtils.Invert(magickImg);
            magickImg = ImgUtils.ReplaceColorWithTransparency(magickImg, MagickColors.White);
            RawMask = magickImg.ToBitmap();
            Apply();
            pictBox.Invalidate();
        }

        public void SaveMask()
        {
            string initDir = Directory.CreateDirectory(Path.Combine(Paths.GetExeDir(), Constants.Dirs.Masks)).FullName;
            string fname = Path.GetFileNameWithoutExtension(MainUi.CurrentInitImgPaths.First()).Trunc(20);
            string initFilename = $"mask_{fname}_{RawMask.Size.AsString()}_{DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss")}";

            CommonSaveFileDialog dialog = new CommonSaveFileDialog
            {
                AddToMostRecentlyUsedList = false,
                AlwaysAppendDefaultExtension = true,
                DefaultExtension = ".png",
                DefaultDirectory = initDir,
                InitialDirectory = initDir,
                DefaultFileName = initFilename,
            };

            dialog.Filters.Add(new CommonFileDialogFilter("PNG Image Files", "*.png"));
            dialog.ShowDialog();

            try
            {
                if (!string.IsNullOrWhiteSpace(dialog.FileName))
                    RawMask.Save(dialog.FileName);
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to save mask: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
            }
        }

        public void LoadMask()
        {
            string initDir = Directory.CreateDirectory(Path.Combine(Paths.GetExeDir(), Constants.Dirs.Masks)).FullName;

            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                AddToMostRecentlyUsedList = false,
                DefaultExtension = ".png",
                DefaultDirectory = initDir,
                InitialDirectory = initDir,
            };

            dialog.Filters.Add(new CommonFileDialogFilter("PNG Image Files", "*.png"));
            dialog.ShowDialog();

            try
            {
                if (!File.Exists(dialog.FileName))
                {
                    Logger.Log($"Can't load mask: Invalid file.");
                    return;
                }

                Image mask = IoUtils.GetImage(dialog.FileName);

                if (mask.Size != pictBox.Image.Size)
                {
                    Logger.Log($"Can't load mask: Mask ({mask.Size.AsString()}) does not match image dimensions ({pictBox.Image.Size.AsString()}).");
                    return;
                }

                RawMask = (Bitmap)mask;
                sliderBlur_Scroll(null, null);
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to load mask: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
            }
        }
    }
}
