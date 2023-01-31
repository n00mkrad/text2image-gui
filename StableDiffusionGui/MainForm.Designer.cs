using StableDiffusionGui.Controls;

namespace StableDiffusionGui
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.runBtn = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.logBox = new System.Windows.Forms.TextBox();
            this.labelImgInfo = new System.Windows.Forms.Label();
            this.progressCircle = new CircularProgressBar.CircularProgressBar();
            this.progressBar = new HTAlt.WinForms.HTProgressBar();
            this.menuStripOutputImg = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openOutputFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToFavoritesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyImageToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copySidebySideComparisonImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copySeedToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reGenerateImageWithCurrentSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useAsInitImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.postProcessImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fitWindowSizeToImageSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.labelImgPrompt = new System.Windows.Forms.Label();
            this.labelImgPromptNeg = new System.Windows.Forms.Label();
            this.comboxSampler = new System.Windows.Forms.ComboBox();
            this.upDownSeed = new System.Windows.Forms.NumericUpDown();
            this.sliderScale = new StableDiffusionGui.Controls.CustomSlider();
            this.textboxSliderScale = new System.Windows.Forms.TextBox();
            this.textboxExtraScales = new System.Windows.Forms.TextBox();
            this.upDownIterations = new System.Windows.Forms.NumericUpDown();
            this.btnResetMask = new HTAlt.WinForms.HTButton();
            this.sliderInitStrength = new StableDiffusionGui.Controls.CustomSlider();
            this.textboxSliderInitStrength = new System.Windows.Forms.TextBox();
            this.btnInitImgBrowse = new HTAlt.WinForms.HTButton();
            this.btnEmbeddingBrowse = new HTAlt.WinForms.HTButton();
            this.textboxPrompt = new StableDiffusionGui.Controls.CustomTextbox();
            this.comboxSeamless = new System.Windows.Forms.ComboBox();
            this.textboxClipsegMask = new System.Windows.Forms.TextBox();
            this.textboxExtraSteps = new System.Windows.Forms.TextBox();
            this.btnEditMask = new HTAlt.WinForms.HTButton();
            this.btnDreambooth = new System.Windows.Forms.Button();
            this.btnDeleteBatch = new System.Windows.Forms.Button();
            this.btnPromptHistory = new System.Windows.Forms.Button();
            this.btnQueue = new System.Windows.Forms.Button();
            this.btnPostProc = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnDebug = new System.Windows.Forms.Button();
            this.cliButton = new System.Windows.Forms.Button();
            this.btnOpenOutFolder = new System.Windows.Forms.Button();
            this.btnPrevImg = new System.Windows.Forms.Button();
            this.btnNextImg = new System.Windows.Forms.Button();
            this.installerBtn = new System.Windows.Forms.Button();
            this.sliderScaleImg = new StableDiffusionGui.Controls.CustomSlider();
            this.textboxSliderScaleImg = new System.Windows.Forms.TextBox();
            this.textboxExtraScalesImg = new System.Windows.Forms.TextBox();
            this.textboxPromptNeg = new StableDiffusionGui.Controls.CustomTextbox();
            this.checkboxHiresFix = new System.Windows.Forms.CheckBox();
            this.cbBaW = new System.Windows.Forms.CheckBox();
            this.cbDetFace = new System.Windows.Forms.CheckBox();
            this.cbSepia = new System.Windows.Forms.CheckBox();
            this.labelCurrentImage = new System.Windows.Forms.Label();
            this.menuStripLogs = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewLogInRealtimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.progressBarImg = new HTAlt.WinForms.HTProgressBar();
            this.menuStripRunQueue = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.generateCurrentPromptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateAllQueuedPromptsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripAddToQueue = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addCurrentSettingsToQueueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripDeleteImages = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteThisImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAllCurrentImagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripDevTools = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openCliToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openCmdInPythonEnvironmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openModelMergeToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openModelPruningTrimmingToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertModelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripPostProcess = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.upscaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.faceRestorationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applyAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelSettings = new StableDiffusionGui.Controls.CustomPanel();
            this.panelRes = new System.Windows.Forms.Panel();
            this.panelIterations = new System.Windows.Forms.Panel();
            this.panelInitImgStrength = new System.Windows.Forms.Panel();
            this.panelPromptNeg = new System.Windows.Forms.Panel();
            this.panelDebugPerlinThresh = new System.Windows.Forms.Panel();
            this.textboxThresh = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.textboxPerlin = new System.Windows.Forms.TextBox();
            this.panelDebugSendStdin = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.textboxCliTest = new System.Windows.Forms.TextBox();
            this.panelSeamless = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.panelSeed = new System.Windows.Forms.Panel();
            this.checkboxLoopback = new System.Windows.Forms.CheckBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.htSwitch1 = new HTAlt.WinForms.HTSwitch();
            this.label5 = new System.Windows.Forms.Label();
            this.panelScaleImg = new System.Windows.Forms.Panel();
            this.label17 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panelScale = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelSteps = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.textboxSliderSteps = new System.Windows.Forms.TextBox();
            this.sliderSteps = new StableDiffusionGui.Controls.CustomSlider();
            this.panelInpainting = new System.Windows.Forms.Panel();
            this.comboxInpaintMode = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.panelAiInputs = new System.Windows.Forms.Panel();
            this.panelTest = new System.Windows.Forms.Panel();
            this.labelAspectRatio = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictBoxInitImg = new System.Windows.Forms.PictureBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboxResH = new System.Windows.Forms.ComboBox();
            this.comboxResW = new System.Windows.Forms.ComboBox();
            this.panelPrompt = new System.Windows.Forms.Panel();
            this.promptAutocomplete = new AutocompleteMenuNS.AutocompleteMenu();
            this.pictBoxImgViewer = new System.Windows.Forms.PictureBox();
            this.separator = new System.Windows.Forms.Button();
            this.tableLayoutPanelImgViewers = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboxSdModel = new System.Windows.Forms.ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.ImgListView = new System.Windows.Forms.ListView();
            this.label2 = new System.Windows.Forms.Label();
            this.comboxVaeModel = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbScheduler = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbXPilot = new System.Windows.Forms.ComboBox();
            this.cbYPilot = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.tbXPilot = new System.Windows.Forms.TextBox();
            this.tbYPilot = new System.Windows.Forms.TextBox();
            this.cbXYPilot = new System.Windows.Forms.CheckBox();
            this.menuStripOutputImg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upDownSeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDownIterations)).BeginInit();
            this.menuStripLogs.SuspendLayout();
            this.menuStripRunQueue.SuspendLayout();
            this.menuStripAddToQueue.SuspendLayout();
            this.menuStripDeleteImages.SuspendLayout();
            this.menuStripDevTools.SuspendLayout();
            this.menuStripPostProcess.SuspendLayout();
            this.panelSettings.SuspendLayout();
            this.panelPromptNeg.SuspendLayout();
            this.panelDebugPerlinThresh.SuspendLayout();
            this.panelDebugSendStdin.SuspendLayout();
            this.panelSeamless.SuspendLayout();
            this.panelSeed.SuspendLayout();
            this.panelScaleImg.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panelScale.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelSteps.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.panelInpainting.SuspendLayout();
            this.panelAiInputs.SuspendLayout();
            this.panelTest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxInitImg)).BeginInit();
            this.panelPrompt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxImgViewer)).BeginInit();
            this.tableLayoutPanelImgViewers.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // runBtn
            // 
            this.runBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.runBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.runBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.runBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.runBtn.ForeColor = System.Drawing.Color.White;
            this.runBtn.Location = new System.Drawing.Point(925, 965);
            this.runBtn.Margin = new System.Windows.Forms.Padding(4);
            this.runBtn.Name = "runBtn";
            this.runBtn.Size = new System.Drawing.Size(160, 49);
            this.runBtn.TabIndex = 100;
            this.runBtn.Text = "Generate!";
            this.toolTip.SetToolTip(this.runBtn, "Generate Images");
            this.runBtn.UseVisualStyleBackColor = false;
            this.runBtn.Click += new System.EventHandler(this.runBtn_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.titleLabel.Font = new System.Drawing.Font("Yu Gothic UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.Location = new System.Drawing.Point(15, 11);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 12);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(115, 52);
            this.titleLabel.TabIndex = 11;
            this.titleLabel.Text = "SD UI";
            // 
            // logBox
            // 
            this.logBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.promptAutocomplete.SetAutocompleteMenu(this.logBox, null);
            this.logBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.logBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.logBox.ForeColor = System.Drawing.Color.Silver;
            this.logBox.Location = new System.Drawing.Point(16, 943);
            this.logBox.Margin = new System.Windows.Forms.Padding(4);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logBox.Size = new System.Drawing.Size(898, 100);
            this.logBox.TabIndex = 78;
            this.logBox.TabStop = false;
            // 
            // labelImgInfo
            // 
            this.labelImgInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelImgInfo.AutoSize = true;
            this.labelImgInfo.ForeColor = System.Drawing.Color.Silver;
            this.labelImgInfo.Location = new System.Drawing.Point(923, 941);
            this.labelImgInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelImgInfo.Name = "labelImgInfo";
            this.labelImgInfo.Size = new System.Drawing.Size(124, 16);
            this.labelImgInfo.TabIndex = 81;
            this.labelImgInfo.Text = "No images to show.";
            // 
            // progressCircle
            // 
            this.progressCircle.AnimationFunction = WinFormAnimation.KnownAnimationFunctions.Liner;
            this.progressCircle.AnimationSpeed = 500;
            this.progressCircle.BackColor = System.Drawing.Color.Transparent;
            this.progressCircle.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Bold);
            this.progressCircle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.progressCircle.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.progressCircle.InnerMargin = 2;
            this.progressCircle.InnerWidth = -1;
            this.progressCircle.Location = new System.Drawing.Point(512, 34);
            this.progressCircle.Margin = new System.Windows.Forms.Padding(4);
            this.progressCircle.MarqueeAnimationSpeed = 2000;
            this.progressCircle.Name = "progressCircle";
            this.progressCircle.OuterColor = System.Drawing.Color.Gray;
            this.progressCircle.OuterMargin = -21;
            this.progressCircle.OuterWidth = 21;
            this.progressCircle.ProgressColor = System.Drawing.Color.LimeGreen;
            this.progressCircle.ProgressWidth = 8;
            this.progressCircle.SecondaryFont = new System.Drawing.Font("Microsoft Sans Serif", 36F);
            this.progressCircle.Size = new System.Drawing.Size(45, 45);
            this.progressCircle.StartAngle = 270;
            this.progressCircle.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressCircle.SubscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.progressCircle.SubscriptMargin = new System.Windows.Forms.Padding(10, -35, 0, 0);
            this.progressCircle.SubscriptText = ".23";
            this.progressCircle.SuperscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.progressCircle.SuperscriptMargin = new System.Windows.Forms.Padding(10, 35, 0, 0);
            this.progressCircle.SuperscriptText = "°C";
            this.progressCircle.TabIndex = 99;
            this.progressCircle.TextMargin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.progressCircle.Value = 33;
            this.progressCircle.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.progressBar.BorderThickness = 0;
            this.progressBar.Location = new System.Drawing.Point(922, 1025);
            this.progressBar.Margin = new System.Windows.Forms.Padding(4);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(668, 18);
            this.progressBar.TabIndex = 100;
            this.progressBar.TabStop = false;
            // 
            // menuStripOutputImg
            // 
            this.menuStripOutputImg.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStripOutputImg.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileToolStripMenuItem,
            this.openOutputFolderToolStripMenuItem,
            this.copyToFavoritesToolStripMenuItem,
            this.copyImageToClipboardToolStripMenuItem,
            this.copySidebySideComparisonImageToolStripMenuItem,
            this.copySeedToClipboardToolStripMenuItem,
            this.reGenerateImageWithCurrentSettingsToolStripMenuItem,
            this.useAsInitImageToolStripMenuItem,
            this.postProcessImageToolStripMenuItem,
            this.fitWindowSizeToImageSizeToolStripMenuItem});
            this.menuStripOutputImg.Name = "menuStripOutputImg";
            this.menuStripOutputImg.ShowImageMargin = false;
            this.menuStripOutputImg.Size = new System.Drawing.Size(353, 244);
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(352, 24);
            this.openFileToolStripMenuItem.Text = "Open File";
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // openOutputFolderToolStripMenuItem
            // 
            this.openOutputFolderToolStripMenuItem.Name = "openOutputFolderToolStripMenuItem";
            this.openOutputFolderToolStripMenuItem.Size = new System.Drawing.Size(352, 24);
            this.openOutputFolderToolStripMenuItem.Text = "Open Output Folder";
            this.openOutputFolderToolStripMenuItem.Click += new System.EventHandler(this.openOutputFolderToolStripMenuItem_Click);
            // 
            // copyToFavoritesToolStripMenuItem
            // 
            this.copyToFavoritesToolStripMenuItem.Name = "copyToFavoritesToolStripMenuItem";
            this.copyToFavoritesToolStripMenuItem.Size = new System.Drawing.Size(352, 24);
            this.copyToFavoritesToolStripMenuItem.Text = "Copy To Favorites";
            this.copyToFavoritesToolStripMenuItem.Click += new System.EventHandler(this.copyToFavoritesToolStripMenuItem_Click);
            // 
            // copyImageToClipboardToolStripMenuItem
            // 
            this.copyImageToClipboardToolStripMenuItem.Name = "copyImageToClipboardToolStripMenuItem";
            this.copyImageToClipboardToolStripMenuItem.Size = new System.Drawing.Size(352, 24);
            this.copyImageToClipboardToolStripMenuItem.Text = "Copy Image";
            this.copyImageToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyImageToClipboardToolStripMenuItem_Click);
            // 
            // copySidebySideComparisonImageToolStripMenuItem
            // 
            this.copySidebySideComparisonImageToolStripMenuItem.Name = "copySidebySideComparisonImageToolStripMenuItem";
            this.copySidebySideComparisonImageToolStripMenuItem.Size = new System.Drawing.Size(352, 24);
            this.copySidebySideComparisonImageToolStripMenuItem.Text = "Copy Side-by-Side Comparison Image";
            this.copySidebySideComparisonImageToolStripMenuItem.Click += new System.EventHandler(this.copySidebySideComparisonImageToolStripMenuItem_Click);
            // 
            // copySeedToClipboardToolStripMenuItem
            // 
            this.copySeedToClipboardToolStripMenuItem.Name = "copySeedToClipboardToolStripMenuItem";
            this.copySeedToClipboardToolStripMenuItem.Size = new System.Drawing.Size(352, 24);
            this.copySeedToClipboardToolStripMenuItem.Text = "Copy Seed";
            this.copySeedToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copySeedToClipboardToolStripMenuItem_Click);
            // 
            // reGenerateImageWithCurrentSettingsToolStripMenuItem
            // 
            this.reGenerateImageWithCurrentSettingsToolStripMenuItem.Name = "reGenerateImageWithCurrentSettingsToolStripMenuItem";
            this.reGenerateImageWithCurrentSettingsToolStripMenuItem.Size = new System.Drawing.Size(352, 24);
            this.reGenerateImageWithCurrentSettingsToolStripMenuItem.Text = "Re-Generate Image With Current Settings";
            this.reGenerateImageWithCurrentSettingsToolStripMenuItem.Click += new System.EventHandler(this.reGenerateImageWithCurrentSettingsToolStripMenuItem_Click);
            // 
            // useAsInitImageToolStripMenuItem
            // 
            this.useAsInitImageToolStripMenuItem.Name = "useAsInitImageToolStripMenuItem";
            this.useAsInitImageToolStripMenuItem.Size = new System.Drawing.Size(352, 24);
            this.useAsInitImageToolStripMenuItem.Text = "Use as Initialization Image";
            this.useAsInitImageToolStripMenuItem.Click += new System.EventHandler(this.useAsInitImageToolStripMenuItem_Click);
            // 
            // postProcessImageToolStripMenuItem
            // 
            this.postProcessImageToolStripMenuItem.Name = "postProcessImageToolStripMenuItem";
            this.postProcessImageToolStripMenuItem.Size = new System.Drawing.Size(352, 24);
            this.postProcessImageToolStripMenuItem.Text = "Post-Process Image...";
            this.postProcessImageToolStripMenuItem.Click += new System.EventHandler(this.postProcessImageToolStripMenuItem_Click);
            // 
            // fitWindowSizeToImageSizeToolStripMenuItem
            // 
            this.fitWindowSizeToImageSizeToolStripMenuItem.Name = "fitWindowSizeToImageSizeToolStripMenuItem";
            this.fitWindowSizeToImageSizeToolStripMenuItem.Size = new System.Drawing.Size(352, 24);
            this.fitWindowSizeToImageSizeToolStripMenuItem.Text = "Fit Window Size To Image Size (Pixel-Perfect)";
            this.fitWindowSizeToImageSizeToolStripMenuItem.Click += new System.EventHandler(this.fitWindowSizeToImageSizeToolStripMenuItem_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 200;
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 200;
            this.toolTip.ReshowDelay = 40;
            // 
            // labelImgPrompt
            // 
            this.labelImgPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelImgPrompt.AutoEllipsis = true;
            this.labelImgPrompt.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.labelImgPrompt.ForeColor = System.Drawing.Color.Silver;
            this.labelImgPrompt.Location = new System.Drawing.Point(922, 75);
            this.labelImgPrompt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelImgPrompt.Name = "labelImgPrompt";
            this.labelImgPrompt.Size = new System.Drawing.Size(666, 20);
            this.labelImgPrompt.TabIndex = 115;
            this.labelImgPrompt.Text = "No prompt to display.";
            this.toolTip.SetToolTip(this.labelImgPrompt, "Shows the prompt of the displayed image. Click to copy.");
            this.labelImgPrompt.Click += new System.EventHandler(this.labelImgPrompt_Click);
            // 
            // labelImgPromptNeg
            // 
            this.labelImgPromptNeg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelImgPromptNeg.AutoEllipsis = true;
            this.labelImgPromptNeg.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.labelImgPromptNeg.ForeColor = System.Drawing.Color.Silver;
            this.labelImgPromptNeg.Location = new System.Drawing.Point(922, 95);
            this.labelImgPromptNeg.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelImgPromptNeg.Name = "labelImgPromptNeg";
            this.labelImgPromptNeg.Size = new System.Drawing.Size(666, 20);
            this.labelImgPromptNeg.TabIndex = 116;
            this.labelImgPromptNeg.Text = "No negative prompt to display.";
            this.toolTip.SetToolTip(this.labelImgPromptNeg, "Shows the prompt of the displayed image. Click to copy.");
            this.labelImgPromptNeg.Click += new System.EventHandler(this.labelImgPromptNeg_Click);
            // 
            // comboxSampler
            // 
            this.comboxSampler.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxSampler.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxSampler.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxSampler.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboxSampler.ForeColor = System.Drawing.Color.White;
            this.comboxSampler.FormattingEnabled = true;
            this.comboxSampler.Location = new System.Drawing.Point(727, 34);
            this.comboxSampler.Margin = new System.Windows.Forms.Padding(4);
            this.comboxSampler.Name = "comboxSampler";
            this.comboxSampler.Size = new System.Drawing.Size(187, 26);
            this.comboxSampler.TabIndex = 105;
            this.toolTip.SetToolTip(this.comboxSampler, "Changes how the image is sampled.\r\nEuler Ancestral works very well at low step co" +
        "unts.");
            // 
            // upDownSeed
            // 
            this.upDownSeed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.upDownSeed.Enabled = false;
            this.upDownSeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.upDownSeed.ForeColor = System.Drawing.Color.White;
            this.upDownSeed.Location = new System.Drawing.Point(311, 10);
            this.upDownSeed.Margin = new System.Windows.Forms.Padding(4);
            this.upDownSeed.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.upDownSeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.upDownSeed.Name = "upDownSeed";
            this.upDownSeed.Size = new System.Drawing.Size(133, 24);
            this.upDownSeed.TabIndex = 4;
            this.toolTip.SetToolTip(this.upDownSeed, "Set this to a specific value to reproduce the same image.\r\nImportant: Resolution " +
        "and sampler need to be identical as well.");
            this.upDownSeed.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            // 
            // sliderScale
            // 
            this.sliderScale.ActualMaximum = new decimal(new int[] {
            100,
            0,
            0,
            65536});
            this.sliderScale.ActualMinimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.sliderScale.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.sliderScale.BorderRoundRectSize = new System.Drawing.Size(12, 12);
            this.sliderScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sliderScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.sliderScale.ForeColor = System.Drawing.Color.Black;
            this.sliderScale.InitValue = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.sliderScale.LargeChange = ((uint)(5u));
            this.sliderScale.Location = new System.Drawing.Point(0, 0);
            this.sliderScale.Margin = new System.Windows.Forms.Padding(0);
            this.sliderScale.Maximum = 20;
            this.sliderScale.Name = "sliderScale";
            this.sliderScale.OverlayColor = System.Drawing.Color.White;
            this.sliderScale.Size = new System.Drawing.Size(420, 43);
            this.sliderScale.SmallChange = ((uint)(1u));
            this.sliderScale.TabIndex = 4;
            this.sliderScale.ThumbRoundRectSize = new System.Drawing.Size(16, 16);
            this.sliderScale.ThumbSize = new System.Drawing.Size(14, 14);
            this.toolTip.SetToolTip(this.sliderScale, "Higher tries to match your prompt better, but can get chaotic. 7-12 is a safe ran" +
        "ge in most cases.");
            this.sliderScale.Value = 0;
            this.sliderScale.ValueBox = this.textboxSliderScale;
            this.sliderScale.ValueStep = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // textboxSliderScale
            // 
            this.textboxSliderScale.AllowDrop = true;
            this.textboxSliderScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.promptAutocomplete.SetAutocompleteMenu(this.textboxSliderScale, null);
            this.textboxSliderScale.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.textboxSliderScale.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textboxSliderScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textboxSliderScale.ForeColor = System.Drawing.Color.Silver;
            this.textboxSliderScale.Location = new System.Drawing.Point(420, 11);
            this.textboxSliderScale.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.textboxSliderScale.MinimumSize = new System.Drawing.Size(5, 21);
            this.textboxSliderScale.Name = "textboxSliderScale";
            this.textboxSliderScale.Size = new System.Drawing.Size(47, 22);
            this.textboxSliderScale.TabIndex = 93;
            this.textboxSliderScale.Text = "0";
            // 
            // textboxExtraScales
            // 
            this.textboxExtraScales.AllowDrop = true;
            this.textboxExtraScales.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.promptAutocomplete.SetAutocompleteMenu(this.textboxExtraScales, null);
            this.textboxExtraScales.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textboxExtraScales.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textboxExtraScales.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textboxExtraScales.ForeColor = System.Drawing.Color.White;
            this.textboxExtraScales.Location = new System.Drawing.Point(471, 9);
            this.textboxExtraScales.Margin = new System.Windows.Forms.Padding(4);
            this.textboxExtraScales.MinimumSize = new System.Drawing.Size(5, 21);
            this.textboxExtraScales.Name = "textboxExtraScales";
            this.textboxExtraScales.Size = new System.Drawing.Size(92, 24);
            this.textboxExtraScales.TabIndex = 3;
            this.toolTip.SetToolTip(this.textboxExtraScales, resources.GetString("textboxExtraScales.ToolTip"));
            // 
            // upDownIterations
            // 
            this.upDownIterations.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.upDownIterations.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.upDownIterations.ForeColor = System.Drawing.Color.White;
            this.upDownIterations.Location = new System.Drawing.Point(812, 78);
            this.upDownIterations.Margin = new System.Windows.Forms.Padding(4);
            this.upDownIterations.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.upDownIterations.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.upDownIterations.Name = "upDownIterations";
            this.upDownIterations.Size = new System.Drawing.Size(60, 24);
            this.upDownIterations.TabIndex = 2;
            this.toolTip.SetToolTip(this.upDownIterations, "Amount of images to create for the entered promt.\r\nThe seed will be incremented b" +
        "y 1 for each image after the first.");
            this.upDownIterations.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // btnResetMask
            // 
            this.btnResetMask.AutoColor = true;
            this.btnResetMask.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnResetMask.ButtonImage = null;
            this.btnResetMask.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnResetMask.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnResetMask.DrawImage = false;
            this.btnResetMask.ForeColor = System.Drawing.Color.White;
            this.btnResetMask.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.btnResetMask.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnResetMask.Location = new System.Drawing.Point(585, 7);
            this.btnResetMask.Margin = new System.Windows.Forms.Padding(4);
            this.btnResetMask.Name = "btnResetMask";
            this.btnResetMask.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnResetMask.Size = new System.Drawing.Size(105, 28);
            this.btnResetMask.TabIndex = 108;
            this.btnResetMask.TabStop = false;
            this.btnResetMask.Text = "Clear Mask";
            this.toolTip.SetToolTip(this.btnResetMask, "Reset Inpainting Mask");
            this.btnResetMask.Visible = false;
            this.btnResetMask.Click += new System.EventHandler(this.btnResetMask_Click);
            // 
            // sliderInitStrength
            // 
            this.sliderInitStrength.ActualMaximum = new decimal(new int[] {
            9,
            0,
            0,
            65536});
            this.sliderInitStrength.ActualMinimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.sliderInitStrength.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.sliderInitStrength.BorderRoundRectSize = new System.Drawing.Size(12, 12);
            this.sliderInitStrength.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.sliderInitStrength.ForeColor = System.Drawing.Color.Black;
            this.sliderInitStrength.InitValue = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.sliderInitStrength.LargeChange = ((uint)(2u));
            this.sliderInitStrength.Location = new System.Drawing.Point(250, 165);
            this.sliderInitStrength.Margin = new System.Windows.Forms.Padding(0);
            this.sliderInitStrength.Maximum = 18;
            this.sliderInitStrength.Minimum = 2;
            this.sliderInitStrength.Name = "sliderInitStrength";
            this.sliderInitStrength.OverlayColor = System.Drawing.Color.White;
            this.sliderInitStrength.Size = new System.Drawing.Size(369, 43);
            this.sliderInitStrength.SmallChange = ((uint)(1u));
            this.sliderInitStrength.TabIndex = 4;
            this.sliderInitStrength.ThumbRoundRectSize = new System.Drawing.Size(16, 16);
            this.sliderInitStrength.ThumbSize = new System.Drawing.Size(14, 14);
            this.toolTip.SetToolTip(this.sliderInitStrength, "Lower Value: Result Looks More Like Your Text Prompt\r\nHigher Value: Result Looks " +
        "More Like Your Image");
            this.sliderInitStrength.Value = 2;
            this.sliderInitStrength.ValueBox = this.textboxSliderInitStrength;
            this.sliderInitStrength.ValueStep = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            // 
            // textboxSliderInitStrength
            // 
            this.textboxSliderInitStrength.AllowDrop = true;
            this.textboxSliderInitStrength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.promptAutocomplete.SetAutocompleteMenu(this.textboxSliderInitStrength, null);
            this.textboxSliderInitStrength.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.textboxSliderInitStrength.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textboxSliderInitStrength.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textboxSliderInitStrength.ForeColor = System.Drawing.Color.Silver;
            this.textboxSliderInitStrength.Location = new System.Drawing.Point(618, 172);
            this.textboxSliderInitStrength.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.textboxSliderInitStrength.MinimumSize = new System.Drawing.Size(5, 21);
            this.textboxSliderInitStrength.Name = "textboxSliderInitStrength";
            this.textboxSliderInitStrength.Size = new System.Drawing.Size(32, 22);
            this.textboxSliderInitStrength.TabIndex = 94;
            this.textboxSliderInitStrength.Text = "0,1";
            // 
            // btnInitImgBrowse
            // 
            this.btnInitImgBrowse.AutoColor = true;
            this.btnInitImgBrowse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnInitImgBrowse.ButtonImage = null;
            this.btnInitImgBrowse.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnInitImgBrowse.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnInitImgBrowse.DrawImage = false;
            this.btnInitImgBrowse.ForeColor = System.Drawing.Color.White;
            this.btnInitImgBrowse.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.btnInitImgBrowse.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnInitImgBrowse.Location = new System.Drawing.Point(8, 8);
            this.btnInitImgBrowse.Margin = new System.Windows.Forms.Padding(4);
            this.btnInitImgBrowse.Name = "btnInitImgBrowse";
            this.btnInitImgBrowse.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnInitImgBrowse.Size = new System.Drawing.Size(133, 28);
            this.btnInitImgBrowse.TabIndex = 1;
            this.btnInitImgBrowse.TabStop = false;
            this.btnInitImgBrowse.Text = "Load Image";
            this.toolTip.SetToolTip(this.btnInitImgBrowse, "Load initialization image");
            this.btnInitImgBrowse.Click += new System.EventHandler(this.btnInitImgBrowse_Click);
            // 
            // btnEmbeddingBrowse
            // 
            this.btnEmbeddingBrowse.AutoColor = true;
            this.btnEmbeddingBrowse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnEmbeddingBrowse.ButtonImage = null;
            this.btnEmbeddingBrowse.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnEmbeddingBrowse.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnEmbeddingBrowse.DrawImage = false;
            this.btnEmbeddingBrowse.ForeColor = System.Drawing.Color.White;
            this.btnEmbeddingBrowse.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.btnEmbeddingBrowse.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnEmbeddingBrowse.Location = new System.Drawing.Point(149, 8);
            this.btnEmbeddingBrowse.Margin = new System.Windows.Forms.Padding(4);
            this.btnEmbeddingBrowse.Name = "btnEmbeddingBrowse";
            this.btnEmbeddingBrowse.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnEmbeddingBrowse.Size = new System.Drawing.Size(133, 28);
            this.btnEmbeddingBrowse.TabIndex = 84;
            this.btnEmbeddingBrowse.TabStop = false;
            this.btnEmbeddingBrowse.Text = "Load Concept";
            this.toolTip.SetToolTip(this.btnEmbeddingBrowse, "Load a concept trained using Textual Inversion");
            this.btnEmbeddingBrowse.Click += new System.EventHandler(this.btnEmbeddingBrowse_Click);
            // 
            // textboxPrompt
            // 
            this.textboxPrompt.AllowDrop = true;
            this.textboxPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.promptAutocomplete.SetAutocompleteMenu(this.textboxPrompt, null);
            this.textboxPrompt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textboxPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textboxPrompt.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.textboxPrompt.Location = new System.Drawing.Point(4, 8);
            this.textboxPrompt.Margin = new System.Windows.Forms.Padding(4, 4, 0, 4);
            this.textboxPrompt.MinimumSize = new System.Drawing.Size(4, 25);
            this.textboxPrompt.Multiline = true;
            this.textboxPrompt.Name = "textboxPrompt";
            this.textboxPrompt.Placeholder = "Enter your prompt here...";
            this.textboxPrompt.PlaceholderTextColor = System.Drawing.Color.Silver;
            this.textboxPrompt.Size = new System.Drawing.Size(866, 80);
            this.textboxPrompt.TabIndex = 0;
            this.toolTip.SetToolTip(this.textboxPrompt, "Text prompt. The AI will try to generate an image matching this description.");
            // 
            // comboxSeamless
            // 
            this.comboxSeamless.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxSeamless.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxSeamless.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxSeamless.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboxSeamless.ForeColor = System.Drawing.Color.White;
            this.comboxSeamless.FormattingEnabled = true;
            this.comboxSeamless.Location = new System.Drawing.Point(311, 9);
            this.comboxSeamless.Margin = new System.Windows.Forms.Padding(4);
            this.comboxSeamless.Name = "comboxSeamless";
            this.comboxSeamless.Size = new System.Drawing.Size(265, 26);
            this.comboxSeamless.TabIndex = 107;
            this.toolTip.SetToolTip(this.comboxSeamless, "Changes how the image is sampled.\r\nEuler Ancestral works very well at low step co" +
        "unts.");
            // 
            // textboxClipsegMask
            // 
            this.textboxClipsegMask.AllowDrop = true;
            this.textboxClipsegMask.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.promptAutocomplete.SetAutocompleteMenu(this.textboxClipsegMask, null);
            this.textboxClipsegMask.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textboxClipsegMask.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textboxClipsegMask.ForeColor = System.Drawing.Color.White;
            this.textboxClipsegMask.Location = new System.Drawing.Point(585, 9);
            this.textboxClipsegMask.Margin = new System.Windows.Forms.Padding(4);
            this.textboxClipsegMask.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxClipsegMask.Name = "textboxClipsegMask";
            this.textboxClipsegMask.Size = new System.Drawing.Size(287, 24);
            this.textboxClipsegMask.TabIndex = 110;
            this.toolTip.SetToolTip(this.textboxClipsegMask, "Describe what objects you want to replace");
            // 
            // textboxExtraSteps
            // 
            this.textboxExtraSteps.AllowDrop = true;
            this.textboxExtraSteps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.promptAutocomplete.SetAutocompleteMenu(this.textboxExtraSteps, null);
            this.textboxExtraSteps.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textboxExtraSteps.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textboxExtraSteps.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textboxExtraSteps.ForeColor = System.Drawing.Color.White;
            this.textboxExtraSteps.Location = new System.Drawing.Point(471, 9);
            this.textboxExtraSteps.Margin = new System.Windows.Forms.Padding(4);
            this.textboxExtraSteps.MinimumSize = new System.Drawing.Size(5, 21);
            this.textboxExtraSteps.Name = "textboxExtraSteps";
            this.textboxExtraSteps.Size = new System.Drawing.Size(92, 24);
            this.textboxExtraSteps.TabIndex = 93;
            this.toolTip.SetToolTip(this.textboxExtraSteps, resources.GetString("textboxExtraSteps.ToolTip"));
            // 
            // btnEditMask
            // 
            this.btnEditMask.AutoColor = true;
            this.btnEditMask.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnEditMask.ButtonImage = null;
            this.btnEditMask.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnEditMask.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnEditMask.DrawImage = false;
            this.btnEditMask.ForeColor = System.Drawing.Color.White;
            this.btnEditMask.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.btnEditMask.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnEditMask.Location = new System.Drawing.Point(699, 7);
            this.btnEditMask.Margin = new System.Windows.Forms.Padding(4);
            this.btnEditMask.Name = "btnEditMask";
            this.btnEditMask.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnEditMask.Size = new System.Drawing.Size(105, 28);
            this.btnEditMask.TabIndex = 111;
            this.btnEditMask.TabStop = false;
            this.btnEditMask.Text = "Edit Mask";
            this.toolTip.SetToolTip(this.btnEditMask, "Edit Inpainting Mask");
            this.btnEditMask.Visible = false;
            this.btnEditMask.Click += new System.EventHandler(this.btnEditMask_Click);
            // 
            // btnDreambooth
            // 
            this.btnDreambooth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDreambooth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnDreambooth.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDreambooth.BackgroundImage")));
            this.btnDreambooth.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDreambooth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDreambooth.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDreambooth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnDreambooth.Location = new System.Drawing.Point(1291, 15);
            this.btnDreambooth.Margin = new System.Windows.Forms.Padding(4);
            this.btnDreambooth.Name = "btnDreambooth";
            this.btnDreambooth.Size = new System.Drawing.Size(53, 49);
            this.btnDreambooth.TabIndex = 117;
            this.btnDreambooth.TabStop = false;
            this.toolTip.SetToolTip(this.btnDreambooth, "Train DreamBooth Model");
            this.btnDreambooth.UseVisualStyleBackColor = false;
            this.btnDreambooth.Click += new System.EventHandler(this.btnDreambooth_Click);
            // 
            // btnDeleteBatch
            // 
            this.btnDeleteBatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteBatch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnDeleteBatch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDeleteBatch.BackgroundImage")));
            this.btnDeleteBatch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDeleteBatch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteBatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteBatch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnDeleteBatch.Location = new System.Drawing.Point(1469, 965);
            this.btnDeleteBatch.Margin = new System.Windows.Forms.Padding(4);
            this.btnDeleteBatch.Name = "btnDeleteBatch";
            this.btnDeleteBatch.Size = new System.Drawing.Size(53, 49);
            this.btnDeleteBatch.TabIndex = 114;
            this.btnDeleteBatch.TabStop = false;
            this.toolTip.SetToolTip(this.btnDeleteBatch, "Delete one or all images...");
            this.btnDeleteBatch.UseVisualStyleBackColor = false;
            this.btnDeleteBatch.Click += new System.EventHandler(this.btnDeleteBatch_Click);
            // 
            // btnPromptHistory
            // 
            this.btnPromptHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPromptHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnPromptHistory.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPromptHistory.BackgroundImage")));
            this.btnPromptHistory.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPromptHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPromptHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPromptHistory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnPromptHistory.Location = new System.Drawing.Point(1154, 965);
            this.btnPromptHistory.Margin = new System.Windows.Forms.Padding(4);
            this.btnPromptHistory.Name = "btnPromptHistory";
            this.btnPromptHistory.Size = new System.Drawing.Size(53, 49);
            this.btnPromptHistory.TabIndex = 112;
            this.btnPromptHistory.TabStop = false;
            this.toolTip.SetToolTip(this.btnPromptHistory, "View Prompt History");
            this.btnPromptHistory.UseVisualStyleBackColor = false;
            this.btnPromptHistory.Click += new System.EventHandler(this.btnPromptHistory_Click);
            // 
            // btnQueue
            // 
            this.btnQueue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnQueue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnQueue.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnQueue.BackgroundImage")));
            this.btnQueue.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnQueue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQueue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQueue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnQueue.Location = new System.Drawing.Point(1093, 965);
            this.btnQueue.Margin = new System.Windows.Forms.Padding(4);
            this.btnQueue.Name = "btnQueue";
            this.btnQueue.Size = new System.Drawing.Size(53, 49);
            this.btnQueue.TabIndex = 111;
            this.btnQueue.TabStop = false;
            this.toolTip.SetToolTip(this.btnQueue, "View Prompt Queue");
            this.btnQueue.UseVisualStyleBackColor = false;
            this.btnQueue.Click += new System.EventHandler(this.btnQueue_Click);
            this.btnQueue.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnQueue_MouseDown);
            // 
            // btnPostProc
            // 
            this.btnPostProc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPostProc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnPostProc.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPostProc.BackgroundImage")));
            this.btnPostProc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPostProc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPostProc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPostProc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnPostProc.Location = new System.Drawing.Point(1229, 15);
            this.btnPostProc.Margin = new System.Windows.Forms.Padding(4);
            this.btnPostProc.Name = "btnPostProc";
            this.btnPostProc.Size = new System.Drawing.Size(53, 49);
            this.btnPostProc.TabIndex = 109;
            this.btnPostProc.TabStop = false;
            this.toolTip.SetToolTip(this.btnPostProc, "Post Processing Settings");
            this.btnPostProc.UseVisualStyleBackColor = false;
            this.btnPostProc.Click += new System.EventHandler(this.btnPostProc_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnSettings.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSettings.BackgroundImage")));
            this.btnSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSettings.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnSettings.Location = new System.Drawing.Point(1536, 15);
            this.btnSettings.Margin = new System.Windows.Forms.Padding(4);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(53, 49);
            this.btnSettings.TabIndex = 108;
            this.btnSettings.TabStop = false;
            this.toolTip.SetToolTip(this.btnSettings, "Open Settings");
            this.btnSettings.UseVisualStyleBackColor = false;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnDebug
            // 
            this.btnDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDebug.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnDebug.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDebug.BackgroundImage")));
            this.btnDebug.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDebug.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDebug.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDebug.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnDebug.Location = new System.Drawing.Point(1475, 15);
            this.btnDebug.Margin = new System.Windows.Forms.Padding(4);
            this.btnDebug.Name = "btnDebug";
            this.btnDebug.Size = new System.Drawing.Size(53, 49);
            this.btnDebug.TabIndex = 107;
            this.btnDebug.TabStop = false;
            this.toolTip.SetToolTip(this.btnDebug, "Logs...");
            this.btnDebug.UseVisualStyleBackColor = false;
            this.btnDebug.Click += new System.EventHandler(this.btnDebug_Click);
            // 
            // cliButton
            // 
            this.cliButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cliButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.cliButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cliButton.BackgroundImage")));
            this.cliButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cliButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cliButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cliButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.cliButton.Location = new System.Drawing.Point(1352, 15);
            this.cliButton.Margin = new System.Windows.Forms.Padding(4);
            this.cliButton.Name = "cliButton";
            this.cliButton.Size = new System.Drawing.Size(53, 49);
            this.cliButton.TabIndex = 103;
            this.cliButton.TabStop = false;
            this.toolTip.SetToolTip(this.cliButton, "Developer Tools...");
            this.cliButton.UseVisualStyleBackColor = false;
            this.cliButton.Click += new System.EventHandler(this.cliButton_Click);
            // 
            // btnOpenOutFolder
            // 
            this.btnOpenOutFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenOutFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnOpenOutFolder.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOpenOutFolder.BackgroundImage")));
            this.btnOpenOutFolder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnOpenOutFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenOutFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenOutFolder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnOpenOutFolder.Location = new System.Drawing.Point(1530, 965);
            this.btnOpenOutFolder.Margin = new System.Windows.Forms.Padding(4);
            this.btnOpenOutFolder.Name = "btnOpenOutFolder";
            this.btnOpenOutFolder.Size = new System.Drawing.Size(53, 49);
            this.btnOpenOutFolder.TabIndex = 94;
            this.btnOpenOutFolder.TabStop = false;
            this.toolTip.SetToolTip(this.btnOpenOutFolder, "Open Output Folder");
            this.btnOpenOutFolder.UseVisualStyleBackColor = false;
            this.btnOpenOutFolder.Click += new System.EventHandler(this.btnOpenOutFolder_Click);
            // 
            // btnPrevImg
            // 
            this.btnPrevImg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrevImg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnPrevImg.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPrevImg.BackgroundImage")));
            this.btnPrevImg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPrevImg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevImg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrevImg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnPrevImg.Location = new System.Drawing.Point(6, 633);
            this.btnPrevImg.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrevImg.Name = "btnPrevImg";
            this.btnPrevImg.Size = new System.Drawing.Size(30, 30);
            this.btnPrevImg.TabIndex = 82;
            this.btnPrevImg.TabStop = false;
            this.toolTip.SetToolTip(this.btnPrevImg, "Previous Image");
            this.btnPrevImg.UseVisualStyleBackColor = false;
            this.btnPrevImg.Click += new System.EventHandler(this.btnPrevImg_Click);
            // 
            // btnNextImg
            // 
            this.btnNextImg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNextImg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnNextImg.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNextImg.BackgroundImage")));
            this.btnNextImg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnNextImg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextImg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNextImg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnNextImg.Location = new System.Drawing.Point(630, 631);
            this.btnNextImg.Margin = new System.Windows.Forms.Padding(4);
            this.btnNextImg.Name = "btnNextImg";
            this.btnNextImg.Size = new System.Drawing.Size(30, 30);
            this.btnNextImg.TabIndex = 80;
            this.btnNextImg.TabStop = false;
            this.toolTip.SetToolTip(this.btnNextImg, "Next Image");
            this.btnNextImg.UseVisualStyleBackColor = false;
            this.btnNextImg.Click += new System.EventHandler(this.btnNextImg_Click);
            // 
            // installerBtn
            // 
            this.installerBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.installerBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.installerBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("installerBtn.BackgroundImage")));
            this.installerBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.installerBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.installerBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.installerBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.installerBtn.Location = new System.Drawing.Point(1413, 15);
            this.installerBtn.Margin = new System.Windows.Forms.Padding(4);
            this.installerBtn.Name = "installerBtn";
            this.installerBtn.Size = new System.Drawing.Size(53, 49);
            this.installerBtn.TabIndex = 76;
            this.installerBtn.TabStop = false;
            this.toolTip.SetToolTip(this.installerBtn, "Open Installer");
            this.installerBtn.UseVisualStyleBackColor = false;
            this.installerBtn.Click += new System.EventHandler(this.installerBtn_Click);
            // 
            // sliderScaleImg
            // 
            this.sliderScaleImg.ActualMaximum = new decimal(new int[] {
            25,
            0,
            0,
            65536});
            this.sliderScaleImg.ActualMinimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.sliderScaleImg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.sliderScaleImg.BorderRoundRectSize = new System.Drawing.Size(12, 12);
            this.sliderScaleImg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sliderScaleImg.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.sliderScaleImg.ForeColor = System.Drawing.Color.Black;
            this.sliderScaleImg.InitValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.sliderScaleImg.LargeChange = ((uint)(5u));
            this.sliderScaleImg.Location = new System.Drawing.Point(0, 0);
            this.sliderScaleImg.Margin = new System.Windows.Forms.Padding(0);
            this.sliderScaleImg.Maximum = 25;
            this.sliderScaleImg.Minimum = 5;
            this.sliderScaleImg.Name = "sliderScaleImg";
            this.sliderScaleImg.OverlayColor = System.Drawing.Color.White;
            this.sliderScaleImg.Size = new System.Drawing.Size(420, 43);
            this.sliderScaleImg.SmallChange = ((uint)(1u));
            this.sliderScaleImg.TabIndex = 4;
            this.sliderScaleImg.ThumbRoundRectSize = new System.Drawing.Size(16, 16);
            this.sliderScaleImg.ThumbSize = new System.Drawing.Size(14, 14);
            this.toolTip.SetToolTip(this.sliderScaleImg, "Higher tries to match your image better, but can get chaotic. 1-2 is a safe range" +
        " in most cases.");
            this.sliderScaleImg.Value = 15;
            this.sliderScaleImg.ValueBox = this.textboxSliderScaleImg;
            this.sliderScaleImg.ValueStep = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            // 
            // textboxSliderScaleImg
            // 
            this.textboxSliderScaleImg.AllowDrop = true;
            this.textboxSliderScaleImg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.promptAutocomplete.SetAutocompleteMenu(this.textboxSliderScaleImg, null);
            this.textboxSliderScaleImg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.textboxSliderScaleImg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textboxSliderScaleImg.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textboxSliderScaleImg.ForeColor = System.Drawing.Color.Silver;
            this.textboxSliderScaleImg.Location = new System.Drawing.Point(420, 11);
            this.textboxSliderScaleImg.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.textboxSliderScaleImg.MinimumSize = new System.Drawing.Size(5, 21);
            this.textboxSliderScaleImg.Name = "textboxSliderScaleImg";
            this.textboxSliderScaleImg.Size = new System.Drawing.Size(47, 22);
            this.textboxSliderScaleImg.TabIndex = 93;
            this.textboxSliderScaleImg.Text = "0,75";
            // 
            // textboxExtraScalesImg
            // 
            this.textboxExtraScalesImg.AllowDrop = true;
            this.textboxExtraScalesImg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.promptAutocomplete.SetAutocompleteMenu(this.textboxExtraScalesImg, null);
            this.textboxExtraScalesImg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textboxExtraScalesImg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textboxExtraScalesImg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textboxExtraScalesImg.ForeColor = System.Drawing.Color.White;
            this.textboxExtraScalesImg.Location = new System.Drawing.Point(471, 9);
            this.textboxExtraScalesImg.Margin = new System.Windows.Forms.Padding(4);
            this.textboxExtraScalesImg.MinimumSize = new System.Drawing.Size(5, 21);
            this.textboxExtraScalesImg.Name = "textboxExtraScalesImg";
            this.textboxExtraScalesImg.Size = new System.Drawing.Size(92, 24);
            this.textboxExtraScalesImg.TabIndex = 3;
            this.toolTip.SetToolTip(this.textboxExtraScalesImg, resources.GetString("textboxExtraScalesImg.ToolTip"));
            // 
            // textboxPromptNeg
            // 
            this.textboxPromptNeg.AllowDrop = true;
            this.textboxPromptNeg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.promptAutocomplete.SetAutocompleteMenu(this.textboxPromptNeg, null);
            this.textboxPromptNeg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textboxPromptNeg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textboxPromptNeg.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.textboxPromptNeg.Location = new System.Drawing.Point(4, 93);
            this.textboxPromptNeg.Margin = new System.Windows.Forms.Padding(4);
            this.textboxPromptNeg.MinimumSize = new System.Drawing.Size(4, 25);
            this.textboxPromptNeg.Multiline = true;
            this.textboxPromptNeg.Name = "textboxPromptNeg";
            this.textboxPromptNeg.Placeholder = "Enter your negative prompt here...";
            this.textboxPromptNeg.PlaceholderTextColor = System.Drawing.Color.Silver;
            this.textboxPromptNeg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textboxPromptNeg.Size = new System.Drawing.Size(866, 43);
            this.textboxPromptNeg.TabIndex = 2;
            this.toolTip.SetToolTip(this.textboxPromptNeg, "Negative text prompt. The AI will try to avoid generating things you describe her" +
        "e.");
            // 
            // checkboxHiresFix
            // 
            this.checkboxHiresFix.AutoSize = true;
            this.checkboxHiresFix.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkboxHiresFix.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkboxHiresFix.Location = new System.Drawing.Point(255, 86);
            this.checkboxHiresFix.Name = "checkboxHiresFix";
            this.checkboxHiresFix.Size = new System.Drawing.Size(152, 21);
            this.checkboxHiresFix.TabIndex = 115;
            this.checkboxHiresFix.Text = "High-Resolution Fix";
            this.toolTip.SetToolTip(this.checkboxHiresFix, "Avoid duplications in high-resolution images, at the cost of generation speed.");
            this.checkboxHiresFix.UseVisualStyleBackColor = true;
            // 
            // cbBaW
            // 
            this.cbBaW.AutoSize = true;
            this.cbBaW.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbBaW.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.cbBaW.Location = new System.Drawing.Point(255, 60);
            this.cbBaW.Name = "cbBaW";
            this.cbBaW.Size = new System.Drawing.Size(98, 21);
            this.cbBaW.TabIndex = 113;
            this.cbBaW.Text = "b&&w image";
            this.toolTip.SetToolTip(this.cbBaW, "Adds an image \"black and white\" filter to the request. (Not supported on some mod" +
        "els)");
            this.cbBaW.UseVisualStyleBackColor = true;
            // 
            // cbDetFace
            // 
            this.cbDetFace.AutoSize = true;
            this.cbDetFace.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbDetFace.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.cbDetFace.Location = new System.Drawing.Point(255, 8);
            this.cbDetFace.Name = "cbDetFace";
            this.cbDetFace.Size = new System.Drawing.Size(113, 21);
            this.cbDetFace.TabIndex = 112;
            this.cbDetFace.Text = "Detailed face";
            this.toolTip.SetToolTip(this.cbDetFace, "Adds hair, eye, and face details to the query.");
            this.cbDetFace.UseVisualStyleBackColor = true;
            // 
            // cbSepia
            // 
            this.cbSepia.AutoSize = true;
            this.cbSepia.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbSepia.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.cbSepia.Location = new System.Drawing.Point(255, 34);
            this.cbSepia.Name = "cbSepia";
            this.cbSepia.Size = new System.Drawing.Size(66, 21);
            this.cbSepia.TabIndex = 114;
            this.cbSepia.Text = "Sepia";
            this.toolTip.SetToolTip(this.cbSepia, "Adds an image sepia filter to the request. (Not supported on some models)");
            this.cbSepia.UseVisualStyleBackColor = true;
            // 
            // labelCurrentImage
            // 
            this.labelCurrentImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCurrentImage.AutoEllipsis = true;
            this.labelCurrentImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurrentImage.ForeColor = System.Drawing.Color.Silver;
            this.labelCurrentImage.Location = new System.Drawing.Point(298, 10);
            this.labelCurrentImage.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.labelCurrentImage.Name = "labelCurrentImage";
            this.labelCurrentImage.Size = new System.Drawing.Size(485, 26);
            this.labelCurrentImage.TabIndex = 91;
            this.labelCurrentImage.MouseEnter += new System.EventHandler(this.labelCurrentImage_MouseEnter);
            this.labelCurrentImage.MouseLeave += new System.EventHandler(this.labelCurrentImage_MouseLeave);
            // 
            // menuStripLogs
            // 
            this.menuStripLogs.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStripLogs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewLogInRealtimeToolStripMenuItem});
            this.menuStripLogs.Name = "menuStripLogs";
            this.menuStripLogs.ShowImageMargin = false;
            this.menuStripLogs.Size = new System.Drawing.Size(194, 28);
            // 
            // viewLogInRealtimeToolStripMenuItem
            // 
            this.viewLogInRealtimeToolStripMenuItem.Name = "viewLogInRealtimeToolStripMenuItem";
            this.viewLogInRealtimeToolStripMenuItem.Size = new System.Drawing.Size(193, 24);
            this.viewLogInRealtimeToolStripMenuItem.Text = "View Log In Realtime";
            this.viewLogInRealtimeToolStripMenuItem.Click += new System.EventHandler(this.viewLogInRealtimeToolStripMenuItem_Click);
            // 
            // progressBarImg
            // 
            this.progressBarImg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarImg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.progressBarImg.BorderThickness = 0;
            this.progressBarImg.Location = new System.Drawing.Point(922, 921);
            this.progressBarImg.Margin = new System.Windows.Forms.Padding(0);
            this.progressBarImg.Name = "progressBarImg";
            this.progressBarImg.Size = new System.Drawing.Size(669, 12);
            this.progressBarImg.TabIndex = 110;
            this.progressBarImg.TabStop = false;
            this.progressBarImg.Visible = false;
            // 
            // menuStripRunQueue
            // 
            this.menuStripRunQueue.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStripRunQueue.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateCurrentPromptToolStripMenuItem,
            this.generateAllQueuedPromptsToolStripMenuItem});
            this.menuStripRunQueue.Name = "menuStripRunQueue";
            this.menuStripRunQueue.ShowImageMargin = false;
            this.menuStripRunQueue.Size = new System.Drawing.Size(251, 52);
            // 
            // generateCurrentPromptToolStripMenuItem
            // 
            this.generateCurrentPromptToolStripMenuItem.Name = "generateCurrentPromptToolStripMenuItem";
            this.generateCurrentPromptToolStripMenuItem.Size = new System.Drawing.Size(250, 24);
            this.generateCurrentPromptToolStripMenuItem.Text = "Generate Current Prompt";
            this.generateCurrentPromptToolStripMenuItem.Click += new System.EventHandler(this.generateCurrentPromptToolStripMenuItem_Click);
            // 
            // generateAllQueuedPromptsToolStripMenuItem
            // 
            this.generateAllQueuedPromptsToolStripMenuItem.Name = "generateAllQueuedPromptsToolStripMenuItem";
            this.generateAllQueuedPromptsToolStripMenuItem.Size = new System.Drawing.Size(250, 24);
            this.generateAllQueuedPromptsToolStripMenuItem.Text = "Generate All Queued Prompts";
            this.generateAllQueuedPromptsToolStripMenuItem.Click += new System.EventHandler(this.generateAllQueuedPromptsToolStripMenuItem_Click);
            // 
            // menuStripAddToQueue
            // 
            this.menuStripAddToQueue.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStripAddToQueue.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addCurrentSettingsToQueueToolStripMenuItem});
            this.menuStripAddToQueue.Name = "menuStripAddToQueue";
            this.menuStripAddToQueue.ShowImageMargin = false;
            this.menuStripAddToQueue.Size = new System.Drawing.Size(256, 28);
            // 
            // addCurrentSettingsToQueueToolStripMenuItem
            // 
            this.addCurrentSettingsToQueueToolStripMenuItem.Name = "addCurrentSettingsToQueueToolStripMenuItem";
            this.addCurrentSettingsToQueueToolStripMenuItem.Size = new System.Drawing.Size(255, 24);
            this.addCurrentSettingsToQueueToolStripMenuItem.Text = "Add Current Settings to Queue";
            this.addCurrentSettingsToQueueToolStripMenuItem.Click += new System.EventHandler(this.addCurrentSettingsToQueueToolStripMenuItem_Click);
            // 
            // menuStripDeleteImages
            // 
            this.menuStripDeleteImages.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStripDeleteImages.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteThisImageToolStripMenuItem,
            this.deleteAllCurrentImagesToolStripMenuItem});
            this.menuStripDeleteImages.Name = "menuStripDeleteImages";
            this.menuStripDeleteImages.ShowImageMargin = false;
            this.menuStripDeleteImages.Size = new System.Drawing.Size(224, 52);
            // 
            // deleteThisImageToolStripMenuItem
            // 
            this.deleteThisImageToolStripMenuItem.Name = "deleteThisImageToolStripMenuItem";
            this.deleteThisImageToolStripMenuItem.Size = new System.Drawing.Size(223, 24);
            this.deleteThisImageToolStripMenuItem.Text = "Delete This Image";
            this.deleteThisImageToolStripMenuItem.Click += new System.EventHandler(this.deleteThisImageToolStripMenuItem_Click);
            // 
            // deleteAllCurrentImagesToolStripMenuItem
            // 
            this.deleteAllCurrentImagesToolStripMenuItem.Name = "deleteAllCurrentImagesToolStripMenuItem";
            this.deleteAllCurrentImagesToolStripMenuItem.Size = new System.Drawing.Size(223, 24);
            this.deleteAllCurrentImagesToolStripMenuItem.Text = "Delete All Current Images";
            this.deleteAllCurrentImagesToolStripMenuItem.Click += new System.EventHandler(this.deleteAllCurrentImagesToolStripMenuItem_Click);
            // 
            // menuStripDevTools
            // 
            this.menuStripDevTools.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStripDevTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openCliToolStripMenuItem,
            this.openCmdInPythonEnvironmentToolStripMenuItem,
            this.openModelMergeToolToolStripMenuItem,
            this.openModelPruningTrimmingToolToolStripMenuItem,
            this.convertModelsToolStripMenuItem});
            this.menuStripDevTools.Name = "menuStripDevTools";
            this.menuStripDevTools.ShowImageMargin = false;
            this.menuStripDevTools.Size = new System.Drawing.Size(279, 124);
            // 
            // openCliToolStripMenuItem
            // 
            this.openCliToolStripMenuItem.Name = "openCliToolStripMenuItem";
            this.openCliToolStripMenuItem.Size = new System.Drawing.Size(278, 24);
            this.openCliToolStripMenuItem.Text = "Open Stable Diffusion CLI";
            this.openCliToolStripMenuItem.Click += new System.EventHandler(this.openDreampyCLIToolStripMenuItem_Click);
            // 
            // openCmdInPythonEnvironmentToolStripMenuItem
            // 
            this.openCmdInPythonEnvironmentToolStripMenuItem.Name = "openCmdInPythonEnvironmentToolStripMenuItem";
            this.openCmdInPythonEnvironmentToolStripMenuItem.Size = new System.Drawing.Size(278, 24);
            this.openCmdInPythonEnvironmentToolStripMenuItem.Text = "Open CMD in Python Environment";
            this.openCmdInPythonEnvironmentToolStripMenuItem.Click += new System.EventHandler(this.openCmdInPythonEnvironmentToolStripMenuItem_Click);
            // 
            // openModelMergeToolToolStripMenuItem
            // 
            this.openModelMergeToolToolStripMenuItem.Name = "openModelMergeToolToolStripMenuItem";
            this.openModelMergeToolToolStripMenuItem.Size = new System.Drawing.Size(278, 24);
            this.openModelMergeToolToolStripMenuItem.Text = "Merge Models";
            this.openModelMergeToolToolStripMenuItem.Click += new System.EventHandler(this.openModelMergeToolToolStripMenuItem_Click);
            // 
            // openModelPruningTrimmingToolToolStripMenuItem
            // 
            this.openModelPruningTrimmingToolToolStripMenuItem.Name = "openModelPruningTrimmingToolToolStripMenuItem";
            this.openModelPruningTrimmingToolToolStripMenuItem.Size = new System.Drawing.Size(278, 24);
            this.openModelPruningTrimmingToolToolStripMenuItem.Text = "Prune (Trim) Models";
            this.openModelPruningTrimmingToolToolStripMenuItem.Click += new System.EventHandler(this.openModelPruningTrimmingToolToolStripMenuItem_Click);
            // 
            // convertModelsToolStripMenuItem
            // 
            this.convertModelsToolStripMenuItem.Name = "convertModelsToolStripMenuItem";
            this.convertModelsToolStripMenuItem.Size = new System.Drawing.Size(278, 24);
            this.convertModelsToolStripMenuItem.Text = "Convert Models";
            this.convertModelsToolStripMenuItem.Click += new System.EventHandler(this.convertModelsToolStripMenuItem_Click);
            // 
            // menuStripPostProcess
            // 
            this.menuStripPostProcess.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStripPostProcess.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.upscaleToolStripMenuItem,
            this.faceRestorationToolStripMenuItem,
            this.applyAllToolStripMenuItem});
            this.menuStripPostProcess.Name = "menuStripPostProcess";
            this.menuStripPostProcess.ShowImageMargin = false;
            this.menuStripPostProcess.Size = new System.Drawing.Size(206, 76);
            // 
            // upscaleToolStripMenuItem
            // 
            this.upscaleToolStripMenuItem.Name = "upscaleToolStripMenuItem";
            this.upscaleToolStripMenuItem.Size = new System.Drawing.Size(205, 24);
            this.upscaleToolStripMenuItem.Text = "Apply Upscaling";
            this.upscaleToolStripMenuItem.Click += new System.EventHandler(this.upscaleToolStripMenuItem_Click);
            // 
            // faceRestorationToolStripMenuItem
            // 
            this.faceRestorationToolStripMenuItem.Name = "faceRestorationToolStripMenuItem";
            this.faceRestorationToolStripMenuItem.Size = new System.Drawing.Size(205, 24);
            this.faceRestorationToolStripMenuItem.Text = "Apply Face Restoration";
            this.faceRestorationToolStripMenuItem.Click += new System.EventHandler(this.applyFaceRestorationToolStripMenuItem_Click);
            // 
            // applyAllToolStripMenuItem
            // 
            this.applyAllToolStripMenuItem.Name = "applyAllToolStripMenuItem";
            this.applyAllToolStripMenuItem.Size = new System.Drawing.Size(205, 24);
            this.applyAllToolStripMenuItem.Text = "Apply All";
            this.applyAllToolStripMenuItem.Click += new System.EventHandler(this.applyAllToolStripMenuItem_Click);
            // 
            // panelSettings
            // 
            this.panelSettings.AllowScrolling = true;
            this.panelSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelSettings.AutoScroll = true;
            this.panelSettings.Controls.Add(this.panelRes);
            this.panelSettings.Controls.Add(this.panelIterations);
            this.panelSettings.Controls.Add(this.panelInitImgStrength);
            this.panelSettings.Controls.Add(this.panelPromptNeg);
            this.panelSettings.Controls.Add(this.panelDebugPerlinThresh);
            this.panelSettings.Controls.Add(this.panelDebugSendStdin);
            this.panelSettings.Controls.Add(this.panelSeamless);
            this.panelSettings.Controls.Add(this.panelSeed);
            this.panelSettings.Controls.Add(this.panelScaleImg);
            this.panelSettings.Controls.Add(this.panelScale);
            this.panelSettings.Controls.Add(this.panelSteps);
            this.panelSettings.Controls.Add(this.panelInpainting);
            this.panelSettings.Controls.Add(this.panelAiInputs);
            this.panelSettings.Controls.Add(this.panelTest);
            this.panelSettings.Controls.Add(this.panelPrompt);
            this.panelSettings.CtrlDisablesScrolling = true;
            this.panelSettings.Location = new System.Drawing.Point(16, 76);
            this.panelSettings.Margin = new System.Windows.Forms.Padding(4);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.OnlyAllowScrollIfNeeded = true;
            this.panelSettings.Size = new System.Drawing.Size(899, 859);
            this.panelSettings.TabIndex = 106;
            this.panelSettings.SizeChanged += new System.EventHandler(this.panelSettings_SizeChanged);
            this.panelSettings.Click += new System.EventHandler(this.panel1_Click);
            // 
            // panelRes
            // 
            this.panelRes.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelRes.Location = new System.Drawing.Point(0, 998);
            this.panelRes.Margin = new System.Windows.Forms.Padding(4);
            this.panelRes.Name = "panelRes";
            this.panelRes.Size = new System.Drawing.Size(878, 43);
            this.panelRes.TabIndex = 5;
            // 
            // panelIterations
            // 
            this.panelIterations.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelIterations.Location = new System.Drawing.Point(0, 955);
            this.panelIterations.Margin = new System.Windows.Forms.Padding(4);
            this.panelIterations.Name = "panelIterations";
            this.panelIterations.Size = new System.Drawing.Size(878, 43);
            this.panelIterations.TabIndex = 1;
            // 
            // panelInitImgStrength
            // 
            this.panelInitImgStrength.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInitImgStrength.Location = new System.Drawing.Point(0, 912);
            this.panelInitImgStrength.Margin = new System.Windows.Forms.Padding(4);
            this.panelInitImgStrength.Name = "panelInitImgStrength";
            this.panelInitImgStrength.Size = new System.Drawing.Size(878, 43);
            this.panelInitImgStrength.TabIndex = 8;
            this.panelInitImgStrength.Visible = false;
            // 
            // panelPromptNeg
            // 
            this.panelPromptNeg.Controls.Add(this.cbXYPilot);
            this.panelPromptNeg.Controls.Add(this.tbYPilot);
            this.panelPromptNeg.Controls.Add(this.tbXPilot);
            this.panelPromptNeg.Controls.Add(this.label21);
            this.panelPromptNeg.Controls.Add(this.label20);
            this.panelPromptNeg.Controls.Add(this.cbYPilot);
            this.panelPromptNeg.Controls.Add(this.cbXPilot);
            this.panelPromptNeg.Controls.Add(this.label7);
            this.panelPromptNeg.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPromptNeg.Location = new System.Drawing.Point(0, 759);
            this.panelPromptNeg.Margin = new System.Windows.Forms.Padding(4);
            this.panelPromptNeg.Name = "panelPromptNeg";
            this.panelPromptNeg.Padding = new System.Windows.Forms.Padding(4);
            this.panelPromptNeg.Size = new System.Drawing.Size(878, 153);
            this.panelPromptNeg.TabIndex = 16;
            // 
            // panelDebugPerlinThresh
            // 
            this.panelDebugPerlinThresh.Controls.Add(this.textboxThresh);
            this.panelDebugPerlinThresh.Controls.Add(this.label15);
            this.panelDebugPerlinThresh.Controls.Add(this.label14);
            this.panelDebugPerlinThresh.Controls.Add(this.label13);
            this.panelDebugPerlinThresh.Controls.Add(this.textboxPerlin);
            this.panelDebugPerlinThresh.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDebugPerlinThresh.Location = new System.Drawing.Point(0, 706);
            this.panelDebugPerlinThresh.Margin = new System.Windows.Forms.Padding(4);
            this.panelDebugPerlinThresh.Name = "panelDebugPerlinThresh";
            this.panelDebugPerlinThresh.Size = new System.Drawing.Size(878, 53);
            this.panelDebugPerlinThresh.TabIndex = 18;
            this.panelDebugPerlinThresh.Visible = false;
            // 
            // textboxThresh
            // 
            this.textboxThresh.AllowDrop = true;
            this.promptAutocomplete.SetAutocompleteMenu(this.textboxThresh, null);
            this.textboxThresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textboxThresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textboxThresh.ForeColor = System.Drawing.Color.White;
            this.textboxThresh.Location = new System.Drawing.Point(627, 9);
            this.textboxThresh.Margin = new System.Windows.Forms.Padding(4);
            this.textboxThresh.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxThresh.Name = "textboxThresh";
            this.textboxThresh.Size = new System.Drawing.Size(132, 24);
            this.textboxThresh.TabIndex = 108;
            this.textboxThresh.Text = "0";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(547, 14);
            this.label15.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(74, 18);
            this.label15.TabIndex = 107;
            this.label15.Text = "Threshold";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(307, 14);
            this.label14.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(88, 18);
            this.label14.TabIndex = 106;
            this.label14.Text = "Perlin Noise";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(3, 14);
            this.label13.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(201, 18);
            this.label13.TabIndex = 105;
            this.label13.Text = "Set Perlin Noise + Threshold ";
            // 
            // textboxPerlin
            // 
            this.textboxPerlin.AllowDrop = true;
            this.promptAutocomplete.SetAutocompleteMenu(this.textboxPerlin, null);
            this.textboxPerlin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textboxPerlin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textboxPerlin.ForeColor = System.Drawing.Color.White;
            this.textboxPerlin.Location = new System.Drawing.Point(399, 9);
            this.textboxPerlin.Margin = new System.Windows.Forms.Padding(4);
            this.textboxPerlin.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxPerlin.Name = "textboxPerlin";
            this.textboxPerlin.Size = new System.Drawing.Size(132, 24);
            this.textboxPerlin.TabIndex = 4;
            this.textboxPerlin.Text = "0.0";
            // 
            // panelDebugSendStdin
            // 
            this.panelDebugSendStdin.Controls.Add(this.label12);
            this.panelDebugSendStdin.Controls.Add(this.textboxCliTest);
            this.panelDebugSendStdin.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDebugSendStdin.Location = new System.Drawing.Point(0, 663);
            this.panelDebugSendStdin.Margin = new System.Windows.Forms.Padding(4);
            this.panelDebugSendStdin.Name = "panelDebugSendStdin";
            this.panelDebugSendStdin.Size = new System.Drawing.Size(878, 43);
            this.panelDebugSendStdin.TabIndex = 14;
            this.panelDebugSendStdin.Visible = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(3, 14);
            this.label12.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(231, 18);
            this.label12.TabIndex = 105;
            this.label12.Text = "Send stdin to running InvokeAI CLI";
            // 
            // textboxCliTest
            // 
            this.textboxCliTest.AllowDrop = true;
            this.textboxCliTest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.promptAutocomplete.SetAutocompleteMenu(this.textboxCliTest, null);
            this.textboxCliTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textboxCliTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textboxCliTest.ForeColor = System.Drawing.Color.White;
            this.textboxCliTest.Location = new System.Drawing.Point(311, 9);
            this.textboxCliTest.Margin = new System.Windows.Forms.Padding(4);
            this.textboxCliTest.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxCliTest.Name = "textboxCliTest";
            this.textboxCliTest.Size = new System.Drawing.Size(562, 24);
            this.textboxCliTest.TabIndex = 4;
            this.textboxCliTest.DoubleClick += new System.EventHandler(this.textboxCliTest_DoubleClick);
            // 
            // panelSeamless
            // 
            this.panelSeamless.Controls.Add(this.comboxSeamless);
            this.panelSeamless.Controls.Add(this.label8);
            this.panelSeamless.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSeamless.Location = new System.Drawing.Point(0, 620);
            this.panelSeamless.Margin = new System.Windows.Forms.Padding(4);
            this.panelSeamless.Name = "panelSeamless";
            this.panelSeamless.Size = new System.Drawing.Size(878, 43);
            this.panelSeamless.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(3, 14);
            this.label8.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(255, 18);
            this.label8.TabIndex = 105;
            this.label8.Text = "Generate Seamless (Tileable) Images";
            // 
            // panelSeed
            // 
            this.panelSeed.Controls.Add(this.checkboxLoopback);
            this.panelSeed.Controls.Add(this.label18);
            this.panelSeed.Controls.Add(this.label16);
            this.panelSeed.Controls.Add(this.htSwitch1);
            this.panelSeed.Controls.Add(this.label5);
            this.panelSeed.Controls.Add(this.upDownSeed);
            this.panelSeed.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSeed.Location = new System.Drawing.Point(0, 577);
            this.panelSeed.Margin = new System.Windows.Forms.Padding(4);
            this.panelSeed.Name = "panelSeed";
            this.panelSeed.Size = new System.Drawing.Size(878, 43);
            this.panelSeed.TabIndex = 4;
            // 
            // checkboxLoopback
            // 
            this.checkboxLoopback.AutoSize = true;
            this.checkboxLoopback.ForeColor = System.Drawing.Color.White;
            this.checkboxLoopback.Location = new System.Drawing.Point(854, 12);
            this.checkboxLoopback.Margin = new System.Windows.Forms.Padding(4);
            this.checkboxLoopback.Name = "checkboxLoopback";
            this.checkboxLoopback.Padding = new System.Windows.Forms.Padding(4);
            this.checkboxLoopback.Size = new System.Drawing.Size(26, 25);
            this.checkboxLoopback.TabIndex = 110;
            this.checkboxLoopback.UseVisualStyleBackColor = true;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label18.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label18.Location = new System.Drawing.Point(521, 12);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(71, 20);
            this.label18.TabIndex = 94;
            this.label18.Text = "Random";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.White;
            this.label16.Location = new System.Drawing.Point(718, 14);
            this.label16.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(134, 18);
            this.label16.TabIndex = 105;
            this.label16.Text = "Loopback Img2Img";
            // 
            // htSwitch1
            // 
            this.htSwitch1.BackColor = System.Drawing.Color.Transparent;
            this.htSwitch1.Checked = true;
            this.htSwitch1.Location = new System.Drawing.Point(464, 12);
            this.htSwitch1.Name = "htSwitch1";
            this.htSwitch1.Size = new System.Drawing.Size(50, 19);
            this.htSwitch1.TabIndex = 93;
            this.htSwitch1.CheckedChanged += new HTAlt.WinForms.HTSwitch.CheckedChangedDelegate(this.htSwitch1_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(3, 14);
            this.label5.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(172, 18);
            this.label5.TabIndex = 92;
            this.label5.Text = "Seed (Empty = Random)";
            // 
            // panelScaleImg
            // 
            this.panelScaleImg.Controls.Add(this.label17);
            this.panelScaleImg.Controls.Add(this.tableLayoutPanel2);
            this.panelScaleImg.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelScaleImg.Location = new System.Drawing.Point(0, 534);
            this.panelScaleImg.Margin = new System.Windows.Forms.Padding(4);
            this.panelScaleImg.Name = "panelScaleImg";
            this.panelScaleImg.Size = new System.Drawing.Size(878, 43);
            this.panelScaleImg.TabIndex = 20;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.White;
            this.label17.Location = new System.Drawing.Point(3, 14);
            this.label17.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(202, 18);
            this.label17.TabIndex = 90;
            this.label17.Text = "Image Guidance (CFG Scale)";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.Controls.Add(this.textboxSliderScaleImg, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.sliderScaleImg, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.textboxExtraScalesImg, 2, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(311, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(567, 43);
            this.tableLayoutPanel2.TabIndex = 91;
            // 
            // panelScale
            // 
            this.panelScale.Controls.Add(this.label4);
            this.panelScale.Controls.Add(this.tableLayoutPanel1);
            this.panelScale.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelScale.Location = new System.Drawing.Point(0, 491);
            this.panelScale.Margin = new System.Windows.Forms.Padding(4);
            this.panelScale.Name = "panelScale";
            this.panelScale.Size = new System.Drawing.Size(878, 43);
            this.panelScale.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(3, 14);
            this.label4.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(211, 18);
            this.label4.TabIndex = 90;
            this.label4.Text = "Prompt Guidance (CFG Scale)";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.Controls.Add(this.textboxSliderScale, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.sliderScale, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textboxExtraScales, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(311, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(567, 43);
            this.tableLayoutPanel1.TabIndex = 91;
            // 
            // panelSteps
            // 
            this.panelSteps.Controls.Add(this.label3);
            this.panelSteps.Controls.Add(this.tableLayoutPanel6);
            this.panelSteps.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSteps.Location = new System.Drawing.Point(0, 448);
            this.panelSteps.Margin = new System.Windows.Forms.Padding(4);
            this.panelSteps.Name = "panelSteps";
            this.panelSteps.Size = new System.Drawing.Size(878, 43);
            this.panelSteps.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(3, 14);
            this.label3.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 18);
            this.label3.TabIndex = 89;
            this.label3.Text = "Generation Steps";
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel6.ColumnCount = 3;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel6.Controls.Add(this.textboxExtraSteps, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.textboxSliderSteps, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.sliderSteps, 0, 0);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(311, 0);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(567, 43);
            this.tableLayoutPanel6.TabIndex = 88;
            // 
            // textboxSliderSteps
            // 
            this.textboxSliderSteps.AllowDrop = true;
            this.textboxSliderSteps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.promptAutocomplete.SetAutocompleteMenu(this.textboxSliderSteps, null);
            this.textboxSliderSteps.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.textboxSliderSteps.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textboxSliderSteps.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textboxSliderSteps.ForeColor = System.Drawing.Color.Silver;
            this.textboxSliderSteps.Location = new System.Drawing.Point(420, 11);
            this.textboxSliderSteps.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.textboxSliderSteps.MinimumSize = new System.Drawing.Size(5, 21);
            this.textboxSliderSteps.Name = "textboxSliderSteps";
            this.textboxSliderSteps.Size = new System.Drawing.Size(47, 22);
            this.textboxSliderSteps.TabIndex = 92;
            this.textboxSliderSteps.Text = "100";
            // 
            // sliderSteps
            // 
            this.sliderSteps.ActualMaximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.sliderSteps.ActualMinimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.sliderSteps.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.sliderSteps.BorderRoundRectSize = new System.Drawing.Size(12, 12);
            this.sliderSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sliderSteps.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.sliderSteps.ForeColor = System.Drawing.Color.Black;
            this.sliderSteps.InitValue = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.sliderSteps.LargeChange = ((uint)(5u));
            this.sliderSteps.Location = new System.Drawing.Point(0, 0);
            this.sliderSteps.Margin = new System.Windows.Forms.Padding(0);
            this.sliderSteps.Maximum = 500;
            this.sliderSteps.Minimum = 1;
            this.sliderSteps.Name = "sliderSteps";
            this.sliderSteps.OverlayColor = System.Drawing.Color.White;
            this.sliderSteps.Size = new System.Drawing.Size(420, 43);
            this.sliderSteps.SmallChange = ((uint)(1u));
            this.sliderSteps.TabIndex = 13;
            this.sliderSteps.Text = "sliderSteps";
            this.sliderSteps.ThumbRoundRectSize = new System.Drawing.Size(16, 16);
            this.sliderSteps.ThumbSize = new System.Drawing.Size(14, 14);
            this.sliderSteps.Value = 20;
            this.sliderSteps.ValueBox = this.textboxSliderSteps;
            this.sliderSteps.ValueStep = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // panelInpainting
            // 
            this.panelInpainting.Controls.Add(this.textboxClipsegMask);
            this.panelInpainting.Controls.Add(this.comboxInpaintMode);
            this.panelInpainting.Controls.Add(this.btnEditMask);
            this.panelInpainting.Controls.Add(this.btnResetMask);
            this.panelInpainting.Controls.Add(this.label10);
            this.panelInpainting.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInpainting.Location = new System.Drawing.Point(0, 405);
            this.panelInpainting.Margin = new System.Windows.Forms.Padding(4);
            this.panelInpainting.Name = "panelInpainting";
            this.panelInpainting.Size = new System.Drawing.Size(878, 43);
            this.panelInpainting.TabIndex = 12;
            this.panelInpainting.Visible = false;
            // 
            // comboxInpaintMode
            // 
            this.comboxInpaintMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxInpaintMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxInpaintMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxInpaintMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboxInpaintMode.ForeColor = System.Drawing.Color.White;
            this.comboxInpaintMode.FormattingEnabled = true;
            this.comboxInpaintMode.Location = new System.Drawing.Point(311, 9);
            this.comboxInpaintMode.Margin = new System.Windows.Forms.Padding(4);
            this.comboxInpaintMode.Name = "comboxInpaintMode";
            this.comboxInpaintMode.Size = new System.Drawing.Size(265, 26);
            this.comboxInpaintMode.TabIndex = 109;
            this.comboxInpaintMode.SelectedIndexChanged += new System.EventHandler(this.comboxInpaintMode_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(3, 14);
            this.label10.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(69, 18);
            this.label10.TabIndex = 105;
            this.label10.Text = "Inpainting";
            // 
            // panelAiInputs
            // 
            this.panelAiInputs.Controls.Add(this.labelCurrentImage);
            this.panelAiInputs.Controls.Add(this.btnInitImgBrowse);
            this.panelAiInputs.Controls.Add(this.btnEmbeddingBrowse);
            this.panelAiInputs.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelAiInputs.Location = new System.Drawing.Point(0, 358);
            this.panelAiInputs.Margin = new System.Windows.Forms.Padding(4);
            this.panelAiInputs.Name = "panelAiInputs";
            this.panelAiInputs.Size = new System.Drawing.Size(878, 47);
            this.panelAiInputs.TabIndex = 17;
            // 
            // panelTest
            // 
            this.panelTest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTest.Controls.Add(this.labelAspectRatio);
            this.panelTest.Controls.Add(this.checkboxHiresFix);
            this.panelTest.Controls.Add(this.label1);
            this.panelTest.Controls.Add(this.upDownIterations);
            this.panelTest.Controls.Add(this.pictBoxInitImg);
            this.panelTest.Controls.Add(this.textboxSliderInitStrength);
            this.panelTest.Controls.Add(this.label11);
            this.panelTest.Controls.Add(this.sliderInitStrength);
            this.panelTest.Controls.Add(this.label6);
            this.panelTest.Controls.Add(this.comboxResH);
            this.panelTest.Controls.Add(this.cbBaW);
            this.panelTest.Controls.Add(this.cbDetFace);
            this.panelTest.Controls.Add(this.cbSepia);
            this.panelTest.Controls.Add(this.comboxResW);
            this.panelTest.Controls.Add(this.progressCircle);
            this.panelTest.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTest.Location = new System.Drawing.Point(0, 143);
            this.panelTest.Margin = new System.Windows.Forms.Padding(4);
            this.panelTest.Name = "panelTest";
            this.panelTest.Size = new System.Drawing.Size(878, 215);
            this.panelTest.TabIndex = 19;
            // 
            // labelAspectRatio
            // 
            this.labelAspectRatio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelAspectRatio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAspectRatio.ForeColor = System.Drawing.Color.Silver;
            this.labelAspectRatio.Location = new System.Drawing.Point(773, 8);
            this.labelAspectRatio.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAspectRatio.Name = "labelAspectRatio";
            this.labelAspectRatio.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelAspectRatio.Size = new System.Drawing.Size(80, 27);
            this.labelAspectRatio.TabIndex = 110;
            this.labelAspectRatio.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(676, 81);
            this.label1.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 18);
            this.label1.TabIndex = 85;
            this.label1.Text = "Out images count";
            // 
            // pictBoxInitImg
            // 
            this.pictBoxInitImg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictBoxInitImg.Location = new System.Drawing.Point(5, 6);
            this.pictBoxInitImg.Name = "pictBoxInitImg";
            this.pictBoxInitImg.Size = new System.Drawing.Size(232, 201);
            this.pictBoxInitImg.TabIndex = 1;
            this.pictBoxInitImg.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(248, 156);
            this.label11.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(258, 18);
            this.label11.TabIndex = 90;
            this.label11.Text = "Initialization Image Strength (Influence)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(686, 7);
            this.label6.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 18);
            this.label6.TabIndex = 95;
            this.label6.Text = "Size";
            // 
            // comboxResH
            // 
            this.comboxResH.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxResH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxResH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxResH.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboxResH.ForeColor = System.Drawing.Color.White;
            this.comboxResH.FormattingEnabled = true;
            this.comboxResH.Location = new System.Drawing.Point(790, 37);
            this.comboxResH.Margin = new System.Windows.Forms.Padding(4);
            this.comboxResH.Name = "comboxResH";
            this.comboxResH.Size = new System.Drawing.Size(80, 26);
            this.comboxResH.TabIndex = 107;
            this.comboxResH.SelectedIndexChanged += new System.EventHandler(this.comboxResH_SelectedIndexChanged);
            // 
            // comboxResW
            // 
            this.comboxResW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxResW.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxResW.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxResW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboxResW.ForeColor = System.Drawing.Color.White;
            this.comboxResW.FormattingEnabled = true;
            this.comboxResW.Location = new System.Drawing.Point(693, 37);
            this.comboxResW.Margin = new System.Windows.Forms.Padding(4);
            this.comboxResW.Name = "comboxResW";
            this.comboxResW.Size = new System.Drawing.Size(85, 26);
            this.comboxResW.TabIndex = 106;
            this.comboxResW.SelectedIndexChanged += new System.EventHandler(this.comboxResW_SelectedIndexChanged);
            // 
            // panelPrompt
            // 
            this.panelPrompt.Controls.Add(this.textboxPromptNeg);
            this.panelPrompt.Controls.Add(this.textboxPrompt);
            this.panelPrompt.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPrompt.Location = new System.Drawing.Point(0, 0);
            this.panelPrompt.Margin = new System.Windows.Forms.Padding(4);
            this.panelPrompt.Name = "panelPrompt";
            this.panelPrompt.Padding = new System.Windows.Forms.Padding(4);
            this.panelPrompt.Size = new System.Drawing.Size(878, 143);
            this.panelPrompt.TabIndex = 15;
            // 
            // promptAutocomplete
            // 
            this.promptAutocomplete.AllowsTabKey = true;
            this.promptAutocomplete.AppearInterval = 250;
            this.promptAutocomplete.Colors = ((AutocompleteMenuNS.Colors)(resources.GetObject("promptAutocomplete.Colors")));
            this.promptAutocomplete.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.promptAutocomplete.ImageList = null;
            this.promptAutocomplete.Items = new string[0];
            this.promptAutocomplete.LeftPadding = 0;
            this.promptAutocomplete.MaximumSize = new System.Drawing.Size(400, 150);
            this.promptAutocomplete.MinFragmentLength = 100;
            this.promptAutocomplete.SearchPattern = "[\\w\\.-]";
            this.promptAutocomplete.TargetControlWrapper = null;
            // 
            // pictBoxImgViewer
            // 
            this.pictBoxImgViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictBoxImgViewer.Image = ((System.Drawing.Image)(resources.GetObject("pictBoxImgViewer.Image")));
            this.pictBoxImgViewer.Location = new System.Drawing.Point(0, 0);
            this.pictBoxImgViewer.Margin = new System.Windows.Forms.Padding(0);
            this.pictBoxImgViewer.Name = "pictBoxImgViewer";
            this.pictBoxImgViewer.Size = new System.Drawing.Size(667, 667);
            this.pictBoxImgViewer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictBoxImgViewer.TabIndex = 113;
            this.pictBoxImgViewer.TabStop = false;
            this.pictBoxImgViewer.Click += new System.EventHandler(this.pictBoxImgViewer_Click);
            // 
            // separator
            // 
            this.separator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.separator.BackColor = System.Drawing.Color.Transparent;
            this.separator.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.separator.Enabled = false;
            this.separator.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.separator.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.separator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.separator.Location = new System.Drawing.Point(1168, 15);
            this.separator.Margin = new System.Windows.Forms.Padding(4);
            this.separator.Name = "separator";
            this.separator.Size = new System.Drawing.Size(53, 49);
            this.separator.TabIndex = 75;
            this.separator.TabStop = false;
            this.separator.UseVisualStyleBackColor = false;
            // 
            // tableLayoutPanelImgViewers
            // 
            this.tableLayoutPanelImgViewers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelImgViewers.ColumnCount = 2;
            this.tableLayoutPanelImgViewers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0F));
            this.tableLayoutPanelImgViewers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelImgViewers.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanelImgViewers.Location = new System.Drawing.Point(922, 118);
            this.tableLayoutPanelImgViewers.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanelImgViewers.Name = "tableLayoutPanelImgViewers";
            this.tableLayoutPanelImgViewers.RowCount = 1;
            this.tableLayoutPanelImgViewers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelImgViewers.Size = new System.Drawing.Size(667, 670);
            this.tableLayoutPanelImgViewers.TabIndex = 119;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnNextImg);
            this.panel1.Controls.Add(this.btnPrevImg);
            this.panel1.Controls.Add(this.pictBoxImgViewer);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(667, 667);
            this.panel1.TabIndex = 0;
            // 
            // comboxSdModel
            // 
            this.comboxSdModel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxSdModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxSdModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxSdModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboxSdModel.ForeColor = System.Drawing.Color.White;
            this.comboxSdModel.FormattingEnabled = true;
            this.comboxSdModel.Location = new System.Drawing.Point(139, 35);
            this.comboxSdModel.Margin = new System.Windows.Forms.Padding(4);
            this.comboxSdModel.Name = "comboxSdModel";
            this.comboxSdModel.Size = new System.Drawing.Size(337, 24);
            this.comboxSdModel.TabIndex = 112;
            this.comboxSdModel.SelectedIndexChanged += new System.EventHandler(this.comboxSdModel_SelectedIndexChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.White;
            this.label19.Location = new System.Drawing.Point(136, 11);
            this.label19.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(50, 17);
            this.label19.TabIndex = 112;
            this.label19.Text = "Model:";
            // 
            // ImgListView
            // 
            this.ImgListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ImgListView.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ImgListView.HideSelection = false;
            this.ImgListView.Location = new System.Drawing.Point(922, 800);
            this.ImgListView.Name = "ImgListView";
            this.ImgListView.ShowGroups = false;
            this.ImgListView.Size = new System.Drawing.Size(667, 118);
            this.ImgListView.TabIndex = 21;
            this.ImgListView.UseCompatibleStateImageBehavior = false;
            this.ImgListView.SelectedIndexChanged += new System.EventHandler(this.ImgListView_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(483, 9);
            this.label2.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 17);
            this.label2.TabIndex = 120;
            this.label2.Text = "VAE:";
            // 
            // comboxVaeModel
            // 
            this.comboxVaeModel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxVaeModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxVaeModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxVaeModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboxVaeModel.ForeColor = System.Drawing.Color.White;
            this.comboxVaeModel.FormattingEnabled = true;
            this.comboxVaeModel.Location = new System.Drawing.Point(484, 34);
            this.comboxVaeModel.Margin = new System.Windows.Forms.Padding(4);
            this.comboxVaeModel.Name = "comboxVaeModel";
            this.comboxVaeModel.Size = new System.Drawing.Size(237, 26);
            this.comboxVaeModel.TabIndex = 121;
            this.comboxVaeModel.SelectedIndexChanged += new System.EventHandler(this.comboxVaeModel_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(725, 9);
            this.label9.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(76, 17);
            this.label9.TabIndex = 122;
            this.label9.Text = "Scheduler:";
            // 
            // cbScheduler
            // 
            this.cbScheduler.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbScheduler.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbScheduler.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbScheduler.ForeColor = System.Drawing.Color.White;
            this.cbScheduler.FormattingEnabled = true;
            this.cbScheduler.Items.AddRange(new object[] {
            "EulerA",
            "EulerD",
            "PNDM",
            "DDPM",
            "LMSD",
            "DDIM",
            "DPMSM"});
            this.cbScheduler.Location = new System.Drawing.Point(729, 35);
            this.cbScheduler.Margin = new System.Windows.Forms.Padding(4);
            this.cbScheduler.Name = "cbScheduler";
            this.cbScheduler.Size = new System.Drawing.Size(119, 24);
            this.cbScheduler.TabIndex = 123;
            this.cbScheduler.SelectedIndexChanged += new System.EventHandler(this.cbScheduler_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(7, 13);
            this.label7.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(106, 18);
            this.label7.TabIndex = 109;
            this.label7.Text = "X/Y Pilot (WIP)";
            // 
            // cbXPilot
            // 
            this.cbXPilot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbXPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbXPilot.Enabled = false;
            this.cbXPilot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbXPilot.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbXPilot.ForeColor = System.Drawing.Color.White;
            this.cbXPilot.FormattingEnabled = true;
            this.cbXPilot.Items.AddRange(new object[] {
            "None",
            "Seed",
            "Prompt Scale"});
            this.cbXPilot.Location = new System.Drawing.Point(29, 45);
            this.cbXPilot.Margin = new System.Windows.Forms.Padding(4);
            this.cbXPilot.Name = "cbXPilot";
            this.cbXPilot.Size = new System.Drawing.Size(205, 26);
            this.cbXPilot.TabIndex = 108;
            this.toolTip.SetToolTip(this.cbXPilot, "Changes how the image is sampled.\r\nEuler Ancestral works very well at low step co" +
        "unts.");
            this.cbXPilot.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // cbYPilot
            // 
            this.cbYPilot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbYPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbYPilot.Enabled = false;
            this.cbYPilot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbYPilot.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbYPilot.ForeColor = System.Drawing.Color.White;
            this.cbYPilot.FormattingEnabled = true;
            this.cbYPilot.Items.AddRange(new object[] {
            "None",
            "Seed",
            "Prompt Scale"});
            this.cbYPilot.Location = new System.Drawing.Point(29, 89);
            this.cbYPilot.Margin = new System.Windows.Forms.Padding(4);
            this.cbYPilot.Name = "cbYPilot";
            this.cbYPilot.Size = new System.Drawing.Size(205, 26);
            this.cbYPilot.TabIndex = 110;
            this.toolTip.SetToolTip(this.cbYPilot, "Changes how the image is sampled.\r\nEuler Ancestral works very well at low step co" +
        "unts.");
            this.cbYPilot.SelectedIndexChanged += new System.EventHandler(this.cbYPilot_SelectedIndexChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(4, 48);
            this.label20.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(18, 18);
            this.label20.TabIndex = 111;
            this.label20.Text = "X";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.Color.White;
            this.label21.Location = new System.Drawing.Point(3, 92);
            this.label21.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(17, 18);
            this.label21.TabIndex = 112;
            this.label21.Text = "Y";
            // 
            // tbXPilot
            // 
            this.tbXPilot.AllowDrop = true;
            this.tbXPilot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.promptAutocomplete.SetAutocompleteMenu(this.tbXPilot, null);
            this.tbXPilot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tbXPilot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbXPilot.Enabled = false;
            this.tbXPilot.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbXPilot.ForeColor = System.Drawing.Color.White;
            this.tbXPilot.Location = new System.Drawing.Point(311, 43);
            this.tbXPilot.Margin = new System.Windows.Forms.Padding(4);
            this.tbXPilot.MinimumSize = new System.Drawing.Size(5, 21);
            this.tbXPilot.Name = "tbXPilot";
            this.tbXPilot.Size = new System.Drawing.Size(448, 24);
            this.tbXPilot.TabIndex = 113;
            this.toolTip.SetToolTip(this.tbXPilot, resources.GetString("tbXPilot.ToolTip"));
            this.tbXPilot.TextChanged += new System.EventHandler(this.tbXPilot_TextChanged);
            // 
            // tbYPilot
            // 
            this.tbYPilot.AllowDrop = true;
            this.tbYPilot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.promptAutocomplete.SetAutocompleteMenu(this.tbYPilot, null);
            this.tbYPilot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tbYPilot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbYPilot.Enabled = false;
            this.tbYPilot.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbYPilot.ForeColor = System.Drawing.Color.White;
            this.tbYPilot.Location = new System.Drawing.Point(311, 91);
            this.tbYPilot.Margin = new System.Windows.Forms.Padding(4);
            this.tbYPilot.MinimumSize = new System.Drawing.Size(5, 21);
            this.tbYPilot.Name = "tbYPilot";
            this.tbYPilot.Size = new System.Drawing.Size(448, 24);
            this.tbYPilot.TabIndex = 114;
            this.toolTip.SetToolTip(this.tbYPilot, resources.GetString("tbYPilot.ToolTip"));
            this.tbYPilot.TextChanged += new System.EventHandler(this.tbYPilot_TextChanged);
            // 
            // cbXYPilot
            // 
            this.cbXYPilot.AutoSize = true;
            this.cbXYPilot.ForeColor = System.Drawing.Color.White;
            this.cbXYPilot.Location = new System.Drawing.Point(121, 12);
            this.cbXYPilot.Margin = new System.Windows.Forms.Padding(4);
            this.cbXYPilot.Name = "cbXYPilot";
            this.cbXYPilot.Padding = new System.Windows.Forms.Padding(4);
            this.cbXYPilot.Size = new System.Drawing.Size(26, 25);
            this.cbXYPilot.TabIndex = 111;
            this.cbXYPilot.UseVisualStyleBackColor = true;
            this.cbXYPilot.CheckedChanged += new System.EventHandler(this.cbXYPilot_CheckedChanged);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(1605, 1052);
            this.Controls.Add(this.comboxSampler);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cbScheduler);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboxVaeModel);
            this.Controls.Add(this.ImgListView);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.comboxSdModel);
            this.Controls.Add(this.progressBarImg);
            this.Controls.Add(this.tableLayoutPanelImgViewers);
            this.Controls.Add(this.btnDreambooth);
            this.Controls.Add(this.labelImgPromptNeg);
            this.Controls.Add(this.labelImgPrompt);
            this.Controls.Add(this.btnDeleteBatch);
            this.Controls.Add(this.btnPromptHistory);
            this.Controls.Add(this.btnQueue);
            this.Controls.Add(this.btnPostProc);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnDebug);
            this.Controls.Add(this.panelSettings);
            this.Controls.Add(this.cliButton);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnOpenOutFolder);
            this.Controls.Add(this.labelImgInfo);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.installerBtn);
            this.Controls.Add(this.separator);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.runBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stable Diffusion GUI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.menuStripOutputImg.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.upDownSeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDownIterations)).EndInit();
            this.menuStripLogs.ResumeLayout(false);
            this.menuStripRunQueue.ResumeLayout(false);
            this.menuStripAddToQueue.ResumeLayout(false);
            this.menuStripDeleteImages.ResumeLayout(false);
            this.menuStripDevTools.ResumeLayout(false);
            this.menuStripPostProcess.ResumeLayout(false);
            this.panelSettings.ResumeLayout(false);
            this.panelPromptNeg.ResumeLayout(false);
            this.panelPromptNeg.PerformLayout();
            this.panelDebugPerlinThresh.ResumeLayout(false);
            this.panelDebugPerlinThresh.PerformLayout();
            this.panelDebugSendStdin.ResumeLayout(false);
            this.panelDebugSendStdin.PerformLayout();
            this.panelSeamless.ResumeLayout(false);
            this.panelSeamless.PerformLayout();
            this.panelSeed.ResumeLayout(false);
            this.panelSeed.PerformLayout();
            this.panelScaleImg.ResumeLayout(false);
            this.panelScaleImg.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panelScale.ResumeLayout(false);
            this.panelScale.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panelSteps.ResumeLayout(false);
            this.panelSteps.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.panelInpainting.ResumeLayout(false);
            this.panelInpainting.PerformLayout();
            this.panelAiInputs.ResumeLayout(false);
            this.panelTest.ResumeLayout(false);
            this.panelTest.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxInitImg)).EndInit();
            this.panelPrompt.ResumeLayout(false);
            this.panelPrompt.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxImgViewer)).EndInit();
            this.tableLayoutPanelImgViewers.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button separator;
        public System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem openOutputFolderToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem copyImageToClipboardToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem copySeedToClipboardToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem useAsInitImageToolStripMenuItem;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.ToolStripMenuItem generateCurrentPromptToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem generateAllQueuedPromptsToolStripMenuItem;
        private System.Windows.Forms.Label label10;
        private HTAlt.WinForms.HTButton btnResetMask;
        public System.Windows.Forms.ToolStripMenuItem addCurrentSettingsToQueueToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem reGenerateImageWithCurrentSettingsToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem deleteThisImageToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem deleteAllCurrentImagesToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem openCliToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem openModelMergeToolToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem openModelPruningTrimmingToolToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem viewLogInRealtimeToolStripMenuItem;
        private System.Windows.Forms.TextBox textboxSliderSteps;
        private System.Windows.Forms.TextBox textboxSliderScale;
        public System.Windows.Forms.ToolStripMenuItem fitWindowSizeToImageSizeToolStripMenuItem;
        private System.Windows.Forms.Label label12;
        public System.Windows.Forms.ToolStripMenuItem openCmdInPythonEnvironmentToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem copyToFavoritesToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem upscaleToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem faceRestorationToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem applyAllToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem postProcessImageToolStripMenuItem;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        public AutocompleteMenuNS.AutocompleteMenu promptAutocomplete;
        public System.Windows.Forms.ComboBox comboxSampler;
        public CustomTextbox textboxPrompt;
        public System.Windows.Forms.NumericUpDown upDownIterations;
        public CustomSlider sliderScale;
        public System.Windows.Forms.NumericUpDown upDownSeed;
        public HTAlt.WinForms.HTButton btnInitImgBrowse;
        public CustomSlider sliderInitStrength;
        public HTAlt.WinForms.HTButton btnEmbeddingBrowse;
        public CustomSlider sliderSteps;
        public System.Windows.Forms.Label labelImgPrompt;
        public System.Windows.Forms.Label labelImgPromptNeg;
        public System.Windows.Forms.TextBox textboxClipsegMask;
        public System.Windows.Forms.ComboBox comboxInpaintMode;
        public System.Windows.Forms.TextBox textboxCliTest;
        public System.Windows.Forms.ComboBox comboxResH;
        public System.Windows.Forms.ComboBox comboxResW;
        public System.Windows.Forms.ComboBox comboxSeamless;
        public System.Windows.Forms.TextBox textboxThresh;
        public System.Windows.Forms.TextBox textboxPerlin;
        public System.Windows.Forms.Button runBtn;
        public System.Windows.Forms.Button installerBtn;
        public System.Windows.Forms.TextBox logBox;
        public System.Windows.Forms.Button btnNextImg;
        public System.Windows.Forms.Label labelImgInfo;
        public System.Windows.Forms.Button btnPrevImg;
        public System.Windows.Forms.Button btnOpenOutFolder;
        public System.Windows.Forms.Button cliButton;
        public System.Windows.Forms.Button btnDebug;
        public System.Windows.Forms.Button btnSettings;
        public System.Windows.Forms.Button btnPostProc;
        public HTAlt.WinForms.HTProgressBar progressBarImg;
        public System.Windows.Forms.Button btnQueue;
        public System.Windows.Forms.Button btnPromptHistory;
        public System.Windows.Forms.PictureBox pictBoxImgViewer;
        public System.Windows.Forms.Button btnDeleteBatch;
        public System.Windows.Forms.Button btnDreambooth;
        public System.Windows.Forms.ContextMenuStrip menuStripOutputImg;
        public System.Windows.Forms.ToolTip toolTip;
        public System.Windows.Forms.ContextMenuStrip menuStripLogs;
        public System.Windows.Forms.ContextMenuStrip menuStripRunQueue;
        public System.Windows.Forms.ContextMenuStrip menuStripAddToQueue;
        public System.Windows.Forms.ContextMenuStrip menuStripDeleteImages;
        public System.Windows.Forms.ContextMenuStrip menuStripDevTools;
        public System.Windows.Forms.ContextMenuStrip menuStripPostProcess;
        public CustomPanel panelSettings;
        public System.Windows.Forms.Panel panelSteps;
        public System.Windows.Forms.Panel panelIterations;
        public System.Windows.Forms.Panel panelRes;
        public System.Windows.Forms.Panel panelSeed;
        public System.Windows.Forms.Panel panelScale;
        public System.Windows.Forms.Panel panelSeamless;
        public System.Windows.Forms.Panel panelInitImgStrength;
        public System.Windows.Forms.Panel panelInpainting;
        public System.Windows.Forms.Panel panelDebugSendStdin;
        public System.Windows.Forms.Panel panelAiInputs;
        public System.Windows.Forms.Panel panelDebugPerlinThresh;
        public System.Windows.Forms.Panel panelPromptNeg;
        public System.Windows.Forms.Panel panelPrompt;
        public System.Windows.Forms.Label labelCurrentImage;
        public HTAlt.WinForms.HTProgressBar progressBar;
        public CircularProgressBar.CircularProgressBar progressCircle;
        public System.Windows.Forms.TextBox textboxExtraScales;
        public System.Windows.Forms.TextBox textboxExtraSteps;
        public System.Windows.Forms.Panel panelTest;
        public System.Windows.Forms.CheckBox checkboxLoopback;
        private System.Windows.Forms.Label label16;
        private HTAlt.WinForms.HTButton btnEditMask;
        private System.Windows.Forms.ToolStripMenuItem convertModelsToolStripMenuItem;
        public System.Windows.Forms.Panel panelScaleImg;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox textboxSliderScaleImg;
        public CustomSlider sliderScaleImg;
        public System.Windows.Forms.TextBox textboxExtraScalesImg;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanelImgViewers;
        private System.Windows.Forms.Label labelAspectRatio;
        private System.Windows.Forms.ToolStripMenuItem copySidebySideComparisonImageToolStripMenuItem;
        private HTAlt.WinForms.HTSwitch htSwitch1;
        private System.Windows.Forms.Label label18;
        public System.Windows.Forms.ComboBox comboxSdModel;
        private System.Windows.Forms.Label label19;
        public System.Windows.Forms.ListView ImgListView;
        public System.Windows.Forms.CheckBox cbDetFace;
        public System.Windows.Forms.CheckBox cbBaW;
        public System.Windows.Forms.CheckBox cbSepia;
        public System.Windows.Forms.PictureBox pictBoxInitImg;
        public CustomTextbox textboxPromptNeg;
        public System.Windows.Forms.Label label11;
        public System.Windows.Forms.TextBox textboxSliderInitStrength;
        public System.Windows.Forms.CheckBox checkboxHiresFix;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ComboBox comboxVaeModel;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.ComboBox cbScheduler;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        public System.Windows.Forms.ComboBox cbYPilot;
        public System.Windows.Forms.ComboBox cbXPilot;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.TextBox tbYPilot;
        public System.Windows.Forms.TextBox tbXPilot;
        public System.Windows.Forms.CheckBox cbXYPilot;
    }
}

