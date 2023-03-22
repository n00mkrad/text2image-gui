using HTAlt.WinForms;
using Microsoft.WindowsAPICodePack.Taskbar;
using StableDiffusionGui.Data;
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
        public bool AnyInits { get { return MainUi.CurrentInitImgPaths.Any(); } }

        public void InitializeControls()
        {
            // Fill data
            comboxSampler.FillFromEnum<Sampler>(Strings.Samplers, 0);
            comboxSeamless.FillFromEnum<SeamlessMode>(Strings.SeamlessMode, 0);
            comboxSymmetry.FillFromEnum<SymmetryMode>(Strings.SymmetryMode, 0);
            comboxInpaintMode.FillFromEnum<InpaintMode>(Strings.InpaintMode, 0);
            comboxResizeGravity.FillFromEnum<ImageMagick.Gravity>(Strings.ImageGravity, 4, new List<ImageMagick.Gravity> { ImageMagick.Gravity.Undefined });
            comboxEmbeddingList.SetItems(new[] { "None" }.Concat(Models.GetEmbeddings().Select(m => m.FormatIndependentName)), UiExtensions.SelectMode.First);
            comboxBackend.FillFromEnum<Implementation>(Strings.Implementation, -1);
            comboxBackend.Text = Strings.Implementation.Get(Config.Get<string>(Config.Keys.ImplementationName));
            UpdateModel();

            // Set categories
            _categoryPanels.Add(btnCollapseImplementation, new List<Control> { panelBackend, panelModel });
            _categoryPanels.Add(btnCollapseDebug, new List<Control> { panelDebugAppendArgs, panelDebugSendStdin, panelDebugPerlinThresh, panelDebugLoopback });
            _categoryPanels.Add(btnCollapseRendering, new List<Control> { panelRes, panelSampler });
            _categoryPanels.Add(btnCollapseSymmetry, new List<Control> { panelSeamless, panelSymmetry });
            _categoryPanels.Add(btnCollapseGeneration, new List<Control> { panelInpainting, panelIterations, panelSteps, panelScale, panelScaleImg, panelSeed });

            // Expand default categories
            _expandedCategories = new List<Control> { btnCollapseImplementation, btnCollapseRendering, btnCollapseGeneration };
            _categoryPanels.Keys.ToList().ForEach(c => c.Click += (s, e) => CollapseToggle((Control)s));
            _categoryPanels.Keys.ToList().ForEach(c => CollapseToggle(c, _expandedCategories.Contains(c)));

            _debugControls.ForEach(c => c.SetVisible(Program.Debug)); // Show debug controls if debug mode is enabled

            // Events
            comboxBackend.SelectedIndexChanged += (s, e) => { Config.Set(Config.Keys.ImplementationName, Strings.Implementation.GetReverse(comboxBackend.Text)); TryRefreshUiState(); }; // Implementation change
            comboxModel.SelectedIndexChanged += (s, e) => ConfigParser.SaveGuiElement(comboxModel, Config.Keys.Model);
            comboxModel.DropDown += (s, e) => ReloadModelsCombox();
            comboxModel.DropDownClosed += (s, e) => panelSettings.Focus();
            comboxResW.SelectedIndexChanged += (s, e) => ResolutionChanged(); // Resolution change
            comboxResH.SelectedIndexChanged += (s, e) => ResolutionChanged(); // Resolution change

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
            catch (Exception ex)
            {
                this.ResumeRendering();
                Logger.LogException(ex, true, "TryRefreshUiState:");
            }
        }

        private void RefreshUiState()
        {
            Implementation imp = ConfigParser.CurrentImplementation;

            // Panel visibility
            SetVisibility(new Control[] { panelPromptNeg, panelEmbeddings, panelInitImgStrength, panelInpainting, panelScaleImg, panelRes, panelSampler, panelSeamless, panelSymmetry, checkboxHiresFix,
                textboxClipsegMask, panelResizeGravity, labelResChange, btnResetRes, checkboxShowInitImg, panelModel }, imp);

            bool adv = Config.Get<bool>(Config.Keys.AdvancedUi);
            upDownIterations.Maximum = !adv ? 10000 : 100000;
            sliderSteps.ActualMaximum = !adv ? 120 : 500;
            sliderSteps.ChangeStep(!adv ? 5 : 1);
            sliderScale.ActualMaximum = !adv ? 25 : 50;
            var validResolutions = MainUi.GetResolutions(320, adv ? 4096 : 2048).Select(i => i.ToString());
            comboxResW.SetItems(validResolutions, UiExtensions.SelectMode.Retain, UiExtensions.SelectMode.First);
            comboxResH.SetItems(validResolutions, UiExtensions.SelectMode.Retain, UiExtensions.SelectMode.First);

            #region Init Img & Embeddings Stuff

            TtiUtils.CleanInitImageList();

            btnInitImgBrowse.Text = AnyInits ? $"Clear Image{(MainUi.CurrentInitImgPaths.Count == 1 ? "" : "s")}" : "Load Image(s)";

            labelCurrentImage.Text = !AnyInits ? "No image(s) loaded." : (MainUi.CurrentInitImgPaths.Count == 1 ? $"{IoUtils.GetImage(MainUi.CurrentInitImgPaths[0]).Size.AsString()} Image: {Path.GetFileName(MainUi.CurrentInitImgPaths[0])}" : $"{MainUi.CurrentInitImgPaths.Count} Images");
            toolTip.SetToolTip(labelCurrentImage, $"{labelCurrentImage.Text.Trunc(100)}\n\nShift + Hover to preview.");

            ImageViewer.UpdateInitImgViewer();
            ResolutionChanged();
            UpdateModel();
            _categoryPanels.Keys.ToList().ForEach(btn => btn.SetVisible(_categoryPanels[btn].Any(p => p.Visible))); // Hide collapse buttons if their category has 0 visible panels

            #endregion
        }

        private void ResolutionChanged()
        {
            SetVisibility(new Control[] { checkboxHiresFix, labelResChange });

            int w = comboxResW.GetInt();
            int h = comboxResH.GetInt();

            if (labelResChange.Visible && pictBoxInitImg.Image != null)
            {
                int diffW = w - pictBoxInitImg.Image.Width;
                int diffH = h - pictBoxInitImg.Image.Height;
                labelResChange.Text = diffW != 0 || diffH != 0 ? $"+{diffW}, +{diffH}" : "";
                SetVisibility(btnResetRes);
            }
            else
            {
                labelResChange.SetVisible(false);
                btnResetRes.SetVisible(false);
            }

            if (labelAspectRatio.Visible)
            {
                int gcd = GreatestCommonDivisor(w, h);
                string ratioText = $"{w / gcd}:{h / gcd}";
                labelAspectRatio.Text = ratioText.Length <= 5 ? $"Ratio {ratioText.Replace("8:5", "8:5 (16:10)").Replace("7:3", "7:3 (21:9)")}" : "";
            }
        }

        private int GreatestCommonDivisor(int a, int b)
        {
            return b == 0 ? a : GreatestCommonDivisor(b, a % b);
        }

        public void UpdateInpaintUi()
        {
            btnResetMask.SetVisible(Inpainting.CurrentMask != null);
            btnEditMask.SetVisible(Inpainting.CurrentMask != null);
        }

        public void OpenLogsMenu()
        {
            var existing = menuStripLogs.Items.Cast<ToolStripMenuItem>().ToArray();
            menuStripLogs.Items.Clear();
            menuStripLogs.Items.AddRange(existing);
            var openLogs = menuStripLogs.Items.Add($"Open Logs Folder");
            openLogs.Click += (s, ea) => { Process.Start("explorer", Paths.GetLogPath().Wrap()); };

            foreach (var log in Logger.CachedEntries)
            {
                ToolStripMenuItem logItem = new ToolStripMenuItem($"{log.Key}...");

                ToolStripItem openItem = new ToolStripMenuItem($"Open Log File");
                openItem.Click += (s, ea) => { Process.Start(Path.Combine(Paths.GetLogPath(), log.Key)); };
                logItem.DropDownItems.Add(openItem);

                ToolStripItem copyItem = new ToolStripMenuItem($"Copy Text to Clipboard");
                copyItem.Click += (s, ea) => { OsUtils.SetClipboard(Logger.EntriesToString(Logger.CachedEntries[log.Key], true, true)); };
                logItem.DropDownItems.Add(copyItem);

                menuStripLogs.Items.Add(logItem);
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
                    reGenerateImageWithCurrentSettingsToolStripMenuItem.Visible = !Program.Busy;
                    useAsInitImageToolStripMenuItem.Visible = !Program.Busy;
                    postProcessImageToolStripMenuItem.Visible = !Program.Busy && TextToImage.CurrentTaskSettings.Implementation == Implementation.InvokeAi;
                    copyImageToClipboardToolStripMenuItem.Visible = pictBoxImgViewer.Image != null;
                    fitWindowSizeToImageSizeToolStripMenuItem.Visible = MainUi.GetPreferredSize() != System.Drawing.Size.Empty;
                    copySidebySideComparisonImageToolStripMenuItem.Visible = pictBoxInitImg.Image != null && pictBoxImgViewer.Image != null;
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

        public void CollapseToggle(Control collapseBtn, bool? overrideState = null)
        {
            ((Action)(() =>
            {
                List<Control> controls = _categoryPanels[collapseBtn];
                bool show = overrideState != null ? (bool)overrideState : controls.Any(c => c.Height == 0);
                controls.ForEach(c => c.Height = show ? 35 : 0);
                string catName = Strings.MainUiCategories.Get(collapseBtn.Name, true);
                collapseBtn.Text = show ? $"Hide {catName}" : $"{catName}...";
            })).RunWithUiStopped(this);
        }

        public void UpdateWindowTitle()
        {
            string busyText = Program.State == Program.BusyState.Standby ? "" : "Busy...";
            Text = string.Join(" - ", new[] { $"Stable Diffusion GUI {Program.Version}", MainUi.GpuInfo, busyText }.Where(s => s.IsNotEmpty()));
        }

        public void UpdateModel(bool reloadList = false, Implementation imp = (Implementation)(-1))
        {
            this.PrintThread(); // TODO: Remove me later

            if (!comboxModel.Visible)
                return;

            if (imp == (Implementation)(-1))
                imp = ConfigParser.CurrentImplementation;

            if (imp == (Implementation)(-1))
                return;

            string currentModel = Config.Get<string>(Config.Keys.Model);

            if (reloadList)
            {
                ReloadModelsCombox(imp);
            }
            else if (!comboxModel.Items.Cast<string>().Any(m => m == currentModel))
            {
                comboxModel.SetItems(new string[] { currentModel }, 0);
            }

            comboxModel.Text = currentModel;
        }

        private void ReloadModelsCombox (Implementation imp = (Implementation)(-1))
        {
            if (imp == (Implementation)(-1))
                imp = ConfigParser.CurrentImplementation;

            IEnumerable<Model> models = Models.GetModelsAll().Where(m => imp.GetInfo().SupportedModelFormats.Contains(m.Format));
            comboxModel.SetItems(models.Select(m => m.Name), UiExtensions.SelectMode.Retain, UiExtensions.SelectMode.None);
        }
    }
}
