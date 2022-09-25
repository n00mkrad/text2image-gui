using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using System;

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
            return $"--gfpgan_dir ../gfpgan --gfpgan_model_path gfpgan.pth";
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
    }
}
