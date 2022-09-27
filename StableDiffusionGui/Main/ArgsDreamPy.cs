using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using StableDiffusionGui.Os;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StableDiffusionGui.Main
{
    internal class ArgsDreamPy
    {
        public static string GetPrecisionArg()
        {
            return $"{(Config.GetBool("checkboxFullPrecision") ? "--precision float32" : "")}";
        }

        public static string GetDefaultArgs ()
        {
            bool lowVram = GpuUtils.CachedGpus.Count > 0 && GpuUtils.CachedGpus.First().VramGb < 7.9f;

            List<string> args = new List<string>();

            if (lowVram)
                args.Add("--free_gpu_mem");

            args.Add("--gfpgan_dir ../gfpgan");
            args.Add("--gfpgan_model_path gfpgan.pth");

            return string.Join(" ", args);
        }

        public static string GetEmbeddingArg (string embeddingPath)
        {
            return !string.IsNullOrWhiteSpace(embeddingPath) ? $"--embedding_path {embeddingPath.Wrap()}" : "";
        }

        public static string GetFaceRestoreArgs()
        {
            var faceRestoreOpt = (Forms.PostProcSettingsForm.FaceRestoreOption)Config.GetInt("comboxFaceRestoration");

            if (faceRestoreOpt == Forms.PostProcSettingsForm.FaceRestoreOption.Disabled)
                return "";

            if (!InstallationStatus.HasSdUpscalers())
                return "";

            string tool = "";
            string strength = Config.GetFloat("sliderFaceRestoreStrength").ToStringDot("0.000");

            if (faceRestoreOpt == Forms.PostProcSettingsForm.FaceRestoreOption.CodeFormer)
                tool = $"codeformer -cf {Config.GetFloat(Config.Key.sliderCodeformerFidelity).ToStringDot()}";

            if (faceRestoreOpt == Forms.PostProcSettingsForm.FaceRestoreOption.Gfpgan)
                tool = "gfpgan";

            return $"-G {strength} -ft {tool}";
        }

        public static string GetUpscaleArgs ()
        {
            var upscaleSetting = (Forms.PostProcSettingsForm.UpscaleOption)Config.GetInt("comboxUpscale");

            if (upscaleSetting == Forms.PostProcSettingsForm.UpscaleOption.Disabled)
                return "";

            int factor = 2;

            if (upscaleSetting == Forms.PostProcSettingsForm.UpscaleOption.X2) factor = 2;
            if (upscaleSetting == Forms.PostProcSettingsForm.UpscaleOption.X3) factor = 3;
            if (upscaleSetting == Forms.PostProcSettingsForm.UpscaleOption.X4) factor = 4;

            return $"-U {factor}";
        }
    }
}
