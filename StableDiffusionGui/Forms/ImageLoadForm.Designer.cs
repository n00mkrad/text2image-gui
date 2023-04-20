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
            this.panelRoot = new System.Windows.Forms.Panel();
            this.tablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.textboxInfo = new StableDiffusionGui.Controls.CustomTextbox();
            this.panelOk = new System.Windows.Forms.Panel();
            this.btnOk = new HTAlt.WinForms.HTButton();
            this.comboxImportAction = new System.Windows.Forms.ComboBox();
            this.panelChromaKey = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.comboxChromaKey = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictBox)).BeginInit();
            this.panelRoot.SuspendLayout();
            this.tablePanel.SuspendLayout();
            this.panelInfo.SuspendLayout();
            this.panelOk.SuspendLayout();
            this.panelChromaKey.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 512F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.pictBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelRoot, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(924, 512);
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
            // panelRoot
            // 
            this.panelRoot.Controls.Add(this.tablePanel);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(512, 0);
            this.panelRoot.Margin = new System.Windows.Forms.Padding(0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Size = new System.Drawing.Size(412, 512);
            this.panelRoot.TabIndex = 2;
            // 
            // tablePanel
            // 
            this.tablePanel.ColumnCount = 1;
            this.tablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tablePanel.Controls.Add(this.panelInfo, 0, 0);
            this.tablePanel.Controls.Add(this.panelOk, 0, 2);
            this.tablePanel.Controls.Add(this.panelChromaKey, 0, 1);
            this.tablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel.Location = new System.Drawing.Point(0, 0);
            this.tablePanel.Margin = new System.Windows.Forms.Padding(0);
            this.tablePanel.Name = "tablePanel";
            this.tablePanel.RowCount = 3;
            this.tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tablePanel.Size = new System.Drawing.Size(412, 512);
            this.tablePanel.TabIndex = 80;
            // 
            // panelInfo
            // 
            this.panelInfo.Controls.Add(this.textboxInfo);
            this.panelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInfo.Location = new System.Drawing.Point(3, 3);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(406, 416);
            this.panelInfo.TabIndex = 114;
            // 
            // textboxInfo
            // 
            this.textboxInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.textboxInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textboxInfo.DisableUnfocusedInput = true;
            this.textboxInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textboxInfo.ForeColor = System.Drawing.Color.Silver;
            this.textboxInfo.Location = new System.Drawing.Point(0, 0);
            this.textboxInfo.MaxTextZoomFactor = 2F;
            this.textboxInfo.Multiline = true;
            this.textboxInfo.Name = "textboxInfo";
            this.textboxInfo.Placeholder = "";
            this.textboxInfo.PlaceholderTextColor = System.Drawing.Color.Silver;
            this.textboxInfo.ReadOnly = true;
            this.textboxInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textboxInfo.Size = new System.Drawing.Size(406, 416);
            this.textboxInfo.TabIndex = 79;
            this.textboxInfo.TabStop = false;
            // 
            // panelOk
            // 
            this.panelOk.Controls.Add(this.btnOk);
            this.panelOk.Controls.Add(this.comboxImportAction);
            this.panelOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOk.Location = new System.Drawing.Point(3, 455);
            this.panelOk.Name = "panelOk";
            this.panelOk.Size = new System.Drawing.Size(406, 54);
            this.panelOk.TabIndex = 112;
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
            this.btnOk.Location = new System.Drawing.Point(0, 31);
            this.btnOk.Margin = new System.Windows.Forms.Padding(0);
            this.btnOk.Name = "btnOk";
            this.btnOk.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnOk.Size = new System.Drawing.Size(406, 23);
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
            this.comboxImportAction.Location = new System.Drawing.Point(0, 1);
            this.comboxImportAction.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.comboxImportAction.Name = "comboxImportAction";
            this.comboxImportAction.Size = new System.Drawing.Size(406, 24);
            this.comboxImportAction.TabIndex = 110;
            // 
            // panelChromaKey
            // 
            this.panelChromaKey.Controls.Add(this.label10);
            this.panelChromaKey.Controls.Add(this.comboxChromaKey);
            this.panelChromaKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChromaKey.Location = new System.Drawing.Point(3, 425);
            this.panelChromaKey.Name = "panelChromaKey";
            this.panelChromaKey.Size = new System.Drawing.Size(406, 24);
            this.panelChromaKey.TabIndex = 113;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(0, 5);
            this.label10.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(191, 13);
            this.label10.TabIndex = 111;
            this.label10.Text = "Transparency From Color (Chroma Key)";
            // 
            // comboxChromaKey
            // 
            this.comboxChromaKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboxChromaKey.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxChromaKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxChromaKey.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxChromaKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.comboxChromaKey.ForeColor = System.Drawing.Color.White;
            this.comboxChromaKey.FormattingEnabled = true;
            this.comboxChromaKey.Location = new System.Drawing.Point(196, 2);
            this.comboxChromaKey.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.comboxChromaKey.Name = "comboxChromaKey";
            this.comboxChromaKey.Size = new System.Drawing.Size(210, 21);
            this.comboxChromaKey.TabIndex = 110;
            // 
            // ImageLoadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(924, 512);
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
            this.panelRoot.ResumeLayout(false);
            this.tablePanel.ResumeLayout(false);
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            this.panelOk.ResumeLayout(false);
            this.panelChromaKey.ResumeLayout(false);
            this.panelChromaKey.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelRoot;
        private StableDiffusionGui.Controls.CustomTextbox textboxInfo;
        public System.Windows.Forms.ComboBox comboxImportAction;
        private HTAlt.WinForms.HTButton btnOk;
        private System.Windows.Forms.Panel panelOk;
        private System.Windows.Forms.TableLayoutPanel tablePanel;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Panel panelChromaKey;
        public System.Windows.Forms.ComboBox comboxChromaKey;
        private System.Windows.Forms.Label label10;
    }
}