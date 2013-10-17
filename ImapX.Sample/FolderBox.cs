using System;
using System.Linq;
using System.Windows.Forms;

namespace ImapX.Sample
{
    public sealed partial class FolderBox : Form
    {
        public FolderBox(string title, string text)
        {
            InitializeComponent();

            Text = title;
            lblTitle.Text = title;
            lblText.Text = text;

            trwFolders.Nodes.Add(Program.ImapClient.Host);
            trwFolders.Nodes[0].Nodes.AddRange(Program.ImapClient.Folders.Select(FolderToNode).ToArray());
            trwFolders.Nodes[0].Expand();
        }

        public Folder SelectedFolder
        {
            get { return trwFolders.SelectedNode.Tag as Folder; }
        }

        private TreeNode FolderToNode(Folder folder)
        {
            var node = new TreeNode(folder.Name);
            node.Nodes.AddRange(folder.SubFolders.Select(FolderToNode).ToArray());
            node.Tag = folder;
            return node;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void trwFolders_AfterSelect(object sender, TreeViewEventArgs e)
        {
            btnOK.Enabled = trwFolders.SelectedNode != null && trwFolders.SelectedNode != trwFolders.Nodes[0];
        }

        private void trwFolders_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            var folder = e.Node.Tag as Folder;
            if (folder == null || !folder.Selectable)
                e.Cancel = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public static Folder Show(string title, string text, IWin32Window owner = null)
        {
            using (var dlg = new FolderBox(title, text))
                if (dlg.ShowDialog(owner) == DialogResult.OK)
                    return dlg.SelectedFolder;
            return null;
        }

        private void trwFolders_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (btnOK.Enabled)
                btnOK_Click(null, null);
        }
    }
}