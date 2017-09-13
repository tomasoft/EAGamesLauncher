namespace EAGamesLauncher
{
    partial class FormMain
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
            this.eaGamesFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.icosList = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtEaRootFolder = new System.Windows.Forms.Label();
            this.btnChooseRootFolder = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lstInstalledGames = new System.Windows.Forms.ListView();
            this.launcherNotify = new System.Windows.Forms.NotifyIcon(this.components);
            this.modsmenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // eaGamesFolder
            // 
            this.eaGamesFolder.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.eaGamesFolder.ShowNewFolderButton = false;
            // 
            // icosList
            // 
            this.icosList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.icosList.ImageSize = new System.Drawing.Size(32, 32);
            this.icosList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel1.Controls.Add(this.txtEaRootFolder);
            this.splitContainer1.Panel1.Controls.Add(this.btnChooseRootFolder);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lstInstalledGames);
            this.splitContainer1.Size = new System.Drawing.Size(654, 376);
            this.splitContainer1.SplitterDistance = 36;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 4;
            // 
            // txtEaRootFolder
            // 
            this.txtEaRootFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEaRootFolder.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEaRootFolder.Location = new System.Drawing.Point(196, 7);
            this.txtEaRootFolder.Name = "txtEaRootFolder";
            this.txtEaRootFolder.Size = new System.Drawing.Size(397, 23);
            this.txtEaRootFolder.TabIndex = 6;
            this.txtEaRootFolder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnChooseRootFolder
            // 
            this.btnChooseRootFolder.Font = new System.Drawing.Font("Arial Black", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChooseRootFolder.Location = new System.Drawing.Point(599, 7);
            this.btnChooseRootFolder.Name = "btnChooseRootFolder";
            this.btnChooseRootFolder.Size = new System.Drawing.Size(43, 23);
            this.btnChooseRootFolder.TabIndex = 5;
            this.btnChooseRootFolder.Text = "...";
            this.btnChooseRootFolder.UseVisualStyleBackColor = true;
            this.btnChooseRootFolder.Click += new System.EventHandler(this.BtnChooseRootFolder_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 14);
            this.label1.TabIndex = 3;
            this.label1.Text = "EA Games Mods Root folder: ";
            // 
            // lstInstalledGames
            // 
            this.lstInstalledGames.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lstInstalledGames.BackgroundImage = global::EAGamesLauncher.Properties.Resources.bg6;
            this.lstInstalledGames.BackgroundImageTiled = true;
            this.lstInstalledGames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstInstalledGames.Font = new System.Drawing.Font("Arial Rounded MT Bold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstInstalledGames.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstInstalledGames.Location = new System.Drawing.Point(0, 0);
            this.lstInstalledGames.MultiSelect = false;
            this.lstInstalledGames.Name = "lstInstalledGames";
            this.lstInstalledGames.Size = new System.Drawing.Size(654, 339);
            this.lstInstalledGames.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstInstalledGames.TabIndex = 4;
            this.lstInstalledGames.TileSize = new System.Drawing.Size(32, 32);
            this.lstInstalledGames.UseCompatibleStateImageBehavior = false;
            this.lstInstalledGames.View = System.Windows.Forms.View.List;
            this.lstInstalledGames.DoubleClick += new System.EventHandler(this.LstInstalledGames_DoubleClick);
            // 
            // launcherNotify
            // 
            this.launcherNotify.ContextMenuStrip = this.modsmenuStrip;
            this.launcherNotify.Icon = global::EAGamesLauncher.Properties.Resources.launcher_icon;
            this.launcherNotify.Text = "EA C&C Mods Launcher";
            this.launcherNotify.Visible = true;
            this.launcherNotify.DoubleClick += new System.EventHandler(this.LauncherNotify_DoubleClick);
            // 
            // modsmenuStrip
            // 
            this.modsmenuStrip.Name = "modsmenuStrip";
            this.modsmenuStrip.ShowImageMargin = false;
            this.modsmenuStrip.ShowItemToolTips = false;
            this.modsmenuStrip.Size = new System.Drawing.Size(36, 4);
            this.modsmenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ModsmenuStrip_ItemClicked);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 376);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Icon = global::EAGamesLauncher.Properties.Resources.launcher_icon;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(670, 415);
            this.Name = "FormMain";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EA C&C Launcher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.FrmMain_Resize);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog eaGamesFolder;
        private System.Windows.Forms.ImageList icosList;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnChooseRootFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lstInstalledGames;
        private System.Windows.Forms.Label txtEaRootFolder;
        private System.Windows.Forms.NotifyIcon launcherNotify;
        private System.Windows.Forms.ContextMenuStrip modsmenuStrip;
    }
}

