using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StableDiffusionGui.Main
{
    internal class MainUi
    {
        public static int CurrentSteps;
        public static float CurrentScale;

        public static int CurrentResW;
        public static int CurrentResH;

        public static void HandleDroppedFiles(string[] paths)
        {
            foreach(string path in paths)
            {
                if (path.EndsWith(".png"))
                {
                    ImageMetadata meta = IoUtils.GetImageMetadata(path);

                    if(!string.IsNullOrWhiteSpace(meta.Prompt))
                        Logger.Log(meta.ParsedText);
                }
            }
        }
    }
}
