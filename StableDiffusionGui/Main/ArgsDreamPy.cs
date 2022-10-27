using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using StableDiffusionGui.Os;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StableDiffusionGui.Main
{
    internal class ArgsInvoke
    {
        public static string GetPrecisionArg()
        {
            return $"{(Config.GetBool("checkboxFullPrecision") ? "--precision float32" : "")}";
        }

        public static string GetDefaultArgsStartup()
        {
            bool lowVram = GpuUtils.CachedGpus.Count > 0 && GpuUtils.CachedGpus.First().VramGb < 7.9f;

            List<string> args = new List<string>();

            if (lowVram)
            {
                if (Config.GetBool("medVramFreeGpuMem", true))
                    args.Add("--free_gpu_mem");

                if (Config.GetBool("medVramDisablePostProcessing", false))
                    args.Add("--no_upscale --no_restore");
            }

            args.Add("--gfpgan_dir ../gfpgan");
            args.Add("--gfpgan_model_path gfpgan.pth");

            return string.Join(" ", args);
        }

        public static string GetDefaultArgsCommand()
        {
            List<string> args = new List<string>();

            if (Config.GetBool("checkboxSaveUnprocessedImages"))
                args.Add("-save_orig");

            args.Add("-t");

            return string.Join(" ", args);
        }

        public static string GetEmbeddingArg(string embeddingPath)
        {
            return !string.IsNullOrWhiteSpace(embeddingPath) ? $"--embedding_path {embeddingPath.Wrap()}" : "";
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

            return $"-U {factor}";
        }
    }
}
