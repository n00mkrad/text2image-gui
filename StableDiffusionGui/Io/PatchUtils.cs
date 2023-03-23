using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.IO;

namespace StableDiffusionGui.Io
{
    internal class PatchUtils
    {
        public static void PatchAllPkgs ()
        {
            PatchDiffusers();
            PatchHuggingfaceHub();
        }

        public static void PatchDiffusers () // Compatible as of Stax124/diffusers@5d41b5383c6a0421a24b17f4520d71a1834c1dac 
        {
            string diffRootPath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdVenv, "lib", "site-packages", "diffusers");
            ((Action)(() => PatchDiffusersConvCkpt(diffRootPath))).RunInTryCatch("Patch Diffusers Exception:");
        }

        public static void PatchHuggingfaceHub() // Compatible as of 0.13.3
        {
            string hfRootPath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdVenv, "lib", "site-packages", "huggingface_hub");
            ((Action)(() => PatchHfDownloader(hfRootPath))).RunInTryCatch("Patch HF Hub Exception:");
        }

        private static void PatchDiffusersConvCkpt (string root)
        {
            string scriptPath = Path.Combine(root, "pipelines", "stable_diffusion", "convert_from_ckpt.py");

            string textOld = File.ReadAllText(scriptPath);
            string textNew = textOld.Replace("model_type == \"FrozenCLIPEmbedder\"", "model_type.endswith(\"FrozenCLIPEmbedder\")");

            if (textNew != textOld)
                File.WriteAllText(scriptPath, textNew);
        }

        private static void PatchHfDownloader(string root)
        {
            string scriptPath = Path.Combine(root, "file_download.py");

            string textOld = File.ReadAllText(scriptPath);
            string textNew = textOld.Replace("_are_symlinks_supported_in_dir[cache_dir] = True", "_are_symlinks_supported_in_dir[cache_dir] = False");

            if (textNew != textOld)
                File.WriteAllText(scriptPath, textNew);
        }
    }
}
