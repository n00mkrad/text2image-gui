namespace StableDiffusionGui.Forms
{
    partial class UpdaterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdaterForm));
            this.parentPanel = new System.Windows.Forms.Panel();
            this.panelCopy = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkboxKeepModels = new System.Windows.Forms.CheckBox();
            this.checkboxKeepSettings = new System.Windows.Forms.CheckBox();
            this.checkboxKeepImages = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panelVersion = new System.Windows.Forms.Panel();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.panel25 = new System.Windows.Forms.Panel();
            this.comboxVersion = new System.Windows.Forms.ComboBox();
            this.panel26 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.panelInstalled = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.labelCurrVersion = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnInstall = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.parentPanel.SuspendLayout();
            this.panelCopy.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panelVersion.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.panel25.SuspendLayout();
            this.panel26.SuspendLayout();
            this.panelInstalled.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // parentPanel
            // 
            this.parentPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parentPanel.AutoScroll = true;
            this.parentPanel.Controls.Add(this.panelCopy);
            this.parentPanel.Controls.Add(this.panelVersion);
            this.parentPanel.Controls.Add(this.panelInstalled);
            this.parentPanel.Controls.Add(this.btnInstall);
            this.parentPanel.Location = new System.Drawing.Point(12, 62);
            this.parentPanel.Name = "parentPanel";
            this.parentPanel.Size = new System.Drawing.Size(560, 187);
            this.parentPanel.TabIndex = 16;
            // 
            // panelCopy
            // 
            this.panelCopy.Controls.Add(this.tableLayoutPanel1);
            this.panelCopy.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCopy.Location = new System.Drawing.Point(0, 70);
            this.panelCopy.Name = "panelCopy";
            this.panelCopy.Size = new System.Drawing.Size(560, 35);
            this.panelCopy.TabIndex = 104;
            this.panelCopy.Visible = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
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
            this.panel2.Controls.Add(this.checkboxKeepModels);
            this.panel2.Controls.Add(this.checkboxKeepSettings);
            this.panel2.Controls.Add(this.checkboxKeepImages);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(224, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(336, 35);
            this.panel2.TabIndex = 88;
            // 
            // checkboxKeepModels
            // 
            this.checkboxKeepModels.AutoSize = true;
            this.checkboxKeepModels.Checked = true;
            this.checkboxKeepModels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkboxKeepModels.Location = new System.Drawing.Point(94, 10);
            this.checkboxKeepModels.Margin = new System.Windows.Forms.Padding(0, 3, 6, 3);
            this.checkboxKeepModels.Name = "checkboxKeepModels";
            this.checkboxKeepModels.Size = new System.Drawing.Size(88, 17);
            this.checkboxKeepModels.TabIndex = 2;
            this.checkboxKeepModels.Text = "Keep Models";
            this.checkboxKeepModels.UseVisualStyleBackColor = true;
            // 
            // checkboxKeepSettings
            // 
            this.checkboxKeepSettings.AutoSize = true;
            this.checkboxKeepSettings.Checked = true;
            this.checkboxKeepSettings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkboxKeepSettings.Location = new System.Drawing.Point(188, 10);
            this.checkboxKeepSettings.Margin = new System.Windows.Forms.Padding(0, 3, 6, 3);
            this.checkboxKeepSettings.Name = "checkboxKeepSettings";
            this.checkboxKeepSettings.Size = new System.Drawing.Size(92, 17);
            this.checkboxKeepSettings.TabIndex = 1;
            this.checkboxKeepSettings.Text = "Keep Settings";
            this.checkboxKeepSettings.UseVisualStyleBackColor = true;
            // 
            // checkboxKeepImages
            // 
            this.checkboxKeepImages.AutoSize = true;
            this.checkboxKeepImages.Checked = true;
            this.checkboxKeepImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkboxKeepImages.Location = new System.Drawing.Point(0, 10);
            this.checkboxKeepImages.Margin = new System.Windows.Forms.Padding(0, 3, 6, 3);
            this.checkboxKeepImages.Name = "checkboxKeepImages";
            this.checkboxKeepImages.Size = new System.Drawing.Size(88, 17);
            this.checkboxKeepImages.TabIndex = 0;
            this.checkboxKeepImages.Text = "Keep Images";
            this.checkboxKeepImages.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(224, 35);
            this.panel3.TabIndex = 87;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(5, 11);
            this.label2.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(172, 13);
            this.label2.TabIndex = 85;
            this.label2.Text = "Keep Files From Current Installation";
            // 
            // panelVersion
            // 
            this.panelVersion.Controls.Add(this.tableLayoutPanel8);
            this.panelVersion.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelVersion.Location = new System.Drawing.Point(0, 35);
            this.panelVersion.Name = "panelVersion";
            this.panelVersion.Size = new System.Drawing.Size(560, 35);
            this.panelVersion.TabIndex = 20;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 2;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
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
            this.panel25.Controls.Add(this.comboxVersion);
            this.panel25.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel25.Location = new System.Drawing.Point(224, 0);
            this.panel25.Margin = new System.Windows.Forms.Padding(0);
            this.panel25.Name = "panel25";
            this.panel25.Size = new System.Drawing.Size(336, 35);
            this.panel25.TabIndex = 88;
            // 
            // comboxVersion
            // 
            this.comboxVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboxVersion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxVersion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxVersion.ForeColor = System.Drawing.Color.White;
            this.comboxVersion.FormattingEnabled = true;
            this.comboxVersion.Location = new System.Drawing.Point(0, 7);
            this.comboxVersion.Name = "comboxVersion";
            this.comboxVersion.Size = new System.Drawing.Size(336, 21);
            this.comboxVersion.TabIndex = 106;
            // 
            // panel26
            // 
            this.panel26.Controls.Add(this.label10);
            this.panel26.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel26.Location = new System.Drawing.Point(0, 0);
            this.panel26.Margin = new System.Windows.Forms.Padding(0);
            this.panel26.Name = "panel26";
            this.panel26.Size = new System.Drawing.Size(224, 35);
            this.panel26.TabIndex = 87;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(5, 11);
            this.label10.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(88, 13);
            this.label10.TabIndex = 85;
            this.label10.Text = "Version To Install";
            // 
            // panelInstalled
            // 
            this.panelInstalled.Controls.Add(this.tableLayoutPanel3);
            this.panelInstalled.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInstalled.Location = new System.Drawing.Point(0, 0);
            this.panelInstalled.Name = "panelInstalled";
            this.panelInstalled.Size = new System.Drawing.Size(560, 35);
            this.panelInstalled.TabIndex = 103;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel3.Controls.Add(this.panel6, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.panel7, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(560, 35);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.labelCurrVersion);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(224, 0);
            this.panel6.Margin = new System.Windows.Forms.Padding(0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(336, 35);
            this.panel6.TabIndex = 88;
            // 
            // labelCurrVersion
            // 
            this.labelCurrVersion.AutoSize = true;
            this.labelCurrVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurrVersion.ForeColor = System.Drawing.Color.White;
            this.labelCurrVersion.Location = new System.Drawing.Point(0, 11);
            this.labelCurrVersion.Name = "labelCurrVersion";
            this.labelCurrVersion.Size = new System.Drawing.Size(16, 13);
            this.labelCurrVersion.TabIndex = 86;
            this.labelCurrVersion.Text = "...";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.label3);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Margin = new System.Windows.Forms.Padding(0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(224, 35);
            this.panel7.TabIndex = 87;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(5, 11);
            this.label3.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 85;
            this.label3.Text = "Installed Version";
            // 
            // btnInstall
            // 
            this.btnInstall.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnInstall.Enabled = false;
            this.btnInstall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInstall.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.btnInstall.ForeColor = System.Drawing.Color.White;
            this.btnInstall.Location = new System.Drawing.Point(0, 147);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(200, 40);
            this.btnInstall.TabIndex = 101;
            this.btnInstall.Text = "Install Selected Version";
            this.btnInstall.UseVisualStyleBackColor = false;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Yu Gothic UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.Location = new System.Drawing.Point(11, 9);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(121, 40);
            this.titleLabel.TabIndex = 2;
            this.titleLabel.Text = "Updater";
            // 
            // UpdaterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(584, 261);
            this.Controls.Add(this.parentPanel);
            this.Controls.Add(this.titleLabel);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UpdaterForm";
            this.Opacity = 0D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Updater";
            this.Load += new System.EventHandler(this.UpdaterForm_Load);
            this.Shown += new System.EventHandler(this.UpdaterForm_Shown);
            this.parentPanel.ResumeLayout(false);
            this.panelCopy.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panelVersion.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.panel25.ResumeLayout(false);
            this.panel26.ResumeLayout(false);
            this.panel26.PerformLayout();
            this.panelInstalled.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Panel parentPanel;
        private System.Windows.Forms.Panel panelInstalled;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.Panel panelVersion;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.Panel panel25;
        private System.Windows.Forms.ComboBox comboxVersion;
        private System.Windows.Forms.Panel panel26;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label labelCurrVersion;
        private System.Windows.Forms.Panel panelCopy;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkboxKeepModels;
        private System.Windows.Forms.CheckBox checkboxKeepSettings;
        private System.Windows.Forms.CheckBox checkboxKeepImages;
    }
}