using ImageMagick;
using StableDiffusionGui.Io;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static MagickImage AlphaMask (MagickImage image, MagickImage mask, bool invert)
        {
            if(invert)
                mask.Negate();

            image.Composite(mask, CompositeOperator.CopyAlpha);
            return image;
        }

        public static void Overlay (string path, string overlayImg)
        {
            Image imgBase = IoUtils.GetImage(path);
            Image imgOverlay = IoUtils.GetImage(overlayImg);

            Image img = new Bitmap(imgBase.Width, imgBase.Height);

            using (Graphics g = Graphics.FromImage(img))
            {
                g.DrawImage(imgBase, new Point(0, 0));
                g.DrawImage(imgOverlay, new Point(0, 0));
            }

            img.Save(path);
        }
    }
}
