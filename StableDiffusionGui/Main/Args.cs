using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using StableDiffusionGui.Os;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StableDiffusionGui.Main
{
    public class Args
    {
        public class InvokeAi
        {
            public static string GetArgsStartup(string embedding = "", string model = "")
            {
                bool lowVram = GpuUtils.CachedGpus.Count > 0 && GpuUtils.CachedGpus.First().VramGb < 7.9f;

                List<string> args = new List<string>();

                if (!string.IsNullOrWhiteSpace(model))
                    args.Add($"--model {model}");

                if (Config.Get<bool>(Config.Keys.FullPrecision))
                    args.Add("--precision float32");

                args.Add(GetEmbeddingArg(embedding));

                if (lowVram)
                {
                    if (Config.Get<bool>(Config.Keys.MedVramDisablePostProcessing, false))
                    {
                        args.Add("--no_upscale");
                        args.Add("--no_restore");
                    }
                }

                if (!args.Contains("--no_restore"))
                {
                    args.Add("--gfpgan_model_path ../gfpgan/gfpgan.pth");
                }

                int maxCachedModels = 0;

                if (Config.Get<bool>(Config.Keys.InvokeAllowModelCaching)) // Disable caching if <6GB free, no matter the total RAM
                {
                    maxCachedModels = (int)Math.Floor((HwInfo.GetTotalRamGb - 11f) / 4f); // >16GB => 1 - >20GB => 2 - >24GB => 3 - >24GB => 4 - ...
                    Logger.Log($"InvokeAI model caching: Cache up to {maxCachedModels} models in RAM", true);
                }

                args.Add($"--max_loaded_models {maxCachedModels + 1} --no-nsfw_checker --no-patchmatch --no-xformers"); // Add 1 to model count because the arg counts the VRAM loaded model as well
                return string.Join(" ", args);
            }

            public static string GetDefaultArgsCommand()
            {
                var args = new List<string>();

                args.Add("-n 1");

                if (Config.Get<bool>(Config.Keys.SaveUnprocessedImages))
                    args.Add("-save_orig");

                if (Config.Get<bool>(Config.Keys.EnableTokenizationLogging))
                    args.Add("-t");

                return string.Join(" ", args);
            }

            public static string GetSeamlessArg(Enums.StableDiffusion.SeamlessMode mode)
            {
                switch (mode)
                {
                    case Enums.StableDiffusion.SeamlessMode.Disabled:     return "";
                    case Enums.StableDiffusion.SeamlessMode.SeamlessBoth: return "--seamless";
                    case Enums.StableDiffusion.SeamlessMode.SeamlessHor:  return "--seamless --seamless_axes x";
                    case Enums.StableDiffusion.SeamlessMode.SeamlessVert: return "--seamless --seamless_axes y";
                    default: return "";
                }
            }

            public static string GetEmbeddingArg(string embeddingPath)
            {
                return File.Exists(embeddingPath) ? $"--embedding_path {embeddingPath.Wrap()}" : "";
            }

            public static string GetFaceRestoreArgs(bool force = false)
            {
                if (!force && !Config.Get<bool>(Config.Keys.FaceRestoreEnable))
                    return "";

                if (!InstallationStatus.HasSdUpscalers())
                    return "";

                var faceRestoreOpt = (Enums.Utils.FaceTool)Config.Get<int>(Config.Keys.FaceRestoreIdx);
                string tool = "";
                string strength = Config.Get<float>(Config.Keys.FaceRestoreStrength).ToStringDot("0.###");

                if (faceRestoreOpt == Enums.Utils.FaceTool.CodeFormer)
                    tool = $"codeformer -cf {Config.Get<float>(Config.Keys.CodeformerFidelity).ToStringDot()}";

                if (faceRestoreOpt == Enums.Utils.FaceTool.Gfpgan)
                    tool = "gfpgan";

                return $"-G {strength} -ft {tool}";
            }

            public static string GetUpscaleArgs(bool force = false)
            {
                if (!force && !Config.Get<bool>(Config.Keys.UpscaleEnable))
                    return "";

                var upscaleSetting = (Forms.PostProcSettingsForm.UpscaleOption)Config.Get<int>(Config.Keys.UpscaleIdx);
                int factor = 2;

                if (upscaleSetting == Forms.PostProcSettingsForm.UpscaleOption.X2) factor = 2;
                if (upscaleSetting == Forms.PostProcSettingsForm.UpscaleOption.X3) factor = 3;
                if (upscaleSetting == Forms.PostProcSettingsForm.UpscaleOption.X4) factor = 4;

                return $"-U {factor} {Config.Get<float>(Config.Keys.UpscaleStrength).ToStringDot("0.###")}";
            }
        }

        public class OptimizedSd
        {
            public static string GetDefaultArgsStartup()
            {
                List<string> args = new List<string>();

                args.Add($"--precision {(Config.Get<bool>(Config.Keys.FullPrecision) ? "full" : "autocast")}"); // Precision

                return string.Join(" ", args);
            }
        }
    }
}
