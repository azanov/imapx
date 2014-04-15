Imports ImapX.Enums
Imports System.IO
Imports System.Globalization
Imports System.Threading
Imports System.Linq
Imports System.ComponentModel
Imports System.Web

Public Class FrmMain

    Private _lastClickedNode As TreeNode
    Private ReadOnly _messageItems As Dictionary(Of Long, ListViewItem)
    Private _messages As List(Of Message)
    Private _selectedFolder As Folder
    Private _selectedMessage As Message

    Public Sub New()
        Me.components = Nothing
        Me.InitializeComponent()
        Me._messageItems = New Dictionary(Of Long, ListViewItem)
        Using frmStart As FrmConnect = New FrmConnect
            If (frmStart.ShowDialog = DialogResult.OK) Then
                Me.wbrMain.Navigate("about:blank")
                Me.BindFolders()
                Me.ConfigureClient()
            Else
                Application.Exit()
            End If
        End Using
    End Sub

    Private Sub AddSubFolder(ByVal folder As Folder, ByVal folderName As String)
        Dim args As ServerCallCompletedEventArgs
        Try
            Dim subFolder As Folder = folder.SubFolders.Add(folderName)
            args = New ServerCallCompletedEventArgs(True, Nothing, Nothing) With { _
                .Arg = subFolder _
            }

            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.AddSubFolderCompleted), New Object() {My.MyApplication.ImapClient, args})
        Catch ex As Exception
            args = New ServerCallCompletedEventArgs(False, ex, Nothing)
            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.AddSubFolderCompleted), New Object() {My.MyApplication.ImapClient, args})
        End Try
    End Sub

    Private Sub AddSubFolderCompleted(ByVal sender As Object, ByVal e As ServerCallCompletedEventArgs)
        Me.trwFolders.Enabled = True
        If e.Result Then
            Me._lastClickedNode.Nodes.Add(Me.FolderToNode(TryCast(e.Arg, Folder)))
        Else
            MessageBox.Show("Failed to create subfolder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End If
    End Sub

    Private Sub addSubfolderToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles addSubfolderToolStripMenuItem.Click
        Dim folderName As String
        Dim folder As Folder = TryCast(Me._lastClickedNode.Tag, Folder)
        If (Not folder Is Nothing) Then
            folderName = InputBox.Show("Create subfolder", "Enter a new folder name", "", Me)
            If (Not folderName Is Nothing) Then
                If String.IsNullOrEmpty(folderName.Trim) Then
                    MessageBox.Show("A valid folder name is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
                End If
                Me.trwFolders.Enabled = False
                Dim thread As New Thread(Function(f As Object)
                                             Me.AddSubFolder(folder, folderName)
                                         End Function)
                thread.Start()

            End If
        End If
    End Sub

    Private Sub AutoResizeMessageListViewColumn()
        Dim scrollBars As ScrollBars = NativeMethods.GetVisibleScrollbars(Me.lsvMessages)
        Me.clmMessages.Width = If(((scrollBars And scrollBars.Vertical) = scrollBars.Vertical), (Me.lsvMessages.Width - SystemInformation.VerticalScrollBarWidth), Me.lsvMessages.Width)
    End Sub

    Private Sub BindFolders()
        Me.lnkArchive.Visible = (Not My.MyApplication.ImapClient.Folders.Archive Is Nothing)
        Me.lnkAll.Visible = (Not My.MyApplication.ImapClient.Folders.All Is Nothing)
        Me.lnkTrash.Visible = (Not My.MyApplication.ImapClient.Folders.Trash Is Nothing)
        Me.lnkJunk.Visible = (Not My.MyApplication.ImapClient.Folders.Junk Is Nothing)
        Me.lnkFlagged.Visible = (Not My.MyApplication.ImapClient.Folders.Flagged Is Nothing)
        Me.lnkImportant.Visible = (Not My.MyApplication.ImapClient.Folders.Important Is Nothing)
        Me.lnkDrafts.Visible = (Not My.MyApplication.ImapClient.Folders.Drafts Is Nothing)
        Me.lnkSent.Visible = (Not My.MyApplication.ImapClient.Folders.Sent Is Nothing)
        Me.lnkInbox.Visible = (Not My.MyApplication.ImapClient.Folders.Inbox Is Nothing)
        Me.trwFolders.Nodes.Add(My.MyApplication.ImapClient.Host)
        Me.trwFolders.Nodes.Item(0).Nodes.AddRange(Enumerable.ToArray(Of TreeNode)(Enumerable.Select(Of Folder, TreeNode)(My.MyApplication.ImapClient.Folders, New Func(Of Folder, TreeNode)(AddressOf Me.FolderToNode))))
        Me.trwFolders.Nodes.Item(0).Expand()
    End Sub

    Private Sub ConfigureClient()
        My.MyApplication.ImapClient.Behavior.AutoDownloadBodyOnAccess = False
        My.MyApplication.ImapClient.Behavior.AutoPopulateFolderMessages = False
        My.MyApplication.ImapClient.Behavior.ExamineFolders = False
        My.MyApplication.ImapClient.Behavior.MessageFetchMode = (MessageFetchMode.GMailLabels Or MessageFetchMode.Tiny)
        My.MyApplication.ImapClient.Behavior.RequestedHeaders = New String() {"from", "date", "subject", "content-type", "importance"}
    End Sub

    Private Sub CopyMessageToFolder(ByVal message As Message, ByVal folder As Folder)
        Dim args As ServerCallCompletedEventArgs
        Try
            args = New ServerCallCompletedEventArgs(True, Nothing, Nothing) With { _
                .Result = message.CopyTo(folder, False) _
            }

            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.CopyMessageToFolderCompleted), New Object() {My.MyApplication.ImapClient, args})
        Catch ex As Exception
            args = New ServerCallCompletedEventArgs(False, ex, Nothing)
            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.CopyMessageToFolderCompleted), New Object() {My.MyApplication.ImapClient, args})
        End Try
    End Sub

    Private Sub CopyMessageToFolderCompleted(ByVal sender As Object, ByVal e As ServerCallCompletedEventArgs)
        Me.trwFolders.Enabled = Me.lsvMessages.Enabled = True
        If e.Result Then
            MessageBox.Show("Message copied", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        Else
            MessageBox.Show("Failed to copy message", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End If
    End Sub

    Private Sub copyToToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles copyToToolStripMenuItem.Click
        Dim folder As Folder = FolderBox.Show("Copy message to folder", "Please select target folder", Me)
        If (Not folder Is Nothing) Then
            Me.trwFolders.Enabled = Me.lsvMessages.Enabled = False
            Dim thread As New Thread(Function(f As Object)
                                         Me.CopyMessageToFolder(Me._selectedMessage, folder)
                                     End Function)
            thread.Start()

        End If
    End Sub

    Private Sub createNewMessageToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Private Sub DeleteFolder(ByVal folder As Folder)
        Dim args As ServerCallCompletedEventArgs
        Try
            args = New ServerCallCompletedEventArgs(True, Nothing, Nothing) With { _
                .Arg = folder, _
                .Result = folder.Remove _
            }

            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.DeleteFolderCompleted), New Object() {My.MyApplication.ImapClient, args})
        Catch ex As Exception
            args = New ServerCallCompletedEventArgs(False, ex, Nothing)
            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.DeleteFolderCompleted), New Object() {My.MyApplication.ImapClient, args})
        End Try
    End Sub

    Private Sub DeleteFolderCompleted(ByVal sender As Object, ByVal e As ServerCallCompletedEventArgs)
        Me.trwFolders.Enabled = True
        If e.Result Then
            Dim folder As Folder = TryCast(e.Arg, Folder)
            Dim lnk As LinkLabel = Me.FindFavoriteLnk(folder)
            If (Not lnk Is Nothing) Then
                lnk.Hide()
            End If
            If (Me._selectedFolder Is folder) Then
                Me.lsvMessages.VirtualListSize = 0
                Me._messages.Clear()
                Me.trwFolders.SelectedNode = Nothing
                Me.pnlSelectFolder.Show()
                Me.pnlMessages.Visible = Me.pnlInfo.Visible = Me.pnlView.Visible = Me.pnlDownloadingBody.Visible = False
            End If
            Me._lastClickedNode.Remove()
        Else
            MessageBox.Show("Failed to delete folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End If
    End Sub

    Private Sub deleteToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles deleteToolStripMenuItem.Click
        Dim folder As Folder = TryCast(Me._lastClickedNode.Tag, Folder)
        If ((Not folder Is Nothing) AndAlso (MessageBox.Show(("Do you really want to remove folder """ & folder.Name & """?"), "Remove folder", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes)) Then
            Me.trwFolders.Enabled = False
            Dim thread As New Thread(Function(f As Object)
                                         Me.DeleteFolder(folder)
                                     End Function)
            thread.Start()

        End If
    End Sub

    Private Sub deleteToolStripMenuItem1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles deleteToolStripMenuItem1.Click
        If (MessageBox.Show("Do you really want to remove this message?", "Remove folder", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes) Then
            Me.trwFolders.Enabled = Me.lsvMessages.Enabled = False
            Dim thread As New Thread(Function(f As Object)
                                         Me.RemoveMessage(Me._selectedMessage)
                                     End Function)
            thread.Start()

        End If
    End Sub

    Private Sub DownloadAttachment(ByVal index As Integer)
        Dim args As ServerCallCompletedEventArgs
        Try
            args = New ServerCallCompletedEventArgs(True, Nothing, Nothing) With { _
                .Arg = index _
            }

            Me._selectedMessage.Attachments(index).Download()
            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.DownloadAttachmentCompleted), New Object() {My.MyApplication.ImapClient, args})
        Catch ex As Exception
            args = New ServerCallCompletedEventArgs(False, ex, Nothing)
            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.DownloadAttachmentCompleted), New Object() {My.MyApplication.ImapClient, args})
        End Try
    End Sub

    Private Sub DownloadAttachmentCompleted(ByVal sender As Object, ByVal e As ServerCallCompletedEventArgs)
        Me.lsvAttachments.Enabled = Me.lsvMessages.Enabled = Me.trwFolders.Enabled = True
        If e.Result Then
            Dim index As Integer = CInt(e.Arg)
            Dim file As Attachment = Me._selectedMessage.Attachments(index)
            Me.lsvAttachments.Items.Item(index).Text = (file.FileName & " (" & Me.FormatFileSize(file.FileSize) & ")")
        Else
            MessageBox.Show("Failed to export message", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End If
    End Sub

    Private Sub DownloadBody()
        Dim args As ServerCallCompletedEventArgs
        Try
            Me._selectedMessage.Body.Download(BodyType.TextAndHtml)
            args = New ServerCallCompletedEventArgs(True, Nothing, Nothing)
            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.DownloadBodyCompleted), New Object() {My.MyApplication.ImapClient, args})
        Catch ex As Exception
            args = New ServerCallCompletedEventArgs(False, ex, Nothing)
            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.DownloadBodyCompleted), New Object() {My.MyApplication.ImapClient, args})
        End Try
    End Sub

    Private Sub DownloadBodyCompleted(ByVal sender As Object, ByVal e As ServerCallCompletedEventArgs)
        Me.trwFolders.Enabled = Me.lsvMessages.Enabled = True
        If e.Result Then
            Dim body As String = If(Me._selectedMessage.Body.HasHtml, Me._selectedMessage.Body.Html, Me._selectedMessage.Body.Text)
            Me.wbrMain.Document.OpenNew(True)
            Me.wbrMain.Document.Write(If(Me._selectedMessage.Body.HasHtml, body, HttpUtility.HtmlEncode(body).Replace(Environment.NewLine, "<br />")))
            If Not wbrMain.Document.Body Is Nothing Then
                Me.wbrMain.Document.Body.SetAttribute("scroll", "auto")
            End If
            Me.pnlDownloadingBody.Hide()
            Me.pnlView.Show()
            If ((Not Me._selectedMessage.Labels Is Nothing) AndAlso Enumerable.Any(Of String)(Me._selectedMessage.Labels)) Then
                Me.lblLabels.Text = ("Labels:" & String.Join(", ", Enumerable.ToArray(Of String)(Me._selectedMessage.Labels)))
                Me.lblLabels.Visible = True
            Else
                Me.lblLabels.Visible = False
            End If
            Me.pnlEmbeddedResources.Visible = Enumerable.Any(Of Attachment)(Me._selectedMessage.EmbeddedResources, Function(a As Attachment)
                                                                                                                       Return Not a.Downloaded
                                                                                                                   End Function)
        Else
            Me.lblDownloadingBody.Hide()
            Me.lblFailedDownloadBody.Show()
        End If
    End Sub

    Private Sub DownloadEmbeddedResources()
        Dim args As ServerCallCompletedEventArgs
        Try
            Dim res As Attachment
            For Each res In Me._selectedMessage.EmbeddedResources
                If Not res.Downloaded Then
                    res.Download()
                End If
            Next
            args = New ServerCallCompletedEventArgs(True, Nothing, Nothing)
            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.DownloadEmbeddedResourcesCompleted), New Object() {My.MyApplication.ImapClient, args})
        Catch ex As Exception
            args = New ServerCallCompletedEventArgs(False, ex, Nothing)
            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.DownloadEmbeddedResourcesCompleted), New Object() {My.MyApplication.ImapClient, args})
        End Try
    End Sub

    Private Sub DownloadEmbeddedResourcesCompleted(ByVal sender As Object, ByVal e As ServerCallCompletedEventArgs)
        Me.lnkDownloadEmbeddedResources.Enabled = True
        If e.Result Then
            Me.pnlEmbeddedResources.Hide()
            Dim body As String = If(Me._selectedMessage.Body.HasHtml, Me._selectedMessage.Body.Html, Me._selectedMessage.Body.Text)
            Dim msgTmpDir As String = Path.Combine(Path.Combine(Application.StartupPath, "tmp"), Me._selectedMessage.UId.ToString(CultureInfo.InvariantCulture))
            If Not Directory.Exists(msgTmpDir) Then
                Directory.CreateDirectory(msgTmpDir)
            End If
            Dim res As Attachment
            For Each res In Me._selectedMessage.EmbeddedResources
                Try
                    Dim path As String = IO.Path.Combine(msgTmpDir, res.FileName)
                    File.WriteAllBytes(path, res.FileData)
                    body = body.Replace(("cid:" & res.ContentId), path)
                Catch exception1 As Exception
                End Try
            Next
            Me.wbrMain.Document.OpenNew(True)
            Me.wbrMain.Document.Write(If(Me._selectedMessage.Body.HasHtml, body, body.Replace(ChrW(10), "<br />")))
        Else
            MessageBox.Show("Failed to download embedded images", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End If
    End Sub

    Private Sub downloadToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles downloadToolStripMenuItem.Click
        Me.lsvAttachments.Enabled = Me.lsvMessages.Enabled = Me.trwFolders.Enabled = False
        Dim index As Integer = Me.lsvAttachments.SelectedItems.Item(0).Index
        Dim item As Attachment = Me._selectedMessage.Attachments(index)
        If Not item.Downloaded Then
            Dim thread As New Thread(Function(f As Object)
                                         Me.DownloadAttachment(index)
                                     End Function)
            thread.Start()

        End If
    End Sub

    Private Sub EmptyFolder(ByVal folder As Folder)
        Dim args As ServerCallCompletedEventArgs
        Try
            args = New ServerCallCompletedEventArgs(True, Nothing, Nothing) With { _
                .Arg = folder, _
                .Result = folder.EmptyFolder _
            }

            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.EmptyFolderCompleted), New Object() {My.MyApplication.ImapClient, args})
        Catch ex As Exception
            args = New ServerCallCompletedEventArgs(False, ex, Nothing)
            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.EmptyFolderCompleted), New Object() {My.MyApplication.ImapClient, args})
        End Try
    End Sub

    Private Sub EmptyFolderCompleted(ByVal sender As Object, ByVal e As ServerCallCompletedEventArgs)
        Me.trwFolders.Enabled = True
        If e.Result Then
            Dim folder As Folder = TryCast(e.Arg, Folder)
            If (Me._selectedFolder Is folder) Then
                Me.lsvMessages.VirtualListSize = 0
                Me._messages.Clear()
            End If
        Else
            MessageBox.Show("Failed to empty folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End If
    End Sub

    Private Sub emptyToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles emptyToolStripMenuItem.Click
        Dim folder As Folder = TryCast(Me._lastClickedNode.Tag, Folder)
        If ((Not folder Is Nothing) AndAlso (MessageBox.Show(("Do you really want to empty the folder """ & folder.Name & """?"), "Remove folder", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes)) Then
            Me.trwFolders.Enabled = False
            Dim thread As New Thread(Function(f As Object)
                                         Me.EmptyFolder(folder)
                                     End Function)
            thread.Start()

        End If
    End Sub

    Private Sub ExportMessage(ByVal path As String)
        Dim args As ServerCallCompletedEventArgs
        Dim headers As String() = My.MyApplication.ImapClient.Behavior.RequestedHeaders
        Try
            args = New ServerCallCompletedEventArgs(True, Nothing, Nothing)
            My.MyApplication.ImapClient.Behavior.RequestedHeaders = Nothing
            Me._selectedMessage.Download(MessageFetchMode.Full, True)
            Me._selectedMessage.Save(path)
            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.ExportMessageCompleted), New Object() {My.MyApplication.ImapClient, args})
        Catch ex As Exception
            args = New ServerCallCompletedEventArgs(False, ex, Nothing)
            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.ExportMessageCompleted), New Object() {My.MyApplication.ImapClient, args})
        Finally
            My.MyApplication.ImapClient.Behavior.RequestedHeaders = headers
        End Try
    End Sub

    Private Sub ExportMessageCompleted(ByVal sender As Object, ByVal e As ServerCallCompletedEventArgs)
        Me.trwFolders.Enabled = True
        If e.Result Then
            MessageBox.Show("Message exported!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        Else
            MessageBox.Show("Failed to export message", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End If
    End Sub

    Private Sub exportMessageToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles exportMessageToolStripMenuItem.Click
        If (Me.sfdExportMessage.ShowDialog(Me) = DialogResult.OK) Then
            Me.trwFolders.Enabled = False
            Dim thread As New Thread(Function(f As Object)
                                         Me.ExportMessage(Me.sfdExportMessage.FileName)
                                     End Function)
            thread.Start()

        End If
    End Sub

    Private Function FindFavoriteLnk(ByVal folder As Folder) As LinkLabel
        If (folder Is My.MyApplication.ImapClient.Folders.All) Then
            Return Me.lnkAll
        End If
        If (folder Is My.MyApplication.ImapClient.Folders.Archive) Then
            Return Me.lnkArchive
        End If
        If (folder Is My.MyApplication.ImapClient.Folders.Drafts) Then
            Return Me.lnkDrafts
        End If
        If (folder Is My.MyApplication.ImapClient.Folders.Flagged) Then
            Return Me.lnkFlagged
        End If
        If (folder Is My.MyApplication.ImapClient.Folders.Important) Then
            Return Me.lnkImportant
        End If
        If (folder Is My.MyApplication.ImapClient.Folders.Inbox) Then
            Return Me.lnkInbox
        End If
        If (folder Is My.MyApplication.ImapClient.Folders.Junk) Then
            Return Me.lnkJunk
        End If
        If (folder Is My.MyApplication.ImapClient.Folders.Sent) Then
            Return Me.lnkSent
        End If
        Return If((folder Is My.MyApplication.ImapClient.Folders.Trash), Me.lnkTrash, Nothing)
    End Function

    Private Function FolderToNode(ByVal folder As Folder) As TreeNode
        Dim node As New TreeNode(folder.Name)
        node.Name = folder.Path
        node.Nodes.AddRange(Enumerable.ToArray(Of TreeNode)(Enumerable.Select(Of Folder, TreeNode)(folder.SubFolders, New Func(Of Folder, TreeNode)(AddressOf Me.FolderToNode))))
        node.Tag = folder
        Return node
    End Function

    Private Function FormatFileSize(ByVal bytes As Long) As String
        If (bytes < &H400) Then
            Return (bytes & " B")
        End If
        bytes = (bytes / &H400)
        If (bytes < &H400) Then
            Return (bytes & " KB")
        End If
        bytes = (bytes / &H400)
        Return (bytes & " MB")
    End Function

    Private Sub FrmMain_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub FrmMainOrLsvMails_SizeChanged(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.SizeChanged, lsvMessages.SizeChanged
        Me.AutoResizeMessageListViewColumn()
    End Sub

    Private Sub GetMails()
        Dim args As ServerCallCompletedEventArgs
        Try
            Me._messages = Enumerable.ToList(Of Message)(Enumerable.OrderByDescending(Of Message, Nullable(Of DateTime))(Me._selectedFolder.Search("ALL", -1, -1), Function(m As Message)
                                                                                                                                                                       Return m.Date
                                                                                                                                                                   End Function))
            args = New ServerCallCompletedEventArgs(True, Nothing, Nothing)
            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.GetMailsCompleted), New Object() {My.MyApplication.ImapClient, args})
        Catch ex As Exception
            args = New ServerCallCompletedEventArgs(False, ex, Nothing)
            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.GetMailsCompleted), New Object() {My.MyApplication.ImapClient, args})
        End Try
    End Sub

    Private Sub GetMailsCompleted(ByVal sender As Object, ByVal e As ServerCallCompletedEventArgs)
        If e.Result Then
            Me.lsvMessages.VirtualListSize = Me._messages.Count
            Me.lsvMessages.Invalidate()
            Dim count As Integer = Enumerable.Count(Of Message)(Me._messages, Function(m As Message)
                                                                                  Return Not m.Seen
                                                                              End Function)
            Me.trwFolders.SelectedNode.Text = (Me._selectedFolder.Name & If((count = 0), "", String.Format(" ({0})", count)))
            Me.AutoResizeMessageListViewColumn()
        Else
            Using frm As FrmError = New FrmError(e.Exception)
                frm.ShowDialog()
            End Using
        End If
        Me.pnlLoading.Hide()
        Me.pnlMessages.Show()
        Me.trwFolders.Enabled = Me.lsvMessages.Enabled = Me.pnlFavorites.Enabled = True
    End Sub






    Private Sub ImportMessage(ByVal folder As Folder, ByVal eml As String)
        Dim args As ServerCallCompletedEventArgs
        Try
            args = New ServerCallCompletedEventArgs(True, Nothing, Nothing) With { _
                .Arg = folder, _
                .Result = folder.AppendMessage(eml, Nothing, Nothing) _
            }

            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.ImportMessageCompleted), New Object() {My.MyApplication.ImapClient, args})
        Catch ex As Exception
            args = New ServerCallCompletedEventArgs(False, ex, Nothing)
            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.ImportMessageCompleted), New Object() {My.MyApplication.ImapClient, args})
        End Try
    End Sub

    Private Sub ImportMessageCompleted(ByVal sender As Object, ByVal e As ServerCallCompletedEventArgs)
        Me.trwFolders.Enabled = True
        If e.Result Then
            MessageBox.Show("Message imported!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        Else
            MessageBox.Show("Failed to import message", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End If
    End Sub

    Private Sub importMessageToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles importMessageToolStripMenuItem.Click
        Dim eml As String
        Dim folder As Folder = TryCast(Me._lastClickedNode.Tag, Folder)
        If ((Not folder Is Nothing) AndAlso (Me.ofdImportMessage.ShowDialog = DialogResult.OK)) Then
            eml = File.ReadAllText(Me.ofdImportMessage.FileName)
            Me.trwFolders.Enabled = False
            Dim thread As New Thread(Function(f As Object)
                                         Me.ImportMessage(folder, eml)
                                     End Function)
            thread.Start()

        End If
    End Sub

    Private Sub lnkDownloadEmbeddedResources_LinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs) Handles lnkDownloadEmbeddedResources.LinkClicked
        Me.lnkDownloadEmbeddedResources.Enabled = False
        Dim thread As New Thread(New ThreadStart(AddressOf Me.DownloadEmbeddedResources))
        thread.Start()
    End Sub

    Private Sub lnkFavorite_LinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs) Handles lnkTrash.LinkClicked, lnkSent.LinkClicked, lnkJunk.LinkClicked, lnkInbox.LinkClicked, lnkImportant.LinkClicked, lnkFlagged.LinkClicked, lnkDrafts.LinkClicked, lnkArchive.LinkClicked, lnkAll.LinkClicked
        If sender.Equals(Me.lnkAll) Then
            Me.SelectFolder(My.MyApplication.ImapClient.Folders.All)
        ElseIf sender.Equals(Me.lnkArchive) Then
            Me.SelectFolder(My.MyApplication.ImapClient.Folders.Archive)
        ElseIf sender.Equals(Me.lnkDrafts) Then
            Me.SelectFolder(My.MyApplication.ImapClient.Folders.Drafts)
        ElseIf sender.Equals(Me.lnkFlagged) Then
            Me.SelectFolder(My.MyApplication.ImapClient.Folders.Flagged)
        ElseIf sender.Equals(Me.lnkImportant) Then
            Me.SelectFolder(My.MyApplication.ImapClient.Folders.Important)
        ElseIf sender.Equals(Me.lnkInbox) Then
            Me.SelectFolder(My.MyApplication.ImapClient.Folders.Inbox)
        ElseIf sender.Equals(Me.lnkJunk) Then
            Me.SelectFolder(My.MyApplication.ImapClient.Folders.Junk)
        ElseIf sender.Equals(Me.lnkSent) Then
            Me.SelectFolder(My.MyApplication.ImapClient.Folders.Sent)
        ElseIf sender.Equals(Me.lnkTrash) Then
            Me.SelectFolder(My.MyApplication.ImapClient.Folders.Trash)
        End If
    End Sub

    Private Sub lsvMessages_RetrieveVirtualItem(ByVal sender As Object, ByVal e As RetrieveVirtualItemEventArgs) Handles lsvMessages.RetrieveVirtualItem
        If (e.ItemIndex < Me._messages.Count) Then
            Dim msg As Message = Me._messages.Item(e.ItemIndex)
            Dim color As Color = color.Black
            Select Case msg.Importance
                Case MessageImportance.High
                    color = color.Red
                    Exit Select
                Case MessageImportance.Medium
                    color = color.Orange
                    Exit Select
                Case MessageImportance.Low
                    color = color.Gray
                    Exit Select
            End Select
            If Me._messageItems.ContainsKey(msg.UId) Then
                e.Item = Me._messageItems.Item(msg.UId)
            Else
                Dim item As New ListViewItem(If(String.IsNullOrEmpty(msg.Subject), "( No subject )", msg.Subject))
                item.ForeColor = color
                item.ImageIndex = If(Enumerable.Any(Of Attachment)(msg.Attachments), 0, -1)

                item.Font = New Font(item.Font, If(msg.Seen, FontStyle.Regular, FontStyle.Bold))
                Me._messageItems.Add(msg.UId, item)
                e.Item = item
            End If
        End If
    End Sub

    Private Sub lsvMessages_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lsvMessages.SelectedIndexChanged
        If (Me.lsvMessages.SelectedIndices.Count <> 0) Then
            Me._selectedMessage = Me._messages.Item(Me.lsvMessages.SelectedIndices.Item(0))
            Me.lblDate.Text = If(Me._selectedMessage.Date.HasValue, Me._selectedMessage.Date.Value.ToString("ddd, dd MMM yyyy HH:mm:ss"), "Unknown date")
            Me.lblFrom.Text = ("From: " & If(((Me._selectedMessage.From Is Nothing) AndAlso (Me._selectedMessage.Sender Is Nothing)), "Unknown sender", If(Not Me._selectedMessage.From Is Nothing, Me._selectedMessage.From, Me._selectedMessage.Sender).ToString))
            Me.lblSubject.Text = If(String.IsNullOrEmpty(Me._selectedMessage.Subject), "( No subject )", Me._selectedMessage.Subject)
            Me.pnlInfo.Show()
            Me.lsvAttachments.Items.Clear()
            Me.pnlAttachments.Visible = Enumerable.Any(Of Attachment)(Me._selectedMessage.Attachments)
            If Enumerable.Any(Of Attachment)(Me._selectedMessage.Attachments) Then
                Dim msgTmpDir As String = Path.Combine(Path.Combine(Application.StartupPath, "tmp"), Me._selectedMessage.UId.ToString(CultureInfo.InvariantCulture))
                If Not Directory.Exists(msgTmpDir) Then
                    Directory.CreateDirectory(msgTmpDir)
                End If
                Dim files As New List(Of String)
                Dim file As Attachment
                For Each file In Me._selectedMessage.Attachments
                    Dim path As String = IO.Path.Combine(msgTmpDir, file.FileName)
                    If Not IO.File.Exists(path) Then
                        IO.File.Create(path)
                    End If
                    files.Add(path)
                Next
                Me.UpdateAttachmentIcons(files)
                Me.lsvAttachments.Items.AddRange(Enumerable.ToArray(Of ListViewItem)(Enumerable.Select(Of Attachment, ListViewItem)(Me._selectedMessage.Attachments, Function(f As Attachment)
                                                                                                                                                                         Return New ListViewItem(String.Format("{0} ({1})", f.FileName, If(f.Downloaded, Me.FormatFileSize(f.FileSize), "..."))) With { _
                                                                                                                                                                             .ImageKey = Enumerable.Last(Of String)(f.FileName.Split(New Char() {"."c})) _
                                                                                                                                                                         }
                                                                                                                                                                     End Function)))
            End If
            Me.pnlView.Hide()
            Me.lblFailedDownloadBody.Hide()
            Me.lblDownloadingBody.Show()
            Me.pnlDownloadingBody.Hide()
            If (Me._selectedMessage.Body.Downloaded = BodyType.None) Then
                Me.pnlDownloadingBody.Show()
                Me.trwFolders.Enabled = Me.lsvMessages.Enabled = False
                Dim thread As New Thread(New ThreadStart(AddressOf Me.DownloadBody))
                thread.Start()

            Else
                Me.DownloadBodyCompleted(My.MyApplication.ImapClient, New ServerCallCompletedEventArgs(True, Nothing, Nothing))
            End If
        End If
    End Sub

    Private Sub mnuAttachment_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles mnuAttachment.Opening
        e.Cancel = (Me.lsvAttachments.SelectedIndices.Count = 0)
        If Not e.Cancel Then
            Me.downloadToolStripMenuItem.Visible = Not Me._selectedMessage.Attachments(Me.lsvAttachments.SelectedIndices.Item(0)).Downloaded
            Me.openToolStripMenuItem.Visible = Me.saveAsToolStripMenuItem.Visible = Me._selectedMessage.Attachments(Me.lsvAttachments.SelectedIndices.Item(0)).Downloaded
        End If
    End Sub

    Private Sub mnuMessage_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles mnuMessage.Opening
        e.Cancel = (Me.lsvMessages.SelectedIndices.Count = 0)
        If Not e.Cancel Then
            Me.seenToolStripMenuItem.Checked = Me._selectedMessage.Seen
        End If
    End Sub

    Private Sub MoveMessageToFolder(ByVal message As Message, ByVal folder As Folder)
        Dim args As ServerCallCompletedEventArgs
        Try
            args = New ServerCallCompletedEventArgs(True, Nothing, Nothing)
            args.Result = message.MoveTo(folder)

            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.MoveMessageToFolderCompleted), New Object() {My.MyApplication.ImapClient, args})
        Catch ex As Exception
            args = New ServerCallCompletedEventArgs(False, ex, Nothing)
            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.MoveMessageToFolderCompleted), New Object() {My.MyApplication.ImapClient, args})
        End Try
    End Sub

    Private Sub MoveMessageToFolderCompleted(ByVal sender As Object, ByVal e As ServerCallCompletedEventArgs)
        Me.trwFolders.Enabled = Me.lsvMessages.Enabled = True
        If e.Result Then
            Me.lsvMessages.VirtualListSize -= 1
            Me._messages.Remove(Me._selectedMessage)
            Me.lsvMessages.SelectedIndices.Clear()
            MessageBox.Show("Message moved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        Else
            MessageBox.Show("Failed to move message", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End If
    End Sub

    Private Sub moveToToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles moveToToolStripMenuItem.Click
        Dim folder As Folder = FolderBox.Show("Move message to folder", "Please select target folder", Me)
        If (Not folder Is Nothing) Then
            Me.trwFolders.Enabled = Me.lsvMessages.Enabled = False
            Dim thread As New Thread(Function(f As Object)
                                         Me.MoveMessageToFolder(Me._selectedMessage, folder)
                                     End Function)
            thread.Start()
        End If
    End Sub

    Private Sub openToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles openToolStripMenuItem.Click
        Dim index As Integer = Me.lsvAttachments.SelectedItems.Item(0).Index
        Dim item As Attachment = Me._selectedMessage.Attachments(index)
        Dim path As String = IO.Path.Combine(IO.Path.Combine(IO.Path.Combine(Application.StartupPath, "tmp"), Me._selectedMessage.UId.ToString(CultureInfo.InvariantCulture)), item.FileName)
        Try
            If Not (File.Exists(path) AndAlso (New FileInfo(path).Length <> 0)) Then
                File.WriteAllBytes(path, item.FileData)
            End If
        Catch obj1 As Exception
            MessageBox.Show("Failed to write data, file might be in use", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End Try
        Process.Start(path)
    End Sub

    Private Sub RemoveMessage(ByVal message As Message)
        Dim args As ServerCallCompletedEventArgs
        Try
            args = New ServerCallCompletedEventArgs(True, Nothing, Nothing)
            args.Result = message.Remove

            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.RemoveMessageCompleted), New Object() {My.MyApplication.ImapClient, args})
        Catch ex As Exception
            args = New ServerCallCompletedEventArgs(False, ex, Nothing)
            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.RemoveMessageCompleted), New Object() {My.MyApplication.ImapClient, args})
        End Try
    End Sub

    Private Sub RemoveMessageCompleted(ByVal sender As Object, ByVal e As ServerCallCompletedEventArgs)
        Me.trwFolders.Enabled = Me.lsvMessages.Enabled = True
        If e.Result Then
            Me.lsvMessages.VirtualListSize -= 1
            Me._messages.Remove(Me._selectedMessage)
            Me.lsvMessages.SelectedIndices.Clear()
            MessageBox.Show("Message removed", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        Else
            MessageBox.Show("Failed to remove message", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End If
    End Sub

    Private Sub RenameFolder(ByVal folder As Folder, ByVal newName As String)
        Dim args As ServerCallCompletedEventArgs
        Try
            folder.Name = newName
            args = New ServerCallCompletedEventArgs(True, Nothing, Nothing)
            args.Arg = newName

            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.RenameFolderCompleted), New Object() {My.MyApplication.ImapClient, args})
        Catch ex As Exception
            args = New ServerCallCompletedEventArgs(False, ex, Nothing)
            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.RenameFolderCompleted), New Object() {My.MyApplication.ImapClient, args})
        End Try
    End Sub

    Private Sub RenameFolderCompleted(ByVal sender As Object, ByVal e As ServerCallCompletedEventArgs)
        Me.trwFolders.Enabled = True
        If e.Result Then
            Me._lastClickedNode.Text = If(TryCast(e.Arg, String) <> Nothing, TryCast(e.Arg, String), Me._lastClickedNode.Text)
            Dim folder As Folder = TryCast(Me._lastClickedNode.Tag, Folder)
            If ((Not folder Is Nothing) AndAlso (folder Is Me._selectedFolder)) Then
                Me.lblFolder.Text = Me._lastClickedNode.Text
                Dim count As Integer = Enumerable.Count(Of Message)(Me._messages, Function(m As Message)
                                                                                      Return Not m.Seen
                                                                                  End Function)
                Me.trwFolders.SelectedNode.Text = (Me._selectedFolder.Name & If((count = 0), "", String.Format(" ({0})", count)))
            End If
        Else
            MessageBox.Show("Failed to rename folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End If
    End Sub

    Private Sub renameToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles renameToolStripMenuItem.Click
        Dim newName As String
        Dim folder As Folder = TryCast(Me._lastClickedNode.Tag, Folder)
        If (Not folder Is Nothing) Then
            newName = InputBox.Show("Rename folder", "Enter a new folder name", folder.Name, Me)
            If (Not newName Is Nothing) Then
                If String.IsNullOrEmpty(newName.Trim) Then
                    MessageBox.Show("A valid folder name is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
                End If
                Me.trwFolders.Enabled = False
                Dim thread = New Thread(Function(f As Object)
                                            Me.RenameFolder(folder, newName)
                                        End Function)
                thread.Start()
            End If
        End If
    End Sub

    Private Sub saveAsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles saveAsToolStripMenuItem.Click
        If (Me.sfdSaveAttachment.ShowDialog = DialogResult.OK) Then
            Dim index As Integer = Me.lsvAttachments.SelectedItems.Item(0).Index
            Dim item As Attachment = Me._selectedMessage.Attachments(index)
            Dim path As String = IO.Path.Combine(IO.Path.GetTempPath, item.FileName)
            If Not File.Exists(path) Then
                File.WriteAllBytes(Me.sfdSaveAttachment.FileName, item.FileData)
            Else
                File.Copy(path, Me.sfdSaveAttachment.FileName)
            End If
            MessageBox.Show("File saved!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        End If
    End Sub

    Private Sub seenToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles seenToolStripMenuItem.Click
        Me.trwFolders.Enabled = Me.lsvMessages.Enabled = False
        Dim thread As New Thread(Function(f As Object)
                                     Me.ToggleSeen(Me._selectedMessage)
                                 End Function)
        thread.Start()
    End Sub

    Private Sub SelectFolder(ByVal folder As Folder)
        Me._selectedFolder = folder
        Me.SetFavoriteSelection(folder)
        Dim node As TreeNode = Enumerable.FirstOrDefault(Of TreeNode)(Me.trwFolders.Nodes.Find(folder.Path, True))
        If (Not node Is Nothing) Then
            Me.trwFolders.SelectedNode = node
        End If
        Me.pnlSelectFolder.Hide()
        Me.pnlInfo.Visible = Me.pnlView.Visible = Me.pnlDownloadingBody.Visible = Me.trwFolders.Enabled = Me.lsvMessages.Enabled = Me.pnlFavorites.Enabled = False
        Me.lsvMessages.VirtualListSize = 0
        Me.pnlLoading.Show()
        Me.pnlMessages.Hide()
        Me._selectedMessage = Nothing
        Me._messageItems.Clear()
        Me.lblFolder.Text = Me._selectedFolder.Name
        Dim thread As New Thread(New ThreadStart(AddressOf Me.GetMails))
        thread.Start()
    End Sub

    Private Sub SetFavoriteSelection(ByVal folder As Folder)
        If (Not folder Is Nothing) Then
            Dim lnk As LinkLabel = Me.FindFavoriteLnk(folder)
            Dim font = New Font(Me.lnkTrash.Font, FontStyle.Regular)
            Me.lnkAll.Font = font
            Me.lnkArchive.Font = font
            Me.lnkDrafts.Font = font
            Me.lnkFlagged.Font = font
            Me.lnkImportant.Font = font
            Me.lnkInbox.Font = font
            Me.lnkJunk.Font = font
            Me.lnkSent.Font = font
            Me.lnkTrash.Font = font
            If (Not lnk Is Nothing) Then
                lnk.Font = New Font(lnk.Font, FontStyle.Bold)
            End If
        End If
    End Sub

    Private Sub ToggleSeen(ByVal message As Message)
        Dim args As ServerCallCompletedEventArgs
        Try
            message.Seen = Not message.Seen
            args = New ServerCallCompletedEventArgs(True, Nothing, Nothing)
            args.Arg = message

            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.ToggleSeenCompleted), New Object() {My.MyApplication.ImapClient, args})
        Catch ex As Exception
            args = New ServerCallCompletedEventArgs(False, ex, Nothing)
            MyBase.Invoke(New EventHandler(Of ServerCallCompletedEventArgs)(AddressOf Me.ToggleSeenCompleted), New Object() {My.MyApplication.ImapClient, args})
        End Try
    End Sub

    Private Sub ToggleSeenCompleted(ByVal sender As Object, ByVal e As ServerCallCompletedEventArgs)
        Me.trwFolders.Enabled = Me.lsvMessages.Enabled = True
        If e.Result Then
            Dim msg As Message = TryCast(e.Arg, Message)
            If (Not msg Is Nothing) Then
                Me.seenToolStripMenuItem.Checked = msg.Seen
                Me._messageItems.Item(msg.UId).Font = New Font(Me._messageItems.Item(msg.UId).Font, If(msg.Seen, FontStyle.Regular, FontStyle.Bold))
                Dim count As Integer = Enumerable.Count(Of Message)(Me._messages, Function(m As Message)
                                                                                      Return Not m.Seen
                                                                                  End Function)
                Me.trwFolders.SelectedNode.Text = (Me._selectedFolder.Name & If((count = 0), "", String.Format(" ({0})", count)))
            End If
        Else
            MessageBox.Show("Failed to toggle \SEEN flag", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End If
    End Sub

    Private Sub trwFolders_AfterSelect(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles trwFolders.AfterSelect
        If ((Not Me.trwFolders.SelectedNode Is Nothing) AndAlso (Not Me.trwFolders.SelectedNode Is Me.trwFolders.Nodes.Item(0))) Then
            Dim folder As Folder = TryCast(Me.trwFolders.SelectedNode.Tag, Folder)
            Me.trwFolders.SelectedNode.NodeFont = New Font(Me.trwFolders.Font, FontStyle.Bold)
            If (Not Me._selectedFolder Is folder) Then
                Me.SelectFolder(folder)
            End If
        End If
    End Sub

    Private Sub trwFolders_BeforeSelect(ByVal sender As Object, ByVal e As TreeViewCancelEventArgs) Handles trwFolders.BeforeSelect
        Dim folder As Folder = TryCast(e.Node.Tag, Folder)
        If Not ((Not folder Is Nothing) AndAlso folder.Selectable) Then
            e.Cancel = True
        End If
        If (Not Me.trwFolders.SelectedNode Is Nothing) Then
            Me.trwFolders.SelectedNode.NodeFont = New Font(Me.trwFolders.Font, FontStyle.Regular)
        End If
    End Sub

    Private Sub trwFolders_NodeMouseClick(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs) Handles trwFolders.NodeMouseClick
        Me._lastClickedNode = e.Node
        If (TypeOf e.Node.Tag Is Folder AndAlso (e.Button = MouseButtons.Right)) Then
            Me.mnuFolder.Show(Me.trwFolders, e.Location)
        End If
    End Sub

    Private Sub UpdateAttachmentIcons(ByVal files As IEnumerable(Of String))
        Dim image As Image
        For Each image In Me.istAttachments.Images
            image.Dispose()
        Next
        Me.istAttachments.Images.Clear()
        Dim file As String
        For Each file In files
            Dim key As String = Enumerable.Last(Of String)(file.Split(New Char() {"."c}))
            If Not Me.istAttachments.Images.ContainsKey(key) Then
                Try
                    Me.istAttachments.Images.Add(Enumerable.Last(Of String)(file.Split(New Char() {"."c})), NativeMethods.GetSystemIcon(file))
                Catch obj1 As Exception
                End Try
            End If
        Next
    End Sub




End Class
