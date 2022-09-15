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
            this.pictBox = new System.Windows.Forms.PictureBox();
            this.menuStripOptions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelBlur = new System.Windows.Forms.Panel();
            this.sliderBlur = new HTAlt.WinForms.HTSlider();
            this.label1 = new System.Windows.Forms.Label();
            this.panelBrushSize = new System.Windows.Forms.Panel();
            this.sliderBrushSize = new HTAlt.WinForms.HTSlider();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOk = new HTAlt.WinForms.HTButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictBox)).BeginInit();
            this.menuStripOptions.SuspendLayout();
            this.panelBlur.SuspendLayout();
            this.panelBrushSize.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictBox
            // 
            this.pictBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictBox.Location = new System.Drawing.Point(0, 0);
            this.pictBox.Margin = new System.Windows.Forms.Padding(0);
            this.pictBox.Name = "pictBox";
            this.pictBox.Size = new System.Drawing.Size(704, 371);
            this.pictBox.TabIndex = 0;
            this.pictBox.TabStop = false;
            this.pictBox.Click += new System.EventHandler(this.pictBox_Click);
            this.pictBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictBox_MouseDown);
            this.pictBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictBox_MouseMove);
            this.pictBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictBox_MouseUp);
            // 
            // menuStripOptions
            // 
            this.menuStripOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem});
            this.menuStripOptions.Name = "menuStripOptions";
            this.menuStripOptions.Size = new System.Drawing.Size(102, 26);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // panelBlur
            // 
            this.panelBlur.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBlur.Controls.Add(this.sliderBlur);
            this.panelBlur.Controls.Add(this.label1);
            this.panelBlur.Location = new System.Drawing.Point(0, 371);
            this.panelBlur.Margin = new System.Windows.Forms.Padding(0);
            this.panelBlur.Name = "panelBlur";
            this.panelBlur.Size = new System.Drawing.Size(706, 35);
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
            this.sliderBlur.Size = new System.Drawing.Size(595, 21);
            this.sliderBlur.SmallChange = ((uint)(1u));
            this.sliderBlur.TabIndex = 4;
            this.sliderBlur.ThumbRoundRectSize = new System.Drawing.Size(16, 16);
            this.sliderBlur.ThumbSize = new System.Drawing.Size(14, 14);
            this.sliderBlur.Value = 5;
            this.sliderBlur.Scroll += new System.Windows.Forms.ScrollEventHandler(this.sliderBlur_Scroll);
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
            this.panelBrushSize.Location = new System.Drawing.Point(0, 336);
            this.panelBrushSize.Margin = new System.Windows.Forms.Padding(0);
            this.panelBrushSize.Name = "panelBrushSize";
            this.panelBrushSize.Size = new System.Drawing.Size(706, 35);
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
            this.sliderBrushSize.Size = new System.Drawing.Size(595, 21);
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
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Location = new System.Drawing.Point(0, 406);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(706, 35);
            this.panel1.TabIndex = 5;
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
            this.btnOk.Location = new System.Drawing.Point(618, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnOk.Size = new System.Drawing.Size(79, 23);
            this.btnOk.TabIndex = 109;
            this.btnOk.TabStop = false;
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // DrawForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(704, 441);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelBlur);
            this.Controls.Add(this.panelBrushSize);
            this.Controls.Add(this.pictBox);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DrawForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Draw Mask";
            this.Load += new System.EventHandler(this.DrawForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictBox)).EndInit();
            this.menuStripOptions.ResumeLayout(false);
            this.panelBlur.ResumeLayout(false);
            this.panelBlur.PerformLayout();
            this.panelBrushSize.ResumeLayout(false);
            this.panelBrushSize.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictBox;
        private System.Windows.Forms.ContextMenuStrip menuStripOptions;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.Panel panelBrushSize;
        private System.Windows.Forms.Label label3;
        private HTAlt.WinForms.HTSlider sliderBrushSize;
        private System.Windows.Forms.Panel panelBlur;
        private HTAlt.WinForms.HTSlider sliderBlur;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private HTAlt.WinForms.HTButton btnOk;
    }
}