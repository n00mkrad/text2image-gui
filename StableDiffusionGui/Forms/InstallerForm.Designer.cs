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
            this.installBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // installBtn
            // 
            this.installBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.installBtn.ForeColor = System.Drawing.Color.White;
            this.installBtn.Location = new System.Drawing.Point(12, 12);
            this.installBtn.Name = "installBtn";
            this.installBtn.Size = new System.Drawing.Size(75, 23);
            this.installBtn.TabIndex = 1;
            this.installBtn.Text = "Install";
            this.installBtn.UseVisualStyleBackColor = false;
            this.installBtn.Click += new System.EventHandler(this.installBtn_Click);
            // 
            // InstallerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(384, 261);
            this.Controls.Add(this.installBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InstallerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SD Installer";
            this.Load += new System.EventHandler(this.InstallerForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button installBtn;
    }
}