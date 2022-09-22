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
    }
}
