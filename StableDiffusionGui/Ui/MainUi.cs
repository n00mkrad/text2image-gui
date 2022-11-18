using StableDiffusionGui.Data;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Forms;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using System.Windows.Forms;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Ui
{
    internal class MainUi
    {
        private static List<string> _currentInitImgPaths;
        public static List<string> CurrentInitImgPaths {
            get => _currentInitImgPaths;
            set {
                _currentInitImgPaths = value;

                if(value != null && value.Count() > 0)
                    Logger.Log(value.Count() == 1 ? $"Now using initialization image {Path.GetFileName(value[0]).Wrap()}." : $"Now using {value.Count()} initialization images.");

                if (InpaintingUtils.CurrentMask != null)
                {
                    InpaintingUtils.CurrentMask = null;
                    Logger.Log("Inpainting mask has been cleared.");
                }
            }
        }
        
        private static string _currentEmbeddingPath;
        public static string CurrentEmbeddingPath {
            get => _currentEmbeddingPath;
            set {
                _currentEmbeddingPath = value;
                Logger.Log(string.IsNullOrWhiteSpace(value) ? "" : $"Now using learned concept {Path.GetFileName(value).Wrap()}.");
            }
        }

        public static List<TtiSettings> Queue = new List<TtiSettings>();

        public static List<int> Resolutions { get { return GetNumbers(384, 2048, 64); } }

        public static List<int> GetNumbers (int min, int max, int step)
        {
            return Enumerable.Range(min, (max - min) + 1).Where(x => x % step == 0).ToList();
        }

        public static void DoStartupChecks ()
        {
            if (!Program.Debug)
            {
                string dir = Paths.GetExeDir();

                List<char> nonAsciiCharsInPath = FormatUtils.GetNonAsciiChars(dir);

                if (nonAsciiCharsInPath.Count > 0)
                {
                    UiUtils.ShowMessageBox($"You are running this program from a path that contains special characters ({string.Join(", ", nonAsciiCharsInPath.Distinct())}).\n" +
                        $"Please move it to a path without special characters and try again.", UiUtils.MessageType.Error, Nmkoder.Forms.MessageForm.FontSize.Big);
                    Application.Exit();
                }

                if (dir.Lower().Replace("\\", "/").MatchesWildcard("*/users/*/onedrive/*"))
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

            if (!InstallationStatus.IsInstalledBasic)
            {
                UiUtils.ShowMessageBox("No complete installation of the Stable Diffusion files was found.\n\nThe GUI will now open the installer.\nPlease press \"Install\" in the next window to install all required files.");
                new InstallerForm().ShowDialogForm();
            }
        }

        public static bool IsInstalledWithWarning(bool showInstaller = true)
        {
            if (!InstallationStatus.IsInstalledBasic)
            {
                UiUtils.ShowMessageBox("A valid installation is required.");

                if (showInstaller)
                    new InstallerForm().ShowDialogForm();

                return false;
            }

            return true;
        }

        public static void HandleDroppedFiles(string[] paths, bool noConfirmations = false)
        {
            if (Program.Busy || paths == null || paths.Length < 1)
                return;

            if (paths.Length == 1)
            {
                if (Constants.FileExts.ValidImages.Contains(Path.GetExtension(paths[0]).Lower())) // Ask to use as init img
                {
                    ImageLoadForm imgForm = new ImageLoadForm(paths[0]);
                    imgForm.ShowDialogForm();

                    if (imgForm.Action == ImageLoadForm.ImageAction.InitImage)
                        AddInitImages(paths.ToList());
                    else if (imgForm.Action == ImageLoadForm.ImageAction.LoadSettings)
                        Program.MainForm.LoadMetadataIntoUi(imgForm.CurrentMetadata);
                    else if (imgForm.Action == ImageLoadForm.ImageAction.CopyPrompt)
                        OsUtils.SetClipboard(imgForm.CurrentMetadata.Prompt);
                }

                if (Constants.FileExts.ValidEmbeddings.Contains(Path.GetExtension(paths[0]).Lower())) // Ask to use as embedding (TI)
                {
                    DialogResult dialogResult = UiUtils.ShowMessageBox($"Do you want to load this concept?", $"Dropped {Path.GetFileName(paths[0]).Trunc(40)}", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                        CurrentEmbeddingPath = paths[0];
                }

                Program.MainForm.UpdateInitImgAndEmbeddingUi();
            }
            else
            {
                paths = paths.OrderBy(path => Path.GetFileName(path)).ToArray(); // Sort by filename
                var validImagesInPathList = paths.Where(path => Constants.FileExts.ValidImages.Contains(Path.GetExtension(path).Lower()));

                if (validImagesInPathList.Any())
                {
                    DialogResult dialogResult = noConfirmations ? DialogResult.Yes : UiUtils.ShowMessageBox($"Do you want to load these images as initialization images?", $"Dropped {paths.Length} Images", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                        AddInitImages(paths.ToList());
                }
            }
        }

        private static void AddInitImages(List<string> paths)
        {
            if (paths.Count < 1)
                return;

            if(CurrentInitImgPaths != null)
            {
                bool oldIs1 = CurrentInitImgPaths.Count == 1;
                bool newIs1 = paths.Count == 1;

                string msg = $"Do you want to replace the currently loaded {(oldIs1 ? $"image '{Path.GetFileName(CurrentInitImgPaths[0])}'" : $"{CurrentInitImgPaths.Count} images")}?\n\n" +
                    $"Press \"No\" to append {(newIs1 ? "it" : "them")} to the list instead.";
                DialogResult dialogResult = UiUtils.ShowMessageBox(msg, $"Replace current image{(oldIs1 ? "" : "s")}?", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                    CurrentInitImgPaths = paths;
                else
                    CurrentInitImgPaths = CurrentInitImgPaths.Concat(paths).ToList();
            }
            else
            {
                CurrentInitImgPaths = paths;
            }

            Program.MainForm.UpdateInitImgAndEmbeddingUi();
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
            prompt = prompt.Replace(" -", " "); // Don't allow leading hyphens (might be misunderstood as args)

            while (prompt.StartsWith("-"))
                prompt = prompt.Substring(1); // Remove leading hypens

            prompt = InvokeAiUtils.ConvertOldAttentionSyntax(prompt); // Convert old (multi-bracket) emphasis/attention syntax to new one (with +/-)

            return prompt;
        }

        public static List<float> GetScales(string customScalesText)
        {
            List<float> scales = new List<float> { Program.MainForm.SliderScale.ActualValueFloat };

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
            List<float> strengths = new List<float> { 1f - Program.MainForm.SliderStrength.ActualValueFloat };

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

        public static void SetPromptFieldSize (PromptFieldSizeMode sizeMode = PromptFieldSizeMode.Toggle, bool negativePromptField = false)
        {
            var form = Program.MainForm;
            var panel = negativePromptField ? form.TextboxPromptNeg.Parent : form.TextboxPrompt.Parent;
            var btn = negativePromptField ? form.BtnExpandPromptNegField : form.BtnExpandPromptField;
            int smallHeight = negativePromptField ? 40 : 65;

            if (sizeMode == PromptFieldSizeMode.Toggle)
                sizeMode = panel.Height == smallHeight ? PromptFieldSizeMode.Expand : PromptFieldSizeMode.Collapse;

            if (sizeMode == PromptFieldSizeMode.Expand)
            {
                btn.BackgroundImage = Resources.upArrowIcon;
                panel.Height = smallHeight * 4;
            }

            if (sizeMode == PromptFieldSizeMode.Collapse)
            {
                btn.BackgroundImage = Resources.downArrowIcon;
                panel.Height = smallHeight;
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

        public static void SetSettingsVertScrollbar ()
        {
            try
            {
                Program.MainForm.PanelSettings.AutoScrollMinSize = new Size(Program.MainForm.PanelSettings.AutoScrollMinSize.Width, Program.MainForm.PanelSettings.Height + 1);
            }
            catch { }
        }

        public static void SetHiresFixVisible (ComboBox w, ComboBox h, CheckBox fix)
        {
            fix.Visible = w.GetInt() > 512 && h.GetInt() > 512 && (Implementation)Config.GetInt("comboxImplementation") == Implementation.InvokeAi;
        }

        public static void FitWindowSizeToImageSize ()
        {
            if (Program.MainForm.PictBoxImgViewer.Image.Size == Program.MainForm.PictBoxImgViewer.Size)
                return;

            int formWidthWithoutImgViewer = Program.MainForm.Size.Width - Program.MainForm.PictBoxImgViewer.Width;
            int formHeightWithoutImgViewer = Program.MainForm.Size.Height - Program.MainForm.PictBoxImgViewer.Height;

            Size targetSize = new Size(Program.MainForm.PictBoxImgViewer.Image.Width + formWidthWithoutImgViewer, Program.MainForm.PictBoxImgViewer.Image.Height + formHeightWithoutImgViewer);
            Program.MainForm.Size = new Size(targetSize.Width.Clamp(512, int.MaxValue), targetSize.Height.Clamp(512, int.MaxValue));
        }

        public static void LoadAutocompleteData(AutocompleteMenuNS.AutocompleteMenu menu, TextBox textbox)
        {
            LoadAutocompleteData(menu, new[] { textbox });
        }

        public static void LoadAutocompleteData (AutocompleteMenuNS.AutocompleteMenu menu, IEnumerable<TextBox> textboxes)
        {
            foreach(TextBox textbox in textboxes)
            {
                List<string> autoCompleteStrings = new List<string>();

                autoCompleteStrings.AddRange(IoUtils.GetFileInfosSorted(Path.Combine(Paths.GetExeDir(), "Wildcards")).Select(x => $"{x.NameNoExt()}"));
                // autoCompleteStrings.AddRange(IoUtils.GetFileInfosSorted(Path.Combine(Paths.GetExeDir(), "Wildcards")).Select(x => $"~{x.NameNoExt()}"));
                // autoCompleteStrings.AddRange(IoUtils.GetFileInfosSorted(Path.Combine(Paths.GetExeDir(), "Wildcards")).Select(x => $"~~{x.NameNoExt()}"));
                // autoCompleteStrings.AddRange(IoUtils.GetFileInfosSorted(Path.Combine(Paths.GetExeDir(), "Wildcards")).Select(x => $"~~~{x.NameNoExt()}"));

                menu.Items = autoCompleteStrings.ToArray();
            }
        }

        public static string[] GetAutocompleteStrings ()
        {
            List<string> strings = new List<string>();
            strings.AddRange(IoUtils.GetFileInfosSorted(Path.Combine(Paths.GetExeDir(), "Wildcards")).Select(x => $"{x.NameNoExt()}"));
            return strings.ToArray();
        }

        public static AutocompleteMenuNS.AutocompleteMenu ShowAutocompleteMenu (TextBox textbox)
        {
            var menu = MakeAutocompleteMenu(textbox.Font);
            menu.Show(textbox, true);
            return menu;
        }

        public static AutocompleteMenuNS.AutocompleteMenu MakeAutocompleteMenu(Font font)
        {
            AutocompleteMenuNS.AutocompleteMenu menu = new AutocompleteMenuNS.AutocompleteMenu();
            menu.AllowsTabKey = true;
            menu.AppearInterval = 250;
            menu.Colors = ((AutocompleteMenuNS.Colors)(new ResourceManager(typeof(MainForm)).GetObject("promptAutocomplete.Colors")));
            menu.Font = font;
            menu.Items = GetAutocompleteStrings();
            menu.LeftPadding = 0;
            menu.MaximumSize = new Size(300, 100);
            menu.MinFragmentLength = 100;
            menu.SearchPattern = "[\\w\\.-]";
            menu.TargetControlWrapper = null;
            return menu;
        }
    }
}
