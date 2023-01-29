using HTAlt.WinForms;
using Microsoft.WindowsAPICodePack.Taskbar;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Forms;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Ui.MainFormUtils
{
    internal class FormControls
    {

        public static MainForm F { get { return Program.MainForm; } }
        public static bool IsUsingInpaintingModel { get { return Path.ChangeExtension(Config.Get<string>(Config.Keys.Model), null).EndsWith(Constants.SuffixesPrefixes.InpaintingMdlSuf); } }

        public static void InitializeControls()
        {
            F.comboxSampler.FillFromEnum<Sampler>(Strings.Samplers, 0);
            F.comboxSeamless.FillFromEnum<SeamlessMode>(Strings.SeamlessMode, 0);
            F.comboxInpaintMode.FillFromEnum<InpaintMode>(Strings.InpaintMode, 0);
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
            ConfigParser.LoadGuiElement(F.checkboxHiresFix, Config.Keys.HiresFix);
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
            ConfigParser.SaveGuiElement(F.checkboxHiresFix, Config.Keys.HiresFix);
        }

        public static void RefreshUiAfterSettingsChanged()
        {
            F.panelPromptNeg.Visible = ConfigParser.CurrentImplementation != Implementation.OptimizedSd && !IsUsingInpaintingModel;
            F.btnEmbeddingBrowse.Visible = MainForm.ShouldControlBeVisible(F, F.btnEmbeddingBrowse);
            F.panelAiInputs.Height = MainForm.ShouldControlBeVisible(F, F.btnEmbeddingBrowse) ? 65 : 35;
            F.panelSampler.Visible = ConfigParser.CurrentImplementation == Implementation.InvokeAi;
            F.panelSeamless.Visible = ConfigParser.CurrentImplementation == Implementation.InvokeAi;
            F.panelRes.Visible = MainForm.ShouldControlBeVisible(F, F.panelRes);
            F.panelScaleImg.Visible = MainForm.ShouldControlBeVisible(F, F.panelScaleImg);

            bool adv = Config.Get<bool>(Config.Keys.AdvancedUi);
            F.upDownIterations.Maximum = !adv ? 10000 : 100000;
            F.sliderSteps.ActualMaximum = !adv ? 120 : 500;
            F.sliderSteps.ChangeStep(!adv ? 5 : 1);
            F.sliderScale.ActualMaximum = !adv ? 25 : 50;
            F.comboxResW.SetItems(MainUi.GetResolutions(320, adv ? 2048 : 1280).Select(x => x.ToString()), UiExtensions.SelectMode.Retain, UiExtensions.SelectMode.Last);
            F.comboxResH.SetItems(MainUi.GetResolutions(320, adv ? 2048 : 1280).Select(x => x.ToString()), UiExtensions.SelectMode.Retain, UiExtensions.SelectMode.Last);

            if (!TtiUtils.CurrentSdModelExists())
                Config.Set(Config.Keys.Model, "");

            #region Init Img & Embeddings Stuff

            TtiUtils.CleanInitImageList();

            if (!string.IsNullOrWhiteSpace(MainUi.CurrentEmbeddingPath) && !File.Exists(MainUi.CurrentEmbeddingPath))
            {
                MainUi.CurrentEmbeddingPath = "";
                Logger.Log($"Concept was cleared because the file no longer exists.");
            }

            bool img2img = MainUi.CurrentInitImgPaths != null;

            F.panelInpainting.Visible = MainForm.ShouldControlBeVisible(F, F.panelInpainting);
            F.panelInitImgStrength.Visible = MainForm.ShouldControlBeVisible(F, F.panelInitImgStrength);
            F.textboxClipsegMask.Visible = (InpaintMode)F.comboxInpaintMode.SelectedIndex == InpaintMode.TextMask;

            F.btnInitImgBrowse.Text = img2img ? $"Clear Image{(MainUi.CurrentInitImgPaths.Count == 1 ? "" : "s")}" : "Load Image(s)";

            bool embeddingExists = File.Exists(MainUi.CurrentEmbeddingPath);
            F.btnEmbeddingBrowse.Text = embeddingExists ? "Clear Concept" : "Load Concept";

            F.labelCurrentImage.Text = !img2img ? "No initialization image loaded." : (MainUi.CurrentInitImgPaths.Count == 1 ? $"Currently using {Path.GetFileName(MainUi.CurrentInitImgPaths[0])}" : $"Currently using {MainUi.CurrentInitImgPaths.Count} images.");
            
            if (!string.IsNullOrWhiteSpace(MainUi.CurrentEmbeddingPath))
            {
                F.labelCurrentImage.Text += $"Currently trained concept using {Path.GetFileName(MainUi.CurrentEmbeddingPath)}";
            }
            
            F.toolTip.SetToolTip(F.labelCurrentImage, $"{F.labelCurrentImage.Text.Trunc(100)}\n\nShift + Hover to preview.");

            #endregion
        }

        public static void OpenLogsMenu()
        {
            var first = F.menuStripLogs.Items.Cast<ToolStripMenuItem>().First();
            F.menuStripLogs.Items.Clear();
            F.menuStripLogs.Items.Add(first);
            var openLogs = F.menuStripLogs.Items.Add($"Open Logs Folder");
            openLogs.Click += (s, ea) => { Process.Start("explorer", Paths.GetLogPath().Wrap()); };

            foreach (var log in Logger.CachedEntries)
            {
                ToolStripItem newItem = F.menuStripLogs.Items.Add($"Copy {log.Key}");
                newItem.Click += (s, ea) => { OsUtils.SetClipboard(Logger.EntriesToString(Logger.CachedEntries[log.Key], true, true)); };
            }

            F.menuStripLogs.Show(Cursor.Position);
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
                    F.copyImageToClipboardToolStripMenuItem.Visible = F.pictBoxInitImg.Image != null;
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
            bool compatible = ConfigParser.CurrentImplementation == Implementation.InvokeAi;
            F.checkboxHiresFix.Visible = (F.comboxResW.GetInt() > 512 || F.comboxResH.GetInt() > 512) && txt2img && compatible;
        }
    }
}
