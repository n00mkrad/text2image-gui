namespace StableDiffusionGui.Forms
{
    partial class ImageLoadForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageLoadForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictBox = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOk = new HTAlt.WinForms.HTButton();
            this.comboxImportAction = new System.Windows.Forms.ComboBox();
            this.textboxInfo = new StableDiffusionGui.Controls.CustomTextbox();
            this.btnInitImage = new HTAlt.WinForms.HTButton();
            this.btnLoadSettings = new HTAlt.WinForms.HTButton();
            this.btnCopyPrompt = new HTAlt.WinForms.HTButton();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 512F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.pictBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(884, 512);
            this.tableLayoutPanel1.TabIndex = 111;
            // 
            // pictBox
            // 
            this.pictBox.BackgroundImage = global::StableDiffusionGui.Properties.Resources.checkerboard;
            this.pictBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictBox.Location = new System.Drawing.Point(0, 0);
            this.pictBox.Margin = new System.Windows.Forms.Padding(0);
            this.pictBox.Name = "pictBox";
            this.pictBox.Size = new System.Drawing.Size(512, 512);
            this.pictBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictBox.TabIndex = 1;
            this.pictBox.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.comboxImportAction);
            this.panel1.Controls.Add(this.textboxInfo);
            this.panel1.Controls.Add(this.btnInitImage);
            this.panel1.Controls.Add(this.btnLoadSettings);
            this.panel1.Controls.Add(this.btnCopyPrompt);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(512, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(372, 512);
            this.panel1.TabIndex = 2;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.AutoColor = true;
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnOk.ButtonImage = null;
            this.btnOk.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnOk.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnOk.DrawImage = false;
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.btnOk.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnOk.Location = new System.Drawing.Point(3, 486);
            this.btnOk.Name = "btnOk";
            this.btnOk.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnOk.Size = new System.Drawing.Size(366, 23);
            this.btnOk.TabIndex = 111;
            this.btnOk.TabStop = false;
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // comboxImportAction
            // 
            this.comboxImportAction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboxImportAction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxImportAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxImportAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxImportAction.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboxImportAction.ForeColor = System.Drawing.Color.White;
            this.comboxImportAction.FormattingEnabled = true;
            this.comboxImportAction.Location = new System.Drawing.Point(3, 456);
            this.comboxImportAction.Name = "comboxImportAction";
            this.comboxImportAction.Size = new System.Drawing.Size(366, 24);
            this.comboxImportAction.TabIndex = 110;
            // 
            // textboxInfo
            // 
            this.textboxInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.textboxInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textboxInfo.ForeColor = System.Drawing.Color.Silver;
            this.textboxInfo.Location = new System.Drawing.Point(3, 3);
            this.textboxInfo.Multiline = true;
            this.textboxInfo.Name = "textboxInfo";
            this.textboxInfo.ReadOnly = true;
            this.textboxInfo.Size = new System.Drawing.Size(366, 447);
            this.textboxInfo.TabIndex = 79;
            this.textboxInfo.TabStop = false;
            // 
            // btnInitImage
            // 
            this.btnInitImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInitImage.AutoColor = true;
            this.btnInitImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnInitImage.ButtonImage = null;
            this.btnInitImage.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnInitImage.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnInitImage.DrawImage = false;
            this.btnInitImage.ForeColor = System.Drawing.Color.White;
            this.btnInitImage.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.btnInitImage.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnInitImage.Location = new System.Drawing.Point(363, 353);
            this.btnInitImage.Name = "btnInitImage";
            this.btnInitImage.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnInitImage.Size = new System.Drawing.Size(366, 23);
            this.btnInitImage.TabIndex = 4;
            this.btnInitImage.TabStop = false;
            this.btnInitImage.Text = "Use as Initialization Image";
            this.btnInitImage.Visible = false;
            this.btnInitImage.Click += new System.EventHandler(this.btnInitImage_Click);
            // 
            // btnLoadSettings
            // 
            this.btnLoadSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadSettings.AutoColor = true;
            this.btnLoadSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.btnLoadSettings.ButtonImage = null;
            this.btnLoadSettings.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnLoadSettings.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btnLoadSettings.DrawImage = false;
            this.btnLoadSettings.Enabled = false;
            this.btnLoadSettings.ForeColor = System.Drawing.Color.White;
            this.btnLoadSettings.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnLoadSettings.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnLoadSettings.Location = new System.Drawing.Point(363, 382);
            this.btnLoadSettings.Name = "btnLoadSettings";
            this.btnLoadSettings.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.btnLoadSettings.Size = new System.Drawing.Size(366, 23);
            this.btnLoadSettings.TabIndex = 3;
            this.btnLoadSettings.TabStop = false;
            this.btnLoadSettings.Text = "Load Settings From Metadata";
            this.btnLoadSettings.Visible = false;
            this.btnLoadSettings.Click += new System.EventHandler(this.btnLoadSettings_Click);
            // 
            // btnCopyPrompt
            // 
            this.btnCopyPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyPrompt.AutoColor = true;
            this.btnCopyPrompt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.btnCopyPrompt.ButtonImage = null;
            this.btnCopyPrompt.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnCopyPrompt.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.btnCopyPrompt.DrawImage = false;
            this.btnCopyPrompt.Enabled = false;
            this.btnCopyPrompt.ForeColor = System.Drawing.Color.White;
            this.btnCopyPrompt.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnCopyPrompt.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnCopyPrompt.Location = new System.Drawing.Point(363, 411);
            this.btnCopyPrompt.Name = "btnCopyPrompt";
            this.btnCopyPrompt.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.btnCopyPrompt.Size = new System.Drawing.Size(366, 23);
            this.btnCopyPrompt.TabIndex = 2;
            this.btnCopyPrompt.TabStop = false;
            this.btnCopyPrompt.Text = "Copy Prompt to Clipboard";
            this.btnCopyPrompt.Visible = false;
            this.btnCopyPrompt.Click += new System.EventHandler(this.btnCopyPrompt_Click);
            // 
            // ImageLoadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(884, 512);
            this.Controls.Add(this.tableLayoutPanel1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImageLoadForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Load Image";
            this.Load += new System.EventHandler(this.ImageLoadForm_Load);
            this.Shown += new System.EventHandler(this.ImageLoadForm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ImageLoadForm_KeyDown);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private HTAlt.WinForms.HTButton btnInitImage;
        private HTAlt.WinForms.HTButton btnLoadSettings;
        private HTAlt.WinForms.HTButton btnCopyPrompt;
        private StableDiffusionGui.Controls.CustomTextbox textboxInfo;
        public System.Windows.Forms.ComboBox comboxImportAction;
        private HTAlt.WinForms.HTButton btnOk;
    }
}