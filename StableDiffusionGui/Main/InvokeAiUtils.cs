using StableDiffusionGui.Io;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StableDiffusionGui.Main
{
    internal class InvokeAiUtils
    {
        public static void WriteModelsYaml(string mdlName, string vaeName = "", string keyName = "default")
        {
            var mdl = Paths.GetModel(mdlName, false, Enums.StableDiffusion.ModelType.Normal);
            var vae = Paths.GetModel(vaeName, false, Enums.StableDiffusion.ModelType.Vae);
            WriteModelsYaml(mdl, vae, keyName);
        }

        public static void WriteModelsYaml(FileInfo mdl, FileInfo vae, string keyName = "default")
        {
            string text = $"{keyName}:\n" +
                $"    config: configs/stable-diffusion/v1-inference.yaml\n" +
                $"    weights: {(mdl == null ? $"unknown{Constants.FileExts.SdModel}" : mdl.FullName.Wrap(true))}\n" +
                $"    {(vae != null && File.Exists(vae.FullName) ? $"vae: {vae.FullName.Wrap(true)}" : "")}\n" +
                $"    description: Current NMKD SD GUI model\n" +
                $"    width: 512\n" +
                $"    height: 512\n" +
                $"    default: true";

            File.WriteAllText(Path.Combine(Paths.GetDataPath(), Constants.Dirs.RepoSd, "configs", "models.yaml"), text);
        }

        public static void WriteModelsYamlAll(FileInfo selectedMdl, FileInfo selectedVae, List<FileInfo> cachedModels = null, List<FileInfo> cachedModelsVae = null)
        {
            if (cachedModels == null || cachedModels.Count < 1)
                cachedModels = Paths.GetModels(Enums.StableDiffusion.ModelType.Normal);

            if (cachedModelsVae == null || cachedModelsVae.Count < 1)
                cachedModelsVae = Paths.GetModels(Enums.StableDiffusion.ModelType.Vae);

            string text = "";

            cachedModelsVae.Insert(0, null); // Insert null entry, for looping

            foreach (FileInfo mdl in cachedModels)
            {
                bool inpaint = mdl.Name.MatchesWildcard("*-inpainting.*");

                foreach (FileInfo vae in cachedModelsVae)
                {
                    text += $"{GetMdlNameForYaml(mdl, vae)}:\n" +
                    $"    config: configs/stable-diffusion/{(inpaint ? "v1-inpainting-inference.yaml" : "v1-inference")}.yaml\n" +
                    $"    weights: {mdl.FullName.Wrap(true)}\n" +
                    $"{(vae != null && File.Exists(vae.FullName) ? $"    vae: {vae.FullName.Wrap(true)}\n" : "")}" +
                    $"    description: {mdl.Name}\n" +
                    $"    width: 512\n" +
                    $"    height: 512\n" +
                    $"    default: {IsModelDefault(mdl, vae, selectedMdl, selectedVae).ToString().Lower()}\n\n";
                }
            }

            File.WriteAllText(Path.Combine(Paths.GetDataPath(), Constants.Dirs.RepoSd, "configs", "models.yaml"), text);
        }

        private static bool IsModelDefault(FileInfo mdl, FileInfo vae, FileInfo selectedMdl, FileInfo selectedVae)
        {
            if (mdl == null || selectedMdl == null)
                return false;

            bool mdlMatch = mdl.FullName == selectedMdl.FullName;
            bool vaeMatch;

            if (selectedVae == null)
                vaeMatch = vae == null;
            else
                vaeMatch = vae != null && selectedVae.FullName == vae.FullName;

            return mdlMatch && vaeMatch;
        }

        public static string GetMdlNameForYaml(FileInfo mdl, FileInfo vae)
        {
            return $"{mdl.Name}{(vae == null ? "-noVae" : $"-{vae.Name}")}";
        }
    }
}
