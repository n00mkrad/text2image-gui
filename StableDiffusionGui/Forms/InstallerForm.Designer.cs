namespace StableDiffusionGui.Forms
{
    partial class InstallerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallerForm));
            this.btnInstall = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.checkedListBoxStatus = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnUninstall = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnInstUpscalers = new HTAlt.WinForms.HTButton();
            this.btnRedownloadModel = new HTAlt.WinForms.HTButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnInstall
            // 
            this.btnInstall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnInstall.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnInstall.ForeColor = System.Drawing.Color.White;
            this.btnInstall.Location = new System.Drawing.Point(12, 217);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(177, 30);
            this.btnInstall.TabIndex = 0;
            this.btnInstall.Text = "Install";
            this.btnInstall.UseVisualStyleBackColor = false;
            this.btnInstall.Click += new System.EventHandler(this.installBtn_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Yu Gothic UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.Location = new System.Drawing.Point(11, 9);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(117, 40);
            this.titleLabel.TabIndex = 12;
            this.titleLabel.Text = "Installer";
            // 
            // checkedListBoxStatus
            // 
            this.checkedListBoxStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.checkedListBoxStatus.ForeColor = System.Drawing.Color.White;
            this.checkedListBoxStatus.FormattingEnabled = true;
            this.checkedListBoxStatus.Items.AddRange(new object[] {
            "Miniconda (included)",
            "Stable Diffusion Repository",
            "Python Environment",
            "Stable Diffusion Model File",
            "Upscalers (Optional)"});
            this.checkedListBoxStatus.Location = new System.Drawing.Point(12, 75);
            this.checkedListBoxStatus.Name = "checkedListBoxStatus";
            this.checkedListBoxStatus.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.checkedListBoxStatus.Size = new System.Drawing.Size(360, 79);
            this.checkedListBoxStatus.TabIndex = 13;
            this.checkedListBoxStatus.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(9, 59);
            this.label2.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 84;
            this.label2.Text = "Installation Status:";
            // 
            // btnUninstall
            // 
            this.btnUninstall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUninstall.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnUninstall.ForeColor = System.Drawing.Color.White;
            this.btnUninstall.Location = new System.Drawing.Point(195, 217);
            this.btnUninstall.Name = "btnUninstall";
            this.btnUninstall.Size = new System.Drawing.Size(177, 30);
            this.btnUninstall.TabIndex = 1;
            this.btnUninstall.Text = "Uninstall";
            this.btnUninstall.UseVisualStyleBackColor = false;
            this.btnUninstall.Click += new System.EventHandler(this.btnUninstall_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnInstUpscalers);
            this.groupBox1.Controls.Add(this.btnRedownloadModel);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 163);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 48);
            this.groupBox1.TabIndex = 85;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Debug Tools";
            // 
            // btnInstUpscalers
            // 
            this.btnInstUpscalers.AutoColor = true;
            this.btnInstUpscalers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnInstUpscalers.ButtonImage = null;
            this.btnInstUpscalers.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnInstUpscalers.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnInstUpscalers.DrawImage = false;
            this.btnInstUpscalers.ForeColor = System.Drawing.Color.White;
            this.btnInstUpscalers.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.btnInstUpscalers.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnInstUpscalers.Location = new System.Drawing.Point(183, 19);
            this.btnInstUpscalers.Name = "btnInstUpscalers";
            this.btnInstUpscalers.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnInstUpscalers.Size = new System.Drawing.Size(171, 21);
            this.btnInstUpscalers.TabIndex = 3;
            this.btnInstUpscalers.TabStop = false;
            this.btnInstUpscalers.Text = "Install Upscalers";
            this.btnInstUpscalers.Click += new System.EventHandler(this.btnPatch_Click);
            // 
            // btnRedownloadModel
            // 
            this.btnRedownloadModel.AutoColor = true;
            this.btnRedownloadModel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnRedownloadModel.ButtonImage = null;
            this.btnRedownloadModel.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnRedownloadModel.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnRedownloadModel.DrawImage = false;
            this.btnRedownloadModel.ForeColor = System.Drawing.Color.White;
            this.btnRedownloadModel.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.btnRedownloadModel.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnRedownloadModel.Location = new System.Drawing.Point(6, 19);
            this.btnRedownloadModel.Name = "btnRedownloadModel";
            this.btnRedownloadModel.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnRedownloadModel.Size = new System.Drawing.Size(171, 21);
            this.btnRedownloadModel.TabIndex = 2;
            this.btnRedownloadModel.TabStop = false;
            this.btnRedownloadModel.Text = "Redownload SD Model";
            this.btnRedownloadModel.Click += new System.EventHandler(this.btnRedownloadModel_Click);
            // 
            // InstallerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(384, 259);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnUninstall);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkedListBoxStatus);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.btnInstall);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InstallerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Installer";
            this.Load += new System.EventHandler(this.InstallerForm_Load);
            this.Shown += new System.EventHandler(this.InstallerForm_Shown);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.CheckedListBox checkedListBoxStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnUninstall;
        private System.Windows.Forms.GroupBox groupBox1;
        private HTAlt.WinForms.HTButton btnRedownloadModel;
        private HTAlt.WinForms.HTButton btnInstUpscalers;
    }
}