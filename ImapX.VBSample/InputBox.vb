Public Class InputBox
    Public Sub New(ByVal title As String, ByVal [text] As String, Optional ByVal value As String = "")
        Me.InitializeComponent()
        Me.Text = title
        Me.lblTitle.Text = title
        Me.lblText.Text = [text]
        Me.txtValue.Text = value
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs)
        MyBase.DialogResult = DialogResult.Cancel
        MyBase.Close()
    End Sub

    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As EventArgs)
        MyBase.DialogResult = DialogResult.OK
        MyBase.Close()
    End Sub

    Private Sub InputBox_Load(ByVal sender As Object, ByVal e As EventArgs)
        MyBase.Activate()
        Me.txtValue.Focus()
    End Sub

    Public Overloads Shared Function Show(ByVal title As String, ByVal [text] As String, Optional ByVal value As String = "", Optional ByVal owner As IWin32Window = Nothing) As String
        Using dlg As InputBox = New InputBox(title, [text], value)
            If (dlg.ShowDialog(owner) = DialogResult.OK) Then
                Return dlg.Value
            End If
        End Using
        Return Nothing
    End Function


    ' Properties
    Public ReadOnly Property Value As String
        Get
            Return Me.txtValue.Text
        End Get
    End Property


End Class