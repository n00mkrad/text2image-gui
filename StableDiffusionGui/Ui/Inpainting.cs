using ImageMagick;
using StableDiffusionGui.Data;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Forms;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Paths = StableDiffusionGui.Io.Paths;

namespace StableDiffusionGui.Ui
{
    internal class Inpainting
    {
        public static string MaskImagePath { get { return Path.Combine(Paths.GetSessionDataPath(), "inpaint-mask.png"); } }
        public static string MaskedImagePath { get { return Path.Combine(Paths.GetSessionDataPath(), "inpaint-masked.png"); } }

        private static Image _currentMask;
        public static Image CurrentMask
        {
            get => _currentMask;
            set
            {
                _currentMask = value;
                Program.MainForm.UpdateInpaintUi();
            }
        }

        public static int CurrentBlurValue = -1;

        /// <returns> If inpainting was enabled </returns>
        public static bool PrepareInpaintingIfEnabled(TtiSettings settings)
        {
            List<string> initImgs = settings.Params["initImgs"].FromJson<List<string>>();

            if (initImgs == null)
            {
                return false;
            }

            if (settings.Params["inpainting"].FromJson<Enums.StableDiffusion.InpaintMode>() == Enums.StableDiffusion.InpaintMode.ImageMask)
            {
                if (initImgs.Count > 1)
                {
                    Logger.Log($"Inpainting is currently only available when using a single image as input, but you are using {initImgs.Count}.");
                    return false;
                }

                PrepareInpainting(initImgs[0], settings.Params["res"].FromJson<Size>());
                return true;
            }

            return false;
        }

        public static void PrepareInpainting(string initImgPath, Size targetSize)
        {
            Image img = IoUtils.GetImage(initImgPath);
            img = ImgUtils.ScaleAndPad(ImgUtils.GetMagickImage(img), MainUi.GetResolutionForInitImage(img.Size), targetSize).ToBitmap();

            if (CurrentMask == null)
            {
                EditCurrentMask(img);
            }

            if (CurrentMask == null)
            {
                TextToImage.Cancel("Inpainting is enabled, but no mask was used!", true);
                return;
            }

            if (CurrentMask.Size != img.Size)
                CurrentMask = ImgUtils.ResizeImage(CurrentMask, img.Size);

            CurrentMask.Save(MaskImagePath, System.Drawing.Imaging.ImageFormat.Png);
            MagickImage maskedOverlay = ImgUtils.AlphaMask(ImgUtils.GetMagickImage(img), ImgUtils.GetMagickImage(CurrentMask), true);
            maskedOverlay.Write(MaskedImagePath);
        }

        public static void EditCurrentMask (Image image)
        {
            var maskForm = new Forms.DrawForm(image, CurrentMask);
            maskForm.ShowDialogForm();
            CurrentMask = maskForm.Mask;
        }

        public static void DeleteMaskedImage()
        {
            IoUtils.TryDeleteIfExists(MaskedImagePath);
        }
    }
}
