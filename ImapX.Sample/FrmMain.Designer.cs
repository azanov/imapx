namespace ImapX.Sample
{
    partial class FrmMain
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
            this.label1 = new System.Windows.Forms.Label();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.trwFolders = new System.Windows.Forms.TreeView();
            this.pgbFetchMails = new System.Windows.Forms.ProgressBar();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lblSelectFolder = new System.Windows.Forms.Label();
            this.lsvMails = new System.Windows.Forms.ListView();
            this.clmMessages = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mnuMessages = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.moveToFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportForReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wbrMain = new System.Windows.Forms.WebBrowser();
            this.pnlAttachments = new System.Windows.Forms.Panel();
            this.lsvAttachments = new System.Windows.Forms.ListView();
            this.mnuAttachment = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.istAttachments = new System.Windows.Forms.ImageList(this.components);
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.lblTo = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblSubject = new System.Windows.Forms.Label();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sfdMain = new System.Windows.Forms.SaveFileDialog();
            this.tltMain = new System.Windows.Forms.ToolTip(this.components);
            this.mnuFolders = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.emptyFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.markAsReadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.mnuMessages.SuspendLayout();
            this.pnlAttachments.SuspendLayout();
            this.mnuAttachment.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            this.mnuFolders.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Folders";
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.trwFolders);
            this.pnlLeft.Controls.Add(this.label1);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(222, 557);
            this.pnlLeft.TabIndex = 1;
            // 
            // trwFolders
            // 
            this.trwFolders.BackColor = System.Drawing.Color.White;
            this.trwFolders.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.trwFolders.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trwFolders.HideSelection = false;
            this.trwFolders.Location = new System.Drawing.Point(11, 48);
            this.trwFolders.Name = "trwFolders";
            this.trwFolders.Size = new System.Drawing.Size(200, 492);
            this.trwFolders.TabIndex = 2;
            this.trwFolders.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trwFolders_AfterSelect);
            this.trwFolders.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trwFolders_NodeMouseClick);
            // 
            // pgbFetchMails
            // 
            this.pgbFetchMails.Dock = System.Windows.Forms.DockStyle.Top;
            this.pgbFetchMails.Location = new System.Drawing.Point(12, 12);
            this.pgbFetchMails.Name = "pgbFetchMails";
            this.pgbFetchMails.Size = new System.Drawing.Size(229, 28);
            this.pgbFetchMails.Step = 1;
            this.pgbFetchMails.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pgbFetchMails.TabIndex = 1;
            this.pgbFetchMails.Visible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(222, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.splitContainer1.Panel1.Controls.Add(this.lblSelectFolder);
            this.splitContainer1.Panel1.Controls.Add(this.pgbFetchMails);
            this.splitContainer1.Panel1.Controls.Add(this.lsvMails);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(12);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel2.Controls.Add(this.wbrMain);
            this.splitContainer1.Panel2.Controls.Add(this.pnlAttachments);
            this.splitContainer1.Panel2.Controls.Add(this.pnlInfo);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(5);
            this.splitContainer1.Size = new System.Drawing.Size(762, 557);
            this.splitContainer1.SplitterDistance = 253;
            this.splitContainer1.TabIndex = 2;
            // 
            // lblSelectFolder
            // 
            this.lblSelectFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSelectFolder.Location = new System.Drawing.Point(12, 40);
            this.lblSelectFolder.Name = "lblSelectFolder";
            this.lblSelectFolder.Size = new System.Drawing.Size(229, 16);
            this.lblSelectFolder.TabIndex = 2;
            this.lblSelectFolder.Text = "Please select a folder";
            this.lblSelectFolder.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lsvMails
            // 
            this.lsvMails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.lsvMails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lsvMails.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmMessages});
            this.lsvMails.ContextMenuStrip = this.mnuMessages;
            this.lsvMails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvMails.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lsvMails.FullRowSelect = true;
            this.lsvMails.GridLines = true;
            this.lsvMails.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lsvMails.HideSelection = false;
            this.lsvMails.Location = new System.Drawing.Point(12, 12);
            this.lsvMails.Margin = new System.Windows.Forms.Padding(10);
            this.lsvMails.Name = "lsvMails";
            this.lsvMails.Size = new System.Drawing.Size(229, 533);
            this.lsvMails.TabIndex = 0;
            this.lsvMails.UseCompatibleStateImageBehavior = false;
            this.lsvMails.View = System.Windows.Forms.View.Details;
            this.lsvMails.VirtualMode = true;
            this.lsvMails.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.lsvMails_RetrieveVirtualItem);
            this.lsvMails.SelectedIndexChanged += new System.EventHandler(this.lsvMails_SelectedIndexChanged);
            this.lsvMails.SizeChanged += new System.EventHandler(this.FrmMainOrLsvMails_SizeChanged);
            // 
            // clmMessages
            // 
            this.clmMessages.Text = "Messages";
            // 
            // mnuMessages
            // 
            this.mnuMessages.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveToFolderToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.markAsReadToolStripMenuItem,
            this.exportForReportToolStripMenuItem});
            this.mnuMessages.Name = "mnuMessages";
            this.mnuMessages.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.mnuMessages.Size = new System.Drawing.Size(170, 114);
            this.mnuMessages.Opening += new System.ComponentModel.CancelEventHandler(this.mnuMessages_Opening);
            // 
            // moveToFolderToolStripMenuItem
            // 
            this.moveToFolderToolStripMenuItem.Name = "moveToFolderToolStripMenuItem";
            this.moveToFolderToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.moveToFolderToolStripMenuItem.Text = "Move to folder...";
            this.moveToFolderToolStripMenuItem.Click += new System.EventHandler(this.moveToFolderToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // exportForReportToolStripMenuItem
            // 
            this.exportForReportToolStripMenuItem.Name = "exportForReportToolStripMenuItem";
            this.exportForReportToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.exportForReportToolStripMenuItem.Text = "Export for report...";
            this.exportForReportToolStripMenuItem.Click += new System.EventHandler(this.exportForReportToolStripMenuItem_Click);
            // 
            // wbrMain
            // 
            this.wbrMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbrMain.Location = new System.Drawing.Point(5, 105);
            this.wbrMain.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbrMain.Name = "wbrMain";
            this.wbrMain.Size = new System.Drawing.Size(495, 363);
            this.wbrMain.TabIndex = 2;
            this.wbrMain.Visible = false;
            // 
            // pnlAttachments
            // 
            this.pnlAttachments.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(199)))), ((int)(((byte)(83)))));
            this.pnlAttachments.Controls.Add(this.lsvAttachments);
            this.pnlAttachments.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAttachments.Location = new System.Drawing.Point(5, 468);
            this.pnlAttachments.Name = "pnlAttachments";
            this.pnlAttachments.Padding = new System.Windows.Forms.Padding(12);
            this.pnlAttachments.Size = new System.Drawing.Size(495, 84);
            this.pnlAttachments.TabIndex = 3;
            this.pnlAttachments.Visible = false;
            // 
            // lsvAttachments
            // 
            this.lsvAttachments.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(199)))), ((int)(((byte)(83)))));
            this.lsvAttachments.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lsvAttachments.ContextMenuStrip = this.mnuAttachment;
            this.lsvAttachments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvAttachments.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lsvAttachments.Location = new System.Drawing.Point(12, 12);
            this.lsvAttachments.MultiSelect = false;
            this.lsvAttachments.Name = "lsvAttachments";
            this.lsvAttachments.Size = new System.Drawing.Size(471, 60);
            this.lsvAttachments.SmallImageList = this.istAttachments;
            this.lsvAttachments.TabIndex = 0;
            this.tltMain.SetToolTip(this.lsvAttachments, "Double-click on attachment to open it, right-click to save");
            this.lsvAttachments.UseCompatibleStateImageBehavior = false;
            this.lsvAttachments.View = System.Windows.Forms.View.SmallIcon;
            this.lsvAttachments.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lsvAttachments_MouseDoubleClick);
            // 
            // mnuAttachment
            // 
            this.mnuAttachment.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.mnuAttachment.Name = "mnuAttachment";
            this.mnuAttachment.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.mnuAttachment.Size = new System.Drawing.Size(122, 48);
            this.mnuAttachment.Opening += new System.ComponentModel.CancelEventHandler(this.mnuAttachment_Opening);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.saveAsToolStripMenuItem.Text = "Save as...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // istAttachments
            // 
            this.istAttachments.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.istAttachments.ImageSize = new System.Drawing.Size(16, 16);
            this.istAttachments.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // pnlInfo
            // 
            this.pnlInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(199)))), ((int)(((byte)(83)))));
            this.pnlInfo.Controls.Add(this.lblTo);
            this.pnlInfo.Controls.Add(this.label4);
            this.pnlInfo.Controls.Add(this.lblTime);
            this.pnlInfo.Controls.Add(this.lblFrom);
            this.pnlInfo.Controls.Add(this.label3);
            this.pnlInfo.Controls.Add(this.lblSubject);
            this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInfo.Location = new System.Drawing.Point(5, 5);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(495, 100);
            this.pnlInfo.TabIndex = 1;
            this.pnlInfo.Visible = false;
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(61, 52);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(27, 16);
            this.lblTo.TabIndex = 6;
            this.lblTo.Text = "[to]";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "To:";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(13, 68);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(41, 16);
            this.lblTime.TabIndex = 4;
            this.lblTime.Text = "[time]";
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(61, 35);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(42, 16);
            this.lblFrom.TabIndex = 3;
            this.lblFrom.Text = "[from]";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "From:";
            // 
            // lblSubject
            // 
            this.lblSubject.AutoSize = true;
            this.lblSubject.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubject.Location = new System.Drawing.Point(12, 12);
            this.lblSubject.Name = "lblSubject";
            this.lblSubject.Size = new System.Drawing.Size(71, 20);
            this.lblSubject.TabIndex = 1;
            this.lblSubject.Text = "[Subject]";
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Messages";
            this.columnHeader1.Width = 257;
            // 
            // sfdMain
            // 
            this.sfdMain.ShowHelp = true;
            this.sfdMain.Title = "Save attachment";
            // 
            // mnuFolders
            // 
            this.mnuFolders.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.emptyFolderToolStripMenuItem});
            this.mnuFolders.Name = "mnuFolders";
            this.mnuFolders.Size = new System.Drawing.Size(189, 26);
            // 
            // emptyFolderToolStripMenuItem
            // 
            this.emptyFolderToolStripMenuItem.Name = "emptyFolderToolStripMenuItem";
            this.emptyFolderToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.emptyFolderToolStripMenuItem.Text = "Empty selected folder";
            this.emptyFolderToolStripMenuItem.Click += new System.EventHandler(this.emptyFolderToolStripMenuItem_Click);
            // 
            // markAsReadToolStripMenuItem
            // 
            this.markAsReadToolStripMenuItem.Name = "markAsReadToolStripMenuItem";
            this.markAsReadToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.markAsReadToolStripMenuItem.Text = "Mark as read";
            this.markAsReadToolStripMenuItem.Click += new System.EventHandler(this.markAsReadToolStripMenuItem_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(984, 557);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.pnlLeft);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ImapX 2 Test Application";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.SizeChanged += new System.EventHandler(this.FrmMainOrLsvMails_SizeChanged);
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.mnuMessages.ResumeLayout(false);
            this.pnlAttachments.ResumeLayout(false);
            this.mnuAttachment.ResumeLayout(false);
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            this.mnuFolders.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView lsvMails;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.WebBrowser wbrMain;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblSubject;
        private System.Windows.Forms.ListView lsvAttachments;
        private System.Windows.Forms.ProgressBar pgbFetchMails;
        private System.Windows.Forms.Panel pnlAttachments;
        private System.Windows.Forms.SaveFileDialog sfdMain;
        private System.Windows.Forms.ToolTip tltMain;
        private System.Windows.Forms.ContextMenuStrip mnuAttachment;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ImageList istAttachments;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TreeView trwFolders;
        private System.Windows.Forms.Label lblSelectFolder;
        private System.Windows.Forms.ContextMenuStrip mnuMessages;
        private System.Windows.Forms.ToolStripMenuItem moveToFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader clmMessages;
        private System.Windows.Forms.ToolStripMenuItem exportForReportToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip mnuFolders;
        private System.Windows.Forms.ToolStripMenuItem emptyFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem markAsReadToolStripMenuItem;
    }
}

