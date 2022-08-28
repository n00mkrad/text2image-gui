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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.runBtn = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.installerBtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.discordBtn = new System.Windows.Forms.Button();
            this.patreonBtn = new System.Windows.Forms.Button();
            this.paypalBtn = new System.Windows.Forms.Button();
            this.logBox = new System.Windows.Forms.TextBox();
            this.resultImgBox = new Cyotek.Windows.Forms.ImageBox();
            this.btnNextImg = new System.Windows.Forms.Button();
            this.outputImgLabel = new System.Windows.Forms.Label();
            this.btnPrevImg = new System.Windows.Forms.Button();
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
            ((System.ComponentModel.ISupportInitialize)(this.upDownIterations)).BeginInit();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // runBtn
            // 
            this.runBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.runBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.runBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.runBtn.ForeColor = System.Drawing.Color.White;
            this.runBtn.Location = new System.Drawing.Point(663, 600);
            this.runBtn.Name = "runBtn";
            this.runBtn.Size = new System.Drawing.Size(80, 40);
            this.runBtn.TabIndex = 1;
            this.runBtn.Text = "Run";
            this.runBtn.UseVisualStyleBackColor = false;
            this.runBtn.Click += new System.EventHandler(this.runBtn_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Yu Gothic UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.Location = new System.Drawing.Point(12, 9);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(367, 40);
            this.titleLabel.TabIndex = 11;
            this.titleLabel.Text = "NMKD Stable Diffusion GUI";
            // 
            // installerBtn
            // 
            this.installerBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.installerBtn.BackgroundImage = global::StableDiffusionGui.Properties.Resources.installIcon;
            this.installerBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.installerBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.installerBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.installerBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.installerBtn.Location = new System.Drawing.Point(1132, 9);
            this.installerBtn.Name = "installerBtn";
            this.installerBtn.Size = new System.Drawing.Size(40, 40);
            this.installerBtn.TabIndex = 76;
            this.installerBtn.UseVisualStyleBackColor = false;
            this.installerBtn.Click += new System.EventHandler(this.installerBtn_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BackgroundImage = global::StableDiffusionGui.Properties.Resources.separatorTest1;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.button1.Location = new System.Drawing.Point(1086, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(40, 40);
            this.button1.TabIndex = 75;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // discordBtn
            // 
            this.discordBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.discordBtn.BackgroundImage = global::StableDiffusionGui.Properties.Resources.discordNew;
            this.discordBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.discordBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.discordBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.discordBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.discordBtn.Location = new System.Drawing.Point(1040, 9);
            this.discordBtn.Name = "discordBtn";
            this.discordBtn.Size = new System.Drawing.Size(40, 40);
            this.discordBtn.TabIndex = 74;
            this.discordBtn.UseVisualStyleBackColor = false;
            // 
            // patreonBtn
            // 
            this.patreonBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.patreonBtn.BackgroundImage = global::StableDiffusionGui.Properties.Resources.patreon256pxColored;
            this.patreonBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.patreonBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.patreonBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.patreonBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.patreonBtn.Location = new System.Drawing.Point(994, 9);
            this.patreonBtn.Name = "patreonBtn";
            this.patreonBtn.Size = new System.Drawing.Size(40, 40);
            this.patreonBtn.TabIndex = 73;
            this.patreonBtn.UseVisualStyleBackColor = false;
            // 
            // paypalBtn
            // 
            this.paypalBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.paypalBtn.BackgroundImage = global::StableDiffusionGui.Properties.Resources.paypal256px;
            this.paypalBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.paypalBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.paypalBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.paypalBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.paypalBtn.Location = new System.Drawing.Point(948, 9);
            this.paypalBtn.Name = "paypalBtn";
            this.paypalBtn.Size = new System.Drawing.Size(40, 40);
            this.paypalBtn.TabIndex = 72;
            this.paypalBtn.UseVisualStyleBackColor = false;
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
            this.logBox.Size = new System.Drawing.Size(633, 59);
            this.logBox.TabIndex = 78;
            // 
            // resultImgBox
            // 
            this.resultImgBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.resultImgBox.ForeColor = System.Drawing.Color.White;
            this.resultImgBox.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.resultImgBox.GridColorAlternate = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.resultImgBox.Location = new System.Drawing.Point(658, 62);
            this.resultImgBox.Margin = new System.Windows.Forms.Padding(10);
            this.resultImgBox.Name = "resultImgBox";
            this.resultImgBox.Size = new System.Drawing.Size(514, 514);
            this.resultImgBox.TabIndex = 79;
            this.resultImgBox.Text = "The generated images will be shown here.";
            // 
            // btnNextImg
            // 
            this.btnNextImg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnNextImg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnNextImg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextImg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNextImg.ForeColor = System.Drawing.Color.White;
            this.btnNextImg.Location = new System.Drawing.Point(1132, 598);
            this.btnNextImg.Name = "btnNextImg";
            this.btnNextImg.Size = new System.Drawing.Size(40, 40);
            this.btnNextImg.TabIndex = 80;
            this.btnNextImg.Text = ">";
            this.btnNextImg.UseVisualStyleBackColor = false;
            this.btnNextImg.Click += new System.EventHandler(this.btnNextImg_Click);
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
            // btnPrevImg
            // 
            this.btnPrevImg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnPrevImg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPrevImg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevImg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrevImg.ForeColor = System.Drawing.Color.White;
            this.btnPrevImg.Location = new System.Drawing.Point(1086, 598);
            this.btnPrevImg.Name = "btnPrevImg";
            this.btnPrevImg.Size = new System.Drawing.Size(40, 40);
            this.btnPrevImg.TabIndex = 82;
            this.btnPrevImg.Text = "<";
            this.btnPrevImg.UseVisualStyleBackColor = false;
            this.btnPrevImg.Click += new System.EventHandler(this.btnPrevImg_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(17, 65);
            this.label2.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 83;
            this.label2.Text = "Prompt";
            // 
            // textboxPrompt
            // 
            this.textboxPrompt.AllowDrop = true;
            this.textboxPrompt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textboxPrompt.ForeColor = System.Drawing.Color.White;
            this.textboxPrompt.Location = new System.Drawing.Point(245, 62);
            this.textboxPrompt.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxPrompt.Name = "textboxPrompt";
            this.textboxPrompt.Size = new System.Drawing.Size(400, 20);
            this.textboxPrompt.TabIndex = 84;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(17, 95);
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
            this.upDownIterations.Location = new System.Drawing.Point(545, 93);
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
            this.upDownIterations.TabIndex = 86;
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
            this.tableLayoutPanel6.Location = new System.Drawing.Point(245, 122);
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
            this.label3.Location = new System.Drawing.Point(17, 125);
            this.label3.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 89;
            this.label3.Text = "Quality (Steps)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(17, 155);
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
            this.tableLayoutPanel1.Location = new System.Drawing.Point(245, 155);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(400, 21);
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
            this.sliderScale.Size = new System.Drawing.Size(355, 21);
            this.sliderScale.SmallChange = ((uint)(1u));
            this.sliderScale.TabIndex = 4;
            this.sliderScale.ThumbRoundRectSize = new System.Drawing.Size(16, 16);
            this.sliderScale.ThumbSize = new System.Drawing.Size(14, 14);
            this.sliderScale.Value = 20;
            this.sliderScale.Scroll += new System.Windows.Forms.ScrollEventHandler(this.sliderScale_Scroll);
            // 
            // scaleLabel
            // 
            this.scaleLabel.AutoSize = true;
            this.scaleLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.scaleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scaleLabel.ForeColor = System.Drawing.Color.Silver;
            this.scaleLabel.Location = new System.Drawing.Point(362, 0);
            this.scaleLabel.Name = "scaleLabel";
            this.scaleLabel.Size = new System.Drawing.Size(35, 21);
            this.scaleLabel.TabIndex = 5;
            this.scaleLabel.Text = "1000";
            this.scaleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(1184, 650);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tableLayoutPanel6);
            this.Controls.Add(this.upDownIterations);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textboxPrompt);
            this.Controls.Add(this.btnPrevImg);
            this.Controls.Add(this.outputImgLabel);
            this.Controls.Add(this.btnNextImg);
            this.Controls.Add(this.resultImgBox);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.installerBtn);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.discordBtn);
            this.Controls.Add(this.patreonBtn);
            this.Controls.Add(this.paypalBtn);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.runBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stable Diffusion GUI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.upDownIterations)).EndInit();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button runBtn;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button discordBtn;
        private System.Windows.Forms.Button patreonBtn;
        private System.Windows.Forms.Button paypalBtn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button installerBtn;
        private System.Windows.Forms.TextBox logBox;
        private Cyotek.Windows.Forms.ImageBox resultImgBox;
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
    }
}

