using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StableDiffusionGui.Ui
{
    internal class ImagePreview
    {
        public static string CurrentImagePath { get { return _currentImages.Length > 0 ? _currentImages[_currIndex] : ""; } }
        public static ImageMetadata CurrentImageMetadata { get { return IoUtils.GetImageMetadata(CurrentImagePath); } }

        private static string[] _currentImages = new string[0];
        private static int _currIndex = -1;

        public static void SetImages(string imagesDir, bool show, int amount = -1, string pattern = "*.png", bool recursive = false)
        {
            var imgPaths = IoUtils.GetFileInfosSorted(imagesDir, recursive, pattern).OrderBy(x => x.CreationTime).Reverse().ToList(); // Find images and sort by date, newest to oldest

            if (amount > 0)
                imgPaths = imgPaths.Take(amount).ToList();

            SetImages(imgPaths.Select(x => x.FullName).ToList(), show);
        }

        public static void SetImages(List<string> imagePaths, bool show)
        {
            _currentImages = imagePaths.ToArray();
            _currIndex = 0;

            if (show)
                Show();
        }

        public static void Show ()
        {
            if (_currIndex < 0 || _currIndex >= _currentImages.Length)
                return;

            Program.MainForm.ImgBoxOutput.Text = "";
            Program.MainForm.ImgBoxOutput.Image = IoUtils.GetImage(_currentImages[_currIndex]);
            Program.MainForm.ImgBoxOutput.ZoomToFit();

            ImageMetadata meta = CurrentImageMetadata;

            Program.MainForm.OutputImgLabel.Text = $"Showing Image {_currIndex+1}/{_currentImages.Length} - " +
                $"Seed {meta.Seed} - Scale {meta.Scale} - {meta.GeneratedResolution.Width}x{meta.GeneratedResolution.Height} - Sampler {meta.Sampler}";
        }

        public static void Move(bool previous = false)
        {
            if (!previous)
            {
                _currIndex += 1;

                if (_currIndex >= _currentImages.Length)
                    _currIndex = 0;
            }
            else
            {
                _currIndex -= 1;

                if (_currIndex < 0)
                    _currIndex = _currentImages.Length - 1;
            }
                
            Show();
        }
    }
}
