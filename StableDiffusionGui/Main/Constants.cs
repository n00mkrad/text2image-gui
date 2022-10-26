namespace StableDiffusionGui.Main
{
    public class Constants
    {
        public class Lognames
        {
            public const string Session = "sessionlog";
            public const string Installer = "installer";
            public const string Sd = "sd";
            public const string Prune = "prune";
            public const string Merge = "merge";
            public const string Dreambooth = "dreambooth";
        }

        public class Dirs
        {
            public const string Bins = "bin";
            public const string RepoSd = "repo";
            public const string Dreambooth = "db";
            public const string Wildcards = "Wildcards";
            public const string SdEnv = "ldo";
            public const string Conda = "mb";
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

        public class FileExtensions
        {
            public static readonly string[] ValidImages = new string[] { ".png", ".jpeg", ".jpg", ".jfif", ".bmp", ".webp" };
            public static readonly string[] ValidEmbeddings = new string[] { ".pt", ".bin" };
        }
    }
}
