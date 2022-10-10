using StableDiffusionGui.Data;
using StableDiffusionGui.Forms;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Os;
using StableDiffusionGui.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
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
                Logger.Log(string.IsNullOrWhiteSpace(value) ? "" : $"Now using initialization image {Path.GetFileName(value).Wrap()}.");
                if(InpaintingUtils.CurrentMask != null)
                {
                    InpaintingUtils.CurrentMask = null;
                    Logger.Log("Inpainting mask has been cleared.");
                }
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

        public static Dictionary<string, string> UiStrings = new Dictionary<string, string>
        {
            { Enums.StableDiffusion.Sampler.K_Euler_A.ToString(), "Euler Ancestral" },
            { Enums.StableDiffusion.Sampler.K_Euler.ToString(), "Euler" },
            { Enums.StableDiffusion.Sampler.K_Lms.ToString(), "LMS" },
            { Enums.StableDiffusion.Sampler.Ddim.ToString(), "DDIM" },
            { Enums.StableDiffusion.Sampler.Plms.ToString(), "PLMS" },
            { Enums.StableDiffusion.Sampler.K_Heun.ToString(), "Heun" },
            { Enums.StableDiffusion.Sampler.K_Dpm_2.ToString(), "DPM 2" },
            { Enums.StableDiffusion.Sampler.K_Dpm_2_A.ToString(), "DPM 2 Ancestral" },
        };

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

            if (paths.Length == 1)
            {
                if (ValidInitImgExtensions.Contains(Path.GetExtension(paths[0]).ToLower())) // Ask to use as init img
                {
                    ImageLoadForm imgForm = new ImageLoadForm(paths[0]);
                    imgForm.ShowDialog();

                    if (imgForm.Action == ImageLoadForm.ImageAction.InitImage)
                        CurrentInitImgPath = paths[0];
                    else if (imgForm.Action == ImageLoadForm.ImageAction.LoadSettings)
                        Program.MainForm.LoadMetadataIntoUi(imgForm.CurrentMetadata);
                    else if (imgForm.Action == ImageLoadForm.ImageAction.CopyPrompt)
                        OsUtils.SetClipboard(imgForm.CurrentMetadata.Prompt);
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

        public static void HandlePaste ()
        {
            try
            {
                Image clipboardImg = Clipboard.GetImage();

                if (clipboardImg == null)
                    return;

                string savePath = Path.Combine(Paths.GetSessionDataPath(), "clipboard.png");
                clipboardImg.Save(savePath);
                MainUi.HandleDroppedFiles(new string[] { savePath });
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to paste image from clipboard: {ex.Message}\n{ex.StackTrace}", true);
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

        public enum PromptFieldSizeMode { Expand, Collapse, Toggle }

        public static void SetPromptFieldSize (PromptFieldSizeMode sizeMode = PromptFieldSizeMode.Toggle)
        {
            var form = Program.MainForm;
            int smallHeight = 59;

            if (sizeMode == PromptFieldSizeMode.Toggle)
                sizeMode = form.TextboxPrompt.Height == smallHeight ? PromptFieldSizeMode.Expand : PromptFieldSizeMode.Collapse;

            if (sizeMode == PromptFieldSizeMode.Expand)
            {
                form.BtnExpandPromptField.BackgroundImage = Resources.upArrowIcon;
                form.TextboxPrompt.Height = form.PictBoxImgViewer.Height + 65;
                form.PictBoxImgViewer.Visible = false;
            }

            if (sizeMode == PromptFieldSizeMode.Collapse)
            {
                form.BtnExpandPromptField.BackgroundImage = Resources.downArrowIcon;
                form.TextboxPrompt.Height = smallHeight;
                form.PictBoxImgViewer.Visible = true;
            }
        }

        public static async Task SetGpusInWindowTitle()
        {
            var gpus = await GpuUtils.GetCudaGpus();
            List<string> gpuNames = gpus.Select(x => x.FullName).ToList();
            int maxGpusToListInTitle = 2;

            if (gpuNames.Count < 1)
                Program.MainForm.Text = $"{Program.MainForm.Text} - No CUDA GPUs available.";
            else if (gpuNames.Count <= maxGpusToListInTitle)
                Program.MainForm.Text = $"{Program.MainForm.Text} - CUDA GPU{(gpuNames.Count != 1 ? "s" : "")}: {string.Join(", ", gpuNames)}";
            else
                Program.MainForm.Text = $"{Program.MainForm.Text} - CUDA GPUs: {string.Join(", ", gpuNames.Take(maxGpusToListInTitle))} (+{gpuNames.Count - maxGpusToListInTitle})";

            Logger.Log($"Detected {gpus.Count.ToString().Replace("0", "no")} CUDA-capable GPU{(gpus.Count != 1 ? "s" : "")}.");
        }
    }
}
