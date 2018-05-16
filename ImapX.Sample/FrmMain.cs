using ImapX.Enums;
using ImapX.Sample.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImapX.Sample
{
    public partial class FrmMain : Form
    {
        private bool _initialized = false;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            Hide();
            using (var frmConnect = new FrmConnect())
            {
                if (frmConnect.ShowDialog() == DialogResult.OK)
                {
                    Show(); 
                }
                else
                    Close();
            }
        }

        private async void Init()
        {
            if (_initialized) return;
            _initialized = true;

            Program.Client.Behavior.FolderTreeBrowseMode = FolderTreeBrowseMode.Full;

            var folders = await Task.Run(() => Program.Client.Folders);

            lnkArchive.Folder = Program.Client.Folders.Archive;
            lnkAllMails.Folder = Program.Client.Folders.All;
            lnkTrash.Folder = Program.Client.Folders.Trash;
            lnkJunk.Folder = Program.Client.Folders.Junk;
            lnkFlagged.Folder = Program.Client.Folders.Flagged;
            lnkImportant.Folder = Program.Client.Folders.Important;
            lnkDrafts.Folder = Program.Client.Folders.Drafts;
            lnkSent.Folder = Program.Client.Folders.Sent;
            lnkInbox.Folder = Program.Client.Folders.Inbox;

        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            Init();
        }

        private async void lnkFolder_Click(object sender, EventArgs e)
        {
            var lnk = (FolderLinkLabel)sender;
            var folder = lnk.Folder;
            
            await folder.SelectAsync();

            for (var i = 0; i < panel1.Controls.Count; i++)
                (panel1.Controls[i] as FolderLinkLabel)?.Refresh();

        }
    }
}
