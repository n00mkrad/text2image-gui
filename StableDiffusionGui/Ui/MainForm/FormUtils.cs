﻿using ImageMagick;
using Microsoft.WindowsAPICodePack.Dialogs;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Forms;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static StableDiffusionGui.Main.Enums.StableDiffusion;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using Paths = StableDiffusionGui.Io.Paths;

namespace StableDiffusionGui.Ui.MainFormUtils
{
    internal class FormUtils
    {
        private static StableDiffusionGui.MainForm F { get { return Program.MainForm; } }

        public static void BrowseEmbedding()
        {
            if (Program.Busy)
                return;

            var imp = ConfigParser.CurrentImplementation;

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

            FormControls.RefreshUiAfterSettingsChanged();
        }

        public static void BrowseInitImage()
        {
            if (Program.Busy)
                return;

            if (MainUi.CurrentInitImgPaths != null)
            {
                Program.MainForm.pictBoxInitImg.BackgroundImage = null;
                MainUi.CurrentInitImgPaths = null;
                Program.MainForm.panelTest.Size = new Size(Program.MainForm.panelTest.Size.Width, Program.MainForm.panelTest.Size.Height / 2);

                Program.MainForm.sliderInitStrength.Visible = false;
                Program.MainForm.textboxSliderInitStrength.Visible = false;
                Program.MainForm.pictBoxInitImg.Visible = false;
                Program.MainForm.label11.Visible = false;

                Program.MainForm.cbBaW.Location = new Point(Program.MainForm.cbBaW.Location.X - Program.MainForm.pictBoxInitImg.Size.Width, Program.MainForm.cbBaW.Location.Y);
                Program.MainForm.cbDetFace.Location = new Point(Program.MainForm.cbDetFace.Location.X - Program.MainForm.pictBoxInitImg.Size.Width, Program.MainForm.cbDetFace.Location.Y);
                Program.MainForm.cbSepia.Location = new Point(Program.MainForm.cbSepia.Location.X - Program.MainForm.pictBoxInitImg.Size.Width, Program.MainForm.cbSepia.Location.Y);
                Program.MainForm.checkboxHiresFix.Location = new Point(Program.MainForm.checkboxHiresFix.Location.X - Program.MainForm.pictBoxInitImg.Size.Width, Program.MainForm.checkboxHiresFix.Location.Y);
            }
            else
            {
                CommonOpenFileDialog dialog = new CommonOpenFileDialog { InitialDirectory = MainUi.CurrentInitImgPaths?[0].GetParentDirOfFile(), IsFolderPicker = false, Multiselect = true };

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    var paths = dialog.FileNames.Where(path => Constants.FileExts.ValidImages.Contains(Path.GetExtension(path).Lower()));

                    Program.MainForm.pictBoxInitImg.BackgroundImage = new Bitmap(dialog.FileName);

                    Program.MainForm.panelTest.Size = new Size(Program.MainForm.panelTest.Size.Width, Program.MainForm.panelTest.Size.Height * 2);
                    Program.MainForm.sliderInitStrength.Visible = true;
                    Program.MainForm.textboxSliderInitStrength.Visible = true;
                    Program.MainForm.pictBoxInitImg.Visible = true;
                    Program.MainForm.label11.Visible = true;

                    Program.MainForm.cbBaW.Location = new Point(Program.MainForm.cbBaW.Location.X + Program.MainForm.pictBoxInitImg.Size.Width, Program.MainForm.cbBaW.Location.Y);
                    Program.MainForm.cbDetFace.Location = new Point(Program.MainForm.cbDetFace.Location.X + Program.MainForm.pictBoxInitImg.Size.Width, Program.MainForm.cbDetFace.Location.Y);
                    Program.MainForm.cbSepia.Location = new Point(Program.MainForm.cbSepia.Location.X + Program.MainForm.pictBoxInitImg.Size.Width, Program.MainForm.cbSepia.Location.Y);
                    Program.MainForm.checkboxHiresFix.Location = new Point(Program.MainForm.checkboxHiresFix.Location.X + Program.MainForm.pictBoxInitImg.Size.Width, Program.MainForm.checkboxHiresFix.Location.Y);

                    if (paths.Count() > 0)
                        MainUi.HandleDroppedFiles(paths.ToArray(), true);
                    else
                        UiUtils.ShowMessageBox(dialog.FileNames.Count() == 1 ? "Invalid file type." : "None of the selected files are valid.");
                }
            }

            FormControls.RefreshUiAfterSettingsChanged();
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

        public static async Task TryRun()
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
                    TextToImage.Cancel($"Program is already busy ({Program.State})", false);
                    return;
                }
                else
                {
                    TextToImage.Canceled = false;

                    if (!MainUi.IsInstalledWithWarning())
                        return;

                    Logger.ClearLogBox();
                    F.CleanPrompt();
                    FormControls.RefreshUiAfterSettingsChanged();
                    Inpainting.DeleteMaskedImage();

                    if (fromQueue)
                    {
                        if (MainUi.Queue.Where(x => x != null).Count() < 0)
                        {
                            TextToImage.Cancel("Queue is empty.", true);
                            return;
                        }

                        await TextToImage.RunTti(MainUi.Queue);
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(F.textboxPrompt.Text))
                        {
                            TextToImage.Cancel("No prompt was entered.", true);
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

        public static void TryUseCurrentImgAsInitImg(bool ignoreBusy = false)
        {
            if (Program.Busy && !ignoreBusy)
            {
                UiUtils.ShowMessageBox("Please wait until the generation has finished.");
                return;
            }

            MainUi.AddInitImages(new[] { ImageViewer.CurrentImagePath }.ToList(), true);
        }

        public static void EditMask()
        {
            if (MainUi.CurrentInitImgPaths == null || MainUi.CurrentInitImgPaths.Count < 1)
                return;

            Image img = IoUtils.GetImage(MainUi.CurrentInitImgPaths[0], false);

            if (img == null)
                return;

            Size targetSize = TextToImage.CurrentTaskSettings.Params["res"].FromJson<Size>();
            Size scaleSize = Config.Get<bool>(Config.Keys.InitImageRetainAspectRatio) ? ImgMaths.FitIntoFrame(img.Size, targetSize) : targetSize;
            img = ImgUtils.ScaleAndPad(ImgUtils.GetMagickImage(img), scaleSize, targetSize).ToBitmap();

            Inpainting.EditCurrentMask(img, MainFormUtils.FormControls.IsUsingInpaintingModel);
        }
    }
}
