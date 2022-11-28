using ImageMagick;
using StableDiffusionGui.Io;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading.Tasks;

namespace StableDiffusionGui.MiscUtils
{
    internal class ImgUtils
    {
        public static MagickImage MagickImgFromImage(Image img)
        {
            return MagickImgFromImage(img as Bitmap);
        }

        public static MagickImage MagickImgFromImage(Bitmap bmp)
        {
            var m = new MagickFactory();
            MagickImage image = new MagickImage(m.Image.Create(bmp));
            return image;
        }

        public static Image ImageFromMagickImg(MagickImage img)
        {
            using (var stream = new MemoryStream())
            {
                img.Write(stream);
                return new Bitmap(stream);
            }
        }

        public static Image ResizeImage(Image image, Size size)
        {
            return ResizeImage(image, size.Width, size.Height);
        }

        public static Image ResizeImage(Image image, int w, int h)
        {
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
            MagickImage magickImage = MagickImgFromImage(image);
            magickImage.Negate();
            return ImageFromMagickImg(magickImage);
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
                if (fillColor == null)
                    fillColor = MagickColors.Black;

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
            return await Scale(img, mode, targetScale, write);
        }

        public static async Task<MagickImage> Scale(MagickImage img, ScaleMode mode, float targetScale, bool write)
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

        public static async Task<MagickImage> Pad(string path, Size newSize, bool write, MagickColor fillColor = null)
        {
            MagickImage img = new MagickImage(path);
            return await Pad(img, newSize, write, fillColor);
        }

        public static async Task<MagickImage> Pad(MagickImage img, Size newSize, bool write, MagickColor fillColor = null)
        {
            img.BackgroundColor = fillColor ?? MagickColors.Black;
            img.Extent(newSize.Width, newSize.Height, Gravity.Center);
            img.Write(img.FileName);

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
    }
}
