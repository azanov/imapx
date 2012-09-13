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
    public partial class FrmConnect : Form
    {

        private string _host;
        private int _port;
        private string _login;
        private string _pass;
        private bool _useSSL;
        private string _result;

        public FrmConnect()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLogin.Text) || string.IsNullOrWhiteSpace(txtPort.Text) || string.IsNullOrWhiteSpace(txtServer.Text) || !int.TryParse(txtPort.Text, out _port))
                MessageBox.Show("Please check the values you entered", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            else
            {
                _host = txtServer.Text.Trim();
                _login = txtLogin.Text;
                _pass = txtPass.Text;
                _useSSL = chkUseSSL.Checked;
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
                Program.ImapClient = new ImapClient(_host, _port, _useSSL);
                if (Program.ImapClient.Connection())
                {

                    if(Program.ImapClient.LogIn(_login, _pass))
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
    }
}
