using ImageMagick;
using Microsoft.WindowsAPICodePack.Dialogs;
using StableDiffusionGui.Data;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Main.Utils;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static StableDiffusionGui.Main.Constants;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Forms
{
    public partial class MainForm
    {
        public void BrowseInitImage()
        {
            if (Program.Busy)
                return;

            if (MainUi.CurrentInitImgPaths.Any())
            {
                MainUi.CurrentInitImgPaths.Clear();
            }
            else
            {
                if (!Directory.Exists(Config.Instance.LastInitImageParentPath))
                    Config.Instance.LastInitImageParentPath = MainUi.CurrentInitImgPaths.Any() ? MainUi.CurrentInitImgPaths[0].GetParentDirOfFile() : Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                CommonOpenFileDialog dialog = new CommonOpenFileDialog { RestoreDirectory = false, InitialDirectory = Config.Instance.LastInitImageParentPath, IsFolderPicker = false, Multiselect = true };

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    var paths = dialog.FileNames.Where(path => Constants.FileExts.ValidImages.Contains(Path.GetExtension(path).Lower()));

                    if (paths.Count() > 0)
                    {
                        MainUi.HandleDroppedFiles(paths.ToArray(), true);
                        Config.Instance.LastInitImageParentPath = new FileInfo(paths.First()).Directory.FullName;
                    }
                    else
                    {
                        UiUtils.ShowMessageBox(dialog.FileNames.Count() == 1 ? "Invalid file type." : "None of the selected files are valid.");
                    }
                }
            }

            TryRefreshUiState();
        }

        public async Task RegenerateImageWithCurrentSettings()
        {
            if (Program.Busy)
            {
                UiUtils.ShowMessageBox("Please wait until the current process has finished.");
                return;
            }

            var prevSeedVal = upDownSeed.Value;
            var prevIterVal = upDownIterations.Value;
            upDownSeed.Value = ImageViewer.CurrentImageMetadata.Seed;
            upDownIterations.Value = 1;
            await TryRun();
            SetSeed((long)prevSeedVal);
            upDownIterations.Value = prevIterVal;
        }

        public void TryOpenPostProcessingSettings()
        {
            var imp = Config.Instance.Implementation;
            var supportedImps = new List<Implementation> { Implementation.InvokeAi, Implementation.Comfy };

            if (!supportedImps.Contains(imp))
            {
                UiUtils.ShowMessageBox($"Post-processing is not available with your current implementation: {Strings.Implementation.Get(imp.ToString(), true)}.");
                return;
            }

            new PostProcSettingsForm().ShowDialogForm();
        }

        public void UpdateBusyState()
        {
            SetProgress(0);

            bool imageGen = Program.State == Program.BusyState.ImageGeneration;
            UpdateRunBtnState(imageGen);
            Control[] controlsToDisable = new Control[] { };
            Control[] controlsToHide = new Control[] { };
            progressCircle.SetVisible(Program.State != Program.BusyState.Standby);

            foreach (Control c in controlsToDisable)
                c.Enabled = !imageGen;

            foreach (Control c in controlsToHide)
                c.SetVisible(!imageGen);

            if (Program.State == Program.BusyState.Standby)
                SetProgress(0);

            if (!imageGen)
                SetProgressImg(0);

            progressBarImg.SetVisible(imageGen);
            UpdateWindowTitle();
        }

        public async Task TryRun()
        {
            if (Program.Busy)
            {
                TextToImage.CancelManually();
                return;
            }

            if (MainUi.Queue.Count > 0)
            {
                generateAllQueuedPromptsToolStripMenuItem.Text = $"Generate Queued Prompts ({MainUi.Queue.Count})";
                menuStripRunQueue.Show(Cursor.Position);
            }
            else
            {
                await Run();
            }
        }

        public async Task Run(bool fromQueue = false)
        {
            try
            {
                if (Program.Busy)
                {
                    TextToImage.Cancel($"Program is already busy ({Program.State})", false);
                    return;
                }
                else
                {
                    TextToImage.Canceled = false;

                    if (!MainUi.IsInstalledWithWarning())
                        return;

                    Logger.ClearLogBox();
                    CleanPrompt();
                    TryRefreshUiState();
                    Inpainting.DeleteMaskedImage();

                    if (fromQueue)
                    {
                        if (MainUi.Queue.Where(x => x != null).Count() < 0)
                        {
                            TextToImage.Cancel("Queue is empty.", true);
                            return;
                        }

                        await TextToImage.RunTti();
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(textboxPrompt.Text))
                        {
                            TextToImage.Cancel("No prompt was entered.", true);
                            return;
                        }

                        SaveControls();
                        await TextToImage.RunTti(GetCurrentTtiSettings());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"{ex.Message}\n{ex.StackTrace}");
            }
        }

        public void TryUseCurrentImgAsInitImg(bool ignoreBusy = false)
        {
            if (Program.Busy && !ignoreBusy)
            {
                UiUtils.ShowMessageBox("Please wait until the generation has finished.");
                return;
            }

            MainUi.AddInitImages(ImageViewer.CurrentImagePath.AsList(), true);
        }

        public void EditMask()
        {
            if (!MainUi.CurrentInitImgPaths.Any())
                return;

            Image img = IoUtils.GetImage(MainUi.CurrentInitImgPaths[0], false);

            if (img == null)
                return;

            Size targetSize = TextToImage.CurrentTaskSettings.Res;
            Size scaleSize = Config.Instance.InitImageRetainAspectRatio ? ImgMaths.FitIntoFrame(img.Size, targetSize) : targetSize;
            img = ImgUtils.ScaleAndPad(ImgUtils.GetMagickImage(img), scaleSize, targetSize).ToBitmap();

            Inpainting.EditCurrentMask(img, IsUsingInpaintingModel);
        }

        public void ModelDownloadPrompt(string text = "")
        {
            var form = new PromptForm("Enter Model ID or URL", "Enter a model repository ID or URL. \n(E.g. \"runwayml/stable-diffusion-v1-5\").", text, 1.4f, 1f);
            form.ShowDialog();

            if (form.DialogResult == DialogResult.OK)
                DownloadModels.DownloadModel(form.EnteredText.Trim());
        }

        public void ShowNotification(string title, string text, bool onlyIfWindowIsInBackground = false, float timeout = 0f)
        {
            if (this.RequiresInvoke(new Action<string, string, bool, float>(ShowNotification), title, text, onlyIfWindowIsInBackground, timeout))
                return;

            if (onlyIfWindowIsInBackground && IsInFocus())
                return;

            int timeoutMs = (timeout * 1000f).RoundToInt();

            if (timeout <= 0f)
                timeoutMs = 250 + title.CalculateReadTimeMs() + text.CalculateReadTimeMs();

            var popup = new Tulpep.NotificationWindow.PopupNotifier
            {
                TitleText = title,
                ContentText = text,
                IsRightToLeft = false,
                BodyColor = ColorTranslator.FromHtml("#323232"),
                ContentColor = Color.White,
                TitleColor = Color.LightGray,
                GradientPower = 0,
                AnimationDuration = 200,
                Delay = timeoutMs,
                AnimationInterval = 5,
            };

            popup.TitleFont = popup.TitleFont.ChangeSize(popup.TitleFont.Size * 1.3f);
            popup.ContentFont = popup.ContentFont.ChangeSize(popup.ContentFont.Size * 1.2f);
            popup.Popup();
        }

        public List<Model> ValidateLoraNames(List<Model> loras)
        {
            Regex validPattern = new Regex(@"^[\w]+$"); // Alphanumeric, underscores
            Regex replacePattern = new Regex(@"[^\w]"); // Replace invalid chars
            Regex multiUnderscoresPattern = new Regex(@"_{2,}"); // For detecting multiple consecutive underscores

            var invalidLorasAndCorrectedNames = new EasyDict<Model, string>();

            if (loras.All(l => validPattern.IsMatch(l.FormatIndependentName)))
                return loras; // All are valid.

            foreach (var lora in loras.Where(l => !validPattern.IsMatch(l.FormatIndependentName)))
            {
                string validFilename = replacePattern.Replace(lora.FormatIndependentName, "_"); // Remove invalid chars
                invalidLorasAndCorrectedNames[lora] = multiUnderscoresPattern.Replace(validFilename, "_").Trim('_'); // Remove multiple consecutive underscores and trim start/end
            }

            UiUtils.ShowMessageBox($"The following LoRA files can't be loaded because they have an invalid file name:\n\n{string.Join("\n", invalidLorasAndCorrectedNames.Keys)}\n\n" +
                    $"Only alphanumeric characters and underscores are allowed. Spaces, hyphens and other special characters are not valid.", UiUtils.MessageType.Warning, Nmkoder.Forms.MessageForm.FontSize.Big);

            string msg = $"Do you want to automatically rename the files to a valid name?\n\n{string.Join("\n", invalidLorasAndCorrectedNames.Select(pair => $"{pair.Key.FormatIndependentName} => {pair.Value}"))}";
            DialogResult dialogResult = UiUtils.ShowMessageBox(msg, "Auto-Rename", MessageBoxButtons.YesNo, Nmkoder.Forms.MessageForm.FontSize.Big);

            if (dialogResult == DialogResult.Yes)
            {

                foreach (var pair in invalidLorasAndCorrectedNames)
                {
                    string newPath = IoUtils.GetAvailablePath(Path.Combine(pair.Key.Directory.FullName, $"{pair.Value}.safetensors"), "_{0}");
                    IoUtils.RenameFile(pair.Key.FullName, Path.GetFileNameWithoutExtension(newPath), showLog: true);
                }

                loras = Models.GetLoras(); // Need to reload from disk after renaming, otherwise we still have the old filenames
            }

            return loras.Where(l => validPattern.IsMatch(l.FormatIndependentName)).ToList();
        }

        public List<Model> ValidateEmbeddingNames(List<Model> embeddings)
        {
            Regex validPattern = new Regex(@"^[\w.-]+$"); // Alphanumeric, underscores, hyphens, dots
            Regex replacePattern = new Regex(@"[^\w]"); // Replace invalid chars
            Regex multiUnderscoresPattern = new Regex(@"_{2,}"); // For detecting multiple consecutive underscores

            var invalidEmbeddingsAndCorrectedNames = new EasyDict<Model, string>();

            if (embeddings.All(l => validPattern.IsMatch(l.FormatIndependentName)))
                return embeddings; // All are valid.

            foreach (var embedding in embeddings.Where(l => !validPattern.IsMatch(l.FormatIndependentName)))
            {
                string validFilename = replacePattern.Replace(embedding.FormatIndependentName, "_"); // Remove invalid chars
                invalidEmbeddingsAndCorrectedNames[embedding] = multiUnderscoresPattern.Replace(validFilename, "_").Trim('_'); // Remove multiple consecutive underscores and trim start/end
            }

            UiUtils.ShowMessageBox($"The following embedding files can't be loaded because they have an invalid file name:\n\n{string.Join("\n", invalidEmbeddingsAndCorrectedNames.Keys)}\n\n" +
                    $"Only alphanumeric characters, underscores, dots, and hypens are allowed. Spaces and other special characters are not valid.", UiUtils.MessageType.Warning, Nmkoder.Forms.MessageForm.FontSize.Big);

            string msg = $"Do you want to automatically rename the files to a valid name?\n\n{string.Join("\n", invalidEmbeddingsAndCorrectedNames.Select(pair => $"{pair.Key.FormatIndependentName} => {pair.Value}"))}";
            DialogResult dialogResult = UiUtils.ShowMessageBox(msg, "Auto-Rename", MessageBoxButtons.YesNo, Nmkoder.Forms.MessageForm.FontSize.Big);

            if (dialogResult == DialogResult.Yes)
            {
                invalidEmbeddingsAndCorrectedNames.ToList().ForEach(pair => IoUtils.RenameFile(pair.Key.FullName, pair.Value, showLog: true));
                embeddings = Models.GetEmbeddings(); // Need to reload from disk after renaming, otherwise we still have the old filenames
            }

            return embeddings.Where(l => validPattern.IsMatch(l.FormatIndependentName)).ToList();
        }
    }
}
