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
            this.imgBoxOutput = new Cyotek.Windows.Forms.ImageBox();
            this.outputImgLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textboxPrompt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.upDownIterations = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.sliderSteps = new HTAlt.WinForms.HTSlider();
            this.iterLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.sliderScale = new HTAlt.WinForms.HTSlider();
            this.scaleLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.upDownSeed = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.sliderResW = new HTAlt.WinForms.HTSlider();
            this.labelResW = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.sliderResH = new HTAlt.WinForms.HTSlider();
            this.labelResH = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.progressCircle = new CircularProgressBar.CircularProgressBar();
            this.progressBar = new HTAlt.WinForms.HTProgressBar();
            this.textboxExtraScales = new System.Windows.Forms.TextBox();
            this.menuStripOutputImg = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openOutputFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyImageToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copySeedToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useAsInitImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.comboxSampler = new System.Windows.Forms.ComboBox();
            this.sliderInitStrength = new HTAlt.WinForms.HTSlider();
            this.textboxExtraInitStrengths = new System.Windows.Forms.TextBox();
            this.checkboxSeamless = new System.Windows.Forms.CheckBox();
            this.btnEmbeddingBrowse = new HTAlt.WinForms.HTButton();
            this.btnInitImgBrowse = new HTAlt.WinForms.HTButton();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel12 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.panelSeamless = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.panelSampler = new System.Windows.Forms.Panel();
            this.panelRes = new System.Windows.Forms.Panel();
            this.panelSeed = new System.Windows.Forms.Panel();
            this.btnSeedResetToRandom = new HTAlt.WinForms.HTButton();
            this.btnSeedUsePrevious = new HTAlt.WinForms.HTButton();
            this.panelScale = new System.Windows.Forms.Panel();
            this.panelSteps = new System.Windows.Forms.Panel();
            this.panelIterations = new System.Windows.Forms.Panel();
            this.panelInitImgStrength = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.labelInitStrength = new System.Windows.Forms.Label();
            this.panelPrompt = new System.Windows.Forms.Panel();
            this.labelPromptInfo = new System.Windows.Forms.Label();
            this.menuStripLogs = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.progressBarImg = new HTAlt.WinForms.HTProgressBar();
            this.btnPromptHistory = new System.Windows.Forms.Button();
            this.btnQueue = new System.Windows.Forms.Button();
            this.btnPostProc = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnDebug = new System.Windows.Forms.Button();
            this.btnExpandPromptField = new System.Windows.Forms.Button();
            this.cliButton = new System.Windows.Forms.Button();
            this.btnImgShare = new System.Windows.Forms.Button();
            this.btnOpenOutFolder = new System.Windows.Forms.Button();
            this.btnPrevImg = new System.Windows.Forms.Button();
            this.btnNextImg = new System.Windows.Forms.Button();
            this.installerBtn = new System.Windows.Forms.Button();
            this.separator = new System.Windows.Forms.Button();
            this.discordBtn = new System.Windows.Forms.Button();
            this.patreonBtn = new System.Windows.Forms.Button();
            this.paypalBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.upDownIterations)).BeginInit();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upDownSeed)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.menuStripOutputImg.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelSeamless.SuspendLayout();
            this.panelSampler.SuspendLayout();
            this.panelRes.SuspendLayout();
            this.panelSeed.SuspendLayout();
            this.panelScale.SuspendLayout();
            this.panelSteps.SuspendLayout();
            this.panelIterations.SuspendLayout();
            this.panelInitImgStrength.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.panelPrompt.SuspendLayout();
            this.SuspendLayout();
            // 
            // runBtn
            // 
            this.runBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.runBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.runBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.runBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.runBtn.ForeColor = System.Drawing.Color.White;
            this.runBtn.Location = new System.Drawing.Point(663, 598);
            this.runBtn.Name = "runBtn";
            this.runBtn.Size = new System.Drawing.Size(150, 40);
            this.runBtn.TabIndex = 100;
            this.runBtn.Text = "Generate!";
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
            this.logBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.logBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logBox.ForeColor = System.Drawing.Color.Silver;
            this.logBox.Location = new System.Drawing.Point(12, 579);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logBox.Size = new System.Drawing.Size(633, 83);
            this.logBox.TabIndex = 78;
            this.logBox.TabStop = false;
            // 
            // imgBoxOutput
            // 
            this.imgBoxOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imgBoxOutput.ForeColor = System.Drawing.Color.White;
            this.imgBoxOutput.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.imgBoxOutput.GridColorAlternate = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.imgBoxOutput.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            this.imgBoxOutput.Location = new System.Drawing.Point(658, 62);
            this.imgBoxOutput.Margin = new System.Windows.Forms.Padding(10);
            this.imgBoxOutput.Name = "imgBoxOutput";
            this.imgBoxOutput.Size = new System.Drawing.Size(514, 514);
            this.imgBoxOutput.TabIndex = 79;
            this.imgBoxOutput.TabStop = false;
            this.imgBoxOutput.Text = "The generated images will be shown here.";
            this.imgBoxOutput.Click += new System.EventHandler(this.imgBoxOutput_Click);
            // 
            // outputImgLabel
            // 
            this.outputImgLabel.AutoSize = true;
            this.outputImgLabel.ForeColor = System.Drawing.Color.Silver;
            this.outputImgLabel.Location = new System.Drawing.Point(660, 579);
            this.outputImgLabel.Name = "outputImgLabel";
            this.outputImgLabel.Size = new System.Drawing.Size(133, 13);
            this.outputImgLabel.TabIndex = 81;
            this.outputImgLabel.Text = "No output images to show.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(5, 10);
            this.label2.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 83;
            this.label2.Text = "Prompt";
            // 
            // textboxPrompt
            // 
            this.textboxPrompt.AllowDrop = true;
            this.textboxPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxPrompt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textboxPrompt.ForeColor = System.Drawing.Color.White;
            this.textboxPrompt.Location = new System.Drawing.Point(233, 7);
            this.textboxPrompt.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxPrompt.Multiline = true;
            this.textboxPrompt.Name = "textboxPrompt";
            this.textboxPrompt.Size = new System.Drawing.Size(291, 52);
            this.textboxPrompt.TabIndex = 0;
            this.toolTip.SetToolTip(this.textboxPrompt, "Text prompt. The AI will try to generate an image matching this description.");
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
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel6.Controls.Add(this.sliderSteps, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.iterLabel, 1, 0);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(233, 6);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(6);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(400, 21);
            this.tableLayoutPanel6.TabIndex = 88;
            // 
            // sliderSteps
            // 
            this.sliderSteps.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.sliderSteps.BorderRoundRectSize = new System.Drawing.Size(12, 12);
            this.sliderSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sliderSteps.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.sliderSteps.ForeColor = System.Drawing.Color.Black;
            this.sliderSteps.LargeChange = ((uint)(5u));
            this.sliderSteps.Location = new System.Drawing.Point(0, 0);
            this.sliderSteps.Margin = new System.Windows.Forms.Padding(0);
            this.sliderSteps.Maximum = 50;
            this.sliderSteps.Minimum = 1;
            this.sliderSteps.Name = "sliderSteps";
            this.sliderSteps.OverlayColor = System.Drawing.Color.White;
            this.sliderSteps.Size = new System.Drawing.Size(355, 21);
            this.sliderSteps.SmallChange = ((uint)(1u));
            this.sliderSteps.TabIndex = 4;
            this.sliderSteps.ThumbRoundRectSize = new System.Drawing.Size(16, 16);
            this.sliderSteps.ThumbSize = new System.Drawing.Size(14, 14);
            this.toolTip.SetToolTip(this.sliderSteps, "Higher can create more detail and sometimes higher quality images, but more steps" +
        " take more time.");
            this.sliderSteps.Value = 20;
            this.sliderSteps.Scroll += new System.Windows.Forms.ScrollEventHandler(this.sliderSteps_Scroll);
            // 
            // iterLabel
            // 
            this.iterLabel.AutoSize = true;
            this.iterLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.iterLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iterLabel.ForeColor = System.Drawing.Color.Silver;
            this.iterLabel.Location = new System.Drawing.Point(362, 0);
            this.iterLabel.Name = "iterLabel";
            this.iterLabel.Size = new System.Drawing.Size(35, 21);
            this.iterLabel.TabIndex = 5;
            this.iterLabel.Text = "1000";
            this.iterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(5, 10);
            this.label3.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 89;
            this.label3.Text = "Detail (Steps)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(5, 10);
            this.label4.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(153, 13);
            this.label4.TabIndex = 90;
            this.label4.Text = "Creativeness (Guidance Scale)";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.Controls.Add(this.sliderScale, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.scaleLabel, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(233, 6);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(291, 21);
            this.tableLayoutPanel1.TabIndex = 91;
            // 
            // sliderScale
            // 
            this.sliderScale.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.sliderScale.BorderRoundRectSize = new System.Drawing.Size(12, 12);
            this.sliderScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sliderScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.sliderScale.ForeColor = System.Drawing.Color.Black;
            this.sliderScale.LargeChange = ((uint)(5u));
            this.sliderScale.Location = new System.Drawing.Point(0, 0);
            this.sliderScale.Margin = new System.Windows.Forms.Padding(0);
            this.sliderScale.Maximum = 40;
            this.sliderScale.Name = "sliderScale";
            this.sliderScale.OverlayColor = System.Drawing.Color.White;
            this.sliderScale.Size = new System.Drawing.Size(246, 21);
            this.sliderScale.SmallChange = ((uint)(1u));
            this.sliderScale.TabIndex = 4;
            this.sliderScale.ThumbRoundRectSize = new System.Drawing.Size(16, 16);
            this.sliderScale.ThumbSize = new System.Drawing.Size(14, 14);
            this.toolTip.SetToolTip(this.sliderScale, "Higher tries to match your prompt better, but can get chaotic. 7-12 is a safe ran" +
        "ger for most things.");
            this.sliderScale.Value = 20;
            this.sliderScale.Scroll += new System.Windows.Forms.ScrollEventHandler(this.sliderScale_Scroll);
            // 
            // scaleLabel
            // 
            this.scaleLabel.AutoSize = true;
            this.scaleLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.scaleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scaleLabel.ForeColor = System.Drawing.Color.Silver;
            this.scaleLabel.Location = new System.Drawing.Point(253, 0);
            this.scaleLabel.Name = "scaleLabel";
            this.scaleLabel.Size = new System.Drawing.Size(35, 21);
            this.scaleLabel.TabIndex = 5;
            this.scaleLabel.Text = "1000";
            this.scaleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel2.Controls.Add(this.sliderResW, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelResW, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(233, 5);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(6);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(190, 21);
            this.tableLayoutPanel2.TabIndex = 96;
            // 
            // sliderResW
            // 
            this.sliderResW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.sliderResW.BorderRoundRectSize = new System.Drawing.Size(12, 12);
            this.sliderResW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sliderResW.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.sliderResW.ForeColor = System.Drawing.Color.Black;
            this.sliderResW.LargeChange = ((uint)(4u));
            this.sliderResW.Location = new System.Drawing.Point(0, 0);
            this.sliderResW.Margin = new System.Windows.Forms.Padding(0);
            this.sliderResW.Maximum = 16;
            this.sliderResW.Minimum = 4;
            this.sliderResW.Name = "sliderResW";
            this.sliderResW.OverlayColor = System.Drawing.Color.White;
            this.sliderResW.Size = new System.Drawing.Size(145, 21);
            this.sliderResW.SmallChange = ((uint)(1u));
            this.sliderResW.TabIndex = 4;
            this.sliderResW.ThumbRoundRectSize = new System.Drawing.Size(16, 16);
            this.sliderResW.ThumbSize = new System.Drawing.Size(14, 14);
            this.sliderResW.Value = 16;
            this.sliderResW.Scroll += new System.Windows.Forms.ScrollEventHandler(this.sliderResW_Scroll);
            // 
            // labelResW
            // 
            this.labelResW.AutoSize = true;
            this.labelResW.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelResW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelResW.ForeColor = System.Drawing.Color.Silver;
            this.labelResW.Location = new System.Drawing.Point(152, 0);
            this.labelResW.Name = "labelResW";
            this.labelResW.Size = new System.Drawing.Size(35, 21);
            this.labelResW.TabIndex = 5;
            this.labelResW.Text = "1000";
            this.labelResW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel3.Controls.Add(this.sliderResH, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.labelResH, 1, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(443, 6);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(6);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(190, 21);
            this.tableLayoutPanel3.TabIndex = 97;
            // 
            // sliderResH
            // 
            this.sliderResH.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.sliderResH.BorderRoundRectSize = new System.Drawing.Size(12, 12);
            this.sliderResH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sliderResH.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.sliderResH.ForeColor = System.Drawing.Color.Black;
            this.sliderResH.LargeChange = ((uint)(4u));
            this.sliderResH.Location = new System.Drawing.Point(0, 0);
            this.sliderResH.Margin = new System.Windows.Forms.Padding(0);
            this.sliderResH.Maximum = 16;
            this.sliderResH.Minimum = 4;
            this.sliderResH.Name = "sliderResH";
            this.sliderResH.OverlayColor = System.Drawing.Color.White;
            this.sliderResH.Size = new System.Drawing.Size(145, 21);
            this.sliderResH.SmallChange = ((uint)(1u));
            this.sliderResH.TabIndex = 4;
            this.sliderResH.ThumbRoundRectSize = new System.Drawing.Size(16, 16);
            this.sliderResH.ThumbSize = new System.Drawing.Size(14, 14);
            this.sliderResH.Value = 16;
            this.sliderResH.Scroll += new System.Windows.Forms.ScrollEventHandler(this.sliderResH_Scroll);
            // 
            // labelResH
            // 
            this.labelResH.AutoSize = true;
            this.labelResH.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelResH.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelResH.ForeColor = System.Drawing.Color.Silver;
            this.labelResH.Location = new System.Drawing.Point(152, 0);
            this.labelResH.Name = "labelResH";
            this.labelResH.Size = new System.Drawing.Size(35, 21);
            this.labelResH.TabIndex = 5;
            this.labelResH.Text = "1000";
            this.labelResH.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(432, 10);
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
            this.progressBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.progressBar.BorderThickness = 0;
            this.progressBar.Location = new System.Drawing.Point(663, 647);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(509, 15);
            this.progressBar.TabIndex = 100;
            this.progressBar.TabStop = false;
            // 
            // textboxExtraScales
            // 
            this.textboxExtraScales.AllowDrop = true;
            this.textboxExtraScales.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
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
            this.copyImageToClipboardToolStripMenuItem,
            this.copySeedToClipboardToolStripMenuItem,
            this.useAsInitImageToolStripMenuItem});
            this.menuStripOutputImg.Name = "menuStripOutputImg";
            this.menuStripOutputImg.Size = new System.Drawing.Size(181, 114);
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openFileToolStripMenuItem.Text = "Open File";
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // openOutputFolderToolStripMenuItem
            // 
            this.openOutputFolderToolStripMenuItem.Name = "openOutputFolderToolStripMenuItem";
            this.openOutputFolderToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openOutputFolderToolStripMenuItem.Text = "Open Output Folder";
            this.openOutputFolderToolStripMenuItem.Click += new System.EventHandler(this.openOutputFolderToolStripMenuItem_Click);
            // 
            // copyImageToClipboardToolStripMenuItem
            // 
            this.copyImageToClipboardToolStripMenuItem.Name = "copyImageToClipboardToolStripMenuItem";
            this.copyImageToClipboardToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.copyImageToClipboardToolStripMenuItem.Text = "Copy Image";
            this.copyImageToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyImageToClipboardToolStripMenuItem_Click);
            // 
            // copySeedToClipboardToolStripMenuItem
            // 
            this.copySeedToClipboardToolStripMenuItem.Name = "copySeedToClipboardToolStripMenuItem";
            this.copySeedToClipboardToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.copySeedToClipboardToolStripMenuItem.Text = "Copy Seed";
            this.copySeedToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copySeedToClipboardToolStripMenuItem_Click);
            // 
            // useAsInitImageToolStripMenuItem
            // 
            this.useAsInitImageToolStripMenuItem.Name = "useAsInitImageToolStripMenuItem";
            this.useAsInitImageToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.useAsInitImageToolStripMenuItem.Text = "Use as Init Image";
            this.useAsInitImageToolStripMenuItem.Click += new System.EventHandler(this.useAsInitImageToolStripMenuItem_Click);
            // 
            // comboxSampler
            // 
            this.comboxSampler.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxSampler.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxSampler.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxSampler.ForeColor = System.Drawing.Color.White;
            this.comboxSampler.FormattingEnabled = true;
            this.comboxSampler.Items.AddRange(new object[] {
            "k_euler_a",
            "k_euler",
            "k_lms",
            "ddim",
            "plms",
            "k_heun",
            "k_dpm_2",
            "k_dpm_2_a"});
            this.comboxSampler.Location = new System.Drawing.Point(233, 7);
            this.comboxSampler.Name = "comboxSampler";
            this.comboxSampler.Size = new System.Drawing.Size(100, 21);
            this.comboxSampler.TabIndex = 105;
            this.toolTip.SetToolTip(this.comboxSampler, "Changes how the image is sampled.\r\nk_euler_a works very well at low step counts.");
            // 
            // sliderInitStrength
            // 
            this.sliderInitStrength.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.sliderInitStrength.BorderRoundRectSize = new System.Drawing.Size(12, 12);
            this.sliderInitStrength.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sliderInitStrength.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.sliderInitStrength.ForeColor = System.Drawing.Color.Black;
            this.sliderInitStrength.LargeChange = ((uint)(2u));
            this.sliderInitStrength.Location = new System.Drawing.Point(0, 0);
            this.sliderInitStrength.Margin = new System.Windows.Forms.Padding(0);
            this.sliderInitStrength.Maximum = 38;
            this.sliderInitStrength.Minimum = 2;
            this.sliderInitStrength.Name = "sliderInitStrength";
            this.sliderInitStrength.OverlayColor = System.Drawing.Color.White;
            this.sliderInitStrength.Size = new System.Drawing.Size(246, 21);
            this.sliderInitStrength.SmallChange = ((uint)(1u));
            this.sliderInitStrength.TabIndex = 4;
            this.sliderInitStrength.ThumbRoundRectSize = new System.Drawing.Size(16, 16);
            this.sliderInitStrength.ThumbSize = new System.Drawing.Size(14, 14);
            this.toolTip.SetToolTip(this.sliderInitStrength, "Lower value: Result looks more like your text prompt\r\nHigher value: Result looks " +
        "more like your init image");
            this.sliderInitStrength.Value = 2;
            this.sliderInitStrength.Scroll += new System.Windows.Forms.ScrollEventHandler(this.sliderInitStrength_Scroll);
            // 
            // textboxExtraInitStrengths
            // 
            this.textboxExtraInitStrengths.AllowDrop = true;
            this.textboxExtraInitStrengths.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
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
            this.checkboxSeamless.Location = new System.Drawing.Point(233, 9);
            this.checkboxSeamless.Name = "checkboxSeamless";
            this.checkboxSeamless.Size = new System.Drawing.Size(15, 14);
            this.checkboxSeamless.TabIndex = 106;
            this.toolTip.SetToolTip(this.checkboxSeamless, "Create outputs that can be tiled, for backgrounds or textures");
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
            this.btnEmbeddingBrowse.Location = new System.Drawing.Point(533, 36);
            this.btnEmbeddingBrowse.Name = "btnEmbeddingBrowse";
            this.btnEmbeddingBrowse.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnEmbeddingBrowse.Size = new System.Drawing.Size(97, 23);
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
            this.btnInitImgBrowse.Location = new System.Drawing.Point(533, 7);
            this.btnInitImgBrowse.Name = "btnInitImgBrowse";
            this.btnInitImgBrowse.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnInitImgBrowse.Size = new System.Drawing.Size(97, 23);
            this.btnInitImgBrowse.TabIndex = 1;
            this.btnInitImgBrowse.TabStop = false;
            this.btnInitImgBrowse.Text = "Load Image";
            this.toolTip.SetToolTip(this.btnInitImgBrowse, "Load initialization image");
            this.btnInitImgBrowse.Click += new System.EventHandler(this.btnInitImgBrowse_Click);
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
            this.panel1.Controls.Add(this.panel12);
            this.panel1.Controls.Add(this.panel11);
            this.panel1.Controls.Add(this.panelSeamless);
            this.panel1.Controls.Add(this.panelSampler);
            this.panel1.Controls.Add(this.panelRes);
            this.panel1.Controls.Add(this.panelSeed);
            this.panel1.Controls.Add(this.panelScale);
            this.panel1.Controls.Add(this.panelSteps);
            this.panel1.Controls.Add(this.panelIterations);
            this.panel1.Controls.Add(this.panelInitImgStrength);
            this.panel1.Controls.Add(this.panelPrompt);
            this.panel1.Location = new System.Drawing.Point(12, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(633, 514);
            this.panel1.TabIndex = 106;
            // 
            // panel12
            // 
            this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel12.Location = new System.Drawing.Point(0, 380);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(633, 35);
            this.panel12.TabIndex = 11;
            // 
            // panel11
            // 
            this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel11.Location = new System.Drawing.Point(0, 345);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(633, 35);
            this.panel11.TabIndex = 10;
            // 
            // panelSeamless
            // 
            this.panelSeamless.Controls.Add(this.checkboxSeamless);
            this.panelSeamless.Controls.Add(this.label8);
            this.panelSeamless.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSeamless.Location = new System.Drawing.Point(0, 310);
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
            this.panelSampler.Location = new System.Drawing.Point(0, 275);
            this.panelSampler.Name = "panelSampler";
            this.panelSampler.Size = new System.Drawing.Size(633, 35);
            this.panelSampler.TabIndex = 6;
            // 
            // panelRes
            // 
            this.panelRes.Controls.Add(this.label6);
            this.panelRes.Controls.Add(this.tableLayoutPanel2);
            this.panelRes.Controls.Add(this.tableLayoutPanel3);
            this.panelRes.Controls.Add(this.label9);
            this.panelRes.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelRes.Location = new System.Drawing.Point(0, 240);
            this.panelRes.Name = "panelRes";
            this.panelRes.Size = new System.Drawing.Size(633, 35);
            this.panelRes.TabIndex = 5;
            // 
            // panelSeed
            // 
            this.panelSeed.Controls.Add(this.btnSeedResetToRandom);
            this.panelSeed.Controls.Add(this.btnSeedUsePrevious);
            this.panelSeed.Controls.Add(this.label5);
            this.panelSeed.Controls.Add(this.upDownSeed);
            this.panelSeed.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSeed.Location = new System.Drawing.Point(0, 205);
            this.panelSeed.Name = "panelSeed";
            this.panelSeed.Size = new System.Drawing.Size(633, 35);
            this.panelSeed.TabIndex = 4;
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
            this.btnSeedUsePrevious.Click += new System.EventHandler(this.btnSeedUsePrevious_Click);
            // 
            // panelScale
            // 
            this.panelScale.Controls.Add(this.label4);
            this.panelScale.Controls.Add(this.tableLayoutPanel1);
            this.panelScale.Controls.Add(this.textboxExtraScales);
            this.panelScale.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelScale.Location = new System.Drawing.Point(0, 170);
            this.panelScale.Name = "panelScale";
            this.panelScale.Size = new System.Drawing.Size(633, 35);
            this.panelScale.TabIndex = 3;
            // 
            // panelSteps
            // 
            this.panelSteps.Controls.Add(this.label3);
            this.panelSteps.Controls.Add(this.tableLayoutPanel6);
            this.panelSteps.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSteps.Location = new System.Drawing.Point(0, 135);
            this.panelSteps.Name = "panelSteps";
            this.panelSteps.Size = new System.Drawing.Size(633, 35);
            this.panelSteps.TabIndex = 2;
            // 
            // panelIterations
            // 
            this.panelIterations.Controls.Add(this.label1);
            this.panelIterations.Controls.Add(this.upDownIterations);
            this.panelIterations.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelIterations.Location = new System.Drawing.Point(0, 100);
            this.panelIterations.Name = "panelIterations";
            this.panelIterations.Size = new System.Drawing.Size(633, 35);
            this.panelIterations.TabIndex = 1;
            // 
            // panelInitImgStrength
            // 
            this.panelInitImgStrength.Controls.Add(this.textboxExtraInitStrengths);
            this.panelInitImgStrength.Controls.Add(this.label11);
            this.panelInitImgStrength.Controls.Add(this.tableLayoutPanel4);
            this.panelInitImgStrength.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInitImgStrength.Location = new System.Drawing.Point(0, 65);
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
            this.tableLayoutPanel4.Controls.Add(this.sliderInitStrength, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.labelInitStrength, 1, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(233, 6);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(6);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(291, 21);
            this.tableLayoutPanel4.TabIndex = 89;
            // 
            // labelInitStrength
            // 
            this.labelInitStrength.AutoSize = true;
            this.labelInitStrength.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelInitStrength.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInitStrength.ForeColor = System.Drawing.Color.Silver;
            this.labelInitStrength.Location = new System.Drawing.Point(253, 0);
            this.labelInitStrength.Name = "labelInitStrength";
            this.labelInitStrength.Size = new System.Drawing.Size(35, 21);
            this.labelInitStrength.TabIndex = 5;
            this.labelInitStrength.Text = "1000";
            this.labelInitStrength.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelPrompt
            // 
            this.panelPrompt.Controls.Add(this.btnExpandPromptField);
            this.panelPrompt.Controls.Add(this.labelPromptInfo);
            this.panelPrompt.Controls.Add(this.btnEmbeddingBrowse);
            this.panelPrompt.Controls.Add(this.btnInitImgBrowse);
            this.panelPrompt.Controls.Add(this.label2);
            this.panelPrompt.Controls.Add(this.textboxPrompt);
            this.panelPrompt.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPrompt.Location = new System.Drawing.Point(0, 0);
            this.panelPrompt.Margin = new System.Windows.Forms.Padding(0);
            this.panelPrompt.Name = "panelPrompt";
            this.panelPrompt.Size = new System.Drawing.Size(633, 65);
            this.panelPrompt.TabIndex = 0;
            // 
            // labelPromptInfo
            // 
            this.labelPromptInfo.AutoSize = true;
            this.labelPromptInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPromptInfo.ForeColor = System.Drawing.Color.Silver;
            this.labelPromptInfo.Location = new System.Drawing.Point(5, 29);
            this.labelPromptInfo.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.labelPromptInfo.Name = "labelPromptInfo";
            this.labelPromptInfo.Size = new System.Drawing.Size(0, 13);
            this.labelPromptInfo.TabIndex = 85;
            // 
            // menuStripLogs
            // 
            this.menuStripLogs.Name = "menuStripLogs";
            this.menuStripLogs.Size = new System.Drawing.Size(61, 4);
            // 
            // progressBarImg
            // 
            this.progressBarImg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.progressBarImg.BorderThickness = 0;
            this.progressBarImg.Location = new System.Drawing.Point(658, 571);
            this.progressBarImg.Name = "progressBarImg";
            this.progressBarImg.Size = new System.Drawing.Size(514, 5);
            this.progressBarImg.TabIndex = 110;
            this.progressBarImg.TabStop = false;
            // 
            // btnPromptHistory
            // 
            this.btnPromptHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnPromptHistory.BackgroundImage = global::StableDiffusionGui.Properties.Resources.historyIcon;
            this.btnPromptHistory.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPromptHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPromptHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPromptHistory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnPromptHistory.Location = new System.Drawing.Point(865, 598);
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
            this.btnQueue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnQueue.BackgroundImage = global::StableDiffusionGui.Properties.Resources.queueIcon;
            this.btnQueue.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnQueue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQueue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQueue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnQueue.Location = new System.Drawing.Point(819, 598);
            this.btnQueue.Name = "btnQueue";
            this.btnQueue.Size = new System.Drawing.Size(40, 40);
            this.btnQueue.TabIndex = 111;
            this.btnQueue.TabStop = false;
            this.toolTip.SetToolTip(this.btnQueue, "Add Current Prompts to Queue");
            this.btnQueue.UseVisualStyleBackColor = false;
            // 
            // btnPostProc
            // 
            this.btnPostProc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnPostProc.BackgroundImage = global::StableDiffusionGui.Properties.Resources.baseline_auto_fix_high_white_48dp;
            this.btnPostProc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPostProc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPostProc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPostProc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnPostProc.Location = new System.Drawing.Point(948, 9);
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
            this.btnSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnSettings.BackgroundImage = global::StableDiffusionGui.Properties.Resources.baseline_settings_white_48dp;
            this.btnSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSettings.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnSettings.Location = new System.Drawing.Point(1132, 9);
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
            this.btnDebug.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnDebug.BackgroundImage = global::StableDiffusionGui.Properties.Resources.baseline_list_alt_white_48dp;
            this.btnDebug.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDebug.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDebug.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDebug.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnDebug.Location = new System.Drawing.Point(1086, 9);
            this.btnDebug.Name = "btnDebug";
            this.btnDebug.Size = new System.Drawing.Size(40, 40);
            this.btnDebug.TabIndex = 107;
            this.btnDebug.TabStop = false;
            this.toolTip.SetToolTip(this.btnDebug, "Logs...");
            this.btnDebug.UseVisualStyleBackColor = false;
            this.btnDebug.Click += new System.EventHandler(this.btnDebug_Click);
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
            this.btnExpandPromptField.Location = new System.Drawing.Point(207, 7);
            this.btnExpandPromptField.Name = "btnExpandPromptField";
            this.btnExpandPromptField.Size = new System.Drawing.Size(20, 20);
            this.btnExpandPromptField.TabIndex = 86;
            this.btnExpandPromptField.TabStop = false;
            this.btnExpandPromptField.UseVisualStyleBackColor = false;
            this.btnExpandPromptField.Click += new System.EventHandler(this.btnExpandPromptField_Click);
            // 
            // cliButton
            // 
            this.cliButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.cliButton.BackgroundImage = global::StableDiffusionGui.Properties.Resources.cliIcon;
            this.cliButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cliButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cliButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cliButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.cliButton.Location = new System.Drawing.Point(994, 9);
            this.cliButton.Name = "cliButton";
            this.cliButton.Size = new System.Drawing.Size(40, 40);
            this.cliButton.TabIndex = 103;
            this.cliButton.TabStop = false;
            this.toolTip.SetToolTip(this.cliButton, "Open Dream.py Command Line Interface");
            this.cliButton.UseVisualStyleBackColor = false;
            this.cliButton.Click += new System.EventHandler(this.cliButton_Click);
            // 
            // btnImgShare
            // 
            this.btnImgShare.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnImgShare.BackgroundImage = global::StableDiffusionGui.Properties.Resources.shareIco;
            this.btnImgShare.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnImgShare.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImgShare.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImgShare.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnImgShare.Location = new System.Drawing.Point(1040, 598);
            this.btnImgShare.Margin = new System.Windows.Forms.Padding(6);
            this.btnImgShare.Name = "btnImgShare";
            this.btnImgShare.Size = new System.Drawing.Size(40, 40);
            this.btnImgShare.TabIndex = 102;
            this.btnImgShare.TabStop = false;
            this.toolTip.SetToolTip(this.btnImgShare, "Share or export image");
            this.btnImgShare.UseVisualStyleBackColor = false;
            this.btnImgShare.Click += new System.EventHandler(this.btnImgShare_Click);
            // 
            // btnOpenOutFolder
            // 
            this.btnOpenOutFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnOpenOutFolder.BackgroundImage = global::StableDiffusionGui.Properties.Resources.baseline_folder_open_white_48dp;
            this.btnOpenOutFolder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnOpenOutFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenOutFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenOutFolder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnOpenOutFolder.Location = new System.Drawing.Point(994, 598);
            this.btnOpenOutFolder.Name = "btnOpenOutFolder";
            this.btnOpenOutFolder.Size = new System.Drawing.Size(40, 40);
            this.btnOpenOutFolder.TabIndex = 94;
            this.btnOpenOutFolder.TabStop = false;
            this.toolTip.SetToolTip(this.btnOpenOutFolder, "Open output folder");
            this.btnOpenOutFolder.UseVisualStyleBackColor = false;
            this.btnOpenOutFolder.Click += new System.EventHandler(this.btnOpenOutFolder_Click);
            // 
            // btnPrevImg
            // 
            this.btnPrevImg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnPrevImg.BackgroundImage = global::StableDiffusionGui.Properties.Resources.backArrowIcon;
            this.btnPrevImg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPrevImg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevImg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrevImg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnPrevImg.Location = new System.Drawing.Point(1086, 598);
            this.btnPrevImg.Name = "btnPrevImg";
            this.btnPrevImg.Size = new System.Drawing.Size(40, 40);
            this.btnPrevImg.TabIndex = 82;
            this.btnPrevImg.TabStop = false;
            this.toolTip.SetToolTip(this.btnPrevImg, "Previous image");
            this.btnPrevImg.UseVisualStyleBackColor = false;
            this.btnPrevImg.Click += new System.EventHandler(this.btnPrevImg_Click);
            // 
            // btnNextImg
            // 
            this.btnNextImg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnNextImg.BackgroundImage = global::StableDiffusionGui.Properties.Resources.forwardArrowIcon;
            this.btnNextImg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnNextImg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextImg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNextImg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnNextImg.Location = new System.Drawing.Point(1132, 598);
            this.btnNextImg.Name = "btnNextImg";
            this.btnNextImg.Size = new System.Drawing.Size(40, 40);
            this.btnNextImg.TabIndex = 80;
            this.btnNextImg.TabStop = false;
            this.toolTip.SetToolTip(this.btnNextImg, "Next image");
            this.btnNextImg.UseVisualStyleBackColor = false;
            this.btnNextImg.Click += new System.EventHandler(this.btnNextImg_Click);
            // 
            // installerBtn
            // 
            this.installerBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.installerBtn.BackgroundImage = global::StableDiffusionGui.Properties.Resources.installIcon;
            this.installerBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.installerBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.installerBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.installerBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.installerBtn.Location = new System.Drawing.Point(1040, 9);
            this.installerBtn.Name = "installerBtn";
            this.installerBtn.Size = new System.Drawing.Size(40, 40);
            this.installerBtn.TabIndex = 76;
            this.installerBtn.TabStop = false;
            this.toolTip.SetToolTip(this.installerBtn, "Open Installer");
            this.installerBtn.UseVisualStyleBackColor = false;
            this.installerBtn.Click += new System.EventHandler(this.installerBtn_Click);
            // 
            // separator
            // 
            this.separator.BackColor = System.Drawing.Color.Transparent;
            this.separator.BackgroundImage = global::StableDiffusionGui.Properties.Resources.separatorTest1;
            this.separator.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.separator.Enabled = false;
            this.separator.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.separator.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.separator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.separator.Location = new System.Drawing.Point(902, 9);
            this.separator.Name = "separator";
            this.separator.Size = new System.Drawing.Size(40, 40);
            this.separator.TabIndex = 75;
            this.separator.TabStop = false;
            this.separator.UseVisualStyleBackColor = false;
            // 
            // discordBtn
            // 
            this.discordBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.discordBtn.BackgroundImage = global::StableDiffusionGui.Properties.Resources.discordNew;
            this.discordBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.discordBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.discordBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.discordBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.discordBtn.Location = new System.Drawing.Point(856, 9);
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
            this.patreonBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.patreonBtn.BackgroundImage = global::StableDiffusionGui.Properties.Resources.patreon256pxColored;
            this.patreonBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.patreonBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.patreonBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.patreonBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.patreonBtn.Location = new System.Drawing.Point(810, 9);
            this.patreonBtn.Name = "patreonBtn";
            this.patreonBtn.Size = new System.Drawing.Size(40, 40);
            this.patreonBtn.TabIndex = 73;
            this.patreonBtn.TabStop = false;
            this.toolTip.SetToolTip(this.patreonBtn, "Support me on Patreon");
            this.patreonBtn.UseVisualStyleBackColor = false;
            this.patreonBtn.Click += new System.EventHandler(this.patreonBtn_Click);
            // 
            // paypalBtn
            // 
            this.paypalBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.paypalBtn.BackgroundImage = global::StableDiffusionGui.Properties.Resources.paypal256px;
            this.paypalBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.paypalBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.paypalBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.paypalBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.paypalBtn.Location = new System.Drawing.Point(764, 9);
            this.paypalBtn.Name = "paypalBtn";
            this.paypalBtn.Size = new System.Drawing.Size(40, 40);
            this.paypalBtn.TabIndex = 72;
            this.paypalBtn.TabStop = false;
            this.toolTip.SetToolTip(this.paypalBtn, "Donate one-time via PayPal");
            this.paypalBtn.UseVisualStyleBackColor = false;
            this.paypalBtn.Click += new System.EventHandler(this.paypalBtn_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(1184, 671);
            this.Controls.Add(this.btnPromptHistory);
            this.Controls.Add(this.btnQueue);
            this.Controls.Add(this.progressBarImg);
            this.Controls.Add(this.btnPostProc);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnDebug);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cliButton);
            this.Controls.Add(this.btnImgShare);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.progressCircle);
            this.Controls.Add(this.btnOpenOutFolder);
            this.Controls.Add(this.btnPrevImg);
            this.Controls.Add(this.outputImgLabel);
            this.Controls.Add(this.btnNextImg);
            this.Controls.Add(this.imgBoxOutput);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.installerBtn);
            this.Controls.Add(this.separator);
            this.Controls.Add(this.discordBtn);
            this.Controls.Add(this.patreonBtn);
            this.Controls.Add(this.paypalBtn);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.runBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stable Diffusion GUI 1.3.1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.upDownIterations)).EndInit();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upDownSeed)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.menuStripOutputImg.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
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
            this.panelInitImgStrength.ResumeLayout(false);
            this.panelInitImgStrength.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.panelPrompt.ResumeLayout(false);
            this.panelPrompt.PerformLayout();
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
        private Cyotek.Windows.Forms.ImageBox imgBoxOutput;
        private System.Windows.Forms.Button btnNextImg;
        private System.Windows.Forms.Label outputImgLabel;
        private System.Windows.Forms.Button btnPrevImg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textboxPrompt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown upDownIterations;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private HTAlt.WinForms.HTSlider sliderSteps;
        private System.Windows.Forms.Label iterLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private HTAlt.WinForms.HTSlider sliderScale;
        private System.Windows.Forms.Label scaleLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown upDownSeed;
        private System.Windows.Forms.Button btnOpenOutFolder;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private HTAlt.WinForms.HTSlider sliderResW;
        private System.Windows.Forms.Label labelResW;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private HTAlt.WinForms.HTSlider sliderResH;
        private System.Windows.Forms.Label labelResH;
        private System.Windows.Forms.Label label9;
        private CircularProgressBar.CircularProgressBar progressCircle;
        private HTAlt.WinForms.HTProgressBar progressBar;
        private System.Windows.Forms.TextBox textboxExtraScales;
        private System.Windows.Forms.Button separator;
        private System.Windows.Forms.Button btnImgShare;
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
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Panel panelSeamless;
        private System.Windows.Forms.Panel panelInitImgStrength;
        private HTAlt.WinForms.HTButton btnInitImgBrowse;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private HTAlt.WinForms.HTSlider sliderInitStrength;
        private System.Windows.Forms.Label labelInitStrength;
        private System.Windows.Forms.Button btnDebug;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.TextBox textboxExtraInitStrengths;
        private HTAlt.WinForms.HTButton btnEmbeddingBrowse;
        private System.Windows.Forms.Label labelPromptInfo;
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
    }
}

