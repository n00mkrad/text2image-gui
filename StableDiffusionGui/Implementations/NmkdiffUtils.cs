using StableDiffusionGui.Data;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Implementations
{
    internal class NmkdiffUtils
    {
        public static string GetGenerationMode (TtiSettings s, Model model)
        {
            string mode = "txt2img";
            bool inpaintingMdl = model.Name.EndsWith(Constants.SuffixesPrefixes.InpaintingMdlSuf);

            if (s.InitImgs != null && s.InitImgs.Length > 0)
            {
                mode = "img2img";

                if (inpaintingMdl && s.ImgMode != ImgMode.InitializationImage)
                    mode = "inpaint";
            }

            return mode;
        }
    }
}
