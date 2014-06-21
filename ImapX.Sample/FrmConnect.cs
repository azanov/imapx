using System;
using System.Diagnostics;
using System.Security.Authentication;
using System.Threading;
using System.Windows.Forms;
using ImapX.Authentication;
using ImapX.Sample.Google;

namespace ImapX.Sample
{
    public partial class FrmConnect : Form
    {
        private string _googleOAuth2Key;

        public FrmConnect()
        {
            InitializeComponent();

            cmbPort.SelectedIndex = 1;
            cmbEncryption.SelectedIndex = 1;
        }

        private void picLogo_Click(object sender, EventArgs e)
        {
            Process.Start("http://imapx.codeplex.com");
        }

        private void picGMailLogin_Click(object sender, EventArgs e)
        {
            wbrMain.Show();
            pnlLogin.Hide();
            btnDefaultAuth.Show();
            picGMailLogin.Hide();
            wbrMain.Navigate(GoogleOAuth2Provider.BuildAuthenticationUri());
        }

        private void wbrMain_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                HtmlElement element = wbrMain.Document.GetElementById("code");

                if (element == null) return;

                _googleOAuth2Key = element.GetAttribute("value");
                wbrMain.Hide();
                lblWait.Text = "Connecting...";
                lblWait.Show();
                btnDefaultAuth.Hide();

                InitClient(true);

                (new Thread(Connect)).Start(true);
            }
            catch
            {
            }
        }

        private void btnDefaultAuth_Click(object sender, EventArgs e)
        {
            btnDefaultAuth.Hide();
            picGMailLogin.Show();
            wbrMain.Navigate("about:blank");
            wbrMain.Hide();

            pnlLogin.Show();
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbServer.Text))
                MessageBox.Show("Server cannot be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (string.IsNullOrEmpty(cmbPort.Text))
                MessageBox.Show("Port cannot be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            else if (string.IsNullOrEmpty(txtLogin.Text))
                MessageBox.Show("Login cannot be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (string.IsNullOrEmpty(txtPassword.Text))
                MessageBox.Show("Password cannot be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                pnlLogin.Hide();
                lblWait.Text = "Connecting...";
                lblWait.Show();
                picGMailLogin.Hide();
                btnDefaultAuth.Hide();

                InitClient();

                (new Thread(Connect)).Start(false);
            }
        }

        private void InitClient(bool isGMail = false)
        {
            if (Program.ImapClient == null)
                Program.ImapClient = new ImapClient();

            if (isGMail)
            {
               
                Program.ImapClient.Host = "imap.gmail.com";
                Program.ImapClient.Port = 993;
                Program.ImapClient.SslProtocol = SslProtocols.Default;
                Program.ImapClient.ValidateServerCertificate = true;
            }
            else
            {
                SslProtocols ssl = cmbEncryption.SelectedIndex == 0
                    ? SslProtocols.None
                    : (cmbEncryption.SelectedIndex == 1 ? SslProtocols.Default : SslProtocols.Tls);

                Program.ImapClient.Host = cmbServer.Text;
                Program.ImapClient.Port = int.Parse(cmbPort.Text);
                Program.ImapClient.SslProtocol = ssl;
                Program.ImapClient.ValidateServerCertificate = !chkValidateCertificate.Enabled ||
                                                               chkValidateCertificate.Checked;
            }
            Program.ImapClient.IsDebug = true;
        }

        private void Connect(object arg)
        {
            try
            {
                if (Program.ImapClient.Connect())
                    Invoke(new SuccessDelegate(OnConnectSuccessful), new[] {arg});
                else
                    Invoke(new FailedDelegate(OnConnectFailed), new[] {null, arg});
            }
            catch (Exception ex)
            {
                Invoke(new FailedDelegate(OnConnectFailed), new[] {ex, arg});
            }
        }

        private void Authenticate(object arg)
        {
            try
            {
                if ((bool) arg)
                {
                    GoogleAccessToken token = GoogleOAuth2Provider.GetAccessToken(_googleOAuth2Key);
                    GoogleProfile profile = GoogleOAuth2Provider.GetUserProfile(token);

                    Program.ImapClient.Credentials = new OAuth2Credentials(profile.email, token.access_token);
                }


                if (Program.ImapClient.Login())
                    Invoke(new SuccessDelegate(OnAuthenticateSuccessful), new[] {arg});
                else
                    Invoke(new FailedDelegate(OnAuthenticateFailed), new[] {null, arg});
            }
            catch (Exception ex)
            {
                Invoke(new FailedDelegate(OnAuthenticateFailed), new[] {ex, arg});
            }
        }

        private void OnAuthenticateSuccessful(bool isOAuth2)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void OnAuthenticateFailed(Exception ex, bool isOAuth2)
        {
            MessageBox.Show("Authentication failed" + (ex == null ? "" : (Environment.NewLine + ex)),
                "Authentication failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            lblWait.Hide();

            Program.ImapClient.Disconnect();

            if (isOAuth2)
            {
                picGMailLogin_Click(null, null);
                btnDefaultAuth.Show();
            }
            else
            {
                pnlLogin.Show();
                picGMailLogin.Show();
            }
        }

        private void OnConnectSuccessful(bool isOAuth2)
        {
            lblWait.Text = "Connected. Authenticating...";

            if (!isOAuth2)
                Program.ImapClient.Credentials = new PlainCredentials(txtLogin.Text, txtPassword.Text);

            (new Thread(Authenticate)).Start(isOAuth2);
        }

        private void OnConnectFailed(Exception ex, bool isOAuth2)
        {
            MessageBox.Show("Connection failed" + (ex == null ? "" : (Environment.NewLine + ex)),
                "Connection failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            lblWait.Hide();


            if (isOAuth2)
            {
                picGMailLogin_Click(null, null);
                btnDefaultAuth.Show();
            }
            else
            {
                pnlLogin.Show();
                picGMailLogin.Show();
            }
        }

        private void cmbPort_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnSignIn_Click(null, null);
            else
                e.SuppressKeyPress = !(e.KeyValue >= 48 && e.KeyValue <= 57);
        }

        private void cmbEncryption_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkValidateCertificate.Enabled = cmbEncryption.SelectedIndex > 0;
            if (cmbEncryption.SelectedIndex > 1) return;
            cmbPort.SelectedIndex = cmbEncryption.SelectedIndex;
        }

        private delegate void FailedDelegate(Exception ex, bool isOAuth2);

        private delegate void SuccessDelegate(bool isOAuth2);

        private void frmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                btnSignIn_Click(null, null);
        }
    }
}