using StableDiffusionGui.Io;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StableDiffusionGui.Main
{
    internal class TtiUtils
    {
        /// <returns> Path to resized image </returns>
        public static string ResizeInitImg (string path, Size targetSize, bool print)
        {
            string outPath = Path.Combine(Paths.GetSessionDataPath(), "init.png");
            Image resized = ResizeImage(IoUtils.GetImage(path), targetSize.Width, targetSize.Height);
            resized.Save(outPath);

            if (print)
                Logger.Log($"Resized init image to {targetSize.Width}x{targetSize.Height}");

            return outPath;
        }

        private static Image ResizeImage (Image image, int w, int h)
        {
            Bitmap bmp = new Bitmap(w, h);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, w, h);
                return bmp;
            }
        }

        public static void WriteModelsYaml (string mdlName)
        {
            string text = $"{mdlName}:\n" +
                $"    config: configs/stable-diffusion/v1-inference.yaml\n" +
                $"    weights: ../models/{mdlName}.ckpt\n" +
                $"    width: 512\n" +
                $"    height: 512\n";

            File.WriteAllText(Path.Combine(Paths.GetDataPath(), "repo", "configs", "models.yaml"), text);
        }

        public static void WarnIfPromptLong (List<string> prompts)
        {
            string prompt = prompts.OrderByDescending(s => s.Length).First();

            char[] delimiters = new char[] { ' ', '\r', '\n' };
            int words = prompt.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;

            int thresh = 70;

            if(words > thresh)
            {
                UiUtils.ShowMessageBox($"{(prompts.Count > 1 ? "One of your prompts" : "Your prompt")} is very long (>{thresh} words).\nThe AI might ignore parts of your prompt. Shorten the prompt to avoid this.");
            }
        }

        public static string GetCudaDevice ()
        {
            int opt = Config.GetInt(Config.Key.comboxCudaDevice);

            if (opt == 0)
                return "cpu";
            else
                return $"cuda:{opt-1}";
        }
    }
}
