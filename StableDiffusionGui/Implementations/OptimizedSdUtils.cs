using StableDiffusionGui.Data;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StableDiffusionGui.Implementations
{
    internal class OptimizedSdUtils
    {
        public static async Task RunPickleScan (Model model)
        {
            var pickleScanResults = await TtiUtils.VerifyModelsWithPseudoHash(new[] { model }.ToList());

            if(pickleScanResults != null && pickleScanResults.Count > 0 && pickleScanResults.FirstOrDefault().Value == false)
                TextToImage.Cancel("Selected model appears to contain malware.");
        }
    }
}
