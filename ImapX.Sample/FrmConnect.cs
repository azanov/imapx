using ImapX.Enums;
using ImapX.Sample.Extensions;
using System;
using System.Windows.Forms;

namespace ImapX.Sample
{
    public partial class FrmConnect : Form
    {
        public FrmConnect()
        {
            InitializeComponent();

            var securityNames = Enum.GetNames(typeof(ImapConnectionSecurity));
            cmbSecurity.Items.AddRange(securityNames);
            cmbSecurity.SelectedItem = ImapConnectionSecurity.SSL.ToString();
            cmbHost.SelectedIndex = 0;
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            if (Program.Client == null)
                Program.Client = new ImapClient() {
                     IsDebug = true
                };

            pnlConnect.Enabled = false;

            var result = await Program.Client.ConnectAsync(
                cmbHost.Text, int.Parse(txtPort.Text),
                (ImapConnectionSecurity)Enum.Parse(typeof(ImapConnectionSecurity), cmbSecurity.Text),
                chkValidateCertificate.Checked);

            if (result.Success)
            {
                if (Program.Client.IsAuthenticated)
                {
                    DialogResult = DialogResult.OK; Close();
                }
                else
                {
                    lblCurrentHost.Text = Program.Client.Host;
                    cmbLanguage.Items.Clear();

                    cmbLanguage.Enabled = false;
                    cmbLanguage.Items.AddRange(await Program.Client.GetSupportedLanguagesAsync());
                    cmbLanguage.SelectedItem = "i-default";
                    cmbLanguage.Enabled = true;

                    tbcMain.SelectedTab = tbpSignIn;
                    txtUser.Focus();
                }
            }
            else
                MessageBox.Show("Failed to connect. Details: " + result.Exception?.ToString() ?? "", "Connectiong failed");

            pnlConnect.Enabled = true;
        }

        private async void btnSignIn_Click(object sender, EventArgs e)
        {
            pnlSignIn.Enabled = false;

            var result = await Program.Client.LoginAsync(txtUser.Text, txtPassword.Text);

            if (result.Success)
            {
                DialogResult = DialogResult.OK; Close();
            }
            else
                MessageBox.Show("Failed to sign in. Details: " + result.Exception?.ToString() ?? "", "Authentication failed");

            pnlSignIn.Enabled = true;
        }

        private void Header_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) this.NotifyMovable();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            Program.Client.Disconnect();
            tbcMain.SelectedTab = tbpConnect;
        }

        private void SignIn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnSignIn_Click(this, null);
        }

        private void Connect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnConnect_Click(this, null);
        }

        private void FrmConnect_Load(object sender, EventArgs e)
        {
            cmbHost.Focus();
        }

        private void cmbSecurity_SelectedIndexChanged(object sender, EventArgs e)
        {
            var security = (ImapConnectionSecurity)Enum.Parse(typeof(ImapConnectionSecurity), cmbSecurity.Text);
            switch (security)
            {
                case ImapConnectionSecurity.SSL:
                    txtPort.Text = ImapBase.DefaultImapSslPort.ToString();
                    break;

                default:
                    txtPort.Text = ImapBase.DefaultImapPort.ToString();
                    break;
            }
        }
    }
}
