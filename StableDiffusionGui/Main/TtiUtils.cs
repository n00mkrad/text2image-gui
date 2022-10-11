using StableDiffusionGui.Forms;
using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Path = System.IO.Path;
using Paths = StableDiffusionGui.Io.Paths;

namespace StableDiffusionGui.Main
{
    internal class TtiUtils
    {
        /// <returns> Path to resized image </returns>
        public static string ResizeInitImg(string path, Size targetSize, bool print = false)
        {
            string outPath = Path.Combine(Paths.GetSessionDataPath(), "init.bmp");
            Image resized = ImgUtils.ResizeImage(IoUtils.GetImage(path), targetSize.Width, targetSize.Height);
            resized.Save(outPath, System.Drawing.Imaging.ImageFormat.Bmp);

            if (print)
                Logger.Log($"Resized init image to {targetSize.Width}x{targetSize.Height}.");

            return outPath;
        }

        public static void WriteModelsYaml(string mdlName, string keyName = "default")
        {
            var mdl = Paths.GetModel(mdlName);

            string text = $"{keyName}:\n" +
                $"    config: configs/stable-diffusion/v1-inference.yaml\n" +
                $"    weights: {(mdl == null ? "unknown.ckpt" : mdl.FullName.Replace(@"\", "/").Wrap())}\n" +
                $"    width: 512\n" +
                $"    height: 512\n";

            File.WriteAllText(Path.Combine(Paths.GetDataPath(), Constants.Dirs.RepoSd, "configs", "models.yaml"), text);
        }

        public static void ShowPromptWarnings(List<string> prompts)
        {
            string prompt = prompts.OrderByDescending(s => s.Length).First();

            char[] delimiters = new char[] { ' ', '\r', '\n' };
            int words = prompt.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;

            int thresh = 55;

            if (words > thresh)
                UiUtils.ShowMessageBox($"{(prompts.Count > 1 ? "One of your prompts" : "Your prompt")} is very long (>{thresh} words).\n\nThe AI might ignore parts of your prompt. Shorten the prompt to avoid this.");

            if(Config.GetBool("checkboxOptimizedSd") && prompts.Where(x => x.MatchesRegex(@"(?:(?!\[)(?:.|\n))*\[(?:(?!\])(?:.|\n))*\]")).Any())
                UiUtils.ShowMessageBox($"{(prompts.Count > 1 ? "One of your prompts" : "Your prompt")} contains square brackets used for exclusion words.\n\nThis is currently not supported in Low Memory Mode.");
        }

        public static string GetCudaDevice(string arg)
        {
            int opt = Config.GetInt("comboxCudaDevice");

            if (opt == 0)
                return "";
            else if (opt == 1)
                return $"{arg} cpu";
            else
                return $"{arg} cuda:{opt - 2}";
        }

        public static void SoftCancelDreamPy()
        {
            var childProcesses = OsUtils.GetChildProcesses(TtiProcess.CurrentProcess);

            foreach (System.Diagnostics.Process p in childProcesses)
                OsUtils.SendCtrlC(p.Id);
        }

        public static bool CheckIfSdModelExists()
        {
            string savedModelFileName = Config.Get(Config.Key.comboxSdModel);

            if (string.IsNullOrWhiteSpace(savedModelFileName))
            {
                TextToImage.Cancel($"No Stable Diffusion model file has been set.\nPlease set one in the settings.");
                new SettingsForm().ShowDialog();
                return false;
            }
            else
            {
                var model = Paths.GetModel(savedModelFileName);

                if(model == null)
                {
                    TextToImage.Cancel($"Stable Diffusion model file {savedModelFileName.Wrap()} not found.\nPossibly it was moved, renamed, or deleted.");
                    return false;
                }
            }

            return true;
        }

        public static string GetEnvVarsSd (bool allCudaDevices = false, string baseDir = ".")
        {
            string path = OsUtils.GetTemporaryPathVariable(new string[] { $"{baseDir}/mb", $"{baseDir}/mb/Scripts", $"{baseDir}/mb/condabin", $"{baseDir}/mb/Library/bin" });

            int cudaDeviceOpt = Config.GetInt("comboxCudaDevice");
            string devicesArg = ""; // Don't set env var if cudaDeviceOpt == 0 (=> automatic)

            if (!allCudaDevices && cudaDeviceOpt > 0)
            {
                if (cudaDeviceOpt == 1) // CPU
                    devicesArg = $" && SET CUDA_VISIBLE_DEVICES=\"\""; // Set env var to empty string
                else
                    devicesArg = $" && SET CUDA_VISIBLE_DEVICES={cudaDeviceOpt - 2}"; // Set env var to selected GPU ID (-2 because the first two options are Automatic and CPU)
            }

            return $"SET PATH={path}{devicesArg}";
        }
    }
}
