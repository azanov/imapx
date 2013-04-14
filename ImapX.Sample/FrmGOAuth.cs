using ImapX.Sample.Google;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImapX.Sample
{
    public partial class FrmGOAuth : Form
    {

        private string _result;

        public FrmGOAuth()
        {
            InitializeComponent();
        }

        private void FrmGOAuth_Load(object sender, EventArgs e)
        {
            wbrAuth.Navigate(GoogleOAuth2Provider.BuildAuthenticationUri());
        }

        private void wbrAuth_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                var element = wbrAuth.Document.GetElementById("code");
                var code = element.GetAttribute("value");
                wbrAuth.Hide();
                lblWait.Text = string.Format(lblWait.Text, code);
                lblWait.Show();
                bgwConnect.RunWorkerAsync(code);
            }
            catch { 
                
            }
        }

        private void bgwConnect_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                e.Result = false;
                Program.ImapClient = new ImapClient("imap.gmail.com", 993, true);
                if (Program.ImapClient.Connection())
                {
                    var token = GoogleOAuth2Provider.GetAccessToken(e.Argument as string);
                    var profile = GoogleOAuth2Provider.GetUserProfile(token);

                    if (Program.ImapClient.OAuth2LogIn(profile.email, token.access_token))
                        e.Result = true;
                    else
                        _result = "Failed to login";
                }
                else
                    _result = "Failed to connect";
            }
            catch (Exception ex)
            {
                _result = ex.ToString();

            }
        }

        private void bgwConnect_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (InvokeRequired)
                Invoke(new RunWorkerCompletedEventHandler(bgwConnect_RunWorkerCompleted), new { sender, e });
            else
            {
                
                if ((bool)e.Result)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(_result, "Error", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }

            }
        }
    }
}
