Imports System.ComponentModel

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMain))
        Me.pnlFolders = New System.Windows.Forms.Panel()
        Me.trwFolders = New System.Windows.Forms.TreeView()
        Me.panel1 = New System.Windows.Forms.Panel()
        Me.panel3 = New System.Windows.Forms.Panel()
        Me.panel2 = New System.Windows.Forms.Panel()
        Me.pnlFavorites = New System.Windows.Forms.Panel()
        Me.lnkArchive = New System.Windows.Forms.LinkLabel()
        Me.lnkAll = New System.Windows.Forms.LinkLabel()
        Me.lnkTrash = New System.Windows.Forms.LinkLabel()
        Me.lnkJunk = New System.Windows.Forms.LinkLabel()
        Me.lnkFlagged = New System.Windows.Forms.LinkLabel()
        Me.lnkImportant = New System.Windows.Forms.LinkLabel()
        Me.lnkDrafts = New System.Windows.Forms.LinkLabel()
        Me.lnkSent = New System.Windows.Forms.LinkLabel()
        Me.lnkInbox = New System.Windows.Forms.LinkLabel()
        Me.lblFavorites = New System.Windows.Forms.Label()
        Me.pnlWrap = New System.Windows.Forms.Panel()
        Me.splitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.pnlMessages = New System.Windows.Forms.Panel()
        Me.lsvMessages = New System.Windows.Forms.ListView()
        Me.clmMessages = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.mnuMessage = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.seenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.toolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator()
        Me.copyToToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.moveToToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.toolStripMenuItem4 = New System.Windows.Forms.ToolStripSeparator()
        Me.deleteToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.toolStripMenuItem6 = New System.Windows.Forms.ToolStripSeparator()
        Me.exportMessageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.istPlaceHolder = New System.Windows.Forms.ImageList(Me.components)
        Me.panel5 = New System.Windows.Forms.Panel()
        Me.lblFolder = New System.Windows.Forms.Label()
        Me.pnlSelectFolder = New System.Windows.Forms.Panel()
        Me.label2 = New System.Windows.Forms.Label()
        Me.pnlLoading = New System.Windows.Forms.Panel()
        Me.pgbMessages = New System.Windows.Forms.ProgressBar()
        Me.label1 = New System.Windows.Forms.Label()
        Me.pnlView = New System.Windows.Forms.Panel()
        Me.wbrMain = New System.Windows.Forms.WebBrowser()
        Me.pnlAttachments = New System.Windows.Forms.Panel()
        Me.panel4 = New System.Windows.Forms.Panel()
        Me.lsvAttachments = New System.Windows.Forms.ListView()
        Me.mnuAttachment = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.downloadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.openToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.saveAsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.istAttachments = New System.Windows.Forms.ImageList(Me.components)
        Me.label3 = New System.Windows.Forms.Label()
        Me.panel8 = New System.Windows.Forms.Panel()
        Me.pnlDownloadingBody = New System.Windows.Forms.Panel()
        Me.lblFailedDownloadBody = New System.Windows.Forms.Label()
        Me.lblDownloadingBody = New System.Windows.Forms.Label()
        Me.pnlInfo = New System.Windows.Forms.Panel()
        Me.pnlEmbeddedResources = New System.Windows.Forms.Panel()
        Me.lnkDownloadEmbeddedResources = New System.Windows.Forms.LinkLabel()
        Me.panel7 = New System.Windows.Forms.Panel()
        Me.panel6 = New System.Windows.Forms.Panel()
        Me.lblLabels = New System.Windows.Forms.Label()
        Me.lblFrom = New System.Windows.Forms.Label()
        Me.lblSubject = New System.Windows.Forms.Label()
        Me.lblDate = New System.Windows.Forms.Label()
        Me.mnuFolder = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.renameToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.deleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.toolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.addSubfolderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.toolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.importMessageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.toolStripMenuItem5 = New System.Windows.Forms.ToolStripSeparator()
        Me.emptyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ofdImportMessage = New System.Windows.Forms.OpenFileDialog()
        Me.sfdExportMessage = New System.Windows.Forms.SaveFileDialog()
        Me.sfdSaveAttachment = New System.Windows.Forms.SaveFileDialog()
        Me.pnlFolders.SuspendLayout()
        Me.pnlFavorites.SuspendLayout()
        Me.pnlWrap.SuspendLayout()
        CType(Me.splitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContainer1.Panel1.SuspendLayout()
        Me.splitContainer1.Panel2.SuspendLayout()
        Me.splitContainer1.SuspendLayout()
        Me.pnlMessages.SuspendLayout()
        Me.mnuMessage.SuspendLayout()
        Me.pnlSelectFolder.SuspendLayout()
        Me.pnlLoading.SuspendLayout()
        Me.pnlView.SuspendLayout()
        Me.pnlAttachments.SuspendLayout()
        Me.panel4.SuspendLayout()
        Me.mnuAttachment.SuspendLayout()
        Me.pnlDownloadingBody.SuspendLayout()
        Me.pnlInfo.SuspendLayout()
        Me.pnlEmbeddedResources.SuspendLayout()
        Me.mnuFolder.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlFolders
        '
        Me.pnlFolders.BackColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.pnlFolders.Controls.Add(Me.trwFolders)
        Me.pnlFolders.Controls.Add(Me.panel1)
        Me.pnlFolders.Controls.Add(Me.panel3)
        Me.pnlFolders.Controls.Add(Me.panel2)
        Me.pnlFolders.Controls.Add(Me.pnlFavorites)
        Me.pnlFolders.Controls.Add(Me.lblFavorites)
        Me.pnlFolders.Dock = System.Windows.Forms.DockStyle.Left
        Me.pnlFolders.Location = New System.Drawing.Point(0, 0)
        Me.pnlFolders.Name = "pnlFolders"
        Me.pnlFolders.Padding = New System.Windows.Forms.Padding(10, 10, 0, 0)
        Me.pnlFolders.Size = New System.Drawing.Size(220, 557)
        Me.pnlFolders.TabIndex = 0
        '
        'trwFolders
        '
        Me.trwFolders.BackColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.trwFolders.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.trwFolders.Dock = System.Windows.Forms.DockStyle.Fill
        Me.trwFolders.FullRowSelect = True
        Me.trwFolders.HideSelection = False
        Me.trwFolders.ItemHeight = 27
        Me.trwFolders.Location = New System.Drawing.Point(10, 327)
        Me.trwFolders.Name = "trwFolders"
        Me.trwFolders.ShowRootLines = False
        Me.trwFolders.Size = New System.Drawing.Size(210, 230)
        Me.trwFolders.TabIndex = 17
        '
        'panel1
        '
        Me.panel1.BackColor = System.Drawing.Color.Transparent
        Me.panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.panel1.Location = New System.Drawing.Point(10, 309)
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New System.Drawing.Size(210, 18)
        Me.panel1.TabIndex = 17
        '
        'panel3
        '
        Me.panel3.BackColor = System.Drawing.Color.FromArgb(CType(CType(171, Byte), Integer), CType(CType(171, Byte), Integer), CType(CType(171, Byte), Integer))
        Me.panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.panel3.Location = New System.Drawing.Point(10, 308)
        Me.panel3.Name = "panel3"
        Me.panel3.Size = New System.Drawing.Size(210, 1)
        Me.panel3.TabIndex = 16
        '
        'panel2
        '
        Me.panel2.BackColor = System.Drawing.Color.Transparent
        Me.panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.panel2.Location = New System.Drawing.Point(10, 290)
        Me.panel2.Name = "panel2"
        Me.panel2.Size = New System.Drawing.Size(210, 18)
        Me.panel2.TabIndex = 15
        '
        'pnlFavorites
        '
        Me.pnlFavorites.AutoSize = True
        Me.pnlFavorites.BackColor = System.Drawing.Color.Transparent
        Me.pnlFavorites.Controls.Add(Me.lnkArchive)
        Me.pnlFavorites.Controls.Add(Me.lnkAll)
        Me.pnlFavorites.Controls.Add(Me.lnkTrash)
        Me.pnlFavorites.Controls.Add(Me.lnkJunk)
        Me.pnlFavorites.Controls.Add(Me.lnkFlagged)
        Me.pnlFavorites.Controls.Add(Me.lnkImportant)
        Me.pnlFavorites.Controls.Add(Me.lnkDrafts)
        Me.pnlFavorites.Controls.Add(Me.lnkSent)
        Me.pnlFavorites.Controls.Add(Me.lnkInbox)
        Me.pnlFavorites.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlFavorites.Location = New System.Drawing.Point(10, 47)
        Me.pnlFavorites.Name = "pnlFavorites"
        Me.pnlFavorites.Padding = New System.Windows.Forms.Padding(10, 0, 0, 0)
        Me.pnlFavorites.Size = New System.Drawing.Size(210, 243)
        Me.pnlFavorites.TabIndex = 18
        '
        'lnkArchive
        '
        Me.lnkArchive.Dock = System.Windows.Forms.DockStyle.Top
        Me.lnkArchive.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnkArchive.Image = Global.ImapX.VBSample.My.Resources.Resources.archive
        Me.lnkArchive.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lnkArchive.LinkColor = System.Drawing.Color.Black
        Me.lnkArchive.Location = New System.Drawing.Point(10, 216)
        Me.lnkArchive.Name = "lnkArchive"
        Me.lnkArchive.Padding = New System.Windows.Forms.Padding(24, 3, 5, 7)
        Me.lnkArchive.Size = New System.Drawing.Size(200, 27)
        Me.lnkArchive.TabIndex = 22
        Me.lnkArchive.TabStop = True
        Me.lnkArchive.Text = "Archive"
        Me.lnkArchive.Visible = False
        '
        'lnkAll
        '
        Me.lnkAll.Dock = System.Windows.Forms.DockStyle.Top
        Me.lnkAll.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnkAll.Image = Global.ImapX.VBSample.My.Resources.Resources.mails
        Me.lnkAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lnkAll.LinkColor = System.Drawing.Color.Black
        Me.lnkAll.Location = New System.Drawing.Point(10, 189)
        Me.lnkAll.Name = "lnkAll"
        Me.lnkAll.Padding = New System.Windows.Forms.Padding(24, 4, 5, 6)
        Me.lnkAll.Size = New System.Drawing.Size(200, 27)
        Me.lnkAll.TabIndex = 21
        Me.lnkAll.TabStop = True
        Me.lnkAll.Text = "All mails"
        Me.lnkAll.Visible = False
        '
        'lnkTrash
        '
        Me.lnkTrash.Dock = System.Windows.Forms.DockStyle.Top
        Me.lnkTrash.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnkTrash.Image = Global.ImapX.VBSample.My.Resources.Resources.empty_trash
        Me.lnkTrash.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lnkTrash.LinkColor = System.Drawing.Color.Black
        Me.lnkTrash.Location = New System.Drawing.Point(10, 162)
        Me.lnkTrash.Name = "lnkTrash"
        Me.lnkTrash.Padding = New System.Windows.Forms.Padding(24, 4, 5, 6)
        Me.lnkTrash.Size = New System.Drawing.Size(200, 27)
        Me.lnkTrash.TabIndex = 20
        Me.lnkTrash.TabStop = True
        Me.lnkTrash.Text = "Trash"
        Me.lnkTrash.Visible = False
        '
        'lnkJunk
        '
        Me.lnkJunk.Dock = System.Windows.Forms.DockStyle.Top
        Me.lnkJunk.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnkJunk.Image = Global.ImapX.VBSample.My.Resources.Resources.junk
        Me.lnkJunk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lnkJunk.LinkColor = System.Drawing.Color.Black
        Me.lnkJunk.Location = New System.Drawing.Point(10, 135)
        Me.lnkJunk.Name = "lnkJunk"
        Me.lnkJunk.Padding = New System.Windows.Forms.Padding(24, 4, 5, 6)
        Me.lnkJunk.Size = New System.Drawing.Size(200, 27)
        Me.lnkJunk.TabIndex = 19
        Me.lnkJunk.TabStop = True
        Me.lnkJunk.Text = "Junk"
        Me.lnkJunk.Visible = False
        '
        'lnkFlagged
        '
        Me.lnkFlagged.Dock = System.Windows.Forms.DockStyle.Top
        Me.lnkFlagged.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnkFlagged.Image = Global.ImapX.VBSample.My.Resources.Resources.flag
        Me.lnkFlagged.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lnkFlagged.LinkColor = System.Drawing.Color.Black
        Me.lnkFlagged.Location = New System.Drawing.Point(10, 108)
        Me.lnkFlagged.Name = "lnkFlagged"
        Me.lnkFlagged.Padding = New System.Windows.Forms.Padding(24, 4, 5, 6)
        Me.lnkFlagged.Size = New System.Drawing.Size(200, 27)
        Me.lnkFlagged.TabIndex = 18
        Me.lnkFlagged.TabStop = True
        Me.lnkFlagged.Text = "Flagged"
        Me.lnkFlagged.Visible = False
        '
        'lnkImportant
        '
        Me.lnkImportant.Dock = System.Windows.Forms.DockStyle.Top
        Me.lnkImportant.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnkImportant.Image = Global.ImapX.VBSample.My.Resources.Resources.important
        Me.lnkImportant.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lnkImportant.LinkColor = System.Drawing.Color.Black
        Me.lnkImportant.Location = New System.Drawing.Point(10, 81)
        Me.lnkImportant.Name = "lnkImportant"
        Me.lnkImportant.Padding = New System.Windows.Forms.Padding(24, 4, 5, 6)
        Me.lnkImportant.Size = New System.Drawing.Size(200, 27)
        Me.lnkImportant.TabIndex = 17
        Me.lnkImportant.TabStop = True
        Me.lnkImportant.Text = "Important"
        Me.lnkImportant.Visible = False
        '
        'lnkDrafts
        '
        Me.lnkDrafts.Dock = System.Windows.Forms.DockStyle.Top
        Me.lnkDrafts.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnkDrafts.Image = Global.ImapX.VBSample.My.Resources.Resources.pencil
        Me.lnkDrafts.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lnkDrafts.LinkColor = System.Drawing.Color.Black
        Me.lnkDrafts.Location = New System.Drawing.Point(10, 54)
        Me.lnkDrafts.Name = "lnkDrafts"
        Me.lnkDrafts.Padding = New System.Windows.Forms.Padding(24, 4, 5, 6)
        Me.lnkDrafts.Size = New System.Drawing.Size(200, 27)
        Me.lnkDrafts.TabIndex = 16
        Me.lnkDrafts.TabStop = True
        Me.lnkDrafts.Text = "Drafts"
        Me.lnkDrafts.Visible = False
        '
        'lnkSent
        '
        Me.lnkSent.Dock = System.Windows.Forms.DockStyle.Top
        Me.lnkSent.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnkSent.Image = Global.ImapX.VBSample.My.Resources.Resources.paper_plane
        Me.lnkSent.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lnkSent.LinkColor = System.Drawing.Color.Black
        Me.lnkSent.Location = New System.Drawing.Point(10, 27)
        Me.lnkSent.Name = "lnkSent"
        Me.lnkSent.Padding = New System.Windows.Forms.Padding(24, 4, 5, 6)
        Me.lnkSent.Size = New System.Drawing.Size(200, 27)
        Me.lnkSent.TabIndex = 15
        Me.lnkSent.TabStop = True
        Me.lnkSent.Text = "Sent"
        Me.lnkSent.Visible = False
        '
        'lnkInbox
        '
        Me.lnkInbox.Dock = System.Windows.Forms.DockStyle.Top
        Me.lnkInbox.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lnkInbox.Image = Global.ImapX.VBSample.My.Resources.Resources.inbox
        Me.lnkInbox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lnkInbox.LinkColor = System.Drawing.Color.Black
        Me.lnkInbox.Location = New System.Drawing.Point(10, 0)
        Me.lnkInbox.Name = "lnkInbox"
        Me.lnkInbox.Padding = New System.Windows.Forms.Padding(24, 4, 5, 6)
        Me.lnkInbox.Size = New System.Drawing.Size(200, 27)
        Me.lnkInbox.TabIndex = 14
        Me.lnkInbox.TabStop = True
        Me.lnkInbox.Text = "Inbox"
        Me.lnkInbox.Visible = False
        '
        'lblFavorites
        '
        Me.lblFavorites.AutoSize = True
        Me.lblFavorites.BackColor = System.Drawing.Color.Transparent
        Me.lblFavorites.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblFavorites.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFavorites.Location = New System.Drawing.Point(10, 10)
        Me.lblFavorites.Name = "lblFavorites"
        Me.lblFavorites.Padding = New System.Windows.Forms.Padding(8)
        Me.lblFavorites.Size = New System.Drawing.Size(89, 37)
        Me.lblFavorites.TabIndex = 4
        Me.lblFavorites.Text = "Favorites"
        '
        'pnlWrap
        '
        Me.pnlWrap.Controls.Add(Me.splitContainer1)
        Me.pnlWrap.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlWrap.Location = New System.Drawing.Point(220, 0)
        Me.pnlWrap.Name = "pnlWrap"
        Me.pnlWrap.Padding = New System.Windows.Forms.Padding(10)
        Me.pnlWrap.Size = New System.Drawing.Size(764, 557)
        Me.pnlWrap.TabIndex = 1
        '
        'splitContainer1
        '
        Me.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitContainer1.Location = New System.Drawing.Point(10, 10)
        Me.splitContainer1.Name = "splitContainer1"
        '
        'splitContainer1.Panel1
        '
        Me.splitContainer1.Panel1.BackColor = System.Drawing.Color.White
        Me.splitContainer1.Panel1.Controls.Add(Me.pnlMessages)
        Me.splitContainer1.Panel1.Controls.Add(Me.pnlSelectFolder)
        Me.splitContainer1.Panel1.Controls.Add(Me.pnlLoading)
        '
        'splitContainer1.Panel2
        '
        Me.splitContainer1.Panel2.BackColor = System.Drawing.Color.White
        Me.splitContainer1.Panel2.Controls.Add(Me.pnlView)
        Me.splitContainer1.Panel2.Controls.Add(Me.pnlDownloadingBody)
        Me.splitContainer1.Panel2.Controls.Add(Me.pnlInfo)
        Me.splitContainer1.Panel2.Padding = New System.Windows.Forms.Padding(10)
        Me.splitContainer1.Size = New System.Drawing.Size(744, 537)
        Me.splitContainer1.SplitterDistance = 307
        Me.splitContainer1.TabIndex = 0
        '
        'pnlMessages
        '
        Me.pnlMessages.Controls.Add(Me.lsvMessages)
        Me.pnlMessages.Controls.Add(Me.panel5)
        Me.pnlMessages.Controls.Add(Me.lblFolder)
        Me.pnlMessages.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlMessages.Location = New System.Drawing.Point(0, 101)
        Me.pnlMessages.Name = "pnlMessages"
        Me.pnlMessages.Padding = New System.Windows.Forms.Padding(10)
        Me.pnlMessages.Size = New System.Drawing.Size(307, 436)
        Me.pnlMessages.TabIndex = 4
        Me.pnlMessages.Visible = False
        '
        'lsvMessages
        '
        Me.lsvMessages.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lsvMessages.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.clmMessages})
        Me.lsvMessages.ContextMenuStrip = Me.mnuMessage
        Me.lsvMessages.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lsvMessages.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.lsvMessages.FullRowSelect = True
        Me.lsvMessages.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lsvMessages.HideSelection = False
        Me.lsvMessages.Location = New System.Drawing.Point(10, 51)
        Me.lsvMessages.MultiSelect = False
        Me.lsvMessages.Name = "lsvMessages"
        Me.lsvMessages.Size = New System.Drawing.Size(287, 375)
        Me.lsvMessages.SmallImageList = Me.istPlaceHolder
        Me.lsvMessages.TabIndex = 3
        Me.lsvMessages.UseCompatibleStateImageBehavior = False
        Me.lsvMessages.View = System.Windows.Forms.View.Details
        Me.lsvMessages.VirtualMode = True
        '
        'clmMessages
        '
        Me.clmMessages.Text = "Message"
        '
        'mnuMessage
        '
        Me.mnuMessage.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.seenToolStripMenuItem, Me.toolStripMenuItem3, Me.copyToToolStripMenuItem, Me.moveToToolStripMenuItem, Me.toolStripMenuItem4, Me.deleteToolStripMenuItem1, Me.toolStripMenuItem6, Me.exportMessageToolStripMenuItem})
        Me.mnuMessage.Name = "mnuMessage"
        Me.mnuMessage.Size = New System.Drawing.Size(166, 132)
        '
        'seenToolStripMenuItem
        '
        Me.seenToolStripMenuItem.Name = "seenToolStripMenuItem"
        Me.seenToolStripMenuItem.Size = New System.Drawing.Size(165, 22)
        Me.seenToolStripMenuItem.Text = "&Seen"
        '
        'toolStripMenuItem3
        '
        Me.toolStripMenuItem3.Name = "toolStripMenuItem3"
        Me.toolStripMenuItem3.Size = New System.Drawing.Size(162, 6)
        '
        'copyToToolStripMenuItem
        '
        Me.copyToToolStripMenuItem.Name = "copyToToolStripMenuItem"
        Me.copyToToolStripMenuItem.Size = New System.Drawing.Size(165, 22)
        Me.copyToToolStripMenuItem.Text = "&Copy to..."
        '
        'moveToToolStripMenuItem
        '
        Me.moveToToolStripMenuItem.Name = "moveToToolStripMenuItem"
        Me.moveToToolStripMenuItem.Size = New System.Drawing.Size(165, 22)
        Me.moveToToolStripMenuItem.Text = "&Move to..."
        '
        'toolStripMenuItem4
        '
        Me.toolStripMenuItem4.Name = "toolStripMenuItem4"
        Me.toolStripMenuItem4.Size = New System.Drawing.Size(162, 6)
        '
        'deleteToolStripMenuItem1
        '
        Me.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1"
        Me.deleteToolStripMenuItem1.Size = New System.Drawing.Size(165, 22)
        Me.deleteToolStripMenuItem1.Text = "&Delete"
        '
        'toolStripMenuItem6
        '
        Me.toolStripMenuItem6.Name = "toolStripMenuItem6"
        Me.toolStripMenuItem6.Size = New System.Drawing.Size(162, 6)
        '
        'exportMessageToolStripMenuItem
        '
        Me.exportMessageToolStripMenuItem.Name = "exportMessageToolStripMenuItem"
        Me.exportMessageToolStripMenuItem.Size = New System.Drawing.Size(165, 22)
        Me.exportMessageToolStripMenuItem.Text = "&Export message..."
        '
        'istPlaceHolder
        '
        Me.istPlaceHolder.ImageStream = CType(resources.GetObject("istPlaceHolder.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.istPlaceHolder.TransparentColor = System.Drawing.Color.Transparent
        Me.istPlaceHolder.Images.SetKeyName(0, "attach.png")
        '
        'panel5
        '
        Me.panel5.BackColor = System.Drawing.Color.Transparent
        Me.panel5.Dock = System.Windows.Forms.DockStyle.Top
        Me.panel5.Location = New System.Drawing.Point(10, 41)
        Me.panel5.Name = "panel5"
        Me.panel5.Size = New System.Drawing.Size(287, 10)
        Me.panel5.TabIndex = 18
        '
        'lblFolder
        '
        Me.lblFolder.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.lblFolder.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblFolder.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFolder.Location = New System.Drawing.Point(10, 10)
        Me.lblFolder.Name = "lblFolder"
        Me.lblFolder.Padding = New System.Windows.Forms.Padding(5)
        Me.lblFolder.Size = New System.Drawing.Size(287, 31)
        Me.lblFolder.TabIndex = 5
        Me.lblFolder.Text = "Folder"
        '
        'pnlSelectFolder
        '
        Me.pnlSelectFolder.Controls.Add(Me.label2)
        Me.pnlSelectFolder.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlSelectFolder.Location = New System.Drawing.Point(0, 64)
        Me.pnlSelectFolder.Name = "pnlSelectFolder"
        Me.pnlSelectFolder.Padding = New System.Windows.Forms.Padding(10)
        Me.pnlSelectFolder.Size = New System.Drawing.Size(307, 37)
        Me.pnlSelectFolder.TabIndex = 2
        '
        'label2
        '
        Me.label2.Dock = System.Windows.Forms.DockStyle.Top
        Me.label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.label2.Location = New System.Drawing.Point(10, 10)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(287, 17)
        Me.label2.TabIndex = 1
        Me.label2.Text = "please select a folder"
        Me.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'pnlLoading
        '
        Me.pnlLoading.Controls.Add(Me.pgbMessages)
        Me.pnlLoading.Controls.Add(Me.label1)
        Me.pnlLoading.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlLoading.Location = New System.Drawing.Point(0, 0)
        Me.pnlLoading.Name = "pnlLoading"
        Me.pnlLoading.Padding = New System.Windows.Forms.Padding(10)
        Me.pnlLoading.Size = New System.Drawing.Size(307, 64)
        Me.pnlLoading.TabIndex = 1
        Me.pnlLoading.Visible = False
        '
        'pgbMessages
        '
        Me.pgbMessages.Dock = System.Windows.Forms.DockStyle.Top
        Me.pgbMessages.Location = New System.Drawing.Point(10, 35)
        Me.pgbMessages.Name = "pgbMessages"
        Me.pgbMessages.Size = New System.Drawing.Size(287, 17)
        Me.pgbMessages.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.pgbMessages.TabIndex = 0
        '
        'label1
        '
        Me.label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.label1.Location = New System.Drawing.Point(10, 10)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(287, 25)
        Me.label1.TabIndex = 1
        Me.label1.Text = "loading messages..."
        Me.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'pnlView
        '
        Me.pnlView.Controls.Add(Me.wbrMain)
        Me.pnlView.Controls.Add(Me.pnlAttachments)
        Me.pnlView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlView.Location = New System.Drawing.Point(10, 198)
        Me.pnlView.Name = "pnlView"
        Me.pnlView.Padding = New System.Windows.Forms.Padding(0, 10, 0, 0)
        Me.pnlView.Size = New System.Drawing.Size(413, 329)
        Me.pnlView.TabIndex = 1
        Me.pnlView.Visible = False
        '
        'wbrMain
        '
        Me.wbrMain.AllowWebBrowserDrop = False
        Me.wbrMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.wbrMain.IsWebBrowserContextMenuEnabled = False
        Me.wbrMain.Location = New System.Drawing.Point(0, 10)
        Me.wbrMain.MinimumSize = New System.Drawing.Size(20, 20)
        Me.wbrMain.Name = "wbrMain"
        Me.wbrMain.ScriptErrorsSuppressed = True
        Me.wbrMain.Size = New System.Drawing.Size(413, 211)
        Me.wbrMain.TabIndex = 0
        '
        'pnlAttachments
        '
        Me.pnlAttachments.BackColor = System.Drawing.Color.White
        Me.pnlAttachments.Controls.Add(Me.panel4)
        Me.pnlAttachments.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlAttachments.Location = New System.Drawing.Point(0, 221)
        Me.pnlAttachments.Name = "pnlAttachments"
        Me.pnlAttachments.Padding = New System.Windows.Forms.Padding(0, 5, 0, 0)
        Me.pnlAttachments.Size = New System.Drawing.Size(413, 108)
        Me.pnlAttachments.TabIndex = 1
        Me.pnlAttachments.Visible = False
        '
        'panel4
        '
        Me.panel4.BackColor = System.Drawing.Color.White
        Me.panel4.Controls.Add(Me.lsvAttachments)
        Me.panel4.Controls.Add(Me.label3)
        Me.panel4.Controls.Add(Me.panel8)
        Me.panel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panel4.Location = New System.Drawing.Point(0, 5)
        Me.panel4.Name = "panel4"
        Me.panel4.Size = New System.Drawing.Size(413, 103)
        Me.panel4.TabIndex = 2
        '
        'lsvAttachments
        '
        Me.lsvAttachments.BackColor = System.Drawing.Color.White
        Me.lsvAttachments.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lsvAttachments.ContextMenuStrip = Me.mnuAttachment
        Me.lsvAttachments.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lsvAttachments.Location = New System.Drawing.Point(0, 28)
        Me.lsvAttachments.MultiSelect = False
        Me.lsvAttachments.Name = "lsvAttachments"
        Me.lsvAttachments.Size = New System.Drawing.Size(413, 75)
        Me.lsvAttachments.SmallImageList = Me.istAttachments
        Me.lsvAttachments.TabIndex = 0
        Me.lsvAttachments.UseCompatibleStateImageBehavior = False
        Me.lsvAttachments.View = System.Windows.Forms.View.List
        '
        'mnuAttachment
        '
        Me.mnuAttachment.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.downloadToolStripMenuItem, Me.openToolStripMenuItem, Me.saveAsToolStripMenuItem})
        Me.mnuAttachment.Name = "mnuAttachment"
        Me.mnuAttachment.Size = New System.Drawing.Size(129, 70)
        '
        'downloadToolStripMenuItem
        '
        Me.downloadToolStripMenuItem.Name = "downloadToolStripMenuItem"
        Me.downloadToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.downloadToolStripMenuItem.Text = "&Download"
        '
        'openToolStripMenuItem
        '
        Me.openToolStripMenuItem.Name = "openToolStripMenuItem"
        Me.openToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.openToolStripMenuItem.Text = "&Open"
        '
        'saveAsToolStripMenuItem
        '
        Me.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem"
        Me.saveAsToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.saveAsToolStripMenuItem.Text = "&Save as..."
        '
        'istAttachments
        '
        Me.istAttachments.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.istAttachments.ImageSize = New System.Drawing.Size(16, 16)
        Me.istAttachments.TransparentColor = System.Drawing.Color.Transparent
        '
        'label3
        '
        Me.label3.Dock = System.Windows.Forms.DockStyle.Top
        Me.label3.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label3.Location = New System.Drawing.Point(0, 1)
        Me.label3.Name = "label3"
        Me.label3.Padding = New System.Windows.Forms.Padding(0, 5, 0, 5)
        Me.label3.Size = New System.Drawing.Size(413, 27)
        Me.label3.TabIndex = 1
        Me.label3.Text = "Attachments"
        '
        'panel8
        '
        Me.panel8.BackColor = System.Drawing.Color.FromArgb(CType(CType(171, Byte), Integer), CType(CType(171, Byte), Integer), CType(CType(171, Byte), Integer))
        Me.panel8.Dock = System.Windows.Forms.DockStyle.Top
        Me.panel8.Location = New System.Drawing.Point(0, 0)
        Me.panel8.Name = "panel8"
        Me.panel8.Size = New System.Drawing.Size(413, 1)
        Me.panel8.TabIndex = 17
        '
        'pnlDownloadingBody
        '
        Me.pnlDownloadingBody.AutoSize = True
        Me.pnlDownloadingBody.Controls.Add(Me.lblFailedDownloadBody)
        Me.pnlDownloadingBody.Controls.Add(Me.lblDownloadingBody)
        Me.pnlDownloadingBody.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlDownloadingBody.Location = New System.Drawing.Point(10, 144)
        Me.pnlDownloadingBody.Name = "pnlDownloadingBody"
        Me.pnlDownloadingBody.Padding = New System.Windows.Forms.Padding(10)
        Me.pnlDownloadingBody.Size = New System.Drawing.Size(413, 54)
        Me.pnlDownloadingBody.TabIndex = 20
        Me.pnlDownloadingBody.Visible = False
        '
        'lblFailedDownloadBody
        '
        Me.lblFailedDownloadBody.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblFailedDownloadBody.ForeColor = System.Drawing.Color.Red
        Me.lblFailedDownloadBody.Location = New System.Drawing.Point(10, 27)
        Me.lblFailedDownloadBody.Name = "lblFailedDownloadBody"
        Me.lblFailedDownloadBody.Size = New System.Drawing.Size(393, 17)
        Me.lblFailedDownloadBody.TabIndex = 1
        Me.lblFailedDownloadBody.Text = "Failed to download body"
        Me.lblFailedDownloadBody.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lblFailedDownloadBody.Visible = False
        '
        'lblDownloadingBody
        '
        Me.lblDownloadingBody.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblDownloadingBody.Location = New System.Drawing.Point(10, 10)
        Me.lblDownloadingBody.Name = "lblDownloadingBody"
        Me.lblDownloadingBody.Size = New System.Drawing.Size(393, 17)
        Me.lblDownloadingBody.TabIndex = 0
        Me.lblDownloadingBody.Text = "Downloading body..."
        Me.lblDownloadingBody.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'pnlInfo
        '
        Me.pnlInfo.AutoSize = True
        Me.pnlInfo.BackColor = System.Drawing.Color.White
        Me.pnlInfo.Controls.Add(Me.pnlEmbeddedResources)
        Me.pnlInfo.Controls.Add(Me.panel7)
        Me.pnlInfo.Controls.Add(Me.panel6)
        Me.pnlInfo.Controls.Add(Me.lblLabels)
        Me.pnlInfo.Controls.Add(Me.lblFrom)
        Me.pnlInfo.Controls.Add(Me.lblSubject)
        Me.pnlInfo.Controls.Add(Me.lblDate)
        Me.pnlInfo.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlInfo.Location = New System.Drawing.Point(10, 10)
        Me.pnlInfo.Name = "pnlInfo"
        Me.pnlInfo.Size = New System.Drawing.Size(413, 134)
        Me.pnlInfo.TabIndex = 0
        Me.pnlInfo.Visible = False
        '
        'pnlEmbeddedResources
        '
        Me.pnlEmbeddedResources.AutoSize = True
        Me.pnlEmbeddedResources.BackColor = System.Drawing.Color.DarkGreen
        Me.pnlEmbeddedResources.Controls.Add(Me.lnkDownloadEmbeddedResources)
        Me.pnlEmbeddedResources.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlEmbeddedResources.Location = New System.Drawing.Point(0, 107)
        Me.pnlEmbeddedResources.Name = "pnlEmbeddedResources"
        Me.pnlEmbeddedResources.Padding = New System.Windows.Forms.Padding(5)
        Me.pnlEmbeddedResources.Size = New System.Drawing.Size(413, 27)
        Me.pnlEmbeddedResources.TabIndex = 22
        Me.pnlEmbeddedResources.Visible = False
        '
        'lnkDownloadEmbeddedResources
        '
        Me.lnkDownloadEmbeddedResources.AutoSize = True
        Me.lnkDownloadEmbeddedResources.BackColor = System.Drawing.Color.Transparent
        Me.lnkDownloadEmbeddedResources.DisabledLinkColor = System.Drawing.Color.Black
        Me.lnkDownloadEmbeddedResources.Dock = System.Windows.Forms.DockStyle.Top
        Me.lnkDownloadEmbeddedResources.LinkColor = System.Drawing.Color.White
        Me.lnkDownloadEmbeddedResources.Location = New System.Drawing.Point(5, 5)
        Me.lnkDownloadEmbeddedResources.Name = "lnkDownloadEmbeddedResources"
        Me.lnkDownloadEmbeddedResources.Size = New System.Drawing.Size(404, 17)
        Me.lnkDownloadEmbeddedResources.TabIndex = 1
        Me.lnkDownloadEmbeddedResources.TabStop = True
        Me.lnkDownloadEmbeddedResources.Text = "Message contains embedded images. Click here to download them."
        '
        'panel7
        '
        Me.panel7.BackColor = System.Drawing.Color.FromArgb(CType(CType(171, Byte), Integer), CType(CType(171, Byte), Integer), CType(CType(171, Byte), Integer))
        Me.panel7.Dock = System.Windows.Forms.DockStyle.Top
        Me.panel7.Location = New System.Drawing.Point(0, 106)
        Me.panel7.Name = "panel7"
        Me.panel7.Size = New System.Drawing.Size(413, 1)
        Me.panel7.TabIndex = 21
        '
        'panel6
        '
        Me.panel6.BackColor = System.Drawing.Color.Transparent
        Me.panel6.Dock = System.Windows.Forms.DockStyle.Top
        Me.panel6.Location = New System.Drawing.Point(0, 96)
        Me.panel6.Name = "panel6"
        Me.panel6.Size = New System.Drawing.Size(413, 10)
        Me.panel6.TabIndex = 20
        '
        'lblLabels
        '
        Me.lblLabels.AutoSize = True
        Me.lblLabels.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblLabels.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLabels.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(102, Byte), Integer))
        Me.lblLabels.Location = New System.Drawing.Point(0, 69)
        Me.lblLabels.Name = "lblLabels"
        Me.lblLabels.Padding = New System.Windows.Forms.Padding(1, 5, 0, 5)
        Me.lblLabels.Size = New System.Drawing.Size(79, 27)
        Me.lblLabels.TabIndex = 23
        Me.lblLabels.Text = "GmailLabels"
        Me.lblLabels.Visible = False
        '
        'lblFrom
        '
        Me.lblFrom.AutoSize = True
        Me.lblFrom.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblFrom.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFrom.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(102, Byte), Integer))
        Me.lblFrom.Location = New System.Drawing.Point(0, 42)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Padding = New System.Windows.Forms.Padding(1, 5, 0, 5)
        Me.lblFrom.Size = New System.Drawing.Size(154, 27)
        Me.lblFrom.TabIndex = 1
        Me.lblFrom.Text = "From: mail@domain.com"
        '
        'lblSubject
        '
        Me.lblSubject.AutoSize = True
        Me.lblSubject.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblSubject.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubject.ForeColor = System.Drawing.Color.FromArgb(CType(CType(68, Byte), Integer), CType(CType(68, Byte), Integer), CType(CType(68, Byte), Integer))
        Me.lblSubject.Location = New System.Drawing.Point(0, 22)
        Me.lblSubject.Name = "lblSubject"
        Me.lblSubject.Size = New System.Drawing.Size(58, 20)
        Me.lblSubject.TabIndex = 2
        Me.lblSubject.Text = "Subject"
        '
        'lblDate
        '
        Me.lblDate.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblDate.ForeColor = System.Drawing.Color.FromArgb(CType(CType(102, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(102, Byte), Integer))
        Me.lblDate.Location = New System.Drawing.Point(0, 0)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.Padding = New System.Windows.Forms.Padding(1, 0, 0, 5)
        Me.lblDate.Size = New System.Drawing.Size(413, 22)
        Me.lblDate.TabIndex = 0
        Me.lblDate.Text = "Date"
        '
        'mnuFolder
        '
        Me.mnuFolder.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.renameToolStripMenuItem, Me.deleteToolStripMenuItem, Me.toolStripMenuItem1, Me.addSubfolderToolStripMenuItem, Me.toolStripMenuItem2, Me.importMessageToolStripMenuItem, Me.toolStripMenuItem5, Me.emptyToolStripMenuItem})
        Me.mnuFolder.Name = "mnuFolder"
        Me.mnuFolder.Size = New System.Drawing.Size(160, 132)
        '
        'renameToolStripMenuItem
        '
        Me.renameToolStripMenuItem.Name = "renameToolStripMenuItem"
        Me.renameToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
        Me.renameToolStripMenuItem.Text = "&Rename"
        '
        'deleteToolStripMenuItem
        '
        Me.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem"
        Me.deleteToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
        Me.deleteToolStripMenuItem.Text = "&Delete"
        '
        'toolStripMenuItem1
        '
        Me.toolStripMenuItem1.Name = "toolStripMenuItem1"
        Me.toolStripMenuItem1.Size = New System.Drawing.Size(156, 6)
        '
        'addSubfolderToolStripMenuItem
        '
        Me.addSubfolderToolStripMenuItem.Name = "addSubfolderToolStripMenuItem"
        Me.addSubfolderToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
        Me.addSubfolderToolStripMenuItem.Text = "&Add subfolder"
        '
        'toolStripMenuItem2
        '
        Me.toolStripMenuItem2.Name = "toolStripMenuItem2"
        Me.toolStripMenuItem2.Size = New System.Drawing.Size(156, 6)
        '
        'importMessageToolStripMenuItem
        '
        Me.importMessageToolStripMenuItem.Name = "importMessageToolStripMenuItem"
        Me.importMessageToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
        Me.importMessageToolStripMenuItem.Text = "&Import message"
        '
        'toolStripMenuItem5
        '
        Me.toolStripMenuItem5.Name = "toolStripMenuItem5"
        Me.toolStripMenuItem5.Size = New System.Drawing.Size(156, 6)
        '
        'emptyToolStripMenuItem
        '
        Me.emptyToolStripMenuItem.Name = "emptyToolStripMenuItem"
        Me.emptyToolStripMenuItem.Size = New System.Drawing.Size(159, 22)
        Me.emptyToolStripMenuItem.Text = "&Empty folder"
        '
        'ofdImportMessage
        '
        Me.ofdImportMessage.Filter = "Message files|*.eml|All files|*.*"
        Me.ofdImportMessage.Title = "Import message"
        '
        'sfdExportMessage
        '
        Me.sfdExportMessage.Filter = "Eml file|*.eml|All files|*.*"
        Me.sfdExportMessage.Title = "Export message"
        '
        'sfdSaveAttachment
        '
        Me.sfdSaveAttachment.Filter = "All files|*.*"
        Me.sfdSaveAttachment.Title = "Save attachment"
        '
        'FrmMain
        '
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(984, 557)
        Me.Controls.Add(Me.pnlWrap)
        Me.Controls.Add(Me.pnlFolders)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "FrmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ImapX"
        Me.pnlFolders.ResumeLayout(False)
        Me.pnlFolders.PerformLayout()
        Me.pnlFavorites.ResumeLayout(False)
        Me.pnlWrap.ResumeLayout(False)
        Me.splitContainer1.Panel1.ResumeLayout(False)
        Me.splitContainer1.Panel2.ResumeLayout(False)
        Me.splitContainer1.Panel2.PerformLayout()
        CType(Me.splitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitContainer1.ResumeLayout(False)
        Me.pnlMessages.ResumeLayout(False)
        Me.mnuMessage.ResumeLayout(False)
        Me.pnlSelectFolder.ResumeLayout(False)
        Me.pnlLoading.ResumeLayout(False)
        Me.pnlView.ResumeLayout(False)
        Me.pnlAttachments.ResumeLayout(False)
        Me.panel4.ResumeLayout(False)
        Me.mnuAttachment.ResumeLayout(False)
        Me.pnlDownloadingBody.ResumeLayout(False)
        Me.pnlInfo.ResumeLayout(False)
        Me.pnlInfo.PerformLayout()
        Me.pnlEmbeddedResources.ResumeLayout(False)
        Me.pnlEmbeddedResources.PerformLayout()
        Me.mnuFolder.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Private WithEvents addSubfolderToolStripMenuItem As ToolStripMenuItem
    Private clmMessages As ColumnHeader
    Private WithEvents copyToToolStripMenuItem As ToolStripMenuItem
    Private WithEvents deleteToolStripMenuItem As ToolStripMenuItem
    Private WithEvents deleteToolStripMenuItem1 As ToolStripMenuItem
    Private WithEvents downloadToolStripMenuItem As ToolStripMenuItem
    Private WithEvents emptyToolStripMenuItem As ToolStripMenuItem
    Private WithEvents exportMessageToolStripMenuItem As ToolStripMenuItem
    Private WithEvents importMessageToolStripMenuItem As ToolStripMenuItem
    Private istAttachments As ImageList
    Private istPlaceHolder As ImageList
    Private label1 As Label
    Private label2 As Label
    Private label3 As Label
    Private lblDate As Label
    Private lblDownloadingBody As Label
    Private lblFailedDownloadBody As Label
    Private lblFavorites As Label
    Private lblFolder As Label
    Private WithEvents lblFrom As Label
    Private WithEvents lblLabels As Label
    Private WithEvents lblSubject As Label
    Private WithEvents lnkAll As LinkLabel
    Private WithEvents lnkArchive As LinkLabel
    Private WithEvents lnkDownloadEmbeddedResources As LinkLabel
    Private WithEvents lnkDrafts As LinkLabel
    Private WithEvents lnkFlagged As LinkLabel
    Private WithEvents lnkImportant As LinkLabel
    Private WithEvents lnkInbox As LinkLabel
    Private WithEvents lnkJunk As LinkLabel
    Private WithEvents lnkSent As LinkLabel
    Private WithEvents lnkTrash As LinkLabel
    Private WithEvents lsvAttachments As ListView
    Private WithEvents lsvMessages As ListView
    Private WithEvents mnuAttachment As ContextMenuStrip
    Private mnuFolder As ContextMenuStrip
    Private WithEvents mnuMessage As ContextMenuStrip
    Private WithEvents moveToToolStripMenuItem As ToolStripMenuItem
    Private ofdImportMessage As OpenFileDialog
    Private WithEvents openToolStripMenuItem As ToolStripMenuItem
    Private panel1 As Panel
    Private panel2 As Panel
    Private panel3 As Panel
    Private panel4 As Panel
    Private panel5 As Panel
    Private panel6 As Panel
    Private panel7 As Panel
    Private panel8 As Panel
    Private pgbMessages As ProgressBar
    Private pnlAttachments As Panel
    Private pnlDownloadingBody As Panel
    Private pnlEmbeddedResources As Panel
    Private pnlFavorites As Panel
    Private pnlFolders As Panel
    Private pnlInfo As Panel
    Private pnlLoading As Panel
    Private pnlMessages As Panel
    Private pnlSelectFolder As Panel
    Private pnlView As Panel
    Private pnlWrap As Panel
    Private WithEvents renameToolStripMenuItem As ToolStripMenuItem
    Private WithEvents saveAsToolStripMenuItem As ToolStripMenuItem
    Private WithEvents seenToolStripMenuItem As ToolStripMenuItem
    Private sfdExportMessage As SaveFileDialog
    Private sfdSaveAttachment As SaveFileDialog
    Private splitContainer1 As SplitContainer
    Private toolStripMenuItem1 As ToolStripSeparator
    Private toolStripMenuItem2 As ToolStripSeparator
    Private toolStripMenuItem3 As ToolStripSeparator
    Private toolStripMenuItem4 As ToolStripSeparator
    Private toolStripMenuItem5 As ToolStripSeparator
    Private toolStripMenuItem6 As ToolStripSeparator
    Private WithEvents trwFolders As TreeView
    Private wbrMain As WebBrowser


End Class
