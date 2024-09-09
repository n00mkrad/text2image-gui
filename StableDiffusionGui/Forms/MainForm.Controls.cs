using Dasync.Collections;
using HTAlt.WinForms;
using Microsoft.WindowsAPICodePack.Taskbar;
using StableDiffusionGui.Data;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Implementations;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Os;
using StableDiffusionGui.Properties;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static StableDiffusionGui.Main.Enums.StableDiffusion;
using static StableDiffusionGui.Ui.MainUi;

namespace StableDiffusionGui.Forms
{
    public partial class MainForm
    {
        public Dictionary<Control, List<Panel>> CategoryPanels = new Dictionary<Control, List<Panel>>(); // Key: Collapse Button - Value: Child Panels
        private List<Control> _expandedCategories = new List<Control>();

        private List<Control> _debugControls { get { return new List<Control> { panelDebugLoopback, checkboxPreview }; } }

        public bool IsUsingInpaintingModel { get { return Path.ChangeExtension(Config.Instance.Model, null).EndsWith(Constants.SuffixesPrefixes.InpaintingMdlSuf); } }
        public bool AnyInits { get { return MainUi.CurrentInitImgPaths.Any(); } }
        private Dictionary<Panel, int> _panelHeights = new Dictionary<Panel, int>();
        private Implementation _lastImplementation = (Implementation)(-1);

        public void InitializeControls()
        {
            // Fill data
            comboxSampler.FillFromEnum<Sampler>(Strings.Samplers, 0);
            comboxUpscaleMode.FillFromEnum<UpscaleMode>(Strings.UpscaleModes, 0);
            comboxSeamless.FillFromEnum<SeamlessMode>(Strings.SeamlessMode, 0);
            comboxSymmetry.FillFromEnum<SymmetryMode>(Strings.SymmetryMode, 0);
            comboxInpaintMode.FillFromEnum<ImgMode>(Strings.InpaintMode, 0);
            comboxResizeGravity.FillFromEnum<ImageMagick.Gravity>(Strings.ImageGravity, 4, new List<ImageMagick.Gravity> { ImageMagick.Gravity.Undefined });
            comboxBackend.FillFromEnum<Implementation>(Strings.Implementation, -1);
            comboxBackend.Text = Strings.Implementation.Get(Config.Instance.Implementation.ToString());
            comboxModelArch.FillFromEnum<ModelArch>(Strings.ModelArch, 0);
            comboxPreprocessor.FillFromEnum<ImagePreprocessor>(Strings.ImagePreprocessors, 0);
            comboxControlnetSlot.SetItems(Enumerable.Range(1, Controlnets.Length).Select(i => $"Slot {i}"), 0);

            // Set categories
            CategoryPanels.Add(btnCollapseImplementation, new List<Panel> { panelBackend, panelModel, panelModel2 });
            CategoryPanels.Add(btnCollapsePrompt, new List<Panel> { panelPrompt, panelPromptNeg, panelEmbeddings, panelLoras, panelBaseImg });
            CategoryPanels.Add(btnCollapseGeneration, new List<Panel> { panelControlnet, panelInpainting, panelInitImgStrength, panelIterations, panelSteps, panelRefineStart, panelScale, panelScaleImg, panelSeed });
            CategoryPanels.Add(btnCollapseRendering, new List<Panel> { panelRes, panelUpscaling, panelSampler });
            CategoryPanels.Add(btnCollapseSymmetry, new List<Panel> { panelSeamless });
            CategoryPanels.Add(btnCollapseDebug, new List<Panel> { panelDebugLoopback });

            // Expand default categories
            _expandedCategories = new List<Control> { btnCollapseImplementation, btnCollapsePrompt, btnCollapseRendering, btnCollapseGeneration };
            CategoryPanels.Keys.ToList().ForEach(c => c.Click += (s, e) => CollapseToggle((Control)s));
            CategoryPanels.Keys.ToList().ForEach(c => CollapseToggle(c, _expandedCategories.Contains(c)));

            _debugControls.ForEach(c => c.SetVisible(Program.Debug)); // Show debug controls if debug mode is enabled

            // Events
            comboxBackend.SelectedIndexChanged += (s, e) => { Config.Instance.Implementation = ParseUtils.GetEnum<Implementation>(comboxBackend.Text, true, Strings.Implementation); TryRefreshUiState(); }; // Implementation change
            comboxModel.SelectedIndexChanged += (s, e) => ModelChanged();
            comboxModel.DropDown += (s, e) => ReloadModelsCombox();
            comboxModel.DropDownClosed += (s, e) => panelSettings.Focus();
            comboxModel2.DropDown += (s, e) => ReloadModelsCombox();
            comboxVae.SelectedIndexChanged += (s, e) => ConfigParser.SaveGuiElement(comboxVae, ref Config.Instance.ModelVae);
            comboxResW.SelectedIndexChanged += (s, e) => ResolutionChanged(); // Resolution change
            comboxResH.SelectedIndexChanged += (s, e) => ResolutionChanged(); // Resolution change
            ImageViewer.OnImageChanged += () => UpdateImgViewerBtns(); // Image change
            comboxEmbeddingList.DropDown += (s, e) => ReloadEmbeddings(); // Reload embeddings
            comboxControlnet.DropDown += (s, e) => ReloadControlnets(); // Reload controlnets
            gridLoras.KeyUp += (s, e) => { if (e.KeyCode == Keys.Enter) GridMoveUp(gridLoras); };
            gridLoras.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (gridLoras.CurrentCell == null || gridLoras.CurrentCell.ValueType != typeof(bool))
                    return;

                gridLoras.EndEdit();
                BeginInvoke(new MethodInvoker(() => { SortLoras(); }));
            };
            comboxResH.LostFocus += (s, e) => { ValidateResolution(comboxResH); };
            comboxResW.LostFocus += (s, e) => { ValidateResolution(comboxResW); };
            comboxUpscaleMode.SelectedIndexChanged += (s, e) => { RefreshUpscaleUi(true); };
            updownUpscaleFactor.ValueChanged += (s, e) => { ValidateResolution(); };
            comboxControlnetSlot.SelectedIndexChanged += (s, e) => ControlnetSlotChanged();
            comboxModelArch.SelectedIndexChanged += (s, e) => { SetVisibility(new[] { panelModel2, panelRefineStart }, Config.Instance.Implementation); };
            tbLoraFilter.TextChanged += (s, e) => FilterLoras();
        }

        public void LoadControls()
        {
            ConfigParser.LoadGuiElement(upDownIterations, ref Config.Instance.Iterations);
            ConfigParser.LoadGuiElement(sliderSteps, ref Config.Instance.Steps);
            ConfigParser.LoadGuiElement(sliderRefinerStart, ref Config.Instance.SdXlRefinerStrength);
            ConfigParser.LoadGuiElement(sliderScale, ref Config.Instance.Scale);
            ConfigParser.LoadGuiElement(sliderGuidance, ref Config.Instance.Guidance);
            ConfigParser.LoadGuiElement(comboxResW, ref Config.Instance.ResW);
            ConfigParser.LoadGuiElement(comboxResH, ref Config.Instance.ResH);
            ConfigParser.LoadComboxIndex(comboxSampler, ref Config.Instance.SamplerIdx);
            ConfigParser.LoadGuiElement(sliderInitStrength, ref Config.Instance.InitStrength);
            ConfigParser.LoadGuiElement(checkboxHiresFix, ref Config.Instance.HiresFix);

            SetLoras(Config.Instance.LoraWeights, false);
        }

        public void SaveControls()
        {
            ControlnetSlotChanged();

            ConfigParser.SaveGuiElement(upDownIterations, ref Config.Instance.Iterations);
            ConfigParser.SaveGuiElement(sliderSteps, ref Config.Instance.Steps);
            ConfigParser.SaveGuiElement(sliderRefinerStart, ref Config.Instance.SdXlRefinerStrength);
            ConfigParser.SaveGuiElement(sliderScale, ref Config.Instance.Scale);
            ConfigParser.SaveGuiElement(comboxResW, ref Config.Instance.ResW);
            ConfigParser.SaveGuiElement(comboxResH, ref Config.Instance.ResH);
            ConfigParser.SaveComboxIndex(comboxSampler, ref Config.Instance.SamplerIdx);
            ConfigParser.SaveGuiElement(sliderInitStrength, ref Config.Instance.InitStrength);
            ConfigParser.SaveGuiElement(checkboxHiresFix, ref Config.Instance.HiresFix);

            if (Config.Instance != null && comboxModel.SelectedIndex >= 0)
            {
                var arch = ParseUtils.GetEnum<ModelArch>(comboxModelArch.Text, true, Strings.ModelArch);
                Config.Instance.ModelSettings.GetPopulate(((Model)comboxModel.SelectedItem).Name, new Models.ModelSettings()).Arch = arch;
                Config.Instance.ModelSettings.Get(((Model)comboxModel.SelectedItem).Name).ClipSkip = comboxClipSkip.GetInt();

            }
            //Config.Instance.ModelSettings[((Model)comboxModel.SelectedItem).Name].Arch = ParseUtils.GetEnum<ModelArch>(comboxModelArch.Text, true, Strings.ModelArch);

            Config.Instance.LoraWeights = new EasyDict<string, List<float>>(GetLoras(false).Where(x => x.Value.Count != 1 || x.Value[0] != Constants.Ui.DefaultLoraStrength).ToDictionary(p => p.Key, p => p.Value));
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

            UpdateModel();
            ModelChanged();

            // Panel visibility
            SetVisibility(new Control[] { panelBaseImg, panelPromptNeg, panelEmbeddings, panelRefineStart, panelInitImgStrength, panelInpainting, panelScaleImg, panelGuidance, panelRes, panelSampler, panelSeamless, checkboxHiresFix,
                textboxClipsegMask, panelResizeGravity, comboxControlnetSlot, labelResChange, btnResetRes, checkboxShowInitImg, panelModel, panelLoras, panelModel2, panelUpscaling, panelControlnet, panelModelSettings }, imp);

            bool adv = Config.Instance.AdvancedUi;
            upDownIterations.Maximum = !adv ? Config.IniInstance.IterationsMax : Config.IniInstance.IterationsMax * 10;
            sliderSteps.ActualMaximum = !adv ? Config.IniInstance.StepsMax : Config.IniInstance.StepsMax * 4;
            sliderScale.ActualMaximum = (decimal)(!adv ? Config.IniInstance.ScaleMax : Config.IniInstance.ScaleMax * 2);
            int resMax = !adv ? Config.IniInstance.ResolutionMax : Config.IniInstance.ResolutionMax * 2;
            var validResolutions = MainUi.GetResolutions(Config.IniInstance.ResolutionMin, resMax).Select(i => i.ToString());
            comboxResW.SetItems(validResolutions, UiExtensions.SelectMode.Retain, UiExtensions.SelectMode.First);
            comboxResH.SetItems(validResolutions, UiExtensions.SelectMode.Retain, UiExtensions.SelectMode.First);

            TtiUtils.CleanInitImageList();

            btnInitImgBrowse.Text = AnyInits ? $"Clear Image{(MainUi.CurrentInitImgPaths.Count == 1 ? "" : "s")}" : "Load Image(s)";
            labelCurrentImage.Text = !AnyInits ? "No image(s) loaded." : (MainUi.CurrentInitImgPaths.Count == 1 ? $"{IoUtils.GetImage(MainUi.CurrentInitImgPaths[0]).Size.AsString()} Image: {Path.GetFileName(MainUi.CurrentInitImgPaths[0])}" : $"{MainUi.CurrentInitImgPaths.Count} Images");
            toolTip.SetToolTip(labelCurrentImage, $"{labelCurrentImage.Text.Trunc(100)}\n\nShift + Hover to preview.");

            ImageViewer.UpdateInitImgViewer();
            UpdateImgViewerBtns();
            ResolutionChanged();
            ReloadLoras();
            ReloadEmbeddings();
            ReloadControlnets();
            RefreshUpscaleUi();
            CategoryPanels.Keys.ToList().ForEach(btn => btn.Parent.SetVisible(CategoryPanels[btn].Any(p => p.Visible))); // Hide collapse buttons if their category has 0 visible panels
             

            if (imp != _lastImplementation)
            {
                var res = new Size();

                if (imp == Implementation.Comfy)
                    res = Models.GetDefaultRes(ParseUtils.GetEnum<ModelArch>(comboxModelArch.Text, stringMap: Strings.ModelArch));
                else if (imp == Implementation.DiffusersOnnx)
                    res = new Size(512, 512);
                else if (imp == Implementation.InvokeAi)
                    res = comboxModelArch.Text.Contains("768") ? new Size(768, 768) : new Size(512, 512);

                if (!res.IsEmpty)
                {
                    comboxResW.Text = res.Width.ToString();
                    comboxResH.Text = res.Height.ToString();
                }
            }

            _lastImplementation = imp;
        }

        public void UpdateRunBtnState(bool? busy = null)
        {
            if (busy == null)
                busy = Program.State == Program.BusyState.ImageGeneration;

            runBtn.Text = busy == true ? "Cancel" : "Generate!";
            runBtn.ForeColor = busy == true ? Color.IndianRed : Color.White;
        }

        private void ValidateResolution(ComboBox box = null)
        {
            if (box == null || box == comboxResW)
            {
                int w = comboxResW.Text.GetInt();
                w = w.Clamp(256, 8192).RoundMod(MainUi.CurrentModulo);
                comboxResW.Text = w.ToString();
            }

            if (box == null || box == comboxResH)
            {
                int h = comboxResH.Text.GetInt();
                h = h.Clamp(256, 8192).RoundMod(MainUi.CurrentModulo);
                comboxResH.Text = h.ToString();
            }

            if (updownUpscaleFactor.Visible)
            {
                float factor = (float)updownUpscaleFactor.Value;
                updownUpscaleResultH.SetValue((comboxResH.GetInt() * factor).RoundToInt().RoundMod(MainUi.CurrentModulo));
                updownUpscaleResultW.SetValue((comboxResW.GetInt() * factor).RoundToInt().RoundMod(MainUi.CurrentModulo));
            }
        }

        private string _prevSelectedModel = "";

        private void ModelChanged()
        {
            Model mdl = (Model)comboxModel.SelectedItem;

            if (mdl == null || mdl.FullName == _prevSelectedModel)
                return;

            _prevSelectedModel = mdl.FullName;
            var exclusionList = new List<ModelArch>();

            var validImpls = new[] { Implementation.InvokeAi, Implementation.Comfy }; // Only enable selection for these implementations

            if (!validImpls.Contains(Config.Instance.Implementation) || mdl.Format == Enums.Models.Format.Diffusers || mdl.Format == Enums.Models.Format.DiffusersOnnx)
                exclusionList = Enum.GetValues(typeof(ModelArch)).Cast<ModelArch>().Skip(1).ToList();

            comboxModelArch.FillFromEnum<ModelArch>(Strings.ModelArch, 0, exclusionList);

            var modelSettings = Config.Instance.ModelSettings.GetPopulate(mdl.Name, new Models.ModelSettings() { Arch = Models.AssumeModelArch(mdl.Name) });
            comboxModelArch.SetWithText(Config.Instance.ModelSettings[mdl.Name].Arch.ToString(), false, Strings.ModelArch);
            comboxClipSkip.SelectedIndex = modelSettings.ClipSkip;
            SetVisibility(panelModel2);

            ConfigParser.SaveGuiElement(comboxModel, ref Config.Instance.Model);

            if (comboxModel2.Visible)
            {
                string preferredRefinerName = comboxModel.Text.Lower().Replace("base", "refiner");

                if (comboxModel2.Items.Cast<Model>().Any(m => m.ToString().Lower() == preferredRefinerName))
                    comboxModel2.Text = preferredRefinerName;
            }
        }

        private string _lastEmbeddings = "";

        public void ReloadEmbeddings()
        {
            var embeddings = Models.GetEmbeddings();
            string currEmbeddings = embeddings.Select(l => l.FormatIndependentName).AsString();
            IEnumerable<string> embeddingNames = embeddings.Select(m => m.FormatIndependentName);
            comboxEmbeddingList.SetItems(new[] { Constants.NoneMdl }.Concat(embeddingNames), UiExtensions.SelectMode.Retain);

            if (currEmbeddings != _lastEmbeddings)
                embeddings = ValidateEmbeddingNames(embeddings);

            _lastEmbeddings = embeddings.Select(l => l.FormatIndependentName).AsString();
            SetVisibility(panelEmbeddings);
        }

        private string _lastLoras = "";

        public void ReloadLoras()
        {
            string defaultStrength = Constants.Ui.DefaultLoraStrength.ToStringDot("0.0##");
            var selection = GetLoras(); // Save current selection
            List<Model> loras = Models.GetLoras();
            string currLoras = loras.Select(l => l.FormatIndependentName).AsString();

            if (currLoras != _lastLoras)
                loras = ValidateLoraNames(loras);

            _lastLoras = loras.Select(l => l.FormatIndependentName).AsString();

            if (!loras.Any())
                return;

            FilterLoras();
            var previousData = gridLoras.Rows.Cast<DataGridViewRow>().Select(row => (bool)row.Cells[0].Value + (string)row.Cells[1].Value + (string)row.Cells[2].Value).ToList();
            var previousLoraList = gridLoras.Rows.Cast<DataGridViewRow>().Select(row => (string)row.Cells[1].Value).ToList();

            if (string.Join("", loras.Select(l => l.FormatIndependentName).OrderBy(l => l)) == string.Join("", previousLoraList.OrderBy(l => l)))
                return;

            gridLoras.Rows.Clear();
            loras.ToList().ForEach(l => gridLoras.Rows.Add(false, l.FormatIndependentName, defaultStrength));
            SetLoras(selection); // Restore selection
            SetVisibility(panelLoras);
        }

        public void FilterLoras ()
        {
            string filter = tbLoraFilter.Text.Lower();

            foreach (DataGridViewRow row in gridLoras.Rows)
            {
                row.Visible = row.Cells[1].Value != null && row.Cells[1].Value.ToString().Lower().Contains(filter);
            }
        }

        public void SortLoras(bool force = false)
        {
            if (gridLoras.RowCount < 2)
                return;

            var sortedRows = gridLoras.GetRows().OrderBy(r => $"{!((bool)r.Cells[0].Value)}{r.Cells[1].Value}").ToList();
            gridLoras.SetRows(sortedRows);
        }

        private string _lastControlnets = "";

        public void ReloadControlnets()
        {
            var controlnets = Models.GetControlNets();
            string currEmbeddings = controlnets.Select(l => l.FormatIndependentName).AsString();
            IEnumerable<string> names = controlnets.Select(m => m.FormatIndependentName);
            comboxControlnet.SetItems(new[] { Constants.NoneMdl }.Concat(names), UiExtensions.SelectMode.Retain);

            // if (currEmbeddings != _lastControlnets)
            //     controlnets = ValidateEmbeddingNames(controlnets);

            _lastControlnets = controlnets.Select(l => l.FormatIndependentName).AsString();
            SetVisibility(panelEmbeddings);
        }

        private void ResolutionChanged()
        {
            SetVisibility(new Control[] { checkboxHiresFix, labelResChange }, Config.Instance.Implementation);

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
                int GreatestCommonDivisor(int a, int b) => b == 0 ? a : GreatestCommonDivisor(b, a % b);
                int gcd = GreatestCommonDivisor(w, h);
                string ratioText = $"{w / gcd}:{h / gcd}";
                float megapixels = w * h / 1000000f;
                string mpStr = $"{megapixels.ToString("0.#")} MP";
                labelAspectRatio.Text = ratioText.Length <= 5 ? $"{mpStr}, {ratioText.Replace("8:5", "8:5 (16:10)").Replace("7:3", "7:3 (21:9)")}" : mpStr;
            }

            RefreshUpscaleUi();
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
                    bool enablePostProc = new[] { Implementation.InvokeAi, Implementation.Comfy }.Contains(TextToImage.CurrentTaskSettings.Implementation);
                    reGenerateImageWithCurrentSettingsToolStripMenuItem.Visible = !Program.Busy;
                    useAsInitImageToolStripMenuItem.Visible = !Program.Busy;
                    postProcessImageToolStripMenuItem.Visible = !Program.Busy && enablePostProc;
                    copyImageToClipboardToolStripMenuItem.Visible = pictBoxImgViewer.Image != null;
                    fitWindowSizeToImageSizeToolStripMenuItem.Visible = MainUi.GetPreferredSize() != Size.Empty;
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
                List<Panel> panels = CategoryPanels[collapseBtn];
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

        public void UpdateModel(Implementation imp = (Implementation)(-1))
        {
            if (!comboxModel.Visible)
                return;

            if (imp == (Implementation)(-1))
                imp = Config.Instance.Implementation;

            if (imp == (Implementation)(-1))
                return;

            ReloadModelsCombox(imp);
            comboxModel.Text = Config.Instance.Model;
            comboxModel.InitCombox(0);

            if (comboxModel2.Visible)
            {
                comboxModel2.Text = Config.Instance.ModelAux;
                comboxModel2.InitCombox(0);
            }

            if (comboxVae.Visible)
            {
                comboxVae.Text = Config.Instance.ModelVae;
                comboxVae.InitCombox(0);
            }
        }

        public void ReloadModelsCombox(Implementation imp = (Implementation)(-1))
        {
            if (imp == (Implementation)(-1))
                imp = Config.Instance.Implementation;

            IEnumerable<Model> models = Models.GetModels((Enums.Models.Type)(-1), imp);
            comboxModel.SetItems(models.Where(m => m.Type == Enums.Models.Type.Normal), UiExtensions.SelectMode.Retain, UiExtensions.SelectMode.None);
            comboxVae.SetItems(new[] { new Model() }.Concat(Models.GetVaes().Where(m => m.Type == Enums.Models.Type.Vae)), UiExtensions.SelectMode.Retain, UiExtensions.SelectMode.None);

            if (imp == Implementation.Comfy)
                comboxModel2.SetItems(models.Where(m => m.Type == Enums.Models.Type.Refiner), UiExtensions.SelectMode.Retain, UiExtensions.SelectMode.None);
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

        public void ToggleSaveMode()
        {
            Config.Instance.AutoDeleteImgs = !Config.Instance.AutoDeleteImgs;
            UpdateSaveModeBtn();
        }

        public void UpdateSaveModeBtn()
        {
            if (Config.Instance.AutoDeleteImgs && btnSaveMode.BackgroundImage != Resources.IconArchiveOff)
            {
                btnSaveMode.BackgroundImage = Resources.IconArchiveOff;
                toolTip.SetToolTip(btnSaveMode, "Auto-Delete is enabled: Generated images will be deleted as soon as another batch is generated or the program closes.\n\nClick to disable.");
            }
            else if (!Config.Instance.AutoDeleteImgs && btnSaveMode.BackgroundImage != Resources.IconArchiveOn)
            {
                btnSaveMode.BackgroundImage = Resources.IconArchiveOn;
                toolTip.SetToolTip(btnSaveMode, "Auto-Delete is disabled: All generated images will be saved.\n\nClick to switch to Auto-Delete mode.");
            }
        }

        public Size GetUpscaleTargetSize()
        {
            int w = comboxResW.GetInt();
            int h = comboxResH.GetInt();

            if (comboxUpscaleMode.SelectedIndex <= 0) // Upscaling disabled
                return new Size(w, h);

            if (updownUpscaleFactor.Visible)
            {
                w = (w * (float)updownUpscaleFactor.Value).RoundToInt().RoundMod(CurrentModulo);
                h = (h * (float)updownUpscaleFactor.Value).RoundToInt().RoundMod(CurrentModulo);
            }
            else
            {
                w = (int)updownUpscaleResultW.Value;
                h = (int)updownUpscaleResultH.Value;
            }

            return new Size(w, h);
        }

        private void RefreshUpscaleUi(bool stopUi = false)
        {
            stopUi = stopUi & this.IsRendering();

            if (stopUi)
                this.StopRendering();

            try
            {
                UpscaleMode mode = ParseUtils.GetEnum<UpscaleMode>(comboxUpscaleMode.Text, true, Strings.UpscaleModes);
                new Control[] { labelUpscale, updownUpscaleResultW, updownUpscaleResultH, labelUpscaleX }.SetVisible(mode != UpscaleMode.Disabled);
                bool factor = mode == UpscaleMode.LatentsFactor || mode == UpscaleMode.UltimeUpsFactor;
                labelUpscale.Text = factor ? "Factor:" : "Target Resolution:";
                updownUpscaleFactor.SetVisible(factor);
                labelUpscaleEquals.SetVisible(factor);
                updownUpscaleResultW.Enabled = mode == UpscaleMode.LatentsTargetRes;
                updownUpscaleResultH.Enabled = mode == UpscaleMode.LatentsTargetRes;
                ValidateResolution();

                if (mode == UpscaleMode.Disabled)
                {
                    updownUpscaleResultW.Value = comboxResW.GetInt().Clamp((int)updownUpscaleResultW.Minimum, (int)updownUpscaleResultW.Maximum);
                    updownUpscaleResultH.Value = comboxResH.GetInt().Clamp((int)updownUpscaleResultW.Minimum, (int)updownUpscaleResultW.Maximum);
                }
            }
            catch { }

            if (stopUi)
                this.ResumeRendering();
        }

        public ComfyData.ControlnetInfo[] Controlnets = new ComfyData.ControlnetInfo[Constants.Ui.ControlnetSlots];
        private int _previousControlnetSlot = 0;
        private bool _ignoreControlnetSlotChanged = false;

        private void ControlnetSlotChanged(bool allowSaving = true)
        {
            if (_ignoreControlnetSlotChanged)
            {
                _ignoreControlnetSlotChanged = false;
                return;
            }

            int idxOld = _previousControlnetSlot;
            int idxNew = comboxControlnetSlot.SelectedIndex;

            // Store settings in current slot
            if (allowSaving)
            {
                var preproc = ParseUtils.GetEnum<ImagePreprocessor>(comboxPreprocessor.Text, stringMap: Strings.ImagePreprocessors);
                Controlnets[idxOld] = new ComfyData.ControlnetInfo { Model = comboxControlnet.Text, Preprocessor = preproc, Strength = (float)updownControlnetStrength.Value };
            }

            if (Controlnets[idxNew] != null) // Load from slot if it's not empty
            {
                comboxControlnet.Text = Controlnets[idxNew].Model;
                comboxPreprocessor.SetWithText(Controlnets[idxNew].Preprocessor.ToString(), stringMap: Strings.ImagePreprocessors);
                updownControlnetStrength.Value = (decimal)Controlnets[idxNew].Strength;
            }
            else // Reset if slot empty
            {
                comboxControlnet.SelectedIndex = 0;
                comboxPreprocessor.SelectedIndex = 0;
                updownControlnetStrength.Value = (decimal)Constants.Ui.DefaultControlnetStrength;
            }

            _previousControlnetSlot = idxNew;

            var newStrings = Enumerable.Range(1, Controlnets.Length).Select(i => $"Slot {i}{(Controlnets[i - 1] == null ? "" : $": {Controlnets[i - 1].Model}")}");
            var oldStrings = comboxControlnetSlot.Items.Cast<string>();
            _ignoreControlnetSlotChanged = string.Join("", newStrings) != string.Join("", oldStrings); // If old and new string lists mismatch, ControlnetSlotChanged() will be triggered, and we need to ignore it once
            comboxControlnetSlot.SetItems(newStrings, comboxControlnetSlot.SelectedIndex);
        }
    }
}
