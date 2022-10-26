using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System;
using System.IO;
using System.Linq;

namespace StableDiffusionGui.Installation
{
    internal class InstallationStatus
    {
        public static bool IsInstalledBasic { get { return HasConda() && HasSdRepo() && HasSdEnv() && HasSdModel(); } }
        public static bool IsInstalledAll { get { return IsInstalledBasic && HasSdUpscalers(); } }

        public static bool HasConda ()
        {
            string minicondaScriptsPath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.Conda, "Scripts");
            bool hasBat = IoUtils.GetAmountOfFiles(minicondaScriptsPath, false, "*.bat") > 0;

            string minicondaExePath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.Conda, "_conda.exe");
            bool hasExe = File.Exists(minicondaExePath);

            Logger.Log($"HasConda - Has *.bat: {hasBat} - Has _conda.exe: {hasExe}", true);

            return hasBat && hasExe;
        }

        public static bool HasSdRepo ()
        {
            string repoPath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.RepoSd);
            bool hasDreamScript = File.Exists(Path.Combine(repoPath, "scripts", "invoke.py"));

            Logger.Log($"HasSdRepo - Has invoke.py: {hasDreamScript}", true);

            return hasDreamScript;
        }

        public static bool HasSdEnv()
        {
            string pyExePath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.Conda, "envs", Constants.Dirs.SdEnv, "python.exe");
            bool hasPyExe = File.Exists(pyExePath);

            string torchPath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.Conda, "envs", Constants.Dirs.SdEnv, "Lib", "site-packages", "torch");
            bool hasTorch = Directory.Exists(torchPath);

            string binPath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.Conda, "envs", Constants.Dirs.SdEnv, "Library", "bin");
            bool hasBin = Directory.Exists(binPath);

            Logger.Log($"HasSdEnv - Has Python Exe: {hasPyExe} - Has Pytorch: {hasTorch} - Has bin: {hasBin}", true);

            return hasPyExe && hasTorch && hasBin;
        }

        public static bool HasSdModel ()
        {
            return Paths.GetModels().Count() > 0;
        }

        public static bool HasSdUpscalers()
        {
            string esrganPath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.Conda, "envs", Constants.Dirs.SdEnv, "Lib", "site-packages", "basicsr");
            bool hasEsrgan = Directory.Exists(esrganPath);

            string gfpPath = Path.Combine(Paths.GetDataPath(), "gfpgan");
            string gfpMdlPath = Path.Combine(Paths.GetDataPath(), "gfpgan", "gfpgan.pth");
            bool hasGfp = Directory.Exists(gfpPath) && File.Exists(gfpMdlPath);

            string cfMdlPath = Path.Combine(Paths.GetDataPath(), "codeformer", "codeformer.pth");
            bool hasCf = File.Exists(cfMdlPath);

            Logger.Log($"HasSdUpscalers - Has ESRGAN: {hasEsrgan} - Has GFPGAN: {hasGfp} - Has Codeformer: {hasCf}", true);

            return hasEsrgan && hasGfp && hasCf;
        }
    }
}
