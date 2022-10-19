using ImageMagick;
using StableDiffusionGui.Data;
using StableDiffusionGui.Forms;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Documents;
using Paths = StableDiffusionGui.Io.Paths;

namespace StableDiffusionGui.Ui
{
    internal class InpaintingUtils
    {
        public static string MaskedImagePath { get { return Path.Combine(Paths.GetSessionDataPath(), "masked.png"); } }

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

            if(initImgs == null)
            {
                return false;
            }

            if(initImgs.Count > 1)
            {
                Logger.Log($"Inpainting is currently only available when using a single image as input, but you are currently using {initImgs.Count}.");
                return false;
            }

            if (settings.Params["inpainting"].FromJson<string>() == "masked")
            {
                PrepareInpainting(initImgs[0], settings.Params["res"].FromJson<Size>());
                return true;
            }

            return false;
        }

        public static void PrepareInpainting(string initImgPath, Size targetSize)
        {
            Image img = ImgUtils.ResizeImage(IoUtils.GetImage(initImgPath), targetSize.Width, targetSize.Height);

            if (CurrentMask == null)
            {
                var maskForm = new DrawForm(img);
                maskForm.ShowDialog();
                CurrentMask = maskForm.Mask;
            }

            if (CurrentMask == null)
            {
                TextToImage.Cancel("Inpainting is enabled, but no mask was used!");
                return;
            }

            if (CurrentMask.Size != img.Size)
                CurrentMask = ImgUtils.ResizeImage(CurrentMask, img.Size);

            MagickImage maskedOverlay = ImgUtils.AlphaMask(ImgUtils.MagickImgFromImage(img), ImgUtils.MagickImgFromImage(CurrentMask), true);
            maskedOverlay.Write(MaskedImagePath);
        }

        public static void DeleteMaskedImage ()
        {
            IoUtils.TryDeleteIfExists(MaskedImagePath);
        }
    }
}
