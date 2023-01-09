using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace StableDiffusionGui.MiscUtils
{
    internal class ImgMaths
    {
        /// <summary>
        /// Calculates the new size of an image (<paramref name="inputSize"/>) in order to fill the canvas (<paramref name="frameSize"/>). Allows resizing.
        /// <returns> Best size of <paramref name="inputSize"/> that fits into <paramref name="frameSize"/> </returns>
        public static Size FitIntoFrame (Size inputSize, Size frameSize)
        {
            float currentWidth = inputSize.Width;
            float currentHeight = inputSize.Height;

            // Increase size if smaller than frame
            while (IsSmallerThanFrame(inputSize.Width, inputSize.Height, frameSize.Width, frameSize.Height))
            {
                int longerSideLength = GetLongerSideLength(inputSize);
                float increaseFactor = (float)(longerSideLength + 1) / longerSideLength;
                currentWidth *= increaseFactor;
                currentHeight *= increaseFactor;
                inputSize = new Size(currentWidth.RoundToInt().Clamp(0, frameSize.Width), currentHeight.RoundToInt().Clamp(0, frameSize.Height));
            }

            // Decrease size if bigger than frame
            while (IsBiggerThanFrame(inputSize.Width, inputSize.Height, frameSize.Width, frameSize.Height))
            {
                int longerSideLength = GetLongerSideLength(inputSize);
                float decreaseFactor = (float)(longerSideLength - 1) / longerSideLength;
                currentWidth *= decreaseFactor;
                currentHeight *= decreaseFactor;
                inputSize = new Size(currentWidth.RoundToInt().Clamp(0, int.MaxValue), currentHeight.RoundToInt().Clamp(0, int.MaxValue));
            }

            return inputSize;
        }

        public static bool IsSmallerThanFrame(int w, int h, int wFrame, int hFrame)
        {
            return (w < wFrame) && (h < hFrame);
        }

        public static bool IsBiggerThanFrame(int w, int h, int wFrame, int hFrame)
        {
            return (w > wFrame) || (h > hFrame);
        }

        private static int GetShorterSideLength(Size s)
        {
            return new[] { s.Width, s.Height }.Min();
        }

        private static int GetLongerSideLength(Size s)
        {
            return new[] { s.Width, s.Height }.Max();
        }
    }
}
