namespace StableDiffusionGui.Forms
{
    partial class PruneModelsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PruneModelsForm));
            this.titleLabel = new System.Windows.Forms.Label();
            this.parentPanel = new System.Windows.Forms.Panel();
            this.panelDelInput = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.checkboxPruneDeleteInput = new System.Windows.Forms.CheckBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRun = new System.Windows.Forms.Button();
            this.panelPrecision = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.comboxPrunePrecision = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panelModel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.panel25 = new System.Windows.Forms.Panel();
            this.comboxModel = new System.Windows.Forms.ComboBox();
            this.panel26 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnReloadModels = new System.Windows.Forms.Button();
            this.btnOpenModelFolder = new System.Windows.Forms.Button();
            this.parentPanel.SuspendLayout();
            this.panelDelInput.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panelPrecision.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panelModel.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.panel25.SuspendLayout();
            this.panel26.SuspendLayout();
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
            this.titleLabel.Size = new System.Drawing.Size(192, 40);
            this.titleLabel.TabIndex = 14;
            this.titleLabel.Text = "Prune Models";
            // 
            // parentPanel
            // 
            this.parentPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parentPanel.AutoScroll = true;
            this.parentPanel.Controls.Add(this.panelDelInput);
            this.parentPanel.Controls.Add(this.btnRun);
            this.parentPanel.Controls.Add(this.panelPrecision);
            this.parentPanel.Controls.Add(this.panelModel);
            this.parentPanel.Location = new System.Drawing.Point(12, 62);
            this.parentPanel.Name = "parentPanel";
            this.parentPanel.Size = new System.Drawing.Size(560, 187);
            this.parentPanel.TabIndex = 15;
            // 
            // panelDelInput
            // 
            this.panelDelInput.Controls.Add(this.tableLayoutPanel2);
            this.panelDelInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDelInput.Location = new System.Drawing.Point(0, 70);
            this.panelDelInput.Name = "panelDelInput";
            this.panelDelInput.Size = new System.Drawing.Size(560, 35);
            this.panelDelInput.TabIndex = 102;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.panel4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel5, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(560, 35);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.checkboxPruneDeleteInput);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(283, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(274, 29);
            this.panel4.TabIndex = 88;
            // 
            // checkboxPruneDeleteInput
            // 
            this.checkboxPruneDeleteInput.AutoSize = true;
            this.checkboxPruneDeleteInput.ForeColor = System.Drawing.Color.White;
            this.checkboxPruneDeleteInput.Location = new System.Drawing.Point(5, 7);
            this.checkboxPruneDeleteInput.Name = "checkboxPruneDeleteInput";
            this.checkboxPruneDeleteInput.Size = new System.Drawing.Size(15, 14);
            this.checkboxPruneDeleteInput.TabIndex = 112;
            this.checkboxPruneDeleteInput.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label2);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(274, 29);
            this.panel5.TabIndex = 87;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(2, 8);
            this.label2.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 13);
            this.label2.TabIndex = 85;
            this.label2.Text = "Delete Input File If Successful";
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRun.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.btnRun.ForeColor = System.Drawing.Color.White;
            this.btnRun.Location = new System.Drawing.Point(0, 147);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(120, 40);
            this.btnRun.TabIndex = 101;
            this.btnRun.Text = "Prune!";
            this.toolTip.SetToolTip(this.btnRun, "Prune Model");
            this.btnRun.UseVisualStyleBackColor = false;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // panelPrecision
            // 
            this.panelPrecision.Controls.Add(this.tableLayoutPanel1);
            this.panelPrecision.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPrecision.Location = new System.Drawing.Point(0, 35);
            this.panelPrecision.Name = "panelPrecision";
            this.panelPrecision.Size = new System.Drawing.Size(560, 35);
            this.panelPrecision.TabIndex = 21;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(560, 35);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.comboxPrunePrecision);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(283, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(274, 29);
            this.panel2.TabIndex = 88;
            // 
            // comboxPrunePrecision
            // 
            this.comboxPrunePrecision.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboxPrunePrecision.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxPrunePrecision.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxPrunePrecision.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxPrunePrecision.ForeColor = System.Drawing.Color.White;
            this.comboxPrunePrecision.FormattingEnabled = true;
            this.comboxPrunePrecision.Location = new System.Drawing.Point(0, 4);
            this.comboxPrunePrecision.Name = "comboxPrunePrecision";
            this.comboxPrunePrecision.Size = new System.Drawing.Size(274, 21);
            this.comboxPrunePrecision.TabIndex = 107;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(274, 29);
            this.panel3.TabIndex = 87;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(2, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 85;
            this.label1.Text = "Model Output Format";
            // 
            // panelModel
            // 
            this.panelModel.Controls.Add(this.tableLayoutPanel8);
            this.panelModel.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelModel.Location = new System.Drawing.Point(0, 0);
            this.panelModel.Name = "panelModel";
            this.panelModel.Size = new System.Drawing.Size(560, 35);
            this.panelModel.TabIndex = 20;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 2;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.Controls.Add(this.panel25, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.panel26, 0, 0);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(560, 35);
            this.tableLayoutPanel8.TabIndex = 1;
            // 
            // panel25
            // 
            this.panel25.Controls.Add(this.comboxModel);
            this.panel25.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel25.Location = new System.Drawing.Point(283, 3);
            this.panel25.Name = "panel25";
            this.panel25.Size = new System.Drawing.Size(274, 29);
            this.panel25.TabIndex = 88;
            // 
            // comboxModel
            // 
            this.comboxModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboxModel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxModel.ForeColor = System.Drawing.Color.White;
            this.comboxModel.FormattingEnabled = true;
            this.comboxModel.Location = new System.Drawing.Point(0, 4);
            this.comboxModel.Name = "comboxModel";
            this.comboxModel.Size = new System.Drawing.Size(274, 21);
            this.comboxModel.TabIndex = 106;
            // 
            // panel26
            // 
            this.panel26.Controls.Add(this.label10);
            this.panel26.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel26.Location = new System.Drawing.Point(3, 3);
            this.panel26.Name = "panel26";
            this.panel26.Size = new System.Drawing.Size(274, 29);
            this.panel26.TabIndex = 87;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(2, 8);
            this.label10.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 13);
            this.label10.TabIndex = 85;
            this.label10.Text = "Model File";
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 200;
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 200;
            this.toolTip.ReshowDelay = 40;
            // 
            // btnReloadModels
            // 
            this.btnReloadModels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReloadModels.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnReloadModels.BackgroundImage = global::StableDiffusionGui.Properties.Resources.baseline_refresh_white_48dp;
            this.btnReloadModels.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnReloadModels.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReloadModels.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReloadModels.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnReloadModels.Location = new System.Drawing.Point(532, 9);
            this.btnReloadModels.Name = "btnReloadModels";
            this.btnReloadModels.Size = new System.Drawing.Size(40, 40);
            this.btnReloadModels.TabIndex = 110;
            this.btnReloadModels.TabStop = false;
            this.toolTip.SetToolTip(this.btnReloadModels, "Reload Model List");
            this.btnReloadModels.UseVisualStyleBackColor = false;
            this.btnReloadModels.Click += new System.EventHandler(this.btnReloadModels_Click);
            // 
            // btnOpenModelFolder
            // 
            this.btnOpenModelFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenModelFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnOpenModelFolder.BackgroundImage = global::StableDiffusionGui.Properties.Resources.baseline_folder_open_white_48dp;
            this.btnOpenModelFolder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnOpenModelFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenModelFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenModelFolder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnOpenModelFolder.Location = new System.Drawing.Point(486, 9);
            this.btnOpenModelFolder.Name = "btnOpenModelFolder";
            this.btnOpenModelFolder.Size = new System.Drawing.Size(40, 40);
            this.btnOpenModelFolder.TabIndex = 109;
            this.btnOpenModelFolder.TabStop = false;
            this.toolTip.SetToolTip(this.btnOpenModelFolder, "Manage Model Folders");
            this.btnOpenModelFolder.UseVisualStyleBackColor = false;
            this.btnOpenModelFolder.Click += new System.EventHandler(this.btnOpenModelFolder_Click);
            // 
            // PruneModelsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(584, 261);
            this.Controls.Add(this.btnReloadModels);
            this.Controls.Add(this.btnOpenModelFolder);
            this.Controls.Add(this.parentPanel);
            this.Controls.Add(this.titleLabel);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PruneModelsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Prune Models";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PruneModelsForm_FormClosing);
            this.Load += new System.EventHandler(this.PruneModelsForm_Load);
            this.parentPanel.ResumeLayout(false);
            this.panelDelInput.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panelPrecision.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panelModel.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.panel25.ResumeLayout(false);
            this.panel26.ResumeLayout(false);
            this.panel26.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Panel parentPanel;
        private System.Windows.Forms.Panel panelModel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.Panel panel25;
        private System.Windows.Forms.ComboBox comboxModel;
        private System.Windows.Forms.Panel panel26;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panelPrecision;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOpenModelFolder;
        private System.Windows.Forms.Button btnReloadModels;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.ComboBox comboxPrunePrecision;
        private System.Windows.Forms.Panel panelDelInput;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkboxPruneDeleteInput;
    }
}