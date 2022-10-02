using ImageMagick;
using StableDiffusionGui.Io;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace StableDiffusionGui.MiscUtils
{
    internal class ImgUtils
    {
        public static MagickImage MagickImgFromImage(Image img)
        {
            return MagickImgFromImage(img as Bitmap);
        }

        public static MagickImage MagickImgFromImage (Bitmap bmp)
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

        public static Image Negate (Image image)
        {
            MagickImage magickImage = MagickImgFromImage(image);
            magickImage.Negate();
            return ImageFromMagickImg(magickImage);
        }

        public static MagickImage AlphaMask (MagickImage image, MagickImage mask, bool invert)
        {
            if(invert)
                mask.Negate();

            image.Composite(mask, CompositeOperator.CopyAlpha);
            return image;
        }

        public static void Overlay (string path, string overlayImg, bool matchSize = true)
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
    }
}
