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
            string minicondaScriptsPath = Path.Combine(Paths.GetDataPath(), "mc", "Scripts");
            bool hasBat = IoUtils.GetAmountOfFiles(minicondaScriptsPath, false, "*.bat") > 0;

            string minicondaExePath = Path.Combine(Paths.GetDataPath(), "mc", "_conda.exe");
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
            string pyExePath = Path.Combine(Paths.GetDataPath(), "mc", "envs", "ldo", "python.exe");
            bool hasPyExe = File.Exists(pyExePath);

            string torchPath = Path.Combine(Paths.GetDataPath(), "mc", "envs", "ldo", "Lib", "site-packages", "torch");
            bool hasTorch = Directory.Exists(torchPath);

            string binPath = Path.Combine(Paths.GetDataPath(), "mc", "envs", "ldo", "Library", "bin");
            bool hasBin = Directory.Exists(binPath);

            return hasPyExe && hasTorch && hasBin;
        }

        public static bool HasSdModel ()
        {
            string mdlPath = Path.Combine(Paths.GetDataPath(), "repo", "model.ckpt");
            return File.Exists(mdlPath);
        }
    }
}
