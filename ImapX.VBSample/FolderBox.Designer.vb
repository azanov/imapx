<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FolderBox
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
        Me.panel1 = New Panel
        Me.btnOK = New Button
        Me.panel3 = New Panel
        Me.btnCancel = New Button
        Me.lblText = New Label
        Me.lblTitle = New Label
        Me.panel2 = New Panel
        Me.trwFolders = New TreeView
        Me.panel1.SuspendLayout()
        Me.panel2.SuspendLayout()
        MyBase.SuspendLayout()
        Me.panel1.Controls.Add(Me.btnOK)
        Me.panel1.Controls.Add(Me.panel3)
        Me.panel1.Controls.Add(Me.btnCancel)
        Me.panel1.Dock = DockStyle.Bottom
        Me.panel1.Location = New Point(0, &H162)
        Me.panel1.Name = "panel1"
        Me.panel1.Padding = New Padding(10, 0, 10, 13)
        Me.panel1.Size = New Size(&H14C, &H2D)
        Me.panel1.TabIndex = 11
        Me.btnOK.Dock = DockStyle.Right
        Me.btnOK.Enabled = False
        Me.btnOK.Location = New Point(180, 0)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New Size(&H42, &H20)
        Me.btnOK.TabIndex = 1
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        AddHandler Me.btnOK.Click, New EventHandler(AddressOf Me.btnOK_Click)
        Me.panel3.Dock = DockStyle.Right
        Me.panel3.Location = New Point(&HF6, 0)
        Me.panel3.Name = "panel3"
        Me.panel3.Size = New Size(10, &H20)
        Me.panel3.TabIndex = 1
        Me.btnCancel.DialogResult = DialogResult.Cancel
        Me.btnCancel.Dock = DockStyle.Right
        Me.btnCancel.Location = New Point(&H100, 0)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New Size(&H42, &H20)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        AddHandler Me.btnCancel.Click, New EventHandler(AddressOf Me.btnCancel_Click)
        Me.lblText.BackColor = Color.WhiteSmoke
        Me.lblText.Dock = DockStyle.Top
        Me.lblText.Font = New Font("Segoe UI", 9.75!, FontStyle.Regular, GraphicsUnit.Point, 0)
        Me.lblText.Location = New Point(0, &H2D)
        Me.lblText.Name = "lblText"
        Me.lblText.Padding = New Padding(11, 0, 0, 13)
        Me.lblText.Size = New Size(&H14C, &H1F)
        Me.lblText.TabIndex = 10
        Me.lblText.Text = "#"
        Me.lblTitle.BackColor = Color.WhiteSmoke
        Me.lblTitle.Dock = DockStyle.Top
        Me.lblTitle.Font = New Font("Segoe UI", 14.25!, FontStyle.Regular, GraphicsUnit.Point, 0)
        Me.lblTitle.Location = New Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Padding = New Padding(10, 13, 10, 5)
        Me.lblTitle.Size = New Size(&H14C, &H2D)
        Me.lblTitle.TabIndex = 9
        Me.lblTitle.Text = "Title"
        Me.panel2.Controls.Add(Me.trwFolders)
        Me.panel2.Dock = DockStyle.Fill
        Me.panel2.Location = New Point(0, &H4C)
        Me.panel2.Name = "panel2"
        Me.panel2.Padding = New Padding(10, 13, 10, 13)
        Me.panel2.Size = New Size(&H14C, &H116)
        Me.panel2.TabIndex = 12
        Me.trwFolders.Dock = DockStyle.Fill
        Me.trwFolders.FullRowSelect = True
        Me.trwFolders.HideSelection = False
        Me.trwFolders.Location = New Point(10, 13)
        Me.trwFolders.Name = "trwFolders"
        Me.trwFolders.Size = New Size(&H138, &HFC)
        Me.trwFolders.TabIndex = 0
        AddHandler Me.trwFolders.BeforeSelect, New TreeViewCancelEventHandler(AddressOf Me.trwFolders_BeforeSelect)
        AddHandler Me.trwFolders.AfterSelect, New TreeViewEventHandler(AddressOf Me.trwFolders_AfterSelect)
        AddHandler Me.trwFolders.NodeMouseDoubleClick, New TreeNodeMouseClickEventHandler(AddressOf Me.trwFolders_NodeMouseDoubleClick)
        MyBase.AcceptButton = Me.btnOK
        MyBase.AutoScaleDimensions = New SizeF(7.0!, 17.0!)
        MyBase.AutoScaleMode = AutoScaleMode.Font
        Me.BackColor = Color.White
        MyBase.CancelButton = Me.btnCancel
        MyBase.ClientSize = New Size(&H14C, &H18F)
        MyBase.Controls.Add(Me.panel2)
        MyBase.Controls.Add(Me.panel1)
        MyBase.Controls.Add(Me.lblText)
        MyBase.Controls.Add(Me.lblTitle)
        Me.Font = New Font("Segoe UI", 9.75!, FontStyle.Regular, GraphicsUnit.Point, 0)
        MyBase.FormBorderStyle = FormBorderStyle.FixedToolWindow
        MyBase.Margin = New Padding(4)
        MyBase.Name = "FolderBox"
        MyBase.ShowInTaskbar = False
        MyBase.StartPosition = FormStartPosition.CenterParent
        Me.panel1.ResumeLayout(False)
        Me.panel2.ResumeLayout(False)
        MyBase.ResumeLayout(False)
    End Sub


    Private btnCancel As Button
    Private btnOK As Button
    Private lblText As Label
    Private lblTitle As Label
    Private panel1 As Panel
    Private panel2 As Panel
    Private panel3 As Panel
    Private trwFolders As TreeView


End Class
