Imports System.ComponentModel

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmConnect
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
        Me.pnlTop = New System.Windows.Forms.Panel()
        Me.btnDefaultAuth = New System.Windows.Forms.Button()
        Me.picGMailLogin = New System.Windows.Forms.PictureBox()
        Me.picLogo = New System.Windows.Forms.PictureBox()
        Me.pnlLogin = New System.Windows.Forms.Panel()
        Me.chkValidateCertificate = New System.Windows.Forms.CheckBox()
        Me.btnSignIn = New System.Windows.Forms.Button()
        Me.lblPassword = New System.Windows.Forms.Label()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.cmbEncryption = New System.Windows.Forms.ComboBox()
        Me.lblPort = New System.Windows.Forms.Label()
        Me.cmbPort = New System.Windows.Forms.ComboBox()
        Me.lblLogin = New System.Windows.Forms.Label()
        Me.txtLogin = New System.Windows.Forms.TextBox()
        Me.lblServer = New System.Windows.Forms.Label()
        Me.txtServer = New System.Windows.Forms.TextBox()
        Me.lblWait = New System.Windows.Forms.Label()
        Me.wbrMain = New System.Windows.Forms.WebBrowser()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.tltMain = New System.Windows.Forms.ToolTip(Me.components)
        Me.pnlTop.SuspendLayout()
        CType(Me.picGMailLogin, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlLogin.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlTop
        '
        Me.pnlTop.BackColor = System.Drawing.Color.White
        Me.pnlTop.Controls.Add(Me.btnDefaultAuth)
        Me.pnlTop.Controls.Add(Me.picGMailLogin)
        Me.pnlTop.Controls.Add(Me.picLogo)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Padding = New System.Windows.Forms.Padding(24, 12, 12, 12)
        Me.pnlTop.Size = New System.Drawing.Size(544, 85)
        Me.pnlTop.TabIndex = 1
        '
        'btnDefaultAuth
        '
        Me.btnDefaultAuth.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.btnDefaultAuth.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDefaultAuth.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDefaultAuth.ForeColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.btnDefaultAuth.Location = New System.Drawing.Point(337, 22)
        Me.btnDefaultAuth.Name = "btnDefaultAuth"
        Me.btnDefaultAuth.Size = New System.Drawing.Size(182, 40)
        Me.btnDefaultAuth.TabIndex = 2
        Me.btnDefaultAuth.Text = "Default authentication"
        Me.btnDefaultAuth.UseVisualStyleBackColor = False
        Me.btnDefaultAuth.Visible = False
        '
        'picGMailLogin
        '
        Me.picGMailLogin.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picGMailLogin.Image = Global.ImapX.VBSample.My.Resources.Resources.sign_in_with_google
        Me.picGMailLogin.Location = New System.Drawing.Point(337, 22)
        Me.picGMailLogin.Name = "picGMailLogin"
        Me.picGMailLogin.Size = New System.Drawing.Size(182, 40)
        Me.picGMailLogin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.picGMailLogin.TabIndex = 1
        Me.picGMailLogin.TabStop = False
        '
        'picLogo
        '
        Me.picLogo.Cursor = System.Windows.Forms.Cursors.Hand
        Me.picLogo.Dock = System.Windows.Forms.DockStyle.Left
        Me.picLogo.Image = Global.ImapX.VBSample.My.Resources.Resources.logo
        Me.picLogo.Location = New System.Drawing.Point(24, 12)
        Me.picLogo.Name = "picLogo"
        Me.picLogo.Size = New System.Drawing.Size(180, 61)
        Me.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.picLogo.TabIndex = 0
        Me.picLogo.TabStop = False
        '
        'pnlLogin
        '
        Me.pnlLogin.BackColor = System.Drawing.Color.Transparent
        Me.pnlLogin.Controls.Add(Me.chkValidateCertificate)
        Me.pnlLogin.Controls.Add(Me.btnSignIn)
        Me.pnlLogin.Controls.Add(Me.lblPassword)
        Me.pnlLogin.Controls.Add(Me.txtPassword)
        Me.pnlLogin.Controls.Add(Me.cmbEncryption)
        Me.pnlLogin.Controls.Add(Me.lblPort)
        Me.pnlLogin.Controls.Add(Me.cmbPort)
        Me.pnlLogin.Controls.Add(Me.lblLogin)
        Me.pnlLogin.Controls.Add(Me.txtLogin)
        Me.pnlLogin.Controls.Add(Me.lblServer)
        Me.pnlLogin.Controls.Add(Me.txtServer)
        Me.pnlLogin.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlLogin.Location = New System.Drawing.Point(0, 85)
        Me.pnlLogin.Name = "pnlLogin"
        Me.pnlLogin.Size = New System.Drawing.Size(544, 356)
        Me.pnlLogin.TabIndex = 2
        '
        'chkValidateCertificate
        '
        Me.chkValidateCertificate.AutoSize = True
        Me.chkValidateCertificate.Checked = True
        Me.chkValidateCertificate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkValidateCertificate.Location = New System.Drawing.Point(403, 126)
        Me.chkValidateCertificate.Name = "chkValidateCertificate"
        Me.chkValidateCertificate.Size = New System.Drawing.Size(15, 14)
        Me.chkValidateCertificate.TabIndex = 3
        Me.tltMain.SetToolTip(Me.chkValidateCertificate, "Validate server certificate")
        Me.chkValidateCertificate.UseVisualStyleBackColor = True
        '
        'btnSignIn
        '
        Me.btnSignIn.Location = New System.Drawing.Point(208, 213)
        Me.btnSignIn.Name = "btnSignIn"
        Me.btnSignIn.Size = New System.Drawing.Size(210, 30)
        Me.btnSignIn.TabIndex = 6
        Me.btnSignIn.Text = "Sign in"
        Me.btnSignIn.UseVisualStyleBackColor = True
        '
        'lblPassword
        '
        Me.lblPassword.AutoSize = True
        Me.lblPassword.Location = New System.Drawing.Point(114, 185)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.Size = New System.Drawing.Size(64, 17)
        Me.lblPassword.TabIndex = 12
        Me.lblPassword.Text = "Password"
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(208, 182)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(9679)
        Me.txtPassword.Size = New System.Drawing.Size(210, 25)
        Me.txtPassword.TabIndex = 5
        '
        'cmbEncryption
        '
        Me.cmbEncryption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbEncryption.FormattingEnabled = True
        Me.cmbEncryption.Items.AddRange(New Object() {"None", "SSL", "TLS"})
        Me.cmbEncryption.Location = New System.Drawing.Point(316, 120)
        Me.cmbEncryption.Name = "cmbEncryption"
        Me.cmbEncryption.Size = New System.Drawing.Size(79, 25)
        Me.cmbEncryption.TabIndex = 2
        '
        'lblPort
        '
        Me.lblPort.AutoSize = True
        Me.lblPort.Location = New System.Drawing.Point(114, 123)
        Me.lblPort.Name = "lblPort"
        Me.lblPort.Size = New System.Drawing.Size(32, 17)
        Me.lblPort.TabIndex = 9
        Me.lblPort.Text = "Port"
        '
        'cmbPort
        '
        Me.cmbPort.FormattingEnabled = True
        Me.cmbPort.Items.AddRange(New Object() {"143", "993"})
        Me.cmbPort.Location = New System.Drawing.Point(208, 120)
        Me.cmbPort.Name = "cmbPort"
        Me.cmbPort.Size = New System.Drawing.Size(102, 25)
        Me.cmbPort.TabIndex = 1
        '
        'lblLogin
        '
        Me.lblLogin.AutoSize = True
        Me.lblLogin.Location = New System.Drawing.Point(114, 154)
        Me.lblLogin.Name = "lblLogin"
        Me.lblLogin.Size = New System.Drawing.Size(40, 17)
        Me.lblLogin.TabIndex = 7
        Me.lblLogin.Text = "Login"
        '
        'txtLogin
        '
        Me.txtLogin.Location = New System.Drawing.Point(208, 151)
        Me.txtLogin.Name = "txtLogin"
        Me.txtLogin.Size = New System.Drawing.Size(210, 25)
        Me.txtLogin.TabIndex = 4
        '
        'lblServer
        '
        Me.lblServer.AutoSize = True
        Me.lblServer.Location = New System.Drawing.Point(114, 92)
        Me.lblServer.Name = "lblServer"
        Me.lblServer.Size = New System.Drawing.Size(45, 17)
        Me.lblServer.TabIndex = 3
        Me.lblServer.Text = "Server"
        '
        'txtServer
        '
        Me.txtServer.Location = New System.Drawing.Point(208, 89)
        Me.txtServer.Name = "txtServer"
        Me.txtServer.Size = New System.Drawing.Size(210, 25)
        Me.txtServer.TabIndex = 0
        Me.txtServer.Text = "imap.gmail.com"
        '
        'lblWait
        '
        Me.lblWait.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.lblWait.ForeColor = System.Drawing.Color.White
        Me.lblWait.Location = New System.Drawing.Point(169, 255)
        Me.lblWait.Name = "lblWait"
        Me.lblWait.Padding = New System.Windows.Forms.Padding(15, 10, 15, 10)
        Me.lblWait.Size = New System.Drawing.Size(212, 37)
        Me.lblWait.TabIndex = 15
        Me.lblWait.Text = "Connecting..."
        Me.lblWait.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lblWait.Visible = False
        '
        'wbrMain
        '
        Me.wbrMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.wbrMain.Location = New System.Drawing.Point(0, 0)
        Me.wbrMain.MinimumSize = New System.Drawing.Size(20, 20)
        Me.wbrMain.Name = "wbrMain"
        Me.wbrMain.Size = New System.Drawing.Size(544, 441)
        Me.wbrMain.TabIndex = 14
        Me.wbrMain.Visible = False
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.Color.WhiteSmoke
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 85)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Padding = New System.Windows.Forms.Padding(12)
        Me.lblTitle.Size = New System.Drawing.Size(544, 49)
        Me.lblTitle.TabIndex = 3
        Me.lblTitle.Text = "Sign in"
        '
        'FrmConnect
        '
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(544, 441)
        Me.Controls.Add(Me.lblWait)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.pnlLogin)
        Me.Controls.Add(Me.pnlTop)
        Me.Controls.Add(Me.wbrMain)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.Name = "FrmConnect"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ImapX"
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        CType(Me.picGMailLogin, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlLogin.ResumeLayout(False)
        Me.pnlLogin.PerformLayout()
        Me.ResumeLayout(False)

    End Sub






    Private WithEvents btnDefaultAuth As Button
    Private WithEvents btnSignIn As Button
    Private WithEvents chkValidateCertificate As CheckBox
    Private WithEvents cmbEncryption As ComboBox
    Private WithEvents cmbPort As ComboBox
    Private lblLogin As Label
    Private lblPassword As Label
    Private lblPort As Label
    Private lblServer As Label
    Private lblTitle As Label
    Private lblWait As Label
    Private WithEvents picGMailLogin As PictureBox
    Private WithEvents picLogo As PictureBox
    Private pnlLogin As Panel
    Private pnlTop As Panel
    Private WithEvents tltMain As ToolTip
    Private WithEvents txtLogin As TextBox
    Private WithEvents txtPassword As TextBox
    Private WithEvents txtServer As TextBox
    Private WithEvents wbrMain As WebBrowser

End Class
