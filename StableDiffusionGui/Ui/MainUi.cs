using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StableDiffusionGui.Ui
{
    internal class MainUi
    {
        public static int CurrentSteps;
        public static float CurrentScale;

        public static int CurrentResW;
        public static int CurrentResH;

        private static string _currentInitImgPath;
        public static string CurrentInitImgPath {
            get => _currentInitImgPath;
            set {
                _currentInitImgPath = value;
                Logger.Log(string.IsNullOrWhiteSpace(value) ? "Cleared init image." : $"Now using initialization image {Path.GetFileName(value).Wrap()}.");
            } }
        
        public static float CurrentInitImgStrength;

        private static string _currentEmbeddingPath;
        public static string CurrentEmbeddingPath {
            get => _currentEmbeddingPath;
            set {
                _currentEmbeddingPath = value;
                Logger.Log(string.IsNullOrWhiteSpace(value) ? "Cleared embedding." : $"Now using fine-tuned embedding {Path.GetFileName(value).Wrap()}.");
            } }

        public static readonly string[] ValidInitImgExtensions = new string[] { ".png", ".jpeg", ".jpg", ".jfif", ".bmp" };
        public static readonly string[] ValidInitEmbeddingExtensions = new string[] { ".pt" };

        public static void HandleDroppedFiles(string[] paths)
        {
            if (Program.Busy)
                return;

            foreach (string path in paths.Where(x => Path.GetExtension(x) == ".png"))
            {
                ImageMetadata meta = IoUtils.GetImageMetadata(path);

                if (!string.IsNullOrWhiteSpace(meta.Prompt))
                    Logger.Log($"Found metadata in {Path.GetFileName(path)}:\n{meta.ParsedText}");
            }

            if (paths.Length == 1)
            {
                if (ValidInitImgExtensions.Contains(Path.GetExtension(paths[0]))) // Ask to use as init img
                {
                    DialogResult dialogResult = UiUtils.ShowMessageBox($"Do you want to load this image as an initialization image?", $"Dropped {Path.GetFileName(paths[0]).Trunc(40)}", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                        CurrentInitImgPath = paths[0];
                }

                if (ValidInitEmbeddingExtensions.Contains(Path.GetExtension(paths[0]))) // Ask to use as embedding (finetuned model)
                {
                    DialogResult dialogResult = UiUtils.ShowMessageBox($"Do you want to load this embedding?", $"Dropped {Path.GetFileName(paths[0]).Trunc(40)}", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                        CurrentEmbeddingPath = paths[0];
                }

                Program.MainForm.UpdateInitImgAndEmbeddingUi();
            }
        }

        public static string SanitizePrompt (string prompt)
        {
            prompt = new Regex(@"[^a-zA-Z0-9 -!*,.:()\-]").Replace(prompt, "");
            prompt = prompt.Replace(" -", " ");

            while (prompt.StartsWith("-"))
                prompt = prompt.Substring(1);
            
            while (prompt.EndsWith("-"))
                prompt = prompt.Remove(prompt.Length - 1);

            return prompt;
        }

        public static List<float> GetScales(string customScalesText)
        {
   
            if (customScalesText.MatchesWildcard("* > * : *"))
            {
                var splitMinMax = customScalesText.Trim().Split(':')[0].Split('>');
                float valFrom = splitMinMax[0].GetFloat();
                float valTo = splitMinMax[1].Trim().GetFloat();
                float step = Math.Abs(customScalesText.Split(':').Last().GetFloat());
                
                if (valFrom > valTo) 
                    (valFrom, valTo) = (valTo, valFrom);

                return new List<float>(
                    from x in Enumerable.Range(0, 1 + (int)((valTo - valFrom) / step))
                    select valFrom + x * step);
            }
            List<float> scales = new List<float> { CurrentScale };
            scales.AddRange(customScalesText.Replace(" ", "").Split(",").Select(x => x.GetFloat()));
            
            return scales;
        }

        public static List<float> GetInitStrengths(string customStrengthsText)
        {
  
            if (customStrengthsText.MatchesWildcard("* > * : *"))
            {
                var splitMinMax = customStrengthsText.Trim().Split(':')[0].Split('>');
                float valFrom = splitMinMax[0].GetFloat();
                float valTo = splitMinMax[1].Trim().GetFloat();
                float step = customStrengthsText.Split(':').Last().GetFloat();

                List<float> incrementStrengths = new List<float>();

                if (valFrom > valTo) 
                    (valFrom, valTo) = (valTo, valFrom);

                return new List<float>(
                    from x in Enumerable.Range(0, 1 + (int)((valTo - valFrom) / step))
                    select 1f - ( valFrom + x * step));
            } 
            
            List<float> strengths = new List<float> { 1f - CurrentInitImgStrength };
            strengths.AddRange(customStrengthsText.Replace(" ", "").Split(",").Select(x => x.GetFloat()).Where(x => x > 0.05f).Select(x => 1f - x));
            return strengths;
        }
    }
}
