using Dasync.Collections;
using HTAlt.WinForms;
using Microsoft.WindowsAPICodePack.Taskbar;
using StableDiffusionGui.Data;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Properties;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static StableDiffusionGui.Main.Enums.StableDiffusion;
using static StableDiffusionGui.Ui.MainUi;

namespace StableDiffusionGui.Forms
{
    public partial class MainForm
    {
        private Dictionary<Control, List<Panel>> _categoryPanels = new Dictionary<Control, List<Panel>>(); // Key: Collapse Button - Value: Child Panels
        private List<Control> _expandedCategories = new List<Control>();

        private List<Control> _debugControls { get { return new List<Control> { panelDebugLoopback, panelDebugPerlinThresh, panelDebugSendStdin, panelDebugAppendArgs }; } }

        public bool IsUsingInpaintingModel { get { return Path.ChangeExtension(Config.Instance.Model, null).EndsWith(Constants.SuffixesPrefixes.InpaintingMdlSuf); } }
        public bool AnyInits { get { return MainUi.CurrentInitImgPaths.Any(); } }
        private Dictionary<Panel, int> _panelHeights = new Dictionary<Panel, int>();

        public void InitializeControls()
        {
            // Fill data
            comboxSampler.FillFromEnum<Sampler>(Strings.Samplers, 0);
            comboxSeamless.FillFromEnum<SeamlessMode>(Strings.SeamlessMode, 0);
            comboxSymmetry.FillFromEnum<SymmetryMode>(Strings.SymmetryMode, 0);
            comboxInpaintMode.FillFromEnum<ImgMode>(Strings.InpaintMode, 0);
            comboxResizeGravity.FillFromEnum<ImageMagick.Gravity>(Strings.ImageGravity, 4, new List<ImageMagick.Gravity> { ImageMagick.Gravity.Undefined });
            comboxBackend.FillFromEnum<Implementation>(Strings.Implementation, -1, Implementation.OptimizedSd.AsList());
            comboxBackend.Text = Strings.Implementation.Get(Config.Instance.Implementation.ToString());
            ReloadModelsCombox();
            UpdateModel();
            ReloadEmbeddings();
            ReloadLoras();
            comboxModelArch.FillFromEnum<Enums.Models.SdArch>(Strings.SdModelArch, 0);

            // Set categories
            _categoryPanels.Add(btnCollapseImplementation, new List<Panel> { panelBackend, panelModel });
            _categoryPanels.Add(btnCollapsePrompt, new List<Panel> { panelPrompt, panelPromptNeg, panelEmbeddings, panelLoras, panelAiInputs });
            _categoryPanels.Add(btnCollapseGeneration, new List<Panel> { panelInpainting, panelInitImgStrength, panelIterations, panelSteps, panelScale, panelScaleImg, panelSeed });
            _categoryPanels.Add(btnCollapseRendering, new List<Panel> { panelRes, panelSampler });
            _categoryPanels.Add(btnCollapseSymmetry, new List<Panel> { panelSeamless, panelSymmetry });
            _categoryPanels.Add(btnCollapseDebug, new List<Panel> { panelDebugAppendArgs, panelDebugSendStdin, panelDebugPerlinThresh, panelDebugLoopback });

            // Expand default categories
            _expandedCategories = new List<Control> { btnCollapsePrompt, btnCollapseRendering, btnCollapseGeneration };
            _categoryPanels.Keys.ToList().ForEach(c => c.Click += (s, e) => CollapseToggle((Control)s));
            _categoryPanels.Keys.ToList().ForEach(c => CollapseToggle(c, _expandedCategories.Contains(c)));

            _debugControls.ForEach(c => c.SetVisible(Program.Debug)); // Show debug controls if debug mode is enabled

            // Events
            comboxBackend.SelectedIndexChanged += (s, e) => { Config.Instance.Implementation = ParseUtils.GetEnum<Implementation>(comboxBackend.Text, true, Strings.Implementation); TryRefreshUiState(); }; // Implementation change
            comboxModel.SelectedIndexChanged += (s, e) => ModelChanged();
            comboxModel.DropDown += (s, e) => ReloadModelsCombox();
            comboxModel.DropDownClosed += (s, e) => panelSettings.Focus();
            comboxResW.SelectedIndexChanged += (s, e) => ResolutionChanged(); // Resolution change
            comboxResH.SelectedIndexChanged += (s, e) => ResolutionChanged(); // Resolution change
            ImageViewer.OnImageChanged += () => UpdateImgViewerBtns(); // Image change
            comboxEmbeddingList.DropDown += (s, e) => ReloadEmbeddings(); // Reload embeddings
            gridLoras.KeyUp += (s, e) => { if (e.KeyCode == Keys.Enter) GridMoveUp(gridLoras); };
            gridLoras.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (gridLoras.CurrentCell == null || gridLoras.CurrentCell.ValueType != typeof(bool))
                    return;

                gridLoras.EndEdit();
                BeginInvoke(new MethodInvoker(() => { SortLoras(); }));
            };
        }

        public void LoadControls()
        {
            ConfigParser.LoadGuiElement(upDownIterations, ref Config.Instance.Iterations);
            ConfigParser.LoadGuiElement(sliderSteps, ref Config.Instance.Steps);
            ConfigParser.LoadGuiElement(sliderScale, ref Config.Instance.Scale);
            ConfigParser.LoadGuiElement(comboxResW, ref Config.Instance.ResW);
            ConfigParser.LoadGuiElement(comboxResH, ref Config.Instance.ResH);
            ConfigParser.LoadComboxIndex(comboxSampler, ref Config.Instance.SamplerIdx);
            ConfigParser.LoadGuiElement(sliderInitStrength, ref Config.Instance.InitStrength);
            ConfigParser.LoadGuiElement(checkboxHiresFix, ref Config.Instance.HiresFix);
        }

        public void SaveControls()
        {
            ConfigParser.SaveGuiElement(upDownIterations, ref Config.Instance.Iterations);
            ConfigParser.SaveGuiElement(sliderSteps, ref Config.Instance.Steps);
            ConfigParser.SaveGuiElement(sliderScale, ref Config.Instance.Scale);
            ConfigParser.SaveGuiElement(comboxResW, ref Config.Instance.ResW);
            ConfigParser.SaveGuiElement(comboxResH, ref Config.Instance.ResH);
            ConfigParser.SaveComboxIndex(comboxSampler, ref Config.Instance.SamplerIdx);
            ConfigParser.SaveGuiElement(sliderInitStrength, ref Config.Instance.InitStrength);
            ConfigParser.SaveGuiElement(checkboxHiresFix, ref Config.Instance.HiresFix);

            if (Config.Instance != null && comboxModel.SelectedIndex >= 0)
                Config.Instance.ModelArchs[((Model)comboxModel.SelectedItem).FullName] = ParseUtils.GetEnum<Enums.Models.SdArch>(comboxModelArch.Text, true, Strings.SdModelArch);

            Config.Save();
        }

        public void TryRefreshUiState(bool skipIfHidden = true)
        {
            if (skipIfHidden && Opacity < 1f)
                return;

            ((Action)(() => RefreshUiState())).RunWithUiStopped(this, "TryRefreshUiState:", true);
        }

        private void RefreshUiState()
        {
            Implementation imp = Config.Instance.Implementation;
            comboxBackend.Text = Strings.Implementation.Get(imp.ToString());

            // Panel visibility
            SetVisibility(new Control[] { panelPromptNeg, panelEmbeddings, panelInitImgStrength, panelInpainting, panelScaleImg, panelRes, panelSampler, panelSeamless, panelSymmetry, checkboxHiresFix,
                textboxClipsegMask, panelResizeGravity, labelResChange, btnResetRes, checkboxShowInitImg, panelModel, panelLoras }, imp);

            bool adv = Config.Instance.AdvancedUi;
            upDownIterations.Maximum = !adv ? Config.IniInstance.IterationsMax : Config.IniInstance.IterationsMax * 10;
            sliderSteps.ActualMaximum = !adv ? Config.IniInstance.StepsMax : Config.IniInstance.StepsMax * 4;
            sliderScale.ActualMaximum = (decimal)(!adv ? Config.IniInstance.ScaleMax : Config.IniInstance.ScaleMax * 2);
            int resMax = !adv ? Config.IniInstance.ResolutionMax : Config.IniInstance.ResolutionMax * 2;
            var validResolutions = MainUi.GetResolutions(Config.IniInstance.ResolutionMin, resMax).Select(i => i.ToString());
            comboxResW.SetItems(validResolutions, UiExtensions.SelectMode.Retain, UiExtensions.SelectMode.First);
            comboxResH.SetItems(validResolutions, UiExtensions.SelectMode.Retain, UiExtensions.SelectMode.First);

            #region Init Img & Embeddings Stuff

            TtiUtils.CleanInitImageList();

            btnInitImgBrowse.Text = AnyInits ? $"Clear Image{(MainUi.CurrentInitImgPaths.Count == 1 ? "" : "s")}" : "Load Image(s)";
            labelCurrentImage.Text = !AnyInits ? "No image(s) loaded." : (MainUi.CurrentInitImgPaths.Count == 1 ? $"{IoUtils.GetImage(MainUi.CurrentInitImgPaths[0]).Size.AsString()} Image: {Path.GetFileName(MainUi.CurrentInitImgPaths[0])}" : $"{MainUi.CurrentInitImgPaths.Count} Images");
            toolTip.SetToolTip(labelCurrentImage, $"{labelCurrentImage.Text.Trunc(100)}\n\nShift + Hover to preview.");

            ImageViewer.UpdateInitImgViewer();
            UpdateImgViewerBtns();
            ResolutionChanged();
            UpdateModel();
            ModelChanged();
            ReloadLoras();
            ReloadEmbeddings();
            _categoryPanels.Keys.ToList().ForEach(btn => btn.Parent.SetVisible(_categoryPanels[btn].Any(p => p.Visible))); // Hide collapse buttons if their category has 0 visible panels

            #endregion
        }

        private string _prevSelectedModel = "";

        private void ModelChanged()
        {
            Model mdl = (Model)comboxModel.SelectedItem;

            if (mdl == null || mdl.FullName == _prevSelectedModel)
                return;

            _prevSelectedModel = mdl.FullName;
            var formats = new List<Enums.Models.Format> { Enums.Models.Format.Pytorch, Enums.Models.Format.Safetensors };

            List<Enums.Models.SdArch> exclusionList = formats.Contains(mdl.Format) ? new List<Enums.Models.SdArch>() : Enum.GetValues(typeof(Enums.Models.SdArch)).Cast<Enums.Models.SdArch>().Skip(1).ToList();
            comboxModelArch.FillFromEnum<Enums.Models.SdArch>(Strings.SdModelArch, 0, exclusionList);

            if (Config.Instance.ModelArchs.ContainsKey(mdl.FullName))
                comboxModelArch.SetWithText(Config.Instance.ModelArchs[mdl.FullName].ToString(), false, Strings.SdModelArch);

            ConfigParser.SaveGuiElement(comboxModel, ref Config.Instance.Model);
        }

        public void ReloadEmbeddings()
        {
            IEnumerable<string> embeddings = Models.GetEmbeddings().Select(m => m.FormatIndependentName);
            comboxEmbeddingList.SetItems(new[] { "None" }.Concat(embeddings), UiExtensions.SelectMode.Retain);
            panelEmbeddings.SetVisible(embeddings.Any()); // Disable panel if no embeddings in folder
        }

        private bool _shownLoraFilenameWarning = false;

        public void ReloadLoras()
        {
            gridLoras.Rows.Cast<DataGridViewRow>().Where(row => row.Cells[2].Value.ToString().GetFloat() <= 0f).ToList().ForEach(row => row.Cells[2].Value = "1.0");
            var selection = GetLoras(); // Save current selection
            List<Model> loras = Models.GetLoras();
            loras = ValidateLoraNames(loras);
            panelLoras.SetVisible(loras.Any()); // Disable panel if no LoRAs in folder

            if (!loras.Any())
                return;

            var previousData = gridLoras.Rows.Cast<DataGridViewRow>().Select(row => (bool)row.Cells[0].Value + (string)row.Cells[1].Value + (string)row.Cells[2].Value).ToList();
            var previousLoraList = gridLoras.Rows.Cast<DataGridViewRow>().Select(row => (string)row.Cells[1].Value).ToList();

            if (string.Join("", loras.Select(l => l.FormatIndependentName).OrderBy(l => l)) == string.Join("", previousLoraList.OrderBy(l => l)))
                return;

            gridLoras.Rows.Clear();
            loras.ToList().ForEach(l => gridLoras.Rows.Add(false, l.FormatIndependentName, "1.0"));
            SetLoras(selection); // Restore selection
        }

        public void SortLoras(bool force = false)
        {
            List<object[]> rowValues = gridLoras.Rows.Cast<DataGridViewRow>().ToList().Select(r => new object[] { r.Cells[0].Value, r.Cells[1].Value, r.Cells[2].Value }).ToList();
            
            if (rowValues.Count < 2) // No need to sort 1 item
                return;
            
            List<object[]> newRowValues = rowValues.OrderByDescending(row => (bool)row[0]).ThenBy(row => (string)row[1]).ToList();
            
            if (!force && (rowValues.ToJson() == newRowValues.ToJson())) // Brute-force comparison because SequenceEqual didn't work (?)
                return; // Skip sort if data is already in correct order
            
            gridLoras.Rows.Clear();
            newRowValues.ForEach(row => gridLoras.Rows.Add((bool)row[0], (string)row[1], (string)row[2]));
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
            var existing = menuStripLogs.Items.Cast<ToolStripMenuItem>().Take(1).ToArray();
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

        public void OpenLogViewerWindow()
        {
            Application.OpenForms.Cast<Form>().Where(f => f is RealtimeLoggerForm).ToList().ForEach(f => f.Close());
            new RealtimeLoggerForm().Show();
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
            if (this.RequiresInvoke(new Action<int, bool, HTProgressBar>(SetProgress), percent, taskbarProgress, bar))
                return;

            if (bar == null)
                bar = progressBar;

            percent = percent.Clamp(0, 100);

            if (bar.Value == percent)
                return;

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
                List<Panel> panels = _categoryPanels[collapseBtn];
                bool show = overrideState != null ? (bool)overrideState : panels.Any(c => c.Height == 0);
                panels.Where(p => p.Height > 0).ToList().ForEach(p => _panelHeights[p] = p.Height);
                panels.ForEach(p => p.Height = show ? _panelHeights[p] : 0);
                string catName = Strings.MainUiCategories.Get(collapseBtn.Name, true);
                collapseBtn.Text = show ? $"Hide {catName}" : $"{catName}...";
            })).RunWithUiStopped(this);
        }

        public void UpdateWindowTitle()
        {
            if (this.RequiresInvoke(new Action(UpdateWindowTitle)))
                return;

            string busyText = Program.State == Program.BusyState.Standby ? "" : "Busy...";
            Text = string.Join(" - ", new[] { $"Stable Diffusion GUI {Program.Version}", MainUi.GpuInfo, busyText }.Where(s => s.IsNotEmpty()));
        }

        public void UpdateModel(bool reloadList = false, Implementation imp = (Implementation)(-1))
        {
            if (!comboxModel.Visible)
                return;

            if (imp == (Implementation)(-1))
                imp = Config.Instance.Implementation;

            if (imp == (Implementation)(-1))
                return;

            string currentModel = Config.Instance.Model;
            ReloadModelsCombox(imp);
            comboxModel.Text = currentModel;

            if (comboxModel.SelectedIndex < 0 && comboxModel.Items.Count > 0)
                comboxModel.SelectedIndex = 0;
        }

        public void ReloadModelsCombox(Implementation imp = (Implementation)(-1))
        {
            if (imp == (Implementation)(-1))
                imp = Config.Instance.Implementation;

            IEnumerable<Model> models = Models.GetModelsAll().Where(m => m.Type == Enums.Models.Type.Normal && imp.GetInfo().SupportedModelFormats.Contains(m.Format));
            comboxModel.SetItems(models, UiExtensions.SelectMode.Retain, UiExtensions.SelectMode.None);
        }

        private void SetLoraPanelSize(PromptFieldSizeMode sizeMode = PromptFieldSizeMode.Toggle)
        {
            ((Action)(() =>
            {
                int smallHeight = 70;

                if (panelLoras.Height == 0)
                    return;

                if (sizeMode == PromptFieldSizeMode.Toggle)
                    sizeMode = panelLoras.Height == smallHeight ? PromptFieldSizeMode.Expand : PromptFieldSizeMode.Collapse;

                if (sizeMode == PromptFieldSizeMode.Expand)
                {
                    btnExpandLoras.BackgroundImage = Resources.upArrowIcon;
                    panelLoras.Height = smallHeight * 4;
                }

                if (sizeMode == PromptFieldSizeMode.Collapse)
                {
                    btnExpandLoras.BackgroundImage = Resources.downArrowIcon;
                    panelLoras.Height = smallHeight;
                }

                Program.MainForm.panelSettings.Focus();
            })).RunWithUiStopped(Program.MainForm);
        }

        private void GridMoveUp(DataGridView grid)
        {
            if (grid.CurrentCell != null)
            {
                int currentRowIndex = grid.CurrentCell.RowIndex;

                if (currentRowIndex > 0)
                    grid.Rows[currentRowIndex - 1].Cells[grid.CurrentCell.ColumnIndex].Selected = true;
            }
        }

        public void UpdateImgViewerBtns()
        {
            ((Action)(() =>
            {
                bool hasImage = pictBoxImgViewer.Image != null;
                btnNextImg.SetVisible(hasImage);
                btnPrevImg.SetVisible(hasImage);
                btnDeleteBatch.SetVisible(hasImage);
                btnSaveToFavs.SetVisible(hasImage);
                flowPanelImgButtons.Controls.Cast<Control>().ToList().ForEach(c => c.Padding = new Padding(3, 3, 3, 3));
                Control rightmost = flowPanelImgButtons.Controls.Cast<Control>().OrderBy(c => c.Right).LastOrDefault();
                rightmost.Padding = new Padding(rightmost.Padding.Left, rightmost.Padding.Top, 0, rightmost.Padding.Bottom);
                UpdateSaveModeBtn();
            })).RunWithUiStopped(this);
        }

        public void ToggleSaveMode ()
        {
            Config.Instance.AutoDeleteImgs = !Config.Instance.AutoDeleteImgs;
            UpdateSaveModeBtn();
        }

        public void UpdateSaveModeBtn ()
        {
            if (Config.Instance.AutoDeleteImgs && btnSaveMode.BackgroundImage != Resources.IconArchiveOff)
            {
                btnSaveMode.BackgroundImage = Resources.IconArchiveOff;
                toolTip.SetToolTip(btnSaveMode, "Auto-Delete is enabled: Generated images will be deleted as soon as another batch is generated or the program closes.\n\nClick to disable.");
            }
            else if(!Config.Instance.AutoDeleteImgs && btnSaveMode.BackgroundImage != Resources.IconArchiveOn)
            {
                btnSaveMode.BackgroundImage = Resources.IconArchiveOn;
                toolTip.SetToolTip(btnSaveMode, "Auto-Delete is disabled: All generated images will be saved.\n\nClick to switch to Auto-Delete mode.");
            }
        }
    }
}
