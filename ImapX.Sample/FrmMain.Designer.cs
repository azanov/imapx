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
            this.lstFolders = new System.Windows.Forms.ListBox();
            this.pgbFetchMails = new System.Windows.Forms.ProgressBar();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lstMails = new System.Windows.Forms.ListBox();
            this.wbrMain = new System.Windows.Forms.WebBrowser();
            this.pnlAttachments = new System.Windows.Forms.Panel();
            this.lsvAttachments = new System.Windows.Forms.ListView();
            this.istAttachments = new System.Windows.Forms.ImageList(this.components);
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblSubject = new System.Windows.Forms.Label();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bgwMain = new System.ComponentModel.BackgroundWorker();
            this.sfdMain = new System.Windows.Forms.SaveFileDialog();
            this.tltMain = new System.Windows.Forms.ToolTip(this.components);
            this.mnuAttachment = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblTo = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.pnlAttachments.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            this.mnuAttachment.SuspendLayout();
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
            this.pnlLeft.Controls.Add(this.lstFolders);
            this.pnlLeft.Controls.Add(this.label1);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(222, 657);
            this.pnlLeft.TabIndex = 1;
            // 
            // lstFolders
            // 
            this.lstFolders.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstFolders.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstFolders.FormattingEnabled = true;
            this.lstFolders.ItemHeight = 16;
            this.lstFolders.Location = new System.Drawing.Point(18, 48);
            this.lstFolders.Name = "lstFolders";
            this.lstFolders.Size = new System.Drawing.Size(188, 592);
            this.lstFolders.TabIndex = 1;
            this.lstFolders.SelectedIndexChanged += new System.EventHandler(this.lstFolders_SelectedIndexChanged);
            // 
            // pgbFetchMails
            // 
            this.pgbFetchMails.Dock = System.Windows.Forms.DockStyle.Top;
            this.pgbFetchMails.Location = new System.Drawing.Point(12, 12);
            this.pgbFetchMails.Name = "pgbFetchMails";
            this.pgbFetchMails.Size = new System.Drawing.Size(267, 28);
            this.pgbFetchMails.Step = 1;
            this.pgbFetchMails.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pgbFetchMails.TabIndex = 1;
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
            this.splitContainer1.Panel1.Controls.Add(this.pgbFetchMails);
            this.splitContainer1.Panel1.Controls.Add(this.lstMails);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(12);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel2.Controls.Add(this.wbrMain);
            this.splitContainer1.Panel2.Controls.Add(this.pnlAttachments);
            this.splitContainer1.Panel2.Controls.Add(this.pnlInfo);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(5);
            this.splitContainer1.Size = new System.Drawing.Size(875, 657);
            this.splitContainer1.SplitterDistance = 291;
            this.splitContainer1.TabIndex = 2;
            // 
            // lstMails
            // 
            this.lstMails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.lstMails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstMails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstMails.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstMails.ItemHeight = 18;
            this.lstMails.Location = new System.Drawing.Point(12, 12);
            this.lstMails.Margin = new System.Windows.Forms.Padding(10);
            this.lstMails.Name = "lstMails";
            this.lstMails.Size = new System.Drawing.Size(267, 633);
            this.lstMails.TabIndex = 0;
            this.lstMails.SelectedIndexChanged += new System.EventHandler(this.lsvMails_SelectedIndexChanged);
            // 
            // wbrMain
            // 
            this.wbrMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbrMain.Location = new System.Drawing.Point(5, 105);
            this.wbrMain.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbrMain.Name = "wbrMain";
            this.wbrMain.Size = new System.Drawing.Size(570, 463);
            this.wbrMain.TabIndex = 2;
            this.wbrMain.Visible = false;
            // 
            // pnlAttachments
            // 
            this.pnlAttachments.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(199)))), ((int)(((byte)(83)))));
            this.pnlAttachments.Controls.Add(this.lsvAttachments);
            this.pnlAttachments.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAttachments.Location = new System.Drawing.Point(5, 568);
            this.pnlAttachments.Name = "pnlAttachments";
            this.pnlAttachments.Padding = new System.Windows.Forms.Padding(12);
            this.pnlAttachments.Size = new System.Drawing.Size(570, 84);
            this.pnlAttachments.TabIndex = 3;
            this.pnlAttachments.Visible = false;
            // 
            // lsvAttachments
            // 
            this.lsvAttachments.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(199)))), ((int)(((byte)(83)))));
            this.lsvAttachments.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lsvAttachments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvAttachments.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lsvAttachments.Location = new System.Drawing.Point(12, 12);
            this.lsvAttachments.MultiSelect = false;
            this.lsvAttachments.Name = "lsvAttachments";
            this.lsvAttachments.Size = new System.Drawing.Size(546, 60);
            this.lsvAttachments.SmallImageList = this.istAttachments;
            this.lsvAttachments.TabIndex = 0;
            this.tltMain.SetToolTip(this.lsvAttachments, "Double-click on attachment to open it, right-click to save");
            this.lsvAttachments.UseCompatibleStateImageBehavior = false;
            this.lsvAttachments.View = System.Windows.Forms.View.SmallIcon;
            this.lsvAttachments.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lsvAttachments_MouseClick);
            this.lsvAttachments.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lsvAttachments_MouseDoubleClick);
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
            this.pnlInfo.Size = new System.Drawing.Size(570, 100);
            this.pnlInfo.TabIndex = 1;
            this.pnlInfo.Visible = false;
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
            // bgwMain
            // 
            this.bgwMain.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwMain_DoWork);
            this.bgwMain.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwMain_RunWorkerCompleted);
            // 
            // sfdMain
            // 
            this.sfdMain.ShowHelp = true;
            this.sfdMain.Title = "Save attachment";
            // 
            // mnuAttachment
            // 
            this.mnuAttachment.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.mnuAttachment.Name = "mnuAttachment";
            this.mnuAttachment.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.mnuAttachment.ShowImageMargin = false;
            this.mnuAttachment.Size = new System.Drawing.Size(128, 70);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.saveAsToolStripMenuItem.Text = "Save as...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
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
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1097, 657);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.pnlLeft);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ImapX 2 Test Application";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.pnlAttachments.ResumeLayout(false);
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            this.mnuAttachment.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.ListBox lstFolders;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox lstMails;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.WebBrowser wbrMain;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblSubject;
        private System.Windows.Forms.ListView lsvAttachments;
        private System.Windows.Forms.ProgressBar pgbFetchMails;
        private System.ComponentModel.BackgroundWorker bgwMain;
        private System.Windows.Forms.Panel pnlAttachments;
        private System.Windows.Forms.SaveFileDialog sfdMain;
        private System.Windows.Forms.ToolTip tltMain;
        private System.Windows.Forms.ContextMenuStrip mnuAttachment;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ImageList istAttachments;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label label4;
    }
}

