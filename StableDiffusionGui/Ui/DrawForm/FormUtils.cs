using ImageMagick;
using Microsoft.WindowsAPICodePack.Dialogs;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Paths = StableDiffusionGui.Io.Paths;

namespace StableDiffusionGui.Ui.DrawForm
{
    internal class FormUtils
    {
        public static Forms.DrawForm F;

        public static float ScaleFactor = 1f;
        public static int LastPointX = 0;
        public static int LastPointY = 0;
        public static bool MouseIsDown = false;

        public static void Reset ()
        {
            ScaleFactor = 1f;
            LastPointX = 0;
            LastPointY = 0;
            MouseIsDown = false;
        }

        public static void DrawStart (Point mouseLocation)
        {
            ScaleFactor = F.pictBox.Image == null ? 1f : (float)F.pictBox.Width / F.pictBox.Image.Width;
            LastPointX = mouseLocation.X;
            LastPointY = mouseLocation.Y;
            MouseIsDown = true;
        }

        public static void DrawEnd()
        {
            MouseIsDown = false;
            LastPointX = 0;
            LastPointY = 0;
            Apply();
            HistorySave();
        }

        public static void Draw (MouseEventArgs e)
        {
            if (!MouseIsDown || e.Button != MouseButtons.Left)
                return;

            if (F.pictBox.Image == null)
                F.pictBox.Image = new Bitmap(F.BackgroundImg.Width, F.BackgroundImg.Height);

            int brushSize = F.sliderBrushSize.Value;

            using (Graphics g = Graphics.FromImage(F.RawMask))
            {
                Point scaledPoint = new Point((LastPointX / ScaleFactor).RoundToInt(), (LastPointY / ScaleFactor).RoundToInt());
                g.DrawEllipse(new Pen(Color.Black, brushSize), new RectangleF(scaledPoint, new SizeF(brushSize, brushSize)));
                g.SmoothingMode = SmoothingMode.HighQuality;
            }

            Apply(false); // Disable blur while drawing for performance reasons
            F.pictBox.Invalidate();
            LastPointX = e.Location.X;
            LastPointY = e.Location.Y;
        }

        public static void Apply(bool blur = true)
        {
            if (F.RawMask != null)
            {
                if (blur && !F.DisableBlurOption && Inpainting.CurrentBlurValue > 0)
                    F.pictBox.Image = new GaussianBlur(F.RawMask).Process(Inpainting.CurrentBlurValue);
                else
                    F.pictBox.Image = F.RawMask;
            }
        }

        public static void PasteMask()
        {
            Image clipboardImg = Clipboard.GetImage();

            if (clipboardImg != null)
            {
                if (clipboardImg.Size != F.pictBox.Image.Size)
                {
                    UiUtils.ShowMessageBox($"The pasted mask ({clipboardImg.Width}x{clipboardImg.Height}) needs to have the same dimensions as the initialization image ({F.pictBox.Image.Width}x{F.pictBox.Image.Height}).");
                    return;
                }

                var magickImg = ImgUtils.GetMagickImage(clipboardImg);
                Image pastedMask = ImgUtils.ReplaceOtherColorsWithTransparency(magickImg).ToBitmap();
                F.RawMask = (Bitmap)pastedMask;
                F.sliderBlur.Value = 0;
                F.sliderBlur_Scroll(null, null);
                F.pictBox.Invalidate();
                HistorySave();
            }
        }

        #region History

        public static void HistorySave()
        {
            if (F.History.Count >= F.HistoryLimitNormalized)
                F.History = F.History.Skip(1).ToList(); // Remove first (oldest) entry if we maxed out the capacity

            F.History.Add(new Bitmap(F.RawMask));
        }

        public static void HistoryUndo()
        {
            if (F.History.Count <= 1)
                return;

            F.History.Remove(F.History.Last());
            F.RawMask = new Bitmap(F.History.Last());

            F.sliderBlur_Scroll(null, null);
            F.pictBox.Invalidate();
        }

        #endregion

        public static void ClearMask()
        {
            F.RawMask = new Bitmap(F.BackgroundImg.Width, F.BackgroundImg.Height);
            F.pictBox.Image = null;
            F.Invalidate();
        }

        public static void InvertMask()
        {
            var magickImg = ImgUtils.GetMagickImage(F.RawMask);
            magickImg = ImgUtils.RemoveTransparency(magickImg, ImgUtils.NoAlphaMode.Fill, MagickColors.White);
            magickImg = ImgUtils.Invert(magickImg);
            magickImg = ImgUtils.ReplaceColorWithTransparency(magickImg, MagickColors.White);
            F.RawMask = magickImg.ToBitmap();
            Apply();
            F.pictBox.Invalidate();
        }

        public static void SaveMask()
        {
            string initDir = Directory.CreateDirectory(Path.Combine(Paths.GetExeDir(), Constants.Dirs.Masks)).FullName;
            string fname = Path.GetFileNameWithoutExtension(MainUi.CurrentInitImgPaths.First()).Trunc(20);
            string initFilename = $"mask_{fname}_{F.RawMask.Size.AsString()}_{DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss")}";

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
                    F.RawMask.Save(dialog.FileName);
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to save mask: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
            }
        }

        public static void LoadMask()
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

                if (mask.Size != F.pictBox.Image.Size)
                {
                    Logger.Log($"Can't load mask: Mask ({mask.Size.AsString()}) does not match image dimensions ({F.pictBox.Image.Size.AsString()}).");
                    return;
                }

                F.RawMask = (Bitmap)mask;
                F.sliderBlur_Scroll(null, null);
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to load mask: {ex.Message}");
                Logger.Log(ex.StackTrace, true);
            }
        }
    }
}
