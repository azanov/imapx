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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.pnlFolders = new System.Windows.Forms.Panel();
            this.trwFolders = new System.Windows.Forms.TreeView();
            this.panel12 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnlFavorites = new System.Windows.Forms.Panel();
            this.lnkArchive = new System.Windows.Forms.LinkLabel();
            this.lnkAll = new System.Windows.Forms.LinkLabel();
            this.lnkTrash = new System.Windows.Forms.LinkLabel();
            this.lnkJunk = new System.Windows.Forms.LinkLabel();
            this.lnkFlagged = new System.Windows.Forms.LinkLabel();
            this.lnkImportant = new System.Windows.Forms.LinkLabel();
            this.lnkDrafts = new System.Windows.Forms.LinkLabel();
            this.lnkSent = new System.Windows.Forms.LinkLabel();
            this.lnkInbox = new System.Windows.Forms.LinkLabel();
            this.lblFavorites = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.lblIdle = new System.Windows.Forms.Label();
            this.pnlWrap = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pnlMessages = new System.Windows.Forms.Panel();
            this.lsvMessages = new System.Windows.Forms.ListView();
            this.clmMessages = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mnuMessage = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.seenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.copyToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.exportMessageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.istPlaceHolder = new System.Windows.Forms.ImageList(this.components);
            this.panel5 = new System.Windows.Forms.Panel();
            this.lblFolder = new System.Windows.Forms.Label();
            this.pnlSelectFolder = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlLoading = new System.Windows.Forms.Panel();
            this.pgbMessages = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlView = new System.Windows.Forms.Panel();
            this.wbrMain = new System.Windows.Forms.WebBrowser();
            this.pnlAttachments = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lsvAttachments = new System.Windows.Forms.ListView();
            this.mnuAttachment = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.downloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.istAttachments = new System.Windows.Forms.ImageList(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.pnlDownloadingBody = new System.Windows.Forms.Panel();
            this.lblFailedDownloadBody = new System.Windows.Forms.Label();
            this.lblDownloadingBody = new System.Windows.Forms.Label();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.pnlEmbeddedResources = new System.Windows.Forms.Panel();
            this.lnkDownloadEmbeddedResources = new System.Windows.Forms.LinkLabel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.lblLabels = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblSubject = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.mnuFolder = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.addSubfolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.importMessageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.emptyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ofdImportMessage = new System.Windows.Forms.OpenFileDialog();
            this.sfdExportMessage = new System.Windows.Forms.SaveFileDialog();
            this.sfdSaveAttachment = new System.Windows.Forms.SaveFileDialog();
            this.pnlFolders.SuspendLayout();
            this.pnlFavorites.SuspendLayout();
            this.panel9.SuspendLayout();
            this.pnlWrap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.pnlMessages.SuspendLayout();
            this.mnuMessage.SuspendLayout();
            this.pnlSelectFolder.SuspendLayout();
            this.pnlLoading.SuspendLayout();
            this.pnlView.SuspendLayout();
            this.pnlAttachments.SuspendLayout();
            this.panel4.SuspendLayout();
            this.mnuAttachment.SuspendLayout();
            this.pnlDownloadingBody.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            this.pnlEmbeddedResources.SuspendLayout();
            this.mnuFolder.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFolders
            // 
            this.pnlFolders.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.pnlFolders.Controls.Add(this.trwFolders);
            this.pnlFolders.Controls.Add(this.panel12);
            this.pnlFolders.Controls.Add(this.panel11);
            this.pnlFolders.Controls.Add(this.panel10);
            this.pnlFolders.Controls.Add(this.panel1);
            this.pnlFolders.Controls.Add(this.panel3);
            this.pnlFolders.Controls.Add(this.panel2);
            this.pnlFolders.Controls.Add(this.pnlFavorites);
            this.pnlFolders.Controls.Add(this.lblFavorites);
            this.pnlFolders.Controls.Add(this.panel9);
            this.pnlFolders.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlFolders.Location = new System.Drawing.Point(0, 0);
            this.pnlFolders.Name = "pnlFolders";
            this.pnlFolders.Padding = new System.Windows.Forms.Padding(10, 10, 0, 0);
            this.pnlFolders.Size = new System.Drawing.Size(220, 557);
            this.pnlFolders.TabIndex = 0;
            // 
            // trwFolders
            // 
            this.trwFolders.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.trwFolders.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.trwFolders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trwFolders.FullRowSelect = true;
            this.trwFolders.HideSelection = false;
            this.trwFolders.ItemHeight = 27;
            this.trwFolders.Location = new System.Drawing.Point(10, 327);
            this.trwFolders.Name = "trwFolders";
            this.trwFolders.ShowRootLines = false;
            this.trwFolders.Size = new System.Drawing.Size(210, 156);
            this.trwFolders.TabIndex = 17;
            this.trwFolders.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.trwFolders_BeforeSelect);
            this.trwFolders.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trwFolders_AfterSelect);
            this.trwFolders.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trwFolders_NodeMouseClick);
            // 
            // panel12
            // 
            this.panel12.BackColor = System.Drawing.Color.Transparent;
            this.panel12.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel12.Location = new System.Drawing.Point(10, 483);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(210, 18);
            this.panel12.TabIndex = 22;
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
            this.panel11.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel11.Location = new System.Drawing.Point(10, 501);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(210, 1);
            this.panel11.TabIndex = 21;
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.Transparent;
            this.panel10.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel10.Location = new System.Drawing.Point(10, 502);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(210, 18);
            this.panel10.TabIndex = 20;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(10, 309);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(210, 18);
            this.panel1.TabIndex = 17;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 308);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(210, 1);
            this.panel3.TabIndex = 16;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(10, 290);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(210, 18);
            this.panel2.TabIndex = 15;
            // 
            // pnlFavorites
            // 
            this.pnlFavorites.AutoSize = true;
            this.pnlFavorites.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlFavorites.BackColor = System.Drawing.Color.Transparent;
            this.pnlFavorites.Controls.Add(this.lnkArchive);
            this.pnlFavorites.Controls.Add(this.lnkAll);
            this.pnlFavorites.Controls.Add(this.lnkTrash);
            this.pnlFavorites.Controls.Add(this.lnkJunk);
            this.pnlFavorites.Controls.Add(this.lnkFlagged);
            this.pnlFavorites.Controls.Add(this.lnkImportant);
            this.pnlFavorites.Controls.Add(this.lnkDrafts);
            this.pnlFavorites.Controls.Add(this.lnkSent);
            this.pnlFavorites.Controls.Add(this.lnkInbox);
            this.pnlFavorites.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFavorites.Location = new System.Drawing.Point(10, 47);
            this.pnlFavorites.Name = "pnlFavorites";
            this.pnlFavorites.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.pnlFavorites.Size = new System.Drawing.Size(210, 243);
            this.pnlFavorites.TabIndex = 18;
            // 
            // lnkArchive
            // 
            this.lnkArchive.Dock = System.Windows.Forms.DockStyle.Top;
            this.lnkArchive.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkArchive.Image = global::ImapX.Sample.Properties.Resources.archive;
            this.lnkArchive.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkArchive.LinkColor = System.Drawing.Color.Black;
            this.lnkArchive.Location = new System.Drawing.Point(10, 216);
            this.lnkArchive.Name = "lnkArchive";
            this.lnkArchive.Padding = new System.Windows.Forms.Padding(24, 3, 5, 7);
            this.lnkArchive.Size = new System.Drawing.Size(200, 27);
            this.lnkArchive.TabIndex = 22;
            this.lnkArchive.TabStop = true;
            this.lnkArchive.Text = "Archive";
            this.lnkArchive.Visible = false;
            this.lnkArchive.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFavorite_LinkClicked);
            // 
            // lnkAll
            // 
            this.lnkAll.Dock = System.Windows.Forms.DockStyle.Top;
            this.lnkAll.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkAll.Image = global::ImapX.Sample.Properties.Resources.mails;
            this.lnkAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkAll.LinkColor = System.Drawing.Color.Black;
            this.lnkAll.Location = new System.Drawing.Point(10, 189);
            this.lnkAll.Name = "lnkAll";
            this.lnkAll.Padding = new System.Windows.Forms.Padding(24, 4, 5, 6);
            this.lnkAll.Size = new System.Drawing.Size(200, 27);
            this.lnkAll.TabIndex = 21;
            this.lnkAll.TabStop = true;
            this.lnkAll.Text = "All mails";
            this.lnkAll.Visible = false;
            this.lnkAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFavorite_LinkClicked);
            // 
            // lnkTrash
            // 
            this.lnkTrash.Dock = System.Windows.Forms.DockStyle.Top;
            this.lnkTrash.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkTrash.Image = global::ImapX.Sample.Properties.Resources.empty_trash;
            this.lnkTrash.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkTrash.LinkColor = System.Drawing.Color.Black;
            this.lnkTrash.Location = new System.Drawing.Point(10, 162);
            this.lnkTrash.Name = "lnkTrash";
            this.lnkTrash.Padding = new System.Windows.Forms.Padding(24, 4, 5, 6);
            this.lnkTrash.Size = new System.Drawing.Size(200, 27);
            this.lnkTrash.TabIndex = 20;
            this.lnkTrash.TabStop = true;
            this.lnkTrash.Text = "Trash";
            this.lnkTrash.Visible = false;
            this.lnkTrash.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFavorite_LinkClicked);
            // 
            // lnkJunk
            // 
            this.lnkJunk.Dock = System.Windows.Forms.DockStyle.Top;
            this.lnkJunk.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkJunk.Image = global::ImapX.Sample.Properties.Resources.junk;
            this.lnkJunk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkJunk.LinkColor = System.Drawing.Color.Black;
            this.lnkJunk.Location = new System.Drawing.Point(10, 135);
            this.lnkJunk.Name = "lnkJunk";
            this.lnkJunk.Padding = new System.Windows.Forms.Padding(24, 4, 5, 6);
            this.lnkJunk.Size = new System.Drawing.Size(200, 27);
            this.lnkJunk.TabIndex = 19;
            this.lnkJunk.TabStop = true;
            this.lnkJunk.Text = "Junk";
            this.lnkJunk.Visible = false;
            this.lnkJunk.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFavorite_LinkClicked);
            // 
            // lnkFlagged
            // 
            this.lnkFlagged.Dock = System.Windows.Forms.DockStyle.Top;
            this.lnkFlagged.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkFlagged.Image = global::ImapX.Sample.Properties.Resources.flag;
            this.lnkFlagged.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkFlagged.LinkColor = System.Drawing.Color.Black;
            this.lnkFlagged.Location = new System.Drawing.Point(10, 108);
            this.lnkFlagged.Name = "lnkFlagged";
            this.lnkFlagged.Padding = new System.Windows.Forms.Padding(24, 4, 5, 6);
            this.lnkFlagged.Size = new System.Drawing.Size(200, 27);
            this.lnkFlagged.TabIndex = 18;
            this.lnkFlagged.TabStop = true;
            this.lnkFlagged.Text = "Flagged";
            this.lnkFlagged.Visible = false;
            this.lnkFlagged.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFavorite_LinkClicked);
            // 
            // lnkImportant
            // 
            this.lnkImportant.Dock = System.Windows.Forms.DockStyle.Top;
            this.lnkImportant.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkImportant.Image = global::ImapX.Sample.Properties.Resources.important;
            this.lnkImportant.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkImportant.LinkColor = System.Drawing.Color.Black;
            this.lnkImportant.Location = new System.Drawing.Point(10, 81);
            this.lnkImportant.Name = "lnkImportant";
            this.lnkImportant.Padding = new System.Windows.Forms.Padding(24, 4, 5, 6);
            this.lnkImportant.Size = new System.Drawing.Size(200, 27);
            this.lnkImportant.TabIndex = 17;
            this.lnkImportant.TabStop = true;
            this.lnkImportant.Text = "Important";
            this.lnkImportant.Visible = false;
            this.lnkImportant.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFavorite_LinkClicked);
            // 
            // lnkDrafts
            // 
            this.lnkDrafts.Dock = System.Windows.Forms.DockStyle.Top;
            this.lnkDrafts.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkDrafts.Image = global::ImapX.Sample.Properties.Resources.pencil;
            this.lnkDrafts.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkDrafts.LinkColor = System.Drawing.Color.Black;
            this.lnkDrafts.Location = new System.Drawing.Point(10, 54);
            this.lnkDrafts.Name = "lnkDrafts";
            this.lnkDrafts.Padding = new System.Windows.Forms.Padding(24, 4, 5, 6);
            this.lnkDrafts.Size = new System.Drawing.Size(200, 27);
            this.lnkDrafts.TabIndex = 16;
            this.lnkDrafts.TabStop = true;
            this.lnkDrafts.Text = "Drafts";
            this.lnkDrafts.Visible = false;
            this.lnkDrafts.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFavorite_LinkClicked);
            // 
            // lnkSent
            // 
            this.lnkSent.Dock = System.Windows.Forms.DockStyle.Top;
            this.lnkSent.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkSent.Image = global::ImapX.Sample.Properties.Resources.paper_plane;
            this.lnkSent.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkSent.LinkColor = System.Drawing.Color.Black;
            this.lnkSent.Location = new System.Drawing.Point(10, 27);
            this.lnkSent.Name = "lnkSent";
            this.lnkSent.Padding = new System.Windows.Forms.Padding(24, 4, 5, 6);
            this.lnkSent.Size = new System.Drawing.Size(200, 27);
            this.lnkSent.TabIndex = 15;
            this.lnkSent.TabStop = true;
            this.lnkSent.Text = "Sent";
            this.lnkSent.Visible = false;
            this.lnkSent.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFavorite_LinkClicked);
            // 
            // lnkInbox
            // 
            this.lnkInbox.Dock = System.Windows.Forms.DockStyle.Top;
            this.lnkInbox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkInbox.Image = global::ImapX.Sample.Properties.Resources.inbox;
            this.lnkInbox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkInbox.LinkColor = System.Drawing.Color.Black;
            this.lnkInbox.Location = new System.Drawing.Point(10, 0);
            this.lnkInbox.Name = "lnkInbox";
            this.lnkInbox.Padding = new System.Windows.Forms.Padding(24, 4, 5, 6);
            this.lnkInbox.Size = new System.Drawing.Size(200, 27);
            this.lnkInbox.TabIndex = 14;
            this.lnkInbox.TabStop = true;
            this.lnkInbox.Text = "Inbox";
            this.lnkInbox.Visible = false;
            this.lnkInbox.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFavorite_LinkClicked);
            // 
            // lblFavorites
            // 
            this.lblFavorites.AutoSize = true;
            this.lblFavorites.BackColor = System.Drawing.Color.Transparent;
            this.lblFavorites.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFavorites.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFavorites.Location = new System.Drawing.Point(10, 10);
            this.lblFavorites.Name = "lblFavorites";
            this.lblFavorites.Padding = new System.Windows.Forms.Padding(8);
            this.lblFavorites.Size = new System.Drawing.Size(89, 37);
            this.lblFavorites.TabIndex = 4;
            this.lblFavorites.Text = "Favorites";
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.lblIdle);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel9.Location = new System.Drawing.Point(10, 520);
            this.panel9.Name = "panel9";
            this.panel9.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.panel9.Size = new System.Drawing.Size(210, 37);
            this.panel9.TabIndex = 19;
            // 
            // lblIdle
            // 
            this.lblIdle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIdle.Location = new System.Drawing.Point(0, 0);
            this.lblIdle.Name = "lblIdle";
            this.lblIdle.Padding = new System.Windows.Forms.Padding(5);
            this.lblIdle.Size = new System.Drawing.Size(210, 27);
            this.lblIdle.TabIndex = 0;
            this.lblIdle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlWrap
            // 
            this.pnlWrap.Controls.Add(this.splitContainer1);
            this.pnlWrap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWrap.Location = new System.Drawing.Point(220, 0);
            this.pnlWrap.Name = "pnlWrap";
            this.pnlWrap.Padding = new System.Windows.Forms.Padding(10);
            this.pnlWrap.Size = new System.Drawing.Size(764, 557);
            this.pnlWrap.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(10, 10);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel1.Controls.Add(this.pnlMessages);
            this.splitContainer1.Panel1.Controls.Add(this.pnlSelectFolder);
            this.splitContainer1.Panel1.Controls.Add(this.pnlLoading);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel2.Controls.Add(this.pnlView);
            this.splitContainer1.Panel2.Controls.Add(this.pnlDownloadingBody);
            this.splitContainer1.Panel2.Controls.Add(this.pnlInfo);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(10);
            this.splitContainer1.Size = new System.Drawing.Size(744, 537);
            this.splitContainer1.SplitterDistance = 307;
            this.splitContainer1.TabIndex = 0;
            // 
            // pnlMessages
            // 
            this.pnlMessages.Controls.Add(this.lsvMessages);
            this.pnlMessages.Controls.Add(this.panel5);
            this.pnlMessages.Controls.Add(this.lblFolder);
            this.pnlMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMessages.Location = new System.Drawing.Point(0, 101);
            this.pnlMessages.Name = "pnlMessages";
            this.pnlMessages.Padding = new System.Windows.Forms.Padding(10);
            this.pnlMessages.Size = new System.Drawing.Size(307, 436);
            this.pnlMessages.TabIndex = 4;
            this.pnlMessages.Visible = false;
            // 
            // lsvMessages
            // 
            this.lsvMessages.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lsvMessages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmMessages});
            this.lsvMessages.ContextMenuStrip = this.mnuMessage;
            this.lsvMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvMessages.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lsvMessages.FullRowSelect = true;
            this.lsvMessages.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lsvMessages.HideSelection = false;
            this.lsvMessages.Location = new System.Drawing.Point(10, 51);
            this.lsvMessages.MultiSelect = false;
            this.lsvMessages.Name = "lsvMessages";
            this.lsvMessages.Size = new System.Drawing.Size(287, 375);
            this.lsvMessages.SmallImageList = this.istPlaceHolder;
            this.lsvMessages.TabIndex = 3;
            this.lsvMessages.UseCompatibleStateImageBehavior = false;
            this.lsvMessages.View = System.Windows.Forms.View.Details;
            this.lsvMessages.VirtualMode = true;
            this.lsvMessages.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.lsvMessages_RetrieveVirtualItem);
            this.lsvMessages.SelectedIndexChanged += new System.EventHandler(this.lsvMessages_SelectedIndexChanged);
            this.lsvMessages.SizeChanged += new System.EventHandler(this.FrmMainOrLsvMails_SizeChanged);
            // 
            // clmMessages
            // 
            this.clmMessages.Text = "Message";
            // 
            // mnuMessage
            // 
            this.mnuMessage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.seenToolStripMenuItem,
            this.toolStripMenuItem3,
            this.copyToToolStripMenuItem,
            this.moveToToolStripMenuItem,
            this.toolStripMenuItem4,
            this.deleteToolStripMenuItem1,
            this.toolStripMenuItem6,
            this.exportMessageToolStripMenuItem});
            this.mnuMessage.Name = "mnuMessage";
            this.mnuMessage.Size = new System.Drawing.Size(166, 132);
            this.mnuMessage.Opening += new System.ComponentModel.CancelEventHandler(this.mnuMessage_Opening);
            // 
            // seenToolStripMenuItem
            // 
            this.seenToolStripMenuItem.Name = "seenToolStripMenuItem";
            this.seenToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.seenToolStripMenuItem.Text = "&Seen";
            this.seenToolStripMenuItem.Click += new System.EventHandler(this.seenToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(162, 6);
            // 
            // copyToToolStripMenuItem
            // 
            this.copyToToolStripMenuItem.Name = "copyToToolStripMenuItem";
            this.copyToToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.copyToToolStripMenuItem.Text = "&Copy to...";
            this.copyToToolStripMenuItem.Click += new System.EventHandler(this.copyToToolStripMenuItem_Click);
            // 
            // moveToToolStripMenuItem
            // 
            this.moveToToolStripMenuItem.Name = "moveToToolStripMenuItem";
            this.moveToToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.moveToToolStripMenuItem.Text = "&Move to...";
            this.moveToToolStripMenuItem.Click += new System.EventHandler(this.moveToToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(162, 6);
            // 
            // deleteToolStripMenuItem1
            // 
            this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(165, 22);
            this.deleteToolStripMenuItem1.Text = "&Delete";
            this.deleteToolStripMenuItem1.Click += new System.EventHandler(this.deleteToolStripMenuItem1_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(162, 6);
            // 
            // exportMessageToolStripMenuItem
            // 
            this.exportMessageToolStripMenuItem.Name = "exportMessageToolStripMenuItem";
            this.exportMessageToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.exportMessageToolStripMenuItem.Text = "&Export message...";
            this.exportMessageToolStripMenuItem.Click += new System.EventHandler(this.exportMessageToolStripMenuItem_Click);
            // 
            // istPlaceHolder
            // 
            this.istPlaceHolder.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("istPlaceHolder.ImageStream")));
            this.istPlaceHolder.TransparentColor = System.Drawing.Color.Transparent;
            this.istPlaceHolder.Images.SetKeyName(0, "attach.png");
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Transparent;
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(10, 41);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(287, 10);
            this.panel5.TabIndex = 18;
            // 
            // lblFolder
            // 
            this.lblFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.lblFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFolder.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFolder.Location = new System.Drawing.Point(10, 10);
            this.lblFolder.Name = "lblFolder";
            this.lblFolder.Padding = new System.Windows.Forms.Padding(5);
            this.lblFolder.Size = new System.Drawing.Size(287, 31);
            this.lblFolder.TabIndex = 5;
            this.lblFolder.Text = "Folder";
            // 
            // pnlSelectFolder
            // 
            this.pnlSelectFolder.Controls.Add(this.label2);
            this.pnlSelectFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSelectFolder.Location = new System.Drawing.Point(0, 64);
            this.pnlSelectFolder.Name = "pnlSelectFolder";
            this.pnlSelectFolder.Padding = new System.Windows.Forms.Padding(10);
            this.pnlSelectFolder.Size = new System.Drawing.Size(307, 37);
            this.pnlSelectFolder.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(10, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(287, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "please select a folder";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pnlLoading
            // 
            this.pnlLoading.Controls.Add(this.pgbMessages);
            this.pnlLoading.Controls.Add(this.label1);
            this.pnlLoading.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLoading.Location = new System.Drawing.Point(0, 0);
            this.pnlLoading.Name = "pnlLoading";
            this.pnlLoading.Padding = new System.Windows.Forms.Padding(10);
            this.pnlLoading.Size = new System.Drawing.Size(307, 64);
            this.pnlLoading.TabIndex = 1;
            this.pnlLoading.Visible = false;
            // 
            // pgbMessages
            // 
            this.pgbMessages.Dock = System.Windows.Forms.DockStyle.Top;
            this.pgbMessages.Location = new System.Drawing.Point(10, 35);
            this.pgbMessages.Name = "pgbMessages";
            this.pgbMessages.Size = new System.Drawing.Size(287, 17);
            this.pgbMessages.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pgbMessages.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(287, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "loading messages...";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pnlView
            // 
            this.pnlView.Controls.Add(this.wbrMain);
            this.pnlView.Controls.Add(this.pnlAttachments);
            this.pnlView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlView.Location = new System.Drawing.Point(10, 198);
            this.pnlView.Name = "pnlView";
            this.pnlView.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.pnlView.Size = new System.Drawing.Size(413, 329);
            this.pnlView.TabIndex = 1;
            this.pnlView.Visible = false;
            // 
            // wbrMain
            // 
            this.wbrMain.AllowWebBrowserDrop = false;
            this.wbrMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbrMain.IsWebBrowserContextMenuEnabled = false;
            this.wbrMain.Location = new System.Drawing.Point(0, 10);
            this.wbrMain.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbrMain.Name = "wbrMain";
            this.wbrMain.ScriptErrorsSuppressed = true;
            this.wbrMain.Size = new System.Drawing.Size(413, 211);
            this.wbrMain.TabIndex = 0;
            // 
            // pnlAttachments
            // 
            this.pnlAttachments.BackColor = System.Drawing.Color.White;
            this.pnlAttachments.Controls.Add(this.panel4);
            this.pnlAttachments.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAttachments.Location = new System.Drawing.Point(0, 221);
            this.pnlAttachments.Name = "pnlAttachments";
            this.pnlAttachments.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.pnlAttachments.Size = new System.Drawing.Size(413, 108);
            this.pnlAttachments.TabIndex = 1;
            this.pnlAttachments.Visible = false;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.Controls.Add(this.lsvAttachments);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.panel8);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 5);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(413, 103);
            this.panel4.TabIndex = 2;
            // 
            // lsvAttachments
            // 
            this.lsvAttachments.BackColor = System.Drawing.Color.White;
            this.lsvAttachments.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lsvAttachments.ContextMenuStrip = this.mnuAttachment;
            this.lsvAttachments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvAttachments.Location = new System.Drawing.Point(0, 28);
            this.lsvAttachments.MultiSelect = false;
            this.lsvAttachments.Name = "lsvAttachments";
            this.lsvAttachments.Size = new System.Drawing.Size(413, 75);
            this.lsvAttachments.SmallImageList = this.istAttachments;
            this.lsvAttachments.TabIndex = 0;
            this.lsvAttachments.UseCompatibleStateImageBehavior = false;
            this.lsvAttachments.View = System.Windows.Forms.View.List;
            // 
            // mnuAttachment
            // 
            this.mnuAttachment.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.downloadToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.mnuAttachment.Name = "mnuAttachment";
            this.mnuAttachment.Size = new System.Drawing.Size(129, 70);
            this.mnuAttachment.Opening += new System.ComponentModel.CancelEventHandler(this.mnuAttachment_Opening);
            // 
            // downloadToolStripMenuItem
            // 
            this.downloadToolStripMenuItem.Name = "downloadToolStripMenuItem";
            this.downloadToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.downloadToolStripMenuItem.Text = "&Download";
            this.downloadToolStripMenuItem.Click += new System.EventHandler(this.downloadToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.saveAsToolStripMenuItem.Text = "&Save as...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // istAttachments
            // 
            this.istAttachments.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.istAttachments.ImageSize = new System.Drawing.Size(16, 16);
            this.istAttachments.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(0, 1);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.label3.Size = new System.Drawing.Size(413, 27);
            this.label3.TabIndex = 1;
            this.label3.Text = "Attachments";
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(413, 1);
            this.panel8.TabIndex = 17;
            // 
            // pnlDownloadingBody
            // 
            this.pnlDownloadingBody.AutoSize = true;
            this.pnlDownloadingBody.Controls.Add(this.lblFailedDownloadBody);
            this.pnlDownloadingBody.Controls.Add(this.lblDownloadingBody);
            this.pnlDownloadingBody.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDownloadingBody.Location = new System.Drawing.Point(10, 144);
            this.pnlDownloadingBody.Name = "pnlDownloadingBody";
            this.pnlDownloadingBody.Padding = new System.Windows.Forms.Padding(10);
            this.pnlDownloadingBody.Size = new System.Drawing.Size(413, 54);
            this.pnlDownloadingBody.TabIndex = 20;
            this.pnlDownloadingBody.Visible = false;
            // 
            // lblFailedDownloadBody
            // 
            this.lblFailedDownloadBody.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFailedDownloadBody.ForeColor = System.Drawing.Color.Red;
            this.lblFailedDownloadBody.Location = new System.Drawing.Point(10, 27);
            this.lblFailedDownloadBody.Name = "lblFailedDownloadBody";
            this.lblFailedDownloadBody.Size = new System.Drawing.Size(393, 17);
            this.lblFailedDownloadBody.TabIndex = 1;
            this.lblFailedDownloadBody.Text = "Failed to download body";
            this.lblFailedDownloadBody.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblFailedDownloadBody.Visible = false;
            // 
            // lblDownloadingBody
            // 
            this.lblDownloadingBody.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDownloadingBody.Location = new System.Drawing.Point(10, 10);
            this.lblDownloadingBody.Name = "lblDownloadingBody";
            this.lblDownloadingBody.Size = new System.Drawing.Size(393, 17);
            this.lblDownloadingBody.TabIndex = 0;
            this.lblDownloadingBody.Text = "Downloading body...";
            this.lblDownloadingBody.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pnlInfo
            // 
            this.pnlInfo.AutoSize = true;
            this.pnlInfo.BackColor = System.Drawing.Color.White;
            this.pnlInfo.Controls.Add(this.pnlEmbeddedResources);
            this.pnlInfo.Controls.Add(this.panel7);
            this.pnlInfo.Controls.Add(this.panel6);
            this.pnlInfo.Controls.Add(this.lblLabels);
            this.pnlInfo.Controls.Add(this.lblFrom);
            this.pnlInfo.Controls.Add(this.lblSubject);
            this.pnlInfo.Controls.Add(this.lblDate);
            this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInfo.Location = new System.Drawing.Point(10, 10);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(413, 134);
            this.pnlInfo.TabIndex = 0;
            this.pnlInfo.Visible = false;
            // 
            // pnlEmbeddedResources
            // 
            this.pnlEmbeddedResources.AutoSize = true;
            this.pnlEmbeddedResources.BackColor = System.Drawing.Color.DarkGreen;
            this.pnlEmbeddedResources.Controls.Add(this.lnkDownloadEmbeddedResources);
            this.pnlEmbeddedResources.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEmbeddedResources.Location = new System.Drawing.Point(0, 107);
            this.pnlEmbeddedResources.Name = "pnlEmbeddedResources";
            this.pnlEmbeddedResources.Padding = new System.Windows.Forms.Padding(5);
            this.pnlEmbeddedResources.Size = new System.Drawing.Size(413, 27);
            this.pnlEmbeddedResources.TabIndex = 22;
            this.pnlEmbeddedResources.Visible = false;
            // 
            // lnkDownloadEmbeddedResources
            // 
            this.lnkDownloadEmbeddedResources.AutoSize = true;
            this.lnkDownloadEmbeddedResources.BackColor = System.Drawing.Color.Transparent;
            this.lnkDownloadEmbeddedResources.DisabledLinkColor = System.Drawing.Color.Black;
            this.lnkDownloadEmbeddedResources.Dock = System.Windows.Forms.DockStyle.Top;
            this.lnkDownloadEmbeddedResources.LinkColor = System.Drawing.Color.White;
            this.lnkDownloadEmbeddedResources.Location = new System.Drawing.Point(5, 5);
            this.lnkDownloadEmbeddedResources.Name = "lnkDownloadEmbeddedResources";
            this.lnkDownloadEmbeddedResources.Size = new System.Drawing.Size(404, 17);
            this.lnkDownloadEmbeddedResources.TabIndex = 1;
            this.lnkDownloadEmbeddedResources.TabStop = true;
            this.lnkDownloadEmbeddedResources.Text = "Message contains embedded images. Click here to download them.";
            this.lnkDownloadEmbeddedResources.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDownloadEmbeddedResources_LinkClicked);
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 106);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(413, 1);
            this.panel7.TabIndex = 21;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.Transparent;
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 96);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(413, 10);
            this.panel6.TabIndex = 20;
            // 
            // lblLabels
            // 
            this.lblLabels.AutoSize = true;
            this.lblLabels.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLabels.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLabels.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.lblLabels.Location = new System.Drawing.Point(0, 69);
            this.lblLabels.Name = "lblLabels";
            this.lblLabels.Padding = new System.Windows.Forms.Padding(1, 5, 0, 5);
            this.lblLabels.Size = new System.Drawing.Size(79, 27);
            this.lblLabels.TabIndex = 23;
            this.lblLabels.Text = "GmailLabels";
            this.lblLabels.Visible = false;
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFrom.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.lblFrom.Location = new System.Drawing.Point(0, 42);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Padding = new System.Windows.Forms.Padding(1, 5, 0, 5);
            this.lblFrom.Size = new System.Drawing.Size(154, 27);
            this.lblFrom.TabIndex = 1;
            this.lblFrom.Text = "From: mail@domain.com";
            // 
            // lblSubject
            // 
            this.lblSubject.AutoSize = true;
            this.lblSubject.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSubject.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubject.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.lblSubject.Location = new System.Drawing.Point(0, 22);
            this.lblSubject.Name = "lblSubject";
            this.lblSubject.Size = new System.Drawing.Size(58, 20);
            this.lblSubject.TabIndex = 2;
            this.lblSubject.Text = "Subject";
            // 
            // lblDate
            // 
            this.lblDate.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.lblDate.Location = new System.Drawing.Point(0, 0);
            this.lblDate.Name = "lblDate";
            this.lblDate.Padding = new System.Windows.Forms.Padding(1, 0, 0, 5);
            this.lblDate.Size = new System.Drawing.Size(413, 22);
            this.lblDate.TabIndex = 0;
            this.lblDate.Text = "Date";
            // 
            // mnuFolder
            // 
            this.mnuFolder.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripMenuItem1,
            this.addSubfolderToolStripMenuItem,
            this.toolStripMenuItem2,
            this.importMessageToolStripMenuItem,
            this.toolStripMenuItem5,
            this.emptyToolStripMenuItem});
            this.mnuFolder.Name = "mnuFolder";
            this.mnuFolder.Size = new System.Drawing.Size(160, 132);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.renameToolStripMenuItem.Text = "&Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.deleteToolStripMenuItem.Text = "&Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(156, 6);
            // 
            // addSubfolderToolStripMenuItem
            // 
            this.addSubfolderToolStripMenuItem.Name = "addSubfolderToolStripMenuItem";
            this.addSubfolderToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.addSubfolderToolStripMenuItem.Text = "&Add subfolder";
            this.addSubfolderToolStripMenuItem.Click += new System.EventHandler(this.addSubfolderToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(156, 6);
            // 
            // importMessageToolStripMenuItem
            // 
            this.importMessageToolStripMenuItem.Name = "importMessageToolStripMenuItem";
            this.importMessageToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.importMessageToolStripMenuItem.Text = "&Import message";
            this.importMessageToolStripMenuItem.Click += new System.EventHandler(this.importMessageToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(156, 6);
            // 
            // emptyToolStripMenuItem
            // 
            this.emptyToolStripMenuItem.Name = "emptyToolStripMenuItem";
            this.emptyToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.emptyToolStripMenuItem.Text = "&Empty folder";
            this.emptyToolStripMenuItem.Click += new System.EventHandler(this.emptyToolStripMenuItem_Click);
            // 
            // ofdImportMessage
            // 
            this.ofdImportMessage.Filter = "Message files|*.eml|All files|*.*";
            this.ofdImportMessage.Title = "Import message";
            // 
            // sfdExportMessage
            // 
            this.sfdExportMessage.Filter = "Eml file|*.eml|All files|*.*";
            this.sfdExportMessage.Title = "Export message";
            // 
            // sfdSaveAttachment
            // 
            this.sfdSaveAttachment.Filter = "All files|*.*";
            this.sfdSaveAttachment.Title = "Save attachment";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(984, 557);
            this.Controls.Add(this.pnlWrap);
            this.Controls.Add(this.pnlFolders);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ImapX";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.SizeChanged += new System.EventHandler(this.FrmMainOrLsvMails_SizeChanged);
            this.pnlFolders.ResumeLayout(false);
            this.pnlFolders.PerformLayout();
            this.pnlFavorites.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.pnlWrap.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.pnlMessages.ResumeLayout(false);
            this.mnuMessage.ResumeLayout(false);
            this.pnlSelectFolder.ResumeLayout(false);
            this.pnlLoading.ResumeLayout(false);
            this.pnlView.ResumeLayout(false);
            this.pnlAttachments.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.mnuAttachment.ResumeLayout(false);
            this.pnlDownloadingBody.ResumeLayout(false);
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            this.pnlEmbeddedResources.ResumeLayout(false);
            this.pnlEmbeddedResources.PerformLayout();
            this.mnuFolder.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlFolders;
        private System.Windows.Forms.Label lblFavorites;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TreeView trwFolders;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlWrap;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel pnlLoading;
        private System.Windows.Forms.ProgressBar pgbMessages;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlSelectFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlFavorites;
        private System.Windows.Forms.LinkLabel lnkArchive;
        private System.Windows.Forms.LinkLabel lnkAll;
        private System.Windows.Forms.LinkLabel lnkTrash;
        private System.Windows.Forms.LinkLabel lnkJunk;
        private System.Windows.Forms.LinkLabel lnkFlagged;
        private System.Windows.Forms.LinkLabel lnkImportant;
        private System.Windows.Forms.LinkLabel lnkDrafts;
        private System.Windows.Forms.LinkLabel lnkSent;
        private System.Windows.Forms.LinkLabel lnkInbox;
        private System.Windows.Forms.ListView lsvMessages;
        private System.Windows.Forms.Panel pnlMessages;
        private System.Windows.Forms.ColumnHeader clmMessages;
        private System.Windows.Forms.ImageList istPlaceHolder;
        private System.Windows.Forms.Label lblFolder;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.Label lblSubject;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Panel pnlView;
        private System.Windows.Forms.Panel pnlDownloadingBody;
        private System.Windows.Forms.Label lblFailedDownloadBody;
        private System.Windows.Forms.Label lblDownloadingBody;
        private System.Windows.Forms.WebBrowser wbrMain;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel pnlEmbeddedResources;
        private System.Windows.Forms.LinkLabel lnkDownloadEmbeddedResources;
        private System.Windows.Forms.ContextMenuStrip mnuFolder;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem addSubfolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem importMessageToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip mnuMessage;
        private System.Windows.Forms.ToolStripMenuItem seenToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem copyToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem emptyToolStripMenuItem;
        private System.Windows.Forms.Panel pnlAttachments;
        private System.Windows.Forms.ListView lsvAttachments;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.ImageList istAttachments;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem exportMessageToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog ofdImportMessage;
        private System.Windows.Forms.SaveFileDialog sfdExportMessage;
        private System.Windows.Forms.ContextMenuStrip mnuAttachment;
        private System.Windows.Forms.ToolStripMenuItem downloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog sfdSaveAttachment;
        private System.Windows.Forms.Label lblLabels;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label lblIdle;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Panel panel10;
    }
}