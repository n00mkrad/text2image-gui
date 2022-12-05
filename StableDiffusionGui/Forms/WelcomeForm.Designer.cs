namespace StableDiffusionGui.Forms
{
    partial class WelcomeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WelcomeForm));
            this.titleLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOk = new HTAlt.WinForms.HTButton();
            this.panelNews = new System.Windows.Forms.Panel();
            this.newsLabel = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.panelPatrons = new System.Windows.Forms.Panel();
            this.patronsLabel = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnGithub = new System.Windows.Forms.Button();
            this.btnItch = new System.Windows.Forms.Button();
            this.checkboxDoNotShow = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.panelNews.SuspendLayout();
            this.panelPatrons.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
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
            this.titleLabel.Size = new System.Drawing.Size(143, 40);
            this.titleLabel.TabIndex = 12;
            this.titleLabel.Text = "Welcome!";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.checkboxDoNotShow);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Location = new System.Drawing.Point(12, 62);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(963, 490);
            this.panel1.TabIndex = 13;
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
            this.btnOk.Location = new System.Drawing.Point(884, 467);
            this.btnOk.Name = "btnOk";
            this.btnOk.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnOk.Size = new System.Drawing.Size(79, 23);
            this.btnOk.TabIndex = 110;
            this.btnOk.TabStop = false;
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // panelNews
            // 
            this.panelNews.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelNews.AutoScroll = true;
            this.panelNews.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.panelNews.Controls.Add(this.newsLabel);
            this.panelNews.Controls.Add(this.label15);
            this.panelNews.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.panelNews.Location = new System.Drawing.Point(0, 0);
            this.panelNews.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.panelNews.Name = "panelNews";
            this.panelNews.Size = new System.Drawing.Size(640, 460);
            this.panelNews.TabIndex = 4;
            // 
            // newsLabel
            // 
            this.newsLabel.AutoSize = true;
            this.newsLabel.ForeColor = System.Drawing.Color.White;
            this.newsLabel.Location = new System.Drawing.Point(8, 31);
            this.newsLabel.Margin = new System.Windows.Forms.Padding(8, 8, 3, 0);
            this.newsLabel.Name = "newsLabel";
            this.newsLabel.Size = new System.Drawing.Size(0, 16);
            this.newsLabel.TabIndex = 8;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(8, 8);
            this.label15.Margin = new System.Windows.Forms.Padding(8, 8, 3, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(45, 17);
            this.label15.TabIndex = 7;
            this.label15.Text = "News:";
            // 
            // panelPatrons
            // 
            this.panelPatrons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPatrons.AutoScroll = true;
            this.panelPatrons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.panelPatrons.Controls.Add(this.patronsLabel);
            this.panelPatrons.Controls.Add(this.label21);
            this.panelPatrons.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.panelPatrons.Location = new System.Drawing.Point(650, 0);
            this.panelPatrons.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.panelPatrons.Name = "panelPatrons";
            this.panelPatrons.Size = new System.Drawing.Size(313, 460);
            this.panelPatrons.TabIndex = 5;
            // 
            // patronsLabel
            // 
            this.patronsLabel.AutoSize = true;
            this.patronsLabel.ForeColor = System.Drawing.Color.White;
            this.patronsLabel.Location = new System.Drawing.Point(8, 31);
            this.patronsLabel.Margin = new System.Windows.Forms.Padding(8, 8, 3, 0);
            this.patronsLabel.Name = "patronsLabel";
            this.patronsLabel.Size = new System.Drawing.Size(0, 16);
            this.patronsLabel.TabIndex = 9;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.Color.White;
            this.label21.Location = new System.Drawing.Point(8, 8);
            this.label21.Margin = new System.Windows.Forms.Padding(8, 8, 3, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(131, 17);
            this.label21.TabIndex = 8;
            this.label21.Text = "Patreon Supporters:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel1.Controls.Add(this.panelNews, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelPatrons, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 62);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(963, 460);
            this.tableLayoutPanel1.TabIndex = 111;
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 200;
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 200;
            this.toolTip.ReshowDelay = 40;
            // 
            // btnGithub
            // 
            this.btnGithub.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGithub.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnGithub.BackgroundImage = global::StableDiffusionGui.Properties.Resources.github;
            this.btnGithub.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnGithub.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGithub.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGithub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnGithub.Location = new System.Drawing.Point(889, 9);
            this.btnGithub.Name = "btnGithub";
            this.btnGithub.Size = new System.Drawing.Size(40, 40);
            this.btnGithub.TabIndex = 113;
            this.btnGithub.TabStop = false;
            this.toolTip.SetToolTip(this.btnGithub, "Open Github repository");
            this.btnGithub.UseVisualStyleBackColor = false;
            this.btnGithub.Click += new System.EventHandler(this.btnGithub_Click);
            // 
            // btnItch
            // 
            this.btnItch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnItch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnItch.BackgroundImage = global::StableDiffusionGui.Properties.Resources.itch2;
            this.btnItch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnItch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnItch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnItch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnItch.Location = new System.Drawing.Point(935, 9);
            this.btnItch.Name = "btnItch";
            this.btnItch.Size = new System.Drawing.Size(40, 40);
            this.btnItch.TabIndex = 112;
            this.btnItch.TabStop = false;
            this.toolTip.SetToolTip(this.btnItch, "Open itch.io page");
            this.btnItch.UseVisualStyleBackColor = false;
            this.btnItch.Click += new System.EventHandler(this.btnItch_Click);
            // 
            // checkboxDoNotShow
            // 
            this.checkboxDoNotShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkboxDoNotShow.AutoSize = true;
            this.checkboxDoNotShow.ForeColor = System.Drawing.Color.Silver;
            this.checkboxDoNotShow.Location = new System.Drawing.Point(0, 473);
            this.checkboxDoNotShow.Name = "checkboxDoNotShow";
            this.checkboxDoNotShow.Size = new System.Drawing.Size(196, 17);
            this.checkboxDoNotShow.TabIndex = 111;
            this.checkboxDoNotShow.Text = "Do Not Display This Message Again";
            this.checkboxDoNotShow.UseVisualStyleBackColor = true;
            this.checkboxDoNotShow.Visible = false;
            // 
            // WelcomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.btnGithub);
            this.Controls.Add(this.btnItch);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.titleLabel);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(750, 450);
            this.Name = "WelcomeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Welcome";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WelcomeForm_FormClosing);
            this.Load += new System.EventHandler(this.WelcomeForm_Load);
            this.Shown += new System.EventHandler(this.WelcomeForm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WelcomeForm_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelNews.ResumeLayout(false);
            this.panelNews.PerformLayout();
            this.panelPatrons.ResumeLayout(false);
            this.panelPatrons.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelNews;
        private System.Windows.Forms.Label newsLabel;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Panel panelPatrons;
        private System.Windows.Forms.Label patronsLabel;
        private System.Windows.Forms.Label label21;
        private HTAlt.WinForms.HTButton btnOk;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnItch;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button btnGithub;
        private System.Windows.Forms.CheckBox checkboxDoNotShow;
    }
}