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
    public partial class FrmMoveToFolder : Form
    {
        public FrmMoveToFolder()
        {
            InitializeComponent();
            trwFolders.Nodes.Add(Program.ImapClient._client.Host);
            trwFolders.Nodes[0].Nodes.AddRange(Program.ImapClient.Folders.Select(FolderToNode).ToArray());
            trwFolders.Nodes[0].Expand();
        }

        public Folder SelectedFolder { 
            get { return trwFolders.SelectedNode.Tag as Folder; }   
        }

        private TreeNode FolderToNode(Folder folder)
        {
            var node = new TreeNode(folder.Name);
            node.Nodes.AddRange(folder.SubFolder.Select(FolderToNode).ToArray());
            node.Tag = folder;
            return node;

        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void trwFolders_AfterSelect(object sender, TreeViewEventArgs e)
        {
            btnSelect.Enabled = trwFolders.SelectedNode != null && trwFolders.SelectedNode != trwFolders.Nodes[0];
        }
    }
}
