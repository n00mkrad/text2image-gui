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
            this.label2 = new System.Windows.Forms.Label();
            this.textboxPrompt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.upDownIterations = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.textboxSliderSteps = new System.Windows.Forms.TextBox();
            this.sliderSteps = new StableDiffusionGui.Controls.CustomSlider();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textboxSliderScale = new System.Windows.Forms.TextBox();
            this.sliderScale = new StableDiffusionGui.Controls.CustomSlider();
            this.label5 = new System.Windows.Forms.Label();
            this.upDownSeed = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.progressCircle = new CircularProgressBar.CircularProgressBar();
            this.progressBar = new HTAlt.WinForms.HTProgressBar();
            this.textboxExtraScales = new System.Windows.Forms.TextBox();
            this.menuStripOutputImg = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openOutputFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToFavoritesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyImageToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copySeedToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reGenerateImageWithCurrentSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useAsInitImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.postProcessImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fitWindowSizeToImageSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.comboxSampler = new System.Windows.Forms.ComboBox();
            this.textboxExtraInitStrengths = new System.Windows.Forms.TextBox();
            this.checkboxSeamless = new System.Windows.Forms.CheckBox();
            this.btnEmbeddingBrowse = new HTAlt.WinForms.HTButton();
            this.btnInitImgBrowse = new HTAlt.WinForms.HTButton();
            this.btnSeedResetToRandom = new HTAlt.WinForms.HTButton();
            this.btnSeedUsePrevious = new HTAlt.WinForms.HTButton();
            this.btnResetMask = new HTAlt.WinForms.HTButton();
            this.checkboxInpainting = new System.Windows.Forms.CheckBox();
            this.btnDeleteBatch = new System.Windows.Forms.Button();
            this.btnPromptHistory = new System.Windows.Forms.Button();
            this.btnQueue = new System.Windows.Forms.Button();
            this.btnPostProc = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnDebug = new System.Windows.Forms.Button();
            this.cliButton = new System.Windows.Forms.Button();
            this.btnExpandPromptField = new System.Windows.Forms.Button();
            this.btnOpenOutFolder = new System.Windows.Forms.Button();
            this.btnPrevImg = new System.Windows.Forms.Button();
            this.btnNextImg = new System.Windows.Forms.Button();
            this.installerBtn = new System.Windows.Forms.Button();
            this.discordBtn = new System.Windows.Forms.Button();
            this.patreonBtn = new System.Windows.Forms.Button();
            this.paypalBtn = new System.Windows.Forms.Button();
            this.checkboxHiresFix = new System.Windows.Forms.CheckBox();
            this.checkboxLockSeed = new System.Windows.Forms.CheckBox();
            this.labelImgPrompt = new System.Windows.Forms.Label();
            this.sliderInitStrength = new StableDiffusionGui.Controls.CustomSlider();
            this.textboxSliderInitStrength = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelDebugSendStdin = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.textboxCliTest = new System.Windows.Forms.TextBox();
            this.panelSeamless = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.panelSampler = new System.Windows.Forms.Panel();
            this.panelRes = new System.Windows.Forms.Panel();
            this.comboxResH = new System.Windows.Forms.ComboBox();
            this.comboxResW = new System.Windows.Forms.ComboBox();
            this.panelSeed = new System.Windows.Forms.Panel();
            this.panelScale = new System.Windows.Forms.Panel();
            this.panelSteps = new System.Windows.Forms.Panel();
            this.panelIterations = new System.Windows.Forms.Panel();
            this.panelInpainting = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.panelInitImgStrength = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.panelPrompt = new System.Windows.Forms.Panel();
            this.labelCurrentConcept = new System.Windows.Forms.Label();
            this.labelCurrentImage = new System.Windows.Forms.Label();
            this.menuStripLogs = new System.Windows.Forms.ContextMenuStrip(this.components);
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
            this.openCmdInCondaEnvironmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openModelMergeToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openModelPruningTrimmingToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewLogInRealtimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trainDreamBoothModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictBoxImgViewer = new System.Windows.Forms.PictureBox();
            this.separator = new System.Windows.Forms.Button();
            this.menuStripPostProcess = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.upscaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.faceRestorationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applyAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.upDownIterations)).BeginInit();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upDownSeed)).BeginInit();
            this.menuStripOutputImg.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelDebugSendStdin.SuspendLayout();
            this.panelSeamless.SuspendLayout();
            this.panelSampler.SuspendLayout();
            this.panelRes.SuspendLayout();
            this.panelSeed.SuspendLayout();
            this.panelScale.SuspendLayout();
            this.panelSteps.SuspendLayout();
            this.panelIterations.SuspendLayout();
            this.panelInpainting.SuspendLayout();
            this.panelInitImgStrength.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.panelPrompt.SuspendLayout();
            this.menuStripRunQueue.SuspendLayout();
            this.menuStripAddToQueue.SuspendLayout();
            this.menuStripDeleteImages.SuspendLayout();
            this.menuStripDevTools.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxImgViewer)).BeginInit();
            this.menuStripPostProcess.SuspendLayout();
            this.SuspendLayout();
            // 
            // runBtn
            // 
            this.runBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.runBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.runBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.runBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.runBtn.ForeColor = System.Drawing.Color.White;
            this.runBtn.Location = new System.Drawing.Point(654, 679);
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
            this.titleLabel.Location = new System.Drawing.Point(11, 9);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(367, 40);
            this.titleLabel.TabIndex = 11;
            this.titleLabel.Text = "NMKD Stable Diffusion GUI";
            // 
            // logBox
            // 
            this.logBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.logBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.logBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logBox.ForeColor = System.Drawing.Color.Silver;
            this.logBox.Location = new System.Drawing.Point(12, 661);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logBox.Size = new System.Drawing.Size(633, 82);
            this.logBox.TabIndex = 78;
            this.logBox.TabStop = false;
            // 
            // labelImgInfo
            // 
            this.labelImgInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelImgInfo.AutoSize = true;
            this.labelImgInfo.ForeColor = System.Drawing.Color.Silver;
            this.labelImgInfo.Location = new System.Drawing.Point(653, 660);
            this.labelImgInfo.Name = "labelImgInfo";
            this.labelImgInfo.Size = new System.Drawing.Size(100, 13);
            this.labelImgInfo.TabIndex = 81;
            this.labelImgInfo.Text = "No images to show.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(5, 10);
            this.label2.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 83;
            this.label2.Text = "Prompt (AI Inputs)";
            // 
            // textboxPrompt
            // 
            this.textboxPrompt.AllowDrop = true;
            this.textboxPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxPrompt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textboxPrompt.ForeColor = System.Drawing.Color.White;
            this.textboxPrompt.Location = new System.Drawing.Point(654, 62);
            this.textboxPrompt.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxPrompt.Multiline = true;
            this.textboxPrompt.Name = "textboxPrompt";
            this.textboxPrompt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textboxPrompt.Size = new System.Drawing.Size(512, 59);
            this.textboxPrompt.TabIndex = 0;
            this.toolTip.SetToolTip(this.textboxPrompt, "Text prompt. The AI will try to generate an image matching this description.");
            this.textboxPrompt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textboxPrompt_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(5, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 13);
            this.label1.TabIndex = 85;
            this.label1.Text = "Amount Of Images To Generate";
            // 
            // upDownIterations
            // 
            this.upDownIterations.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.upDownIterations.ForeColor = System.Drawing.Color.White;
            this.upDownIterations.Location = new System.Drawing.Point(233, 8);
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
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel6.Controls.Add(this.textboxSliderSteps, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.sliderSteps, 0, 0);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(233, 6);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(400, 21);
            this.tableLayoutPanel6.TabIndex = 88;
            // 
            // textboxSliderSteps
            // 
            this.textboxSliderSteps.AllowDrop = true;
            this.textboxSliderSteps.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.textboxSliderSteps.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textboxSliderSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textboxSliderSteps.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textboxSliderSteps.ForeColor = System.Drawing.Color.Silver;
            this.textboxSliderSteps.Location = new System.Drawing.Point(360, 1);
            this.textboxSliderSteps.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.textboxSliderSteps.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxSliderSteps.Name = "textboxSliderSteps";
            this.textboxSliderSteps.Size = new System.Drawing.Size(40, 17);
            this.textboxSliderSteps.TabIndex = 92;
            this.textboxSliderSteps.Text = "10";
            // 
            // sliderSteps
            // 
            this.sliderSteps.ActualMaximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.sliderSteps.ActualMinimum = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.sliderSteps.ActualValue = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.sliderSteps.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.sliderSteps.BorderRoundRectSize = new System.Drawing.Size(12, 12);
            this.sliderSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sliderSteps.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.sliderSteps.ForeColor = System.Drawing.Color.Black;
            this.sliderSteps.LargeChange = ((uint)(5u));
            this.sliderSteps.Location = new System.Drawing.Point(0, 0);
            this.sliderSteps.Margin = new System.Windows.Forms.Padding(0);
            this.sliderSteps.Maximum = 20;
            this.sliderSteps.Name = "sliderSteps";
            this.sliderSteps.OverlayColor = System.Drawing.Color.White;
            this.sliderSteps.Size = new System.Drawing.Size(360, 21);
            this.sliderSteps.SmallChange = ((uint)(1u));
            this.sliderSteps.TabIndex = 13;
            this.sliderSteps.Text = "sliderSteps";
            this.sliderSteps.ThumbRoundRectSize = new System.Drawing.Size(16, 16);
            this.sliderSteps.ThumbSize = new System.Drawing.Size(14, 14);
            this.sliderSteps.Value = 2;
            this.sliderSteps.ValueBox = this.textboxSliderSteps;
            this.sliderSteps.ValueStep = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(5, 10);
            this.label3.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 89;
            this.label3.Text = "Generation Steps";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(5, 10);
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
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Controls.Add(this.textboxSliderScale, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.sliderScale, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(233, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(291, 21);
            this.tableLayoutPanel1.TabIndex = 91;
            // 
            // textboxSliderScale
            // 
            this.textboxSliderScale.AllowDrop = true;
            this.textboxSliderScale.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.textboxSliderScale.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textboxSliderScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textboxSliderScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textboxSliderScale.ForeColor = System.Drawing.Color.Silver;
            this.textboxSliderScale.Location = new System.Drawing.Point(251, 1);
            this.textboxSliderScale.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.textboxSliderScale.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxSliderScale.Name = "textboxSliderScale";
            this.textboxSliderScale.Size = new System.Drawing.Size(40, 17);
            this.textboxSliderScale.TabIndex = 93;
            this.textboxSliderScale.Text = "0";
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
            this.sliderScale.ActualValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.sliderScale.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.sliderScale.BorderRoundRectSize = new System.Drawing.Size(12, 12);
            this.sliderScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sliderScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.sliderScale.ForeColor = System.Drawing.Color.Black;
            this.sliderScale.LargeChange = ((uint)(5u));
            this.sliderScale.Location = new System.Drawing.Point(0, 0);
            this.sliderScale.Margin = new System.Windows.Forms.Padding(0);
            this.sliderScale.Maximum = 20;
            this.sliderScale.Name = "sliderScale";
            this.sliderScale.OverlayColor = System.Drawing.Color.White;
            this.sliderScale.Size = new System.Drawing.Size(251, 21);
            this.sliderScale.SmallChange = ((uint)(1u));
            this.sliderScale.TabIndex = 4;
            this.sliderScale.ThumbRoundRectSize = new System.Drawing.Size(16, 16);
            this.sliderScale.ThumbSize = new System.Drawing.Size(14, 14);
            this.toolTip.SetToolTip(this.sliderScale, "Higher tries to match your prompt better, but can get chaotic. 7-12 is a safe ran" +
        "ger for most things.");
            this.sliderScale.Value = 0;
            this.sliderScale.ValueBox = this.textboxSliderScale;
            this.sliderScale.ValueStep = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(5, 10);
            this.label5.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(122, 13);
            this.label5.TabIndex = 92;
            this.label5.Text = "Seed (Empty = Random)";
            // 
            // upDownSeed
            // 
            this.upDownSeed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.upDownSeed.ForeColor = System.Drawing.Color.White;
            this.upDownSeed.Location = new System.Drawing.Point(233, 8);
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
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(5, 10);
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
            this.label9.Location = new System.Drawing.Point(342, 10);
            this.label9.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(12, 13);
            this.label9.TabIndex = 98;
            this.label9.Text = "x";
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
            this.progressCircle.Location = new System.Drawing.Point(384, 9);
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
            this.progressBar.Location = new System.Drawing.Point(654, 728);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(512, 15);
            this.progressBar.TabIndex = 100;
            this.progressBar.TabStop = false;
            // 
            // textboxExtraScales
            // 
            this.textboxExtraScales.AllowDrop = true;
            this.textboxExtraScales.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textboxExtraScales.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textboxExtraScales.ForeColor = System.Drawing.Color.White;
            this.textboxExtraScales.Location = new System.Drawing.Point(533, 7);
            this.textboxExtraScales.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxExtraScales.Name = "textboxExtraScales";
            this.textboxExtraScales.Size = new System.Drawing.Size(100, 20);
            this.textboxExtraScales.TabIndex = 3;
            this.toolTip.SetToolTip(this.textboxExtraScales, resources.GetString("textboxExtraScales.ToolTip"));
            // 
            // menuStripOutputImg
            // 
            this.menuStripOutputImg.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileToolStripMenuItem,
            this.openOutputFolderToolStripMenuItem,
            this.copyToFavoritesToolStripMenuItem,
            this.copyImageToClipboardToolStripMenuItem,
            this.copySeedToClipboardToolStripMenuItem,
            this.reGenerateImageWithCurrentSettingsToolStripMenuItem,
            this.useAsInitImageToolStripMenuItem,
            this.postProcessImageToolStripMenuItem,
            this.fitWindowSizeToImageSizeToolStripMenuItem});
            this.menuStripOutputImg.Name = "menuStripOutputImg";
            this.menuStripOutputImg.ShowImageMargin = false;
            this.menuStripOutputImg.Size = new System.Drawing.Size(285, 202);
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
            this.useAsInitImageToolStripMenuItem.Text = "Use as Initialization Image";
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
            // comboxSampler
            // 
            this.comboxSampler.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxSampler.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxSampler.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxSampler.ForeColor = System.Drawing.Color.White;
            this.comboxSampler.FormattingEnabled = true;
            this.comboxSampler.Location = new System.Drawing.Point(233, 7);
            this.comboxSampler.Name = "comboxSampler";
            this.comboxSampler.Size = new System.Drawing.Size(200, 21);
            this.comboxSampler.TabIndex = 105;
            this.toolTip.SetToolTip(this.comboxSampler, "Changes how the image is sampled.\r\nk_euler_a works very well at low step counts.");
            // 
            // textboxExtraInitStrengths
            // 
            this.textboxExtraInitStrengths.AllowDrop = true;
            this.textboxExtraInitStrengths.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textboxExtraInitStrengths.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textboxExtraInitStrengths.ForeColor = System.Drawing.Color.White;
            this.textboxExtraInitStrengths.Location = new System.Drawing.Point(533, 7);
            this.textboxExtraInitStrengths.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxExtraInitStrengths.Name = "textboxExtraInitStrengths";
            this.textboxExtraInitStrengths.Size = new System.Drawing.Size(100, 20);
            this.textboxExtraInitStrengths.TabIndex = 91;
            this.toolTip.SetToolTip(this.textboxExtraInitStrengths, resources.GetString("textboxExtraInitStrengths.ToolTip"));
            // 
            // checkboxSeamless
            // 
            this.checkboxSeamless.AutoSize = true;
            this.checkboxSeamless.ForeColor = System.Drawing.Color.White;
            this.checkboxSeamless.Location = new System.Drawing.Point(233, 10);
            this.checkboxSeamless.Name = "checkboxSeamless";
            this.checkboxSeamless.Size = new System.Drawing.Size(15, 14);
            this.checkboxSeamless.TabIndex = 106;
            this.toolTip.SetToolTip(this.checkboxSeamless, "Create Outputs That Can Be Tiled, for Backgrounds or Textures");
            this.checkboxSeamless.UseVisualStyleBackColor = true;
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
            this.btnEmbeddingBrowse.Location = new System.Drawing.Point(233, 32);
            this.btnEmbeddingBrowse.Name = "btnEmbeddingBrowse";
            this.btnEmbeddingBrowse.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnEmbeddingBrowse.Size = new System.Drawing.Size(100, 23);
            this.btnEmbeddingBrowse.TabIndex = 84;
            this.btnEmbeddingBrowse.TabStop = false;
            this.btnEmbeddingBrowse.Text = "Load Concept";
            this.toolTip.SetToolTip(this.btnEmbeddingBrowse, "Load a concept trained using Textual Inversion");
            this.btnEmbeddingBrowse.Click += new System.EventHandler(this.btnEmbeddingBrowse_Click);
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
            this.btnInitImgBrowse.Location = new System.Drawing.Point(233, 4);
            this.btnInitImgBrowse.Name = "btnInitImgBrowse";
            this.btnInitImgBrowse.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnInitImgBrowse.Size = new System.Drawing.Size(100, 23);
            this.btnInitImgBrowse.TabIndex = 1;
            this.btnInitImgBrowse.TabStop = false;
            this.btnInitImgBrowse.Text = "Load Image";
            this.toolTip.SetToolTip(this.btnInitImgBrowse, "Load initialization image");
            this.btnInitImgBrowse.Click += new System.EventHandler(this.btnInitImgBrowse_Click);
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
            this.btnSeedResetToRandom.Location = new System.Drawing.Point(445, 6);
            this.btnSeedResetToRandom.Name = "btnSeedResetToRandom";
            this.btnSeedResetToRandom.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnSeedResetToRandom.Size = new System.Drawing.Size(79, 23);
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
            this.btnSeedUsePrevious.Location = new System.Drawing.Point(339, 6);
            this.btnSeedUsePrevious.Name = "btnSeedUsePrevious";
            this.btnSeedUsePrevious.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnSeedUsePrevious.Size = new System.Drawing.Size(100, 23);
            this.btnSeedUsePrevious.TabIndex = 106;
            this.btnSeedUsePrevious.TabStop = false;
            this.btnSeedUsePrevious.Text = "Use Previous";
            this.toolTip.SetToolTip(this.btnSeedUsePrevious, "Use Same Seed as Previous Run");
            this.btnSeedUsePrevious.Click += new System.EventHandler(this.btnSeedUsePrevious_Click);
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
            this.btnResetMask.Location = new System.Drawing.Point(254, 6);
            this.btnResetMask.Name = "btnResetMask";
            this.btnResetMask.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnResetMask.Size = new System.Drawing.Size(79, 23);
            this.btnResetMask.TabIndex = 108;
            this.btnResetMask.TabStop = false;
            this.btnResetMask.Text = "Reset Mask";
            this.toolTip.SetToolTip(this.btnResetMask, "Reset Inpainting Mask");
            this.btnResetMask.Visible = false;
            this.btnResetMask.Click += new System.EventHandler(this.btnResetMask_Click);
            // 
            // checkboxInpainting
            // 
            this.checkboxInpainting.AutoSize = true;
            this.checkboxInpainting.ForeColor = System.Drawing.Color.White;
            this.checkboxInpainting.Location = new System.Drawing.Point(233, 9);
            this.checkboxInpainting.Name = "checkboxInpainting";
            this.checkboxInpainting.Size = new System.Drawing.Size(15, 14);
            this.checkboxInpainting.TabIndex = 106;
            this.toolTip.SetToolTip(this.checkboxInpainting, "Enable Mask-based Inpainting");
            this.checkboxInpainting.UseVisualStyleBackColor = true;
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
            this.btnDeleteBatch.Location = new System.Drawing.Point(988, 679);
            this.btnDeleteBatch.Name = "btnDeleteBatch";
            this.btnDeleteBatch.Size = new System.Drawing.Size(40, 40);
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
            this.btnPromptHistory.BackgroundImage = global::StableDiffusionGui.Properties.Resources.historyIcon;
            this.btnPromptHistory.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPromptHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPromptHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPromptHistory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnPromptHistory.Location = new System.Drawing.Point(826, 679);
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
            this.btnQueue.Location = new System.Drawing.Point(780, 679);
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
            this.btnPostProc.Location = new System.Drawing.Point(942, 12);
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
            this.btnSettings.Location = new System.Drawing.Point(1126, 12);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(40, 40);
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
            this.btnDebug.BackgroundImage = global::StableDiffusionGui.Properties.Resources.baseline_list_alt_white_48dp;
            this.btnDebug.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDebug.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDebug.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDebug.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnDebug.Location = new System.Drawing.Point(1080, 12);
            this.btnDebug.Name = "btnDebug";
            this.btnDebug.Size = new System.Drawing.Size(40, 40);
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
            this.cliButton.BackgroundImage = global::StableDiffusionGui.Properties.Resources.cliIcon;
            this.cliButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cliButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cliButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cliButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.cliButton.Location = new System.Drawing.Point(988, 12);
            this.cliButton.Name = "cliButton";
            this.cliButton.Size = new System.Drawing.Size(40, 40);
            this.cliButton.TabIndex = 103;
            this.cliButton.TabStop = false;
            this.toolTip.SetToolTip(this.cliButton, "Developer Tools...");
            this.cliButton.UseVisualStyleBackColor = false;
            this.cliButton.Click += new System.EventHandler(this.cliButton_Click);
            // 
            // btnExpandPromptField
            // 
            this.btnExpandPromptField.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnExpandPromptField.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnExpandPromptField.BackgroundImage = global::StableDiffusionGui.Properties.Resources.downArrowIcon;
            this.btnExpandPromptField.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnExpandPromptField.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpandPromptField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExpandPromptField.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnExpandPromptField.Location = new System.Drawing.Point(613, 0);
            this.btnExpandPromptField.Name = "btnExpandPromptField";
            this.btnExpandPromptField.Size = new System.Drawing.Size(20, 59);
            this.btnExpandPromptField.TabIndex = 86;
            this.btnExpandPromptField.TabStop = false;
            this.toolTip.SetToolTip(this.btnExpandPromptField, "Expand/Collapse Prompt Field");
            this.btnExpandPromptField.UseVisualStyleBackColor = false;
            this.btnExpandPromptField.Click += new System.EventHandler(this.btnExpandPromptField_Click);
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
            this.btnOpenOutFolder.Location = new System.Drawing.Point(1034, 679);
            this.btnOpenOutFolder.Name = "btnOpenOutFolder";
            this.btnOpenOutFolder.Size = new System.Drawing.Size(40, 40);
            this.btnOpenOutFolder.TabIndex = 94;
            this.btnOpenOutFolder.TabStop = false;
            this.toolTip.SetToolTip(this.btnOpenOutFolder, "Open Output Folder");
            this.btnOpenOutFolder.UseVisualStyleBackColor = false;
            this.btnOpenOutFolder.Click += new System.EventHandler(this.btnOpenOutFolder_Click);
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
            this.btnPrevImg.Location = new System.Drawing.Point(1080, 679);
            this.btnPrevImg.Name = "btnPrevImg";
            this.btnPrevImg.Size = new System.Drawing.Size(40, 40);
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
            this.btnNextImg.BackgroundImage = global::StableDiffusionGui.Properties.Resources.forwardArrowIcon;
            this.btnNextImg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnNextImg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextImg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNextImg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnNextImg.Location = new System.Drawing.Point(1126, 679);
            this.btnNextImg.Name = "btnNextImg";
            this.btnNextImg.Size = new System.Drawing.Size(40, 40);
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
            this.installerBtn.BackgroundImage = global::StableDiffusionGui.Properties.Resources.installIcon;
            this.installerBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.installerBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.installerBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.installerBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.installerBtn.Location = new System.Drawing.Point(1034, 12);
            this.installerBtn.Name = "installerBtn";
            this.installerBtn.Size = new System.Drawing.Size(40, 40);
            this.installerBtn.TabIndex = 76;
            this.installerBtn.TabStop = false;
            this.toolTip.SetToolTip(this.installerBtn, "Open Installer");
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
            this.discordBtn.Location = new System.Drawing.Point(850, 12);
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
            this.patreonBtn.Location = new System.Drawing.Point(804, 12);
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
            this.paypalBtn.Location = new System.Drawing.Point(758, 12);
            this.paypalBtn.Name = "paypalBtn";
            this.paypalBtn.Size = new System.Drawing.Size(40, 40);
            this.paypalBtn.TabIndex = 72;
            this.paypalBtn.TabStop = false;
            this.toolTip.SetToolTip(this.paypalBtn, "Donate One-Time via PayPal");
            this.paypalBtn.UseVisualStyleBackColor = false;
            this.paypalBtn.Click += new System.EventHandler(this.paypalBtn_Click);
            // 
            // checkboxHiresFix
            // 
            this.checkboxHiresFix.AutoSize = true;
            this.checkboxHiresFix.ForeColor = System.Drawing.Color.White;
            this.checkboxHiresFix.Location = new System.Drawing.Point(468, 6);
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
            this.checkboxLockSeed.AutoSize = true;
            this.checkboxLockSeed.ForeColor = System.Drawing.Color.White;
            this.checkboxLockSeed.Location = new System.Drawing.Point(533, 6);
            this.checkboxLockSeed.Name = "checkboxLockSeed";
            this.checkboxLockSeed.Padding = new System.Windows.Forms.Padding(3);
            this.checkboxLockSeed.Size = new System.Drawing.Size(84, 23);
            this.checkboxLockSeed.TabIndex = 109;
            this.checkboxLockSeed.Text = "Lock Seed";
            this.toolTip.SetToolTip(this.checkboxLockSeed, resources.GetString("checkboxLockSeed.ToolTip"));
            this.checkboxLockSeed.UseVisualStyleBackColor = true;
            // 
            // labelImgPrompt
            // 
            this.labelImgPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelImgPrompt.AutoEllipsis = true;
            this.labelImgPrompt.ForeColor = System.Drawing.Color.Silver;
            this.labelImgPrompt.Location = new System.Drawing.Point(651, 126);
            this.labelImgPrompt.Name = "labelImgPrompt";
            this.labelImgPrompt.Size = new System.Drawing.Size(515, 16);
            this.labelImgPrompt.TabIndex = 115;
            this.labelImgPrompt.Text = "No prompt to show.";
            this.toolTip.SetToolTip(this.labelImgPrompt, "Shows the prompt of the displayed image. Click to copy.");
            this.labelImgPrompt.Click += new System.EventHandler(this.labelImgPrompt_Click);
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
            this.sliderInitStrength.ActualValue = new decimal(new int[] {
            3,
            0,
            0,
            65536});
            this.sliderInitStrength.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.sliderInitStrength.BorderRoundRectSize = new System.Drawing.Size(12, 12);
            this.sliderInitStrength.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sliderInitStrength.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.sliderInitStrength.ForeColor = System.Drawing.Color.Black;
            this.sliderInitStrength.LargeChange = ((uint)(2u));
            this.sliderInitStrength.Location = new System.Drawing.Point(0, 0);
            this.sliderInitStrength.Margin = new System.Windows.Forms.Padding(0);
            this.sliderInitStrength.Maximum = 18;
            this.sliderInitStrength.Minimum = 2;
            this.sliderInitStrength.Name = "sliderInitStrength";
            this.sliderInitStrength.OverlayColor = System.Drawing.Color.White;
            this.sliderInitStrength.Size = new System.Drawing.Size(246, 21);
            this.sliderInitStrength.SmallChange = ((uint)(1u));
            this.sliderInitStrength.TabIndex = 4;
            this.sliderInitStrength.ThumbRoundRectSize = new System.Drawing.Size(16, 16);
            this.sliderInitStrength.ThumbSize = new System.Drawing.Size(14, 14);
            this.toolTip.SetToolTip(this.sliderInitStrength, "Lower Value: Result Looks More Like Your Text Prompt\r\nHigher Value: Result Looks " +
        "More Like Your Image");
            this.sliderInitStrength.Value = 6;
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
            this.textboxSliderInitStrength.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.textboxSliderInitStrength.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textboxSliderInitStrength.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textboxSliderInitStrength.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textboxSliderInitStrength.ForeColor = System.Drawing.Color.Silver;
            this.textboxSliderInitStrength.Location = new System.Drawing.Point(246, 1);
            this.textboxSliderInitStrength.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.textboxSliderInitStrength.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxSliderInitStrength.Name = "textboxSliderInitStrength";
            this.textboxSliderInitStrength.Size = new System.Drawing.Size(45, 17);
            this.textboxSliderInitStrength.TabIndex = 94;
            this.textboxSliderInitStrength.Text = "0,3";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(5, 10);
            this.label7.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 104;
            this.label7.Text = "Sampler";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.Controls.Add(this.panelDebugSendStdin);
            this.panel1.Controls.Add(this.panelSeamless);
            this.panel1.Controls.Add(this.panelSampler);
            this.panel1.Controls.Add(this.panelRes);
            this.panel1.Controls.Add(this.panelSeed);
            this.panel1.Controls.Add(this.panelScale);
            this.panel1.Controls.Add(this.panelSteps);
            this.panel1.Controls.Add(this.panelIterations);
            this.panel1.Controls.Add(this.panelInpainting);
            this.panel1.Controls.Add(this.panelInitImgStrength);
            this.panel1.Location = new System.Drawing.Point(12, 127);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(633, 528);
            this.panel1.TabIndex = 106;
            this.panel1.Click += new System.EventHandler(this.panel1_Click);
            // 
            // panelDebugSendStdin
            // 
            this.panelDebugSendStdin.Controls.Add(this.label12);
            this.panelDebugSendStdin.Controls.Add(this.textboxCliTest);
            this.panelDebugSendStdin.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDebugSendStdin.Location = new System.Drawing.Point(0, 315);
            this.panelDebugSendStdin.Name = "panelDebugSendStdin";
            this.panelDebugSendStdin.Size = new System.Drawing.Size(633, 35);
            this.panelDebugSendStdin.TabIndex = 14;
            this.panelDebugSendStdin.Visible = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(5, 10);
            this.label12.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(172, 13);
            this.label12.TabIndex = 105;
            this.label12.Text = "Send stdin to running InvokeAI CLI";
            // 
            // textboxCliTest
            // 
            this.textboxCliTest.AllowDrop = true;
            this.textboxCliTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textboxCliTest.ForeColor = System.Drawing.Color.White;
            this.textboxCliTest.Location = new System.Drawing.Point(233, 7);
            this.textboxCliTest.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxCliTest.Name = "textboxCliTest";
            this.textboxCliTest.Size = new System.Drawing.Size(397, 20);
            this.textboxCliTest.TabIndex = 4;
            this.textboxCliTest.DoubleClick += new System.EventHandler(this.textboxCliTest_DoubleClick);
            // 
            // panelSeamless
            // 
            this.panelSeamless.Controls.Add(this.checkboxSeamless);
            this.panelSeamless.Controls.Add(this.label8);
            this.panelSeamless.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSeamless.Location = new System.Drawing.Point(0, 280);
            this.panelSeamless.Name = "panelSeamless";
            this.panelSeamless.Size = new System.Drawing.Size(633, 35);
            this.panelSeamless.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(5, 10);
            this.label8.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(182, 13);
            this.label8.TabIndex = 105;
            this.label8.Text = "Generate Seamless (Tileable) Images";
            // 
            // panelSampler
            // 
            this.panelSampler.Controls.Add(this.label7);
            this.panelSampler.Controls.Add(this.comboxSampler);
            this.panelSampler.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSampler.Location = new System.Drawing.Point(0, 245);
            this.panelSampler.Name = "panelSampler";
            this.panelSampler.Size = new System.Drawing.Size(633, 35);
            this.panelSampler.TabIndex = 6;
            // 
            // panelRes
            // 
            this.panelRes.Controls.Add(this.checkboxHiresFix);
            this.panelRes.Controls.Add(this.comboxResH);
            this.panelRes.Controls.Add(this.comboxResW);
            this.panelRes.Controls.Add(this.label6);
            this.panelRes.Controls.Add(this.label9);
            this.panelRes.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelRes.Location = new System.Drawing.Point(0, 210);
            this.panelRes.Name = "panelRes";
            this.panelRes.Size = new System.Drawing.Size(633, 35);
            this.panelRes.TabIndex = 5;
            // 
            // comboxResH
            // 
            this.comboxResH.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxResH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxResH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxResH.ForeColor = System.Drawing.Color.White;
            this.comboxResH.FormattingEnabled = true;
            this.comboxResH.Location = new System.Drawing.Point(360, 7);
            this.comboxResH.Name = "comboxResH";
            this.comboxResH.Size = new System.Drawing.Size(100, 21);
            this.comboxResH.TabIndex = 107;
            // 
            // comboxResW
            // 
            this.comboxResW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxResW.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxResW.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxResW.ForeColor = System.Drawing.Color.White;
            this.comboxResW.FormattingEnabled = true;
            this.comboxResW.Location = new System.Drawing.Point(233, 7);
            this.comboxResW.Name = "comboxResW";
            this.comboxResW.Size = new System.Drawing.Size(100, 21);
            this.comboxResW.TabIndex = 106;
            // 
            // panelSeed
            // 
            this.panelSeed.Controls.Add(this.checkboxLockSeed);
            this.panelSeed.Controls.Add(this.btnSeedResetToRandom);
            this.panelSeed.Controls.Add(this.btnSeedUsePrevious);
            this.panelSeed.Controls.Add(this.label5);
            this.panelSeed.Controls.Add(this.upDownSeed);
            this.panelSeed.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSeed.Location = new System.Drawing.Point(0, 175);
            this.panelSeed.Name = "panelSeed";
            this.panelSeed.Size = new System.Drawing.Size(633, 35);
            this.panelSeed.TabIndex = 4;
            // 
            // panelScale
            // 
            this.panelScale.Controls.Add(this.label4);
            this.panelScale.Controls.Add(this.tableLayoutPanel1);
            this.panelScale.Controls.Add(this.textboxExtraScales);
            this.panelScale.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelScale.Location = new System.Drawing.Point(0, 140);
            this.panelScale.Name = "panelScale";
            this.panelScale.Size = new System.Drawing.Size(633, 35);
            this.panelScale.TabIndex = 3;
            // 
            // panelSteps
            // 
            this.panelSteps.Controls.Add(this.label3);
            this.panelSteps.Controls.Add(this.tableLayoutPanel6);
            this.panelSteps.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSteps.Location = new System.Drawing.Point(0, 105);
            this.panelSteps.Name = "panelSteps";
            this.panelSteps.Size = new System.Drawing.Size(633, 35);
            this.panelSteps.TabIndex = 2;
            // 
            // panelIterations
            // 
            this.panelIterations.Controls.Add(this.label1);
            this.panelIterations.Controls.Add(this.upDownIterations);
            this.panelIterations.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelIterations.Location = new System.Drawing.Point(0, 70);
            this.panelIterations.Name = "panelIterations";
            this.panelIterations.Size = new System.Drawing.Size(633, 35);
            this.panelIterations.TabIndex = 1;
            // 
            // panelInpainting
            // 
            this.panelInpainting.Controls.Add(this.btnResetMask);
            this.panelInpainting.Controls.Add(this.checkboxInpainting);
            this.panelInpainting.Controls.Add(this.label10);
            this.panelInpainting.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInpainting.Location = new System.Drawing.Point(0, 35);
            this.panelInpainting.Name = "panelInpainting";
            this.panelInpainting.Size = new System.Drawing.Size(633, 35);
            this.panelInpainting.TabIndex = 12;
            this.panelInpainting.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(5, 10);
            this.label10.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(94, 13);
            this.label10.TabIndex = 105;
            this.label10.Text = "Masked Inpainting";
            // 
            // panelInitImgStrength
            // 
            this.panelInitImgStrength.Controls.Add(this.textboxExtraInitStrengths);
            this.panelInitImgStrength.Controls.Add(this.label11);
            this.panelInitImgStrength.Controls.Add(this.tableLayoutPanel4);
            this.panelInitImgStrength.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInitImgStrength.Location = new System.Drawing.Point(0, 0);
            this.panelInitImgStrength.Name = "panelInitImgStrength";
            this.panelInitImgStrength.Size = new System.Drawing.Size(633, 35);
            this.panelInitImgStrength.TabIndex = 8;
            this.panelInitImgStrength.Visible = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(5, 10);
            this.label11.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(149, 13);
            this.label11.TabIndex = 90;
            this.label11.Text = "Init Image Strength (Influence)";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel4.Controls.Add(this.textboxSliderInitStrength, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.sliderInitStrength, 0, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(233, 6);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(6);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(291, 21);
            this.tableLayoutPanel4.TabIndex = 89;
            // 
            // panelPrompt
            // 
            this.panelPrompt.Controls.Add(this.labelCurrentConcept);
            this.panelPrompt.Controls.Add(this.labelCurrentImage);
            this.panelPrompt.Controls.Add(this.btnExpandPromptField);
            this.panelPrompt.Controls.Add(this.btnEmbeddingBrowse);
            this.panelPrompt.Controls.Add(this.btnInitImgBrowse);
            this.panelPrompt.Controls.Add(this.label2);
            this.panelPrompt.Location = new System.Drawing.Point(12, 62);
            this.panelPrompt.Margin = new System.Windows.Forms.Padding(0);
            this.panelPrompt.Name = "panelPrompt";
            this.panelPrompt.Size = new System.Drawing.Size(639, 65);
            this.panelPrompt.TabIndex = 0;
            // 
            // labelCurrentConcept
            // 
            this.labelCurrentConcept.AutoSize = true;
            this.labelCurrentConcept.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurrentConcept.ForeColor = System.Drawing.Color.Silver;
            this.labelCurrentConcept.Location = new System.Drawing.Point(344, 37);
            this.labelCurrentConcept.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.labelCurrentConcept.Name = "labelCurrentConcept";
            this.labelCurrentConcept.Size = new System.Drawing.Size(0, 13);
            this.labelCurrentConcept.TabIndex = 92;
            // 
            // labelCurrentImage
            // 
            this.labelCurrentImage.AutoSize = true;
            this.labelCurrentImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurrentImage.ForeColor = System.Drawing.Color.Silver;
            this.labelCurrentImage.Location = new System.Drawing.Point(344, 9);
            this.labelCurrentImage.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.labelCurrentImage.Name = "labelCurrentImage";
            this.labelCurrentImage.Size = new System.Drawing.Size(0, 13);
            this.labelCurrentImage.TabIndex = 91;
            // 
            // menuStripLogs
            // 
            this.menuStripLogs.Name = "menuStripLogs";
            this.menuStripLogs.ShowImageMargin = false;
            this.menuStripLogs.Size = new System.Drawing.Size(36, 4);
            // 
            // progressBarImg
            // 
            this.progressBarImg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarImg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.progressBarImg.BorderThickness = 0;
            this.progressBarImg.Location = new System.Drawing.Point(654, 650);
            this.progressBarImg.Name = "progressBarImg";
            this.progressBarImg.Size = new System.Drawing.Size(512, 5);
            this.progressBarImg.TabIndex = 110;
            this.progressBarImg.TabStop = false;
            this.progressBarImg.Visible = false;
            // 
            // menuStripRunQueue
            // 
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
            this.menuStripDevTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openCliToolStripMenuItem,
            this.openCmdInCondaEnvironmentToolStripMenuItem,
            this.openModelMergeToolToolStripMenuItem,
            this.openModelPruningTrimmingToolToolStripMenuItem,
            this.viewLogInRealtimeToolStripMenuItem,
            this.trainDreamBoothModelToolStripMenuItem});
            this.menuStripDevTools.Name = "menuStripDevTools";
            this.menuStripDevTools.ShowImageMargin = false;
            this.menuStripDevTools.Size = new System.Drawing.Size(231, 136);
            // 
            // openCliToolStripMenuItem
            // 
            this.openCliToolStripMenuItem.Name = "openCliToolStripMenuItem";
            this.openCliToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.openCliToolStripMenuItem.Text = "Open Stable Diffusion CLI";
            this.openCliToolStripMenuItem.Click += new System.EventHandler(this.openDreampyCLIToolStripMenuItem_Click);
            // 
            // openCmdInCondaEnvironmentToolStripMenuItem
            // 
            this.openCmdInCondaEnvironmentToolStripMenuItem.Name = "openCmdInCondaEnvironmentToolStripMenuItem";
            this.openCmdInCondaEnvironmentToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.openCmdInCondaEnvironmentToolStripMenuItem.Text = "Open CMD in Conda Environment";
            this.openCmdInCondaEnvironmentToolStripMenuItem.Click += new System.EventHandler(this.openCmdInCondaEnvironmentToolStripMenuItem_Click);
            // 
            // openModelMergeToolToolStripMenuItem
            // 
            this.openModelMergeToolToolStripMenuItem.Name = "openModelMergeToolToolStripMenuItem";
            this.openModelMergeToolToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.openModelMergeToolToolStripMenuItem.Text = "Merge Models";
            this.openModelMergeToolToolStripMenuItem.Click += new System.EventHandler(this.openModelMergeToolToolStripMenuItem_Click);
            // 
            // openModelPruningTrimmingToolToolStripMenuItem
            // 
            this.openModelPruningTrimmingToolToolStripMenuItem.Name = "openModelPruningTrimmingToolToolStripMenuItem";
            this.openModelPruningTrimmingToolToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.openModelPruningTrimmingToolToolStripMenuItem.Text = "Prune (Trim) Models";
            this.openModelPruningTrimmingToolToolStripMenuItem.Click += new System.EventHandler(this.openModelPruningTrimmingToolToolStripMenuItem_Click);
            // 
            // viewLogInRealtimeToolStripMenuItem
            // 
            this.viewLogInRealtimeToolStripMenuItem.Name = "viewLogInRealtimeToolStripMenuItem";
            this.viewLogInRealtimeToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.viewLogInRealtimeToolStripMenuItem.Text = "View Log In Realtime";
            this.viewLogInRealtimeToolStripMenuItem.Click += new System.EventHandler(this.viewLogInRealtimeToolStripMenuItem_Click);
            // 
            // trainDreamBoothModelToolStripMenuItem
            // 
            this.trainDreamBoothModelToolStripMenuItem.Name = "trainDreamBoothModelToolStripMenuItem";
            this.trainDreamBoothModelToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.trainDreamBoothModelToolStripMenuItem.Text = "Train DreamBooth Model";
            this.trainDreamBoothModelToolStripMenuItem.Click += new System.EventHandler(this.trainDreamBoothModelToolStripMenuItem_Click);
            // 
            // pictBoxImgViewer
            // 
            this.pictBoxImgViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictBoxImgViewer.BackgroundImage = global::StableDiffusionGui.Properties.Resources.checkerboard_darkened;
            this.pictBoxImgViewer.Location = new System.Drawing.Point(654, 143);
            this.pictBoxImgViewer.Name = "pictBoxImgViewer";
            this.pictBoxImgViewer.Size = new System.Drawing.Size(512, 512);
            this.pictBoxImgViewer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictBoxImgViewer.TabIndex = 113;
            this.pictBoxImgViewer.TabStop = false;
            this.pictBoxImgViewer.Click += new System.EventHandler(this.pictBoxImgViewer_Click);
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
            this.separator.Location = new System.Drawing.Point(896, 12);
            this.separator.Name = "separator";
            this.separator.Size = new System.Drawing.Size(40, 40);
            this.separator.TabIndex = 75;
            this.separator.TabStop = false;
            this.separator.UseVisualStyleBackColor = false;
            // 
            // menuStripPostProcess
            // 
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
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(1178, 750);
            this.Controls.Add(this.labelImgPrompt);
            this.Controls.Add(this.btnDeleteBatch);
            this.Controls.Add(this.textboxPrompt);
            this.Controls.Add(this.progressBarImg);
            this.Controls.Add(this.pictBoxImgViewer);
            this.Controls.Add(this.btnPromptHistory);
            this.Controls.Add(this.btnQueue);
            this.Controls.Add(this.btnPostProc);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnDebug);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cliButton);
            this.Controls.Add(this.panelPrompt);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.progressCircle);
            this.Controls.Add(this.btnOpenOutFolder);
            this.Controls.Add(this.btnPrevImg);
            this.Controls.Add(this.labelImgInfo);
            this.Controls.Add(this.btnNextImg);
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
            this.Text = "Stable Diffusion GUI 1.6.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.ResizeBegin += new System.EventHandler(this.MainForm_ResizeBegin);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.upDownIterations)).EndInit();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upDownSeed)).EndInit();
            this.menuStripOutputImg.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panelDebugSendStdin.ResumeLayout(false);
            this.panelDebugSendStdin.PerformLayout();
            this.panelSeamless.ResumeLayout(false);
            this.panelSeamless.PerformLayout();
            this.panelSampler.ResumeLayout(false);
            this.panelSampler.PerformLayout();
            this.panelRes.ResumeLayout(false);
            this.panelRes.PerformLayout();
            this.panelSeed.ResumeLayout(false);
            this.panelSeed.PerformLayout();
            this.panelScale.ResumeLayout(false);
            this.panelScale.PerformLayout();
            this.panelSteps.ResumeLayout(false);
            this.panelSteps.PerformLayout();
            this.panelIterations.ResumeLayout(false);
            this.panelIterations.PerformLayout();
            this.panelInpainting.ResumeLayout(false);
            this.panelInpainting.PerformLayout();
            this.panelInitImgStrength.ResumeLayout(false);
            this.panelInitImgStrength.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.panelPrompt.ResumeLayout(false);
            this.panelPrompt.PerformLayout();
            this.menuStripRunQueue.ResumeLayout(false);
            this.menuStripAddToQueue.ResumeLayout(false);
            this.menuStripDeleteImages.ResumeLayout(false);
            this.menuStripDevTools.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxImgViewer)).EndInit();
            this.menuStripPostProcess.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button runBtn;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button discordBtn;
        private System.Windows.Forms.Button patreonBtn;
        private System.Windows.Forms.Button paypalBtn;
        private System.Windows.Forms.Button installerBtn;
        private System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.Button btnNextImg;
        private System.Windows.Forms.Label labelImgInfo;
        private System.Windows.Forms.Button btnPrevImg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textboxPrompt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown upDownIterations;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private CustomSlider sliderScale;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown upDownSeed;
        private System.Windows.Forms.Button btnOpenOutFolder;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private CircularProgressBar.CircularProgressBar progressCircle;
        private HTAlt.WinForms.HTProgressBar progressBar;
        private System.Windows.Forms.TextBox textboxExtraScales;
        private System.Windows.Forms.Button separator;
        private System.Windows.Forms.ContextMenuStrip menuStripOutputImg;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openOutputFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyImageToClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copySeedToClipboardToolStripMenuItem;
        private System.Windows.Forms.Button cliButton;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboxSampler;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelSteps;
        private System.Windows.Forms.Panel panelIterations;
        private System.Windows.Forms.Panel panelPrompt;
        private System.Windows.Forms.Panel panelSampler;
        private System.Windows.Forms.Panel panelRes;
        private System.Windows.Forms.Panel panelSeed;
        private System.Windows.Forms.Panel panelScale;
        private System.Windows.Forms.Panel panelSeamless;
        private System.Windows.Forms.Panel panelInitImgStrength;
        private HTAlt.WinForms.HTButton btnInitImgBrowse;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private StableDiffusionGui.Controls.CustomSlider sliderInitStrength;
        private System.Windows.Forms.Button btnDebug;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.TextBox textboxExtraInitStrengths;
        private HTAlt.WinForms.HTButton btnEmbeddingBrowse;
        private System.Windows.Forms.ToolStripMenuItem useAsInitImageToolStripMenuItem;
        private System.Windows.Forms.Button btnPostProc;
        private System.Windows.Forms.CheckBox checkboxSeamless;
        private System.Windows.Forms.Label label8;
        private HTAlt.WinForms.HTButton btnSeedUsePrevious;
        private HTAlt.WinForms.HTButton btnSeedResetToRandom;
        private System.Windows.Forms.ContextMenuStrip menuStripLogs;
        private HTAlt.WinForms.HTProgressBar progressBarImg;
        private System.Windows.Forms.Button btnExpandPromptField;
        private System.Windows.Forms.Button btnQueue;
        private System.Windows.Forms.Button btnPromptHistory;
        private System.Windows.Forms.ContextMenuStrip menuStripRunQueue;
        private System.Windows.Forms.ToolStripMenuItem generateCurrentPromptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateAllQueuedPromptsToolStripMenuItem;
        private System.Windows.Forms.Panel panelInpainting;
        private System.Windows.Forms.CheckBox checkboxInpainting;
        private System.Windows.Forms.Label label10;
        private HTAlt.WinForms.HTButton btnResetMask;
        private System.Windows.Forms.PictureBox pictBoxImgViewer;
        private System.Windows.Forms.TextBox textboxCliTest;
        private System.Windows.Forms.ContextMenuStrip menuStripAddToQueue;
        private System.Windows.Forms.ToolStripMenuItem addCurrentSettingsToQueueToolStripMenuItem;
        private System.Windows.Forms.Label labelCurrentConcept;
        private System.Windows.Forms.Label labelCurrentImage;
        private System.Windows.Forms.ToolStripMenuItem reGenerateImageWithCurrentSettingsToolStripMenuItem;
        private System.Windows.Forms.Button btnDeleteBatch;
        private System.Windows.Forms.ContextMenuStrip menuStripDeleteImages;
        private System.Windows.Forms.ToolStripMenuItem deleteThisImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteAllCurrentImagesToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip menuStripDevTools;
        private System.Windows.Forms.ToolStripMenuItem openCliToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openModelMergeToolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openModelPruningTrimmingToolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewLogInRealtimeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trainDreamBoothModelToolStripMenuItem;
        private System.Windows.Forms.TextBox textboxSliderSteps;
        private CustomSlider sliderSteps;
        private System.Windows.Forms.TextBox textboxSliderScale;
        private System.Windows.Forms.TextBox textboxSliderInitStrength;
        private System.Windows.Forms.ComboBox comboxResH;
        private System.Windows.Forms.ComboBox comboxResW;
        private System.Windows.Forms.CheckBox checkboxHiresFix;
        private System.Windows.Forms.ToolStripMenuItem fitWindowSizeToImageSizeToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkboxLockSeed;
        private System.Windows.Forms.Label labelImgPrompt;
        private System.Windows.Forms.Panel panelDebugSendStdin;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ToolStripMenuItem openCmdInCondaEnvironmentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToFavoritesToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip menuStripPostProcess;
        private System.Windows.Forms.ToolStripMenuItem upscaleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem faceRestorationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem applyAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem postProcessImageToolStripMenuItem;
    }
}

