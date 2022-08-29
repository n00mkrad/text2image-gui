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
            this.SuspendLayout();
            // 
            // btnInstall
            // 
            this.btnInstall.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnInstall.ForeColor = System.Drawing.Color.White;
            this.btnInstall.Location = new System.Drawing.Point(12, 219);
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
            "Stable Diffusion Model File"});
            this.checkedListBoxStatus.Location = new System.Drawing.Point(12, 75);
            this.checkedListBoxStatus.Name = "checkedListBoxStatus";
            this.checkedListBoxStatus.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.checkedListBoxStatus.Size = new System.Drawing.Size(360, 64);
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
            this.btnUninstall.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnUninstall.ForeColor = System.Drawing.Color.White;
            this.btnUninstall.Location = new System.Drawing.Point(195, 219);
            this.btnUninstall.Name = "btnUninstall";
            this.btnUninstall.Size = new System.Drawing.Size(177, 30);
            this.btnUninstall.TabIndex = 1;
            this.btnUninstall.Text = "Uninstall";
            this.btnUninstall.UseVisualStyleBackColor = false;
            this.btnUninstall.Click += new System.EventHandler(this.btnUninstall_Click);
            // 
            // InstallerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(384, 261);
            this.Controls.Add(this.btnUninstall);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkedListBoxStatus);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.btnInstall);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InstallerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Installer";
            this.Load += new System.EventHandler(this.InstallerForm_Load);
            this.Shown += new System.EventHandler(this.InstallerForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.CheckedListBox checkedListBoxStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnUninstall;
    }
}