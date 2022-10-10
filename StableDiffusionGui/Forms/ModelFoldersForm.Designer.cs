namespace StableDiffusionGui.Forms
{
    partial class ModelFoldersForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelFoldersForm));
            this.titleLabel = new System.Windows.Forms.Label();
            this.folderListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.menuStripDelete = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripPromptHistory = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.loadPromptIntoGUIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadPromptAndSettingsIntoGUIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyPromptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOpenSelectedFolder = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.menuStripDelete.SuspendLayout();
            this.menuStripPromptHistory.SuspendLayout();
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
            this.titleLabel.Size = new System.Drawing.Size(198, 40);
            this.titleLabel.TabIndex = 12;
            this.titleLabel.Text = "Model Folders";
            // 
            // folderListView
            // 
            this.folderListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.folderListView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.folderListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.folderListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.folderListView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.folderListView.ForeColor = System.Drawing.Color.White;
            this.folderListView.FullRowSelect = true;
            this.folderListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.folderListView.HideSelection = false;
            this.folderListView.LabelWrap = false;
            this.folderListView.Location = new System.Drawing.Point(12, 62);
            this.folderListView.Name = "folderListView";
            this.folderListView.Size = new System.Drawing.Size(760, 187);
            this.folderListView.TabIndex = 53;
            this.folderListView.UseCompatibleStateImageBehavior = false;
            this.folderListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 760;
            // 
            // menuStripDelete
            // 
            this.menuStripDelete.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteSelectedToolStripMenuItem,
            this.deleteAllToolStripMenuItem});
            this.menuStripDelete.Name = "contextMenuDelete";
            this.menuStripDelete.Size = new System.Drawing.Size(155, 48);
            // 
            // deleteSelectedToolStripMenuItem
            // 
            this.deleteSelectedToolStripMenuItem.Name = "deleteSelectedToolStripMenuItem";
            this.deleteSelectedToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.deleteSelectedToolStripMenuItem.Text = "Delete Selected";
            // 
            // deleteAllToolStripMenuItem
            // 
            this.deleteAllToolStripMenuItem.Name = "deleteAllToolStripMenuItem";
            this.deleteAllToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.deleteAllToolStripMenuItem.Text = "Delete All";
            // 
            // menuStripPromptHistory
            // 
            this.menuStripPromptHistory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadPromptIntoGUIToolStripMenuItem,
            this.loadPromptAndSettingsIntoGUIToolStripMenuItem,
            this.copyPromptToolStripMenuItem});
            this.menuStripPromptHistory.Name = "menuStripPromptHistory";
            this.menuStripPromptHistory.Size = new System.Drawing.Size(260, 70);
            // 
            // loadPromptIntoGUIToolStripMenuItem
            // 
            this.loadPromptIntoGUIToolStripMenuItem.Name = "loadPromptIntoGUIToolStripMenuItem";
            this.loadPromptIntoGUIToolStripMenuItem.Size = new System.Drawing.Size(259, 22);
            this.loadPromptIntoGUIToolStripMenuItem.Text = "Load Prompt Into GUI";
            // 
            // loadPromptAndSettingsIntoGUIToolStripMenuItem
            // 
            this.loadPromptAndSettingsIntoGUIToolStripMenuItem.Name = "loadPromptAndSettingsIntoGUIToolStripMenuItem";
            this.loadPromptAndSettingsIntoGUIToolStripMenuItem.Size = new System.Drawing.Size(259, 22);
            this.loadPromptAndSettingsIntoGUIToolStripMenuItem.Text = "Load Prompt And Settings Into GUI";
            // 
            // copyPromptToolStripMenuItem
            // 
            this.copyPromptToolStripMenuItem.Name = "copyPromptToolStripMenuItem";
            this.copyPromptToolStripMenuItem.Size = new System.Drawing.Size(259, 22);
            this.copyPromptToolStripMenuItem.Text = "Copy Prompt";
            // 
            // btnOpenSelectedFolder
            // 
            this.btnOpenSelectedFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenSelectedFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnOpenSelectedFolder.BackgroundImage = global::StableDiffusionGui.Properties.Resources.baseline_folder_open_white_48dp;
            this.btnOpenSelectedFolder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnOpenSelectedFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenSelectedFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenSelectedFolder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnOpenSelectedFolder.Location = new System.Drawing.Point(640, 9);
            this.btnOpenSelectedFolder.Name = "btnOpenSelectedFolder";
            this.btnOpenSelectedFolder.Size = new System.Drawing.Size(40, 40);
            this.btnOpenSelectedFolder.TabIndex = 97;
            this.btnOpenSelectedFolder.TabStop = false;
            this.toolTip.SetToolTip(this.btnOpenSelectedFolder, "Open Selected Folder");
            this.btnOpenSelectedFolder.UseVisualStyleBackColor = false;
            this.btnOpenSelectedFolder.Click += new System.EventHandler(this.btnOpenSelectedFolder_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnRemove.BackgroundImage = global::StableDiffusionGui.Properties.Resources.iconRemove;
            this.btnRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemove.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnRemove.Location = new System.Drawing.Point(686, 9);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(40, 40);
            this.btnRemove.TabIndex = 96;
            this.btnRemove.TabStop = false;
            this.toolTip.SetToolTip(this.btnRemove, "Remove Selected Folder");
            this.btnRemove.UseVisualStyleBackColor = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnAdd.BackgroundImage = global::StableDiffusionGui.Properties.Resources.iconAdd;
            this.btnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnAdd.Location = new System.Drawing.Point(732, 9);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(40, 40);
            this.btnAdd.TabIndex = 95;
            this.btnAdd.TabStop = false;
            this.toolTip.SetToolTip(this.btnAdd, "Add Folder");
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // ModelFoldersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(784, 261);
            this.Controls.Add(this.btnOpenSelectedFolder);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.folderListView);
            this.Controls.Add(this.titleLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(800, 600);
            this.MinimumSize = new System.Drawing.Size(800, 300);
            this.Name = "ModelFoldersForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Model Folders";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ModelFoldersForm_FormClosing);
            this.Load += new System.EventHandler(this.ModelFoldersForm_Load);
            this.Shown += new System.EventHandler(this.ModelFoldersForm_Shown);
            this.menuStripDelete.ResumeLayout(false);
            this.menuStripPromptHistory.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.ListView folderListView;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip menuStripDelete;
        private System.Windows.Forms.ToolStripMenuItem deleteSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteAllToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip menuStripPromptHistory;
        private System.Windows.Forms.ToolStripMenuItem loadPromptIntoGUIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadPromptAndSettingsIntoGUIToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.ToolStripMenuItem copyPromptToolStripMenuItem;
        private System.Windows.Forms.Button btnOpenSelectedFolder;
    }
}