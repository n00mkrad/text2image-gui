using HTAlt.WinForms;
using Microsoft.WindowsAPICodePack.Taskbar;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Forms
{
    public partial class MainForm
    {
        private Dictionary<Control, List<Control>> _categoryPanels = new Dictionary<Control, List<Control>>(); // Key: Collapse Button - Value: Child Panels
        private List<Control> _expandedCategories = new List<Control>();

        private List<Control> _debugControls { get { return new List<Control> { panelDebugLoopback, panelDebugPerlinThresh, panelDebugSendStdin, panelDebugAppendArgs }; } }

        public bool IsUsingInpaintingModel { get { return Path.ChangeExtension(Config.Get<string>(Config.Keys.Model), null).EndsWith(Constants.SuffixesPrefixes.InpaintingMdlSuf); } }

        public void InitializeControls()
        {
            comboxSampler.FillFromEnum<Sampler>(Strings.Samplers, 0);
            comboxSeamless.FillFromEnum<SeamlessMode>(Strings.SeamlessMode, 0);
            comboxInpaintMode.FillFromEnum<InpaintMode>(Strings.InpaintMode, 0);
            comboxResizeGravity.FillFromEnum<ImageMagick.Gravity>(Strings.ImageGravity, 4, new List<ImageMagick.Gravity> { ImageMagick.Gravity.Undefined });

            _categoryPanels.Add(btnCollapseDebug, new List<Control> { panelDebugAppendArgs, panelDebugSendStdin, panelDebugPerlinThresh, panelDebugLoopback });
            _categoryPanels.Add(btnCollapseRendering, new List<Control> { panelRes, panelSampler });
            _categoryPanels.Add(btnCollapseSymmetry, new List<Control> { panelSeamless });
            _categoryPanels.Add(btnCollapseGeneration, new List<Control> { panelInpainting, panelIterations, panelSteps, panelScale, panelScaleImg, panelSeed });

            _expandedCategories = new List<Control> { btnCollapseRendering, btnCollapseGeneration };
            _categoryPanels.Keys.ToList().ForEach(c => c.Click += (s, e) => CollapseToggle((Control)s));
            _categoryPanels.Keys.ToList().ForEach(c => CollapseToggle(c, _expandedCategories.Contains(c)));

            _debugControls.ForEach(c => c.SetVisible(Program.Debug)); // Show debug controls if debug mode is enabled
        }

        public void LoadControls()
        {
            ConfigParser.LoadGuiElement(upDownIterations, Config.Keys.Iterations);
            ConfigParser.LoadGuiElement(sliderSteps, Config.Keys.Steps);
            ConfigParser.LoadGuiElement(sliderScale, Config.Keys.Scale);
            ConfigParser.LoadGuiElement(comboxResW, Config.Keys.ResW);
            ConfigParser.LoadGuiElement(comboxResH, Config.Keys.ResH);
            ConfigParser.LoadComboxIndex(comboxSampler, Config.Keys.Sampler);
            ConfigParser.LoadGuiElement(sliderInitStrength, Config.Keys.InitStrength);
            ConfigParser.LoadGuiElement(checkboxHiresFix, Config.Keys.HiresFix);
        }

        public void SaveControls()
        {
            ConfigParser.SaveGuiElement(upDownIterations, Config.Keys.Iterations);
            ConfigParser.SaveGuiElement(sliderSteps, Config.Keys.Steps);
            ConfigParser.SaveGuiElement(sliderScale, Config.Keys.Scale);
            ConfigParser.SaveGuiElement(comboxResW, Config.Keys.ResW);
            ConfigParser.SaveGuiElement(comboxResH, Config.Keys.ResH);
            ConfigParser.SaveComboxIndex(comboxSampler, Config.Keys.Sampler);
            ConfigParser.SaveGuiElement(sliderInitStrength, Config.Keys.InitStrength);
            ConfigParser.SaveGuiElement(checkboxHiresFix, Config.Keys.HiresFix);
        }

        public void TryRefreshUiState(bool skipIfHidden = true)
        {
            if (skipIfHidden && Opacity < 1f)
                return;

            try
            {
                this.StopRendering();
                RefreshUiState();
                this.ResumeRendering();
            }
            catch(Exception ex)
            {
                this.ResumeRendering();
                Logger.LogException(ex, true, "TryRefreshUiState:");
            }
        }

        private void RefreshUiState ()
        {
            panelPromptNeg.SetVisible(ConfigParser.CurrentImplementation.GetInfo().SupportsNegativePrompt && !IsUsingInpaintingModel);
            panelSampler.SetVisible(ConfigParser.CurrentImplementation == Implementation.InvokeAi);
            panelSeamless.SetVisible(ConfigParser.CurrentImplementation == Implementation.InvokeAi);
            panelRes.SetVisible(ShouldControlBeVisible(panelRes));
            panelScaleImg.SetVisible(ShouldControlBeVisible(panelScaleImg));
            bool adv = Config.Get<bool>(Config.Keys.AdvancedUi);
            upDownIterations.Maximum = !adv ? 10000 : 100000;
            sliderSteps.ActualMaximum = !adv ? 120 : 500;
            sliderSteps.ChangeStep(!adv ? 5 : 1);
            sliderScale.ActualMaximum = !adv ? 25 : 50;
            comboxResW.SetItems(MainUi.GetResolutions(320, adv ? 4096 : 2048).Select(x => x.ToString()), UiExtensions.SelectMode.Retain, UiExtensions.SelectMode.Last);
            comboxResH.SetItems(MainUi.GetResolutions(320, adv ? 4096 : 2048).Select(x => x.ToString()), UiExtensions.SelectMode.Retain, UiExtensions.SelectMode.Last);

            #region Init Img & Embeddings Stuff

            TtiUtils.CleanInitImageList();

            bool img2img = MainUi.CurrentInitImgPaths.Any();

            panelInpainting.SetVisible(ShouldControlBeVisible(panelInpainting));
            panelInitImgStrength.SetVisible(ShouldControlBeVisible(panelInitImgStrength));
            textboxClipsegMask.SetVisible((InpaintMode)comboxInpaintMode.SelectedIndex == InpaintMode.TextMask);
            comboxResizeGravity.SetVisible(comboxInpaintMode.Visible && (InpaintMode)comboxInpaintMode.SelectedIndex == InpaintMode.Outpaint);
            panelSeamless.SetVisible(!comboxInpaintMode.Visible || (InpaintMode)comboxInpaintMode.SelectedIndex == InpaintMode.Disabled);

            btnInitImgBrowse.Text = img2img ? $"Clear Image{(MainUi.CurrentInitImgPaths.Count == 1 ? "" : "s")}" : "Load Image(s)";

            labelCurrentImage.Text = !img2img ? "No initialization image loaded." : (MainUi.CurrentInitImgPaths.Count == 1 ? $"Currently using {Path.GetFileName(MainUi.CurrentInitImgPaths[0])}" : $"Currently using {MainUi.CurrentInitImgPaths.Count} images.");
            toolTip.SetToolTip(labelCurrentImage, $"{labelCurrentImage.Text.Trunc(100)}\n\nShift + Hover to preview.");

            Program.MainForm.checkboxShowInitImg.SetVisible(MainUi.CurrentInitImgPaths.Any());
            ImageViewer.UpdateInitImgViewer();
            _categoryPanels.Keys.ToList().ForEach(btn => btn.SetVisible(_categoryPanels[btn].Any(p => p.Visible))); // Hide collapse buttons if their category has 0 visible panels

            #endregion
        }

        public void UpdateInpaintUi()
        {
            btnResetMask.SetVisible(Inpainting.CurrentMask != null);
            btnEditMask.SetVisible(Inpainting.CurrentMask != null);
        }

        public void OpenLogsMenu()
        {
            var first = menuStripLogs.Items.Cast<ToolStripMenuItem>().First();
            menuStripLogs.Items.Clear();
            menuStripLogs.Items.Add(first);
            var openLogs = menuStripLogs.Items.Add($"Open Logs Folder");
            openLogs.Click += (s, ea) => { Process.Start("explorer", Paths.GetLogPath().Wrap()); };

            foreach (var log in Logger.CachedEntries)
            {
                ToolStripItem newItem = menuStripLogs.Items.Add($"Copy {log.Key}");
                newItem.Click += (s, ea) => { OsUtils.SetClipboard(Logger.EntriesToString(Logger.CachedEntries[log.Key], true, true)); };
            }

            menuStripLogs.Show(Cursor.Position);
        }

        public void HandleImageViewerClick(bool rightClick)
        {
            pictBoxImgViewer.Focus();

            if (rightClick)
            {
                if (!string.IsNullOrWhiteSpace(ImageViewer.CurrentImagePath) && File.Exists(ImageViewer.CurrentImagePath))
                {
                    reGenerateImageWithCurrentSettingsToolStripMenuItem.SetVisible(!Program.Busy);
                    useAsInitImageToolStripMenuItem.SetVisible(!Program.Busy);
                    postProcessImageToolStripMenuItem.SetVisible(!Program.Busy && TextToImage.CurrentTaskSettings.Implementation == Implementation.InvokeAi);
                    copyImageToClipboardToolStripMenuItem.SetVisible(pictBoxImgViewer.Image != null);
                    menuStripOutputImg.Show(Cursor.Position);
                }
            }
            else
            {
                if (pictBoxImgViewer.Image != null)
                    ImagePopup.Show(pictBoxImgViewer.Image, ImagePopupForm.SizeMode.Percent100);
            }
        }

        public void SetProgress(int percent, bool taskbarProgress = true)
        {
            SetProgress(percent, taskbarProgress, progressBar);
        }

        public void SetProgressImg(int percent, bool taskbarProgress = false)
        {
            SetProgress(percent, taskbarProgress, progressBarImg);
        }

        public void SetProgress(int percent, bool taskbarProgress, HTProgressBar bar)
        {
            if (bar == null)
                bar = progressBar;

            percent = percent.Clamp(0, 100);
            bar.Value = percent;
            bar.Refresh();

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

        public void SetHiresFixVisible()
        {
            bool txt2img = !MainUi.CurrentInitImgPaths.Any();
            bool compatible = ConfigParser.CurrentImplementation == Implementation.InvokeAi;
            checkboxHiresFix.SetVisible((comboxResW.GetInt() > 512 || comboxResH.GetInt() > 512) && txt2img && compatible);
        }

        public void CollapseToggle(Control collapseBtn, bool? overrideState = null)
        {
            List<Control> controls = _categoryPanels[collapseBtn];
            bool show = overrideState != null ? (bool)overrideState : controls.Any(c => c.Height == 0);
            controls.ForEach(c => c.Height = show ? 35 : 0);
            collapseBtn.Text = $"{(show ? "Hide" : "Show")} {Strings.MainUiCategories.Get(collapseBtn.Name, true)}";
        }
    }
}
