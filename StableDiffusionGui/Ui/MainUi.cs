using StableDiffusionGui.Data;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Forms;
using StableDiffusionGui.Implementations;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Properties;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static StableDiffusionGui.Main.Enums.Misc;
using static StableDiffusionGui.Main.Enums.Program;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Ui
{
    internal class MainUi
    {
        private static List<string> _currentInitImgPaths = new List<string>();
        public static List<string> CurrentInitImgPaths
        {
            get => _currentInitImgPaths;
            set
            {
                if (value == null)
                    value = new List<string>();

                _currentInitImgPaths = value;
                Image first = null;

                if (value != null && value.Count() > 0)
                {
                    Logger.Log(value.Count() == 1 ? $"Now using base image {Path.GetFileName(value[0]).Wrap()}." : $"Now using {value.Count()} base images.");
                    first = IoUtils.GetImage(value[0]);

                    if (Config.Instance.AutoSetResForInitImg)
                    {
                        SetResolutionForInitImage(first);
                    }
                }

                if (first != null && Inpainting.CurrentMask != null || Inpainting.CurrentRawMask != null)
                {
                    var size = GetResolutionForInitImage(first.Size); // Scaled mod64 size, not actual source size

                    if (Config.Instance.AlwaysClearInpaintMask || (size != Inpainting.CurrentMask.Size || size != Inpainting.CurrentRawMask.Size))
                    {
                        Inpainting.ClearMask();
                        Logger.Log("New image has a different size - Inpainting mask has been cleared.");
                    }
                }
            }
        }

        public static ConcurrentQueue<TtiSettings> Queue = new ConcurrentQueue<TtiSettings>();
        public static string GpuInfo = "";
        public static int CurrentModulo { get { return Config.Instance.Implementation == Implementation.InstructPixToPix || Config.Instance.Implementation == Implementation.Comfy ? 8 : 64; } }

        public static List<int> GetResolutions(int min, int max)
        {
            int step = CurrentModulo;

            if (Config.Instance.InvokeAllowMod8 && Config.Instance.Implementation == Implementation.InvokeAi && Program.MainForm.comboxModel.Text.Lower().Contains("inpainting"))
                step = 8;

            return Enumerable.Range(min, (max - min) + 1).Where(x => x % step == 0).ToList();
        }

        public static void DoStartupChecks()
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
                Logger.Log($"Debug mode enabled. {(System.Diagnostics.Debugger.IsAttached ? "Debugger is attached." : "")}");
            }

            if (Program.UserArgs.Get(Constants.Args.Install).GetBool() == true)
            {
                Program.MainForm.BringToFront();
                bool updDeps = Program.UserArgs.Get(Constants.Args.UpdateDeps).GetBool() == true;
                bool upscalers = Program.UserArgs.Get(Constants.Args.InstallUpscalers).GetBool() == true;
                new InstallerForm(true, updDeps, upscalers).ShowDialogForm();
            }
            else
            {
                if (!InstallationStatus.IsInstalledBasic)
                {
                    UiUtils.ShowMessageBox("No complete installation of the Stable Diffusion files was found.\n\nThe GUI will now open the installer.\nPlease press \"Install\" in the next window to install all required files.");
                    new InstallerForm().ShowDialogForm();
                }
            }

            try
            {
                string legacyModelFolder = Path.Combine(Paths.GetDataPath(), "models");
                string legacyVaeFolder = Path.Combine(legacyModelFolder, "vae");
                string legacyEmbeddingFolder = Path.Combine(legacyModelFolder, "embeddings");

                if (Directory.Exists(legacyModelFolder) && !Config.Instance.CustomModelDirs.Contains(legacyModelFolder))
                {
                    if (IoUtils.GetDirSize(legacyModelFolder, true) > 128 * 1024 * 1024)
                    {
                        Logger.LogHidden("Found old non-empty model folder, added to custom model folders so models still show up.");
                        Config.Instance.CustomModelDirs.Add(legacyModelFolder);
                    }
                    else
                    {
                        IoUtils.TryDeleteIfExists(legacyModelFolder); // Legacy folder is empty, delete it.
                    }
                }

                if (Directory.Exists(legacyVaeFolder) && !Config.Instance.CustomVaeDirs.Contains(legacyVaeFolder))
                {
                    if (IoUtils.GetDirSize(legacyVaeFolder, true) > 64 * 1024 * 1024)
                    {
                        Logger.LogHidden("Found old non-empty VAE folder, added to custom VAE folders so VAE models still show up.");
                        Config.Instance.CustomVaeDirs.Add(legacyVaeFolder);
                    }
                    else
                    {
                        IoUtils.TryDeleteIfExists(legacyVaeFolder); // Legacy folder is empty, delete it.
                    }
                }

                if (Directory.Exists(legacyEmbeddingFolder) && Config.Instance.EmbeddingsDir != legacyEmbeddingFolder)
                {
                    if (IoUtils.GetDirSize(legacyEmbeddingFolder, true) < 1024)
                    {
                        Logger.LogHidden("Found old non-empty embeddings folder, moving to new folder.");
                        string moveMsg = ", your files have been moved there automatically.";

                        try
                        {
                            var embeddings = IoUtils.GetFileInfosSorted(legacyEmbeddingFolder, false, "*.*").ToList();
                            embeddings.ForEach(f => IoUtils.MoveTo(f.FullName, Config.Instance.EmbeddingsDir));
                        }
                        catch (Exception ex)
                        {
                            Logger.LogException(ex, true, "Error moving embeddings:");
                            moveMsg = ". Moving them automatically failed (check logs for details). Try moving them manually.";
                        }

                        UiUtils.ShowMessageBox($"Found embedding files in '{legacyEmbeddingFolder}'.\nThis path was changed to {Paths.GetEmbeddingsPath()}{moveMsg}",
                            UiUtils.MessageType.Warning, Nmkoder.Forms.MessageForm.FontSize.Normal);
                    }
                    else
                    {
                        IoUtils.TryDeleteIfExists(legacyEmbeddingFolder); // Legacy folder is empty, delete it.
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            if (Config.Instance.Implementation.Supports(ImplementationInfo.Feature.CustomModels) && Models.GetModelsAll().Count <= 0)
                UiUtils.ShowMessageBox($"No model files have been found. You will not be able to generate images until you either place a model in Data/models, or set an external folder in the settings.",
                    UiUtils.MessageType.Warning, Nmkoder.Forms.MessageForm.FontSize.Normal);
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

                    if (imgForm.Action == ImageImportAction.LoadSettings || imgForm.Action == ImageImportAction.LoadImageAndSettings)
                    {
                        Program.MainForm.LoadMetadataIntoUi(imgForm.CurrentMetadata);
                    }
                    if (imgForm.Action == ImageImportAction.LoadImage || imgForm.Action == ImageImportAction.LoadImageAndSettings)
                    {
                        if (imgForm.ChromaKeyColor != (ChromaKeyColor)(-1) && imgForm.ChromaKeyColor != ChromaKeyColor.None)
                        {
                            ImageMagick.MagickColor keyColor = ImageMagick.MagickColors.Black;
                            if (imgForm.ChromaKeyColor == ChromaKeyColor.White) keyColor = ImageMagick.MagickColors.White;
                            if (imgForm.ChromaKeyColor == ChromaKeyColor.Green) keyColor = new ImageMagick.MagickColor(0, ushort.MaxValue, 0);
                            ImgUtils.ReplaceColorWithTransparency(new ImageMagick.MagickImage(paths[0]), keyColor).Write(paths[0]);
                        }

                        AddInitImages(paths.ToList());
                    }
                    if (imgForm.Action == ImageImportAction.CopyPrompt)
                    {
                        OsUtils.SetClipboard(imgForm.CurrentMetadata.Prompt);
                    }
                }
            }
            else
            {
                paths = paths.OrderBy(path => Path.GetFileName(path)).ToArray(); // Sort by filename
                var validImagesInPathList = paths.Where(path => Constants.FileExts.ValidImages.Contains(Path.GetExtension(path).Lower()));

                if (validImagesInPathList.Any())
                {
                    DialogResult dialogResult = noConfirmations ? DialogResult.Yes : UiUtils.ShowMessageBox($"Do you want to load these images as base images?", $"Dropped {paths.Length} Images", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                        AddInitImages(paths.ToList());
                }
            }
        }

        public static void AddInitImages(List<string> paths, bool silent = false, DialogResult silentReplaceInsteadOfAppendResult = DialogResult.Yes)
        {
            if (paths.Count < 1)
                return;

            if (CurrentInitImgPaths.Any())
            {
                bool oldIs1 = CurrentInitImgPaths.Count == 1;
                bool newIs1 = paths.Count == 1;

                string msg = $"Do you want to replace the currently loaded {(oldIs1 ? $"image '{Path.GetFileName(CurrentInitImgPaths[0])}'" : $"{CurrentInitImgPaths.Count} images")}?\n\n" +
                    $"Press \"No\" to append {(newIs1 ? "it" : "them")} to the list instead.";
                DialogResult dialogResult = silent ? silentReplaceInsteadOfAppendResult : UiUtils.ShowMessageBox(msg, $"Replace current image{(oldIs1 ? "" : "s")}?", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                    CurrentInitImgPaths = paths;
                else
                    CurrentInitImgPaths = CurrentInitImgPaths.Concat(paths).ToList();
            }
            else
            {
                CurrentInitImgPaths = paths;
            }

            Program.MainForm.TryRefreshUiState();
        }

        public static void HandlePaste()
        {
            if (!Clipboard.ContainsImage())
            {
                ((Action)(() =>
                {
                    string text = Clipboard.GetText();

                    if (text.Trim().Contains("//huggingface.co/"))
                        Program.MainForm.ModelDownloadPrompt(text);

                })).RunInTryCatch("HandlePaste Text Error:");
            }
            else
            {
                ((Action)(() =>
                {
                    Image clipboardImg = ClipboardUtils.GetImageFromClipboard();

                    if (clipboardImg == null)
                        return;

                    string savePath = Paths.GetClipboardPath(".png");
                    bool hasAlpha = ImgUtils.IsPartiallyTransparent(clipboardImg.AsBmp());

                    if (!hasAlpha)
                        clipboardImg = clipboardImg.AsBmp().ChangeFormat(PixelFormat.Format24bppRgb); // Ditch the alpha channel if there is no transparency

                    clipboardImg.Save(savePath, ImageFormat.Png);

                    if (!File.Exists(savePath))
                    {
                        Logger.Log($"Can't use clipboard image! It was retrieved successfully but could not be written to disk for temporary storage.");
                        return;
                    }

                    var fi = new FileInfo(savePath);
                    Logger.LogHidden($"Retrieved {clipboardImg.Size.AsString()} image from clipboard - {(hasAlpha ? "Has" : "No")} Alpha - Saved to '{fi.Name}' ({FormatUtils.Bytes(fi.Length)})");

                    HandleDroppedFiles(new string[] { savePath });
                })).RunInTryCatch("HandlePaste Image Error:");
            }
        }

        public static string GetValuesStr(List<float> numbers, bool ignoreSingleValue = true)
        {
            if (numbers == null || numbers.Count <= 0)
                return "";

            if (numbers.Count == 1 && !ignoreSingleValue)
                return numbers[0].ToStringDot("0.###");

            if (numbers.Count == 2)
                return string.Join(",", numbers.Select(f => f.ToStringDot("0.###")));

            const float tolerance = 1e-5f; // Tolerance for float comparison
            float interval = numbers[1] - numbers[0];

            for (int i = 2; i < numbers.Count; i++)
            {
                if (Math.Abs((numbers[i] - numbers[i - 1]) - interval) > tolerance)
                {
                    return string.Join(",", numbers.Select(f => f.ToStringDot("0.###")));
                }
            }

            return $"{numbers[0].ToStringDot("0.###")} > {numbers[numbers.Count - 1].ToStringDot("0.###")} : {interval.ToStringDot("0.###")}";
        }

        public static List<float> GetExtraValues(string text, float baseValue)
        {
            var values = new List<float>() { baseValue };

            if (text.MatchesWildcard("* > * : *"))
            {
                var splitMinMax = text.Trim().Split(':')[0].Split('>');
                float valFrom = splitMinMax[0].GetFloat();
                float valTo = splitMinMax[1].Trim().GetFloat();
                float step = text.Split(':').Last().GetFloat();

                List<float> incrementValues = new List<float>();

                if (valFrom < valTo)
                {
                    for (float f = valFrom; f < (valTo + 0.01f); f += step)
                        incrementValues.Add(f);
                }
                else
                {
                    for (float f = valFrom; f >= (valTo - 0.01f); f -= step)
                        incrementValues.Add(f);
                }

                if (incrementValues.Count > 0)
                    values = incrementValues;
            }
            else if (!string.IsNullOrWhiteSpace(text))
            {
                values = text.Split(",").Select(x => x.GetFloat()).ToList();
            }

            return values;
        }

        public enum PanelSizeMode { Expand, Collapse, Toggle }
        public static Dictionary<Control, int[]> PanelSizes { get; private set; } = new Dictionary<Control, int[]>();

        public static void SetPanelSize(Control panel, Button btn, PanelSizeMode sizeMode = PanelSizeMode.Toggle, int heightMultiplier = 2)
        {
            if (!PanelSizes.ContainsKey(panel))
            {
                PanelSizes.Add(panel, new int[] { panel.Height, panel.Height * heightMultiplier });
            }

            int smallHeight = PanelSizes[panel][0];
            int largeHeight = PanelSizes[panel][1];

            if (panel.Height == 0)
                return;

            ((Action)(() =>
            {
                if (sizeMode == PanelSizeMode.Toggle)
                    sizeMode = panel.Height == smallHeight ? PanelSizeMode.Expand : PanelSizeMode.Collapse;

                if (sizeMode == PanelSizeMode.Expand)
                {
                    btn.BackgroundImage = Resources.upArrowIcon;
                    panel.Height = smallHeight * 4;
                }
                else if (sizeMode == PanelSizeMode.Collapse)
                {
                    btn.BackgroundImage = Resources.downArrowIcon;
                    panel.Height = smallHeight;
                }

                Program.MainForm.panelSettings.Focus();
            })).RunWithUiStopped(Program.MainForm);
        }

        public static async Task GetCudaGpus()
        {
            GpuInfo = "";
            var gpus = await GpuUtils.GetCudaGpus();
            List<string> gpuNames = gpus.Select(x => x.FullName).ToList();
            int maxGpusToListInTitle = 2;

            if (gpuNames.Count < 1)
                GpuInfo = $"No CUDA GPUs available.";
            else if (gpuNames.Count <= maxGpusToListInTitle)
                GpuInfo = $"CUDA GPU{(gpuNames.Count != 1 ? "s" : "")}: {string.Join(", ", gpuNames)}";
            else
                GpuInfo = $"CUDA GPUs: {string.Join(", ", gpuNames.Take(maxGpusToListInTitle))} (+{gpuNames.Count - maxGpusToListInTitle})";

            Logger.Log($"Detected {gpus.Count.ToString().Replace("0", "no")} CUDA-capable GPU{(gpus.Count != 1 ? "s" : "")}.");
            Program.MainForm.UpdateWindowTitle();
        }

        public static async Task PrintVersion()
        {
            string ver = await GetWebInfo.LoadVersion();
            Logger.Log($"Latest version: {ver}");

            if (ver.Trim() != Program.Version)
                Logger.Log($"It seems like you are not running the latest version.{(Program.ReleaseChannel == UpdateChannel.Public ? $" You can download the latest using the updater (check the toolbar on the top right) or on itch: {Constants.Urls.ItchPage}" : "")}");
            else
                Logger.Log($"You are running the latest version ({Program.ReleaseChannel} Channel).");
        }

        public static Size GetPreferredSize()
        {
            Size outputImgSize = new Size();

            if (Program.MainForm.pictBoxImgViewer.GetImageSafe() == null)
            {
                if (Program.MainForm.pictBoxInitImg.GetImageSafe() == null)
                    return Size.Empty;
                else
                    outputImgSize = Program.MainForm.pictBoxInitImg.GetImageSafe().Size;
            }
            else
            {
                outputImgSize = Program.MainForm.pictBoxImgViewer.GetImageSafe().Size;
            }

            int picInWidth = Program.MainForm.tableLayoutPanelImgViewers.ColumnStyles[0].Width > 1 ? outputImgSize.Width : 0;
            int picOutWidth = outputImgSize.Width;
            int picOutHeight = outputImgSize.Height;
            int formWidthWithoutImgViewer = Program.MainForm.Size.Width - Program.MainForm.tableLayoutPanelImgViewers.Width;
            int formHeightWithoutImgViewer = Program.MainForm.Size.Height - Program.MainForm.tableLayoutPanelImgViewers.Height;

            Size targetSize = new Size(picInWidth + picOutWidth + formWidthWithoutImgViewer, picOutHeight.Clamp(512, 8192) + formHeightWithoutImgViewer);
            Size currScreenSize = Screen.FromControl(Program.MainForm).Bounds.Size;

            if (Program.MainForm.Size == targetSize)
                return Size.Empty;

            if (targetSize.Width > currScreenSize.Width || targetSize.Height > currScreenSize.Height)
                return Size.Empty;

            return targetSize;
        }

        public static void FitWindowSizeToImageSize()
        {
            ((Action)(() =>
            {
                Size targetSize = GetPreferredSize();

                if (targetSize == Size.Empty || Program.MainForm.Size == targetSize)
                    return;

                if (Program.MainForm.WindowState == FormWindowState.Maximized)
                    Program.MainForm.WindowState = FormWindowState.Normal;

                Program.MainForm.Size = targetSize;
            })).RunWithUiStopped(Program.MainForm);
        }

        public static void SetResolutionForInitImage(string initImgPath)
        {
            SetResolutionForInitImage(IoUtils.GetImage(initImgPath));
        }

        public static void SetResolutionForInitImage(Image initImg)
        {
            Size newRes = GetResolutionForInitImage(initImg.Size);
            Program.MainForm.comboxResW.Text = newRes.Width.ToString();
            Program.MainForm.comboxResH.Text = newRes.Height.ToString();
        }

        public static Size GetResolutionForInitImage(Size imageSize)
        {
            return ImgUtils.GetValidSize(imageSize, GetValidImageWidths(), GetValidImageHeights());
        }

        public static List<int> GetValidImageWidths()
        {
            return Program.MainForm.comboxResW.Items.Cast<string>().Select(x => x.GetInt()).ToList();
        }

        public static List<int> GetValidImageHeights()
        {
            return Program.MainForm.comboxResH.Items.Cast<string>().Select(x => x.GetInt()).ToList();
        }
    }
}