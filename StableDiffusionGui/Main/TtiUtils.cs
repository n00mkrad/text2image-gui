using ImageMagick;
using StableDiffusionGui.Forms;
using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Properties;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        public static void WriteModelsYaml(string mdlName)
        {
            string text = $"{mdlName}:\n" +
                $"    config: configs/stable-diffusion/v1-inference.yaml\n" +
                $"    weights: ../models/{mdlName}.ckpt\n" +
                $"    width: 512\n" +
                $"    height: 512\n";

            File.WriteAllText(Path.Combine(Paths.GetDataPath(), "repo", "configs", "models.yaml"), text);
        }

        public static void WarnIfPromptLong(List<string> prompts)
        {
            string prompt = prompts.OrderByDescending(s => s.Length).First();

            char[] delimiters = new char[] { ' ', '\r', '\n' };
            int words = prompt.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;

            int thresh = 70;

            if (words > thresh)
                UiUtils.ShowMessageBox($"{(prompts.Count > 1 ? "One of your prompts" : "Your prompt")} is very long (>{thresh} words).\nThe AI might ignore parts of your prompt. Shorten the prompt to avoid this.");
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
            IoUtils.TryDeleteIfExists(Path.Combine(Paths.GetSessionDataPath(), "prompts.txt"));

            var childProcesses = OsUtils.GetChildProcesses(TtiProcess.DreamPyParentProcess);

            foreach (System.Diagnostics.Process p in childProcesses)
                OsUtils.SendCtrlC(p.Id);
        }

        public static bool CheckIfSdModelExists()
        {
            if (!File.Exists(Path.Combine(Paths.GetModelsPath(), GetSdModel(true))))
            {
                string savedModelFileName = Config.Get(Config.Key.comboxSdModel);

                if (string.IsNullOrWhiteSpace(savedModelFileName))
                {
                    TextToImage.Cancel($"No Stable Diffusion model file has been set.\nPlease set one in the settings.");
                    new SettingsForm().ShowDialog();
                }
                else
                {
                    TextToImage.Cancel($"Stable Diffusion model file {savedModelFileName.Wrap()} not found.\nPossibly it was moved, renamed, or deleted.");
                }

                return false;
            }

            return true;
        }

        public static string GetSdModel(bool withExtension = false)
        {
            string filename = Config.Get(Config.Key.comboxSdModel);
            return withExtension ? filename : Path.GetFileNameWithoutExtension(filename);
        }

        public static string GetPathVariableCmd (string baseDir = ".")
        {
            return $"SET PATH={OsUtils.GetTemporaryPathVariable(new string[] { $"{baseDir}/mb", $"{baseDir}/mb/Scripts", $"{baseDir}/mb/condabin", $"{baseDir}/mb/Library/bin" })}";
        }
    }
}
