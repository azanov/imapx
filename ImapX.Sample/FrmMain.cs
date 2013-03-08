using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using ImapX.Sample.Native;
using System.Runtime.Serialization.Formatters.Binary;

namespace ImapX.Sample
{
    public partial class FrmMain : Form
    {
        private List<Message> _messages;
        private Folder _selectedFolder;
        private Message _selectedMessage;
        private TreeNode lastClickedNode;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            using (var frmConnect = new FrmConnect())
            {
                if (frmConnect.ShowDialog() == DialogResult.OK)
                {
                    wbrMain.Navigate("about:blank");
                    //lstFolders.DataSource = Program.ImapClient.Folders.Select(_ => _.Name).ToArray();
                    trwFolders.Nodes.Add(Program.ImapClient._client.Host);
                    trwFolders.Nodes[0].Nodes.AddRange(Program.ImapClient.Folders.Select(FolderToNode).ToArray());
                    trwFolders.Nodes[0].Expand();
                }
                else
                    Application.Exit();
            }
        }

        private void lsvMails_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsvMails.SelectedIndices.Count == 0) return;
            _selectedMessage = _messages[lsvMails.SelectedIndices[0]];

            lblSubject.Text = string.IsNullOrEmpty(_selectedMessage.Subject)
                                  ? "No subject"
                                  : _selectedMessage.Subject.Replace("\n", "").Replace("\t", "");

            lblTime.Text = _selectedMessage.Date.ToString();
            lblFrom.Text = string.Join("; ", _selectedMessage.From.Select(_ => _.ToString()).ToArray());
            lblTo.Text = string.Join("; ", _selectedMessage.To.Select(_ => _.ToString()).ToArray());

            bool isHtml;
            string body = _selectedMessage.GetDecodedBody(out isHtml);
            wbrMain.Document.OpenNew(true);


            lsvAttachments.Items.Clear();

            var tmpDir = Path.Combine(Application.StartupPath, "tmp");
            var msgTmpDir = Path.Combine(tmpDir, _selectedMessage.MessageId.MD5());

            Directory.CreateDirectory(tmpDir);
            Directory.CreateDirectory(msgTmpDir);

            var files = new List<string>();

            foreach (Attachment attachment in _selectedMessage.Attachments)
            {
                try
                {
                    string path = Path.Combine(msgTmpDir,
                                               attachment.FileName);
                    files.Add(path);
                    attachment.SaveFile(msgTmpDir);
                }
                catch (Exception)
                {
                }
            }

            foreach (InlineAttachment inlineAttachment in _selectedMessage.InlineAttachments)
            {
                try
                {
                    string path = Path.Combine(msgTmpDir,
                                               inlineAttachment.FileName);
                    File.WriteAllBytes(path, inlineAttachment.FileData);
                    body = body.Replace("cid:" + inlineAttachment.FileName, path);
                }
                catch (Exception)
                {
                }
            }

            wbrMain.Document.Write(isHtml ? body : body.Replace("\n", "<br />"));

            UpdateAttachmentIcons(files);

            lsvAttachments.Items.AddRange(
                _selectedMessage.Attachments.Where(_ => !string.IsNullOrEmpty(_.FileName)).OrderBy(_ => _.FileName)
                    .Select(
                        _ => new ListViewItem(_.FileName)
                                 {
                                     ImageKey = _.FileName.Split('.').Last()
                                 }).ToArray());

            pnlInfo.Visible = true;
            wbrMain.Visible = true;
            pnlAttachments.Visible = _selectedMessage.Attachments.Any();
        }

        private void lsvAttachments_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lsvAttachments.SelectedItems.Count == 0 || e.Button != MouseButtons.Left) return;
            openToolStripMenuItem_Click(null, null);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lsvAttachments.SelectedItems.Count == 0) return;
            Message msg = _messages[lsvMails.SelectedIndices[0]];
            Attachment attachment =
                msg.Attachments.Where(_ => !string.IsNullOrEmpty(_.FileName)).OrderBy(_ => _.FileName).Skip(
                    lsvAttachments.SelectedItems[0].Index).Take(1).FirstOrDefault();

            var tmpDir = Path.Combine(Application.StartupPath, "tmp");
            var msgTmpDir = Path.Combine(tmpDir, msg.MessageId.MD5());

            Process.Start(Path.Combine(msgTmpDir, attachment.FileName));
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lsvAttachments.SelectedItems.Count == 0) return;
            Message msg = _messages[lsvMails.SelectedIndices[0]];
            Attachment attachment =
                msg.Attachments.Where(_ => !string.IsNullOrEmpty(_.FileName)).Skip(
                    lsvAttachments.SelectedItems[0].Index).Take(1).FirstOrDefault();
            sfdMain.FileName = attachment.FileName;

            if (sfdMain.ShowDialog() != DialogResult.OK) return;

            var tmpDir = Path.Combine(Application.StartupPath, "tmp");
            var msgTmpDir = Path.Combine(tmpDir, msg.MessageId.MD5());

            File.Copy(Path.Combine(msgTmpDir, attachment.FileName),
                      sfdMain.FileName);
        }

        private void mnuMessages_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = lsvMails.SelectedIndices.Count == 0;
        }

        private void mnuAttachment_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = lsvAttachments.SelectedItems.Count == 0;
        }

        #region Get mails from selected folder

        private void trwFolders_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (trwFolders.SelectedNode == null || trwFolders.SelectedNode == trwFolders.Nodes[0]) return;

            lblSelectFolder.Visible = false;

            trwFolders.Enabled = lsvMails.Enabled = false;
            lsvMails.VirtualListSize = 0;
            
            pnlInfo.Visible = wbrMain.Visible = pnlAttachments.Visible = false;
            pgbFetchMails.Visible = true;
            _selectedMessage = null;
            _selectedFolder = trwFolders.SelectedNode.Tag as Folder;

            (new Thread(GetMails)).Start();
        }

        private void GetMails()
        {
            try
            {
                _messages = _selectedFolder.Search("ALL", true).OrderByDescending(_ => _.Date).ToList();
                var args = new ServerCallCompletedEventArgs();
                Invoke(new EventHandler<ServerCallCompletedEventArgs>(GetMailsCompleted), Program.ImapClient, args);
            }
            catch (Exception ex)
            {
                var args = new ServerCallCompletedEventArgs(false, ex);
                Invoke(new EventHandler<ServerCallCompletedEventArgs>(GetMailsCompleted), Program.ImapClient, args);
            }
        }

        private void GetMailsCompleted(object sender, ServerCallCompletedEventArgs e)
        {
            if (e.Result)
            {
                lsvMails.VirtualListSize = _messages.Count;
                AutoResizeMessageListViewColumn();
            }
            else
            {
                using (var frm = new FrmError(e.Exception))
                    frm.ShowDialog();
            }
            pgbFetchMails.Visible = false;
            trwFolders.Enabled = lsvMails.Enabled = true;
        }

        #endregion

        #region Delete selected message

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("Do you really want to delete this message?", "Delete message", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) != DialogResult.Yes)
                return;


            trwFolders.Enabled = lsvMails.Enabled = false;

            (new Thread(DeleteSelectedMessage)).Start();
        }

        private void DeleteSelectedMessage()
        {
            try
            {

                var args = new ServerCallCompletedEventArgs(_selectedFolder.DeleteMessage(_selectedMessage));
                Invoke(new EventHandler<ServerCallCompletedEventArgs>(DeleteSelectedMessageCompleted),
                       Program.ImapClient, args);
            }
            catch (Exception ex)
            {
                var args = new ServerCallCompletedEventArgs(false, ex);
                Invoke(new EventHandler<ServerCallCompletedEventArgs>(DeleteSelectedMessageCompleted),
                       Program.ImapClient, args);
            }
        }

        private void DeleteSelectedMessageCompleted(object sender, ServerCallCompletedEventArgs e)
        {
            if (e.Result)
            {
                _messages.RemoveAt(lsvMails.SelectedIndices[0]);
                lsvMails.VirtualListSize--; lsvMails.Invalidate();
            }
            else if (e.Exception != null)
            {
                using (var frm = new FrmError(e.Exception))
                    frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Failed to delete the message", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            pgbFetchMails.Visible = false;
            trwFolders.Enabled = lsvMails.Enabled = true;
        }

        #endregion

        #region Move selected message

        private void moveToFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmMoveToFolder())
            {
                if (frm.ShowDialog() != DialogResult.OK) return;

                trwFolders.Enabled = lsvMails.Enabled = false;

                (new Thread(MoveSelectedMessage)).Start(frm.SelectedFolder);
            }
        }

        private void MoveSelectedMessage(object targetFolder)
        {
            try
            {
                var args =
                    new ServerCallCompletedEventArgs(_selectedFolder.MoveMessageToFolder(_selectedMessage,
                                                                                         targetFolder as Folder));
                Invoke(new EventHandler<ServerCallCompletedEventArgs>(MoveSelectedMessageCompleted), Program.ImapClient,
                       args);
            }
            catch (Exception ex)
            {
                var args = new ServerCallCompletedEventArgs(false, ex);
                Invoke(new EventHandler<ServerCallCompletedEventArgs>(MoveSelectedMessageCompleted), Program.ImapClient,
                       args);
            }
        }

        private void MoveSelectedMessageCompleted(object sender, ServerCallCompletedEventArgs e)
        {
            if (e.Result)
            {
                _messages.RemoveAt(lsvMails.SelectedIndices[0]);
                lsvMails.VirtualListSize--; lsvMails.Invalidate();
            }
            else if (e.Exception != null)
            {
                using (var frm = new FrmError(e.Exception))
                    frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Failed to move the message", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            pgbFetchMails.Visible = false;
            trwFolders.Enabled = lsvMails.Enabled = true;
        }

        #endregion

        #region Helpers

        private TreeNode FolderToNode(Folder folder)
        {
            var node = new TreeNode(folder.Name);
            node.Nodes.AddRange(folder.SubFolder.Select(FolderToNode).ToArray());
            node.Tag = folder;
            return node;
        }

        private void UpdateAttachmentIcons(IEnumerable<string> files)
        {
            foreach (Image image in istAttachments.Images)
                image.Dispose();
            istAttachments.Images.Clear();

            foreach (string file in files)
            {
                string key = file.Split('.').Last();
                if (!istAttachments.Images.ContainsKey(key))
                    try
                    {
                        istAttachments.Images.Add(file.Split('.').Last(), NativeMethods.GetSystemIcon(file));
                    }
                    catch {
                        
                    }
            }
        }

        #endregion

        private void lsvMails_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if(e.ItemIndex>=_messages.Count) return;
            var msg = _messages[e.ItemIndex];            
            e.Item = new ListViewItem(string.IsNullOrEmpty(msg.Subject)
                                          ? "[No subject]"
                                          : msg.Subject.Replace("\n", "").Replace("\t", ""));
        }

        private void FrmMainOrLsvMails_SizeChanged(object sender, EventArgs e)
        {
            AutoResizeMessageListViewColumn();
        }

        private void AutoResizeMessageListViewColumn()
        {
            var scrollBars = NativeMethods.GetVisibleScrollbars(lsvMails);
            clmMessages.Width = (scrollBars & ScrollBars.Vertical) == ScrollBars.Vertical
                                    ? lsvMails.Width - SystemInformation.VerticalScrollBarWidth
                                    : lsvMails.Width;
        }

        private void exportForReportToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (_selectedMessage == null) return;

            sfdMain.FileName = "message.report";

            if (sfdMain.ShowDialog() != DialogResult.OK) return;

            _selectedMessage.ExportForReport(sfdMain.FileName);

            var test = Message.FromReport(sfdMain.FileName);
            test.ToString();

        }

        #region Empty folder

        private void emptyFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lastClickedNode == null) return;
            if (
                MessageBox.Show("Do you really want to empty this folder?", "Empty folder", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) != DialogResult.Yes)
                return;


            trwFolders.Enabled = lsvMails.Enabled = false;

            (new Thread(EmptyFolder)).Start(lastClickedNode.Tag);
        }

        private void EmptyFolder(object folder)
        {
            try
            {

                var args = new ServerCallCompletedEventArgs((folder as Folder).EmptyFolder(), null, folder);
                Invoke(new EventHandler<ServerCallCompletedEventArgs>(EmptyFolderCompleted),
                       Program.ImapClient, args);
            }
            catch (Exception ex)
            {
                var args = new ServerCallCompletedEventArgs(false, ex, folder);
                Invoke(new EventHandler<ServerCallCompletedEventArgs>(EmptyFolderCompleted),
                       Program.ImapClient, args);
            }
        }

        private void EmptyFolderCompleted(object sender, ServerCallCompletedEventArgs e)
        {
            if (e.Result)
            {
                if (_selectedFolder == e.Arg)
                {
                    _messages.Clear();
                    lsvMails.VirtualListSize = 0;
                    lsvMails.Invalidate();
                }

            }
            else if (e.Exception != null)
            {
                using (var frm = new FrmError(e.Exception))
                    frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Failed to empty folder", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }

            trwFolders.Enabled = lsvMails.Enabled = true;
        }

        #endregion


        private void trwFolders_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            lastClickedNode = e.Node;
            if (e.Node.Tag is Folder && e.Button == MouseButtons.Right)
            {
                emptyFolderToolStripMenuItem.Text = string.Format("Empty \"{0}\" folder", e.Node.Text);
                mnuFolders.Show(trwFolders, e.Location);
            }
        }

        private void markAsReadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trwFolders.Enabled = lsvMails.Enabled = false;

            (new Thread(MarkSelectedMessageAsRead)).Start();
        }

        private void MarkSelectedMessageAsRead()
        {
            try
            {

                var args = new ServerCallCompletedEventArgs(_selectedMessage.SetFlag(ImapFlags.SEEN));
                Invoke(new EventHandler<ServerCallCompletedEventArgs>(MarkSelectedMessageAsReadCompleted),
                       Program.ImapClient, args);
            }
            catch (Exception ex)
            {
                var args = new ServerCallCompletedEventArgs(false, ex);
                Invoke(new EventHandler<ServerCallCompletedEventArgs>(MarkSelectedMessageAsReadCompleted),
                       Program.ImapClient, args);
            }
        }

        private void MarkSelectedMessageAsReadCompleted(object sender, ServerCallCompletedEventArgs e)
        {
            if (e.Result)
            {
                
            }
            else if (e.Exception != null)
            {
                using (var frm = new FrmError(e.Exception))
                    frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Failed to mark the message as read", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            
            trwFolders.Enabled = lsvMails.Enabled = true;
        }
 
    }
}