﻿namespace StableDiffusionGui.Forms
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.titleLabel = new System.Windows.Forms.Label();
            this.parentPanel = new System.Windows.Forms.Panel();
            this.panelNotify = new System.Windows.Forms.Panel();
            this.notificationPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panel14 = new System.Windows.Forms.Panel();
            this.comboxNotify = new System.Windows.Forms.ComboBox();
            this.panel15 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.panelAdvancedMode = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.panel16 = new System.Windows.Forms.Panel();
            this.checkboxAdvancedMode = new System.Windows.Forms.CheckBox();
            this.panel17 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.panelInitImageRetainAspectRatio = new System.Windows.Forms.Panel();
            this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
            this.panel39 = new System.Windows.Forms.Panel();
            this.checkboxInitImageRetainAspectRatio = new System.Windows.Forms.CheckBox();
            this.panel40 = new System.Windows.Forms.Panel();
            this.label19 = new System.Windows.Forms.Label();
            this.panelAutoSetResForInitImg = new System.Windows.Forms.Panel();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.panel37 = new System.Windows.Forms.Panel();
            this.checkboxAutoSetResForInitImg = new System.Windows.Forms.CheckBox();
            this.panel38 = new System.Windows.Forms.Panel();
            this.label18 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label17 = new System.Windows.Forms.Label();
            this.panelSaveUnprocessedImages = new System.Windows.Forms.Panel();
            this.panel29 = new System.Windows.Forms.Panel();
            this.panel30 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.panel31 = new System.Windows.Forms.Panel();
            this.checkboxSaveUnprocessedImages = new System.Windows.Forms.CheckBox();
            this.panel32 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.panelMultiPromptsSameSeed = new System.Windows.Forms.Panel();
            this.panel18 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.checkboxMultiPromptsSameSeed = new System.Windows.Forms.CheckBox();
            this.panel22 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.panelFavsDir = new System.Windows.Forms.Panel();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textboxFavsPath = new System.Windows.Forms.TextBox();
            this.btnFavsPathBrowse = new HTAlt.WinForms.HTButton();
            this.panel35 = new System.Windows.Forms.Panel();
            this.label16 = new System.Windows.Forms.Label();
            this.panelPromptInFilename = new System.Windows.Forms.Panel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.panel19 = new System.Windows.Forms.Panel();
            this.comboxTimestampInFilename = new System.Windows.Forms.ComboBox();
            this.checkboxModelInFilename = new System.Windows.Forms.CheckBox();
            this.checkboxSamplerInFilename = new System.Windows.Forms.CheckBox();
            this.checkboxScaleInFilename = new System.Windows.Forms.CheckBox();
            this.checkboxSeedInFilename = new System.Windows.Forms.CheckBox();
            this.checkboxPromptInFilename = new System.Windows.Forms.CheckBox();
            this.panel20 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.panelPromptSubfolders = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.checkboxOutputIgnoreWildcards = new System.Windows.Forms.CheckBox();
            this.checkboxFolderPerSession = new System.Windows.Forms.CheckBox();
            this.checkboxFolderPerPrompt = new System.Windows.Forms.CheckBox();
            this.panel9 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panelOutPath = new System.Windows.Forms.Panel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.panel23 = new System.Windows.Forms.Panel();
            this.textboxOutPath = new System.Windows.Forms.TextBox();
            this.btnOutPathBrowse = new HTAlt.WinForms.HTButton();
            this.panel24 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.panelCudaDevice = new System.Windows.Forms.Panel();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.panel27 = new System.Windows.Forms.Panel();
            this.comboxCudaDevice = new System.Windows.Forms.ComboBox();
            this.panel28 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.panelVae = new System.Windows.Forms.Panel();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.panel33 = new System.Windows.Forms.Panel();
            this.btnRefreshModelsDropdownVae = new HTAlt.WinForms.HTButton();
            this.comboxSdModelVae = new System.Windows.Forms.ComboBox();
            this.btnOpenModelsFolderVae = new HTAlt.WinForms.HTButton();
            this.panel34 = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.panelSdModel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.panel25 = new System.Windows.Forms.Panel();
            this.btnRefreshModelsDropdown = new HTAlt.WinForms.HTButton();
            this.comboxSdModel = new System.Windows.Forms.ComboBox();
            this.btnOpenModelsFolder = new HTAlt.WinForms.HTButton();
            this.panel26 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.panelUnloadModel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.checkboxUnloadModel = new System.Windows.Forms.CheckBox();
            this.panel21 = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.panelFullPrecision = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.checkboxFullPrecision = new System.Windows.Forms.CheckBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panelImplementation = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panel12 = new System.Windows.Forms.Panel();
            this.comboxImplementation = new System.Windows.Forms.ComboBox();
            this.panel13 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel11 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.parentPanel.SuspendLayout();
            this.panelNotify.SuspendLayout();
            this.notificationPanel.SuspendLayout();
            this.panel14.SuspendLayout();
            this.panel15.SuspendLayout();
            this.panelAdvancedMode.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.panel16.SuspendLayout();
            this.panel17.SuspendLayout();
            this.panelInitImageRetainAspectRatio.SuspendLayout();
            this.tableLayoutPanel15.SuspendLayout();
            this.panel39.SuspendLayout();
            this.panel40.SuspendLayout();
            this.panelAutoSetResForInitImg.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            this.panel37.SuspendLayout();
            this.panel38.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelSaveUnprocessedImages.SuspendLayout();
            this.panel29.SuspendLayout();
            this.panel30.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.panel31.SuspendLayout();
            this.panel32.SuspendLayout();
            this.panelMultiPromptsSameSeed.SuspendLayout();
            this.panel18.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel22.SuspendLayout();
            this.panelFavsDir.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel35.SuspendLayout();
            this.panelPromptInFilename.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.panel19.SuspendLayout();
            this.panel20.SuspendLayout();
            this.panelPromptSubfolders.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panelOutPath.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.panel23.SuspendLayout();
            this.panel24.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panelCudaDevice.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.panel27.SuspendLayout();
            this.panel28.SuspendLayout();
            this.panelVae.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.panel33.SuspendLayout();
            this.panel34.SuspendLayout();
            this.panelSdModel.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.panel25.SuspendLayout();
            this.panel26.SuspendLayout();
            this.panelUnloadModel.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel21.SuspendLayout();
            this.panelFullPrecision.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panelImplementation.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel13.SuspendLayout();
            this.panel11.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Yu Gothic UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.Location = new System.Drawing.Point(15, 11);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 12);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(155, 50);
            this.titleLabel.TabIndex = 13;
            this.titleLabel.Text = "Settings";
            // 
            // parentPanel
            // 
            this.parentPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parentPanel.AutoScroll = true;
            this.parentPanel.Controls.Add(this.panelNotify);
            this.parentPanel.Controls.Add(this.panelAdvancedMode);
            this.parentPanel.Controls.Add(this.panelInitImageRetainAspectRatio);
            this.parentPanel.Controls.Add(this.panelAutoSetResForInitImg);
            this.parentPanel.Controls.Add(this.panel1);
            this.parentPanel.Controls.Add(this.panelSaveUnprocessedImages);
            this.parentPanel.Controls.Add(this.panelMultiPromptsSameSeed);
            this.parentPanel.Controls.Add(this.panelFavsDir);
            this.parentPanel.Controls.Add(this.panelPromptInFilename);
            this.parentPanel.Controls.Add(this.panelPromptSubfolders);
            this.parentPanel.Controls.Add(this.panelOutPath);
            this.parentPanel.Controls.Add(this.panel5);
            this.parentPanel.Controls.Add(this.panelCudaDevice);
            this.parentPanel.Controls.Add(this.panelVae);
            this.parentPanel.Controls.Add(this.panelSdModel);
            this.parentPanel.Controls.Add(this.panelUnloadModel);
            this.parentPanel.Controls.Add(this.panelFullPrecision);
            this.parentPanel.Controls.Add(this.panelImplementation);
            this.parentPanel.Controls.Add(this.panel11);
            this.parentPanel.Location = new System.Drawing.Point(16, 76);
            this.parentPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.parentPanel.Name = "parentPanel";
            this.parentPanel.Size = new System.Drawing.Size(1147, 858);
            this.parentPanel.TabIndex = 14;
            // 
            // panelNotify
            // 
            this.panelNotify.Controls.Add(this.notificationPanel);
            this.panelNotify.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelNotify.Location = new System.Drawing.Point(0, 792);
            this.panelNotify.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelNotify.Name = "panelNotify";
            this.panelNotify.Size = new System.Drawing.Size(1147, 43);
            this.panelNotify.TabIndex = 22;
            // 
            // notificationPanel
            // 
            this.notificationPanel.ColumnCount = 2;
            this.notificationPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.notificationPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.notificationPanel.Controls.Add(this.panel14, 0, 0);
            this.notificationPanel.Controls.Add(this.panel15, 0, 0);
            this.notificationPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.notificationPanel.Location = new System.Drawing.Point(0, 0);
            this.notificationPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.notificationPanel.Name = "notificationPanel";
            this.notificationPanel.RowCount = 1;
            this.notificationPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.notificationPanel.Size = new System.Drawing.Size(1147, 43);
            this.notificationPanel.TabIndex = 1;
            // 
            // panel14
            // 
            this.panel14.Controls.Add(this.comboxNotify);
            this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel14.Location = new System.Drawing.Point(547, 0);
            this.panel14.Margin = new System.Windows.Forms.Padding(0);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(600, 43);
            this.panel14.TabIndex = 88;
            // 
            // comboxNotify
            // 
            this.comboxNotify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboxNotify.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxNotify.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxNotify.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxNotify.ForeColor = System.Drawing.Color.White;
            this.comboxNotify.FormattingEnabled = true;
            this.comboxNotify.Items.AddRange(new object[] {
            "Disabled",
            "Play Sound",
            "Show Notification",
            "Play Sound and Show Notification"});
            this.comboxNotify.Location = new System.Drawing.Point(0, 9);
            this.comboxNotify.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboxNotify.Name = "comboxNotify";
            this.comboxNotify.Size = new System.Drawing.Size(599, 24);
            this.comboxNotify.TabIndex = 106;
            // 
            // panel15
            // 
            this.panel15.Controls.Add(this.label12);
            this.panel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel15.Location = new System.Drawing.Point(0, 0);
            this.panel15.Margin = new System.Windows.Forms.Padding(0);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(547, 43);
            this.panel15.TabIndex = 87;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(3, 14);
            this.label12.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(288, 17);
            this.label12.TabIndex = 85;
            this.label12.Text = "Notify When Image Generation Has Finished";
            // 
            // panelAdvancedMode
            // 
            this.panelAdvancedMode.Controls.Add(this.tableLayoutPanel4);
            this.panelAdvancedMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelAdvancedMode.Location = new System.Drawing.Point(0, 749);
            this.panelAdvancedMode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelAdvancedMode.Name = "panelAdvancedMode";
            this.panelAdvancedMode.Size = new System.Drawing.Size(1147, 43);
            this.panelAdvancedMode.TabIndex = 15;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel4.Controls.Add(this.panel16, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.panel17, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1147, 43);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // panel16
            // 
            this.panel16.Controls.Add(this.checkboxAdvancedMode);
            this.panel16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel16.Location = new System.Drawing.Point(547, 0);
            this.panel16.Margin = new System.Windows.Forms.Padding(0);
            this.panel16.Name = "panel16";
            this.panel16.Size = new System.Drawing.Size(600, 43);
            this.panel16.TabIndex = 88;
            // 
            // checkboxAdvancedMode
            // 
            this.checkboxAdvancedMode.AutoSize = true;
            this.checkboxAdvancedMode.ForeColor = System.Drawing.Color.White;
            this.checkboxAdvancedMode.Location = new System.Drawing.Point(7, 12);
            this.checkboxAdvancedMode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkboxAdvancedMode.Name = "checkboxAdvancedMode";
            this.checkboxAdvancedMode.Size = new System.Drawing.Size(18, 17);
            this.checkboxAdvancedMode.TabIndex = 111;
            this.checkboxAdvancedMode.UseVisualStyleBackColor = true;
            // 
            // panel17
            // 
            this.panel17.Controls.Add(this.label6);
            this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel17.Location = new System.Drawing.Point(0, 0);
            this.panel17.Margin = new System.Windows.Forms.Padding(0);
            this.panel17.Name = "panel17";
            this.panel17.Size = new System.Drawing.Size(547, 43);
            this.panel17.TabIndex = 87;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(3, 14);
            this.label6.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(328, 17);
            this.label6.TabIndex = 85;
            this.label6.Text = "Advanced Mode (Unlock Higher Values for Sliders)";
            // 
            // panelInitImageRetainAspectRatio
            // 
            this.panelInitImageRetainAspectRatio.Controls.Add(this.tableLayoutPanel15);
            this.panelInitImageRetainAspectRatio.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInitImageRetainAspectRatio.Location = new System.Drawing.Point(0, 706);
            this.panelInitImageRetainAspectRatio.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelInitImageRetainAspectRatio.Name = "panelInitImageRetainAspectRatio";
            this.panelInitImageRetainAspectRatio.Size = new System.Drawing.Size(1147, 43);
            this.panelInitImageRetainAspectRatio.TabIndex = 29;
            // 
            // tableLayoutPanel15
            // 
            this.tableLayoutPanel15.ColumnCount = 2;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel15.Controls.Add(this.panel39, 0, 0);
            this.tableLayoutPanel15.Controls.Add(this.panel40, 0, 0);
            this.tableLayoutPanel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel15.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel15.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.RowCount = 1;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(1147, 43);
            this.tableLayoutPanel15.TabIndex = 2;
            // 
            // panel39
            // 
            this.panel39.Controls.Add(this.checkboxInitImageRetainAspectRatio);
            this.panel39.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel39.Location = new System.Drawing.Point(547, 0);
            this.panel39.Margin = new System.Windows.Forms.Padding(0);
            this.panel39.Name = "panel39";
            this.panel39.Size = new System.Drawing.Size(600, 43);
            this.panel39.TabIndex = 88;
            // 
            // checkboxInitImageRetainAspectRatio
            // 
            this.checkboxInitImageRetainAspectRatio.AutoSize = true;
            this.checkboxInitImageRetainAspectRatio.ForeColor = System.Drawing.Color.White;
            this.checkboxInitImageRetainAspectRatio.Location = new System.Drawing.Point(7, 12);
            this.checkboxInitImageRetainAspectRatio.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkboxInitImageRetainAspectRatio.Name = "checkboxInitImageRetainAspectRatio";
            this.checkboxInitImageRetainAspectRatio.Size = new System.Drawing.Size(18, 17);
            this.checkboxInitImageRetainAspectRatio.TabIndex = 111;
            this.checkboxInitImageRetainAspectRatio.UseVisualStyleBackColor = true;
            // 
            // panel40
            // 
            this.panel40.Controls.Add(this.label19);
            this.panel40.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel40.Location = new System.Drawing.Point(0, 0);
            this.panel40.Margin = new System.Windows.Forms.Padding(0);
            this.panel40.Name = "panel40";
            this.panel40.Size = new System.Drawing.Size(547, 43);
            this.panel40.TabIndex = 87;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.White;
            this.label19.Location = new System.Drawing.Point(3, 14);
            this.label19.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(403, 17);
            this.label19.TabIndex = 85;
            this.label19.Text = "Retain Aspect Ratio of Initialization Image (If It Needs Resizing)";
            // 
            // panelAutoSetResForInitImg
            // 
            this.panelAutoSetResForInitImg.Controls.Add(this.tableLayoutPanel14);
            this.panelAutoSetResForInitImg.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelAutoSetResForInitImg.Location = new System.Drawing.Point(0, 663);
            this.panelAutoSetResForInitImg.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelAutoSetResForInitImg.Name = "panelAutoSetResForInitImg";
            this.panelAutoSetResForInitImg.Size = new System.Drawing.Size(1147, 43);
            this.panelAutoSetResForInitImg.TabIndex = 28;
            // 
            // tableLayoutPanel14
            // 
            this.tableLayoutPanel14.ColumnCount = 2;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel14.Controls.Add(this.panel37, 0, 0);
            this.tableLayoutPanel14.Controls.Add(this.panel38, 0, 0);
            this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel14.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel14.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.RowCount = 1;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(1147, 43);
            this.tableLayoutPanel14.TabIndex = 2;
            // 
            // panel37
            // 
            this.panel37.Controls.Add(this.checkboxAutoSetResForInitImg);
            this.panel37.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel37.Location = new System.Drawing.Point(547, 0);
            this.panel37.Margin = new System.Windows.Forms.Padding(0);
            this.panel37.Name = "panel37";
            this.panel37.Size = new System.Drawing.Size(600, 43);
            this.panel37.TabIndex = 88;
            // 
            // checkboxAutoSetResForInitImg
            // 
            this.checkboxAutoSetResForInitImg.AutoSize = true;
            this.checkboxAutoSetResForInitImg.ForeColor = System.Drawing.Color.White;
            this.checkboxAutoSetResForInitImg.Location = new System.Drawing.Point(7, 12);
            this.checkboxAutoSetResForInitImg.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkboxAutoSetResForInitImg.Name = "checkboxAutoSetResForInitImg";
            this.checkboxAutoSetResForInitImg.Size = new System.Drawing.Size(18, 17);
            this.checkboxAutoSetResForInitImg.TabIndex = 111;
            this.checkboxAutoSetResForInitImg.UseVisualStyleBackColor = true;
            // 
            // panel38
            // 
            this.panel38.Controls.Add(this.label18);
            this.panel38.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel38.Location = new System.Drawing.Point(0, 0);
            this.panel38.Margin = new System.Windows.Forms.Padding(0);
            this.panel38.Name = "panel38";
            this.panel38.Size = new System.Drawing.Size(547, 43);
            this.panel38.TabIndex = 87;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.White;
            this.label18.Location = new System.Drawing.Point(3, 14);
            this.label18.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(490, 17);
            this.label18.TabIndex = 85;
            this.label18.Text = "Automatically Set Generation Resolution After Loading an Initialization Image";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label17);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 614);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1147, 49);
            this.panel1.TabIndex = 27;
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(3, 23);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(177, 20);
            this.label17.TabIndex = 1;
            this.label17.Text = "Application Settings";
            // 
            // panelSaveUnprocessedImages
            // 
            this.panelSaveUnprocessedImages.Controls.Add(this.panel29);
            this.panelSaveUnprocessedImages.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSaveUnprocessedImages.Location = new System.Drawing.Point(0, 571);
            this.panelSaveUnprocessedImages.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelSaveUnprocessedImages.Name = "panelSaveUnprocessedImages";
            this.panelSaveUnprocessedImages.Size = new System.Drawing.Size(1147, 43);
            this.panelSaveUnprocessedImages.TabIndex = 23;
            // 
            // panel29
            // 
            this.panel29.Controls.Add(this.panel30);
            this.panel29.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel29.Location = new System.Drawing.Point(0, 0);
            this.panel29.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel29.Name = "panel29";
            this.panel29.Size = new System.Drawing.Size(1147, 43);
            this.panel29.TabIndex = 17;
            // 
            // panel30
            // 
            this.panel30.Controls.Add(this.tableLayoutPanel10);
            this.panel30.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel30.Location = new System.Drawing.Point(0, 0);
            this.panel30.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel30.Name = "panel30";
            this.panel30.Size = new System.Drawing.Size(1147, 43);
            this.panel30.TabIndex = 16;
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.ColumnCount = 2;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel10.Controls.Add(this.panel31, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.panel32, 0, 0);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel10.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 1;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(1147, 43);
            this.tableLayoutPanel10.TabIndex = 2;
            // 
            // panel31
            // 
            this.panel31.Controls.Add(this.checkboxSaveUnprocessedImages);
            this.panel31.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel31.Location = new System.Drawing.Point(547, 0);
            this.panel31.Margin = new System.Windows.Forms.Padding(0);
            this.panel31.Name = "panel31";
            this.panel31.Size = new System.Drawing.Size(600, 43);
            this.panel31.TabIndex = 88;
            // 
            // checkboxSaveUnprocessedImages
            // 
            this.checkboxSaveUnprocessedImages.AutoSize = true;
            this.checkboxSaveUnprocessedImages.ForeColor = System.Drawing.Color.White;
            this.checkboxSaveUnprocessedImages.Location = new System.Drawing.Point(7, 12);
            this.checkboxSaveUnprocessedImages.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkboxSaveUnprocessedImages.Name = "checkboxSaveUnprocessedImages";
            this.checkboxSaveUnprocessedImages.Size = new System.Drawing.Size(18, 17);
            this.checkboxSaveUnprocessedImages.TabIndex = 111;
            this.checkboxSaveUnprocessedImages.UseVisualStyleBackColor = true;
            // 
            // panel32
            // 
            this.panel32.Controls.Add(this.label13);
            this.panel32.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel32.Location = new System.Drawing.Point(0, 0);
            this.panel32.Margin = new System.Windows.Forms.Padding(0);
            this.panel32.Name = "panel32";
            this.panel32.Size = new System.Drawing.Size(547, 43);
            this.panel32.TabIndex = 87;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(3, 14);
            this.label13.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(429, 17);
            this.label13.TabIndex = 85;
            this.label13.Text = "When Post-Processing Is Enabled, Also Save Un-Processed Image";
            // 
            // panelMultiPromptsSameSeed
            // 
            this.panelMultiPromptsSameSeed.Controls.Add(this.panel18);
            this.panelMultiPromptsSameSeed.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMultiPromptsSameSeed.Location = new System.Drawing.Point(0, 528);
            this.panelMultiPromptsSameSeed.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelMultiPromptsSameSeed.Name = "panelMultiPromptsSameSeed";
            this.panelMultiPromptsSameSeed.Size = new System.Drawing.Size(1147, 43);
            this.panelMultiPromptsSameSeed.TabIndex = 17;
            // 
            // panel18
            // 
            this.panel18.Controls.Add(this.panel3);
            this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel18.Location = new System.Drawing.Point(0, 0);
            this.panel18.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel18.Name = "panel18";
            this.panel18.Size = new System.Drawing.Size(1147, 43);
            this.panel18.TabIndex = 17;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.tableLayoutPanel6);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1147, 43);
            this.panel3.TabIndex = 16;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel6.Controls.Add(this.panel6, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.panel22, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(1147, 43);
            this.tableLayoutPanel6.TabIndex = 2;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.checkboxMultiPromptsSameSeed);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(547, 0);
            this.panel6.Margin = new System.Windows.Forms.Padding(0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(600, 43);
            this.panel6.TabIndex = 88;
            // 
            // checkboxMultiPromptsSameSeed
            // 
            this.checkboxMultiPromptsSameSeed.AutoSize = true;
            this.checkboxMultiPromptsSameSeed.ForeColor = System.Drawing.Color.White;
            this.checkboxMultiPromptsSameSeed.Location = new System.Drawing.Point(7, 12);
            this.checkboxMultiPromptsSameSeed.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkboxMultiPromptsSameSeed.Name = "checkboxMultiPromptsSameSeed";
            this.checkboxMultiPromptsSameSeed.Size = new System.Drawing.Size(18, 17);
            this.checkboxMultiPromptsSameSeed.TabIndex = 111;
            this.checkboxMultiPromptsSameSeed.UseVisualStyleBackColor = true;
            // 
            // panel22
            // 
            this.panel22.Controls.Add(this.label8);
            this.panel22.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel22.Location = new System.Drawing.Point(0, 0);
            this.panel22.Margin = new System.Windows.Forms.Padding(0);
            this.panel22.Name = "panel22";
            this.panel22.Size = new System.Drawing.Size(547, 43);
            this.panel22.TabIndex = 87;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(3, 14);
            this.label8.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(469, 17);
            this.label8.TabIndex = 85;
            this.label8.Text = "When Running Multiple Prompts, Use Same Starting Seed for All of Them";
            // 
            // panelFavsDir
            // 
            this.panelFavsDir.Controls.Add(this.tableLayoutPanel13);
            this.panelFavsDir.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFavsDir.Location = new System.Drawing.Point(0, 485);
            this.panelFavsDir.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelFavsDir.Name = "panelFavsDir";
            this.panelFavsDir.Size = new System.Drawing.Size(1147, 43);
            this.panelFavsDir.TabIndex = 26;
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.ColumnCount = 2;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel13.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel13.Controls.Add(this.panel35, 0, 0);
            this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel13.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel13.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 1;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel13.Size = new System.Drawing.Size(1147, 43);
            this.tableLayoutPanel13.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.textboxFavsPath);
            this.panel2.Controls.Add(this.btnFavsPathBrowse);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(547, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(600, 43);
            this.panel2.TabIndex = 88;
            // 
            // textboxFavsPath
            // 
            this.textboxFavsPath.AllowDrop = true;
            this.textboxFavsPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxFavsPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textboxFavsPath.ForeColor = System.Drawing.Color.White;
            this.textboxFavsPath.Location = new System.Drawing.Point(0, 7);
            this.textboxFavsPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textboxFavsPath.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxFavsPath.Name = "textboxFavsPath";
            this.textboxFavsPath.Size = new System.Drawing.Size(491, 22);
            this.textboxFavsPath.TabIndex = 2;
            this.textboxFavsPath.WordWrap = false;
            // 
            // btnFavsPathBrowse
            // 
            this.btnFavsPathBrowse.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnFavsPathBrowse.AutoColor = true;
            this.btnFavsPathBrowse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnFavsPathBrowse.ButtonImage = null;
            this.btnFavsPathBrowse.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnFavsPathBrowse.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnFavsPathBrowse.DrawImage = false;
            this.btnFavsPathBrowse.ForeColor = System.Drawing.Color.White;
            this.btnFavsPathBrowse.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.btnFavsPathBrowse.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnFavsPathBrowse.Location = new System.Drawing.Point(500, 7);
            this.btnFavsPathBrowse.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnFavsPathBrowse.Name = "btnFavsPathBrowse";
            this.btnFavsPathBrowse.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnFavsPathBrowse.Size = new System.Drawing.Size(100, 28);
            this.btnFavsPathBrowse.TabIndex = 3;
            this.btnFavsPathBrowse.TabStop = false;
            this.btnFavsPathBrowse.Text = "Browse...";
            this.toolTip.SetToolTip(this.btnFavsPathBrowse, "Browse for an Image Output Folder");
            this.btnFavsPathBrowse.Click += new System.EventHandler(this.btnFavsPathBrowse_Click);
            // 
            // panel35
            // 
            this.panel35.Controls.Add(this.label16);
            this.panel35.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel35.Location = new System.Drawing.Point(0, 0);
            this.panel35.Margin = new System.Windows.Forms.Padding(0);
            this.panel35.Name = "panel35";
            this.panel35.Size = new System.Drawing.Size(547, 43);
            this.panel35.TabIndex = 87;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.White;
            this.label16.Location = new System.Drawing.Point(3, 14);
            this.label16.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(110, 17);
            this.label16.TabIndex = 85;
            this.label16.Text = "Favorites Folder";
            // 
            // panelPromptInFilename
            // 
            this.panelPromptInFilename.Controls.Add(this.tableLayoutPanel5);
            this.panelPromptInFilename.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPromptInFilename.Location = new System.Drawing.Point(0, 442);
            this.panelPromptInFilename.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelPromptInFilename.Name = "panelPromptInFilename";
            this.panelPromptInFilename.Size = new System.Drawing.Size(1147, 43);
            this.panelPromptInFilename.TabIndex = 16;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel5.Controls.Add(this.panel19, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.panel20, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(1147, 43);
            this.tableLayoutPanel5.TabIndex = 2;
            // 
            // panel19
            // 
            this.panel19.Controls.Add(this.comboxTimestampInFilename);
            this.panel19.Controls.Add(this.checkboxModelInFilename);
            this.panel19.Controls.Add(this.checkboxSamplerInFilename);
            this.panel19.Controls.Add(this.checkboxScaleInFilename);
            this.panel19.Controls.Add(this.checkboxSeedInFilename);
            this.panel19.Controls.Add(this.checkboxPromptInFilename);
            this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel19.Location = new System.Drawing.Point(547, 0);
            this.panel19.Margin = new System.Windows.Forms.Padding(0);
            this.panel19.Name = "panel19";
            this.panel19.Size = new System.Drawing.Size(600, 43);
            this.panel19.TabIndex = 88;
            // 
            // comboxTimestampInFilename
            // 
            this.comboxTimestampInFilename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboxTimestampInFilename.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxTimestampInFilename.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxTimestampInFilename.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxTimestampInFilename.ForeColor = System.Drawing.Color.White;
            this.comboxTimestampInFilename.FormattingEnabled = true;
            this.comboxTimestampInFilename.Location = new System.Drawing.Point(0, 9);
            this.comboxTimestampInFilename.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboxTimestampInFilename.Name = "comboxTimestampInFilename";
            this.comboxTimestampInFilename.Size = new System.Drawing.Size(132, 24);
            this.comboxTimestampInFilename.TabIndex = 116;
            // 
            // checkboxModelInFilename
            // 
            this.checkboxModelInFilename.AutoSize = true;
            this.checkboxModelInFilename.ForeColor = System.Drawing.Color.White;
            this.checkboxModelInFilename.Location = new System.Drawing.Point(476, 11);
            this.checkboxModelInFilename.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkboxModelInFilename.Name = "checkboxModelInFilename";
            this.checkboxModelInFilename.Size = new System.Drawing.Size(67, 20);
            this.checkboxModelInFilename.TabIndex = 115;
            this.checkboxModelInFilename.Text = "Model";
            this.checkboxModelInFilename.UseVisualStyleBackColor = true;
            // 
            // checkboxSamplerInFilename
            // 
            this.checkboxSamplerInFilename.AutoSize = true;
            this.checkboxSamplerInFilename.ForeColor = System.Drawing.Color.White;
            this.checkboxSamplerInFilename.Location = new System.Drawing.Point(383, 11);
            this.checkboxSamplerInFilename.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkboxSamplerInFilename.Name = "checkboxSamplerInFilename";
            this.checkboxSamplerInFilename.Size = new System.Drawing.Size(80, 20);
            this.checkboxSamplerInFilename.TabIndex = 114;
            this.checkboxSamplerInFilename.Text = "Sampler";
            this.checkboxSamplerInFilename.UseVisualStyleBackColor = true;
            // 
            // checkboxScaleInFilename
            // 
            this.checkboxScaleInFilename.AutoSize = true;
            this.checkboxScaleInFilename.ForeColor = System.Drawing.Color.White;
            this.checkboxScaleInFilename.Location = new System.Drawing.Point(304, 11);
            this.checkboxScaleInFilename.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkboxScaleInFilename.Name = "checkboxScaleInFilename";
            this.checkboxScaleInFilename.Size = new System.Drawing.Size(64, 20);
            this.checkboxScaleInFilename.TabIndex = 113;
            this.checkboxScaleInFilename.Text = "Scale";
            this.checkboxScaleInFilename.UseVisualStyleBackColor = true;
            // 
            // checkboxSeedInFilename
            // 
            this.checkboxSeedInFilename.AutoSize = true;
            this.checkboxSeedInFilename.ForeColor = System.Drawing.Color.White;
            this.checkboxSeedInFilename.Location = new System.Drawing.Point(228, 11);
            this.checkboxSeedInFilename.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkboxSeedInFilename.Name = "checkboxSeedInFilename";
            this.checkboxSeedInFilename.Size = new System.Drawing.Size(62, 20);
            this.checkboxSeedInFilename.TabIndex = 112;
            this.checkboxSeedInFilename.Text = "Seed";
            this.checkboxSeedInFilename.UseVisualStyleBackColor = true;
            // 
            // checkboxPromptInFilename
            // 
            this.checkboxPromptInFilename.AutoSize = true;
            this.checkboxPromptInFilename.ForeColor = System.Drawing.Color.White;
            this.checkboxPromptInFilename.Location = new System.Drawing.Point(141, 11);
            this.checkboxPromptInFilename.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkboxPromptInFilename.Name = "checkboxPromptInFilename";
            this.checkboxPromptInFilename.Size = new System.Drawing.Size(72, 20);
            this.checkboxPromptInFilename.TabIndex = 111;
            this.checkboxPromptInFilename.Text = "Prompt";
            this.checkboxPromptInFilename.UseVisualStyleBackColor = true;
            // 
            // panel20
            // 
            this.panel20.Controls.Add(this.label7);
            this.panel20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel20.Location = new System.Drawing.Point(0, 0);
            this.panel20.Margin = new System.Windows.Forms.Padding(0);
            this.panel20.Name = "panel20";
            this.panel20.Size = new System.Drawing.Size(547, 43);
            this.panel20.TabIndex = 87;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(3, 14);
            this.label7.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(179, 17);
            this.label7.TabIndex = 85;
            this.label7.Text = "Data to Include in Filename";
            // 
            // panelPromptSubfolders
            // 
            this.panelPromptSubfolders.Controls.Add(this.tableLayoutPanel2);
            this.panelPromptSubfolders.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPromptSubfolders.Location = new System.Drawing.Point(0, 399);
            this.panelPromptSubfolders.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelPromptSubfolders.Name = "panelPromptSubfolders";
            this.panelPromptSubfolders.Size = new System.Drawing.Size(1147, 43);
            this.panelPromptSubfolders.TabIndex = 12;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel2.Controls.Add(this.panel10, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel9, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1147, 43);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.checkboxOutputIgnoreWildcards);
            this.panel10.Controls.Add(this.checkboxFolderPerSession);
            this.panel10.Controls.Add(this.checkboxFolderPerPrompt);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(547, 0);
            this.panel10.Margin = new System.Windows.Forms.Padding(0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(600, 43);
            this.panel10.TabIndex = 88;
            // 
            // checkboxOutputIgnoreWildcards
            // 
            this.checkboxOutputIgnoreWildcards.AutoSize = true;
            this.checkboxOutputIgnoreWildcards.ForeColor = System.Drawing.Color.White;
            this.checkboxOutputIgnoreWildcards.Location = new System.Drawing.Point(183, 12);
            this.checkboxOutputIgnoreWildcards.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkboxOutputIgnoreWildcards.Name = "checkboxOutputIgnoreWildcards";
            this.checkboxOutputIgnoreWildcards.Size = new System.Drawing.Size(131, 20);
            this.checkboxOutputIgnoreWildcards.TabIndex = 113;
            this.checkboxOutputIgnoreWildcards.Text = "Ignore Wildcards";
            this.toolTip.SetToolTip(this.checkboxOutputIgnoreWildcards, "When enabled, wildcards in prompt will not be applied. This is useful for groupin" +
        "g images created with a wildcard together.");
            this.checkboxOutputIgnoreWildcards.UseVisualStyleBackColor = true;
            // 
            // checkboxFolderPerSession
            // 
            this.checkboxFolderPerSession.AutoSize = true;
            this.checkboxFolderPerSession.ForeColor = System.Drawing.Color.White;
            this.checkboxFolderPerSession.Location = new System.Drawing.Point(332, 12);
            this.checkboxFolderPerSession.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkboxFolderPerSession.Name = "checkboxFolderPerSession";
            this.checkboxFolderPerSession.Size = new System.Drawing.Size(163, 20);
            this.checkboxFolderPerSession.TabIndex = 112;
            this.checkboxFolderPerSession.Text = "Subfolder Per Session";
            this.toolTip.SetToolTip(this.checkboxFolderPerSession, "When enabled, a subfolder will be created for each session (every time you start " +
        "the program).");
            this.checkboxFolderPerSession.UseVisualStyleBackColor = true;
            // 
            // checkboxFolderPerPrompt
            // 
            this.checkboxFolderPerPrompt.AutoSize = true;
            this.checkboxFolderPerPrompt.ForeColor = System.Drawing.Color.White;
            this.checkboxFolderPerPrompt.Location = new System.Drawing.Point(7, 12);
            this.checkboxFolderPerPrompt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkboxFolderPerPrompt.Name = "checkboxFolderPerPrompt";
            this.checkboxFolderPerPrompt.Size = new System.Drawing.Size(157, 20);
            this.checkboxFolderPerPrompt.TabIndex = 111;
            this.checkboxFolderPerPrompt.Text = "Subfolder Per Prompt";
            this.toolTip.SetToolTip(this.checkboxFolderPerPrompt, "When enabled, a subfolder will be created for each prompt.");
            this.checkboxFolderPerPrompt.UseVisualStyleBackColor = true;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.label3);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Margin = new System.Windows.Forms.Padding(0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(547, 43);
            this.panel9.TabIndex = 87;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(3, 14);
            this.label3.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(169, 17);
            this.label3.TabIndex = 85;
            this.label3.Text = "Output Subfolder Options";
            // 
            // panelOutPath
            // 
            this.panelOutPath.Controls.Add(this.tableLayoutPanel7);
            this.panelOutPath.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelOutPath.Location = new System.Drawing.Point(0, 356);
            this.panelOutPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelOutPath.Name = "panelOutPath";
            this.panelOutPath.Size = new System.Drawing.Size(1147, 43);
            this.panelOutPath.TabIndex = 13;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel7.Controls.Add(this.panel23, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.panel24, 0, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(1147, 43);
            this.tableLayoutPanel7.TabIndex = 1;
            // 
            // panel23
            // 
            this.panel23.Controls.Add(this.textboxOutPath);
            this.panel23.Controls.Add(this.btnOutPathBrowse);
            this.panel23.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel23.Location = new System.Drawing.Point(547, 0);
            this.panel23.Margin = new System.Windows.Forms.Padding(0);
            this.panel23.Name = "panel23";
            this.panel23.Size = new System.Drawing.Size(600, 43);
            this.panel23.TabIndex = 88;
            // 
            // textboxOutPath
            // 
            this.textboxOutPath.AllowDrop = true;
            this.textboxOutPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxOutPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textboxOutPath.ForeColor = System.Drawing.Color.White;
            this.textboxOutPath.Location = new System.Drawing.Point(0, 9);
            this.textboxOutPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textboxOutPath.MinimumSize = new System.Drawing.Size(4, 21);
            this.textboxOutPath.Name = "textboxOutPath";
            this.textboxOutPath.Size = new System.Drawing.Size(491, 22);
            this.textboxOutPath.TabIndex = 2;
            this.textboxOutPath.WordWrap = false;
            // 
            // btnOutPathBrowse
            // 
            this.btnOutPathBrowse.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnOutPathBrowse.AutoColor = true;
            this.btnOutPathBrowse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnOutPathBrowse.ButtonImage = null;
            this.btnOutPathBrowse.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnOutPathBrowse.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnOutPathBrowse.DrawImage = false;
            this.btnOutPathBrowse.ForeColor = System.Drawing.Color.White;
            this.btnOutPathBrowse.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.btnOutPathBrowse.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnOutPathBrowse.Location = new System.Drawing.Point(500, 7);
            this.btnOutPathBrowse.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOutPathBrowse.Name = "btnOutPathBrowse";
            this.btnOutPathBrowse.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnOutPathBrowse.Size = new System.Drawing.Size(100, 28);
            this.btnOutPathBrowse.TabIndex = 3;
            this.btnOutPathBrowse.TabStop = false;
            this.btnOutPathBrowse.Text = "Browse...";
            this.toolTip.SetToolTip(this.btnOutPathBrowse, "Browse for an Image Output Folder");
            this.btnOutPathBrowse.Click += new System.EventHandler(this.btnOutPathBrowse_Click);
            // 
            // panel24
            // 
            this.panel24.Controls.Add(this.label9);
            this.panel24.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel24.Location = new System.Drawing.Point(0, 0);
            this.panel24.Margin = new System.Windows.Forms.Padding(0);
            this.panel24.Name = "panel24";
            this.panel24.Size = new System.Drawing.Size(547, 43);
            this.panel24.TabIndex = 87;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(3, 14);
            this.label9.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(137, 17);
            this.label9.TabIndex = 85;
            this.label9.Text = "Image Output Folder";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label5);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 307);
            this.panel5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1147, 49);
            this.panel5.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 23);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(140, 20);
            this.label5.TabIndex = 1;
            this.label5.Text = "Output Settings";
            // 
            // panelCudaDevice
            // 
            this.panelCudaDevice.Controls.Add(this.tableLayoutPanel9);
            this.panelCudaDevice.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCudaDevice.Location = new System.Drawing.Point(0, 264);
            this.panelCudaDevice.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelCudaDevice.Name = "panelCudaDevice";
            this.panelCudaDevice.Size = new System.Drawing.Size(1147, 43);
            this.panelCudaDevice.TabIndex = 21;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 2;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel9.Controls.Add(this.panel27, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.panel28, 0, 0);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(1147, 43);
            this.tableLayoutPanel9.TabIndex = 1;
            // 
            // panel27
            // 
            this.panel27.Controls.Add(this.comboxCudaDevice);
            this.panel27.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel27.Location = new System.Drawing.Point(547, 0);
            this.panel27.Margin = new System.Windows.Forms.Padding(0);
            this.panel27.Name = "panel27";
            this.panel27.Size = new System.Drawing.Size(600, 43);
            this.panel27.TabIndex = 88;
            // 
            // comboxCudaDevice
            // 
            this.comboxCudaDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboxCudaDevice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxCudaDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxCudaDevice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxCudaDevice.ForeColor = System.Drawing.Color.White;
            this.comboxCudaDevice.FormattingEnabled = true;
            this.comboxCudaDevice.Location = new System.Drawing.Point(0, 9);
            this.comboxCudaDevice.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboxCudaDevice.Name = "comboxCudaDevice";
            this.comboxCudaDevice.Size = new System.Drawing.Size(599, 24);
            this.comboxCudaDevice.TabIndex = 106;
            // 
            // panel28
            // 
            this.panel28.Controls.Add(this.label11);
            this.panel28.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel28.Location = new System.Drawing.Point(0, 0);
            this.panel28.Margin = new System.Windows.Forms.Padding(0);
            this.panel28.Name = "panel28";
            this.panel28.Size = new System.Drawing.Size(547, 43);
            this.panel28.TabIndex = 87;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(3, 14);
            this.label11.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(51, 17);
            this.label11.TabIndex = 85;
            this.label11.Text = "Device";
            // 
            // panelVae
            // 
            this.panelVae.Controls.Add(this.tableLayoutPanel12);
            this.panelVae.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelVae.Location = new System.Drawing.Point(0, 221);
            this.panelVae.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelVae.Name = "panelVae";
            this.panelVae.Size = new System.Drawing.Size(1147, 43);
            this.panelVae.TabIndex = 25;
            // 
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.ColumnCount = 2;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel12.Controls.Add(this.panel33, 0, 0);
            this.tableLayoutPanel12.Controls.Add(this.panel34, 0, 0);
            this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel12.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel12.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.RowCount = 1;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(1147, 43);
            this.tableLayoutPanel12.TabIndex = 1;
            // 
            // panel33
            // 
            this.panel33.Controls.Add(this.btnRefreshModelsDropdownVae);
            this.panel33.Controls.Add(this.comboxSdModelVae);
            this.panel33.Controls.Add(this.btnOpenModelsFolderVae);
            this.panel33.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel33.Location = new System.Drawing.Point(547, 0);
            this.panel33.Margin = new System.Windows.Forms.Padding(0);
            this.panel33.Name = "panel33";
            this.panel33.Size = new System.Drawing.Size(600, 43);
            this.panel33.TabIndex = 88;
            // 
            // btnRefreshModelsDropdownVae
            // 
            this.btnRefreshModelsDropdownVae.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnRefreshModelsDropdownVae.AutoColor = true;
            this.btnRefreshModelsDropdownVae.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnRefreshModelsDropdownVae.ButtonImage = null;
            this.btnRefreshModelsDropdownVae.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnRefreshModelsDropdownVae.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnRefreshModelsDropdownVae.DrawImage = false;
            this.btnRefreshModelsDropdownVae.ForeColor = System.Drawing.Color.White;
            this.btnRefreshModelsDropdownVae.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.btnRefreshModelsDropdownVae.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnRefreshModelsDropdownVae.Location = new System.Drawing.Point(392, 6);
            this.btnRefreshModelsDropdownVae.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRefreshModelsDropdownVae.Name = "btnRefreshModelsDropdownVae";
            this.btnRefreshModelsDropdownVae.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnRefreshModelsDropdownVae.Size = new System.Drawing.Size(100, 28);
            this.btnRefreshModelsDropdownVae.TabIndex = 107;
            this.btnRefreshModelsDropdownVae.TabStop = false;
            this.btnRefreshModelsDropdownVae.Text = "Refresh List";
            this.btnRefreshModelsDropdownVae.Click += new System.EventHandler(this.btnRefreshModelsDropdownVae_Click);
            // 
            // comboxSdModelVae
            // 
            this.comboxSdModelVae.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboxSdModelVae.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxSdModelVae.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxSdModelVae.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxSdModelVae.ForeColor = System.Drawing.Color.White;
            this.comboxSdModelVae.FormattingEnabled = true;
            this.comboxSdModelVae.Location = new System.Drawing.Point(0, 9);
            this.comboxSdModelVae.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboxSdModelVae.Name = "comboxSdModelVae";
            this.comboxSdModelVae.Size = new System.Drawing.Size(383, 24);
            this.comboxSdModelVae.TabIndex = 106;
            // 
            // btnOpenModelsFolderVae
            // 
            this.btnOpenModelsFolderVae.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnOpenModelsFolderVae.AutoColor = true;
            this.btnOpenModelsFolderVae.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnOpenModelsFolderVae.ButtonImage = null;
            this.btnOpenModelsFolderVae.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnOpenModelsFolderVae.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnOpenModelsFolderVae.DrawImage = false;
            this.btnOpenModelsFolderVae.ForeColor = System.Drawing.Color.White;
            this.btnOpenModelsFolderVae.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.btnOpenModelsFolderVae.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnOpenModelsFolderVae.Location = new System.Drawing.Point(500, 6);
            this.btnOpenModelsFolderVae.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOpenModelsFolderVae.Name = "btnOpenModelsFolderVae";
            this.btnOpenModelsFolderVae.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnOpenModelsFolderVae.Size = new System.Drawing.Size(100, 28);
            this.btnOpenModelsFolderVae.TabIndex = 4;
            this.btnOpenModelsFolderVae.TabStop = false;
            this.btnOpenModelsFolderVae.Text = "Folders...";
            this.toolTip.SetToolTip(this.btnOpenModelsFolderVae, "Manage VAE Model Folders");
            this.btnOpenModelsFolderVae.Click += new System.EventHandler(this.btnOpenModelsFolderVae_Click);
            // 
            // panel34
            // 
            this.panel34.Controls.Add(this.label15);
            this.panel34.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel34.Location = new System.Drawing.Point(0, 0);
            this.panel34.Margin = new System.Windows.Forms.Padding(0);
            this.panel34.Name = "panel34";
            this.panel34.Size = new System.Drawing.Size(547, 43);
            this.panel34.TabIndex = 87;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(3, 14);
            this.label15.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(304, 17);
            this.label15.TabIndex = 85;
            this.label15.Text = "Stable Diffusion VAE (Variational Autoencoder)";
            // 
            // panelSdModel
            // 
            this.panelSdModel.Controls.Add(this.tableLayoutPanel8);
            this.panelSdModel.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSdModel.Location = new System.Drawing.Point(0, 178);
            this.panelSdModel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelSdModel.Name = "panelSdModel";
            this.panelSdModel.Size = new System.Drawing.Size(1147, 43);
            this.panelSdModel.TabIndex = 20;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 2;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel8.Controls.Add(this.panel25, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.panel26, 0, 0);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(1147, 43);
            this.tableLayoutPanel8.TabIndex = 1;
            // 
            // panel25
            // 
            this.panel25.Controls.Add(this.btnRefreshModelsDropdown);
            this.panel25.Controls.Add(this.comboxSdModel);
            this.panel25.Controls.Add(this.btnOpenModelsFolder);
            this.panel25.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel25.Location = new System.Drawing.Point(547, 0);
            this.panel25.Margin = new System.Windows.Forms.Padding(0);
            this.panel25.Name = "panel25";
            this.panel25.Size = new System.Drawing.Size(600, 43);
            this.panel25.TabIndex = 88;
            // 
            // btnRefreshModelsDropdown
            // 
            this.btnRefreshModelsDropdown.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnRefreshModelsDropdown.AutoColor = true;
            this.btnRefreshModelsDropdown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnRefreshModelsDropdown.ButtonImage = null;
            this.btnRefreshModelsDropdown.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnRefreshModelsDropdown.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnRefreshModelsDropdown.DrawImage = false;
            this.btnRefreshModelsDropdown.ForeColor = System.Drawing.Color.White;
            this.btnRefreshModelsDropdown.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.btnRefreshModelsDropdown.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnRefreshModelsDropdown.Location = new System.Drawing.Point(392, 6);
            this.btnRefreshModelsDropdown.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRefreshModelsDropdown.Name = "btnRefreshModelsDropdown";
            this.btnRefreshModelsDropdown.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnRefreshModelsDropdown.Size = new System.Drawing.Size(100, 28);
            this.btnRefreshModelsDropdown.TabIndex = 107;
            this.btnRefreshModelsDropdown.TabStop = false;
            this.btnRefreshModelsDropdown.Text = "Refresh List";
            this.btnRefreshModelsDropdown.Click += new System.EventHandler(this.btnRefreshModelsDropdown_Click);
            // 
            // comboxSdModel
            // 
            this.comboxSdModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboxSdModel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxSdModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxSdModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxSdModel.ForeColor = System.Drawing.Color.White;
            this.comboxSdModel.FormattingEnabled = true;
            this.comboxSdModel.Location = new System.Drawing.Point(0, 9);
            this.comboxSdModel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboxSdModel.Name = "comboxSdModel";
            this.comboxSdModel.Size = new System.Drawing.Size(383, 24);
            this.comboxSdModel.TabIndex = 106;
            // 
            // btnOpenModelsFolder
            // 
            this.btnOpenModelsFolder.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnOpenModelsFolder.AutoColor = true;
            this.btnOpenModelsFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.btnOpenModelsFolder.ButtonImage = null;
            this.btnOpenModelsFolder.ButtonShape = HTAlt.WinForms.HTButton.ButtonShapes.Rectangle;
            this.btnOpenModelsFolder.ClickColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnOpenModelsFolder.DrawImage = false;
            this.btnOpenModelsFolder.ForeColor = System.Drawing.Color.White;
            this.btnOpenModelsFolder.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.btnOpenModelsFolder.ImageSizeMode = HTAlt.WinForms.HTButton.ButtonImageSizeMode.None;
            this.btnOpenModelsFolder.Location = new System.Drawing.Point(500, 6);
            this.btnOpenModelsFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOpenModelsFolder.Name = "btnOpenModelsFolder";
            this.btnOpenModelsFolder.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnOpenModelsFolder.Size = new System.Drawing.Size(100, 28);
            this.btnOpenModelsFolder.TabIndex = 4;
            this.btnOpenModelsFolder.TabStop = false;
            this.btnOpenModelsFolder.Text = "Folders...";
            this.toolTip.SetToolTip(this.btnOpenModelsFolder, "Manage Model Folders");
            this.btnOpenModelsFolder.Click += new System.EventHandler(this.btnOpenModelsFolder_Click);
            // 
            // panel26
            // 
            this.panel26.Controls.Add(this.label10);
            this.panel26.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel26.Location = new System.Drawing.Point(0, 0);
            this.panel26.Margin = new System.Windows.Forms.Padding(0);
            this.panel26.Name = "panel26";
            this.panel26.Size = new System.Drawing.Size(547, 43);
            this.panel26.TabIndex = 87;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(3, 14);
            this.label10.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(149, 17);
            this.label10.TabIndex = 85;
            this.label10.Text = "Stable Diffusion Model";
            // 
            // panelUnloadModel
            // 
            this.panelUnloadModel.Controls.Add(this.tableLayoutPanel11);
            this.panelUnloadModel.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUnloadModel.Location = new System.Drawing.Point(0, 135);
            this.panelUnloadModel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelUnloadModel.Name = "panelUnloadModel";
            this.panelUnloadModel.Size = new System.Drawing.Size(1147, 43);
            this.panelUnloadModel.TabIndex = 24;
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.ColumnCount = 2;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel11.Controls.Add(this.panel4, 1, 0);
            this.tableLayoutPanel11.Controls.Add(this.panel21, 0, 0);
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel11.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel11.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 1;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(1147, 43);
            this.tableLayoutPanel11.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.checkboxUnloadModel);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(547, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(600, 43);
            this.panel4.TabIndex = 86;
            // 
            // checkboxUnloadModel
            // 
            this.checkboxUnloadModel.AutoSize = true;
            this.checkboxUnloadModel.Location = new System.Drawing.Point(7, 12);
            this.checkboxUnloadModel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkboxUnloadModel.Name = "checkboxUnloadModel";
            this.checkboxUnloadModel.Size = new System.Drawing.Size(18, 17);
            this.checkboxUnloadModel.TabIndex = 86;
            this.checkboxUnloadModel.UseVisualStyleBackColor = true;
            // 
            // panel21
            // 
            this.panel21.Controls.Add(this.label14);
            this.panel21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel21.Location = new System.Drawing.Point(0, 0);
            this.panel21.Margin = new System.Windows.Forms.Padding(0);
            this.panel21.Name = "panel21";
            this.panel21.Size = new System.Drawing.Size(547, 43);
            this.panel21.TabIndex = 85;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(3, 14);
            this.label14.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(377, 17);
            this.label14.TabIndex = 84;
            this.label14.Text = "Unload Model After Each Generation (No Idle RAM Usage)";
            // 
            // panelFullPrecision
            // 
            this.panelFullPrecision.Controls.Add(this.tableLayoutPanel1);
            this.panelFullPrecision.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFullPrecision.Location = new System.Drawing.Point(0, 92);
            this.panelFullPrecision.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelFullPrecision.Name = "panelFullPrecision";
            this.panelFullPrecision.Size = new System.Drawing.Size(1147, 43);
            this.panelFullPrecision.TabIndex = 11;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Controls.Add(this.panel8, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel7, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1147, 43);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.checkboxFullPrecision);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(547, 0);
            this.panel8.Margin = new System.Windows.Forms.Padding(0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(600, 43);
            this.panel8.TabIndex = 86;
            // 
            // checkboxFullPrecision
            // 
            this.checkboxFullPrecision.AutoSize = true;
            this.checkboxFullPrecision.Location = new System.Drawing.Point(7, 12);
            this.checkboxFullPrecision.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkboxFullPrecision.Name = "checkboxFullPrecision";
            this.checkboxFullPrecision.Size = new System.Drawing.Size(18, 17);
            this.checkboxFullPrecision.TabIndex = 86;
            this.checkboxFullPrecision.UseVisualStyleBackColor = true;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.label2);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Margin = new System.Windows.Forms.Padding(0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(547, 43);
            this.panel7.TabIndex = 85;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 14);
            this.label2.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(495, 17);
            this.label2.TabIndex = 84;
            this.label2.Text = "Use Full Precision (Useless in Most Cases, but May Fix GTX 16-Series Cards)";
            // 
            // panelImplementation
            // 
            this.panelImplementation.Controls.Add(this.tableLayoutPanel3);
            this.panelImplementation.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelImplementation.Location = new System.Drawing.Point(0, 49);
            this.panelImplementation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelImplementation.Name = "panelImplementation";
            this.panelImplementation.Size = new System.Drawing.Size(1147, 43);
            this.panelImplementation.TabIndex = 13;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel3.Controls.Add(this.panel12, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.panel13, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1147, 43);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.comboxImplementation);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel12.Location = new System.Drawing.Point(547, 0);
            this.panel12.Margin = new System.Windows.Forms.Padding(0);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(600, 43);
            this.panel12.TabIndex = 86;
            // 
            // comboxImplementation
            // 
            this.comboxImplementation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboxImplementation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboxImplementation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxImplementation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboxImplementation.ForeColor = System.Drawing.Color.White;
            this.comboxImplementation.FormattingEnabled = true;
            this.comboxImplementation.Location = new System.Drawing.Point(0, 9);
            this.comboxImplementation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboxImplementation.Name = "comboxImplementation";
            this.comboxImplementation.Size = new System.Drawing.Size(599, 24);
            this.comboxImplementation.TabIndex = 107;
            this.comboxImplementation.SelectedIndexChanged += new System.EventHandler(this.comboxImplementation_SelectedIndexChanged);
            // 
            // panel13
            // 
            this.panel13.Controls.Add(this.label4);
            this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel13.Location = new System.Drawing.Point(0, 0);
            this.panel13.Margin = new System.Windows.Forms.Padding(0);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(547, 43);
            this.panel13.TabIndex = 85;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(3, 14);
            this.label4.Margin = new System.Windows.Forms.Padding(11, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(220, 17);
            this.label4.TabIndex = 84;
            this.label4.Text = "Image Generation Implementation";
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.label1);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel11.Location = new System.Drawing.Point(0, 0);
            this.panel11.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(1147, 49);
            this.panel11.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 23);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(219, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Stable Diffusion Settings";
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 200;
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 200;
            this.toolTip.ReshowDelay = 40;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(1179, 949);
            this.Controls.Add(this.parentPanel);
            this.Controls.Add(this.titleLabel);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1594, 1171);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1141, 654);
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.Shown += new System.EventHandler(this.SettingsForm_Shown);
            this.parentPanel.ResumeLayout(false);
            this.panelNotify.ResumeLayout(false);
            this.notificationPanel.ResumeLayout(false);
            this.panel14.ResumeLayout(false);
            this.panel15.ResumeLayout(false);
            this.panel15.PerformLayout();
            this.panelAdvancedMode.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.panel16.ResumeLayout(false);
            this.panel16.PerformLayout();
            this.panel17.ResumeLayout(false);
            this.panel17.PerformLayout();
            this.panelInitImageRetainAspectRatio.ResumeLayout(false);
            this.tableLayoutPanel15.ResumeLayout(false);
            this.panel39.ResumeLayout(false);
            this.panel39.PerformLayout();
            this.panel40.ResumeLayout(false);
            this.panel40.PerformLayout();
            this.panelAutoSetResForInitImg.ResumeLayout(false);
            this.tableLayoutPanel14.ResumeLayout(false);
            this.panel37.ResumeLayout(false);
            this.panel37.PerformLayout();
            this.panel38.ResumeLayout(false);
            this.panel38.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelSaveUnprocessedImages.ResumeLayout(false);
            this.panel29.ResumeLayout(false);
            this.panel30.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.panel31.ResumeLayout(false);
            this.panel31.PerformLayout();
            this.panel32.ResumeLayout(false);
            this.panel32.PerformLayout();
            this.panelMultiPromptsSameSeed.ResumeLayout(false);
            this.panel18.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel22.ResumeLayout(false);
            this.panel22.PerformLayout();
            this.panelFavsDir.ResumeLayout(false);
            this.tableLayoutPanel13.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel35.ResumeLayout(false);
            this.panel35.PerformLayout();
            this.panelPromptInFilename.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.panel19.ResumeLayout(false);
            this.panel19.PerformLayout();
            this.panel20.ResumeLayout(false);
            this.panel20.PerformLayout();
            this.panelPromptSubfolders.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panelOutPath.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.panel23.ResumeLayout(false);
            this.panel23.PerformLayout();
            this.panel24.ResumeLayout(false);
            this.panel24.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panelCudaDevice.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.panel27.ResumeLayout(false);
            this.panel28.ResumeLayout(false);
            this.panel28.PerformLayout();
            this.panelVae.ResumeLayout(false);
            this.tableLayoutPanel12.ResumeLayout(false);
            this.panel33.ResumeLayout(false);
            this.panel34.ResumeLayout(false);
            this.panel34.PerformLayout();
            this.panelSdModel.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.panel25.ResumeLayout(false);
            this.panel26.ResumeLayout(false);
            this.panel26.PerformLayout();
            this.panelUnloadModel.ResumeLayout(false);
            this.tableLayoutPanel11.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel21.ResumeLayout(false);
            this.panel21.PerformLayout();
            this.panelFullPrecision.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panelImplementation.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            this.panel13.ResumeLayout(false);
            this.panel13.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Panel parentPanel;
        private System.Windows.Forms.Panel panelImplementation;
        private System.Windows.Forms.Panel panelPromptSubfolders;
        private System.Windows.Forms.Panel panelFullPrecision;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelAdvancedMode;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.CheckBox checkboxFullPrecision;
        private System.Windows.Forms.CheckBox checkboxFolderPerPrompt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panelPromptInFilename;
        private System.Windows.Forms.Panel panelMultiPromptsSameSeed;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Panel panel16;
        private System.Windows.Forms.CheckBox checkboxAdvancedMode;
        private System.Windows.Forms.Panel panel17;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel18;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Panel panel19;
        private System.Windows.Forms.CheckBox checkboxPromptInFilename;
        private System.Windows.Forms.Panel panel20;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.CheckBox checkboxMultiPromptsSameSeed;
        private System.Windows.Forms.Panel panel22;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panelOutPath;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Panel panel23;
        private System.Windows.Forms.Panel panel24;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textboxOutPath;
        private HTAlt.WinForms.HTButton btnOutPathBrowse;
        private System.Windows.Forms.Panel panelSdModel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.Panel panel25;
        private System.Windows.Forms.Panel panel26;
        private System.Windows.Forms.Label label10;
        private HTAlt.WinForms.HTButton btnOpenModelsFolder;
        private System.Windows.Forms.ComboBox comboxSdModel;
        private System.Windows.Forms.Panel panelCudaDevice;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.Panel panel27;
        private System.Windows.Forms.ComboBox comboxCudaDevice;
        private System.Windows.Forms.Panel panel28;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ToolTip toolTip;
        private HTAlt.WinForms.HTButton btnRefreshModelsDropdown;
        private System.Windows.Forms.CheckBox checkboxSamplerInFilename;
        private System.Windows.Forms.CheckBox checkboxScaleInFilename;
        private System.Windows.Forms.CheckBox checkboxSeedInFilename;
        private System.Windows.Forms.CheckBox checkboxModelInFilename;
        private System.Windows.Forms.Panel panelSaveUnprocessedImages;
        private System.Windows.Forms.Panel panel29;
        private System.Windows.Forms.Panel panel30;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.Panel panel31;
        private System.Windows.Forms.CheckBox checkboxSaveUnprocessedImages;
        private System.Windows.Forms.Panel panel32;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panelUnloadModel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.CheckBox checkboxUnloadModel;
        private System.Windows.Forms.Panel panel21;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox checkboxOutputIgnoreWildcards;
        private System.Windows.Forms.CheckBox checkboxFolderPerSession;
        private System.Windows.Forms.Panel panelVae;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
        private System.Windows.Forms.Panel panel33;
        private HTAlt.WinForms.HTButton btnRefreshModelsDropdownVae;
        private System.Windows.Forms.ComboBox comboxSdModelVae;
        private HTAlt.WinForms.HTButton btnOpenModelsFolderVae;
        private System.Windows.Forms.Panel panel34;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Panel panelNotify;
        private System.Windows.Forms.TableLayoutPanel notificationPanel;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.ComboBox comboxNotify;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panelFavsDir;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox textboxFavsPath;
        private HTAlt.WinForms.HTButton btnFavsPathBrowse;
        private System.Windows.Forms.Panel panel35;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox comboxImplementation;
        private System.Windows.Forms.Panel panelAutoSetResForInitImg;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
        private System.Windows.Forms.Panel panel37;
        private System.Windows.Forms.CheckBox checkboxAutoSetResForInitImg;
        private System.Windows.Forms.Panel panel38;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Panel panelInitImageRetainAspectRatio;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel15;
        private System.Windows.Forms.Panel panel39;
        private System.Windows.Forms.CheckBox checkboxInitImageRetainAspectRatio;
        private System.Windows.Forms.Panel panel40;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ComboBox comboxTimestampInFilename;
    }
}