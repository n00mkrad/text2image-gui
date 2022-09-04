using StableDiffusionGui.Io;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StableDiffusionGui.Installation
{
    internal class InstallationStatus
    {
        public static bool IsInstalled { get { return HasConda() && HasSdRepo() && HasSdEnv() && HasSdModel(); } }

        public static bool HasConda ()
        {
            string minicondaScriptsPath = Path.Combine(Paths.GetDataPath(), "mb", "Scripts");
            bool hasBat = IoUtils.GetAmountOfFiles(minicondaScriptsPath, false, "*.bat") > 0;

            string minicondaExePath = Path.Combine(Paths.GetDataPath(), "mb", "_conda.exe");
            bool hasExe = File.Exists(minicondaExePath);

            return hasBat && hasExe;
        }

        public static bool HasSdRepo ()
        {
            string repoPath = Path.Combine(Paths.GetDataPath(), "repo");

            bool hasDreamScript = File.Exists(Path.Combine(repoPath, "scripts", "dream.py"));
            bool hasInstallScript = File.Exists(Path.Combine(repoPath, "install.cmd"));

            return hasDreamScript && hasInstallScript;
        }

        public static bool HasSdEnv()
        {
            string pyExePath = Path.Combine(Paths.GetDataPath(), "mb", "envs", "ldo", "python.exe");
            bool hasPyExe = File.Exists(pyExePath);

            string torchPath = Path.Combine(Paths.GetDataPath(), "mb", "envs", "ldo", "Lib", "site-packages", "torch");
            bool hasTorch = Directory.Exists(torchPath);

            string binPath = Path.Combine(Paths.GetDataPath(), "mb", "envs", "ldo", "Library", "bin");
            bool hasBin = Directory.Exists(binPath);

            return hasPyExe && hasTorch && hasBin;
        }

        public static bool HasSdModel ()
        {
            return IoUtils.GetAmountOfFiles(Paths.GetModelsPath(), false, "*.ckpt") > 0;
        }

        public static bool HasSdUpscalers()
        {
            string esrganPath = Path.Combine(Paths.GetDataPath(), "mb", "envs", "ldo", "Lib", "site-packages", "basicsr");
            bool hasEsrgan = Directory.Exists(esrganPath);

            string gfpPath = Path.Combine(Paths.GetDataPath(), "repo", "GFPGAN");
            string gfpMdlPath = Path.Combine(Paths.GetDataPath(), "repo", "GFPGAN", "model.pth");
            bool hasGfp = Directory.Exists(gfpPath) && File.Exists(gfpMdlPath);

            return hasEsrgan && hasGfp;
        }
    }
}
