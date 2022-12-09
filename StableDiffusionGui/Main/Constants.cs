namespace StableDiffusionGui.Main
{
    public class Constants
    {
        public class Lognames
        {
            public const string General = "general";
            public const string Installer = "installer";
            public const string Sd = "sd";
            public const string Prune = "prune";
            public const string Merge = "merge";
            public const string Convert = "convert";
            public const string Dreambooth = "dreambooth";
        }

        public class Dirs
        {
            // Top Level
            public const string Wildcards = "Wildcards";
            public const string Masks = "Masks";
            // Data and below
            public const string Bins = "bin";
            public const string SdRepo = "repo";
            public const string Dreambooth = "db";
            public const string SdVenv = "venv";
            public const string Python = "py";
            public const string Git = "git";

            public class Cache
            {
                public const string Root = "cache";
                public const string Transformers = "trfm";
                public const string TorchHub = "torch";
                public const string Clip = "clip";
            }
        }

        public class Bins
        {
            public const string WindowsKill = "wkl";
            public const string OrphanHitman = "ok";
        }

        public class Files
        {
            public const string Config = "config.json";
        }

        public class FileExts
        {
            public static readonly string[] ValidImages = new string[] { ".png", ".jpeg", ".jpg", ".jfif", ".bmp", ".webp" };
            public static readonly string[] ValidEmbeddings = new string[] { ".pt", ".bin" };
            public static readonly string[] ValidSdModels = new string[] { ".ckpt" };
            public static readonly string[] ValidSdVaeModels = new string[] { ".ckpt", ".pt" };
        }

        public class Limits
        {
            public const int MaxWildcardCombinations = 100000;
        }
    }
}
