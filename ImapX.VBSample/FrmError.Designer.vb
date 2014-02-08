Imports System.ComponentModel

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmError
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
        Dim resources As New ComponentResourceManager(GetType(FrmError))
        Me.label3 = New Label
        Me.lblMessage = New Label
        Me.txtStacktrace = New TextBox
        Me.label2 = New Label
        MyBase.SuspendLayout()
        Me.label3.Dock = DockStyle.Top
        Me.label3.Font = New Font("Microsoft Sans Serif", 12.0!, FontStyle.Regular, GraphicsUnit.Point, 0)
        Me.label3.Location = New Point(12, 12)
        Me.label3.Margin = New Padding(3, 0, 3, 10)
        Me.label3.Name = "label3"
        Me.label3.Size = New Size(&H28D, &H19)
        Me.label3.TabIndex = 6
        Me.label3.Text = "An unhandled exception has occured"
        Me.lblMessage.Dock = DockStyle.Top
        Me.lblMessage.ForeColor = Color.Red
        Me.lblMessage.Location = New Point(12, &H25)
        Me.lblMessage.Margin = New Padding(3, 10, 3, 0)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New Size(&H28D, &H19)
        Me.lblMessage.TabIndex = 7
        Me.lblMessage.Text = "[message]"
        Me.txtStacktrace.Dock = DockStyle.Top
        Me.txtStacktrace.Location = New Point(12, &H57)
        Me.txtStacktrace.Multiline = True
        Me.txtStacktrace.Name = "txtStacktrace"
        Me.txtStacktrace.ScrollBars = ScrollBars.Vertical
        Me.txtStacktrace.Size = New Size(&H28D, &H178)
        Me.txtStacktrace.TabIndex = 8
        Me.label2.Dock = DockStyle.Top
        Me.label2.Location = New Point(12, &H3E)
        Me.label2.Name = "label2"
        Me.label2.Size = New Size(&H28D, &H19)
        Me.label2.TabIndex = 9
        Me.label2.Text = "Details:"
        MyBase.AutoScaleDimensions = New SizeF(8.0!, 16.0!)
        MyBase.AutoScaleMode = AutoScaleMode.Font
        Me.BackColor = Color.White
        MyBase.ClientSize = New Size(&H2A5, &H1DE)
        MyBase.Controls.Add(Me.txtStacktrace)
        MyBase.Controls.Add(Me.label2)
        MyBase.Controls.Add(Me.lblMessage)
        MyBase.Controls.Add(Me.label3)
        Me.Font = New Font("Microsoft Sans Serif", 10.0!, FontStyle.Regular, GraphicsUnit.Point, 0)
        MyBase.Icon = DirectCast(resources.GetObject("$this.Icon"), Icon)
        MyBase.Margin = New Padding(4)
        MyBase.Name = "FrmError"
        MyBase.Padding = New Padding(12)
        MyBase.StartPosition = FormStartPosition.CenterScreen
        Me.Text = "Unhandled exception has occured"
        MyBase.ResumeLayout(False)
        MyBase.PerformLayout()
    End Sub

    Private label2 As Label
    Private label3 As Label
    Private lblMessage As Label
    Private txtStacktrace As TextBox


End Class
