<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InputBox
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
        Me.lblTitle = New Label
        Me.lblText = New Label
        Me.txtValue = New TextBox
        Me.panel1 = New Panel
        Me.btnOK = New Button
        Me.panel3 = New Panel
        Me.btnCancel = New Button
        Me.panel2 = New Panel
        Me.panel1.SuspendLayout()
        Me.panel2.SuspendLayout()
        MyBase.SuspendLayout()
        Me.lblTitle.BackColor = Color.WhiteSmoke
        Me.lblTitle.Dock = DockStyle.Top
        Me.lblTitle.Font = New Font("Segoe UI", 14.25!, FontStyle.Regular, GraphicsUnit.Point, 0)
        Me.lblTitle.Location = New Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Padding = New Padding(12, 12, 12, 5)
        Me.lblTitle.Size = New Size(&H181, &H2A)
        Me.lblTitle.TabIndex = 4
        Me.lblTitle.Text = "Title"
        Me.lblText.BackColor = Color.WhiteSmoke
        Me.lblText.Dock = DockStyle.Top
        Me.lblText.Font = New Font("Segoe UI", 9.75!, FontStyle.Regular, GraphicsUnit.Point, 0)
        Me.lblText.Location = New Point(0, &H2A)
        Me.lblText.Name = "lblText"
        Me.lblText.Padding = New Padding(13, 0, 0, 12)
        Me.lblText.Size = New Size(&H181, &H1D)
        Me.lblText.TabIndex = 5
        Me.lblText.Text = "#"
        Me.txtValue.Dock = DockStyle.Fill
        Me.txtValue.Location = New Point(12, 12)
        Me.txtValue.Name = "txtValue"
        Me.txtValue.Size = New Size(&H169, &H19)
        Me.txtValue.TabIndex = 0
        Me.panel1.Controls.Add(Me.btnOK)
        Me.panel1.Controls.Add(Me.panel3)
        Me.panel1.Controls.Add(Me.btnCancel)
        Me.panel1.Dock = DockStyle.Bottom
        Me.panel1.Location = New Point(0, &H7B)
        Me.panel1.Name = "panel1"
        Me.panel1.Padding = New Padding(12, 0, 12, 12)
        Me.panel1.Size = New Size(&H181, &H2A)
        Me.panel1.TabIndex = 7
        Me.btnOK.Dock = DockStyle.Right
        Me.btnOK.Location = New Point(&HD3, 0)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New Size(&H4B, 30)
        Me.btnOK.TabIndex = 1
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        AddHandler Me.btnOK.Click, New EventHandler(AddressOf Me.btnOK_Click)
        Me.panel3.Dock = DockStyle.Right
        Me.panel3.Location = New Point(&H11E, 0)
        Me.panel3.Name = "panel3"
        Me.panel3.Size = New Size(12, 30)
        Me.panel3.TabIndex = 1
        Me.btnCancel.DialogResult = DialogResult.Cancel
        Me.btnCancel.Dock = DockStyle.Right
        Me.btnCancel.Location = New Point(&H12A, 0)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New Size(&H4B, 30)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        AddHandler Me.btnCancel.Click, New EventHandler(AddressOf Me.btnCancel_Click)
        Me.panel2.Controls.Add(Me.txtValue)
        Me.panel2.Dock = DockStyle.Fill
        Me.panel2.Location = New Point(0, &H47)
        Me.panel2.Name = "panel2"
        Me.panel2.Padding = New Padding(12)
        Me.panel2.Size = New Size(&H181, &H34)
        Me.panel2.TabIndex = 8
        MyBase.AcceptButton = Me.btnOK
        MyBase.AutoScaleDimensions = New SizeF(7.0!, 17.0!)
        MyBase.AutoScaleMode = AutoScaleMode.Font
        Me.BackColor = Color.White
        MyBase.CancelButton = Me.btnCancel
        MyBase.ClientSize = New Size(&H181, &HA5)
        MyBase.Controls.Add(Me.panel2)
        MyBase.Controls.Add(Me.panel1)
        MyBase.Controls.Add(Me.lblText)
        MyBase.Controls.Add(Me.lblTitle)
        Me.Font = New Font("Segoe UI", 9.75!, FontStyle.Regular, GraphicsUnit.Point, 0)
        MyBase.FormBorderStyle = FormBorderStyle.FixedToolWindow
        MyBase.Margin = New Padding(3, 4, 3, 4)
        MyBase.Name = "InputBox"
        MyBase.ShowInTaskbar = False
        MyBase.StartPosition = FormStartPosition.CenterScreen
        AddHandler MyBase.Load, New EventHandler(AddressOf Me.InputBox_Load)
        Me.panel1.ResumeLayout(False)
        Me.panel2.ResumeLayout(False)
        Me.panel2.PerformLayout()
        MyBase.ResumeLayout(False)
    End Sub



    Private btnCancel As Button
    Private btnOK As Button
    Private lblText As Label
    Private lblTitle As Label
    Private panel1 As Panel
    Private panel2 As Panel
    Private panel3 As Panel
    Private txtValue As TextBox


End Class
