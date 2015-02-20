using System;
using System.Diagnostics;
using System.Security.Authentication;
using System.Threading;
using System.Windows.Forms;
using ImapX.Authentication;
using ImapX.Sample.Google;
using System.ComponentModel;
using ImapX.Sample.Outlook;
using System.Text.RegularExpressions;
using ImapX.Sample.Yahoo;

namespace ImapX.Sample
{
    [DefaultValue(Simple)]
    public enum AuthMode
    {
        Simple,
        Google,
        Outlook,
        Yahoo
    }

    public partial class FrmConnect : Form
    {

        private AuthMode _authMode = AuthMode.Simple;
        private Regex _rexCode = new Regex(@"code=([^&]+)", RegexOptions.IgnoreCase);

        private string _oAuth2Code;

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
            _authMode = AuthMode.Google;

            wbrMain.Show();
            pnlLogin.Hide();
            btnDefaultAuth.Show();
            picGMailLogin.Hide();
            pnlLogin.Hide();
            picOutlook.Hide();
            picYahoo.Hide();
            wbrMain.Navigate(GoogleOAuth2Provider.BuildAuthenticationUri());
        }

        private void wbrMain_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                _oAuth2Code = "";

                if (_authMode == AuthMode.Google)
                {
                    HtmlElement element = wbrMain.Document.GetElementById("code");
                    if (element == null) return;
                    _oAuth2Code = element.GetAttribute("value");
                }
                else if (_authMode == AuthMode.Outlook && wbrMain.Url.ToString().StartsWith(OutlookOAuth2Provider.REDIRECT_URI)) {
                    var match = _rexCode.Match(wbrMain.Url.Query);
                    if (match.Success)
                    {
                        _oAuth2Code = match.Groups[1].Value;
                    }
                }
                else if (_authMode == AuthMode.Yahoo && wbrMain.Url.ToString().StartsWith(YahooOAuth2Provider.REDIRECT_URI))
                {
                    var match = _rexCode.Match(wbrMain.Url.Query);
                    if (match.Success)
                    {
                        _oAuth2Code = match.Groups[1].Value;
                    }
                }

                if (!string.IsNullOrWhiteSpace(_oAuth2Code))
                {
                    wbrMain.Hide();
                    lblWait.Text = "Connecting...";
                    lblWait.Show();
                    btnDefaultAuth.Hide();

                    InitClient();

                    (new Thread(Connect)).Start(true);
                }

            }
            catch
            {
            }
        }

        private void btnDefaultAuth_Click(object sender, EventArgs e)
        {
            _authMode = AuthMode.Simple;
            btnDefaultAuth.Hide();
            picGMailLogin.Show();
            picOutlook.Show();
            picYahoo.Show();
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
                picOutlook.Hide();
                picYahoo.Hide();
                btnDefaultAuth.Hide();

                InitClient();

                (new Thread(Connect)).Start(false);
            }
        }

        private void InitClient()
        {
            if (Program.ImapClient == null)
                Program.ImapClient = new ImapClient();

            if (_authMode == AuthMode.Google)
            {
               
                Program.ImapClient.Host = "imap.gmail.com";
                Program.ImapClient.Port = 993;
                Program.ImapClient.SslProtocol = SslProtocols.Default;
                Program.ImapClient.ValidateServerCertificate = true;
            }
            else if (_authMode == AuthMode.Outlook)
            {
                Program.ImapClient.Host = "imap-mail.outlook.com";
                Program.ImapClient.Port = 993;
                Program.ImapClient.SslProtocol = SslProtocols.Default;
                Program.ImapClient.ValidateServerCertificate = true;
            }
            else if (_authMode == AuthMode.Yahoo)
            {
                Program.ImapClient.Host = "imap.mail.yahoo.com";
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
                    Invoke(new SuccessDelegate(OnConnectSuccessful));
                else
                    Invoke(new FailedDelegate(OnConnectFailed));
            }
            catch (Exception ex)
            {
                Invoke(new FailedDelegate(OnConnectFailed));
            }
        }

        private void Authenticate(object arg)
        {
            try
            {
                if (_authMode == AuthMode.Google)
                {
                    GoogleAccessToken token = GoogleOAuth2Provider.GetAccessToken(_oAuth2Code);
                    GoogleProfile profile = GoogleOAuth2Provider.GetUserProfile(token);

                    Program.ImapClient.Credentials = new OAuth2Credentials(profile.email, token.access_token);
                }
                else if (_authMode == AuthMode.Outlook)
                {
                    var token = OutlookOAuth2Provider.GetAccessToken(_oAuth2Code);
                    var profile = OutlookOAuth2Provider.GetUserProfile(token.access_token);
                    Program.ImapClient.Credentials = new OAuth2Credentials(profile.emails.account, token.access_token);
                }
                else if (_authMode == AuthMode.Yahoo)
                {
                    var token = YahooOAuth2Provider.GetAccessToken(_oAuth2Code);
                    Program.ImapClient.Credentials = new OAuth2Credentials(token.xoauth_yahoo_guid, token.access_token, "ImapX");
                }

                if (Program.ImapClient.Login())
                    Invoke(new SuccessDelegate(OnAuthenticateSuccessful));
                else
                    Invoke(new FailedDelegate(OnAuthenticateFailed));
            }
            catch (Exception ex)
            {
                Invoke(new FailedDelegate(OnAuthenticateFailed), new[] {ex});
            }
        }

        private void OnAuthenticateSuccessful()
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void OnAuthenticateFailed()
        {
            MessageBox.Show("Authentication failed",
                "Authentication failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            lblWait.Hide();

            Program.ImapClient.Disconnect();

            if (_authMode == AuthMode.Simple)
            {
                pnlLogin.Show();
                picGMailLogin.Show();
                picOutlook.Show();
                picYahoo.Show();
                return;
            }

            btnDefaultAuth.Show();

            if (_authMode == AuthMode.Google)
                picGMailLogin_Click(null, null);
            else if (_authMode == AuthMode.Outlook)
                picOutlook_Click(null, null);
            else if (_authMode == AuthMode.Yahoo)
                picYahoo_Click(null, null);
        }

        private void OnConnectSuccessful()
        {
            lblWait.Text = "Connected. Authenticating...";

            if (_authMode == AuthMode.Simple)
                Program.ImapClient.Credentials = new PlainCredentials(txtLogin.Text, txtPassword.Text);

            (new Thread(Authenticate)).Start();
        }

        private void OnConnectFailed()
        {
            MessageBox.Show("Connection failed" ,
                "Connection failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            lblWait.Hide();

            if (_authMode == AuthMode.Simple)
            {
                pnlLogin.Show();
                picGMailLogin.Show();
                picOutlook.Show();
                picYahoo.Show();
                return;
            }

            btnDefaultAuth.Show();

            if (_authMode == AuthMode.Google)
                picGMailLogin_Click(null, null);
            else if (_authMode == AuthMode.Outlook)
                picOutlook_Click(null, null);
            else if (_authMode == AuthMode.Yahoo)
                picYahoo_Click(null, null);
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

        private delegate void FailedDelegate();

        private delegate void SuccessDelegate();

        private void frmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                btnSignIn_Click(null, null);
        }

        private void picOutlook_Click(object sender, EventArgs e)
        {
            _authMode = AuthMode.Outlook;

            wbrMain.Show();
            pnlLogin.Hide();
            btnDefaultAuth.Show();
            picGMailLogin.Hide();
            picOutlook.Hide();
            picYahoo.Hide();
            wbrMain.Navigate(OutlookOAuth2Provider.BuildAuthenticationUri());
        }

        private void picYahoo_Click(object sender, EventArgs e)
        {
            _authMode = AuthMode.Yahoo;

            wbrMain.Show();
            pnlLogin.Hide();
            btnDefaultAuth.Show();
            picGMailLogin.Hide();
            pnlLogin.Hide();
            picOutlook.Hide();
            picYahoo.Hide();
            wbrMain.Navigate(YahooOAuth2Provider.BuildAuthenticationUri());
        }
    }
}