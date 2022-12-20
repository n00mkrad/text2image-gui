namespace StableDiffusionGui.Forms
{
    partial class DrawForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DrawForm));
            this.menuStripOptions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invertMaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteMaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelBlur = new System.Windows.Forms.Panel();
            this.sliderBlur = new HTAlt.WinForms.HTSlider();
            this.labelBlurNotAvailable = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panelBrushSize = new System.Windows.Forms.Panel();
            this.sliderBrushSize = new HTAlt.WinForms.HTSlider();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnMaskLoad = new HTAlt.WinForms.HTButton();
            this.btnMaskSave = new HTAlt.WinForms.HTButton();
            this.btnClear = new HTAlt.WinForms.HTButton();
            this.btnOk = new HTAlt.WinForms.HTButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelImg = new System.Windows.Forms.TableLayoutPanel();
            this.pictBox = new System.Windows.Forms.PictureBox();
            this.menuStripOptions.SuspendLayout();
            this.panelBlur.SuspendLayout();
            this.panelBrushSize.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanelImg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStripOptions
            // 
            this.menuStripOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem,
            this.invertMaskToolStripMenuItem,
            this.pasteMaskToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.undoToolStripMenuItem});
            this.menuStripOptions.Name = "menuStripOptions";
            this.menuStripOptions.Size = new System.Drawing.Size(149, 136);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // invertMaskToolStripMenuItem
            // 
            this.invertMaskToolStripMenuItem.Name = "invertMaskToolStripMenuItem";
            this.invertMaskToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.invertMaskToolStripMenuItem.Text = "Invert (Ctrl+I)";
            this.invertMaskToolStripMenuItem.Click += new System.EventHandler(this.invertMaskToolStripMenuItem_Click);
            // 
            // pasteMaskToolStripMenuItem
            // 
            this.pasteMaskToolStripMenuItem.Name = "pasteMaskToolStripMenuItem";
            this.pasteMaskToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.pasteMaskToolStripMenuItem.Text = "Paste (Ctrl+V)";
            this.pasteMaskToolStripMenuItem.Click += new System.EventHandler(this.pasteMaskToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.saveToolStripMenuItem.Text = "Save (Ctrl+S)";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.loadToolStripMenuItem.Text = "Load (Ctrl+O)";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.undoToolStripMenuItem.Text = "Undo (Ctrl+Z)";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // panelBlur
            // 
            this.panelBlur.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBlur.Controls.Add(this.sliderBlur);
            this.panelBlur.Controls.Add(this.labelBlurNotAvailable);
            this.panelBlur.Controls.Add(this.label1);
            this.panelBlur.Location = new System.Drawing.Point(0, 453);
            this.panelBlur.Margin = new System.Windows.Forms.Padding(0);
            this.panelBlur.Name = "panelBlur";
            this.panelBlur.Size = new System.Drawing.Size(725, 35);
            this.panelBlur.TabIndex = 4;
            // 
            // sliderBlur
            // 
            this.sliderBlur.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.sliderBlur.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.sliderBlur.BorderRoundRectSize = new System.Drawing.Size(12, 12);
            this.sliderBlur.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.sliderBlur.ForeColor = System.Drawing.Color.Black;
            this.sliderBlur.LargeChange = ((uint)(5u));
            this.sliderBlur.Location = new System.Drawing.Point(100, 7);
            this.sliderBlur.Margin = new System.Windows.Forms.Padding(0);
            this.sliderBlur.Maximum = 15;
            this.sliderBlur.Name = "sliderBlur";
            this.sliderBlur.OverlayColor = System.Drawing.Color.White;
            this.sliderBlur.Size = new System.Drawing.Size(614, 21);
            this.sliderBlur.SmallChange = ((uint)(1u));
            this.sliderBlur.TabIndex = 4;
            this.sliderBlur.ThumbRoundRectSize = new System.Drawing.Size(16, 16);
            this.sliderBlur.ThumbSize = new System.Drawing.Size(14, 14);
            this.sliderBlur.Value = 5;
            this.sliderBlur.Scroll += new System.Windows.Forms.ScrollEventHandler(this.sliderBlur_Scroll);
            // 
            // labelBlurNotAvailable
            // 
            this.labelBlurNotAvailable.AutoSize = true;
            this.labelBlurNotAvailable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBlurNotAvailable.ForeColor = System.Drawing.Color.Silver;
            this.labelBlurNotAvailable.Location = new System.Drawing.Point(107, 10);
            this.labelBlurNotAvailable.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.labelBlurNotAvailable.Name = "labelBlurNotAvailable";
            this.labelBlurNotAvailable.Size = new System.Drawing.Size(70, 13);
            this.labelBlurNotAvailable.TabIndex = 90;
            this.labelBlurNotAvailable.Text = "Not Available";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(5, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 89;
            this.label1.Text = "Mask Blur";
            // 
            // panelBrushSize
            // 
            this.panelBrushSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBrushSize.Controls.Add(this.sliderBrushSize);
            this.panelBrushSize.Controls.Add(this.label3);
            this.panelBrushSize.Location = new System.Drawing.Point(0, 418);
            this.panelBrushSize.Margin = new System.Windows.Forms.Padding(0);
            this.panelBrushSize.Name = "panelBrushSize";
            this.panelBrushSize.Size = new System.Drawing.Size(725, 35);
            this.panelBrushSize.TabIndex = 3;
            // 
            // sliderBrushSize
            // 
            this.sliderBrushSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.sliderBrushSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.sliderBrushSize.BorderRoundRectSize = new System.Drawing.Size(12, 12);
            this.sliderBrushSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.sliderBrushSize.ForeColor = System.Drawing.Color.Black;
            this.sliderBrushSize.LargeChange = ((uint)(5u));
            this.sliderBrushSize.Location = new System.Drawing.Point(100, 7);
            this.sliderBrushSize.Margin = new System.Windows.Forms.Padding(0);
            this.sliderBrushSize.Maximum = 35;
            this.sliderBrushSize.Minimum = 5;
            this.sliderBrushSize.Name = "sliderBrushSize";
            this.sliderBrushSize.OverlayColor = System.Drawing.Color.White;
            this.sliderBrushSize.Size = new System.Drawing.Size(614, 21);
            this.sliderBrushSize.SmallChange = ((uint)(1u));
            this.sliderBrushSize.TabIndex = 4;
            this.sliderBrushSize.ThumbRoundRectSize = new System.Drawing.Size(16, 16);
            this.sliderBrushSize.ThumbSize = new System.Drawing.Size(14, 14);
            this.sliderBrushSize.Value = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(5, 10);
            this.label3.Margin = new System.Windows.Forms.Padding(8, 0, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 89;
            this.label3.Text = "Brush Size";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btnMaskLoad);
            this.panel1.Controls.Add(this.btnMaskSave);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Location = new System.Drawing.Point(0, 488);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(725, 35);
            this.panel1.TabIndex = 5;
            // 
            // btnMaskLoad
            // 
            this.btnMaskLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMaskLoad.AutoColor = true;
            this.btnMaskLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnMaskLoad.ButtonImage = null;
            this.btnMaskLoad.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnMaskLoad.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnMaskLoad.DrawImage = false;
            this.btnMaskLoad.ForeColor = System.Drawing.Color.White;
            this.btnMaskLoad.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.btnMaskLoad.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnMaskLoad.Location = new System.Drawing.Point(176, 6);
            this.btnMaskLoad.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnMaskLoad.Name = "btnMaskLoad";
            this.btnMaskLoad.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnMaskLoad.Size = new System.Drawing.Size(79, 23);
            this.btnMaskLoad.TabIndex = 112;
            this.btnMaskLoad.TabStop = false;
            this.btnMaskLoad.Text = "Load Mask";
            this.btnMaskLoad.Click += new System.EventHandler(this.btnMaskLoad_Click);
            // 
            // btnMaskSave
            // 
            this.btnMaskSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMaskSave.AutoColor = true;
            this.btnMaskSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnMaskSave.ButtonImage = null;
            this.btnMaskSave.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnMaskSave.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnMaskSave.DrawImage = false;
            this.btnMaskSave.ForeColor = System.Drawing.Color.White;
            this.btnMaskSave.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.btnMaskSave.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnMaskSave.Location = new System.Drawing.Point(91, 6);
            this.btnMaskSave.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnMaskSave.Name = "btnMaskSave";
            this.btnMaskSave.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnMaskSave.Size = new System.Drawing.Size(79, 23);
            this.btnMaskSave.TabIndex = 111;
            this.btnMaskSave.TabStop = false;
            this.btnMaskSave.Text = "Save Mask";
            this.btnMaskSave.Click += new System.EventHandler(this.btnMaskSave_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClear.AutoColor = true;
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnClear.ButtonImage = null;
            this.btnClear.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnClear.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnClear.DrawImage = false;
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.btnClear.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnClear.Location = new System.Drawing.Point(6, 6);
            this.btnClear.Margin = new System.Windows.Forms.Padding(6, 6, 3, 6);
            this.btnClear.Name = "btnClear";
            this.btnClear.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnClear.Size = new System.Drawing.Size(79, 23);
            this.btnClear.TabIndex = 110;
            this.btnClear.TabStop = false;
            this.btnClear.Text = "Clear Mask";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.AutoColor = true;
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnOk.ButtonImage = null;
            this.btnOk.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnOk.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnOk.DrawImage = false;
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.btnOk.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnOk.Location = new System.Drawing.Point(640, 6);
            this.btnOk.Margin = new System.Windows.Forms.Padding(6);
            this.btnOk.Name = "btnOk";
            this.btnOk.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnOk.Size = new System.Drawing.Size(79, 23);
            this.btnOk.TabIndex = 109;
            this.btnOk.TabStop = false;
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelBlur, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panelBrushSize, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanelImg, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(725, 523);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // tableLayoutPanelImg
            // 
            this.tableLayoutPanelImg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.tableLayoutPanelImg.ColumnCount = 3;
            this.tableLayoutPanelImg.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanelImg.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelImg.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanelImg.Controls.Add(this.pictBox, 1, 1);
            this.tableLayoutPanelImg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelImg.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelImg.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelImg.Name = "tableLayoutPanelImg";
            this.tableLayoutPanelImg.RowCount = 3;
            this.tableLayoutPanelImg.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanelImg.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelImg.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanelImg.Size = new System.Drawing.Size(725, 418);
            this.tableLayoutPanelImg.TabIndex = 6;
            // 
            // pictBox
            // 
            this.pictBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.pictBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictBox.Location = new System.Drawing.Point(0, 0);
            this.pictBox.Margin = new System.Windows.Forms.Padding(0);
            this.pictBox.Name = "pictBox";
            this.pictBox.Size = new System.Drawing.Size(725, 418);
            this.pictBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictBox.TabIndex = 0;
            this.pictBox.TabStop = false;
            this.pictBox.Click += new System.EventHandler(this.pictBox_Click);
            this.pictBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictBox_MouseDown);
            this.pictBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictBox_MouseMove);
            this.pictBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictBox_MouseUp);
            // 
            // DrawForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(725, 523);
            this.Controls.Add(this.tableLayoutPanel1);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "DrawForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Draw Mask";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DrawForm_FormClosing);
            this.Load += new System.EventHandler(this.DrawForm_Load);
            this.Shown += new System.EventHandler(this.DrawForm_Shown);
            this.SizeChanged += new System.EventHandler(this.DrawForm_SizeChanged);
            this.menuStripOptions.ResumeLayout(false);
            this.panelBlur.ResumeLayout(false);
            this.panelBlur.PerformLayout();
            this.panelBrushSize.ResumeLayout(false);
            this.panelBrushSize.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanelImg.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.ContextMenuStrip menuStripOptions;
        public System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        public System.Windows.Forms.Panel panelBrushSize;
        public System.Windows.Forms.Label label3;
        public HTAlt.WinForms.HTSlider sliderBrushSize;
        public System.Windows.Forms.Panel panelBlur;
        public HTAlt.WinForms.HTSlider sliderBlur;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Panel panel1;
        public HTAlt.WinForms.HTButton btnOk;
        public System.Windows.Forms.ToolStripMenuItem invertMaskToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem pasteMaskToolStripMenuItem;
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public System.Windows.Forms.PictureBox pictBox;
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanelImg;
        public System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        public HTAlt.WinForms.HTButton btnMaskLoad;
        public HTAlt.WinForms.HTButton btnMaskSave;
        public HTAlt.WinForms.HTButton btnClear;
        public System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        public System.Windows.Forms.Label labelBlurNotAvailable;
    }
}