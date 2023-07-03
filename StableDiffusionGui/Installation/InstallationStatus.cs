using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Os;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StableDiffusionGui.Installation
{
    internal class InstallationStatus
    {
        public static bool IsInstalledBasic { get { return HasBins() && HasSdRepo() && HasSdEnv(); } }
        public static bool IsInstalledAll { get { return IsInstalledBasic && HasSdUpscalers(); } }
        public static bool HasGit { get { return File.Exists(Path.Combine(Paths.GetDataPath(), Constants.Dirs.Git, "cmd", "git.exe")); } }

        public static bool HasBins()
        {
            bool hasPy = File.Exists(Path.Combine(Paths.GetDataPath(), Constants.Dirs.Python, "python.exe"));
            bool hasGit = HasGit;
            bool hasWkl = File.Exists(Path.Combine(Paths.GetDataPath(), Constants.Dirs.Bins, $"{Constants.Bins.WindowsKill}.exe"));
            bool hasOk = File.Exists(Path.Combine(Paths.GetDataPath(), Constants.Dirs.Bins, $"{Constants.Bins.OrphanHitman}.exe"));

            Logger.Log($"HasBins - Has Python: {hasPy} - Has Git: {hasGit} - Has WKL: {hasWkl} - Has OK: {hasOk}", true);
            return hasPy && hasGit && hasWkl && hasOk;
        }

        public static bool HasSdRepo ()
        {
            bool hasInvoke = File.Exists(Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdVenv, "Scripts", "invoke.exe"));
            Logger.Log($"HasSdRepo - Has invoke.exe: {hasInvoke}", true);
            return hasInvoke;
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
            return Models.GetModels().Count() > 0;
        }

        public static bool HasSdUpscalers(string logFile = Constants.Lognames.General)
        {
            string envPath = Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdVenv);
            bool hasEsrgan = Directory.Exists(Path.Combine(envPath, "Lib", "site-packages", "basicsr"));

            if (!hasEsrgan)
            {
                Logger.Log("Upscalers install incomplete: ESRGAN missing.", true, false, logFile);
                return false;
            }

            List<string> requiredFilesPaths = new List<string>
            {
                @"gfpgan/GFPGANv1.4.pth",
                @"gfpgan/weights/detection_Resnet50_Final.pth",
                @"gfpgan/weights/parsing_parsenet.pth",
                @"codeformer/codeformer.pth",
                @"realesrgan/realesr-general-wdn-x4v3.pth",
                @"realesrgan/realesr-general-x4v3.pth",
            };

            requiredFilesPaths = requiredFilesPaths.Select(f => Path.Combine(Paths.GetDataPath(), "invoke", "models", f)).ToList();
            bool hasAllModels = requiredFilesPaths.All(f => File.Exists(f));

            if (!hasAllModels)
            {
                Logger.Log($"Upscalers install incomplete: Model files missing: {string.Join($", ", requiredFilesPaths.Where(f => !File.Exists(f)))}", true, false, logFile);
                return false;
            }

            return true;
        }

        public static bool HasOnnx(bool fast = true)
        {
            if (fast)
            {
                bool diffusers = Directory.Exists(Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdVenv, "Lib", "site-packages", "diffusers"));
                bool onnx = Directory.Exists(Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdVenv, "Lib", "site-packages", "onnx"));
                bool onnxruntime = Directory.Exists(Path.Combine(Paths.GetDataPath(), Constants.Dirs.SdVenv, "Lib", "site-packages", "onnxruntime"));
                return diffusers && onnx && onnxruntime;
            }
            else
            {
                List<string> modules = OsUtils.GetPythonPkgList().Result;
                return modules.Contains("onnx") && modules.Contains("onnxruntime") && modules.Contains("onnxruntime-directml") && modules.Contains("diffusers");
            }
        }

        public static async Task<bool> HasOnnxAsync(bool fast = false)
        {
            if (fast)
            {
                return HasOnnx(true);
            }
            else
            {
                List<string> modules = await OsUtils.GetPythonPkgList();
                return modules.Contains("onnx") && modules.Contains("onnxruntime") && modules.Contains("onnxruntime-directml") && modules.Contains("diffusers");
            }
        }
    }
}
