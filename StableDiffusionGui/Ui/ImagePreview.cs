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

        /// <returns> The amount of images shown </returns>
        public static int SetImages(string imagesDir, bool show, int amount = -1, string pattern = "*.png", bool recursive = false)
        {
            var imgPaths = IoUtils.GetFileInfosSorted(imagesDir, recursive, pattern).OrderBy(x => x.CreationTime).Reverse().ToList(); // Find images and sort by date, newest to oldest

            if (amount > 0)
                imgPaths = imgPaths.Take(amount).ToList();

            SetImages(imgPaths.Select(x => x.FullName).ToList(), show);

            return imgPaths.Count;
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
            {
                Clear();
                return;
            }

            Program.MainForm.ImgBoxOutput.Text = "";
            Program.MainForm.ImgBoxOutput.Image = IoUtils.GetImage(_currentImages[_currIndex]);
            Program.MainForm.ImgBoxOutput.ZoomToFit();

            ImageMetadata meta = CurrentImageMetadata;

            List<string> infos = new List<string>();

            if(meta.Seed >= 0)
                infos.Add($"Seed {meta.Seed}");

            if (meta.Scale >= 0)
                infos.Add($"Scale {meta.Scale.ToStringDot()}");

            if (!meta.GeneratedResolution.IsEmpty)
                infos.Add($"{meta.GeneratedResolution.Width}x{meta.GeneratedResolution.Height}");

            if (meta.InitStrength > 0.0001f)
                infos.Add($"Strength {meta.InitStrength.ToStringDot()}");

            if (meta.Sampler != "?")
                infos.Add($"{meta.Sampler}");

            Program.MainForm.OutputImgLabel.Text = $"Showing Image {_currIndex+1}/{_currentImages.Length} {(infos.Count > 0 ? $" - {string.Join(" - ", infos)}" : "")}";
        }

        public static void Clear ()
        {
            Program.MainForm.ImgBoxOutput.Text = "";
            Program.MainForm.ImgBoxOutput.Image = null;
            Program.MainForm.OutputImgLabel.Text = "No images to show.";
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
