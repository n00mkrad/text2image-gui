using StableDiffusionGui.Controls;

namespace StableDiffusionGui.Forms
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.runBtn = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.logBox = new StableDiffusionGui.Controls.CustomTextbox();
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
            this.checkboxHiresFix = new System.Windows.Forms.CheckBox();
            this.checkboxLockSeed = new System.Windows.Forms.CheckBox();
            this.btnSeedResetToRandom = new HTAlt.WinForms.HTButton();
            this.btnSeedUsePrevious = new HTAlt.WinForms.HTButton();
            this.upDownSeed = new StableDiffusionGui.Controls.CustomUpDown();
            this.sliderScale = new StableDiffusionGui.Controls.CustomSlider();
            this.textboxSliderScale = new System.Windows.Forms.TextBox();
            this.textboxExtraScales = new StableDiffusionGui.Controls.CustomTextbox();
            this.upDownIterations = new StableDiffusionGui.Controls.CustomUpDown();
            this.btnResetMask = new HTAlt.WinForms.HTButton();
            this.textboxExtraInitStrengths = new StableDiffusionGui.Controls.CustomTextbox();
            this.sliderInitStrength = new StableDiffusionGui.Controls.CustomSlider();
            this.textboxSliderInitStrength = new System.Windows.Forms.TextBox();
            this.btnInitImgBrowse = new HTAlt.WinForms.HTButton();
            this.textboxPromptNeg = new StableDiffusionGui.Controls.CustomTextbox();
            this.textboxPrompt = new StableDiffusionGui.Controls.CustomTextbox();
            this.comboxSeamless = new System.Windows.Forms.ComboBox();
            this.textboxClipsegMask = new System.Windows.Forms.TextBox();
            this.textboxExtraSteps = new StableDiffusionGui.Controls.CustomTextbox();
            this.btnEditMask = new HTAlt.WinForms.HTButton();
            this.sliderScaleImg = new StableDiffusionGui.Controls.CustomSlider();
            this.textboxSliderScaleImg = new System.Windows.Forms.TextBox();
            this.textboxExtraScalesImg = new StableDiffusionGui.Controls.CustomTextbox();
            this.comboxResizeGravity = new System.Windows.Forms.ComboBox();
            this.comboxSymmetry = new System.Windows.Forms.ComboBox();
            this.btnEmbeddingAppend = new HTAlt.WinForms.HTButton();
            this.btnResetRes = new HTAlt.WinForms.HTButton();
            this.comboxModelArch = new System.Windows.Forms.ComboBox();
            this.btnNextImg = new System.Windows.Forms.Button();
            this.btnPrevImg = new System.Windows.Forms.Button();
            this.btnDeleteBatch = new System.Windows.Forms.Button();
            this.btnOpenOutFolder = new System.Windows.Forms.Button();
            this.btnSaveToFavs = new System.Windows.Forms.Button();
            this.btnTrain = new System.Windows.Forms.Button();
            this.btnPromptHistory = new System.Windows.Forms.Button();
            this.btnQueue = new System.Windows.Forms.Button();
            this.btnPostProc = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnDebug = new System.Windows.Forms.Button();
            this.btnExpandLoras = new System.Windows.Forms.Button();
            this.btnExpandPromptNegField = new System.Windows.Forms.Button();
            this.btnExpandPromptField = new System.Windows.Forms.Button();
            this.cliButton = new System.Windows.Forms.Button();
            this.installerBtn = new System.Windows.Forms.Button();
            this.discordBtn = new System.Windows.Forms.Button();
            this.patreonBtn = new System.Windows.Forms.Button();
            this.paypalBtn = new System.Windows.Forms.Button();
            this.textboxExtraRefinerValues = new StableDiffusionGui.Controls.CustomTextbox();
            this.sliderRefinerStart = new StableDiffusionGui.Controls.CustomSlider();
            this.textboxSliderRefineStart = new System.Windows.Forms.TextBox();
            this.updownUpscaleFactor = new StableDiffusionGui.Controls.CustomUpDown();
            this.comboxControlnet = new System.Windows.Forms.ComboBox();
            this.comboxPreprocessor = new System.Windows.Forms.ComboBox();
            this.comboxClipSkip = new System.Windows.Forms.ComboBox();
            this.sliderGuidance = new StableDiffusionGui.Controls.CustomSlider();
            this.textboxSliderGuidance = new System.Windows.Forms.TextBox();
            this.textboxExtraGuidances = new StableDiffusionGui.Controls.CustomTextbox();
            this.updownUpscaleResultW = new StableDiffusionGui.Controls.CustomUpDown();
            this.updownUpscaleResultH = new StableDiffusionGui.Controls.CustomUpDown();
            this.btnCollapseDebug = new HTAlt.WinForms.HTButton();
            this.btnCollapseSymmetry = new HTAlt.WinForms.HTButton();
            this.btnCollapseRendering = new HTAlt.WinForms.HTButton();
            this.btnCollapseGeneration = new HTAlt.WinForms.HTButton();
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
            this.downloadHuggingfaceModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadStableDiffusionModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sD15ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sD15ONNXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sDXL10ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sDXL10RefinerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripPostProcess = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.upscaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.faceRestorationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applyAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelSettings = new StableDiffusionGui.Controls.CustomPanel();
            this.panelDebugLoopback = new System.Windows.Forms.Panel();
            this.checkboxLoopback = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.panelCollapseDebug = new System.Windows.Forms.Panel();
            this.panelSeamless = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.panelCollapseSymmetry = new System.Windows.Forms.Panel();
            this.panelSampler = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.panelUpscaling = new System.Windows.Forms.TableLayoutPanel();
            this.label27 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.comboxUpscaleMode = new System.Windows.Forms.ComboBox();
            this.labelUpscale = new System.Windows.Forms.Label();
            this.labelUpscaleEquals = new System.Windows.Forms.Label();
            this.labelUpscaleX = new System.Windows.Forms.Label();
            this.panelRes = new System.Windows.Forms.Panel();
            this.labelResChange = new System.Windows.Forms.Label();
            this.labelAspectRatio = new System.Windows.Forms.Label();
            this.comboxResH = new System.Windows.Forms.ComboBox();
            this.comboxResW = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panelCollapseRendering = new System.Windows.Forms.Panel();
            this.panelSeed = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.panelGuidance = new System.Windows.Forms.Panel();
            this.label19 = new System.Windows.Forms.Label();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.panelScaleImg = new System.Windows.Forms.Panel();
            this.label17 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panelScale = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelRefineStart = new System.Windows.Forms.Panel();
            this.label25 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panelSteps = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.textboxSliderSteps = new System.Windows.Forms.TextBox();
            this.sliderSteps = new StableDiffusionGui.Controls.CustomSlider();
            this.panelIterations = new System.Windows.Forms.Panel();
            this.checkboxPreview = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelControlnet = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.updownControlnetStrength = new StableDiffusionGui.Controls.CustomUpDown();
            this.label28 = new System.Windows.Forms.Label();
            this.panelInitImgStrength = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.panelInpainting = new System.Windows.Forms.Panel();
            this.comboxControlnetSlot = new System.Windows.Forms.ComboBox();
            this.panelResizeGravity = new System.Windows.Forms.Panel();
            this.label21 = new System.Windows.Forms.Label();
            this.comboxInpaintMode = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.panelCollapseGeneration = new System.Windows.Forms.Panel();
            this.panelBaseImg = new System.Windows.Forms.Panel();
            this.checkboxShowInitImg = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panelLoras = new System.Windows.Forms.Panel();
            this.tbLoraFilter = new System.Windows.Forms.TextBox();
            this.gridLoras = new System.Windows.Forms.DataGridView();
            this.ColEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColWeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label24 = new System.Windows.Forms.Label();
            this.panelEmbeddings = new System.Windows.Forms.Panel();
            this.btnEmbeddingCopy = new HTAlt.WinForms.HTButton();
            this.comboxEmbeddingList = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.panelPromptNeg = new System.Windows.Forms.Panel();
            this.panelPrompt = new System.Windows.Forms.Panel();
            this.panelCollapsePrompt = new System.Windows.Forms.Panel();
            this.btnCollapsePrompt = new HTAlt.WinForms.HTButton();
            this.panelModel2 = new System.Windows.Forms.Panel();
            this.comboxModel2 = new System.Windows.Forms.ComboBox();
            this.label26 = new System.Windows.Forms.Label();
            this.panelModelSettings = new System.Windows.Forms.Panel();
            this.label29 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.panelModel = new System.Windows.Forms.Panel();
            this.comboxModel = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.comboxVae = new System.Windows.Forms.ComboBox();
            this.panelBackend = new System.Windows.Forms.Panel();
            this.comboxBackend = new System.Windows.Forms.ComboBox();
            this.label22 = new System.Windows.Forms.Label();
            this.panelCollapseImplementation = new System.Windows.Forms.Panel();
            this.btnCollapseImplementation = new HTAlt.WinForms.HTButton();
            this.tableLayoutPanelImgViewers = new System.Windows.Forms.TableLayoutPanel();
            this.panelImgViewerParent = new System.Windows.Forms.Panel();
            this.pictBoxPreview = new System.Windows.Forms.PictureBox();
            this.pictBoxImgViewer = new System.Windows.Forms.PictureBox();
            this.pictBoxInitImg = new System.Windows.Forms.PictureBox();
            this.menuStripInstall = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.manageInstallationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.installUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripOpenFolder = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openOutputFolderToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openFavoritesFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flowPanelImgButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSaveMode = new System.Windows.Forms.Button();
            this.separator = new System.Windows.Forms.Button();
            this.menuStripOutputImg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upDownSeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDownIterations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updownUpscaleFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updownUpscaleResultW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updownUpscaleResultH)).BeginInit();
            this.menuStripLogs.SuspendLayout();
            this.menuStripRunQueue.SuspendLayout();
            this.menuStripAddToQueue.SuspendLayout();
            this.menuStripDeleteImages.SuspendLayout();
            this.menuStripDevTools.SuspendLayout();
            this.menuStripPostProcess.SuspendLayout();
            this.panelSettings.SuspendLayout();
            this.panelDebugLoopback.SuspendLayout();
            this.panelCollapseDebug.SuspendLayout();
            this.panelSeamless.SuspendLayout();
            this.panelCollapseSymmetry.SuspendLayout();
            this.panelSampler.SuspendLayout();
            this.panelUpscaling.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panelRes.SuspendLayout();
            this.panelCollapseRendering.SuspendLayout();
            this.panelSeed.SuspendLayout();
            this.panelGuidance.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.panelScaleImg.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panelScale.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelRefineStart.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panelSteps.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.panelIterations.SuspendLayout();
            this.panelControlnet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updownControlnetStrength)).BeginInit();
            this.panelInitImgStrength.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.panelInpainting.SuspendLayout();
            this.panelResizeGravity.SuspendLayout();
            this.panelCollapseGeneration.SuspendLayout();
            this.panelBaseImg.SuspendLayout();
            this.panelLoras.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridLoras)).BeginInit();
            this.panelEmbeddings.SuspendLayout();
            this.panelPromptNeg.SuspendLayout();
            this.panelPrompt.SuspendLayout();
            this.panelCollapsePrompt.SuspendLayout();
            this.panelModel2.SuspendLayout();
            this.panelModelSettings.SuspendLayout();
            this.panelModel.SuspendLayout();
            this.panelBackend.SuspendLayout();
            this.panelCollapseImplementation.SuspendLayout();
            this.tableLayoutPanelImgViewers.SuspendLayout();
            this.panelImgViewerParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxImgViewer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxInitImg)).BeginInit();
            this.menuStripInstall.SuspendLayout();
            this.menuStripOpenFolder.SuspendLayout();
            this.flowPanelImgButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // runBtn
            // 
            this.runBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.runBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.runBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.runBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.runBtn.ForeColor = System.Drawing.Color.White;
            this.runBtn.Location = new System.Drawing.Point(680, 629);
            this.runBtn.Name = "runBtn";
            this.runBtn.Size = new System.Drawing.Size(120, 40);
            this.runBtn.TabIndex = 100;
            this.runBtn.Text = "Generate!";
            this.toolTip.SetToolTip(this.runBtn, "Generate Images");
            this.runBtn.UseVisualStyleBackColor = false;
            this.runBtn.Click += new System.EventHandler(this.runBtn_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Yu Gothic UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.Location = new System.Drawing.Point(3, 6);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(366, 40);
            this.titleLabel.TabIndex = 11;
            this.titleLabel.Text = "NMKD Stable Diffusion GUI";
            // 
            // logBox
            // 
            this.logBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.logBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.logBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logBox.DisableUnfocusedInput = false;
            this.logBox.ForeColor = System.Drawing.Color.Silver;
            this.logBox.Location = new System.Drawing.Point(6, 611);
            this.logBox.MaxTextZoomFactor = 1.5F;
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.Placeholder = "";
            this.logBox.PlaceholderTextColor = System.Drawing.Color.Silver;
            this.logBox.ReadOnly = true;
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logBox.Size = new System.Drawing.Size(668, 82);
            this.logBox.TabIndex = 78;
            this.logBox.TabStop = false;
            // 
            // labelImgInfo
            // 
            this.labelImgInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelImgInfo.AutoSize = true;
            this.labelImgInfo.ForeColor = System.Drawing.Color.Silver;
            this.labelImgInfo.Location = new System.Drawing.Point(679, 610);
            this.labelImgInfo.Name = "labelImgInfo";
            this.labelImgInfo.Size = new System.Drawing.Size(100, 13);
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
            this.progressCircle.Location = new System.Drawing.Point(376, 9);
            this.progressCircle.MarqueeAnimationSpeed = 2000;
            this.progressCircle.Name = "progressCircle";
            this.progressCircle.OuterColor = System.Drawing.Color.Gray;
            this.progressCircle.OuterMargin = -21;
            this.progressCircle.OuterWidth = 21;
            this.progressCircle.ProgressColor = System.Drawing.Color.LimeGreen;
            this.progressCircle.ProgressWidth = 8;
            this.progressCircle.SecondaryFont = new System.Drawing.Font("Microsoft Sans Serif", 36F);
            this.progressCircle.Size = new System.Drawing.Size(40, 40);
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
            this.progressBar.Location = new System.Drawing.Point(680, 678);
            this.progressBar.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(512, 15);
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
            this.menuStripOutputImg.Size = new System.Drawing.Size(285, 224);
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(284, 22);
            this.openFileToolStripMenuItem.Text = "Open File";
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // openOutputFolderToolStripMenuItem
            // 
            this.openOutputFolderToolStripMenuItem.Name = "openOutputFolderToolStripMenuItem";
            this.openOutputFolderToolStripMenuItem.Size = new System.Drawing.Size(284, 22);
            this.openOutputFolderToolStripMenuItem.Text = "Open Output Folder";
            this.openOutputFolderToolStripMenuItem.Click += new System.EventHandler(this.openOutputFolderToolStripMenuItem_Click);
            // 
            // copyToFavoritesToolStripMenuItem
            // 
            this.copyToFavoritesToolStripMenuItem.Name = "copyToFavoritesToolStripMenuItem";
            this.copyToFavoritesToolStripMenuItem.Size = new System.Drawing.Size(284, 22);
            this.copyToFavoritesToolStripMenuItem.Text = "Copy To Favorites";
            this.copyToFavoritesToolStripMenuItem.Click += new System.EventHandler(this.copyToFavoritesToolStripMenuItem_Click);
            // 
            // copyImageToClipboardToolStripMenuItem
            // 
            this.copyImageToClipboardToolStripMenuItem.Name = "copyImageToClipboardToolStripMenuItem";
            this.copyImageToClipboardToolStripMenuItem.Size = new System.Drawing.Size(284, 22);
            this.copyImageToClipboardToolStripMenuItem.Text = "Copy Image";
            this.copyImageToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyImageToClipboardToolStripMenuItem_Click);
            // 
            // copySidebySideComparisonImageToolStripMenuItem
            // 
            this.copySidebySideComparisonImageToolStripMenuItem.Name = "copySidebySideComparisonImageToolStripMenuItem";
            this.copySidebySideComparisonImageToolStripMenuItem.Size = new System.Drawing.Size(284, 22);
            this.copySidebySideComparisonImageToolStripMenuItem.Text = "Copy Side-by-Side Comparison Image";
            this.copySidebySideComparisonImageToolStripMenuItem.Click += new System.EventHandler(this.copySidebySideComparisonImageToolStripMenuItem_Click);
            // 
            // copySeedToClipboardToolStripMenuItem
            // 
            this.copySeedToClipboardToolStripMenuItem.Name = "copySeedToClipboardToolStripMenuItem";
            this.copySeedToClipboardToolStripMenuItem.Size = new System.Drawing.Size(284, 22);
            this.copySeedToClipboardToolStripMenuItem.Text = "Copy Seed";
            this.copySeedToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copySeedToClipboardToolStripMenuItem_Click);
            // 
            // reGenerateImageWithCurrentSettingsToolStripMenuItem
            // 
            this.reGenerateImageWithCurrentSettingsToolStripMenuItem.Name = "reGenerateImageWithCurrentSettingsToolStripMenuItem";
            this.reGenerateImageWithCurrentSettingsToolStripMenuItem.Size = new System.Drawing.Size(284, 22);
            this.reGenerateImageWithCurrentSettingsToolStripMenuItem.Text = "Re-Generate Image With Current Settings";
            this.reGenerateImageWithCurrentSettingsToolStripMenuItem.Click += new System.EventHandler(this.reGenerateImageWithCurrentSettingsToolStripMenuItem_Click);
            // 
            // useAsInitImageToolStripMenuItem
            // 
            this.useAsInitImageToolStripMenuItem.Name = "useAsInitImageToolStripMenuItem";
            this.useAsInitImageToolStripMenuItem.Size = new System.Drawing.Size(284, 22);
            this.useAsInitImageToolStripMenuItem.Text = "Use as Base Image";
            this.useAsInitImageToolStripMenuItem.Click += new System.EventHandler(this.useAsInitImageToolStripMenuItem_Click);
            // 
            // postProcessImageToolStripMenuItem
            // 
            this.postProcessImageToolStripMenuItem.Name = "postProcessImageToolStripMenuItem";
            this.postProcessImageToolStripMenuItem.Size = new System.Drawing.Size(284, 22);
            this.postProcessImageToolStripMenuItem.Text = "Post-Process Image...";
            this.postProcessImageToolStripMenuItem.Click += new System.EventHandler(this.postProcessImageToolStripMenuItem_Click);
            // 
            // fitWindowSizeToImageSizeToolStripMenuItem
            // 
            this.fitWindowSizeToImageSizeToolStripMenuItem.Name = "fitWindowSizeToImageSizeToolStripMenuItem";
            this.fitWindowSizeToImageSizeToolStripMenuItem.Size = new System.Drawing.Size(284, 22);
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
            this.labelImgPrompt.ForeColor = System.Drawing.Color.Silver;
            this.labelImgPrompt.Location = new System.Drawing.Point(679, 58);
            this.labelImgPrompt.Name = "labelImgPrompt";
            this.labelImgPrompt.Size = new System.Drawing.Size(509, 16);
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
            this.labelImgPromptNeg.ForeColor = System.Drawing.Color.Silver;
            this.labelImgPromptNeg.Location = new System.Drawing.Point(679, 75);
            this.labelImgPromptNeg.Name = "labelImgPromptNeg";
            this.labelImgPromptNeg.Size = new System.Drawing.Size(509, 16);
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
            this.comboxSampler.ForeColor = System.Drawing.Color.White;
            this.comboxSampler.FormattingEnabled = true;
            this.comboxSampler.Location = new System.Drawing.Point(233, 0);
            this.comboxSampler.Name = "comboxSampler";
            this.comboxSampler.Size = new System.Drawing.Size(200, 21);
            this.comboxSampler.TabIndex = 105;
            this.toolTip.SetToolTip(this.comboxSampler, "Changes how the image is sampled.\r\nEuler Ancestral works very well at low step co" +
        "unts.");
            // 
            // checkboxHiresFix
            // 
            this.checkboxHiresFix.ForeColor = System.Drawing.Color.White;
            this.checkboxHiresFix.Location = new System.Drawing.Point(439, -1);
            this.checkboxHiresFix.Name = "checkboxHiresFix";
            this.checkboxHiresFix.Padding = new System.Windows.Forms.Padding(3);
            this.checkboxHiresFix.Size = new System.Drawing.Size(123, 23);
            this.checkboxHiresFix.TabIndex = 108;
            this.checkboxHiresFix.Text = "High-Resolution Fix";
            this.toolTip.SetToolTip(this.checkboxHiresFix, "Avoid duplications in high-resolution images, at the cost of generation speed.");
            this.checkboxHiresFix.UseVisualStyleBackColor = true;
            // 
            // checkboxLockSeed
            // 
            this.checkboxLockSeed.ForeColor = System.Drawing.Color.White;
            this.checkboxLockSeed.Location = new System.Drawing.Point(511, -1);
            this.checkboxLockSeed.Name = "checkboxLockSeed";
            this.checkboxLockSeed.Padding = new System.Windows.Forms.Padding(3);
            this.checkboxLockSeed.Size = new System.Drawing.Size(84, 23);
            this.checkboxLockSeed.TabIndex = 109;
            this.checkboxLockSeed.Text = "Lock Seed";
            this.toolTip.SetToolTip(this.checkboxLockSeed, resources.GetString("checkboxLockSeed.ToolTip"));
            this.checkboxLockSeed.UseVisualStyleBackColor = true;
            // 
            // btnSeedResetToRandom
            // 
            this.btnSeedResetToRandom.AutoColor = true;
            this.btnSeedResetToRandom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnSeedResetToRandom.ButtonImage = null;
            this.btnSeedResetToRandom.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnSeedResetToRandom.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnSeedResetToRandom.DrawImage = false;
            this.btnSeedResetToRandom.ForeColor = System.Drawing.Color.White;
            this.btnSeedResetToRandom.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.btnSeedResetToRandom.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnSeedResetToRandom.Location = new System.Drawing.Point(425, -1);
            this.btnSeedResetToRandom.Name = "btnSeedResetToRandom";
            this.btnSeedResetToRandom.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnSeedResetToRandom.Size = new System.Drawing.Size(80, 23);
            this.btnSeedResetToRandom.TabIndex = 107;
            this.btnSeedResetToRandom.TabStop = false;
            this.btnSeedResetToRandom.Text = "Reset";
            this.toolTip.SetToolTip(this.btnSeedResetToRandom, "Reset to Random Seed");
            this.btnSeedResetToRandom.Click += new System.EventHandler(this.btnSeedResetToRandom_Click);
            // 
            // btnSeedUsePrevious
            // 
            this.btnSeedUsePrevious.AutoColor = true;
            this.btnSeedUsePrevious.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnSeedUsePrevious.ButtonImage = null;
            this.btnSeedUsePrevious.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnSeedUsePrevious.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnSeedUsePrevious.DrawImage = false;
            this.btnSeedUsePrevious.ForeColor = System.Drawing.Color.White;
            this.btnSeedUsePrevious.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.btnSeedUsePrevious.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnSeedUsePrevious.Location = new System.Drawing.Point(339, -1);
            this.btnSeedUsePrevious.Name = "btnSeedUsePrevious";
            this.btnSeedUsePrevious.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnSeedUsePrevious.Size = new System.Drawing.Size(80, 23);
            this.btnSeedUsePrevious.TabIndex = 106;
            this.btnSeedUsePrevious.TabStop = false;
            this.btnSeedUsePrevious.Text = "Use Previous";
            this.toolTip.SetToolTip(this.btnSeedUsePrevious, "Use Same Seed as Previous Run");
            this.btnSeedUsePrevious.Click += new System.EventHandler(this.btnSeedUsePrevious_Click);
            // 
            // upDownSeed
            // 
            this.upDownSeed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.upDownSeed.ForeColor = System.Drawing.Color.White;
            this.upDownSeed.Location = new System.Drawing.Point(233, 1);
            this.upDownSeed.Maximum = new decimal(new int[] {
            -1,
            2147483647,
            0,
            0});
            this.upDownSeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.upDownSeed.Name = "upDownSeed";
            this.upDownSeed.Size = new System.Drawing.Size(100, 20);
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
            this.sliderScale.Size = new System.Drawing.Size(308, 25);
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
            this.textboxSliderScale.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.textboxSliderScale.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textboxSliderScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textboxSliderScale.ForeColor = System.Drawing.Color.Silver;
            this.textboxSliderScale.Location = new System.Drawing.Point(308, 2);
            this.textboxSliderScale.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.textboxSliderScale.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxSliderScale.Name = "textboxSliderScale";
            this.textboxSliderScale.Size = new System.Drawing.Size(35, 21);
            this.textboxSliderScale.TabIndex = 93;
            this.textboxSliderScale.Text = "0";
            // 
            // textboxExtraScales
            // 
            this.textboxExtraScales.AllowDrop = true;
            this.textboxExtraScales.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxExtraScales.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textboxExtraScales.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textboxExtraScales.DisableUnfocusedInput = true;
            this.textboxExtraScales.ForeColor = System.Drawing.Color.White;
            this.textboxExtraScales.Location = new System.Drawing.Point(346, 3);
            this.textboxExtraScales.MaxTextZoomFactor = 1F;
            this.textboxExtraScales.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxExtraScales.Name = "textboxExtraScales";
            this.textboxExtraScales.Placeholder = "";
            this.textboxExtraScales.PlaceholderTextColor = System.Drawing.Color.Silver;
            this.textboxExtraScales.Size = new System.Drawing.Size(69, 21);
            this.textboxExtraScales.TabIndex = 3;
            this.toolTip.SetToolTip(this.textboxExtraScales, resources.GetString("textboxExtraScales.ToolTip"));
            // 
            // upDownIterations
            // 
            this.upDownIterations.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.upDownIterations.ForeColor = System.Drawing.Color.White;
            this.upDownIterations.Location = new System.Drawing.Point(233, 1);
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
            this.upDownIterations.Size = new System.Drawing.Size(100, 20);
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
            this.btnResetMask.Location = new System.Drawing.Point(439, -1);
            this.btnResetMask.Name = "btnResetMask";
            this.btnResetMask.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnResetMask.Size = new System.Drawing.Size(79, 23);
            this.btnResetMask.TabIndex = 108;
            this.btnResetMask.TabStop = false;
            this.btnResetMask.Text = "Clear Mask";
            this.toolTip.SetToolTip(this.btnResetMask, "Reset Inpainting Mask");
            this.btnResetMask.Visible = false;
            this.btnResetMask.Click += new System.EventHandler(this.btnResetMask_Click);
            // 
            // textboxExtraInitStrengths
            // 
            this.textboxExtraInitStrengths.AllowDrop = true;
            this.textboxExtraInitStrengths.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxExtraInitStrengths.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textboxExtraInitStrengths.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textboxExtraInitStrengths.DisableUnfocusedInput = true;
            this.textboxExtraInitStrengths.ForeColor = System.Drawing.Color.White;
            this.textboxExtraInitStrengths.Location = new System.Drawing.Point(346, 3);
            this.textboxExtraInitStrengths.MaxTextZoomFactor = 1F;
            this.textboxExtraInitStrengths.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxExtraInitStrengths.Name = "textboxExtraInitStrengths";
            this.textboxExtraInitStrengths.Placeholder = "";
            this.textboxExtraInitStrengths.PlaceholderTextColor = System.Drawing.Color.Silver;
            this.textboxExtraInitStrengths.Size = new System.Drawing.Size(69, 21);
            this.textboxExtraInitStrengths.TabIndex = 91;
            this.toolTip.SetToolTip(this.textboxExtraInitStrengths, resources.GetString("textboxExtraInitStrengths.ToolTip"));
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
            this.sliderInitStrength.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sliderInitStrength.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.sliderInitStrength.ForeColor = System.Drawing.Color.Black;
            this.sliderInitStrength.InitValue = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.sliderInitStrength.LargeChange = ((uint)(2u));
            this.sliderInitStrength.Location = new System.Drawing.Point(0, 0);
            this.sliderInitStrength.Margin = new System.Windows.Forms.Padding(0);
            this.sliderInitStrength.Maximum = 18;
            this.sliderInitStrength.Minimum = 2;
            this.sliderInitStrength.Name = "sliderInitStrength";
            this.sliderInitStrength.OverlayColor = System.Drawing.Color.White;
            this.sliderInitStrength.Size = new System.Drawing.Size(308, 25);
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
            this.textboxSliderInitStrength.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.textboxSliderInitStrength.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textboxSliderInitStrength.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textboxSliderInitStrength.ForeColor = System.Drawing.Color.Silver;
            this.textboxSliderInitStrength.Location = new System.Drawing.Point(308, 2);
            this.textboxSliderInitStrength.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.textboxSliderInitStrength.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxSliderInitStrength.Name = "textboxSliderInitStrength";
            this.textboxSliderInitStrength.Size = new System.Drawing.Size(35, 21);
            this.textboxSliderInitStrength.TabIndex = 94;
            this.textboxSliderInitStrength.Text = "0.1";
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
            this.btnInitImgBrowse.Location = new System.Drawing.Point(233, -1);
            this.btnInitImgBrowse.Name = "btnInitImgBrowse";
            this.btnInitImgBrowse.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnInitImgBrowse.Size = new System.Drawing.Size(100, 23);
            this.btnInitImgBrowse.TabIndex = 1;
            this.btnInitImgBrowse.TabStop = false;
            this.btnInitImgBrowse.Text = "Load Image";
            this.toolTip.SetToolTip(this.btnInitImgBrowse, "Load base image");
            this.btnInitImgBrowse.Click += new System.EventHandler(this.btnInitImgBrowse_Click);
            // 
            // textboxPromptNeg
            // 
            this.textboxPromptNeg.AllowDrop = true;
            this.textboxPromptNeg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxPromptNeg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textboxPromptNeg.DisableUnfocusedInput = true;
            this.textboxPromptNeg.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.textboxPromptNeg.Location = new System.Drawing.Point(0, -1);
            this.textboxPromptNeg.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.textboxPromptNeg.MaxTextZoomFactor = 2F;
            this.textboxPromptNeg.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxPromptNeg.Multiline = true;
            this.textboxPromptNeg.Name = "textboxPromptNeg";
            this.textboxPromptNeg.Placeholder = "Enter your negative prompt here...";
            this.textboxPromptNeg.PlaceholderTextColor = System.Drawing.Color.Silver;
            this.textboxPromptNeg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textboxPromptNeg.Size = new System.Drawing.Size(628, 33);
            this.textboxPromptNeg.TabIndex = 1;
            this.toolTip.SetToolTip(this.textboxPromptNeg, "Negative text prompt. The AI will try to avoid generating things you describe her" +
        "e.");
            // 
            // textboxPrompt
            // 
            this.textboxPrompt.AllowDrop = true;
            this.textboxPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxPrompt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textboxPrompt.DisableUnfocusedInput = true;
            this.textboxPrompt.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.textboxPrompt.Location = new System.Drawing.Point(0, -1);
            this.textboxPrompt.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.textboxPrompt.MaxTextZoomFactor = 2F;
            this.textboxPrompt.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxPrompt.Multiline = true;
            this.textboxPrompt.Name = "textboxPrompt";
            this.textboxPrompt.Placeholder = "Enter your prompt here...";
            this.textboxPrompt.PlaceholderTextColor = System.Drawing.Color.Silver;
            this.textboxPrompt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textboxPrompt.Size = new System.Drawing.Size(628, 58);
            this.textboxPrompt.TabIndex = 0;
            this.toolTip.SetToolTip(this.textboxPrompt, "Text prompt. The AI will try to generate an image matching this description.");
            // 
            // comboxSeamless
            // 
            this.comboxSeamless.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxSeamless.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxSeamless.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxSeamless.ForeColor = System.Drawing.Color.White;
            this.comboxSeamless.FormattingEnabled = true;
            this.comboxSeamless.Location = new System.Drawing.Point(233, 0);
            this.comboxSeamless.Name = "comboxSeamless";
            this.comboxSeamless.Size = new System.Drawing.Size(200, 21);
            this.comboxSeamless.TabIndex = 107;
            this.toolTip.SetToolTip(this.comboxSeamless, "Changes how the image is sampled.\r\nEuler Ancestral works very well at low step co" +
        "unts.");
            // 
            // textboxClipsegMask
            // 
            this.textboxClipsegMask.AllowDrop = true;
            this.textboxClipsegMask.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxClipsegMask.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textboxClipsegMask.ForeColor = System.Drawing.Color.White;
            this.textboxClipsegMask.Location = new System.Drawing.Point(439, 0);
            this.textboxClipsegMask.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxClipsegMask.Name = "textboxClipsegMask";
            this.textboxClipsegMask.Size = new System.Drawing.Size(209, 21);
            this.textboxClipsegMask.TabIndex = 110;
            this.textboxClipsegMask.Text = "0,1";
            this.toolTip.SetToolTip(this.textboxClipsegMask, "Describe what objects you want to replace");
            // 
            // textboxExtraSteps
            // 
            this.textboxExtraSteps.AllowDrop = true;
            this.textboxExtraSteps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxExtraSteps.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textboxExtraSteps.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textboxExtraSteps.DisableUnfocusedInput = true;
            this.textboxExtraSteps.ForeColor = System.Drawing.Color.White;
            this.textboxExtraSteps.Location = new System.Drawing.Point(346, 3);
            this.textboxExtraSteps.MaxTextZoomFactor = 1F;
            this.textboxExtraSteps.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxExtraSteps.Name = "textboxExtraSteps";
            this.textboxExtraSteps.Placeholder = "";
            this.textboxExtraSteps.PlaceholderTextColor = System.Drawing.Color.Silver;
            this.textboxExtraSteps.Size = new System.Drawing.Size(69, 21);
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
            this.btnEditMask.Location = new System.Drawing.Point(524, -1);
            this.btnEditMask.Name = "btnEditMask";
            this.btnEditMask.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnEditMask.Size = new System.Drawing.Size(79, 23);
            this.btnEditMask.TabIndex = 111;
            this.btnEditMask.TabStop = false;
            this.btnEditMask.Text = "Edit Mask";
            this.toolTip.SetToolTip(this.btnEditMask, "Edit Inpainting Mask");
            this.btnEditMask.Visible = false;
            this.btnEditMask.Click += new System.EventHandler(this.btnEditMask_Click);
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
            15,
            0,
            0,
            65536});
            this.sliderScaleImg.LargeChange = ((uint)(5u));
            this.sliderScaleImg.Location = new System.Drawing.Point(0, 0);
            this.sliderScaleImg.Margin = new System.Windows.Forms.Padding(0);
            this.sliderScaleImg.Maximum = 25;
            this.sliderScaleImg.Minimum = 5;
            this.sliderScaleImg.Name = "sliderScaleImg";
            this.sliderScaleImg.OverlayColor = System.Drawing.Color.White;
            this.sliderScaleImg.Size = new System.Drawing.Size(308, 25);
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
            this.textboxSliderScaleImg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.textboxSliderScaleImg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textboxSliderScaleImg.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textboxSliderScaleImg.ForeColor = System.Drawing.Color.Silver;
            this.textboxSliderScaleImg.Location = new System.Drawing.Point(308, 2);
            this.textboxSliderScaleImg.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.textboxSliderScaleImg.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxSliderScaleImg.Name = "textboxSliderScaleImg";
            this.textboxSliderScaleImg.Size = new System.Drawing.Size(35, 21);
            this.textboxSliderScaleImg.TabIndex = 93;
            this.textboxSliderScaleImg.Text = "0.75";
            // 
            // textboxExtraScalesImg
            // 
            this.textboxExtraScalesImg.AllowDrop = true;
            this.textboxExtraScalesImg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxExtraScalesImg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textboxExtraScalesImg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textboxExtraScalesImg.DisableUnfocusedInput = true;
            this.textboxExtraScalesImg.ForeColor = System.Drawing.Color.White;
            this.textboxExtraScalesImg.Location = new System.Drawing.Point(346, 3);
            this.textboxExtraScalesImg.MaxTextZoomFactor = 1F;
            this.textboxExtraScalesImg.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxExtraScalesImg.Name = "textboxExtraScalesImg";
            this.textboxExtraScalesImg.Placeholder = "";
            this.textboxExtraScalesImg.PlaceholderTextColor = System.Drawing.Color.Silver;
            this.textboxExtraScalesImg.Size = new System.Drawing.Size(69, 21);
            this.textboxExtraScalesImg.TabIndex = 3;
            this.toolTip.SetToolTip(this.textboxExtraScalesImg, resources.GetString("textboxExtraScalesImg.ToolTip"));
            // 
            // comboxResizeGravity
            // 
            this.comboxResizeGravity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboxResizeGravity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxResizeGravity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxResizeGravity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxResizeGravity.ForeColor = System.Drawing.Color.White;
            this.comboxResizeGravity.FormattingEnabled = true;
            this.comboxResizeGravity.Location = new System.Drawing.Point(110, 0);
            this.comboxResizeGravity.Margin = new System.Windows.Forms.Padding(0);
            this.comboxResizeGravity.Name = "comboxResizeGravity";
            this.comboxResizeGravity.Size = new System.Drawing.Size(99, 21);
            this.comboxResizeGravity.TabIndex = 106;
            this.toolTip.SetToolTip(this.comboxResizeGravity, "Change from which point the image will be expanded.\r\n\r\nThis does not apply if you" +
        " don\'t adjust the resolution because your input image already has transparent re" +
        "gions.");
            // 
            // comboxSymmetry
            // 
            this.comboxSymmetry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxSymmetry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxSymmetry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxSymmetry.ForeColor = System.Drawing.Color.White;
            this.comboxSymmetry.FormattingEnabled = true;
            this.comboxSymmetry.Location = new System.Drawing.Point(439, 0);
            this.comboxSymmetry.Name = "comboxSymmetry";
            this.comboxSymmetry.Size = new System.Drawing.Size(200, 21);
            this.comboxSymmetry.TabIndex = 107;
            this.toolTip.SetToolTip(this.comboxSymmetry, "Changes how the image is sampled.\r\nEuler Ancestral works very well at low step co" +
        "unts.");
            // 
            // btnEmbeddingAppend
            // 
            this.btnEmbeddingAppend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEmbeddingAppend.AutoColor = true;
            this.btnEmbeddingAppend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnEmbeddingAppend.ButtonImage = null;
            this.btnEmbeddingAppend.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnEmbeddingAppend.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnEmbeddingAppend.DrawImage = false;
            this.btnEmbeddingAppend.ForeColor = System.Drawing.Color.White;
            this.btnEmbeddingAppend.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.btnEmbeddingAppend.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnEmbeddingAppend.Location = new System.Drawing.Point(525, -1);
            this.btnEmbeddingAppend.Name = "btnEmbeddingAppend";
            this.btnEmbeddingAppend.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnEmbeddingAppend.Size = new System.Drawing.Size(123, 23);
            this.btnEmbeddingAppend.TabIndex = 112;
            this.btnEmbeddingAppend.TabStop = false;
            this.btnEmbeddingAppend.Text = "Append To Prompt";
            this.toolTip.SetToolTip(this.btnEmbeddingAppend, "Shift+Click to Append to Negative Prompt Instead");
            this.btnEmbeddingAppend.Visible = false;
            this.btnEmbeddingAppend.Click += new System.EventHandler(this.btnEmbeddingAppend_Click);
            // 
            // btnResetRes
            // 
            this.btnResetRes.AutoColor = true;
            this.btnResetRes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnResetRes.ButtonImage = null;
            this.btnResetRes.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnResetRes.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnResetRes.DrawImage = false;
            this.btnResetRes.ForeColor = System.Drawing.Color.White;
            this.btnResetRes.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.btnResetRes.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnResetRes.Location = new System.Drawing.Point(439, 0);
            this.btnResetRes.Name = "btnResetRes";
            this.btnResetRes.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnResetRes.Size = new System.Drawing.Size(50, 23);
            this.btnResetRes.TabIndex = 114;
            this.btnResetRes.TabStop = false;
            this.btnResetRes.Text = "Reset";
            this.toolTip.SetToolTip(this.btnResetRes, "Reset to input image\'s resolution");
            this.btnResetRes.Visible = false;
            this.btnResetRes.Click += new System.EventHandler(this.btnResetRes_Click);
            // 
            // comboxModelArch
            // 
            this.comboxModelArch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxModelArch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxModelArch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxModelArch.ForeColor = System.Drawing.Color.White;
            this.comboxModelArch.Location = new System.Drawing.Point(270, 0);
            this.comboxModelArch.Name = "comboxModelArch";
            this.comboxModelArch.Size = new System.Drawing.Size(225, 21);
            this.comboxModelArch.TabIndex = 111;
            this.toolTip.SetToolTip(this.comboxModelArch, "Select how the model should be loaded. This is not necessary for Diffusers models" +
        ".");
            // 
            // btnNextImg
            // 
            this.btnNextImg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNextImg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnNextImg.BackgroundImage = global::StableDiffusionGui.Properties.Resources.forwardArrowIcon;
            this.btnNextImg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnNextImg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextImg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNextImg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnNextImg.Location = new System.Drawing.Point(251, 0);
            this.btnNextImg.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.btnNextImg.Name = "btnNextImg";
            this.btnNextImg.Size = new System.Drawing.Size(40, 40);
            this.btnNextImg.TabIndex = 80;
            this.btnNextImg.TabStop = false;
            this.toolTip.SetToolTip(this.btnNextImg, "Next Image");
            this.btnNextImg.UseVisualStyleBackColor = false;
            this.btnNextImg.Click += new System.EventHandler(this.btnNextImg_Click);
            // 
            // btnPrevImg
            // 
            this.btnPrevImg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrevImg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnPrevImg.BackgroundImage = global::StableDiffusionGui.Properties.Resources.backArrowIcon;
            this.btnPrevImg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPrevImg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevImg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrevImg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnPrevImg.Location = new System.Drawing.Point(205, 0);
            this.btnPrevImg.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.btnPrevImg.Name = "btnPrevImg";
            this.btnPrevImg.Size = new System.Drawing.Size(40, 40);
            this.btnPrevImg.TabIndex = 82;
            this.btnPrevImg.TabStop = false;
            this.toolTip.SetToolTip(this.btnPrevImg, "Previous Image");
            this.btnPrevImg.UseVisualStyleBackColor = false;
            this.btnPrevImg.Click += new System.EventHandler(this.btnPrevImg_Click);
            // 
            // btnDeleteBatch
            // 
            this.btnDeleteBatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteBatch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnDeleteBatch.BackgroundImage = global::StableDiffusionGui.Properties.Resources.deleteBtn;
            this.btnDeleteBatch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDeleteBatch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteBatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteBatch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnDeleteBatch.Location = new System.Drawing.Point(159, 0);
            this.btnDeleteBatch.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.btnDeleteBatch.Name = "btnDeleteBatch";
            this.btnDeleteBatch.Size = new System.Drawing.Size(40, 40);
            this.btnDeleteBatch.TabIndex = 114;
            this.btnDeleteBatch.TabStop = false;
            this.toolTip.SetToolTip(this.btnDeleteBatch, "Delete one or all images...");
            this.btnDeleteBatch.UseVisualStyleBackColor = false;
            this.btnDeleteBatch.Click += new System.EventHandler(this.btnDeleteBatch_Click);
            // 
            // btnOpenOutFolder
            // 
            this.btnOpenOutFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenOutFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnOpenOutFolder.BackgroundImage = global::StableDiffusionGui.Properties.Resources.baseline_folder_open_white_48dp;
            this.btnOpenOutFolder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnOpenOutFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenOutFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenOutFolder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnOpenOutFolder.Location = new System.Drawing.Point(113, 0);
            this.btnOpenOutFolder.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.btnOpenOutFolder.Name = "btnOpenOutFolder";
            this.btnOpenOutFolder.Size = new System.Drawing.Size(40, 40);
            this.btnOpenOutFolder.TabIndex = 94;
            this.btnOpenOutFolder.TabStop = false;
            this.toolTip.SetToolTip(this.btnOpenOutFolder, "Open Output Folder...");
            this.btnOpenOutFolder.UseVisualStyleBackColor = false;
            this.btnOpenOutFolder.Click += new System.EventHandler(this.btnOpenOutFolder_Click);
            // 
            // btnSaveToFavs
            // 
            this.btnSaveToFavs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveToFavs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnSaveToFavs.BackgroundImage = global::StableDiffusionGui.Properties.Resources.IconSave;
            this.btnSaveToFavs.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSaveToFavs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveToFavs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveToFavs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnSaveToFavs.Location = new System.Drawing.Point(67, 0);
            this.btnSaveToFavs.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.btnSaveToFavs.Name = "btnSaveToFavs";
            this.btnSaveToFavs.Size = new System.Drawing.Size(40, 40);
            this.btnSaveToFavs.TabIndex = 120;
            this.btnSaveToFavs.TabStop = false;
            this.toolTip.SetToolTip(this.btnSaveToFavs, "Save This Image to Favorites");
            this.btnSaveToFavs.UseVisualStyleBackColor = false;
            this.btnSaveToFavs.Click += new System.EventHandler(this.btnSaveToFavs_Click);
            // 
            // btnTrain
            // 
            this.btnTrain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTrain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnTrain.BackgroundImage = global::StableDiffusionGui.Properties.Resources.IconTrainModel;
            this.btnTrain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnTrain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTrain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrain.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnTrain.Location = new System.Drawing.Point(968, 9);
            this.btnTrain.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(40, 40);
            this.btnTrain.TabIndex = 117;
            this.btnTrain.TabStop = false;
            this.toolTip.SetToolTip(this.btnTrain, "Train LoRA Model");
            this.btnTrain.UseVisualStyleBackColor = false;
            this.btnTrain.Click += new System.EventHandler(this.btnDreambooth_Click);
            // 
            // btnPromptHistory
            // 
            this.btnPromptHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPromptHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnPromptHistory.BackgroundImage = global::StableDiffusionGui.Properties.Resources.historyIcon;
            this.btnPromptHistory.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPromptHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPromptHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPromptHistory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnPromptHistory.Location = new System.Drawing.Point(852, 629);
            this.btnPromptHistory.Name = "btnPromptHistory";
            this.btnPromptHistory.Size = new System.Drawing.Size(40, 40);
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
            this.btnQueue.BackgroundImage = global::StableDiffusionGui.Properties.Resources.queueIcon;
            this.btnQueue.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnQueue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQueue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQueue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnQueue.Location = new System.Drawing.Point(806, 629);
            this.btnQueue.Name = "btnQueue";
            this.btnQueue.Size = new System.Drawing.Size(40, 40);
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
            this.btnPostProc.BackgroundImage = global::StableDiffusionGui.Properties.Resources.baseline_auto_fix_high_white_48dp;
            this.btnPostProc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPostProc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPostProc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPostProc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnPostProc.Location = new System.Drawing.Point(922, 9);
            this.btnPostProc.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.btnPostProc.Name = "btnPostProc";
            this.btnPostProc.Size = new System.Drawing.Size(40, 40);
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
            this.btnSettings.BackgroundImage = global::StableDiffusionGui.Properties.Resources.baseline_settings_white_48dp;
            this.btnSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSettings.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnSettings.Location = new System.Drawing.Point(1152, 9);
            this.btnSettings.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(40, 40);
            this.btnSettings.TabIndex = 108;
            this.btnSettings.TabStop = false;
            this.toolTip.SetToolTip(this.btnSettings, "Settings");
            this.btnSettings.UseVisualStyleBackColor = false;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnDebug
            // 
            this.btnDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDebug.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnDebug.BackgroundImage = global::StableDiffusionGui.Properties.Resources.baseline_list_alt_white_48dp;
            this.btnDebug.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDebug.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDebug.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDebug.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnDebug.Location = new System.Drawing.Point(1106, 9);
            this.btnDebug.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.btnDebug.Name = "btnDebug";
            this.btnDebug.Size = new System.Drawing.Size(40, 40);
            this.btnDebug.TabIndex = 107;
            this.btnDebug.TabStop = false;
            this.toolTip.SetToolTip(this.btnDebug, "Logs");
            this.btnDebug.UseVisualStyleBackColor = false;
            this.btnDebug.Click += new System.EventHandler(this.btnDebug_Click);
            // 
            // btnExpandLoras
            // 
            this.btnExpandLoras.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExpandLoras.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnExpandLoras.BackgroundImage = global::StableDiffusionGui.Properties.Resources.downArrowIcon;
            this.btnExpandLoras.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnExpandLoras.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpandLoras.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExpandLoras.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnExpandLoras.Location = new System.Drawing.Point(628, -1);
            this.btnExpandLoras.Margin = new System.Windows.Forms.Padding(0);
            this.btnExpandLoras.Name = "btnExpandLoras";
            this.btnExpandLoras.Size = new System.Drawing.Size(20, 92);
            this.btnExpandLoras.TabIndex = 107;
            this.btnExpandLoras.TabStop = false;
            this.toolTip.SetToolTip(this.btnExpandLoras, "Expand/Collapse LoRA Table");
            this.btnExpandLoras.UseVisualStyleBackColor = false;
            this.btnExpandLoras.Click += new System.EventHandler(this.btnExpandLoras_Click);
            // 
            // btnExpandPromptNegField
            // 
            this.btnExpandPromptNegField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExpandPromptNegField.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnExpandPromptNegField.BackgroundImage = global::StableDiffusionGui.Properties.Resources.downArrowIcon;
            this.btnExpandPromptNegField.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnExpandPromptNegField.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpandPromptNegField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExpandPromptNegField.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnExpandPromptNegField.Location = new System.Drawing.Point(628, -1);
            this.btnExpandPromptNegField.Margin = new System.Windows.Forms.Padding(0);
            this.btnExpandPromptNegField.Name = "btnExpandPromptNegField";
            this.btnExpandPromptNegField.Size = new System.Drawing.Size(20, 33);
            this.btnExpandPromptNegField.TabIndex = 87;
            this.btnExpandPromptNegField.TabStop = false;
            this.toolTip.SetToolTip(this.btnExpandPromptNegField, "Expand/Collapse Prompt Field");
            this.btnExpandPromptNegField.UseVisualStyleBackColor = false;
            this.btnExpandPromptNegField.Click += new System.EventHandler(this.btnExpandPromptNegField_Click);
            // 
            // btnExpandPromptField
            // 
            this.btnExpandPromptField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExpandPromptField.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnExpandPromptField.BackgroundImage = global::StableDiffusionGui.Properties.Resources.downArrowIcon;
            this.btnExpandPromptField.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnExpandPromptField.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpandPromptField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExpandPromptField.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnExpandPromptField.Location = new System.Drawing.Point(628, -1);
            this.btnExpandPromptField.Margin = new System.Windows.Forms.Padding(0);
            this.btnExpandPromptField.Name = "btnExpandPromptField";
            this.btnExpandPromptField.Size = new System.Drawing.Size(20, 58);
            this.btnExpandPromptField.TabIndex = 86;
            this.btnExpandPromptField.TabStop = false;
            this.toolTip.SetToolTip(this.btnExpandPromptField, "Expand/Collapse Prompt Field");
            this.btnExpandPromptField.UseVisualStyleBackColor = false;
            this.btnExpandPromptField.Click += new System.EventHandler(this.btnExpandPromptField_Click);
            // 
            // cliButton
            // 
            this.cliButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cliButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.cliButton.BackgroundImage = global::StableDiffusionGui.Properties.Resources.iconUtils;
            this.cliButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cliButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cliButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cliButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.cliButton.Location = new System.Drawing.Point(1014, 9);
            this.cliButton.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.cliButton.Name = "cliButton";
            this.cliButton.Size = new System.Drawing.Size(40, 40);
            this.cliButton.TabIndex = 103;
            this.cliButton.TabStop = false;
            this.toolTip.SetToolTip(this.cliButton, "Developer Tools");
            this.cliButton.UseVisualStyleBackColor = false;
            this.cliButton.Click += new System.EventHandler(this.cliButton_Click);
            // 
            // installerBtn
            // 
            this.installerBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.installerBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.installerBtn.BackgroundImage = global::StableDiffusionGui.Properties.Resources.installIcon;
            this.installerBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.installerBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.installerBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.installerBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.installerBtn.Location = new System.Drawing.Point(1060, 9);
            this.installerBtn.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.installerBtn.Name = "installerBtn";
            this.installerBtn.Size = new System.Drawing.Size(40, 40);
            this.installerBtn.TabIndex = 76;
            this.installerBtn.TabStop = false;
            this.toolTip.SetToolTip(this.installerBtn, "Manage Installation and Install Updates");
            this.installerBtn.UseVisualStyleBackColor = false;
            this.installerBtn.Click += new System.EventHandler(this.installerBtn_Click);
            // 
            // discordBtn
            // 
            this.discordBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.discordBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.discordBtn.BackgroundImage = global::StableDiffusionGui.Properties.Resources.discordNew;
            this.discordBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.discordBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.discordBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.discordBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.discordBtn.Location = new System.Drawing.Point(830, 9);
            this.discordBtn.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.discordBtn.Name = "discordBtn";
            this.discordBtn.Size = new System.Drawing.Size(40, 40);
            this.discordBtn.TabIndex = 74;
            this.discordBtn.TabStop = false;
            this.toolTip.SetToolTip(this.discordBtn, "Chat on Discord");
            this.discordBtn.UseVisualStyleBackColor = false;
            this.discordBtn.Click += new System.EventHandler(this.discordBtn_Click);
            // 
            // patreonBtn
            // 
            this.patreonBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.patreonBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.patreonBtn.BackgroundImage = global::StableDiffusionGui.Properties.Resources.patreon256pxColored;
            this.patreonBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.patreonBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.patreonBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.patreonBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.patreonBtn.Location = new System.Drawing.Point(784, 9);
            this.patreonBtn.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.patreonBtn.Name = "patreonBtn";
            this.patreonBtn.Size = new System.Drawing.Size(40, 40);
            this.patreonBtn.TabIndex = 73;
            this.patreonBtn.TabStop = false;
            this.toolTip.SetToolTip(this.patreonBtn, "Support Me on Patreon");
            this.patreonBtn.UseVisualStyleBackColor = false;
            this.patreonBtn.Click += new System.EventHandler(this.patreonBtn_Click);
            // 
            // paypalBtn
            // 
            this.paypalBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.paypalBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.paypalBtn.BackgroundImage = global::StableDiffusionGui.Properties.Resources.paypal256px;
            this.paypalBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.paypalBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.paypalBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.paypalBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.paypalBtn.Location = new System.Drawing.Point(738, 9);
            this.paypalBtn.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.paypalBtn.Name = "paypalBtn";
            this.paypalBtn.Size = new System.Drawing.Size(40, 40);
            this.paypalBtn.TabIndex = 72;
            this.paypalBtn.TabStop = false;
            this.toolTip.SetToolTip(this.paypalBtn, "Donate One-Time via PayPal");
            this.paypalBtn.UseVisualStyleBackColor = false;
            this.paypalBtn.Click += new System.EventHandler(this.paypalBtn_Click);
            // 
            // textboxExtraRefinerValues
            // 
            this.textboxExtraRefinerValues.AllowDrop = true;
            this.textboxExtraRefinerValues.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxExtraRefinerValues.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textboxExtraRefinerValues.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textboxExtraRefinerValues.DisableUnfocusedInput = true;
            this.textboxExtraRefinerValues.ForeColor = System.Drawing.Color.White;
            this.textboxExtraRefinerValues.Location = new System.Drawing.Point(346, 3);
            this.textboxExtraRefinerValues.MaxTextZoomFactor = 1F;
            this.textboxExtraRefinerValues.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxExtraRefinerValues.Name = "textboxExtraRefinerValues";
            this.textboxExtraRefinerValues.Placeholder = "";
            this.textboxExtraRefinerValues.PlaceholderTextColor = System.Drawing.Color.Silver;
            this.textboxExtraRefinerValues.Size = new System.Drawing.Size(69, 21);
            this.textboxExtraRefinerValues.TabIndex = 91;
            this.toolTip.SetToolTip(this.textboxExtraRefinerValues, resources.GetString("textboxExtraRefinerValues.ToolTip"));
            // 
            // sliderRefinerStart
            // 
            this.sliderRefinerStart.ActualMaximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.sliderRefinerStart.ActualMinimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.sliderRefinerStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.sliderRefinerStart.BorderRoundRectSize = new System.Drawing.Size(12, 12);
            this.sliderRefinerStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sliderRefinerStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.sliderRefinerStart.ForeColor = System.Drawing.Color.Black;
            this.sliderRefinerStart.InitValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.sliderRefinerStart.LargeChange = ((uint)(2u));
            this.sliderRefinerStart.Location = new System.Drawing.Point(0, 0);
            this.sliderRefinerStart.Margin = new System.Windows.Forms.Padding(0);
            this.sliderRefinerStart.Maximum = 20;
            this.sliderRefinerStart.Name = "sliderRefinerStart";
            this.sliderRefinerStart.OverlayColor = System.Drawing.Color.White;
            this.sliderRefinerStart.Size = new System.Drawing.Size(308, 25);
            this.sliderRefinerStart.SmallChange = ((uint)(1u));
            this.sliderRefinerStart.TabIndex = 4;
            this.sliderRefinerStart.ThumbRoundRectSize = new System.Drawing.Size(16, 16);
            this.sliderRefinerStart.ThumbSize = new System.Drawing.Size(14, 14);
            this.toolTip.SetToolTip(this.sliderRefinerStart, "Higher Value: Base Model is used more, Refiner Model is used less\r\nLower Value: B" +
        "ase Model is used less, Refiner Model is used more");
            this.sliderRefinerStart.Value = 2;
            this.sliderRefinerStart.ValueBox = this.textboxSliderRefineStart;
            this.sliderRefinerStart.ValueStep = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            // 
            // textboxSliderRefineStart
            // 
            this.textboxSliderRefineStart.AllowDrop = true;
            this.textboxSliderRefineStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxSliderRefineStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.textboxSliderRefineStart.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textboxSliderRefineStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textboxSliderRefineStart.ForeColor = System.Drawing.Color.Silver;
            this.textboxSliderRefineStart.Location = new System.Drawing.Point(308, 2);
            this.textboxSliderRefineStart.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.textboxSliderRefineStart.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxSliderRefineStart.Name = "textboxSliderRefineStart";
            this.textboxSliderRefineStart.Size = new System.Drawing.Size(35, 21);
            this.textboxSliderRefineStart.TabIndex = 94;
            this.textboxSliderRefineStart.Text = "0.1";
            // 
            // updownUpscaleFactor
            // 
            this.updownUpscaleFactor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.updownUpscaleFactor.DecimalPlaces = 3;
            this.updownUpscaleFactor.ForeColor = System.Drawing.Color.White;
            this.updownUpscaleFactor.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.updownUpscaleFactor.Location = new System.Drawing.Point(209, 0);
            this.updownUpscaleFactor.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.updownUpscaleFactor.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.updownUpscaleFactor.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.updownUpscaleFactor.Name = "updownUpscaleFactor";
            this.updownUpscaleFactor.Size = new System.Drawing.Size(55, 20);
            this.updownUpscaleFactor.TabIndex = 110;
            this.toolTip.SetToolTip(this.updownUpscaleFactor, "Latent Upscaling Factor");
            this.updownUpscaleFactor.Value = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            // 
            // comboxControlnet
            // 
            this.comboxControlnet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxControlnet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxControlnet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxControlnet.ForeColor = System.Drawing.Color.White;
            this.comboxControlnet.FormattingEnabled = true;
            this.comboxControlnet.Location = new System.Drawing.Point(319, 36);
            this.comboxControlnet.Name = "comboxControlnet";
            this.comboxControlnet.Size = new System.Drawing.Size(223, 21);
            this.comboxControlnet.TabIndex = 107;
            this.toolTip.SetToolTip(this.comboxControlnet, "Changes how the image is sampled.\r\nEuler Ancestral works very well at low step co" +
        "unts.");
            // 
            // comboxPreprocessor
            // 
            this.comboxPreprocessor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboxPreprocessor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxPreprocessor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxPreprocessor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxPreprocessor.ForeColor = System.Drawing.Color.White;
            this.comboxPreprocessor.FormattingEnabled = true;
            this.comboxPreprocessor.Location = new System.Drawing.Point(319, 0);
            this.comboxPreprocessor.Name = "comboxPreprocessor";
            this.comboxPreprocessor.Size = new System.Drawing.Size(330, 21);
            this.comboxPreprocessor.TabIndex = 112;
            this.toolTip.SetToolTip(this.comboxPreprocessor, "Changes how the image is sampled.\r\nEuler Ancestral works very well at low step co" +
        "unts.");
            // 
            // comboxClipSkip
            // 
            this.comboxClipSkip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboxClipSkip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxClipSkip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxClipSkip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxClipSkip.ForeColor = System.Drawing.Color.White;
            this.comboxClipSkip.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3"});
            this.comboxClipSkip.Location = new System.Drawing.Point(598, 0);
            this.comboxClipSkip.Name = "comboxClipSkip";
            this.comboxClipSkip.Size = new System.Drawing.Size(50, 21);
            this.comboxClipSkip.TabIndex = 111;
            this.toolTip.SetToolTip(this.comboxClipSkip, "Select how the model should be loaded. This is not necessary for Diffusers models" +
        ".");
            // 
            // sliderGuidance
            // 
            this.sliderGuidance.ActualMaximum = new decimal(new int[] {
            100,
            0,
            0,
            65536});
            this.sliderGuidance.ActualMinimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.sliderGuidance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.sliderGuidance.BorderRoundRectSize = new System.Drawing.Size(12, 12);
            this.sliderGuidance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sliderGuidance.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.sliderGuidance.ForeColor = System.Drawing.Color.Black;
            this.sliderGuidance.InitValue = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.sliderGuidance.LargeChange = ((uint)(5u));
            this.sliderGuidance.Location = new System.Drawing.Point(0, 0);
            this.sliderGuidance.Margin = new System.Windows.Forms.Padding(0);
            this.sliderGuidance.Maximum = 20;
            this.sliderGuidance.Name = "sliderGuidance";
            this.sliderGuidance.OverlayColor = System.Drawing.Color.White;
            this.sliderGuidance.Size = new System.Drawing.Size(308, 25);
            this.sliderGuidance.SmallChange = ((uint)(1u));
            this.sliderGuidance.TabIndex = 4;
            this.sliderGuidance.ThumbRoundRectSize = new System.Drawing.Size(16, 16);
            this.sliderGuidance.ThumbSize = new System.Drawing.Size(14, 14);
            this.toolTip.SetToolTip(this.sliderGuidance, "Higher tries to match your prompt better, but can get chaotic. 7-12 is a safe ran" +
        "ge in most cases.");
            this.sliderGuidance.Value = 0;
            this.sliderGuidance.ValueBox = this.textboxSliderGuidance;
            this.sliderGuidance.ValueStep = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // textboxSliderGuidance
            // 
            this.textboxSliderGuidance.AllowDrop = true;
            this.textboxSliderGuidance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxSliderGuidance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.textboxSliderGuidance.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textboxSliderGuidance.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textboxSliderGuidance.ForeColor = System.Drawing.Color.Silver;
            this.textboxSliderGuidance.Location = new System.Drawing.Point(308, 2);
            this.textboxSliderGuidance.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.textboxSliderGuidance.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxSliderGuidance.Name = "textboxSliderGuidance";
            this.textboxSliderGuidance.Size = new System.Drawing.Size(35, 21);
            this.textboxSliderGuidance.TabIndex = 93;
            this.textboxSliderGuidance.Text = "0";
            // 
            // textboxExtraGuidances
            // 
            this.textboxExtraGuidances.AllowDrop = true;
            this.textboxExtraGuidances.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxExtraGuidances.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textboxExtraGuidances.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textboxExtraGuidances.DisableUnfocusedInput = true;
            this.textboxExtraGuidances.ForeColor = System.Drawing.Color.White;
            this.textboxExtraGuidances.Location = new System.Drawing.Point(346, 3);
            this.textboxExtraGuidances.MaxTextZoomFactor = 1F;
            this.textboxExtraGuidances.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxExtraGuidances.Name = "textboxExtraGuidances";
            this.textboxExtraGuidances.Placeholder = "";
            this.textboxExtraGuidances.PlaceholderTextColor = System.Drawing.Color.Silver;
            this.textboxExtraGuidances.Size = new System.Drawing.Size(69, 21);
            this.textboxExtraGuidances.TabIndex = 3;
            this.toolTip.SetToolTip(this.textboxExtraGuidances, resources.GetString("textboxExtraGuidances.ToolTip"));
            // 
            // updownUpscaleResultW
            // 
            this.updownUpscaleResultW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.updownUpscaleResultW.Enabled = false;
            this.updownUpscaleResultW.ForeColor = System.Drawing.Color.White;
            this.updownUpscaleResultW.Increment = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.updownUpscaleResultW.Location = new System.Drawing.Point(283, 0);
            this.updownUpscaleResultW.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.updownUpscaleResultW.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.updownUpscaleResultW.Minimum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.updownUpscaleResultW.Name = "updownUpscaleResultW";
            this.updownUpscaleResultW.Size = new System.Drawing.Size(50, 20);
            this.updownUpscaleResultW.TabIndex = 111;
            this.updownUpscaleResultW.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            // 
            // updownUpscaleResultH
            // 
            this.updownUpscaleResultH.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.updownUpscaleResultH.Enabled = false;
            this.updownUpscaleResultH.ForeColor = System.Drawing.Color.White;
            this.updownUpscaleResultH.Increment = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.updownUpscaleResultH.Location = new System.Drawing.Point(351, 0);
            this.updownUpscaleResultH.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.updownUpscaleResultH.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.updownUpscaleResultH.Minimum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.updownUpscaleResultH.Name = "updownUpscaleResultH";
            this.updownUpscaleResultH.Size = new System.Drawing.Size(50, 20);
            this.updownUpscaleResultH.TabIndex = 112;
            this.updownUpscaleResultH.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            // 
            // btnCollapseDebug
            // 
            this.btnCollapseDebug.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCollapseDebug.AutoColor = true;
            this.btnCollapseDebug.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnCollapseDebug.ButtonImage = null;
            this.btnCollapseDebug.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnCollapseDebug.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
            this.btnCollapseDebug.DrawImage = false;
            this.btnCollapseDebug.ForeColor = System.Drawing.Color.White;
            this.btnCollapseDebug.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.btnCollapseDebug.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnCollapseDebug.Location = new System.Drawing.Point(0, 0);
            this.btnCollapseDebug.Name = "btnCollapseDebug";
            this.btnCollapseDebug.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.btnCollapseDebug.Size = new System.Drawing.Size(649, 22);
            this.btnCollapseDebug.TabIndex = 107;
            this.btnCollapseDebug.TabStop = false;
            // 
            // btnCollapseSymmetry
            // 
            this.btnCollapseSymmetry.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCollapseSymmetry.AutoColor = true;
            this.btnCollapseSymmetry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnCollapseSymmetry.ButtonImage = null;
            this.btnCollapseSymmetry.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnCollapseSymmetry.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
            this.btnCollapseSymmetry.DrawImage = false;
            this.btnCollapseSymmetry.ForeColor = System.Drawing.Color.White;
            this.btnCollapseSymmetry.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.btnCollapseSymmetry.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnCollapseSymmetry.Location = new System.Drawing.Point(0, 0);
            this.btnCollapseSymmetry.Name = "btnCollapseSymmetry";
            this.btnCollapseSymmetry.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.btnCollapseSymmetry.Size = new System.Drawing.Size(649, 22);
            this.btnCollapseSymmetry.TabIndex = 108;
            this.btnCollapseSymmetry.TabStop = false;
            // 
            // btnCollapseRendering
            // 
            this.btnCollapseRendering.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCollapseRendering.AutoColor = true;
            this.btnCollapseRendering.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnCollapseRendering.ButtonImage = null;
            this.btnCollapseRendering.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnCollapseRendering.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
            this.btnCollapseRendering.DrawImage = false;
            this.btnCollapseRendering.ForeColor = System.Drawing.Color.White;
            this.btnCollapseRendering.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.btnCollapseRendering.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnCollapseRendering.Location = new System.Drawing.Point(0, 0);
            this.btnCollapseRendering.Name = "btnCollapseRendering";
            this.btnCollapseRendering.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.btnCollapseRendering.Size = new System.Drawing.Size(649, 22);
            this.btnCollapseRendering.TabIndex = 109;
            this.btnCollapseRendering.TabStop = false;
            // 
            // btnCollapseGeneration
            // 
            this.btnCollapseGeneration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCollapseGeneration.AutoColor = true;
            this.btnCollapseGeneration.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnCollapseGeneration.ButtonImage = null;
            this.btnCollapseGeneration.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnCollapseGeneration.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
            this.btnCollapseGeneration.DrawImage = false;
            this.btnCollapseGeneration.ForeColor = System.Drawing.Color.White;
            this.btnCollapseGeneration.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.btnCollapseGeneration.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnCollapseGeneration.Location = new System.Drawing.Point(0, 0);
            this.btnCollapseGeneration.Name = "btnCollapseGeneration";
            this.btnCollapseGeneration.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.btnCollapseGeneration.Size = new System.Drawing.Size(649, 22);
            this.btnCollapseGeneration.TabIndex = 110;
            this.btnCollapseGeneration.TabStop = false;
            // 
            // labelCurrentImage
            // 
            this.labelCurrentImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCurrentImage.AutoEllipsis = true;
            this.labelCurrentImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurrentImage.ForeColor = System.Drawing.Color.Silver;
            this.labelCurrentImage.Location = new System.Drawing.Point(344, 3);
            this.labelCurrentImage.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.labelCurrentImage.Name = "labelCurrentImage";
            this.labelCurrentImage.Size = new System.Drawing.Size(257, 13);
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
            this.menuStripLogs.Size = new System.Drawing.Size(160, 26);
            // 
            // viewLogInRealtimeToolStripMenuItem
            // 
            this.viewLogInRealtimeToolStripMenuItem.Name = "viewLogInRealtimeToolStripMenuItem";
            this.viewLogInRealtimeToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.viewLogInRealtimeToolStripMenuItem.Text = "View Log In Realtime";
            this.viewLogInRealtimeToolStripMenuItem.Click += new System.EventHandler(this.viewLogInRealtimeToolStripMenuItem_Click);
            // 
            // progressBarImg
            // 
            this.progressBarImg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarImg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.progressBarImg.BorderThickness = 0;
            this.progressBarImg.Location = new System.Drawing.Point(680, 600);
            this.progressBarImg.Margin = new System.Windows.Forms.Padding(0);
            this.progressBarImg.Name = "progressBarImg";
            this.progressBarImg.Size = new System.Drawing.Size(512, 5);
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
            this.menuStripRunQueue.Size = new System.Drawing.Size(207, 48);
            // 
            // generateCurrentPromptToolStripMenuItem
            // 
            this.generateCurrentPromptToolStripMenuItem.Name = "generateCurrentPromptToolStripMenuItem";
            this.generateCurrentPromptToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.generateCurrentPromptToolStripMenuItem.Text = "Generate Current Prompt";
            this.generateCurrentPromptToolStripMenuItem.Click += new System.EventHandler(this.generateCurrentPromptToolStripMenuItem_Click);
            // 
            // generateAllQueuedPromptsToolStripMenuItem
            // 
            this.generateAllQueuedPromptsToolStripMenuItem.Name = "generateAllQueuedPromptsToolStripMenuItem";
            this.generateAllQueuedPromptsToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
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
            this.menuStripAddToQueue.Size = new System.Drawing.Size(212, 26);
            // 
            // addCurrentSettingsToQueueToolStripMenuItem
            // 
            this.addCurrentSettingsToQueueToolStripMenuItem.Name = "addCurrentSettingsToQueueToolStripMenuItem";
            this.addCurrentSettingsToQueueToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
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
            this.menuStripDeleteImages.Size = new System.Drawing.Size(184, 48);
            // 
            // deleteThisImageToolStripMenuItem
            // 
            this.deleteThisImageToolStripMenuItem.Name = "deleteThisImageToolStripMenuItem";
            this.deleteThisImageToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.deleteThisImageToolStripMenuItem.Text = "Delete This Image";
            this.deleteThisImageToolStripMenuItem.Click += new System.EventHandler(this.deleteThisImageToolStripMenuItem_Click);
            // 
            // deleteAllCurrentImagesToolStripMenuItem
            // 
            this.deleteAllCurrentImagesToolStripMenuItem.Name = "deleteAllCurrentImagesToolStripMenuItem";
            this.deleteAllCurrentImagesToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
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
            this.convertModelsToolStripMenuItem,
            this.downloadHuggingfaceModelToolStripMenuItem,
            this.downloadStableDiffusionModelToolStripMenuItem});
            this.menuStripDevTools.Name = "menuStripDevTools";
            this.menuStripDevTools.ShowImageMargin = false;
            this.menuStripDevTools.Size = new System.Drawing.Size(234, 158);
            // 
            // openCliToolStripMenuItem
            // 
            this.openCliToolStripMenuItem.Name = "openCliToolStripMenuItem";
            this.openCliToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.openCliToolStripMenuItem.Text = "Open Stable Diffusion CLI";
            this.openCliToolStripMenuItem.Click += new System.EventHandler(this.openDreampyCLIToolStripMenuItem_Click);
            // 
            // openCmdInPythonEnvironmentToolStripMenuItem
            // 
            this.openCmdInPythonEnvironmentToolStripMenuItem.Name = "openCmdInPythonEnvironmentToolStripMenuItem";
            this.openCmdInPythonEnvironmentToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.openCmdInPythonEnvironmentToolStripMenuItem.Text = "Open CMD in Python Environment";
            this.openCmdInPythonEnvironmentToolStripMenuItem.Click += new System.EventHandler(this.openCmdInPythonEnvironmentToolStripMenuItem_Click);
            // 
            // openModelMergeToolToolStripMenuItem
            // 
            this.openModelMergeToolToolStripMenuItem.Name = "openModelMergeToolToolStripMenuItem";
            this.openModelMergeToolToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.openModelMergeToolToolStripMenuItem.Text = "Merge Models";
            this.openModelMergeToolToolStripMenuItem.Click += new System.EventHandler(this.openModelMergeToolToolStripMenuItem_Click);
            // 
            // openModelPruningTrimmingToolToolStripMenuItem
            // 
            this.openModelPruningTrimmingToolToolStripMenuItem.Name = "openModelPruningTrimmingToolToolStripMenuItem";
            this.openModelPruningTrimmingToolToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.openModelPruningTrimmingToolToolStripMenuItem.Text = "Prune (Trim) Models";
            this.openModelPruningTrimmingToolToolStripMenuItem.Visible = false;
            this.openModelPruningTrimmingToolToolStripMenuItem.Click += new System.EventHandler(this.openModelPruningTrimmingToolToolStripMenuItem_Click);
            // 
            // convertModelsToolStripMenuItem
            // 
            this.convertModelsToolStripMenuItem.Name = "convertModelsToolStripMenuItem";
            this.convertModelsToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.convertModelsToolStripMenuItem.Text = "Convert Models";
            this.convertModelsToolStripMenuItem.Click += new System.EventHandler(this.convertModelsToolStripMenuItem_Click);
            // 
            // downloadHuggingfaceModelToolStripMenuItem
            // 
            this.downloadHuggingfaceModelToolStripMenuItem.Name = "downloadHuggingfaceModelToolStripMenuItem";
            this.downloadHuggingfaceModelToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.downloadHuggingfaceModelToolStripMenuItem.Text = "Download Huggingface Model";
            this.downloadHuggingfaceModelToolStripMenuItem.Click += new System.EventHandler(this.downloadHuggingfaceModelToolStripMenuItem_Click);
            // 
            // downloadStableDiffusionModelToolStripMenuItem
            // 
            this.downloadStableDiffusionModelToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sD15ToolStripMenuItem,
            this.sD15ONNXToolStripMenuItem,
            this.sDXL10ToolStripMenuItem,
            this.sDXL10RefinerToolStripMenuItem});
            this.downloadStableDiffusionModelToolStripMenuItem.Name = "downloadStableDiffusionModelToolStripMenuItem";
            this.downloadStableDiffusionModelToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.downloadStableDiffusionModelToolStripMenuItem.Text = "Download Stable Diffusion Model";
            // 
            // sD15ToolStripMenuItem
            // 
            this.sD15ToolStripMenuItem.Name = "sD15ToolStripMenuItem";
            this.sD15ToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.sD15ToolStripMenuItem.Text = "SD 1.5";
            this.sD15ToolStripMenuItem.Click += new System.EventHandler(this.sD15ToolStripMenuItem_Click);
            // 
            // sD15ONNXToolStripMenuItem
            // 
            this.sD15ONNXToolStripMenuItem.Name = "sD15ONNXToolStripMenuItem";
            this.sD15ONNXToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.sD15ONNXToolStripMenuItem.Text = "SD 1.5 ONNX";
            this.sD15ONNXToolStripMenuItem.Click += new System.EventHandler(this.sD15ONNXToolStripMenuItem_Click);
            // 
            // sDXL10ToolStripMenuItem
            // 
            this.sDXL10ToolStripMenuItem.Name = "sDXL10ToolStripMenuItem";
            this.sDXL10ToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.sDXL10ToolStripMenuItem.Text = "SD XL 1.0 Base";
            this.sDXL10ToolStripMenuItem.Click += new System.EventHandler(this.sDXL10ToolStripMenuItem_Click);
            // 
            // sDXL10RefinerToolStripMenuItem
            // 
            this.sDXL10RefinerToolStripMenuItem.Name = "sDXL10RefinerToolStripMenuItem";
            this.sDXL10RefinerToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.sDXL10RefinerToolStripMenuItem.Text = "SD XL 1.0 Refiner";
            this.sDXL10RefinerToolStripMenuItem.Click += new System.EventHandler(this.sDXL10RefinerToolStripMenuItem_Click);
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
            this.menuStripPostProcess.Size = new System.Drawing.Size(171, 70);
            // 
            // upscaleToolStripMenuItem
            // 
            this.upscaleToolStripMenuItem.Name = "upscaleToolStripMenuItem";
            this.upscaleToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.upscaleToolStripMenuItem.Text = "Apply Upscaling";
            this.upscaleToolStripMenuItem.Click += new System.EventHandler(this.upscaleToolStripMenuItem_Click);
            // 
            // faceRestorationToolStripMenuItem
            // 
            this.faceRestorationToolStripMenuItem.Name = "faceRestorationToolStripMenuItem";
            this.faceRestorationToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.faceRestorationToolStripMenuItem.Text = "Apply Face Restoration";
            this.faceRestorationToolStripMenuItem.Click += new System.EventHandler(this.applyFaceRestorationToolStripMenuItem_Click);
            // 
            // applyAllToolStripMenuItem
            // 
            this.applyAllToolStripMenuItem.Name = "applyAllToolStripMenuItem";
            this.applyAllToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.applyAllToolStripMenuItem.Text = "Apply All";
            this.applyAllToolStripMenuItem.Click += new System.EventHandler(this.applyAllToolStripMenuItem_Click);
            // 
            // panelSettings
            // 
            this.panelSettings.AllowScrolling = true;
            this.panelSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelSettings.AutoScroll = true;
            this.panelSettings.Controls.Add(this.panelDebugLoopback);
            this.panelSettings.Controls.Add(this.panelCollapseDebug);
            this.panelSettings.Controls.Add(this.panelSeamless);
            this.panelSettings.Controls.Add(this.panelCollapseSymmetry);
            this.panelSettings.Controls.Add(this.panelSampler);
            this.panelSettings.Controls.Add(this.panelUpscaling);
            this.panelSettings.Controls.Add(this.panelRes);
            this.panelSettings.Controls.Add(this.panelCollapseRendering);
            this.panelSettings.Controls.Add(this.panelSeed);
            this.panelSettings.Controls.Add(this.panelGuidance);
            this.panelSettings.Controls.Add(this.panelScaleImg);
            this.panelSettings.Controls.Add(this.panelScale);
            this.panelSettings.Controls.Add(this.panelRefineStart);
            this.panelSettings.Controls.Add(this.panelSteps);
            this.panelSettings.Controls.Add(this.panelIterations);
            this.panelSettings.Controls.Add(this.panelControlnet);
            this.panelSettings.Controls.Add(this.panelInitImgStrength);
            this.panelSettings.Controls.Add(this.panelInpainting);
            this.panelSettings.Controls.Add(this.panelCollapseGeneration);
            this.panelSettings.Controls.Add(this.panelBaseImg);
            this.panelSettings.Controls.Add(this.panelLoras);
            this.panelSettings.Controls.Add(this.panelEmbeddings);
            this.panelSettings.Controls.Add(this.panelPromptNeg);
            this.panelSettings.Controls.Add(this.panelPrompt);
            this.panelSettings.Controls.Add(this.panelCollapsePrompt);
            this.panelSettings.Controls.Add(this.panelModel2);
            this.panelSettings.Controls.Add(this.panelModelSettings);
            this.panelSettings.Controls.Add(this.panelModel);
            this.panelSettings.Controls.Add(this.panelBackend);
            this.panelSettings.Controls.Add(this.panelCollapseImplementation);
            this.panelSettings.CtrlDisablesScrolling = true;
            this.panelSettings.Location = new System.Drawing.Point(6, 59);
            this.panelSettings.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.OnlyAllowScrollIfNeeded = true;
            this.panelSettings.Size = new System.Drawing.Size(668, 546);
            this.panelSettings.TabIndex = 106;
            this.panelSettings.SizeChanged += new System.EventHandler(this.panelSettings_SizeChanged);
            this.panelSettings.Click += new System.EventHandler(this.panel1_Click);
            // 
            // panelDebugLoopback
            // 
            this.panelDebugLoopback.Controls.Add(this.checkboxLoopback);
            this.panelDebugLoopback.Controls.Add(this.label16);
            this.panelDebugLoopback.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDebugLoopback.Location = new System.Drawing.Point(0, 1165);
            this.panelDebugLoopback.Name = "panelDebugLoopback";
            this.panelDebugLoopback.Size = new System.Drawing.Size(651, 35);
            this.panelDebugLoopback.TabIndex = 19;
            this.panelDebugLoopback.Visible = false;
            // 
            // checkboxLoopback
            // 
            this.checkboxLoopback.AutoSize = true;
            this.checkboxLoopback.ForeColor = System.Drawing.Color.White;
            this.checkboxLoopback.Location = new System.Drawing.Point(233, 0);
            this.checkboxLoopback.Name = "checkboxLoopback";
            this.checkboxLoopback.Padding = new System.Windows.Forms.Padding(3);
            this.checkboxLoopback.Size = new System.Drawing.Size(21, 20);
            this.checkboxLoopback.TabIndex = 110;
            this.checkboxLoopback.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.White;
            this.label16.Location = new System.Drawing.Point(2, 4);
            this.label16.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(164, 13);
            this.label16.TabIndex = 105;
            this.label16.Text = "Enable Loopback Img2Img Mode";
            // 
            // panelCollapseDebug
            // 
            this.panelCollapseDebug.Controls.Add(this.btnCollapseDebug);
            this.panelCollapseDebug.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCollapseDebug.Location = new System.Drawing.Point(0, 1130);
            this.panelCollapseDebug.Name = "panelCollapseDebug";
            this.panelCollapseDebug.Size = new System.Drawing.Size(651, 35);
            this.panelCollapseDebug.TabIndex = 117;
            // 
            // panelSeamless
            // 
            this.panelSeamless.Controls.Add(this.comboxSymmetry);
            this.panelSeamless.Controls.Add(this.comboxSeamless);
            this.panelSeamless.Controls.Add(this.label8);
            this.panelSeamless.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSeamless.Location = new System.Drawing.Point(0, 1095);
            this.panelSeamless.Name = "panelSeamless";
            this.panelSeamless.Size = new System.Drawing.Size(651, 35);
            this.panelSeamless.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(0, 4);
            this.label8.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(224, 13);
            this.label8.TabIndex = 105;
            this.label8.Text = "Seamless (Tileable) and Symmetry (Mirror Axis)";
            // 
            // panelCollapseSymmetry
            // 
            this.panelCollapseSymmetry.Controls.Add(this.btnCollapseSymmetry);
            this.panelCollapseSymmetry.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCollapseSymmetry.Location = new System.Drawing.Point(0, 1060);
            this.panelCollapseSymmetry.Name = "panelCollapseSymmetry";
            this.panelCollapseSymmetry.Size = new System.Drawing.Size(651, 35);
            this.panelCollapseSymmetry.TabIndex = 118;
            // 
            // panelSampler
            // 
            this.panelSampler.Controls.Add(this.label7);
            this.panelSampler.Controls.Add(this.comboxSampler);
            this.panelSampler.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSampler.Location = new System.Drawing.Point(0, 1025);
            this.panelSampler.Name = "panelSampler";
            this.panelSampler.Size = new System.Drawing.Size(651, 35);
            this.panelSampler.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(0, 4);
            this.label7.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 104;
            this.label7.Text = "Sampler";
            // 
            // panelUpscaling
            // 
            this.panelUpscaling.ColumnCount = 2;
            this.panelUpscaling.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 233F));
            this.panelUpscaling.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelUpscaling.Controls.Add(this.label27, 0, 0);
            this.panelUpscaling.Controls.Add(this.flowLayoutPanel1, 1, 0);
            this.panelUpscaling.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUpscaling.Location = new System.Drawing.Point(0, 990);
            this.panelUpscaling.Margin = new System.Windows.Forms.Padding(0);
            this.panelUpscaling.Name = "panelUpscaling";
            this.panelUpscaling.RowCount = 1;
            this.panelUpscaling.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelUpscaling.Size = new System.Drawing.Size(651, 35);
            this.panelUpscaling.TabIndex = 109;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.ForeColor = System.Drawing.Color.White;
            this.label27.Location = new System.Drawing.Point(0, 4);
            this.label27.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(187, 13);
            this.label27.TabIndex = 109;
            this.label27.Text = "Latent Upscaling (High-Resolution Fix)";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.comboxUpscaleMode);
            this.flowLayoutPanel1.Controls.Add(this.labelUpscale);
            this.flowLayoutPanel1.Controls.Add(this.updownUpscaleFactor);
            this.flowLayoutPanel1.Controls.Add(this.labelUpscaleEquals);
            this.flowLayoutPanel1.Controls.Add(this.updownUpscaleResultW);
            this.flowLayoutPanel1.Controls.Add(this.labelUpscaleX);
            this.flowLayoutPanel1.Controls.Add(this.updownUpscaleResultH);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(233, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(418, 35);
            this.flowLayoutPanel1.TabIndex = 110;
            // 
            // comboxUpscaleMode
            // 
            this.comboxUpscaleMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxUpscaleMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxUpscaleMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxUpscaleMode.ForeColor = System.Drawing.Color.White;
            this.comboxUpscaleMode.FormattingEnabled = true;
            this.comboxUpscaleMode.Items.AddRange(new object[] {
            "By Factor",
            "To Resolution"});
            this.comboxUpscaleMode.Location = new System.Drawing.Point(0, 0);
            this.comboxUpscaleMode.Margin = new System.Windows.Forms.Padding(0, 0, 6, 3);
            this.comboxUpscaleMode.Name = "comboxUpscaleMode";
            this.comboxUpscaleMode.Size = new System.Drawing.Size(160, 21);
            this.comboxUpscaleMode.TabIndex = 108;
            // 
            // labelUpscale
            // 
            this.labelUpscale.AutoSize = true;
            this.labelUpscale.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUpscale.ForeColor = System.Drawing.Color.White;
            this.labelUpscale.Location = new System.Drawing.Point(166, 3);
            this.labelUpscale.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.labelUpscale.Name = "labelUpscale";
            this.labelUpscale.Size = new System.Drawing.Size(40, 13);
            this.labelUpscale.TabIndex = 109;
            this.labelUpscale.Text = "Factor:";
            // 
            // labelUpscaleEquals
            // 
            this.labelUpscaleEquals.AutoSize = true;
            this.labelUpscaleEquals.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUpscaleEquals.ForeColor = System.Drawing.Color.White;
            this.labelUpscaleEquals.Location = new System.Drawing.Point(267, 3);
            this.labelUpscaleEquals.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.labelUpscaleEquals.Name = "labelUpscaleEquals";
            this.labelUpscaleEquals.Size = new System.Drawing.Size(13, 13);
            this.labelUpscaleEquals.TabIndex = 113;
            this.labelUpscaleEquals.Text = "=";
            // 
            // labelUpscaleX
            // 
            this.labelUpscaleX.AutoSize = true;
            this.labelUpscaleX.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUpscaleX.ForeColor = System.Drawing.Color.White;
            this.labelUpscaleX.Location = new System.Drawing.Point(336, 3);
            this.labelUpscaleX.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.labelUpscaleX.Name = "labelUpscaleX";
            this.labelUpscaleX.Size = new System.Drawing.Size(12, 13);
            this.labelUpscaleX.TabIndex = 98;
            this.labelUpscaleX.Text = "x";
            // 
            // panelRes
            // 
            this.panelRes.Controls.Add(this.btnResetRes);
            this.panelRes.Controls.Add(this.labelResChange);
            this.panelRes.Controls.Add(this.labelAspectRatio);
            this.panelRes.Controls.Add(this.checkboxHiresFix);
            this.panelRes.Controls.Add(this.comboxResH);
            this.panelRes.Controls.Add(this.comboxResW);
            this.panelRes.Controls.Add(this.label6);
            this.panelRes.Controls.Add(this.label9);
            this.panelRes.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelRes.Location = new System.Drawing.Point(0, 955);
            this.panelRes.Name = "panelRes";
            this.panelRes.Size = new System.Drawing.Size(651, 35);
            this.panelRes.TabIndex = 5;
            // 
            // labelResChange
            // 
            this.labelResChange.AutoEllipsis = true;
            this.labelResChange.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelResChange.ForeColor = System.Drawing.Color.White;
            this.labelResChange.Location = new System.Drawing.Point(495, -1);
            this.labelResChange.Name = "labelResChange";
            this.labelResChange.Size = new System.Drawing.Size(75, 23);
            this.labelResChange.TabIndex = 111;
            this.labelResChange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelResChange.UseCompatibleTextRendering = true;
            this.labelResChange.Visible = false;
            // 
            // labelAspectRatio
            // 
            this.labelAspectRatio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelAspectRatio.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAspectRatio.ForeColor = System.Drawing.Color.Silver;
            this.labelAspectRatio.Location = new System.Drawing.Point(566, 3);
            this.labelAspectRatio.Name = "labelAspectRatio";
            this.labelAspectRatio.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelAspectRatio.Size = new System.Drawing.Size(85, 13);
            this.labelAspectRatio.TabIndex = 110;
            this.labelAspectRatio.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboxResH
            // 
            this.comboxResH.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxResH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxResH.ForeColor = System.Drawing.Color.White;
            this.comboxResH.FormattingEnabled = true;
            this.comboxResH.Location = new System.Drawing.Point(348, 0);
            this.comboxResH.Name = "comboxResH";
            this.comboxResH.Size = new System.Drawing.Size(85, 21);
            this.comboxResH.TabIndex = 107;
            // 
            // comboxResW
            // 
            this.comboxResW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxResW.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxResW.ForeColor = System.Drawing.Color.White;
            this.comboxResW.FormattingEnabled = true;
            this.comboxResW.Location = new System.Drawing.Point(233, 0);
            this.comboxResW.Name = "comboxResW";
            this.comboxResW.Size = new System.Drawing.Size(85, 21);
            this.comboxResW.TabIndex = 106;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(0, 4);
            this.label6.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(136, 13);
            this.label6.TabIndex = 95;
            this.label6.Text = "Resolution (Width x Height)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(328, 3);
            this.label9.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(12, 13);
            this.label9.TabIndex = 98;
            this.label9.Text = "x";
            // 
            // panelCollapseRendering
            // 
            this.panelCollapseRendering.Controls.Add(this.btnCollapseRendering);
            this.panelCollapseRendering.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCollapseRendering.Location = new System.Drawing.Point(0, 920);
            this.panelCollapseRendering.Name = "panelCollapseRendering";
            this.panelCollapseRendering.Size = new System.Drawing.Size(651, 35);
            this.panelCollapseRendering.TabIndex = 119;
            // 
            // panelSeed
            // 
            this.panelSeed.Controls.Add(this.checkboxLockSeed);
            this.panelSeed.Controls.Add(this.btnSeedResetToRandom);
            this.panelSeed.Controls.Add(this.btnSeedUsePrevious);
            this.panelSeed.Controls.Add(this.label5);
            this.panelSeed.Controls.Add(this.upDownSeed);
            this.panelSeed.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSeed.Location = new System.Drawing.Point(0, 885);
            this.panelSeed.Name = "panelSeed";
            this.panelSeed.Size = new System.Drawing.Size(651, 35);
            this.panelSeed.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(0, 4);
            this.label5.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(122, 13);
            this.label5.TabIndex = 92;
            this.label5.Text = "Seed (Empty = Random)";
            // 
            // panelGuidance
            // 
            this.panelGuidance.Controls.Add(this.label19);
            this.panelGuidance.Controls.Add(this.tableLayoutPanel5);
            this.panelGuidance.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelGuidance.Location = new System.Drawing.Point(0, 850);
            this.panelGuidance.Name = "panelGuidance";
            this.panelGuidance.Size = new System.Drawing.Size(651, 35);
            this.panelGuidance.TabIndex = 128;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.White;
            this.label19.Location = new System.Drawing.Point(0, 4);
            this.label19.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(117, 13);
            this.label19.TabIndex = 90;
            this.label19.Text = "Prompt Guidance (Flux)";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel5.Controls.Add(this.textboxSliderGuidance, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.sliderGuidance, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.textboxExtraGuidances, 2, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(233, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(418, 25);
            this.tableLayoutPanel5.TabIndex = 91;
            // 
            // panelScaleImg
            // 
            this.panelScaleImg.Controls.Add(this.label17);
            this.panelScaleImg.Controls.Add(this.tableLayoutPanel2);
            this.panelScaleImg.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelScaleImg.Location = new System.Drawing.Point(0, 815);
            this.panelScaleImg.Name = "panelScaleImg";
            this.panelScaleImg.Size = new System.Drawing.Size(651, 35);
            this.panelScaleImg.TabIndex = 20;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.White;
            this.label17.Location = new System.Drawing.Point(0, 4);
            this.label17.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(145, 13);
            this.label17.TabIndex = 90;
            this.label17.Text = "Image Guidance (CFG Scale)";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel2.Controls.Add(this.textboxSliderScaleImg, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.sliderScaleImg, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.textboxExtraScalesImg, 2, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(233, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(418, 25);
            this.tableLayoutPanel2.TabIndex = 91;
            // 
            // panelScale
            // 
            this.panelScale.Controls.Add(this.label4);
            this.panelScale.Controls.Add(this.tableLayoutPanel1);
            this.panelScale.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelScale.Location = new System.Drawing.Point(0, 780);
            this.panelScale.Name = "panelScale";
            this.panelScale.Size = new System.Drawing.Size(651, 35);
            this.panelScale.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(0, 4);
            this.label4.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(149, 13);
            this.label4.TabIndex = 90;
            this.label4.Text = "Prompt Guidance (CFG Scale)";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.Controls.Add(this.textboxSliderScale, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.sliderScale, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textboxExtraScales, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(233, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(418, 25);
            this.tableLayoutPanel1.TabIndex = 91;
            // 
            // panelRefineStart
            // 
            this.panelRefineStart.Controls.Add(this.label25);
            this.panelRefineStart.Controls.Add(this.tableLayoutPanel3);
            this.panelRefineStart.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelRefineStart.Location = new System.Drawing.Point(0, 745);
            this.panelRefineStart.Name = "panelRefineStart";
            this.panelRefineStart.Size = new System.Drawing.Size(651, 35);
            this.panelRefineStart.TabIndex = 124;
            this.panelRefineStart.Visible = false;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.ForeColor = System.Drawing.Color.White;
            this.label25.Location = new System.Drawing.Point(0, 4);
            this.label25.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(181, 13);
            this.label25.TabIndex = 90;
            this.label25.Text = "Image Refine Strength (0 = Disabled)";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel3.Controls.Add(this.textboxExtraRefinerValues, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.textboxSliderRefineStart, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.sliderRefinerStart, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(233, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(418, 25);
            this.tableLayoutPanel3.TabIndex = 89;
            // 
            // panelSteps
            // 
            this.panelSteps.Controls.Add(this.label3);
            this.panelSteps.Controls.Add(this.tableLayoutPanel6);
            this.panelSteps.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSteps.Location = new System.Drawing.Point(0, 710);
            this.panelSteps.Name = "panelSteps";
            this.panelSteps.Size = new System.Drawing.Size(651, 35);
            this.panelSteps.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(0, 4);
            this.label3.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 89;
            this.label3.Text = "Generation Steps";
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel6.ColumnCount = 3;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel6.Controls.Add(this.textboxExtraSteps, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.textboxSliderSteps, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.sliderSteps, 0, 0);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(233, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(418, 25);
            this.tableLayoutPanel6.TabIndex = 88;
            // 
            // textboxSliderSteps
            // 
            this.textboxSliderSteps.AllowDrop = true;
            this.textboxSliderSteps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxSliderSteps.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.textboxSliderSteps.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textboxSliderSteps.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textboxSliderSteps.ForeColor = System.Drawing.Color.Silver;
            this.textboxSliderSteps.Location = new System.Drawing.Point(308, 2);
            this.textboxSliderSteps.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.textboxSliderSteps.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxSliderSteps.Name = "textboxSliderSteps";
            this.textboxSliderSteps.Size = new System.Drawing.Size(35, 21);
            this.textboxSliderSteps.TabIndex = 92;
            this.textboxSliderSteps.Text = "20";
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
            this.sliderSteps.Size = new System.Drawing.Size(308, 25);
            this.sliderSteps.SmallChange = ((uint)(1u));
            this.sliderSteps.TabIndex = 13;
            this.sliderSteps.Text = "sliderSteps";
            this.sliderSteps.ThumbRoundRectSize = new System.Drawing.Size(16, 16);
            this.sliderSteps.ThumbSize = new System.Drawing.Size(14, 14);
            this.sliderSteps.Value = 20;
            this.sliderSteps.ValueBox = this.textboxSliderSteps;
            this.sliderSteps.ValueStep = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // panelIterations
            // 
            this.panelIterations.Controls.Add(this.checkboxPreview);
            this.panelIterations.Controls.Add(this.label1);
            this.panelIterations.Controls.Add(this.upDownIterations);
            this.panelIterations.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelIterations.Location = new System.Drawing.Point(0, 675);
            this.panelIterations.Name = "panelIterations";
            this.panelIterations.Size = new System.Drawing.Size(651, 35);
            this.panelIterations.TabIndex = 1;
            // 
            // checkboxPreview
            // 
            this.checkboxPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkboxPreview.ForeColor = System.Drawing.Color.Silver;
            this.checkboxPreview.Location = new System.Drawing.Point(339, 0);
            this.checkboxPreview.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.checkboxPreview.Name = "checkboxPreview";
            this.checkboxPreview.Size = new System.Drawing.Size(125, 23);
            this.checkboxPreview.TabIndex = 94;
            this.checkboxPreview.Text = "Show Preview";
            this.checkboxPreview.UseVisualStyleBackColor = true;
            this.checkboxPreview.Visible = false;
            this.checkboxPreview.CheckedChanged += new System.EventHandler(this.checkboxPreview_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 4);
            this.label1.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 13);
            this.label1.TabIndex = 85;
            this.label1.Text = "Amount Of Images To Generate";
            // 
            // panelControlnet
            // 
            this.panelControlnet.Controls.Add(this.label14);
            this.panelControlnet.Controls.Add(this.label13);
            this.panelControlnet.Controls.Add(this.label12);
            this.panelControlnet.Controls.Add(this.comboxPreprocessor);
            this.panelControlnet.Controls.Add(this.updownControlnetStrength);
            this.panelControlnet.Controls.Add(this.comboxControlnet);
            this.panelControlnet.Controls.Add(this.label28);
            this.panelControlnet.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControlnet.Location = new System.Drawing.Point(0, 605);
            this.panelControlnet.Name = "panelControlnet";
            this.panelControlnet.Size = new System.Drawing.Size(651, 70);
            this.panelControlnet.TabIndex = 126;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(233, 39);
            this.label14.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(75, 13);
            this.label14.TabIndex = 115;
            this.label14.Text = "Control Model:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(548, 39);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(50, 13);
            this.label13.TabIndex = 114;
            this.label13.Text = "Strength:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(233, 4);
            this.label12.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 13);
            this.label12.TabIndex = 113;
            this.label12.Text = "Pre-processing:";
            // 
            // updownControlnetStrength
            // 
            this.updownControlnetStrength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.updownControlnetStrength.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.updownControlnetStrength.DecimalPlaces = 2;
            this.updownControlnetStrength.ForeColor = System.Drawing.Color.White;
            this.updownControlnetStrength.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.updownControlnetStrength.Location = new System.Drawing.Point(601, 36);
            this.updownControlnetStrength.Margin = new System.Windows.Forms.Padding(0, 0, 6, 3);
            this.updownControlnetStrength.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.updownControlnetStrength.Name = "updownControlnetStrength";
            this.updownControlnetStrength.Size = new System.Drawing.Size(50, 20);
            this.updownControlnetStrength.TabIndex = 111;
            this.updownControlnetStrength.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.ForeColor = System.Drawing.Color.White;
            this.label28.Location = new System.Drawing.Point(0, 4);
            this.label28.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(57, 13);
            this.label28.TabIndex = 105;
            this.label28.Text = "ControlNet";
            // 
            // panelInitImgStrength
            // 
            this.panelInitImgStrength.Controls.Add(this.label11);
            this.panelInitImgStrength.Controls.Add(this.tableLayoutPanel4);
            this.panelInitImgStrength.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInitImgStrength.Location = new System.Drawing.Point(0, 570);
            this.panelInitImgStrength.Name = "panelInitImgStrength";
            this.panelInitImgStrength.Size = new System.Drawing.Size(651, 35);
            this.panelInitImgStrength.TabIndex = 8;
            this.panelInitImgStrength.Visible = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(0, 4);
            this.label11.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(132, 13);
            this.label11.TabIndex = 90;
            this.label11.Text = "Image Strength (Influence)";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel4.Controls.Add(this.textboxExtraInitStrengths, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.textboxSliderInitStrength, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.sliderInitStrength, 0, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(233, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(418, 25);
            this.tableLayoutPanel4.TabIndex = 89;
            // 
            // panelInpainting
            // 
            this.panelInpainting.Controls.Add(this.comboxControlnetSlot);
            this.panelInpainting.Controls.Add(this.panelResizeGravity);
            this.panelInpainting.Controls.Add(this.textboxClipsegMask);
            this.panelInpainting.Controls.Add(this.comboxInpaintMode);
            this.panelInpainting.Controls.Add(this.btnEditMask);
            this.panelInpainting.Controls.Add(this.btnResetMask);
            this.panelInpainting.Controls.Add(this.label10);
            this.panelInpainting.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInpainting.Location = new System.Drawing.Point(0, 535);
            this.panelInpainting.Name = "panelInpainting";
            this.panelInpainting.Size = new System.Drawing.Size(651, 35);
            this.panelInpainting.TabIndex = 12;
            this.panelInpainting.Visible = false;
            // 
            // comboxControlnetSlot
            // 
            this.comboxControlnetSlot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboxControlnetSlot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxControlnetSlot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxControlnetSlot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxControlnetSlot.ForeColor = System.Drawing.Color.White;
            this.comboxControlnetSlot.FormattingEnabled = true;
            this.comboxControlnetSlot.Location = new System.Drawing.Point(439, 0);
            this.comboxControlnetSlot.Name = "comboxControlnetSlot";
            this.comboxControlnetSlot.Size = new System.Drawing.Size(210, 21);
            this.comboxControlnetSlot.TabIndex = 116;
            // 
            // panelResizeGravity
            // 
            this.panelResizeGravity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.panelResizeGravity.Controls.Add(this.label21);
            this.panelResizeGravity.Controls.Add(this.comboxResizeGravity);
            this.panelResizeGravity.Location = new System.Drawing.Point(439, 0);
            this.panelResizeGravity.Name = "panelResizeGravity";
            this.panelResizeGravity.Size = new System.Drawing.Size(209, 21);
            this.panelResizeGravity.TabIndex = 112;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.Color.White;
            this.label21.Location = new System.Drawing.Point(0, 4);
            this.label21.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(107, 13);
            this.label21.TabIndex = 107;
            this.label21.Text = "Extend Image From...";
            // 
            // comboxInpaintMode
            // 
            this.comboxInpaintMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxInpaintMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxInpaintMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxInpaintMode.ForeColor = System.Drawing.Color.White;
            this.comboxInpaintMode.FormattingEnabled = true;
            this.comboxInpaintMode.Location = new System.Drawing.Point(233, 0);
            this.comboxInpaintMode.Name = "comboxInpaintMode";
            this.comboxInpaintMode.Size = new System.Drawing.Size(200, 21);
            this.comboxInpaintMode.TabIndex = 109;
            this.comboxInpaintMode.SelectedIndexChanged += new System.EventHandler(this.comboxInpaintMode_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(0, 4);
            this.label10.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 13);
            this.label10.TabIndex = 105;
            this.label10.Text = "Image Usage";
            // 
            // panelCollapseGeneration
            // 
            this.panelCollapseGeneration.Controls.Add(this.btnCollapseGeneration);
            this.panelCollapseGeneration.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCollapseGeneration.Location = new System.Drawing.Point(0, 500);
            this.panelCollapseGeneration.Name = "panelCollapseGeneration";
            this.panelCollapseGeneration.Size = new System.Drawing.Size(651, 35);
            this.panelCollapseGeneration.TabIndex = 120;
            // 
            // panelBaseImg
            // 
            this.panelBaseImg.Controls.Add(this.checkboxShowInitImg);
            this.panelBaseImg.Controls.Add(this.label2);
            this.panelBaseImg.Controls.Add(this.labelCurrentImage);
            this.panelBaseImg.Controls.Add(this.btnInitImgBrowse);
            this.panelBaseImg.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBaseImg.Location = new System.Drawing.Point(0, 465);
            this.panelBaseImg.Name = "panelBaseImg";
            this.panelBaseImg.Size = new System.Drawing.Size(651, 35);
            this.panelBaseImg.TabIndex = 17;
            // 
            // checkboxShowInitImg
            // 
            this.checkboxShowInitImg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkboxShowInitImg.ForeColor = System.Drawing.Color.Silver;
            this.checkboxShowInitImg.Location = new System.Drawing.Point(598, -1);
            this.checkboxShowInitImg.Name = "checkboxShowInitImg";
            this.checkboxShowInitImg.Size = new System.Drawing.Size(53, 23);
            this.checkboxShowInitImg.TabIndex = 93;
            this.checkboxShowInitImg.Text = "Show";
            this.checkboxShowInitImg.UseVisualStyleBackColor = true;
            this.checkboxShowInitImg.Visible = false;
            this.checkboxShowInitImg.CheckedChanged += new System.EventHandler(this.checkboxShowInitImg_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(0, 4);
            this.label2.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 83;
            this.label2.Text = "Base Image";
            // 
            // panelLoras
            // 
            this.panelLoras.Controls.Add(this.tbLoraFilter);
            this.panelLoras.Controls.Add(this.btnExpandLoras);
            this.panelLoras.Controls.Add(this.gridLoras);
            this.panelLoras.Controls.Add(this.label24);
            this.panelLoras.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLoras.Location = new System.Drawing.Point(0, 360);
            this.panelLoras.Name = "panelLoras";
            this.panelLoras.Size = new System.Drawing.Size(651, 105);
            this.panelLoras.TabIndex = 123;
            // 
            // tbLoraFilter
            // 
            this.tbLoraFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLoraFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tbLoraFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbLoraFilter.ForeColor = System.Drawing.Color.White;
            this.tbLoraFilter.Location = new System.Drawing.Point(233, 0);
            this.tbLoraFilter.Margin = new System.Windows.Forms.Padding(0);
            this.tbLoraFilter.Name = "tbLoraFilter";
            this.tbLoraFilter.Size = new System.Drawing.Size(395, 20);
            this.tbLoraFilter.TabIndex = 108;
            // 
            // gridLoras
            // 
            this.gridLoras.AllowUserToAddRows = false;
            this.gridLoras.AllowUserToDeleteRows = false;
            this.gridLoras.AllowUserToResizeColumns = false;
            this.gridLoras.AllowUserToResizeRows = false;
            this.gridLoras.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridLoras.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gridLoras.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridLoras.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridLoras.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridLoras.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridLoras.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColEnabled,
            this.ColName,
            this.ColWeight});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridLoras.DefaultCellStyle = dataGridViewCellStyle2;
            this.gridLoras.Location = new System.Drawing.Point(233, 20);
            this.gridLoras.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.gridLoras.MultiSelect = false;
            this.gridLoras.Name = "gridLoras";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridLoras.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gridLoras.RowHeadersVisible = false;
            this.gridLoras.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridLoras.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gridLoras.Size = new System.Drawing.Size(395, 71);
            this.gridLoras.TabIndex = 106;
            // 
            // ColEnabled
            // 
            this.ColEnabled.HeaderText = "Load";
            this.ColEnabled.Name = "ColEnabled";
            this.ColEnabled.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColEnabled.Width = 45;
            // 
            // ColName
            // 
            this.ColName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColName.HeaderText = "File Name";
            this.ColName.Name = "ColName";
            this.ColName.ReadOnly = true;
            this.ColName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ColWeight
            // 
            this.ColWeight.HeaderText = "Weight";
            this.ColWeight.MaxInputLength = 20;
            this.ColWeight.Name = "ColWeight";
            this.ColWeight.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColWeight.Width = 80;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.ForeColor = System.Drawing.Color.White;
            this.label24.Location = new System.Drawing.Point(2, 4);
            this.label24.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(58, 13);
            this.label24.TabIndex = 105;
            this.label24.Text = "LoRA Files";
            // 
            // panelEmbeddings
            // 
            this.panelEmbeddings.Controls.Add(this.btnEmbeddingAppend);
            this.panelEmbeddings.Controls.Add(this.btnEmbeddingCopy);
            this.panelEmbeddings.Controls.Add(this.comboxEmbeddingList);
            this.panelEmbeddings.Controls.Add(this.label20);
            this.panelEmbeddings.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEmbeddings.Location = new System.Drawing.Point(0, 325);
            this.panelEmbeddings.Name = "panelEmbeddings";
            this.panelEmbeddings.Size = new System.Drawing.Size(651, 35);
            this.panelEmbeddings.TabIndex = 112;
            // 
            // btnEmbeddingCopy
            // 
            this.btnEmbeddingCopy.AutoColor = true;
            this.btnEmbeddingCopy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnEmbeddingCopy.ButtonImage = null;
            this.btnEmbeddingCopy.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnEmbeddingCopy.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnEmbeddingCopy.DrawImage = false;
            this.btnEmbeddingCopy.ForeColor = System.Drawing.Color.White;
            this.btnEmbeddingCopy.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.btnEmbeddingCopy.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnEmbeddingCopy.Location = new System.Drawing.Point(439, -1);
            this.btnEmbeddingCopy.Name = "btnEmbeddingCopy";
            this.btnEmbeddingCopy.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnEmbeddingCopy.Size = new System.Drawing.Size(80, 23);
            this.btnEmbeddingCopy.TabIndex = 111;
            this.btnEmbeddingCopy.TabStop = false;
            this.btnEmbeddingCopy.Text = "Copy";
            this.btnEmbeddingCopy.Visible = false;
            this.btnEmbeddingCopy.Click += new System.EventHandler(this.btnEmbeddingCopy_Click);
            // 
            // comboxEmbeddingList
            // 
            this.comboxEmbeddingList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxEmbeddingList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxEmbeddingList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxEmbeddingList.ForeColor = System.Drawing.Color.White;
            this.comboxEmbeddingList.FormattingEnabled = true;
            this.comboxEmbeddingList.Location = new System.Drawing.Point(233, 0);
            this.comboxEmbeddingList.Name = "comboxEmbeddingList";
            this.comboxEmbeddingList.Size = new System.Drawing.Size(200, 21);
            this.comboxEmbeddingList.TabIndex = 110;
            this.comboxEmbeddingList.SelectedIndexChanged += new System.EventHandler(this.comboxEmbeddingList_SelectedIndexChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(0, 4);
            this.label20.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(149, 13);
            this.label20.TabIndex = 105;
            this.label20.Text = "Textual Inversion Embeddings";
            // 
            // panelPromptNeg
            // 
            this.panelPromptNeg.Controls.Add(this.btnExpandPromptNegField);
            this.panelPromptNeg.Controls.Add(this.textboxPromptNeg);
            this.panelPromptNeg.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPromptNeg.Location = new System.Drawing.Point(0, 280);
            this.panelPromptNeg.Name = "panelPromptNeg";
            this.panelPromptNeg.Padding = new System.Windows.Forms.Padding(3);
            this.panelPromptNeg.Size = new System.Drawing.Size(651, 45);
            this.panelPromptNeg.TabIndex = 16;
            // 
            // panelPrompt
            // 
            this.panelPrompt.Controls.Add(this.textboxPrompt);
            this.panelPrompt.Controls.Add(this.btnExpandPromptField);
            this.panelPrompt.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPrompt.Location = new System.Drawing.Point(0, 210);
            this.panelPrompt.Name = "panelPrompt";
            this.panelPrompt.Padding = new System.Windows.Forms.Padding(3);
            this.panelPrompt.Size = new System.Drawing.Size(651, 70);
            this.panelPrompt.TabIndex = 15;
            // 
            // panelCollapsePrompt
            // 
            this.panelCollapsePrompt.Controls.Add(this.btnCollapsePrompt);
            this.panelCollapsePrompt.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCollapsePrompt.Location = new System.Drawing.Point(0, 175);
            this.panelCollapsePrompt.Name = "panelCollapsePrompt";
            this.panelCollapsePrompt.Size = new System.Drawing.Size(651, 35);
            this.panelCollapsePrompt.TabIndex = 121;
            // 
            // btnCollapsePrompt
            // 
            this.btnCollapsePrompt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCollapsePrompt.AutoColor = true;
            this.btnCollapsePrompt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnCollapsePrompt.ButtonImage = null;
            this.btnCollapsePrompt.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnCollapsePrompt.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
            this.btnCollapsePrompt.DrawImage = false;
            this.btnCollapsePrompt.ForeColor = System.Drawing.Color.White;
            this.btnCollapsePrompt.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.btnCollapsePrompt.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnCollapsePrompt.Location = new System.Drawing.Point(0, 0);
            this.btnCollapsePrompt.Name = "btnCollapsePrompt";
            this.btnCollapsePrompt.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.btnCollapsePrompt.Size = new System.Drawing.Size(649, 22);
            this.btnCollapsePrompt.TabIndex = 116;
            this.btnCollapsePrompt.TabStop = false;
            // 
            // panelModel2
            // 
            this.panelModel2.Controls.Add(this.comboxModel2);
            this.panelModel2.Controls.Add(this.label26);
            this.panelModel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelModel2.Location = new System.Drawing.Point(0, 140);
            this.panelModel2.Name = "panelModel2";
            this.panelModel2.Size = new System.Drawing.Size(651, 35);
            this.panelModel2.TabIndex = 125;
            // 
            // comboxModel2
            // 
            this.comboxModel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboxModel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxModel2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxModel2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxModel2.ForeColor = System.Drawing.Color.White;
            this.comboxModel2.Location = new System.Drawing.Point(233, 0);
            this.comboxModel2.Name = "comboxModel2";
            this.comboxModel2.Size = new System.Drawing.Size(415, 21);
            this.comboxModel2.TabIndex = 110;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.ForeColor = System.Drawing.Color.White;
            this.label26.Location = new System.Drawing.Point(0, 4);
            this.label26.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(73, 13);
            this.label26.TabIndex = 105;
            this.label26.Text = "Refiner Model";
            // 
            // panelModelSettings
            // 
            this.panelModelSettings.Controls.Add(this.label29);
            this.panelModelSettings.Controls.Add(this.comboxModelArch);
            this.panelModelSettings.Controls.Add(this.label18);
            this.panelModelSettings.Controls.Add(this.comboxClipSkip);
            this.panelModelSettings.Controls.Add(this.label15);
            this.panelModelSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelModelSettings.Location = new System.Drawing.Point(0, 105);
            this.panelModelSettings.Name = "panelModelSettings";
            this.panelModelSettings.Size = new System.Drawing.Size(651, 35);
            this.panelModelSettings.TabIndex = 127;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.ForeColor = System.Drawing.Color.White;
            this.label29.Location = new System.Drawing.Point(501, 4);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(91, 13);
            this.label29.TabIndex = 113;
            this.label29.Text = "Skip CLIP Layers:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.White;
            this.label18.Location = new System.Drawing.Point(230, 4);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(34, 13);
            this.label18.TabIndex = 112;
            this.label18.Text = "Type:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(0, 4);
            this.label15.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(77, 13);
            this.label15.TabIndex = 105;
            this.label15.Text = "Model Settings";
            // 
            // panelModel
            // 
            this.panelModel.Controls.Add(this.comboxModel);
            this.panelModel.Controls.Add(this.label23);
            this.panelModel.Controls.Add(this.comboxVae);
            this.panelModel.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelModel.Location = new System.Drawing.Point(0, 70);
            this.panelModel.Name = "panelModel";
            this.panelModel.Size = new System.Drawing.Size(651, 35);
            this.panelModel.TabIndex = 115;
            // 
            // comboxModel
            // 
            this.comboxModel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxModel.ForeColor = System.Drawing.Color.White;
            this.comboxModel.Location = new System.Drawing.Point(233, 0);
            this.comboxModel.Name = "comboxModel";
            this.comboxModel.Size = new System.Drawing.Size(262, 21);
            this.comboxModel.TabIndex = 110;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.White;
            this.label23.Location = new System.Drawing.Point(0, 4);
            this.label23.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(81, 13);
            this.label23.TabIndex = 105;
            this.label23.Text = "Model and VAE";
            // 
            // comboxVae
            // 
            this.comboxVae.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboxVae.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxVae.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxVae.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxVae.ForeColor = System.Drawing.Color.White;
            this.comboxVae.Location = new System.Drawing.Point(501, 1);
            this.comboxVae.Name = "comboxVae";
            this.comboxVae.Size = new System.Drawing.Size(148, 21);
            this.comboxVae.TabIndex = 110;
            // 
            // panelBackend
            // 
            this.panelBackend.Controls.Add(this.comboxBackend);
            this.panelBackend.Controls.Add(this.label22);
            this.panelBackend.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBackend.Location = new System.Drawing.Point(0, 35);
            this.panelBackend.Name = "panelBackend";
            this.panelBackend.Size = new System.Drawing.Size(651, 35);
            this.panelBackend.TabIndex = 114;
            // 
            // comboxBackend
            // 
            this.comboxBackend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboxBackend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxBackend.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxBackend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxBackend.ForeColor = System.Drawing.Color.White;
            this.comboxBackend.Location = new System.Drawing.Point(233, 0);
            this.comboxBackend.Name = "comboxBackend";
            this.comboxBackend.Size = new System.Drawing.Size(415, 21);
            this.comboxBackend.TabIndex = 110;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.White;
            this.label22.Location = new System.Drawing.Point(0, 4);
            this.label22.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(91, 13);
            this.label22.TabIndex = 105;
            this.label22.Text = "AI Implementation";
            // 
            // panelCollapseImplementation
            // 
            this.panelCollapseImplementation.Controls.Add(this.btnCollapseImplementation);
            this.panelCollapseImplementation.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCollapseImplementation.Location = new System.Drawing.Point(0, 0);
            this.panelCollapseImplementation.Name = "panelCollapseImplementation";
            this.panelCollapseImplementation.Size = new System.Drawing.Size(651, 35);
            this.panelCollapseImplementation.TabIndex = 122;
            // 
            // btnCollapseImplementation
            // 
            this.btnCollapseImplementation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCollapseImplementation.AutoColor = true;
            this.btnCollapseImplementation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnCollapseImplementation.ButtonImage = null;
            this.btnCollapseImplementation.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnCollapseImplementation.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
            this.btnCollapseImplementation.DrawImage = false;
            this.btnCollapseImplementation.ForeColor = System.Drawing.Color.White;
            this.btnCollapseImplementation.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(72)))), ((int)(((byte)(72)))));
            this.btnCollapseImplementation.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnCollapseImplementation.Location = new System.Drawing.Point(0, 0);
            this.btnCollapseImplementation.Name = "btnCollapseImplementation";
            this.btnCollapseImplementation.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.btnCollapseImplementation.Size = new System.Drawing.Size(649, 22);
            this.btnCollapseImplementation.TabIndex = 113;
            this.btnCollapseImplementation.TabStop = false;
            // 
            // tableLayoutPanelImgViewers
            // 
            this.tableLayoutPanelImgViewers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelImgViewers.ColumnCount = 2;
            this.tableLayoutPanelImgViewers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0F));
            this.tableLayoutPanelImgViewers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelImgViewers.Controls.Add(this.panelImgViewerParent, 1, 0);
            this.tableLayoutPanelImgViewers.Controls.Add(this.pictBoxInitImg, 0, 0);
            this.tableLayoutPanelImgViewers.Location = new System.Drawing.Point(680, 93);
            this.tableLayoutPanelImgViewers.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.tableLayoutPanelImgViewers.Name = "tableLayoutPanelImgViewers";
            this.tableLayoutPanelImgViewers.RowCount = 1;
            this.tableLayoutPanelImgViewers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelImgViewers.Size = new System.Drawing.Size(512, 512);
            this.tableLayoutPanelImgViewers.TabIndex = 119;
            // 
            // panelImgViewerParent
            // 
            this.panelImgViewerParent.Controls.Add(this.pictBoxPreview);
            this.panelImgViewerParent.Controls.Add(this.pictBoxImgViewer);
            this.panelImgViewerParent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImgViewerParent.Location = new System.Drawing.Point(0, 0);
            this.panelImgViewerParent.Margin = new System.Windows.Forms.Padding(0);
            this.panelImgViewerParent.Name = "panelImgViewerParent";
            this.panelImgViewerParent.Size = new System.Drawing.Size(512, 512);
            this.panelImgViewerParent.TabIndex = 0;
            // 
            // pictBoxPreview
            // 
            this.pictBoxPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictBoxPreview.BackgroundImage = global::StableDiffusionGui.Properties.Resources.checkerboard_darkened;
            this.pictBoxPreview.Location = new System.Drawing.Point(0, 0);
            this.pictBoxPreview.Margin = new System.Windows.Forms.Padding(0);
            this.pictBoxPreview.Name = "pictBoxPreview";
            this.pictBoxPreview.Size = new System.Drawing.Size(512, 512);
            this.pictBoxPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictBoxPreview.TabIndex = 114;
            this.pictBoxPreview.TabStop = false;
            this.pictBoxPreview.Visible = false;
            // 
            // pictBoxImgViewer
            // 
            this.pictBoxImgViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictBoxImgViewer.BackgroundImage = global::StableDiffusionGui.Properties.Resources.checkerboard_darkened;
            this.pictBoxImgViewer.Location = new System.Drawing.Point(0, 0);
            this.pictBoxImgViewer.Margin = new System.Windows.Forms.Padding(0);
            this.pictBoxImgViewer.Name = "pictBoxImgViewer";
            this.pictBoxImgViewer.Size = new System.Drawing.Size(512, 512);
            this.pictBoxImgViewer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictBoxImgViewer.TabIndex = 113;
            this.pictBoxImgViewer.TabStop = false;
            this.pictBoxImgViewer.Click += new System.EventHandler(this.pictBoxImgViewer_Click);
            // 
            // pictBoxInitImg
            // 
            this.pictBoxInitImg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictBoxInitImg.BackgroundImage = global::StableDiffusionGui.Properties.Resources.checkerboard_darkened;
            this.pictBoxInitImg.Location = new System.Drawing.Point(0, 0);
            this.pictBoxInitImg.Margin = new System.Windows.Forms.Padding(0);
            this.pictBoxInitImg.Name = "pictBoxInitImg";
            this.pictBoxInitImg.Size = new System.Drawing.Size(1, 512);
            this.pictBoxInitImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictBoxInitImg.TabIndex = 118;
            this.pictBoxInitImg.TabStop = false;
            // 
            // menuStripInstall
            // 
            this.menuStripInstall.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manageInstallationToolStripMenuItem,
            this.installUpdatesToolStripMenuItem});
            this.menuStripInstall.Name = "menuStripInstall";
            this.menuStripInstall.ShowImageMargin = false;
            this.menuStripInstall.Size = new System.Drawing.Size(154, 48);
            // 
            // manageInstallationToolStripMenuItem
            // 
            this.manageInstallationToolStripMenuItem.Name = "manageInstallationToolStripMenuItem";
            this.manageInstallationToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.manageInstallationToolStripMenuItem.Text = "Manage Installation";
            this.manageInstallationToolStripMenuItem.Click += new System.EventHandler(this.manageInstallationToolStripMenuItem_Click);
            // 
            // installUpdatesToolStripMenuItem
            // 
            this.installUpdatesToolStripMenuItem.Name = "installUpdatesToolStripMenuItem";
            this.installUpdatesToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.installUpdatesToolStripMenuItem.Text = "Install Updates";
            this.installUpdatesToolStripMenuItem.Click += new System.EventHandler(this.installUpdatesToolStripMenuItem_Click);
            // 
            // menuStripOpenFolder
            // 
            this.menuStripOpenFolder.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openOutputFolderToolStripMenuItem1,
            this.openFavoritesFolderToolStripMenuItem});
            this.menuStripOpenFolder.Name = "menuStripOpenFolder";
            this.menuStripOpenFolder.ShowImageMargin = false;
            this.menuStripOpenFolder.Size = new System.Drawing.Size(165, 48);
            // 
            // openOutputFolderToolStripMenuItem1
            // 
            this.openOutputFolderToolStripMenuItem1.Name = "openOutputFolderToolStripMenuItem1";
            this.openOutputFolderToolStripMenuItem1.Size = new System.Drawing.Size(164, 22);
            this.openOutputFolderToolStripMenuItem1.Text = "Open Output Folder";
            this.openOutputFolderToolStripMenuItem1.Click += new System.EventHandler(this.openOutputFolderToolStripMenuItem1_Click);
            // 
            // openFavoritesFolderToolStripMenuItem
            // 
            this.openFavoritesFolderToolStripMenuItem.Name = "openFavoritesFolderToolStripMenuItem";
            this.openFavoritesFolderToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.openFavoritesFolderToolStripMenuItem.Text = "Open Favorites Folder";
            this.openFavoritesFolderToolStripMenuItem.Click += new System.EventHandler(this.openFavoritesFolderToolStripMenuItem_Click);
            // 
            // flowPanelImgButtons
            // 
            this.flowPanelImgButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.flowPanelImgButtons.Controls.Add(this.btnNextImg);
            this.flowPanelImgButtons.Controls.Add(this.btnPrevImg);
            this.flowPanelImgButtons.Controls.Add(this.btnDeleteBatch);
            this.flowPanelImgButtons.Controls.Add(this.btnOpenOutFolder);
            this.flowPanelImgButtons.Controls.Add(this.btnSaveToFavs);
            this.flowPanelImgButtons.Controls.Add(this.btnSaveMode);
            this.flowPanelImgButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowPanelImgButtons.Location = new System.Drawing.Point(898, 629);
            this.flowPanelImgButtons.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.flowPanelImgButtons.Name = "flowPanelImgButtons";
            this.flowPanelImgButtons.Size = new System.Drawing.Size(294, 40);
            this.flowPanelImgButtons.TabIndex = 122;
            // 
            // btnSaveMode
            // 
            this.btnSaveMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnSaveMode.BackgroundImage = global::StableDiffusionGui.Properties.Resources.IconArchiveOn;
            this.btnSaveMode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSaveMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnSaveMode.Location = new System.Drawing.Point(21, 0);
            this.btnSaveMode.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.btnSaveMode.Name = "btnSaveMode";
            this.btnSaveMode.Size = new System.Drawing.Size(40, 40);
            this.btnSaveMode.TabIndex = 121;
            this.btnSaveMode.TabStop = false;
            this.btnSaveMode.UseVisualStyleBackColor = false;
            this.btnSaveMode.Click += new System.EventHandler(this.btnSaveMode_Click);
            // 
            // separator
            // 
            this.separator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.separator.BackColor = System.Drawing.Color.Transparent;
            this.separator.BackgroundImage = global::StableDiffusionGui.Properties.Resources.separatorTest1;
            this.separator.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.separator.Enabled = false;
            this.separator.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.separator.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.separator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.separator.Location = new System.Drawing.Point(876, 9);
            this.separator.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.separator.Name = "separator";
            this.separator.Size = new System.Drawing.Size(40, 40);
            this.separator.TabIndex = 75;
            this.separator.TabStop = false;
            this.separator.UseVisualStyleBackColor = false;
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(1198, 700);
            this.Controls.Add(this.flowPanelImgButtons);
            this.Controls.Add(this.progressBarImg);
            this.Controls.Add(this.tableLayoutPanelImgViewers);
            this.Controls.Add(this.btnTrain);
            this.Controls.Add(this.labelImgPromptNeg);
            this.Controls.Add(this.labelImgPrompt);
            this.Controls.Add(this.btnPromptHistory);
            this.Controls.Add(this.btnQueue);
            this.Controls.Add(this.btnPostProc);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnDebug);
            this.Controls.Add(this.panelSettings);
            this.Controls.Add(this.cliButton);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.progressCircle);
            this.Controls.Add(this.labelImgInfo);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.installerBtn);
            this.Controls.Add(this.separator);
            this.Controls.Add(this.discordBtn);
            this.Controls.Add(this.patreonBtn);
            this.Controls.Add(this.paypalBtn);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.runBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
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
            ((System.ComponentModel.ISupportInitialize)(this.updownUpscaleFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updownUpscaleResultW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updownUpscaleResultH)).EndInit();
            this.menuStripLogs.ResumeLayout(false);
            this.menuStripRunQueue.ResumeLayout(false);
            this.menuStripAddToQueue.ResumeLayout(false);
            this.menuStripDeleteImages.ResumeLayout(false);
            this.menuStripDevTools.ResumeLayout(false);
            this.menuStripPostProcess.ResumeLayout(false);
            this.panelSettings.ResumeLayout(false);
            this.panelDebugLoopback.ResumeLayout(false);
            this.panelDebugLoopback.PerformLayout();
            this.panelCollapseDebug.ResumeLayout(false);
            this.panelSeamless.ResumeLayout(false);
            this.panelSeamless.PerformLayout();
            this.panelCollapseSymmetry.ResumeLayout(false);
            this.panelSampler.ResumeLayout(false);
            this.panelSampler.PerformLayout();
            this.panelUpscaling.ResumeLayout(false);
            this.panelUpscaling.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panelRes.ResumeLayout(false);
            this.panelRes.PerformLayout();
            this.panelCollapseRendering.ResumeLayout(false);
            this.panelSeed.ResumeLayout(false);
            this.panelSeed.PerformLayout();
            this.panelGuidance.ResumeLayout(false);
            this.panelGuidance.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.panelScaleImg.ResumeLayout(false);
            this.panelScaleImg.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panelScale.ResumeLayout(false);
            this.panelScale.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panelRefineStart.ResumeLayout(false);
            this.panelRefineStart.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.panelSteps.ResumeLayout(false);
            this.panelSteps.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.panelIterations.ResumeLayout(false);
            this.panelIterations.PerformLayout();
            this.panelControlnet.ResumeLayout(false);
            this.panelControlnet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updownControlnetStrength)).EndInit();
            this.panelInitImgStrength.ResumeLayout(false);
            this.panelInitImgStrength.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.panelInpainting.ResumeLayout(false);
            this.panelInpainting.PerformLayout();
            this.panelResizeGravity.ResumeLayout(false);
            this.panelResizeGravity.PerformLayout();
            this.panelCollapseGeneration.ResumeLayout(false);
            this.panelBaseImg.ResumeLayout(false);
            this.panelBaseImg.PerformLayout();
            this.panelLoras.ResumeLayout(false);
            this.panelLoras.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridLoras)).EndInit();
            this.panelEmbeddings.ResumeLayout(false);
            this.panelEmbeddings.PerformLayout();
            this.panelPromptNeg.ResumeLayout(false);
            this.panelPromptNeg.PerformLayout();
            this.panelPrompt.ResumeLayout(false);
            this.panelPrompt.PerformLayout();
            this.panelCollapsePrompt.ResumeLayout(false);
            this.panelModel2.ResumeLayout(false);
            this.panelModel2.PerformLayout();
            this.panelModelSettings.ResumeLayout(false);
            this.panelModelSettings.PerformLayout();
            this.panelModel.ResumeLayout(false);
            this.panelModel.PerformLayout();
            this.panelBackend.ResumeLayout(false);
            this.panelBackend.PerformLayout();
            this.panelCollapseImplementation.ResumeLayout(false);
            this.tableLayoutPanelImgViewers.ResumeLayout(false);
            this.panelImgViewerParent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxImgViewer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxInitImg)).EndInit();
            this.menuStripInstall.ResumeLayout(false);
            this.menuStripOpenFolder.ResumeLayout(false);
            this.flowPanelImgButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button discordBtn;
        private System.Windows.Forms.Button patreonBtn;
        private System.Windows.Forms.Button paypalBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button separator;
        public System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem openOutputFolderToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem copyImageToClipboardToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem copySeedToClipboardToolStripMenuItem;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
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
        private System.Windows.Forms.TextBox textboxSliderInitStrength;
        public System.Windows.Forms.ToolStripMenuItem fitWindowSizeToImageSizeToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem openCmdInPythonEnvironmentToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem copyToFavoritesToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem upscaleToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem faceRestorationToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem applyAllToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem postProcessImageToolStripMenuItem;
        public System.Windows.Forms.ComboBox comboxSampler;
        public CustomTextbox textboxPrompt;
        public CustomUpDown upDownIterations;
        public CustomSlider sliderScale;
        public CustomUpDown upDownSeed;
        public HTAlt.WinForms.HTButton btnInitImgBrowse;
        public CustomSlider sliderInitStrength;
        public HTAlt.WinForms.HTButton btnSeedUsePrevious;
        public HTAlt.WinForms.HTButton btnSeedResetToRandom;
        public System.Windows.Forms.Button btnExpandPromptField;
        public CustomSlider sliderSteps;
        public System.Windows.Forms.CheckBox checkboxLockSeed;
        public System.Windows.Forms.Label labelImgPrompt;
        public CustomTextbox textboxPromptNeg;
        public System.Windows.Forms.Button btnExpandPromptNegField;
        public System.Windows.Forms.Label labelImgPromptNeg;
        public System.Windows.Forms.TextBox textboxClipsegMask;
        public System.Windows.Forms.ComboBox comboxInpaintMode;
        public System.Windows.Forms.ComboBox comboxResH;
        public System.Windows.Forms.ComboBox comboxResW;
        public System.Windows.Forms.CheckBox checkboxHiresFix;
        public System.Windows.Forms.ComboBox comboxSeamless;
        public System.Windows.Forms.Button runBtn;
        public System.Windows.Forms.Button installerBtn;
        public CustomTextbox logBox;
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
        public System.Windows.Forms.Button btnTrain;
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
        public System.Windows.Forms.Panel panelSampler;
        public System.Windows.Forms.Panel panelRes;
        public System.Windows.Forms.Panel panelSeed;
        public System.Windows.Forms.Panel panelScale;
        public System.Windows.Forms.Panel panelSeamless;
        public System.Windows.Forms.Panel panelInitImgStrength;
        public System.Windows.Forms.Panel panelInpainting;
        public System.Windows.Forms.Panel panelBaseImg;
        public System.Windows.Forms.Panel panelPromptNeg;
        public System.Windows.Forms.Panel panelPrompt;
        public System.Windows.Forms.Label labelCurrentImage;
        public HTAlt.WinForms.HTProgressBar progressBar;
        public CircularProgressBar.CircularProgressBar progressCircle;
        public CustomTextbox textboxExtraScales;
        public CustomTextbox textboxExtraInitStrengths;
        public CustomTextbox textboxExtraSteps;
        public System.Windows.Forms.Panel panelDebugLoopback;
        public System.Windows.Forms.CheckBox checkboxLoopback;
        private System.Windows.Forms.Label label16;
        private HTAlt.WinForms.HTButton btnEditMask;
        private System.Windows.Forms.ToolStripMenuItem convertModelsToolStripMenuItem;
        public System.Windows.Forms.Panel panelScaleImg;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox textboxSliderScaleImg;
        public CustomSlider sliderScaleImg;
        public CustomTextbox textboxExtraScalesImg;
        public System.Windows.Forms.PictureBox pictBoxInitImg;
        private System.Windows.Forms.Panel panelImgViewerParent;
        public System.Windows.Forms.CheckBox checkboxShowInitImg;
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanelImgViewers;
        private System.Windows.Forms.Label labelAspectRatio;
        private System.Windows.Forms.ToolStripMenuItem copySidebySideComparisonImageToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip menuStripInstall;
        private System.Windows.Forms.ToolStripMenuItem manageInstallationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem installUpdatesToolStripMenuItem;
        public System.Windows.Forms.ComboBox comboxResizeGravity;
        public HTAlt.WinForms.HTButton btnCollapseDebug;
        public HTAlt.WinForms.HTButton btnCollapseSymmetry;
        public HTAlt.WinForms.HTButton btnCollapseRendering;
        public HTAlt.WinForms.HTButton btnCollapseGeneration;
        public System.Windows.Forms.ComboBox comboxSymmetry;
        public System.Windows.Forms.Panel panelEmbeddings;
        private System.Windows.Forms.Label label20;
        public System.Windows.Forms.ComboBox comboxEmbeddingList;
        public HTAlt.WinForms.HTButton btnEmbeddingAppend;
        public HTAlt.WinForms.HTButton btnEmbeddingCopy;
        private System.Windows.Forms.Label labelResChange;
        public HTAlt.WinForms.HTButton btnResetRes;
        private System.Windows.Forms.Panel panelResizeGravity;
        private System.Windows.Forms.Label label21;
        public System.Windows.Forms.ComboBox comboxBackend;
        private System.Windows.Forms.Label label22;
        public HTAlt.WinForms.HTButton btnCollapseImplementation;
        public System.Windows.Forms.Panel panelModel;
        public System.Windows.Forms.ComboBox comboxModel;
        private System.Windows.Forms.Label label23;
        public HTAlt.WinForms.HTButton btnCollapsePrompt;
        public System.Windows.Forms.Panel panelCollapseDebug;
        public System.Windows.Forms.Panel panelCollapseSymmetry;
        public System.Windows.Forms.Panel panelCollapseRendering;
        public System.Windows.Forms.Panel panelCollapseGeneration;
        public System.Windows.Forms.Panel panelCollapsePrompt;
        public System.Windows.Forms.Panel panelCollapseImplementation;
        private System.Windows.Forms.ToolStripMenuItem downloadHuggingfaceModelToolStripMenuItem;
        public System.Windows.Forms.ComboBox comboxModelArch;
        public System.Windows.Forms.Panel panelLoras;
        private System.Windows.Forms.DataGridView gridLoras;
        private System.Windows.Forms.Label label24;
        public System.Windows.Forms.Button btnExpandLoras;
        private System.Windows.Forms.ContextMenuStrip menuStripOpenFolder;
        private System.Windows.Forms.ToolStripMenuItem openOutputFolderToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openFavoritesFolderToolStripMenuItem;
        public System.Windows.Forms.Button btnSaveToFavs;
        public System.Windows.Forms.Button btnSaveMode;
        private System.Windows.Forms.FlowLayoutPanel flowPanelImgButtons;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColWeight;
        public System.Windows.Forms.Panel panelBackend;
        public System.Windows.Forms.Panel panelRefineStart;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        public CustomTextbox textboxExtraRefinerValues;
        private System.Windows.Forms.TextBox textboxSliderRefineStart;
        public CustomSlider sliderRefinerStart;
        private System.Windows.Forms.ToolStripMenuItem downloadStableDiffusionModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sD15ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sDXL10ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sDXL10RefinerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sD15ONNXToolStripMenuItem;
        public System.Windows.Forms.Panel panelModel2;
        public System.Windows.Forms.ComboBox comboxModel2;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label labelUpscaleX;
        public System.Windows.Forms.ComboBox comboxUpscaleMode;
        private System.Windows.Forms.TableLayoutPanel panelUpscaling;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label labelUpscale;
        public CustomUpDown updownUpscaleFactor;
        private System.Windows.Forms.Label labelUpscaleEquals;
        public CustomUpDown updownUpscaleResultW;
        public CustomUpDown updownUpscaleResultH;
        public System.Windows.Forms.CheckBox checkboxPreview;
        public System.Windows.Forms.PictureBox pictBoxPreview;
        public System.Windows.Forms.Panel panelControlnet;
        public CustomUpDown updownControlnetStrength;
        public System.Windows.Forms.ComboBox comboxControlnet;
        private System.Windows.Forms.Label label28;
        public System.Windows.Forms.ComboBox comboxPreprocessor;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        public System.Windows.Forms.ComboBox comboxControlnetSlot;
        public System.Windows.Forms.Panel panelModelSettings;
        public System.Windows.Forms.ComboBox comboxClipSkip;
        public System.Windows.Forms.ComboBox comboxVae;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label18;
        public System.Windows.Forms.Panel panelGuidance;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TextBox textboxSliderGuidance;
        public CustomSlider sliderGuidance;
        public CustomTextbox textboxExtraGuidances;
        private System.Windows.Forms.TextBox tbLoraFilter;
    }
}

