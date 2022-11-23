using Microsoft.WindowsAPICodePack.Dialogs;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Forms;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Ui.MainForm
{
    internal class FormUtils
    {
        private static StableDiffusionGui.MainForm F { get { return Program.MainForm; } }

        public static void BrowseEmbedding()
        {
            if (Program.Busy)
                return;

            var imp = (Implementation)Config.GetInt("comboxImplementation");

            if (imp == Implementation.OptimizedSd)
            {
                Logger.Log($"Not supported with your current implementation ({Strings.Implementation.Get(imp.ToString(), true)}).");
                return;
            }

            if (!string.IsNullOrWhiteSpace(MainUi.CurrentEmbeddingPath))
            {
                MainUi.CurrentEmbeddingPath = "";
            }
            else
            {
                string initDir = File.Exists(MainUi.CurrentEmbeddingPath) ? MainUi.CurrentEmbeddingPath.GetParentDirOfFile() : Path.Combine(Paths.GetExeDir(), "ExampleConcepts");

                CommonOpenFileDialog dialog = new CommonOpenFileDialog { InitialDirectory = initDir, IsFolderPicker = false };

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    if (Constants.FileExts.ValidEmbeddings.Contains(Path.GetExtension(dialog.FileName.Lower())))
                        MainUi.CurrentEmbeddingPath = dialog.FileName;
                    else
                        UiUtils.ShowMessageBox("Invalid file type.");
                }
            }

            FormControls.UpdateInitImgAndEmbeddingUi();
        }

        public static void BrowseInitImage()
        {
            if (Program.Busy)
                return;

            if (MainUi.CurrentInitImgPaths != null)
            {
                MainUi.CurrentInitImgPaths = null;
            }
            else
            {
                CommonOpenFileDialog dialog = new CommonOpenFileDialog { InitialDirectory = MainUi.CurrentInitImgPaths?[0].GetParentDirOfFile(), IsFolderPicker = false, Multiselect = true };

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    var paths = dialog.FileNames.Where(path => Constants.FileExts.ValidImages.Contains(Path.GetExtension(path).Lower()));

                    if (paths.Count() > 0)
                        MainUi.HandleDroppedFiles(paths.ToArray(), true);
                    else
                        UiUtils.ShowMessageBox(dialog.FileNames.Count() == 1 ? "Invalid file type." : "None of the selected files are valid.");
                }
            }

            FormControls.UpdateInitImgAndEmbeddingUi();
        }

        public static async Task RegenerateImageWithCurrentSettings()
        {
            if (Program.Busy)
            {
                UiUtils.ShowMessageBox("Please wait until the current process has finished.");
                return;
            }

            var prevSeedVal = F.upDownSeed.Value;
            var prevIterVal = F.upDownIterations.Value;
            F.upDownSeed.Value = ImageViewer.CurrentImageMetadata.Seed;
            F.upDownIterations.Value = 1;
            await TryRun();
            F.SetSeed((long)prevSeedVal);
            F.upDownIterations.Value = prevIterVal;
        }

        public static void TryOpenPostProcessingSettings()
        {
            var imp = (Implementation)Config.GetInt("comboxImplementation");

            if (imp == Implementation.OptimizedSd)
            {
                UiUtils.ShowMessageBox($"Post-processing is not available with your current implementation ({Strings.Implementation.Get(imp.ToString(), true)}).");
                return;
            }

            new PostProcSettingsForm().ShowDialogForm();
        }

        public static void UpdateBusyState()
        {
            if (F == null)
                return;

            FormControls.SetProgress(0);

            bool imageGen = Program.State == Program.BusyState.ImageGeneration;

            F.runBtn.Text = imageGen ? "Cancel" : "Generate!";
            F.runBtn.ForeColor = imageGen ? Color.IndianRed : Color.White;
            Control[] controlsToDisable = new Control[] { };
            Control[] controlsToHide = new Control[] { };
            F.progressCircle.Visible = Program.State != Program.BusyState.Standby;

            foreach (Control c in controlsToDisable)
                c.Enabled = !imageGen;

            foreach (Control c in controlsToHide)
                c.Visible = !imageGen;

            if (Program.State == Program.BusyState.Standby)
                F.SetProgress(0);

            if (!imageGen)
                F.SetProgressImg(0);

            F.progressBarImg.Visible = imageGen;
        }

        public static async Task TryRun ()
        {
            if (Program.Busy)
            {
                TextToImage.CancelManually();
                return;
            }

            if (MainUi.Queue.Count > 0)
            {
                F.generateAllQueuedPromptsToolStripMenuItem.Text = $"Generate Queued Prompts ({MainUi.Queue.Count})";
                F.menuStripRunQueue.Show(Cursor.Position);
            }
            else
            {
                await Run();
            }
        }

        public static async Task Run(bool fromQueue = false)
        {
            try
            {
                if (Program.Busy)
                {
                    TextToImage.Cancel();
                    return;
                }
                else
                {
                    TextToImage.Canceled = false;

                    if (!MainUi.IsInstalledWithWarning())
                        return;

                    Logger.ClearLogBox();
                    F.CleanPrompt();
                    FormControls.UpdateInitImgAndEmbeddingUi();
                    InpaintingUtils.DeleteMaskedImage();

                    if (fromQueue)
                    {
                        if (MainUi.Queue.Where(x => x != null).Count() < 0)
                        {
                            TextToImage.Cancel("Queue is empty.");
                            return;
                        }

                        await TextToImage.RunTti(MainUi.Queue.AsEnumerable().Reverse().ToList()); // Reverse list to use top entries first
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(F.textboxPrompt.Text))
                        {
                            TextToImage.Cancel("No prompt was entered.");
                            return;
                        }

                        FormControls.Save();
                        await TextToImage.RunTti(FormParsing.GetCurrentTtiSettings());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"{ex.Message}\n{ex.StackTrace}");
            }
        }

        public static void TryUseCurrentImgAsInitImg()
        {
            if (Program.Busy)
            {
                UiUtils.ShowMessageBox("Please wait until the generation has finished.");
                return;
            }

            MainUi.HandleDroppedFiles(new string[] { ImageViewer.CurrentImagePath });
        }

    }
}
