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

        private TreeNode FolderToNode(Folder folder)
        {
            var n = new TreeNode();
            n.Text = folder.Name;
            n.Tag = folder;
            if (folder.HasChildren)
            {
                if (folder.SubFoldersLoaded)
                    n.Nodes.AddRange(folder.SubFolders.Select(FolderToNode).ToArray());
                else
                    n.Nodes.Add("...");
            }
            return n;
        }

        private void BindSpecialFolders()
        {
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

        private async void Init()
        {
            if (_initialized) return;
            _initialized = true;

            Program.Client.Behavior.FolderTreeBrowseMode = FolderTreeBrowseMode.Lazy;
            Program.Client.Behavior.ExamineFolders = true;

            var folders = await Task.Run(() => Program.Client.Folders);

            BindSpecialFolders();

            trwFolders.Nodes.AddRange(folders.Select(FolderToNode).ToArray());

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

        private async void trwFolders_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            var folder = e.Node.Tag as Folder;
            if (folder != null && e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Tag == null)
            {
                e.Node.Nodes.Clear();
                var subFolders = await Task.Run(() => folder.SubFolders);
                e.Node.Nodes.AddRange(subFolders.Select(FolderToNode).ToArray());
                e.Node.Expand();
                BindSpecialFolders();
            }
        }
    }
}
