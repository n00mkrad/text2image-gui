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
            public static string GetPrecisionArg()
            {
                return $"{(Config.GetBool("checkboxFullPrecision") ? "--precision float32" : "")}";
            }

            public static string GetArgsStartup(string embedding = "", string model = "")
            {
                bool lowVram = GpuUtils.CachedGpus.Count > 0 && GpuUtils.CachedGpus.First().VramGb < 7.9f;

                List<string> args = new List<string>();

                if (!string.IsNullOrWhiteSpace(model))
                    args.Add($"--model {model}");

                args.Add(GetPrecisionArg());
                args.Add(GetEmbeddingArg(embedding));

                if (lowVram)
                {
                    if (Config.GetBool("medVramFreeGpuMem", true))
                        args.Add("--free_gpu_mem");

                    if (Config.GetBool("medVramDisablePostProcessing", false))
                    {
                        args.Add("--no_upscale");
                        args.Add("--no_restore");
                    }
                }

                if (!args.Contains("--no_restore"))
                {
                    args.Add("--gfpgan_dir ../gfpgan");
                    args.Add("--gfpgan_model_path gfpgan.pth");
                }

                int maxCachedModels = 0;

                if (HwInfo.GetFreeRamGb > 6f && !Config.GetBool("disableModelCaching")) // Disable caching if there's <6GB free, no matter the total RAM
                {
                    maxCachedModels = (int)Math.Floor((HwInfo.GetTotalRamGb - 12f) / 4f); // >16GB => 1 - >20GB => 2 - >24GB => 3 - >24GB => 4 - ...
                    Logger.Log($"InvokeAI model caching: Cache up to {maxCachedModels} models in RAM", true);
                }

                args.Add($"--max_loaded_models {maxCachedModels + 1}"); // Add 1 because the arg counts the VRAM loaded model as well

                return string.Join(" ", args);
            }

            public static string GetDefaultArgsCommand()
            {
                List<string> args = new List<string>();

                if (Config.GetBool("checkboxSaveUnprocessedImages"))
                    args.Add("-save_orig");

                args.Add("-n 1");
                args.Add("-t");

                return string.Join(" ", args);
            }

            public static string GetSeamlessArg(Enums.StableDiffusion.SeamlessMode mode)
            {
                if (mode == Enums.StableDiffusion.SeamlessMode.Disabled)
                    return "";

                if (mode == Enums.StableDiffusion.SeamlessMode.SeamlessBoth)
                    return $"--seamless";

                if (mode == Enums.StableDiffusion.SeamlessMode.SeamlessHor)
                    return $"--seamless --seamless_axes x";

                if (mode == Enums.StableDiffusion.SeamlessMode.SeamlessVert)
                    return $"--seamless --seamless_axes y";

                return "";
            }

            public static string GetEmbeddingArg(string embeddingPath)
            {
                return File.Exists(embeddingPath) ? $"--embedding_path {embeddingPath.Wrap()}" : "";
            }

            public static string GetFaceRestoreArgs(bool force = false)
            {
                if (!force && !Config.GetBool("checkboxFaceRestorationEnable"))
                    return "";

                if (!InstallationStatus.HasSdUpscalers())
                    return "";

                var faceRestoreOpt = (Forms.PostProcSettingsForm.FaceRestoreOption)Config.GetInt("comboxFaceRestoration");
                string tool = "";
                string strength = Config.GetFloat("sliderFaceRestoreStrength").ToStringDot("0.000");

                if (faceRestoreOpt == Forms.PostProcSettingsForm.FaceRestoreOption.CodeFormer)
                    tool = $"codeformer -cf {Config.GetFloat(Config.Key.sliderCodeformerFidelity).ToStringDot()}";

                if (faceRestoreOpt == Forms.PostProcSettingsForm.FaceRestoreOption.Gfpgan)
                    tool = "gfpgan";

                return $"-G {strength} -ft {tool}";
            }

            public static string GetUpscaleArgs(bool force = false)
            {
                if (!force && !Config.GetBool("checkboxUpscaleEnable"))
                    return "";

                var upscaleSetting = (Forms.PostProcSettingsForm.UpscaleOption)Config.GetInt("comboxUpscale");
                int factor = 2;

                if (upscaleSetting == Forms.PostProcSettingsForm.UpscaleOption.X2) factor = 2;
                if (upscaleSetting == Forms.PostProcSettingsForm.UpscaleOption.X3) factor = 3;
                if (upscaleSetting == Forms.PostProcSettingsForm.UpscaleOption.X4) factor = 4;

                return $"-U {factor} {Config.GetFloat("sliderUpscaleStrength").ToStringDot("0.###")}";
            }
        }

        public class OptimizedSd
        {
            public static string GetDefaultArgsStartup()
            {
                List<string> args = new List<string>();

                args.Add($"--precision {(Config.GetBool("checkboxFullPrecision") ? "full" : "autocast")}"); // Precision

                return string.Join(" ", args);
            }
        }
    }
}
