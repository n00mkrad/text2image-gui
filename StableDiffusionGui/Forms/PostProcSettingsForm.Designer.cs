namespace StableDiffusionGui.Forms
{
    partial class PostProcSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostProcSettingsForm));
            this.titleLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelFaceRestorationStrength = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.sliderFaceRestoreStrength = new HTAlt.WinForms.HTSlider();
            this.labelFaceRestoreStrength = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panelUpscaling = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.comboxUpscale = new System.Windows.Forms.ComboBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel11 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panelFaceRestorationMethod = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.comboxFaceRestoration = new System.Windows.Forms.ComboBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panelFaceRestorationStrength.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel10.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panelUpscaling.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panelFaceRestorationMethod.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Yu Gothic UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.Location = new System.Drawing.Point(11, 9);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(218, 40);
            this.titleLabel.TabIndex = 13;
            this.titleLabel.Text = "Post-Processing";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.panelFaceRestorationStrength);
            this.panel1.Controls.Add(this.panelFaceRestorationMethod);
            this.panel1.Controls.Add(this.panelUpscaling);
            this.panel1.Controls.Add(this.panel11);
            this.panel1.Location = new System.Drawing.Point(12, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(500, 207);
            this.panel1.TabIndex = 14;
            // 
            // panelFaceRestorationStrength
            // 
            this.panelFaceRestorationStrength.Controls.Add(this.tableLayoutPanel2);
            this.panelFaceRestorationStrength.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFaceRestorationStrength.Location = new System.Drawing.Point(0, 105);
            this.panelFaceRestorationStrength.Name = "panelFaceRestorationStrength";
            this.panelFaceRestorationStrength.Size = new System.Drawing.Size(500, 35);
            this.panelFaceRestorationStrength.TabIndex = 12;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel2.Controls.Add(this.panel10, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel9, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(500, 35);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.tableLayoutPanel4);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(203, 3);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(294, 29);
            this.panel10.TabIndex = 88;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel4.Controls.Add(this.sliderFaceRestoreStrength, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.labelFaceRestoreStrength, 1, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 6);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(6);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(288, 21);
            this.tableLayoutPanel4.TabIndex = 91;
            // 
            // sliderFaceRestoreStrength
            // 
            this.sliderFaceRestoreStrength.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.sliderFaceRestoreStrength.BorderRoundRectSize = new System.Drawing.Size(12, 12);
            this.sliderFaceRestoreStrength.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sliderFaceRestoreStrength.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.sliderFaceRestoreStrength.ForeColor = System.Drawing.Color.Black;
            this.sliderFaceRestoreStrength.LargeChange = ((uint)(4u));
            this.sliderFaceRestoreStrength.Location = new System.Drawing.Point(0, 0);
            this.sliderFaceRestoreStrength.Margin = new System.Windows.Forms.Padding(0);
            this.sliderFaceRestoreStrength.Maximum = 20;
            this.sliderFaceRestoreStrength.Name = "sliderFaceRestoreStrength";
            this.sliderFaceRestoreStrength.OverlayColor = System.Drawing.Color.White;
            this.sliderFaceRestoreStrength.Size = new System.Drawing.Size(243, 21);
            this.sliderFaceRestoreStrength.SmallChange = ((uint)(1u));
            this.sliderFaceRestoreStrength.TabIndex = 4;
            this.sliderFaceRestoreStrength.ThumbRoundRectSize = new System.Drawing.Size(16, 16);
            this.sliderFaceRestoreStrength.ThumbSize = new System.Drawing.Size(14, 14);
            this.toolTip.SetToolTip(this.sliderFaceRestoreStrength, "0 = Off, 1 = Full Restoration");
            this.sliderFaceRestoreStrength.Value = 1;
            this.sliderFaceRestoreStrength.Scroll += new System.Windows.Forms.ScrollEventHandler(this.sliderGfpgan_Scroll);
            // 
            // labelFaceRestoreStrength
            // 
            this.labelFaceRestoreStrength.AutoSize = true;
            this.labelFaceRestoreStrength.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelFaceRestoreStrength.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFaceRestoreStrength.ForeColor = System.Drawing.Color.Silver;
            this.labelFaceRestoreStrength.Location = new System.Drawing.Point(250, 0);
            this.labelFaceRestoreStrength.Name = "labelFaceRestoreStrength";
            this.labelFaceRestoreStrength.Size = new System.Drawing.Size(35, 21);
            this.labelFaceRestoreStrength.TabIndex = 5;
            this.labelFaceRestoreStrength.Text = "1000";
            this.labelFaceRestoreStrength.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.label3);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(3, 3);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(194, 29);
            this.panel9.TabIndex = 87;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(5, 10);
            this.label3.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 13);
            this.label3.TabIndex = 85;
            this.label3.Text = "Face Restoration Strength";
            // 
            // panelUpscaling
            // 
            this.panelUpscaling.Controls.Add(this.tableLayoutPanel1);
            this.panelUpscaling.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUpscaling.Location = new System.Drawing.Point(0, 35);
            this.panelUpscaling.Name = "panelUpscaling";
            this.panelUpscaling.Size = new System.Drawing.Size(500, 35);
            this.panelUpscaling.TabIndex = 11;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Controls.Add(this.panel8, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel7, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(500, 35);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.comboxUpscale);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(203, 3);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(294, 29);
            this.panel8.TabIndex = 86;
            // 
            // comboxUpscale
            // 
            this.comboxUpscale.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxUpscale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxUpscale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxUpscale.ForeColor = System.Drawing.Color.White;
            this.comboxUpscale.FormattingEnabled = true;
            this.comboxUpscale.Items.AddRange(new object[] {
            "Disabled",
            "2x",
            "4x"});
            this.comboxUpscale.Location = new System.Drawing.Point(0, 7);
            this.comboxUpscale.Name = "comboxUpscale";
            this.comboxUpscale.Size = new System.Drawing.Size(150, 21);
            this.comboxUpscale.TabIndex = 106;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.label2);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(3, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(194, 29);
            this.panel7.TabIndex = 85;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(5, 10);
            this.label2.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 84;
            this.label2.Text = "Upscaling";
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.label1);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel11.Location = new System.Drawing.Point(0, 0);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(500, 35);
            this.panel11.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "AI Effects";
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 200;
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 200;
            this.toolTip.ReshowDelay = 40;
            // 
            // panelFaceRestorationMethod
            // 
            this.panelFaceRestorationMethod.Controls.Add(this.tableLayoutPanel3);
            this.panelFaceRestorationMethod.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFaceRestorationMethod.Location = new System.Drawing.Point(0, 70);
            this.panelFaceRestorationMethod.Name = "panelFaceRestorationMethod";
            this.panelFaceRestorationMethod.Size = new System.Drawing.Size(500, 35);
            this.panelFaceRestorationMethod.TabIndex = 13;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel3.Controls.Add(this.panel3, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.panel4, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(500, 35);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.comboxFaceRestoration);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(203, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(294, 29);
            this.panel3.TabIndex = 86;
            // 
            // comboxFaceRestoration
            // 
            this.comboxFaceRestoration.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxFaceRestoration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxFaceRestoration.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxFaceRestoration.ForeColor = System.Drawing.Color.White;
            this.comboxFaceRestoration.FormattingEnabled = true;
            this.comboxFaceRestoration.Location = new System.Drawing.Point(0, 7);
            this.comboxFaceRestoration.Name = "comboxFaceRestoration";
            this.comboxFaceRestoration.Size = new System.Drawing.Size(150, 21);
            this.comboxFaceRestoration.TabIndex = 106;
            this.comboxFaceRestoration.SelectedIndexChanged += new System.EventHandler(this.comboxFaceRestoration_SelectedIndexChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label4);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(194, 29);
            this.panel4.TabIndex = 85;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(5, 10);
            this.label4.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 84;
            this.label4.Text = "Face Restoration";
            // 
            // PostProcSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(524, 281);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.titleLabel);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PostProcSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Post-Processing";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PostProcSettingsForm_FormClosing);
            this.Load += new System.EventHandler(this.PostProcSettingsForm_Load);
            this.Shown += new System.EventHandler(this.PostProcSettingsForm_Shown);
            this.panel1.ResumeLayout(false);
            this.panelFaceRestorationStrength.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panelUpscaling.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.panelFaceRestorationMethod.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelFaceRestorationStrength;
        private System.Windows.Forms.Panel panelUpscaling;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private HTAlt.WinForms.HTSlider sliderFaceRestoreStrength;
        private System.Windows.Forms.Label labelFaceRestoreStrength;
        private System.Windows.Forms.ComboBox comboxUpscale;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel panelFaceRestorationMethod;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox comboxFaceRestoration;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label4;
    }
}