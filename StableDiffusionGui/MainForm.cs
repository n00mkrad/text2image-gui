using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Taskbar;
using StableDiffusionGui.Controls;
using StableDiffusionGui.Data;
using StableDiffusionGui.Extensions;
using StableDiffusionGui.Forms;
using StableDiffusionGui.Installation;
using StableDiffusionGui.Io;
using StableDiffusionGui.Main;
using StableDiffusionGui.Os;
using StableDiffusionGui.Ui;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using static StableDiffusionGui.Main.Enums.StableDiffusion;
using Paths = StableDiffusionGui.Io.Paths;

namespace StableDiffusionGui
{
    public partial class MainForm : CustomForm
    {
        [Flags]
        public enum EXECUTION_STATE : uint { ES_AWAYMODE_REQUIRED = 0x00000040, ES_CONTINUOUS = 0x80000000, ES_DISPLAY_REQUIRED = 0x00000002, ES_SYSTEM_REQUIRED = 0x00000001 }
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags); // This should prevent Windows from going to sleep

        #region References

        public Button RunBtn { get { return runBtn; } }
        public TextBox TextboxPrompt { get { return textboxPrompt; } }
        public TextBox TextboxPromptNeg { get { return textboxPromptNeg; } }
        public PictureBox PictBoxImgViewer { get { return pictBoxImgViewer; } }
        public Label LabelImgInfo { get { return labelImgInfo; } }
        public Label LabelImgPrompt { get { return labelImgPrompt; } }
        public Label LabelImgPromptNeg { get { return labelImgPromptNeg; } }
        public Button BtnExpandPromptField { get { return btnExpandPromptField; } }
        public Button BtnExpandPromptNegField { get { return btnExpandPromptNegField; } }
        public Panel PanelSettings { get { return panelSettings; } }
        public CustomSlider SliderStrength { get { return sliderInitStrength; } }
        public CustomSlider SliderSteps { get { return sliderSteps; } }
        public CustomSlider SliderScale { get { return sliderScale; } }
        public ComboBox ComboxResW { get { return comboxResW; } }
        public ComboBox ComboxResH { get { return comboxResH; } }
        public ToolTip ToolTip { get { return toolTip; } }

        #endregion

        public bool IsInFocus() { return (ActiveForm == this); }

        public MainForm()
        {
            InitializeComponent();
            ActiveControl = panelSettings;
            Program.MainForm = this;
            Opacity = 0;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            AllowEscClose = false;
            Logger.Textbox = logBox;
            MinimumSize = Size;
            Text = $"Stable Diffusion GUI {Program.Version}";
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveUiElements();

            if (Program.Busy)
            {
                DialogResult dialogResult = UiUtils.ShowMessageBox($"The program is still busy. Are you sure you want to quit?", UiUtils.MessageType.Warning.ToString(), MessageBoxButtons.YesNo);
                e.Cancel = dialogResult != DialogResult.Yes;
            }
        }

        private async void MainForm_Shown(object sender, EventArgs e)
        {
            Refresh();
            await Initialize();
        }

        private async Task Initialize()
        {
            MainUi.SetSettingsVertScrollbar();
            InitializeControls();
            RefreshAfterSettingsChanged();
            LoadUiElements();
            PromptHistory.Load();
            Setup.PatchFiles();

            pictBoxImgViewer.MouseWheel += (s, e) => { ImagePreview.Move(e.Delta > 0); }; // Scroll on MouseWheel
            comboxResW.SelectedIndexChanged += (s, e) => { MainUi.SetHiresFixVisible(ComboxResW, ComboxResH, checkboxHiresFix); }; // Show/Hide HiRes Fix depending on chosen res
            comboxResH.SelectedIndexChanged += (s, e) => { MainUi.SetHiresFixVisible(ComboxResW, ComboxResH, checkboxHiresFix); }; // Show/Hide HiRes Fix depending on chosen res

            MainUi.LoadAutocompleteData(promptAutocomplete, new[] { textboxPrompt, textboxPromptNeg });
            Task.Run(() => MainUi.SetGpusInWindowTitle());
            upDownSeed.Text = "";
            MainUi.DoStartupChecks();
            UpdateInitImgAndEmbeddingUi();

            TabOrderInit(new List<Control>() {
                textboxPrompt, textboxPromptNeg,
                sliderInitStrength, textboxSliderInitStrength,
                checkboxInpainting,
                upDownIterations,
                sliderSteps, textboxSliderSteps,
                sliderScale, textboxSliderScale,
                upDownSeed, checkboxLockSeed,
                comboxResW, comboxResH, checkboxHiresFix,
                comboxSampler,
                comboxSeamless,
                runBtn
            }, false);

            await Task.Delay(1); // Don't ask. Just keep it here
            Opacity = 1.0;

            if (!Program.Debug)
                new WelcomeForm().ShowDialogForm(0f);

            panelDebugSendStdin.Visible = Program.Debug;
            panelDebugPerlinThresh.Visible = Program.Debug;
        }

        private void InitializeControls()
        {
            comboxSampler.FillFromEnum<Sampler>(Strings.MainUiStrings);
            comboxSeamless.FillFromEnum<SeamlessMode>(Strings.MainUiStrings, 0);
            comboxInpaintMode.FillFromEnum<InpaintMode>(Strings.MainUiStrings, 0);

            var resItems = MainUi.Resolutions.Where(x => x <= (Config.GetBool("checkboxAdvancedMode") ? 2048 : 1024)).Select(x => x.ToString());
            comboxResW.SetItems(resItems, UiExtensions.SelectMode.Last);
            comboxResH.SetItems(resItems, UiExtensions.SelectMode.Last);
        }

        private void LoadUiElements()
        {
            ConfigParser.LoadGuiElement(upDownIterations);
            ConfigParser.LoadGuiElement(sliderSteps);
            ConfigParser.LoadGuiElement(sliderScale);
            ConfigParser.LoadGuiElement(comboxResH);
            ConfigParser.LoadGuiElement(comboxResW);
            ConfigParser.LoadComboxIndex(comboxSampler);
            ConfigParser.LoadGuiElement(sliderInitStrength);
        }

        private void SaveUiElements()
        {
            ConfigParser.SaveGuiElement(upDownIterations);
            ConfigParser.SaveGuiElement(sliderSteps);
            ConfigParser.SaveGuiElement(sliderScale);
            ConfigParser.SaveGuiElement(comboxResH);
            ConfigParser.SaveGuiElement(comboxResW);
            ConfigParser.SaveComboxIndex(comboxSampler);
            ConfigParser.SaveGuiElement(sliderInitStrength);
        }

        public void RefreshAfterSettingsChanged()
        {
            var imp = (Implementation)Config.GetInt("comboxImplementation");
            panelPromptNeg.Visible = imp != Implementation.OptimizedSd;
            btnEmbeddingBrowse.Enabled = imp == Implementation.InvokeAi;
            panelSampler.Visible = imp == Implementation.InvokeAi;
            panelSeamless.Visible = imp == Implementation.InvokeAi;

            bool adv = Config.GetBool("checkboxAdvancedMode");
            upDownIterations.Maximum = !adv ? 10000 : 100000;
            sliderSteps.ActualMaximum = !adv ? 120 : 500;
            sliderSteps.ValueStep = !adv ? 5 : 1;
            sliderScale.ActualMaximum = !adv ? 25 : 50;
            comboxResW.SetItems(MainUi.Resolutions.Where(x => x <= (adv ? 2048 : 1024)).Select(x => x.ToString()), UiExtensions.SelectMode.Retain, UiExtensions.SelectMode.Last);
            comboxResH.SetItems(MainUi.Resolutions.Where(x => x <= (adv ? 2048 : 1024)).Select(x => x.ToString()), UiExtensions.SelectMode.Retain, UiExtensions.SelectMode.Last);
        }

        private void installerBtn_Click(object sender, EventArgs e)
        {
            new InstallerForm().ShowDialogForm();
        }

        public void CleanPrompt()
        {
            if (File.Exists(MainUi.CurrentEmbeddingPath) && Path.GetExtension(MainUi.CurrentEmbeddingPath).Lower() == ".bin")
            {
                string conceptName = Path.GetFileNameWithoutExtension(MainUi.CurrentEmbeddingPath);
                textboxPrompt.Text = textboxPrompt.Text.Replace("*", $"<{conceptName.Trim()}>");
            }

            textboxPrompt.Text = string.Join(Environment.NewLine, textboxPrompt.Text.SplitIntoLines().Select(x => MainUi.SanitizePrompt(x)));
            textboxPromptNeg.Text = MainUi.SanitizePrompt(textboxPromptNeg.Text);

            if (upDownSeed.Text == "")
                SetSeed();
        }

        public void SetSeed(long seed = -1)
        {
            upDownSeed.Value = seed;

            if (seed < 0)
                upDownSeed.Text = "";
        }

        public void LoadMetadataIntoUi(ImageMetadata meta)
        {
            textboxPrompt.Text = meta.Prompt;
            textboxPromptNeg.Text = meta.NegativePrompt;
            sliderSteps.ActualValue = meta.Steps;
            sliderScale.ActualValue = (decimal)meta.Scale;
            comboxResW.Text = meta.GeneratedResolution.Width.ToString();
            comboxResH.Text = meta.GeneratedResolution.Height.ToString();
            upDownSeed.Value = meta.Seed;
            comboxSampler.SetIfTextMatches(meta.Sampler, true, Strings.MainUiStrings);
            // MainUi.CurrentInitImgPaths = new[] { meta.InitImgName }.Where(x => string.IsNullOrWhiteSpace(x)).ToList(); // Does this even work if we only store the temp path?
            MainUi.CurrentInitImgPaths = null;
            comboxSeamless.SelectedIndex = meta.Seamless ? 1 : 0; // TODO: Extend Metadata class to include seamless mode

            if (meta.InitStrength > 0f)
                sliderInitStrength.ActualValue = (decimal)meta.InitStrength;

            UpdateInitImgAndEmbeddingUi();
        }

        public void LoadTtiSettingsIntoUi(string[] prompts, string negPrompt = "")
        {
            textboxPrompt.Text = string.Join(Environment.NewLine, prompts);

            if (!string.IsNullOrWhiteSpace(negPrompt))
                textboxPromptNeg.Text = negPrompt;
        }

        public void LoadTtiSettingsIntoUi(TtiSettings s)
        {
            textboxPrompt.Text = string.Join(Environment.NewLine, s.Prompts);
            textboxPromptNeg.Text = s.NegativePrompt;
            upDownIterations.Value = s.Iterations;

            try
            {
                sliderSteps.ActualValue = s.Params.Get("steps").FromJson<int>();
                sliderScale.ActualValue = (decimal)s.Params.Get("scales").FromJson<List<float>>().FirstOrDefault();
                comboxResW.Text = s.Params.Get("res").FromJson<Size>().Width.ToString();
                comboxResH.Text = s.Params.Get("res").FromJson<Size>().Height.ToString();
                upDownSeed.Value = s.Params.Get("seed").FromJson<long>();
                comboxSampler.SetIfTextMatches(s.Params.Get("sampler").FromJson<string>(), true, Strings.MainUiStrings);
                MainUi.CurrentInitImgPaths = s.Params.Get("initImgs").FromJson<List<string>>();
                sliderInitStrength.ActualValue = (decimal)s.Params.Get("initStrengths").FromJson<List<float>>().FirstOrDefault();
                MainUi.CurrentEmbeddingPath = s.Params.Get("embedding").FromJson<string>();
                comboxSeamless.SetIfTextMatches(s.Params.Get("seamless").FromJson<string>(), true, Strings.MainUiStrings);
                checkboxInpainting.Checked = s.Params.Get("inpainting").FromJson<InpaintMode>() == InpaintMode.ImageMask;
                checkboxHiresFix.Checked = s.Params.Get("hiresFix").FromJson<bool>();
                checkboxLockSeed.Checked = s.Params.Get("lockSeed").FromJson<bool>();
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to load generation settings. This can happen when you try to load prompts from an older version. ({ex.Message})");
                Logger.Log(ex.StackTrace, true);
            }


            UpdateInitImgAndEmbeddingUi();
        }

        public TtiSettings GetCurrentTtiSettings()
        {
            TtiSettings settings = new TtiSettings
            {
                Implementation = (Implementation)Config.GetInt("comboxImplementation"),
                Prompts = textboxPrompt.TextNoPlaceholder.SplitIntoLines().Where(x => !string.IsNullOrWhiteSpace(x)).ToArray(),
                NegativePrompt = textboxPromptNeg.TextNoPlaceholder.Trim().Replace(Environment.NewLine, " "),
                Iterations = (int)upDownIterations.Value,
                Params = new Dictionary<string, string>
                        {
                            { "steps", SliderSteps.ActualValueInt.ToJson() },
                            { "scales", MainUi.GetScales(textboxExtraScales.Text).ToJson() },
                            { "res", new Size(ComboxResW.Text.GetInt(), ComboxResH.Text.GetInt()).ToJson() },
                            { "seed", (upDownSeed.Value < 0 ? new Random().Next(0, int.MaxValue) : ((long)upDownSeed.Value)).ToJson() },
                            { "sampler", ((Sampler)comboxSampler.SelectedIndex).ToString().Lower().ToJson() },
                            { "initImgs", MainUi.CurrentInitImgPaths.ToJson() },
                            { "initStrengths", MainUi.GetInitStrengths(textboxExtraInitStrengths.Text).ToJson() },
                            { "embedding", MainUi.CurrentEmbeddingPath.ToJson() },
                            { "seamless", ((SeamlessMode)comboxSeamless.SelectedIndex).ToJson() },
                            { "inpainting", ((InpaintMode)comboxInpaintMode.SelectedIndex).ToJson() },
                            { "clipSegMask", textboxClipsegMask.Text.Trim().ToJson() },
                            { "model", Config.Get(Config.Key.comboxSdModel).ToJson() },
                            { "hiresFix", checkboxHiresFix.Checked.ToJson() },
                            { "lockSeed", checkboxLockSeed.Checked.ToJson() },
                            { "vae", Config.Get(Config.Key.comboxSdModelVae).ToJson() },
                            { "perlin", textboxPerlin.GetFloat().ToJson() },
                            { "threshold", textboxThresh.GetInt().ToJson() },
                        },
            };

            return settings;
        }

        private void runBtn_Click(object sender, EventArgs e)
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
                Run();
            }
        }

        public void Run(bool fromQueue = false)
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
                    CleanPrompt();
                    UpdateInitImgAndEmbeddingUi();
                    InpaintingUtils.DeleteMaskedImage();

                    if (fromQueue)
                    {
                        if (MainUi.Queue.Where(x => x != null).Count() < 0)
                        {
                            TextToImage.Cancel("Queue is empty.");
                            return;
                        }

                        TextToImage.RunTti(MainUi.Queue.AsEnumerable().Reverse().ToList()); // Reverse list to use top entries first
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(textboxPrompt.Text))
                        {
                            TextToImage.Cancel("No prompt was entered.");
                            return;
                        }

                        TextToImage.RunTti(GetCurrentTtiSettings());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"{ex.Message}\n{ex.StackTrace}");
            }
        }

        public void SetWorking(Program.BusyState state, bool allowCancel = true)
        {
            Logger.Log($"SetWorking({state})", true);
            Program.State = state;
            SetProgress(-1);

            bool imageGen = state == Program.BusyState.ImageGeneration;

            runBtn.Text = imageGen ? "Cancel" : "Generate!";
            runBtn.ForeColor = imageGen ? Color.IndianRed : Color.White;
            Control[] controlsToDisable = new Control[] { };
            Control[] controlsToHide = new Control[] { };
            progressCircle.Visible = state != Program.BusyState.Standby;

            foreach (Control c in controlsToDisable)
                c.Enabled = !imageGen;

            foreach (Control c in controlsToHide)
                c.Visible = !imageGen;

            if (!imageGen)
                SetProgressImg(0);

            progressBarImg.Visible = imageGen;
        }

        public void SetProgress(int percent, bool taskbarProgress = true)
        {
            percent = percent.Clamp(0, 100);
            progressBar.Value = percent;
            progressBar.Refresh();

            if (taskbarProgress)
                TaskbarManager.Instance.SetProgressValue(percent, 100);
        }

        public void SetProgressImg(int percent, bool taskbarProgress = false)
        {
            percent = percent.Clamp(0, 100);
            progressBarImg.Value = percent;
            progressBarImg.Refresh();

            if (taskbarProgress)
                TaskbarManager.Instance.SetProgressValue(percent, 100);
        }

        private void btnPrevImg_Click(object sender, EventArgs e)
        {
            ImagePreview.Move(true);
        }

        private void btnNextImg_Click(object sender, EventArgs e)
        {
            ImagePreview.Move(false);
        }

        private void btnOpenOutFolder_Click(object sender, EventArgs e)
        {
            Process.Start("explorer", Config.Get(Config.Key.textboxOutPath));
        }

        #region Link Buttons

        private void paypalBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.paypal.com/paypalme/nmkd/8");
        }

        private void patreonBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://patreon.com/n00mkrad");
        }

        private void discordBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/fZwWSnV5WA");
        }

        #endregion

        #region Output Image Menu Strip

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImagePreview.OpenCurrent();
        }

        private void openOutputFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImagePreview.OpenFolderOfCurrent();
        }

        private void copyImageToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OsUtils.SetClipboard(pictBoxImgViewer.Image);
        }

        private void copySeedToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OsUtils.SetClipboard(ImagePreview.CurrentImageMetadata.Seed.ToString());
        }

        private void useAsInitImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.Busy)
            {
                UiUtils.ShowMessageBox("Please wait until the generation has finished.");
                return;
            }

            MainUi.HandleDroppedFiles(new string[] { ImagePreview.CurrentImagePath });
        }

        private void copyToFavoritesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImagePreview.CopyCurrentToFavs();
        }

        private void postProcessImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPostProcessMenu();
        }

        #endregion

        private void cliButton_Click(object sender, EventArgs e)
        {
            menuStripDevTools.Show(Cursor.Position);
        }

        private void pictBoxImgViewer_Click(object sender, EventArgs e)
        {
            pictBoxImgViewer.Focus();

            if (((MouseEventArgs)e).Button == MouseButtons.Right)
            {
                if (!string.IsNullOrWhiteSpace(ImagePreview.CurrentImagePath) && File.Exists(ImagePreview.CurrentImagePath))
                {
                    reGenerateImageWithCurrentSettingsToolStripMenuItem.Visible = !Program.Busy;
                    useAsInitImageToolStripMenuItem.Visible = !Program.Busy;
                    postProcessImageToolStripMenuItem.Visible = !Program.Busy && TextToImage.CurrentTaskSettings.Implementation == Implementation.InvokeAi;
                    menuStripOutputImg.Show(Cursor.Position);
                }
            }
            else
            {
                if (pictBoxImgViewer.Image != null)
                    ImagePopup.Show(pictBoxImgViewer.Image, ImagePopupForm.SizeMode.Percent100);
            }
        }

        #region Drag N Drop

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            MainUi.HandleDroppedFiles((string[])e.Data.GetData(DataFormats.FileDrop));
        }

        #endregion

        #region Init Img and Embedding

        private void btnInitImgBrowse_Click(object sender, EventArgs e)
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

            UpdateInitImgAndEmbeddingUi();
        }

        public void UpdateInitImgAndEmbeddingUi()
        {
            TtiUtils.CleanInitImageList();

            if (!string.IsNullOrWhiteSpace(MainUi.CurrentEmbeddingPath) && !File.Exists(MainUi.CurrentEmbeddingPath))
            {
                MainUi.CurrentEmbeddingPath = "";
                Logger.Log($"Concept was cleared because the file no longer exists.");
            }

            bool img2img = MainUi.CurrentInitImgPaths != null;
            panelInpainting.Visible = img2img;
            panelInitImgStrength.Visible = img2img;
            btnInitImgBrowse.Text = img2img ? $"Clear Image{(MainUi.CurrentInitImgPaths.Count == 1 ? "" : "s")}" : "Load Image(s)";

            bool embeddingExists = File.Exists(MainUi.CurrentEmbeddingPath);
            btnEmbeddingBrowse.Text = embeddingExists ? "Clear Concept" : "Load Concept";

            labelCurrentImage.Text = !img2img ? "No initialization image loaded." : (MainUi.CurrentInitImgPaths.Count == 1 ? $"Currently using {Path.GetFileName(MainUi.CurrentInitImgPaths[0]).Trunc(30)}" : $"Currently using {MainUi.CurrentInitImgPaths.Count} images.");
            labelCurrentConcept.Text = string.IsNullOrWhiteSpace(MainUi.CurrentEmbeddingPath) ? "No trained concept loaded." : $"Currently using {Path.GetFileName(MainUi.CurrentEmbeddingPath).Trunc(30)}";

            RefreshAfterSettingsChanged();
        }

        private void btnEmbeddingBrowse_Click(object sender, EventArgs e)
        {
            if (Program.Busy)
                return;

            if ((Implementation)Config.GetInt("comboxImplementation") == Implementation.OptimizedSd)
            {
                Logger.Log("Not supported in Low Memory Mode.");
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

            UpdateInitImgAndEmbeddingUi();
        }

        #endregion

        private void btnDebug_Click(object sender, EventArgs e)
        {
            menuStripLogs.Items.Clear();
            var openLogs = menuStripLogs.Items.Add($"Open Logs Folder");
            openLogs.Click += (s, ea) => { Process.Start("explorer", Paths.GetLogPath().Wrap()); };

            foreach (var log in Logger.SessionLogs)
            {
                ToolStripItem newItem = menuStripLogs.Items.Add($"Copy {log.Key}");
                newItem.Click += (s, ea) => { OsUtils.SetClipboard(Logger.SessionLogs[log.Key]); };
            }

            menuStripLogs.Show(Cursor.Position);
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            new SettingsForm().ShowDialogForm(0.5f);
        }

        private void btnPostProc_Click(object sender, EventArgs e)
        {
            if ((Implementation)Config.GetInt("comboxImplementation") == Implementation.OptimizedSd)
            {
                UiUtils.ShowMessageBox("Post-processing is not available when using Low Memory Mode.");
                return;
            }

            new PostProcSettingsForm().ShowDialogForm();
        }

        private void btnExpandPromptField_Click(object sender, EventArgs e)
        {
            MainUi.SetPromptFieldSize(MainUi.PromptFieldSizeMode.Toggle, false);
        }

        private void btnExpandPromptNegField_Click(object sender, EventArgs e)
        {
            MainUi.SetPromptFieldSize(MainUi.PromptFieldSizeMode.Toggle, true);
        }

        private void btnSeedUsePrevious_Click(object sender, EventArgs e)
        {
            upDownSeed.Value = TextToImage.PreviousSeed;
        }

        private void btnSeedResetToRandom_Click(object sender, EventArgs e)
        {
            upDownSeed.Value = -1;
            upDownSeed.Text = "";
        }

        private void btnPromptHistory_Click(object sender, EventArgs e)
        {
            new PromptListForm(PromptListForm.ListMode.History).ShowDialogForm();
        }

        private void btnQueue_Click(object sender, EventArgs e)
        {
            if (Program.Busy)
                return;

            new PromptListForm(PromptListForm.ListMode.Queue).ShowDialogForm();
        }

        private void generateCurrentPromptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Run();
        }

        private void generateAllQueuedPromptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Run(true);
        }

        public void UpdateInpaintUi()
        {
            btnResetMask.Visible = InpaintingUtils.CurrentMask != null;
        }

        private void btnResetMask_Click(object sender, EventArgs e)
        {
            InpaintingUtils.CurrentMask = null;
        }

        private void textboxCliTest_DoubleClick(object sender, EventArgs e)
        {
            TtiProcess.WriteStdIn(textboxCliTest.Text);
            textboxCliTest.Text = "";
        }

        private void addCurrentSettingsToQueueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var settings = GetCurrentTtiSettings();

            if (settings.Prompts.Where(x => !string.IsNullOrWhiteSpace(x)).Any())
                MainUi.Queue.Add(settings);
        }

        private void btnQueue_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                menuStripAddToQueue.Show(Cursor.Position);
        }

        private void reGenerateImageWithCurrentSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.Busy)
            {
                UiUtils.ShowMessageBox("Please wait until the current process has finished.");
                return;
            }

            var prevSeedVal = upDownSeed.Value;
            var prevIterVal = upDownIterations.Value;
            upDownSeed.Value = ImagePreview.CurrentImageMetadata.Seed;
            upDownIterations.Value = 1;
            runBtn_Click(null, null);
            SetSeed((long)prevSeedVal);
            upDownIterations.Value = prevIterVal;
        }

        private void btnDeleteBatch_Click(object sender, EventArgs e)
        {
            menuStripDeleteImages.Show(Cursor.Position);
        }

        private void deleteThisImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImagePreview.DeleteCurrent();
        }

        private void deleteAllCurrentImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImagePreview.DeleteAll();
        }

        private void openDreampyCLIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.Busy || !MainUi.IsInstalledWithWarning())
                return;

            TtiProcess.RunStableDiffusionCli(Config.Get(Config.Key.textboxOutPath), Config.Get(Config.Key.comboxSdModelVae));
        }

        private void openModelMergeToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new MergeModelsForm().ShowDialogForm();
        }

        private void openModelPruningTrimmingToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new PruneModelsForm().ShowDialogForm();
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            panelSettings.Focus();
        }

        private void viewLogInRealtimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new RealtimeLoggerForm().Show();
        }

        private void fitWindowSizeToImageSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainUi.FitWindowSizeToImageSize();
            CenterToScreen();
        }

        private void labelImgPrompt_Click(object sender, EventArgs e)
        {
            OsUtils.SetClipboard(ImagePreview.CurrentImageMetadata.Prompt);
        }

        private void labelImgPromptNeg_Click(object sender, EventArgs e)
        {
            OsUtils.SetClipboard(ImagePreview.CurrentImageMetadata.NegativePrompt);
        }

        private void openCmdInPythonEnvironmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TtiProcess.StartCmdInSdEnv(false);
        }

        #region Post-Processing

        public void ShowPostProcessMenu()
        {
            menuStripPostProcess.Show(Cursor.Position);
        }

        private async void upscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await InvokeAi.RunFix(ImagePreview.CurrentImagePath, new[] { InvokeAi.FixAction.Upscale }.ToList());
        }

        private async void applyFaceRestorationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await InvokeAi.RunFix(ImagePreview.CurrentImagePath, new[] { InvokeAi.FixAction.FaceRestoration }.ToList());
        }

        private async void applyAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await InvokeAi.RunFix(ImagePreview.CurrentImagePath, new[] { InvokeAi.FixAction.Upscale, InvokeAi.FixAction.FaceRestoration }.ToList());
        }

        #endregion

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            MainUiHotkeys.Handle(e.KeyData);
        }

        private void panelSettings_SizeChanged(object sender, EventArgs e)
        {
            var newPadding = panelSettings.Padding;
            newPadding.Right = panelSettings.VerticalScroll.Visible ? 6 : 0;

            if (panelSettings.Padding.Right != newPadding.Right)
                panelSettings.Padding = newPadding;
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            MainUi.SetSettingsVertScrollbar();
        }

        private async void textboxPrompt_TextChanged(object sender, EventArgs e)
        {
            if (!EnabledFeatures.WildcardAutocomplete)
                return;

            await Task.Delay(1);

            if (textboxPrompt.Text.LastOrDefault() == '~' && promptAutocomplete == null)
            {
                promptAutocomplete = MainUi.ShowAutocompleteMenu((TextBox)FocusedControl);
            }
            else if (textboxPrompt.Text == null || textboxPrompt.Text.Length <= 0 || textboxPrompt.Text.LastOrDefault() == ' ')
            {
                if (promptAutocomplete != null)
                {
                    promptAutocomplete.Close();
                    promptAutocomplete.Dispose();
                    promptAutocomplete = null;
                }
            }
        }

        private void btnDreambooth_Click(object sender, EventArgs e)
        {
            new DreamboothForm().ShowDialogForm();
        }
    }
}
