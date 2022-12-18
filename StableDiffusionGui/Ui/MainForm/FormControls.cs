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
        private static Implementation CurrImpl { get { return (Implementation)Config.Get<int>(Config.Keys.ImplementationIdx); } }
        private static bool IsUsingInpaintingModel { get { return Path.ChangeExtension(Config.Get<string>(Config.Keys.Model), null).EndsWith("-inpainting"); } }

        public static void InitializeControls()
        {
            F.comboxSampler.FillFromEnum<Sampler>(Strings.Samplers, 0);
            F.comboxSeamless.FillFromEnum<SeamlessMode>(Strings.SeamlessMode, 0);
            F.comboxInpaintMode.FillFromEnum<InpaintMode>(Strings.InpaintMode, 0);

            var resItems = MainUi.Resolutions.Where(x => x <= (Config.Get<bool>(Config.Keys.AdvancedUi) ? 2048 : 1024)).Select(x => x.ToString());
            F.comboxResW.SetItems(resItems, UiExtensions.SelectMode.Last);
            F.comboxResH.SetItems(resItems, UiExtensions.SelectMode.Last);
        }

        public static void Load()
        {
            ConfigParser.LoadGuiElement(F.upDownIterations, Config.Keys.Iterations);
            ConfigParser.LoadGuiElement(F.sliderSteps, Config.Keys.Steps);
            ConfigParser.LoadGuiElement(F.sliderScale, Config.Keys.Scale);
            ConfigParser.LoadGuiElement(F.comboxResW, Config.Keys.ResW);
            ConfigParser.LoadGuiElement(F.comboxResH, Config.Keys.ResH);
            ConfigParser.LoadComboxIndex(F.comboxSampler, Config.Keys.Sampler);
            ConfigParser.LoadGuiElement(F.sliderInitStrength, Config.Keys.InitStrength);
        }

        public static void Save()
        {
            ConfigParser.SaveGuiElement(F.upDownIterations, Config.Keys.Iterations);
            ConfigParser.SaveGuiElement(F.sliderSteps, Config.Keys.Steps);
            ConfigParser.SaveGuiElement(F.sliderScale, Config.Keys.Scale);
            ConfigParser.SaveGuiElement(F.comboxResW, Config.Keys.ResW);
            ConfigParser.SaveGuiElement(F.comboxResH, Config.Keys.ResH);
            ConfigParser.SaveComboxIndex(F.comboxSampler, Config.Keys.Sampler);
            ConfigParser.SaveGuiElement(F.sliderInitStrength, Config.Keys.InitStrength);
        }

        public static void RefreshUiAfterSettingsChanged()
        {
            F.panelPromptNeg.Visible = CurrImpl != Implementation.OptimizedSd && !IsUsingInpaintingModel;
            F.btnEmbeddingBrowse.Enabled = CurrImpl == Implementation.InvokeAi;
            F.panelSampler.Visible = CurrImpl == Implementation.InvokeAi;
            F.panelSeamless.Visible = CurrImpl == Implementation.InvokeAi;

            bool adv = Config.Get<bool>(Config.Keys.AdvancedUi);
            F.upDownIterations.Maximum = !adv ? 10000 : 100000;
            F.sliderSteps.ActualMaximum = !adv ? 120 : 500;
            F.sliderSteps.ValueStep = !adv ? 5 : 1;
            F.sliderScale.ActualMaximum = !adv ? 25 : 50;
            F.comboxResW.SetItems(MainUi.Resolutions.Where(x => x <= (adv ? 2048 : 1024)).Select(x => x.ToString()), UiExtensions.SelectMode.Retain, UiExtensions.SelectMode.Last);
            F.comboxResH.SetItems(MainUi.Resolutions.Where(x => x <= (adv ? 2048 : 1024)).Select(x => x.ToString()), UiExtensions.SelectMode.Retain, UiExtensions.SelectMode.Last);
        }

        public static void OpenLogsMenu()
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

            bool img2img = MainUi.CurrentInitImgPaths != null;
            bool inpaintingCompatibleImpl = CurrImpl == Implementation.InvokeAi || CurrImpl == Implementation.DiffusersOnnx;

            F.panelInpainting.Visible = img2img && inpaintingCompatibleImpl;
            F.panelInitImgStrength.Visible = img2img && !IsUsingInpaintingModel;
            F.textboxClipsegMask.Visible = (InpaintMode)F.comboxInpaintMode.SelectedIndex == InpaintMode.TextMask;

            F.btnInitImgBrowse.Text = img2img ? $"Clear Image{(MainUi.CurrentInitImgPaths.Count == 1 ? "" : "s")}" : "Load Image(s)";

            bool embeddingExists = File.Exists(MainUi.CurrentEmbeddingPath);
            F.btnEmbeddingBrowse.Text = embeddingExists ? "Clear Concept" : "Load Concept";

            F.labelCurrentImage.Text = !img2img ? "No initialization image loaded." : (MainUi.CurrentInitImgPaths.Count == 1 ? $"Currently using {Path.GetFileName(MainUi.CurrentInitImgPaths[0]).Trunc(30)}" : $"Currently using {MainUi.CurrentInitImgPaths.Count} images.");
            F.labelCurrentConcept.Text = string.IsNullOrWhiteSpace(MainUi.CurrentEmbeddingPath) ? "No trained concept loaded." : $"Currently using {Path.GetFileName(MainUi.CurrentEmbeddingPath).Trunc(30)}";

            RefreshUiAfterSettingsChanged();
        }

        public static void HandleImageViewerClick(bool rightClick)
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
            bool compatible = CurrImpl == Implementation.InvokeAi;
            F.checkboxHiresFix.Visible = F.comboxResW.GetInt() > 512 && F.comboxResH.GetInt() > 512 && txt2img && compatible;
        }
    }
}
