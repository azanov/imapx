using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ImapX.Collections;

namespace ImapX.Sample
{
    public partial class FrmConsole : Form
    {
        public FrmConsole()
        {
            InitializeComponent();
        }

        private void FrmConsole_Load(object sender, EventArgs e)
        {
            using (var frmStart = new FrmConnect())
            {
                if (frmStart.ShowDialog() == DialogResult.OK)
                {
                    PrintLine("Welcome to ImapX console");
                    PrintLine();
                    PrintLine("--------------");
                    PrintLine();
                    PrintLine("Loading folder structure...");
                    PrintFolders(Program.ImapClient.Folders, 0);
                    PrintLine();
                    PrintLine("--------------");
                }
                else
                    Application.Exit();
            }
        }

        private void PrintLine(string value = "")
        {
            txtOut.AppendText(value + Environment.NewLine);
            txtOut.SelectionStart = txtOut.Text.Length;

            txtOut.ScrollToCaret();
        }

        private void PrintFolders(IEnumerable<Folder> folders, int level)
        {
            foreach (var folder in folders)
            {
                PrintLine(string.Format("-{0} {1}", new string('-', level), folder.Name));
                if(folder.HasChildren)
                    PrintFolders(folder.SubFolders, level + 1);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtOut.Clear();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            var txt = txtIn.Text;
            txtIn.Clear();
            PrintLine("> " + txt);
           
            IList<string> data = new List<string>();
            Program.ImapClient.SendAndReceive(txt, ref data);
            foreach (var line in data)
            {
                PrintLine(line);
            }
            txtIn.Clear();

        }

        private void txtIn_TextChanged(object sender, EventArgs e)
        {
            btnSend.Enabled = !string.IsNullOrEmpty(txtIn.Text);
        }
    }
}
