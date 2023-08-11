namespace StableDiffusionGui.Main
{
    public class Constants
    {
        public static string NoneMdl = "None";

        public class Args
        {
            public const string Install = "install";
            public const string UpdateDeps = "update-deps";
            public const string InstallOnnx = "install-onnx";
            public const string InstallUpscalers = "install-upscalers";
        }

        public class Lognames
        {
            public const string General = "general";
            public const string Installer = "installer";
            public const string Sd = "sd";
            public const string Prune = "prune";
            public const string Merge = "merge";
            public const string Convert = "convert";
            public const string Training = "training";
            public const string Serialization = "serialization";
            public const string Api = "api";
        }

        public class Dirs
        {
            // Top Level
            public const string Data = "Data";
            public const string Images = "Images";
            public const string ImgFavs = "Favs";
            public const string Wildcards = "Wildcards";
            public const string Masks = "Masks";
            public const string Update = "update";
            // Data and below
            public const string Bins = "bin";
            public const string SdRepo = "repo";
            public const string Invoke = "invoke";
            public const string Dreambooth = "db";
            public const string SdVenv = "venv";
            public const string Python = "py";
            public const string Git = "git";

            public class Models
            {
                public const string Root = "Models";
                public const string Ckpts = "Checkpoints";
                public const string Vae = "VAEs";
                public const string Embeddings = "Embeddings";
                public const string Loras = "LoRAs";
                public const string Controlnets = "ControlNet";
                public const string Upscalers = "Upscalers";
            }

            public class Cache
            {
                public const string Root = "cache";
                public const string Diffusers = "diff";
                public const string Hf = "hf";
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
            public const string Ini = "settings.ini";
            public const string PromptHistory = "promptHistory.json";
            public static string VenvActivate { get { return $".\\{Dirs.SdVenv}\\Scripts\\activate.bat"; } }
        }

        public class FileExts
        {
            public static readonly string[] ValidImages = new string[] { ".png", ".jpeg", ".jpg", ".jfif", ".bmp", ".webp" };
            public static readonly string[] ValidEmbeddings = new string[] { ".pt", ".bin" };
            public static readonly string[] ValidSdModels = new string[] { ".ckpt" };
            public static readonly string[] ValidSdVaeModels = new string[] { ".ckpt", ".pt" };
        }

        public class SuffixesPrefixes
        {
            public const string InpaintingMdlSuf = "inpainting";
        }

        public class Limits
        {
            public const int MaxWildcardCombinations = 100000;
        }

        public class Urls
        {
            public const string ItchPage = "https://nmkd.itch.io/t2i-gui";
        }

        public class LogMsgs
        {
            public class Invoke
            {
                public const string TiTriggers = ">> Textual inversion triggers:";
            }
        }

        public class Ui
        {
            public const float DefaultLoraStrength = 1.0f;
            public const float DefaultControlnetStrength = 1.0f;
            public const int ControlnetSlots = 5;
        }
    }
}
