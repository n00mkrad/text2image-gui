using StableDiffusionGui.Io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return $"--gfpgan_dir ../GFPGAN --gfpgan_model_path model.pth";
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

            string tool = "";
            string strength = Config.GetFloat(Config.Key.faceRestoreStrength).ToStringDot("0.000");

            if (faceRestoreOpt == Forms.PostProcSettingsForm.FaceRestoreOption.CodeFormer)
                tool = "codeformer";

            if (faceRestoreOpt == Forms.PostProcSettingsForm.FaceRestoreOption.Gfpgan)
                tool = "gfpgan";

            return $"-G {strength} -ft {tool}";
        }
    }
}
