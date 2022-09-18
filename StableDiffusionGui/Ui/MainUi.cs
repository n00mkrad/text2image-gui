using StableDiffusionGui.Data;
using StableDiffusionGui.Forms;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
                Logger.Log(string.IsNullOrWhiteSpace(value) ? "" : $"Now using initialization image {Path.GetFileName(value).Wrap()}.");
            }
        }
        
        public static float CurrentInitImgStrength;

        private static string _currentEmbeddingPath;
        public static string CurrentEmbeddingPath {
            get => _currentEmbeddingPath;
            set {
                _currentEmbeddingPath = value;
                Logger.Log(string.IsNullOrWhiteSpace(value) ? "" : $"Now using learned concept {Path.GetFileName(value).Wrap()}.");
            }
        }

        public static List<TtiSettings> Queue = new List<TtiSettings>();

        public static readonly string[] ValidInitImgExtensions = new string[] { ".png", ".jpeg", ".jpg", ".jfif", ".bmp", ".webp" };
        public static readonly string[] ValidInitEmbeddingExtensions = new string[] { ".pt", ".bin" };

        public static void DoStartupChecks ()
        {
            if (!Debugger.IsAttached)
            {
                string dir = Paths.GetExeDir();

                if (dir.ToLower().Replace("\\", "/").MatchesWildcard("*/users/*/onedrive/*"))
                {
                    UiUtils.ShowMessageBox($"Running this program out of the OneDrive folder is not supported. Please move it to a local drive and try again.", UiUtils.MessageType.Error, Nmkoder.Forms.MessageForm.FontSize.Big);
                    Application.Exit();
                }

                if (dir.Length > 70)
                    UiUtils.ShowMessageBox($"You are running the program from this path:\n\n{Paths.GetExeDir()}\n\nIt's very long ({dir.Length} characters), this can cause problems.\n" +
                        $"Please move the program to a shorter path or continue at your own risk.", UiUtils.MessageType.Warning, Nmkoder.Forms.MessageForm.FontSize.Big);

                UiUtils.ShowMessageBox("READ THIS FIRST!\n\nThis software is still in development and may contain bugs.\n\nImportant:\n" +
                "- You MUST have a recent (GTX 10 series or newer) Nvidia graphics card to use this.\n" +
                "- You need as much VRAM (graphics card memory) as possible. IF YOU HAVE LESS THAN 8 GB, use this at your own risk, it might not work at all!!\n" +
                "- The resolution settings is very VRAM-heavy. I do not recommend going above 512x512 unless you have 8+ GB VRAM.\n\n" +
                "Last but not least, this GUI includes tooltips, so if you're not sure what a button or other control does, hover over it with your cursor and an info message will pop up.\n\nHave fun!", UiUtils.MessageType.Warning, Nmkoder.Forms.MessageForm.FontSize.Big);
            }
            else
            {
                Logger.Log("Debugger is attached.");
            }

            if (!InstallationStatus.IsInstalled)
            {
                UiUtils.ShowMessageBox("No complete installation of the Stable Diffusion files was found.\n\nThe GUI will now open the installer.\nPlease press \"Install\" in the next window to install all required files.");
                new InstallerForm().ShowDialog();
            }
        }

        public static bool IsInstalledWithWarning(bool showInstaller = true)
        {
            if (!InstallationStatus.IsInstalled)
            {
                UiUtils.ShowMessageBox("A valid installation is required.");

                if (showInstaller)
                    new InstallerForm().ShowDialog();

                return false;
            }

            return true;
        }

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
                if (ValidInitImgExtensions.Contains(Path.GetExtension(paths[0]).ToLower())) // Ask to use as init img
                {
                    DialogResult dialogResult = UiUtils.ShowMessageBox($"Do you want to load this image as an initialization image?", $"Dropped {Path.GetFileName(paths[0]).Trunc(40)}", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                        CurrentInitImgPath = paths[0];
                }

                if (ValidInitEmbeddingExtensions.Contains(Path.GetExtension(paths[0]).ToLower())) // Ask to use as embedding (finetuned model)
                {
                    DialogResult dialogResult = UiUtils.ShowMessageBox($"Do you want to load this concept?", $"Dropped {Path.GetFileName(paths[0]).Trunc(40)}", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                        CurrentEmbeddingPath = paths[0];
                }

                Program.MainForm.UpdateInitImgAndEmbeddingUi();
            }
        }

        public static string SanitizePrompt (string prompt)
        {
            //prompt = new Regex(@"[^a-zA-Z0-9 -!*,.:()_\-]").Replace(prompt, "");
            prompt = prompt.Replace(" -", " ");

            while (prompt.StartsWith("-"))
                prompt = prompt.Substring(1);
            
            while (prompt.EndsWith("-"))
                prompt = prompt.Remove(prompt.Length - 1);

            return prompt;
        }

        public static List<float> GetScales(string customScalesText)
        {
            List<float> scales = new List<float> { CurrentScale };

            if (customScalesText.MatchesWildcard("* > * : *"))
            {
                var splitMinMax = customScalesText.Trim().Split(':')[0].Split('>');
                float valFrom = splitMinMax[0].GetFloat();
                float valTo = splitMinMax[1].Trim().GetFloat();
                float step = customScalesText.Split(':').Last().GetFloat();

                List<float> incrementScales = new List<float>();

                if (valFrom < valTo)
                {
                    for (float f = valFrom; f < (valTo + 0.01f); f += step)
                        incrementScales.Add(f);
                }
                else
                {
                    for (float f = valFrom; f >= (valTo - 0.01f); f -= step)
                        incrementScales.Add(f);
                }

                if (incrementScales.Count > 0)
                    scales = incrementScales; // Replace list, don't use the regular scale slider at all in this mode
            }
            else
            {
                scales.AddRange(customScalesText.Replace(" ", "").Split(",").Select(x => x.GetFloat()).Where(x => x > 0.05f));
            }

            return scales;
        }

        public static List<float> GetInitStrengths(string customStrengthsText)
        {
            List<float> strengths = new List<float> { 1f - CurrentInitImgStrength };

            if (customStrengthsText.MatchesWildcard("* > * : *"))
            {
                var splitMinMax = customStrengthsText.Trim().Split(':')[0].Split('>');
                float valFrom = splitMinMax[0].GetFloat();
                float valTo = splitMinMax[1].Trim().GetFloat();
                float step = customStrengthsText.Split(':').Last().GetFloat();

                List<float> incrementStrengths = new List<float>();

                if(valFrom < valTo)
                {
                    for (float f = valFrom; f < (valTo + 0.01f); f += step)
                        incrementStrengths.Add(1f - f);
                }
                else
                {
                    for (float f = valFrom; f >= (valTo - 0.01f); f -= step)
                        incrementStrengths.Add(1f - f);
                }

                if (incrementStrengths.Count > 0)
                    strengths = incrementStrengths; // Replace list, don't use the regular scale slider at all in this mode
            }
            else
            {
                strengths.AddRange(customStrengthsText.Replace(" ", "").Split(",").Select(x => x.GetFloat()).Where(x => x > 0.05f).Select(x => 1f - x));
            }

            return strengths;
        }
    }
}
