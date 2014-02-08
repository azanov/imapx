Public Class FrmError

    Sub New(ex As Exception)
        Me.components = Nothing
        Me.InitializeComponent()
        Me.lblMessage.Text = ex.Message
        Me.txtStacktrace.Text = ex.ToString

    End Sub


End Class