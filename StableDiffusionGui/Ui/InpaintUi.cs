using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace StableDiffusionGui.Ui
{
    internal class InpaintUi
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
    }
}
