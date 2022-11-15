using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System;
using System.IO;
using System.Linq;

namespace StableDiffusionGui.Installation
{
    internal class InstallationStatus
    {
        public static bool IsInstalledBasic { get { return HasBins() && HasSdRepo() && HasSdEnv() && HasSdModel(); } }
        public static bool IsInstalledAll { get { return IsInstalledBasic && HasSdUpscalers(); } }

        public static bool HasBins()
        {
            bool hasPy = File.Exists(Path.Combine(Paths.GetDataPath(), Constants.Dirs.Python, "python.exe"));
            bool hasGit = File.Exists(Path.Combine(Paths.GetDataPath(), Constants.Dirs.Git, "cmd", "git.exe"));
            bool hasWkl = File.Exists(Path.Combine(Paths.GetDataPath(), Constants.Dirs.Bins, $"{Constants.Bins.WindowsKill}.exe"));
            bool hasOk = File.Exists(Path.Combine(Paths.GetDataPath(), Constants.Dirs.Bins, $"{Constants.Bins.OrphanHitman}.exe"));

            Logger.Log($"HasBins - Has Python: {hasPy} - Has Git: {hasGit} - Has WKL: {hasWkl} - Has OK: {hasOk}", true);

            return hasPy && hasGit && hasWkl && hasOk;
        }

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
            string repoPath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdRepo);
            bool hasDreamScript = File.Exists(Path.Combine(repoPath, "scripts", "invoke.py"));

            Logger.Log($"HasSdRepo - Has invoke.py: {hasDreamScript}", true);

            return hasDreamScript;
        }

        public static bool HasSdEnv(bool conda = false)
        {
            if (conda)
            {
                bool hasPyExe = File.Exists(Path.Combine(Paths.GetDataPath(), Constants.Dirs.Conda, "envs", Constants.Dirs.SdEnv, "python.exe"));
                bool hasTorch = Directory.Exists(Path.Combine(Paths.GetDataPath(), Constants.Dirs.Conda, "envs", Constants.Dirs.SdEnv, "Lib", "site-packages", "torch"));
                bool hasBin = Directory.Exists(Path.Combine(Paths.GetDataPath(), Constants.Dirs.Conda, "envs", Constants.Dirs.SdEnv, "Library", "bin"));

                Logger.Log($"HasSdEnv - Has Python Exe: {hasPyExe} - Has Pytorch: {hasTorch} - Has bin: {hasBin}", true);

                return hasPyExe && hasTorch && hasBin;
            }
            else
            {
                bool hasPy = File.Exists(Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdVenv, "Scripts", "python.exe"));
                bool hasTorch = Directory.Exists(Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdVenv, "Lib", "site-packages", "torch"));

                Logger.Log($"HasSdEnv - Has Python Exe: {hasPy} - Has Pytorch: {hasTorch}", true);

                return hasPy && hasTorch;
            }
        }

        public static bool HasSdModel ()
        {
            return Paths.GetModels().Count() > 0;
        }

        public static bool HasSdUpscalers(bool conda = false)
        {
            string envPath = conda ? Path.Combine(Paths.GetDataPath(), Constants.Dirs.Conda, "envs", Constants.Dirs.SdEnv) : Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdVenv);
            bool hasEsrgan = Directory.Exists(Path.Combine(envPath, "Lib", "site-packages", "basicsr"));

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
