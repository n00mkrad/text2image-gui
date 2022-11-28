using HTAlt.WinForms;
using Microsoft.WindowsAPICodePack.Taskbar;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Forms;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Os;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Ui.MainForm
{
    internal class FormControls
    {

        private static StableDiffusionGui.MainForm F { get { return Program.MainForm; } }

        public static void InitializeControls()
        {
            F.comboxSampler.FillFromEnum<Sampler>(Strings.Samplers, 0);
            F.comboxSeamless.FillFromEnum<SeamlessMode>(Strings.SeamlessMode, 0);
            F.comboxInpaintMode.FillFromEnum<InpaintMode>(Strings.InpaintMode, 0, EnabledFeatures.DisabledInpaintModes);

            var resItems = MainUi.Resolutions.Where(x => x <= (Config.GetBool("checkboxAdvancedMode") ? 2048 : 1024)).Select(x => x.ToString());
            F.comboxResW.SetItems(resItems, UiExtensions.SelectMode.Last);
            F.comboxResH.SetItems(resItems, UiExtensions.SelectMode.Last);
        }

        public static void Load()
        {
            ConfigParser.LoadGuiElement(F.upDownIterations);
            ConfigParser.LoadGuiElement(F.sliderSteps);
            ConfigParser.LoadGuiElement(F.sliderScale);
            ConfigParser.LoadGuiElement(F.comboxResH);
            ConfigParser.LoadGuiElement(F.comboxResW);
            ConfigParser.LoadComboxIndex(F.comboxSampler);
            ConfigParser.LoadGuiElement(F.sliderInitStrength);
        }

        public static void Save()
        {
            ConfigParser.SaveGuiElement(F.upDownIterations);
            ConfigParser.SaveGuiElement(F.sliderSteps);
            ConfigParser.SaveGuiElement(F.sliderScale);
            ConfigParser.SaveGuiElement(F.comboxResH);
            ConfigParser.SaveGuiElement(F.comboxResW);
            ConfigParser.SaveComboxIndex(F.comboxSampler);
            ConfigParser.SaveGuiElement(F.sliderInitStrength);
        }

        public static void RefreshUiAfterSettingsChanged()
        {
            var imp = (Implementation)Config.GetInt("comboxImplementation");
            F.panelPromptNeg.Visible = imp != Implementation.OptimizedSd;
            F.btnEmbeddingBrowse.Enabled = imp == Implementation.InvokeAi;
            F.panelSampler.Visible = imp == Implementation.InvokeAi;
            F.panelSeamless.Visible = imp == Implementation.InvokeAi;

            bool adv = Config.GetBool("checkboxAdvancedMode");
            F.upDownIterations.Maximum = !adv ? 10000 : 100000;
            F.sliderSteps.ActualMaximum = !adv ? 120 : 500;
            F.sliderSteps.ValueStep = !adv ? 5 : 1;
            F.sliderScale.ActualMaximum = !adv ? 25 : 50;
            F.comboxResW.SetItems(MainUi.Resolutions.Where(x => x <= (adv ? 2048 : 1024)).Select(x => x.ToString()), UiExtensions.SelectMode.Retain, UiExtensions.SelectMode.Last);
            F.comboxResH.SetItems(MainUi.Resolutions.Where(x => x <= (adv ? 2048 : 1024)).Select(x => x.ToString()), UiExtensions.SelectMode.Retain, UiExtensions.SelectMode.Last);
        }

        public static void OpenLogsMenu ()
        {
            F.menuStripLogs.Items.Clear();
            var openLogs = F.menuStripLogs.Items.Add($"Open Logs Folder");
            openLogs.Click += (s, ea) => { Process.Start("explorer", Paths.GetLogPath().Wrap()); };

            foreach (var log in Logger.CachedEntries)
            {
                ToolStripItem newItem = F.menuStripLogs.Items.Add($"Copy {log.Key}");
                newItem.Click += (s, ea) => { OsUtils.SetClipboard(Logger.EntriesToString(Logger.CachedEntries[log.Key], true, true)); };
            }

            F.menuStripLogs.Show(Cursor.Position);
        }

        public static void UpdateInitImgAndEmbeddingUi()
        {
            TtiUtils.CleanInitImageList();

            if (!string.IsNullOrWhiteSpace(MainUi.CurrentEmbeddingPath) && !File.Exists(MainUi.CurrentEmbeddingPath))
            {
                MainUi.CurrentEmbeddingPath = "";
                Logger.Log($"Concept was cleared because the file no longer exists.");
            }

            bool inpaintingModel = Path.ChangeExtension(Config.Get("comboxSdModel"), null).EndsWith("-inpainting");
            bool img2img = MainUi.CurrentInitImgPaths != null;
            F.panelInpainting.Visible = img2img;
            F.panelInitImgStrength.Visible = img2img && !inpaintingModel;
            F.btnInitImgBrowse.Text = img2img ? $"Clear Image{(MainUi.CurrentInitImgPaths.Count == 1 ? "" : "s")}" : "Load Image(s)";

            bool embeddingExists = File.Exists(MainUi.CurrentEmbeddingPath);
            F.btnEmbeddingBrowse.Text = embeddingExists ? "Clear Concept" : "Load Concept";

            F.labelCurrentImage.Text = !img2img ? "No initialization image loaded." : (MainUi.CurrentInitImgPaths.Count == 1 ? $"Currently using {Path.GetFileName(MainUi.CurrentInitImgPaths[0]).Trunc(30)}" : $"Currently using {MainUi.CurrentInitImgPaths.Count} images.");
            F.labelCurrentConcept.Text = string.IsNullOrWhiteSpace(MainUi.CurrentEmbeddingPath) ? "No trained concept loaded." : $"Currently using {Path.GetFileName(MainUi.CurrentEmbeddingPath).Trunc(30)}";

            RefreshUiAfterSettingsChanged();
        }

        public static void HandleImageViewerClick (bool rightClick)
        {
            F.pictBoxImgViewer.Focus();

            if (rightClick)
            {
                if (!string.IsNullOrWhiteSpace(ImageViewer.CurrentImagePath) && File.Exists(ImageViewer.CurrentImagePath))
                {
                    F.reGenerateImageWithCurrentSettingsToolStripMenuItem.Visible = !Program.Busy;
                    F.useAsInitImageToolStripMenuItem.Visible = !Program.Busy;
                    F.postProcessImageToolStripMenuItem.Visible = !Program.Busy && TextToImage.CurrentTaskSettings.Implementation == Implementation.InvokeAi;
                    F.menuStripOutputImg.Show(Cursor.Position);
                }
            }
            else
            {
                if (F.pictBoxImgViewer.Image != null)
                    ImagePopup.Show(F.pictBoxImgViewer.Image, ImagePopupForm.SizeMode.Percent100);
            }
        }

        public static void SetProgress(int percent, bool taskbarProgress = false, HTProgressBar progressBar = null)
        {
            if (progressBar == null)
                progressBar = F.progressBar;

            percent = percent.Clamp(0, 100);
            progressBar.Value = percent;
            progressBar.Refresh();

            if (taskbarProgress)
            {
                try
                {
                    TaskbarManager.Instance.SetProgressValue(percent, 100);
                }
                catch (Exception ex)
                {
                    Logger.Log($"Failed to set taskbar progress: {ex.Message}", true);
                }
            } 
        }

        public static void SetHiresFixVisible()
        {
            bool txt2img = MainUi.CurrentInitImgPaths == null;
            bool compatible = (Implementation)Config.GetInt("comboxImplementation") == Implementation.InvokeAi;
            F.checkboxHiresFix.Visible = F.comboxResW.GetInt() > 512 && F.comboxResH.GetInt() > 512 && txt2img && compatible;
        }
    }
}
