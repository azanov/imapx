Imports System.Threading
Imports System.Security.Authentication
Imports ImapX.Authentication

Public Class FrmConnect

    Private _googleOAuth2Key As String

    Public Sub New()
        Me.InitializeComponent()
        Me.cmbPort.SelectedIndex = 1
        Me.cmbEncryption.SelectedIndex = 1
    End Sub

    Private Sub Authenticate(ByVal arg As Object)
        Try
            If CBool(arg) Then
                Dim token As GoogleAccessToken = GoogleOAuth2Provider.GetAccessToken(Me._googleOAuth2Key)
                Dim profile As GoogleProfile = GoogleOAuth2Provider.GetUserProfile(token)
                My.MyApplication.ImapClient.Credentials = New OAuth2Credentials(profile.email, token.access_token)
            End If
            If My.MyApplication.ImapClient.Login Then
                MyBase.Invoke(New SuccessDelegate(AddressOf Me.OnAuthenticateSuccessful), New Object() {arg})
            Else

                MyBase.Invoke(New FailedDelegate(AddressOf Me.OnAuthenticateFailed), New Object() {arg})
            End If
        Catch ex As Exception
            MyBase.Invoke(New FailedDelegate(AddressOf Me.OnAuthenticateFailed), New Object() {ex, arg})
        End Try
    End Sub

    Private Sub btnDefaultAuth_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDefaultAuth.Click
        Me.btnDefaultAuth.Hide()
        Me.picGMailLogin.Show()
        Me.wbrMain.Navigate("about:blank")
        Me.wbrMain.Hide()
        Me.pnlLogin.Show()
    End Sub

    Private Sub btnSignIn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSignIn.Click
        If String.IsNullOrEmpty(Me.txtServer.Text) Then
            MessageBox.Show("Server cannot be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        ElseIf String.IsNullOrEmpty(Me.cmbPort.Text) Then
            MessageBox.Show("Port cannot be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        ElseIf String.IsNullOrEmpty(Me.txtLogin.Text) Then
            MessageBox.Show("Login cannot be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        ElseIf String.IsNullOrEmpty(Me.txtPassword.Text) Then
            MessageBox.Show("Password cannot be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        Else
            Me.pnlLogin.Hide()
            Me.lblWait.Text = "Connecting..."
            Me.lblWait.Show()
            Me.picGMailLogin.Hide()
            Me.btnDefaultAuth.Hide()
            Me.InitClient(False)
            Dim thread As New Thread(New ParameterizedThreadStart(AddressOf Me.Connect))
            thread.Start(False)
        End If
    End Sub

    Private Sub cmbEncryption_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbEncryption.SelectedIndexChanged
        Me.chkValidateCertificate.Enabled = (Me.cmbEncryption.SelectedIndex > 0)
        If (Me.cmbEncryption.SelectedIndex <= 1) Then
            Me.cmbPort.SelectedIndex = Me.cmbEncryption.SelectedIndex
        End If
    End Sub

    Private Sub cmbPort_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles cmbPort.KeyDown
        If (e.KeyCode = Keys.Return) Then
            Me.btnSignIn_Click(Nothing, Nothing)
        Else
            e.SuppressKeyPress = ((e.KeyValue < &H30) OrElse (e.KeyValue > &H39))
        End If
    End Sub

    Private Sub Connect(ByVal arg As Object)
        Try
            If My.MyApplication.ImapClient.Connect Then
                MyBase.Invoke(New SuccessDelegate(AddressOf Me.OnConnectSuccessful), New Object() {arg})
            Else
                MyBase.Invoke(New FailedDelegate(AddressOf Me.OnConnectFailed), New Object() {arg})
            End If
        Catch ex As Exception
            MyBase.Invoke(New FailedDelegate(AddressOf Me.OnConnectFailed), New Object() {ex, arg})
        End Try
    End Sub

    Private Sub frmLogin_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles txtServer.KeyDown, txtPassword.KeyDown, txtLogin.KeyDown, cmbEncryption.KeyDown, chkValidateCertificate.KeyDown
        If (e.KeyCode = Keys.Return) Then
            Me.btnSignIn_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub InitClient(Optional ByVal isGMail As Boolean = False)
        If (My.MyApplication.ImapClient Is Nothing) Then
            My.MyApplication.ImapClient = New ImapClient
        End If
        If isGMail Then
            My.MyApplication.ImapClient.Host = "imap.gmail.com"
            My.MyApplication.ImapClient.Port = &H3E1
            My.MyApplication.ImapClient.SslProtocol = SslProtocols.Default
            My.MyApplication.ImapClient.ValidateServerCertificate = True
        Else
            Dim ssl As SslProtocols = If((Me.cmbEncryption.SelectedIndex = 0), SslProtocols.None, If((Me.cmbEncryption.SelectedIndex = 1), SslProtocols.Default, SslProtocols.Tls))
            My.MyApplication.ImapClient.Host = Me.txtServer.Text
            My.MyApplication.ImapClient.Port = Integer.Parse(Me.cmbPort.Text)
            My.MyApplication.ImapClient.SslProtocol = ssl
            My.MyApplication.ImapClient.ValidateServerCertificate = (Not Me.chkValidateCertificate.Enabled OrElse Me.chkValidateCertificate.Checked)
        End If
        My.MyApplication.ImapClient.IsDebug = True
    End Sub

    Private Sub OnAuthenticateFailed(ByVal ex As Exception, ByVal isOAuth2 As Boolean)
        MessageBox.Show(("Authentication failed" & If((ex Is Nothing), "", (Environment.NewLine + ex.ToString))), "Authentication failed", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        Me.lblWait.Hide()
        My.MyApplication.ImapClient.Disconnect()
        If isOAuth2 Then
            Me.picGMailLogin_Click(Nothing, Nothing)
            Me.btnDefaultAuth.Show()
        Else
            Me.pnlLogin.Show()
            Me.picGMailLogin.Show()
        End If
    End Sub

    Private Sub OnAuthenticateSuccessful(ByVal isOAuth2 As Boolean)
        MyBase.DialogResult = DialogResult.OK
        MyBase.Close()
    End Sub

    Private Sub OnConnectFailed(ByVal ex As Exception, ByVal isOAuth2 As Boolean)
        MessageBox.Show(("Connection failed" & If((ex Is Nothing), "", (Environment.NewLine + ex.ToString()))), "Connection failed", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        Me.lblWait.Hide()
        If isOAuth2 Then
            Me.picGMailLogin_Click(Nothing, Nothing)
            Me.btnDefaultAuth.Show()
        Else
            Me.pnlLogin.Show()
            Me.picGMailLogin.Show()
        End If
    End Sub

    Private Sub OnConnectSuccessful(ByVal isOAuth2 As Boolean)
        Me.lblWait.Text = "Connected. Authenticating..."
        If Not isOAuth2 Then
            My.MyApplication.ImapClient.Credentials = New PlainCredentials(Me.txtLogin.Text, Me.txtPassword.Text)
        End If
        Dim thread As New Thread(New ParameterizedThreadStart(AddressOf Me.Authenticate))
        thread.Start(isOAuth2)
    End Sub

    Private Sub picGMailLogin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles picGMailLogin.Click
        Me.wbrMain.Show()
        Me.pnlLogin.Hide()
        Me.btnDefaultAuth.Show()
        Me.picGMailLogin.Hide()
        Me.wbrMain.Navigate(GoogleOAuth2Provider.BuildAuthenticationUri)
    End Sub

    Private Sub picLogo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles picLogo.Click
        Process.Start("http://imapx.codeplex.com")
    End Sub

    Private Sub wbrMain_DocumentCompleted(ByVal sender As Object, ByVal e As WebBrowserDocumentCompletedEventArgs) Handles wbrMain.DocumentCompleted
        Try
            Dim codeField = Me.wbrMain.Document.GetElementById("code")

            If codeField Is Nothing Then Return

            Me._googleOAuth2Key = Me.wbrMain.Document.GetElementById("code").GetAttribute("value")
            Me.wbrMain.Hide()
            Me.lblWait.Text = "Connecting..."
            Me.lblWait.Show()
            Me.btnDefaultAuth.Hide()
            Me.InitClient(True)
            Dim thread As New Thread(New ParameterizedThreadStart(AddressOf Me.Connect))
            thread.Start(True)
        Catch obj1 As Exception
        End Try
    End Sub

    Private Delegate Sub FailedDelegate(ByVal ex As Exception, ByVal isOAuth2 As Boolean)

    Private Delegate Sub SuccessDelegate(ByVal isOAuth2 As Boolean)


End Class