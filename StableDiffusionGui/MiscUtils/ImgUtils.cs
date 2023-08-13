using ImageMagick;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace StableDiffusionGui.MiscUtils
{
    internal class ImgUtils
    {
        public static MagickImage GetMagickImage(Image img)
        {
            return GetMagickImage(img as Bitmap);
        }

        public static MagickImage GetMagickImage(Bitmap bmp)
        {
            var m = new MagickFactory();
            MagickImage image = new MagickImage(m.Image.Create(bmp));
            return image;
        }

        public static MagickImage GetMagickImage(string base64)
        {
            try
            {
                var magick = MagickImage.FromBase64(base64);
                return (MagickImage)magick;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Image ResizeImage(Image image, Size size)
        {
            return ResizeImage(image, size.Width, size.Height);
        }

        public static Image ResizeImage(Image image, int w, int h)
        {
            if (image.Width == w && image.Height == h)
                return image;

            Bitmap bmp = new Bitmap(w, h);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, w, h);
                return bmp;
            }
        }

        public static Image Negate(Image image)
        {
            MagickImage magickImage = GetMagickImage(image);
            magickImage.Negate();
            return magickImage.ToBitmap();
        }

        public static Image ToBitmap (MagickImage image)
        {
            return image.ToBitmap();
        }

        public static MagickImage AlphaMask(MagickImage image, MagickImage mask, bool invert)
        {
            if (invert)
                mask.Negate();

            image.Composite(mask, CompositeOperator.CopyAlpha);
            return image;
        }

        public static void Overlay(string path, string overlayImg, bool matchSize = true)
        {
            Image imgBase = IoUtils.GetImage(path);
            Image imgOverlay = IoUtils.GetImage(overlayImg);
            Overlay(imgBase, imgOverlay, matchSize).Save(path);
        }

        public static Image Overlay(Image imgBase, Image imgOverlay, bool matchSize = true)
        {
            if (matchSize && imgOverlay.Size != imgBase.Size)
                imgOverlay = ResizeImage(imgOverlay, imgBase.Size);

            Image img = new Bitmap(imgBase.Width, imgBase.Height);

            using (Graphics g = Graphics.FromImage(img))
            {
                g.DrawImage(imgBase, new Point(0, 0));
                g.DrawImage(imgOverlay, new Point(0, 0));
            }

            return img;
        }

        public static MagickImage ReplaceColorWithTransparency(MagickImage image, MagickColor color = null)
        {
            if (color == null)
                color = MagickColors.Black;

            image.Transparent(color);
            image.Alpha(AlphaOption.On);
            image.ColorAlpha(new MagickColor("#00000000"));
            return image;
        }

        public static MagickImage ReplaceOtherColorsWithTransparency(MagickImage image, MagickColor colorToKeep = null)
        {
            if (colorToKeep == null)
                colorToKeep = MagickColors.Black;

            image.InverseTransparent(colorToKeep);
            image.Alpha(AlphaOption.On);
            return image;
        }

        public static MagickImage Invert(MagickImage image)
        {
            image.Negate();
            return image;
        }

        public enum NoAlphaMode { Off, Fill }

        public static MagickImage RemoveTransparency(MagickImage image, NoAlphaMode mode, MagickColor fillColor = null)
        {
            if (mode == NoAlphaMode.Fill)
            {
                fillColor = fillColor ?? MagickColors.Black;
                MagickImage bg = new MagickImage(fillColor, image.Width, image.Height);
                bg.BackgroundColor = fillColor;
                bg.Composite(image, CompositeOperator.Over);
                image = bg;
            }
            if (mode == NoAlphaMode.Off)
            {
                image.Alpha(AlphaOption.Off);
            }

            return image;
        }

        public enum ScaleMode { Percentage, Height, Width, LongerSide, ShorterSide }

        public static async Task<MagickImage> Scale(string path, ScaleMode mode, float targetScale, bool write)
        {
            MagickImage img = new MagickImage(path);
            return Scale(img, mode, targetScale, write);
        }

        /// <summary> Scale and image using <paramref name="mode"/>. Optionally write to disk with <paramref name="write"/> </summary>
        /// <returns> The scaled image if <paramref name="write"/> was true, otherwise null </returns>
        public static MagickImage Scale(MagickImage img, ScaleMode mode, float targetScale, bool write)
        {
            img.FilterType = FilterType.Mitchell;

            bool heightLonger = img.Height > img.Width;
            bool widthLonger = img.Width > img.Height;
            bool square = (img.Height == img.Width);

            MagickGeometry geom = null;

            if ((square && mode != ScaleMode.Percentage) || mode == ScaleMode.Height || (mode == ScaleMode.LongerSide && heightLonger) || (mode == ScaleMode.ShorterSide && widthLonger))
            {
                geom = new MagickGeometry("x" + targetScale);
            }
            if (mode == ScaleMode.Width || (mode == ScaleMode.LongerSide && widthLonger) || (mode == ScaleMode.ShorterSide && heightLonger))
            {
                geom = new MagickGeometry(targetScale + "x");
            }
            if (mode == ScaleMode.Percentage)
            {
                geom = new MagickGeometry(Math.Round(img.Width * targetScale / 100f) + "x" + Math.Round(img.Height * targetScale));
            }

            img.Resize(geom);

            if (write)
            {
                img.Write(img.FileName);
                img.Dispose();
                return null;
            }
            else
            {
                return img;
            }
        }

        public static MagickImage Pad(string path, Size newSize, bool write, MagickColor fillColor = null)
        {
            MagickImage img = new MagickImage(path);
            return Pad(img, newSize, write, fillColor);
        }

        /// <summary> Pads an image using Magick.NET </summary>
        /// <returns> The padded image if <paramref name="write"/>, otherwise null as the image was written to disk and disposed </returns>
        public static MagickImage Pad(MagickImage img, Size newSize, bool write, MagickColor fillColor = null)
        {
            img.BackgroundColor = fillColor ?? MagickColors.Black;
            img.Extent(newSize.Width, newSize.Height, Gravity.Center);

            if (write)
            {
                img.Write(img.FileName);
                img.Dispose();
                return null;
            }
            else
            {
                return img;
            }
        }

        /// <summary>
        /// Returns a valid resolution for an input resolution. If <paramref name="validResolutionsOnly"/>, it will only use numbers from
        /// <paramref name="validWidths"/> and <paramref name="validHeights"/>, otherwise it only resizes if the size is smaller/bigger than the min/max canvas size.
        /// </summary>
        public static Size GetValidSize(Size imageSize, List<int> validWidths, List<int> validHeights, bool validResolutionsOnly = true)
        {
            if (validWidths.Contains(imageSize.Width) && validHeights.Contains(imageSize.Height))
                return imageSize;

            Size smallestFrame = new Size(validWidths.Min(), validHeights.Min());
            Size biggestFrame = new Size(validWidths.Max(), validHeights.Max());

            if (ImgMaths.IsSmallerThanFrame(imageSize.Width, imageSize.Height, smallestFrame.Width, smallestFrame.Height))
                imageSize = ImgMaths.FitIntoFrame(imageSize, smallestFrame);
            else if (ImgMaths.IsBiggerThanFrame(imageSize.Width, imageSize.Height, biggestFrame.Width, biggestFrame.Height))
                imageSize = ImgMaths.FitIntoFrame(imageSize, biggestFrame);

            if (validResolutionsOnly)
            {
                if (!validWidths.Contains(imageSize.Width))
                    imageSize.Width = validWidths.OrderBy(x => x).Where(x => x >= imageSize.Width).First();

                if (!validHeights.Contains(imageSize.Height))
                    imageSize.Height = validHeights.OrderBy(x => x).Where(x => x >= imageSize.Height).First();
            }

            return imageSize;
        }

        /// <summary>
        /// Scale and Pad a MagickImage - Scale to dimensions <paramref name="scaleDimensions"/>, then pad it out to <paramref name="canvasSize"/>
        /// </summary>
        /// <returns> Scaled and padded image </returns>
        public static MagickImage ScaleAndPad(MagickImage img, Size scaleDimensions, Size canvasSize, MagickColor color = null)
        {
            color = color ?? MagickColors.Black;
            img.Scale(new MagickGeometry(scaleDimensions.Width, scaleDimensions.Height) { IgnoreAspectRatio = true });
            img = Pad(img, canvasSize, false, color);
            return img;
        }

        public static MagickImage ResizeCanvas(MagickImage img, Size newDimensions, Gravity gravity = Gravity.Center, MagickColor fillColor = null)
        {
            fillColor = fillColor ?? MagickColors.Transparent;
            img.Extent(newDimensions.Width, newDimensions.Height, gravity, fillColor);
            return img;
        }

        private static Brush CreateCheckerboardBrush(int tileSize = 8, Color c1 = default, Color c2 = default)
        {
            if (c1 == default)
                c1 = Color.LightGray;

            if (c2 == default)
                c2 = Color.White;

            int doubleSize = tileSize * 2;
            Bitmap checkerboard = new Bitmap(doubleSize, doubleSize);
            for (int y = 0; y < doubleSize; y++)
                for (int x = 0; x < doubleSize; x++)
                    checkerboard.SetPixel(x, y, ((x / tileSize + y / tileSize) % 2 == 0) ? c1 : c2);

            return new TextureBrush(checkerboard, WrapMode.Tile);
        }

        public static Image JuxtaposeSameSize(Image img1, Image img2)
        {
            int width = img1.Width * 2;
            int height = img1.Height;

            Image img = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(img))
            {
                Brush checkerboardBrush = CreateCheckerboardBrush(8, Color.FromArgb(21, 21, 21), Color.FromArgb(29, 29, 29));
                g.FillRectangle(checkerboardBrush, new Rectangle(0, 0, width, height));
                g.DrawImage(img1, new Point(0, 0));
                g.DrawImage(img2, new Point(img1.Width, 0));
            }

            return img;
        }

        public static Image Juxtapose(Image img1, Image img2, Size size)
        {
            img1 = ResizeImage(img1, size);
            img2 = ResizeImage(img2, size);
            Image img = new Bitmap(size.Width * 2, size.Height);

            using (Graphics g = Graphics.FromImage(img))
            {
                g.DrawImage(img1, new Point(0, 0));
                g.DrawImage(img2, new Point(size.Width, 0));
            }

            return img;
        }

        public static unsafe Image EdgeAlphaFade(Image img, int distance = 16)
        {
            var width = img.Width;
            var height = img.Height;

            var result = (Bitmap)img.Clone();

            var data = result.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            byte* ptr = (byte*)data.Scan0;

            Parallel.For(0, height, y =>
            {
                byte* row = ptr + (y * data.Stride);
                for (int x = 0; x < width; x++)
                {
                    int nearestEdgeDistance = Math.Min(Math.Min(x, width - x - 1), Math.Min(y, height - y - 1));

                    if (nearestEdgeDistance < distance)
                    {
                        float normalizedDistance = (float)nearestEdgeDistance / distance;
                        float quadraticAlpha = normalizedDistance * normalizedDistance;
                        int alpha = (int)(255 * quadraticAlpha);

                        int pixelIdx = x * 4;
                        row[pixelIdx + 3] = (byte)((row[pixelIdx + 3] * alpha) / 255);
                    }
                }
            });

            result.UnlockBits(data);
            return result;
        }

        public static bool IsPartiallyTransparent(Bitmap bitmap)
        {
            try
            {
                Bitmap bmp = (Bitmap)bitmap.Clone(); // Create a copy, just to be sure we're not messing with the reference variable

                if (bmp.PixelFormat == PixelFormat.Format32bppArgb || bmp.PixelFormat == PixelFormat.Format32bppPArgb)
                {
                    BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
                    byte[] bytes = new byte[bmp.Height * data.Stride];
                    Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
                    bmp.UnlockBits(data);
                    var alphaBytes = new List<byte>();

                    for (int p = 3; p < bytes.Length; p += 4)
                    {
                        alphaBytes.Add(bytes[p]);

                        if (bytes[p] != 255)
                        {
                            Logger.LogIf($"Transparency Check: Pixel {p / 3} alpha is {bytes[p]} => Appears to be (semi)transparent", Program.Debug);
                            return true;
                        }
                    }

                    return false;
                }

                // Brute-forced method but it won't ever be used, unless you encounter types not handled above, like 16bppArgb1555 and 64bppArgb.
                for (int i = 0; i < bmp.Width; i++)
                {
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        if (bmp.GetPixel(i, j).A != 255)
                            return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, true, "Transparency Check Exception:");
            }

            return false;
        }

        public static bool IsAllBlack(Bitmap bitmap)
        {
            var validPixelFormats = new List<PixelFormat> { PixelFormat.Format24bppRgb, PixelFormat.Format32bppArgb, PixelFormat.Format32bppPArgb };

            if (!validPixelFormats.Contains(bitmap.PixelFormat)) // Invalid format, so return false as default
                return false;

            try
            {
                Bitmap bmp = (Bitmap)bitmap.Clone(); // Create a copy, just to be sure we're not messing with the reference variable
                BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
                byte[] bytes = new byte[bmp.Height * data.Stride];
                Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
                bmp.UnlockBits(data);

                int channels = 3;

                if (bmp.PixelFormat == PixelFormat.Format32bppArgb || bmp.PixelFormat == PixelFormat.Format32bppPArgb)
                    channels = 4;

                for (int p = 0; p < bytes.Length; p += channels)
                {
                    if (!(bytes[p] == 0 && bytes[p + 1] == 0 && bytes[p + 2] == 0)) // Condition is true if any pixels is not 0/0/0 and thus not fully black
                        return false;
                }

                return true;
            }
            catch(Exception ex)
            {
                Logger.LogException(ex, true, "Black Image Check Exception:");
                return false;
            }
        }
    }

    public static class ImageExtensionMethods
    {
        public static Bitmap AsBmp (this Image img)
        {
            return (Bitmap)img;
        }

        public static Bitmap ChangeFormat(this Bitmap bmp, PixelFormat targetFormat = PixelFormat.Format24bppRgb)
        {
            Bitmap newBmp = new Bitmap(bmp.Width, bmp.Height, targetFormat);

            using (Graphics g = Graphics.FromImage(newBmp))
            {
                g.DrawImage(bmp, new Rectangle(0, 0, newBmp.Width, newBmp.Height));
            }

            return newBmp;
        }
    }
}
