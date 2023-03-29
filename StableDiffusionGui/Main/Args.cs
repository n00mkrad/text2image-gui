using StableDiffusionGui.Data;
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
            public static string GetArgsStartup(List<Model> cachedModels)
            {
                List<string> args = new List<string>();

                if (Config.Get<bool>(Config.Keys.FullPrecision))
                    args.Add("--precision float32");

                bool lowVram = GpuUtils.CachedGpus.Count > 0 && GpuUtils.CachedGpus.First().VramGb < 5.0f;

                if (lowVram)
                {
                    args.Add("--sequential_guidance");
                    args.Add("--free_gpu_mem");
                }

                if (!InstallationStatus.HasSdUpscalers() || (lowVram && Config.Get(Config.Keys.MedVramDisablePostProcessing, false)))
                {
                    args.Add("--no_upscale");
                    args.Add("--no_restore");
                }

                int maxCachedModels = 0;

                if (Config.Get<bool>(Config.Keys.InvokeAllowModelCaching)) // Disable caching if <6GB free, no matter the total RAM
                {
                    maxCachedModels = ((int)Math.Floor((HwInfo.GetTotalRamGb - 11f) / 4f)).Clamp(0, 16); // >16GB => 1 - >20GB => 2 - >24GB => 3 - >28GB => 4 - ...
                    Logger.Log($"InvokeAI Caching: Store up to {maxCachedModels} models in RAM", true);
                }

                args.Add($"--max_loaded_models {maxCachedModels + 1}"); // Add 1 to model count because the arg counts the VRAM loaded model as well

                if (Config.Get<bool>(Config.Keys.OfflineMode, false))
                    args.Add($"--no-internet");

                if (Models.HasAnyInpaintingModels(cachedModels, Enums.StableDiffusion.Implementation.InvokeAi))
                    args.Add($"--no-patchmatch"); // Disable patchmatch (used for legacy inpainting) if there are any native inpainting models available

                args.Add($"--embedding_path {Path.Combine(Paths.GetDataPath(), Constants.Dirs.Models.Root, Constants.Dirs.Models.Embeddings)}"); // Embeddings folder path
                args.Add("--no-nsfw_checker"); // Disable NSFW checker (might become optional in the future)

                args.Add("--no-xformers"); // Disable xformers until Pytorch >1.11 slowdown is investigated and xformers works
                args.Add("--png_compression 1"); // Higher compression levels are barely worth it

                string joinedArgs = string.Join(" ", args);
                Logger.Log($"InvokeAI Args: {joinedArgs}", true);
                return joinedArgs;
            }

            public static string GetDefaultArgsCommand()
            {
                var args = new List<string>
                {
                    "-n 1", // Always generate 1 image per command
                    "--fnformat {prefix}.png" // Only use prefix as output name since we rename it anyway
                };

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
                    case Enums.StableDiffusion.SeamlessMode.Disabled: return "";
                    case Enums.StableDiffusion.SeamlessMode.SeamlessBoth: return "--seamless";
                    case Enums.StableDiffusion.SeamlessMode.SeamlessHor: return "--seamless --seamless_axes x";
                    case Enums.StableDiffusion.SeamlessMode.SeamlessVert: return "--seamless --seamless_axes y";
                    default: return "";
                }
            }

            public static string GetSymmetryArg(Enums.StableDiffusion.SymmetryMode mode)
            {
                string t = Config.Get<float>(Config.Keys.SymmetryTimepoint, 0.9f).ToStringDot("0.0");

                switch (mode)
                {
                    case Enums.StableDiffusion.SymmetryMode.Disabled: return "";
                    case Enums.StableDiffusion.SymmetryMode.SymVert: return $"--h_symmetry_time_pct {t}"; // [sic] - h seems to be vertical
                    case Enums.StableDiffusion.SymmetryMode.SymHor: return $"--v_symmetry_time_pct {t}"; // [sic] - v seems to be horizontal
                    case Enums.StableDiffusion.SymmetryMode.SymBoth: return $"--v_symmetry_time_pct {t} --h_symmetry_time_pct {t}";
                    default: return "";
                }
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
