using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Windows.Forms;

namespace ImapX.Sample
{
    public partial class FrmConnect : Form
    {

        private string _host;
        private int _port;
        private string _login;
        private string _pass;
        private bool _useSSL;
        private string _result;
        private readonly SslProtocols[] _sslProtocols = { SslProtocols.None, SslProtocols.Default, SslProtocols.Tls  };
        private SslProtocols _selectedProtocol = SslProtocols.None;

        public FrmConnect()
        {
            InitializeComponent();
            cmbEncryption.SelectedIndex = 1;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLogin.Text) || string.IsNullOrEmpty(txtPass.Text) || string.IsNullOrEmpty(txtPort.Text) || string.IsNullOrEmpty(txtServer.Text) || !int.TryParse(txtPort.Text, out _port))
                MessageBox.Show("Please check the values you entered", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            else
            {
                _host = txtServer.Text.Trim();
                _login = txtLogin.Text;
                _pass = txtPass.Text;
                _useSSL = cmbEncryption.SelectedIndex != 0;
                _selectedProtocol = _sslProtocols[cmbEncryption.SelectedIndex];
                btnConnect.Enabled = false;
                bgwMain.RunWorkerAsync();
            }
        }

        private void bgwMain_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (InvokeRequired)
                Invoke(new RunWorkerCompletedEventHandler(bgwMain_RunWorkerCompleted), new {sender, e});
            else
            {
                btnConnect.Enabled = true;
                if((bool)e.Result)
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

        private void bgwMain_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                e.Result = false;
                Program.ImapClient = new ImapClient(_host, _port, _selectedProtocol);
                if (Program.ImapClient.Connect())
                {

                    if(Program.ImapClient.Login(_login, _pass))
                        e.Result = true;
                    else
                        _result = "Failed to login";
                }
                else
                    _result = "Failed to connect";
            }
            catch(Exception ex)
            {
                _result = ex.ToString();

            }
        }

        private void lnkGOAuthSignIn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var frm = new FrmGOAuth())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }
    }
}
