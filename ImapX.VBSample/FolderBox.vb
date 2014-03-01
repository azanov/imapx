Public Class FolderBox

    Public Sub New(ByVal title As String, ByVal [text] As String)
        Me.InitializeComponent()
        Me.Text = title
        Me.lblTitle.Text = title
        Me.lblText.Text = [text]
        Me.trwFolders.Nodes.Add(My.MyApplication.ImapClient.Host)
        Me.trwFolders.Nodes.Item(0).Nodes.AddRange(Enumerable.ToArray(Of TreeNode)(Enumerable.Select(Of Folder, TreeNode)(My.MyApplication.ImapClient.Folders, New Func(Of Folder, TreeNode)(AddressOf Me.FolderToNode))))
        Me.trwFolders.Nodes.Item(0).Expand()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs)
        MyBase.DialogResult = DialogResult.Cancel
        MyBase.Close()
    End Sub

    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As EventArgs)
        MyBase.DialogResult = DialogResult.OK
        MyBase.Close()
    End Sub

    Private Function FolderToNode(ByVal folder As Folder) As TreeNode
        Dim node As New TreeNode(folder.Name)
        node.Nodes.AddRange(Enumerable.ToArray(Of TreeNode)(Enumerable.Select(Of Folder, TreeNode)(folder.SubFolders, New Func(Of Folder, TreeNode)(AddressOf Me.FolderToNode))))
        node.Tag = folder
        Return node
    End Function

    Public Overloads Shared Function Show(ByVal title As String, ByVal [text] As String, Optional ByVal owner As IWin32Window = Nothing) As Folder
        Using dlg As FolderBox = New FolderBox(title, [text])
            If (dlg.ShowDialog(owner) = DialogResult.OK) Then
                Return dlg.SelectedFolder
            End If
        End Using
        Return Nothing
    End Function

    Private Sub trwFolders_AfterSelect(ByVal sender As Object, ByVal e As TreeViewEventArgs)
        Me.btnOK.Enabled = ((Not Me.trwFolders.SelectedNode Is Nothing) AndAlso (Not Me.trwFolders.SelectedNode Is Me.trwFolders.Nodes.Item(0)))
    End Sub

    Private Sub trwFolders_BeforeSelect(ByVal sender As Object, ByVal e As TreeViewCancelEventArgs)
        Dim folder As Folder = TryCast(e.Node.Tag, Folder)
        If Not ((Not folder Is Nothing) AndAlso folder.Selectable) Then
            e.Cancel = True
        End If
    End Sub

    Private Sub trwFolders_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs)
        If Me.btnOK.Enabled Then
            Me.btnOK_Click(Nothing, Nothing)
        End If
    End Sub


    ' Properties
    Public ReadOnly Property SelectedFolder As Folder
        Get
            Return TryCast(Me.trwFolders.SelectedNode.Tag, Folder)
        End Get
    End Property


End Class