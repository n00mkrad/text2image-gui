using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static List<float> GetScales (string customScalesText)
        {
            List<float> scales = new List<float> { CurrentScale };

            if (customScalesText.MatchesWildcard("* > * *"))
            {
                var splitMinMax = customScalesText.Trim().Split('>');
                float min = splitMinMax[0].GetFloat();
                float max = splitMinMax[1].Trim().Split(' ').First().GetFloat();
                float step = splitMinMax.Last().Split(' ').Last().GetFloat();

                List<float> incrementScales = new List<float>();

                for (float f = min; f < (max + 0.01f); f += step)
                    incrementScales.Add(f);

                if (incrementScales.Count > 0)
                    scales = incrementScales; // Replace list, don't use the regular scale slider at all in this mode
            }
            else
            {
                scales.AddRange(customScalesText.Replace(" ", "").Split(",").Select(x => x.GetFloat()).Where(x => x > 0.05f));
            }

            return scales;
        }
    }
}
