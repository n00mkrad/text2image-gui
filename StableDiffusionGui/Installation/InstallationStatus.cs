using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Os;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StableDiffusionGui.Installation
{
    internal class InstallationStatus
    {
        public static bool IsInstalledBasic { get { return HasBins() && HasSdRepo() && HasSdEnv(); } }
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

        public static bool HasSdRepo ()
        {
            string repoPath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdRepo);
            bool hasDreamScript = File.Exists(Path.Combine(repoPath, "scripts", "invoke.py"));

            Logger.Log($"HasSdRepo - Has invoke.py: {hasDreamScript}", true);

            return hasDreamScript;
        }

        public static bool HasSdEnv()
        {
            bool hasPy = File.Exists(Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdVenv, "Scripts", "python.exe"));
            bool hasTorch = Directory.Exists(Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdVenv, "Lib", "site-packages", "torch"));

            Logger.Log($"HasSdEnv - Has Python Exe: {hasPy} - Has Pytorch: {hasTorch}", true);

            return hasPy && hasTorch;
        }

        public static bool HasSdModel ()
        {
            return Paths.GetModels().Count() > 0;
        }

        public static bool HasSdUpscalers()
        {
            string envPath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdVenv);
            bool hasEsrgan = Directory.Exists(Path.Combine(envPath, "Lib", "site-packages", "basicsr"));

            string gfpPath = Path.Combine(Paths.GetDataPath(), "gfpgan");
            string gfpMdlPath = Path.Combine(Paths.GetDataPath(), "gfpgan", "gfpgan.pth");
            bool hasGfp = Directory.Exists(gfpPath) && File.Exists(gfpMdlPath);

            string cfMdlPath = Path.Combine(Paths.GetDataPath(), "codeformer", "codeformer.pth");
            bool hasCf = File.Exists(cfMdlPath);

            Logger.Log($"HasSdUpscalers - Has ESRGAN: {hasEsrgan} - Has GFPGAN: {hasGfp} - Has Codeformer: {hasCf}", true);

            return hasEsrgan && hasGfp && hasCf;
        }

        public static bool HasOnnx()
        {
            List<string> modules = OsUtils.GetPythonPkgList().Result;
            return modules.Contains("onnx") && modules.Contains("onnxruntime") && modules.Contains("onnxruntime-directml") && modules.Contains("diffusers");
        }

        public static async Task<bool> HasOnnxAsync()
        {
            List<string> modules = await OsUtils.GetPythonPkgList();
            return modules.Contains("onnx") && modules.Contains("onnxruntime") && modules.Contains("onnxruntime-directml") && modules.Contains("diffusers");
        }
    }
}
