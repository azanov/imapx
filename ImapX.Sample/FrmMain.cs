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

namespace ImapX.Sample
{
    public partial class FrmMain : Form
    {
        private List<Message> _messages;
        private Folder _selectedFolder;
        private Message _selectedMessage;

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
            if (lsvMails.SelectedItems.Count == 0) return;
            _selectedMessage = _messages[lsvMails.SelectedItems[0].Index];

            lblSubject.Text = string.IsNullOrWhiteSpace(_selectedMessage.Subject)
                                  ? "No subject"
                                  : _selectedMessage.Subject.Replace("\n", "").Replace("\t", "");

            lblTime.Text = _selectedMessage.Date.ToString();
            lblFrom.Text = string.Join("; ", _selectedMessage.From.Select(_ => _.ToString()));
            lblTo.Text = string.Join("; ", _selectedMessage.To.Select(_ => _.ToString()));

            bool isHtml;
            string body = _selectedMessage.GetDecodedBody(out isHtml);
            wbrMain.Document.OpenNew(true);


            lsvAttachments.Items.Clear();


            Directory.CreateDirectory(Path.Combine(Application.StartupPath, "tmp"));
            Directory.CreateDirectory(Path.Combine(Application.StartupPath, "tmp", _selectedMessage.MessageId.MD5()));

            var files = new List<string>();

            foreach (Attachment attachment in _selectedMessage.Attachments)
            {
                try
                {
                    string path = Path.Combine(Application.StartupPath, "tmp", _selectedMessage.MessageId.MD5(),
                                               attachment.FileName);
                    files.Add(path);
                    File.WriteAllBytes(path, attachment.FileData);
                }
                catch (Exception)
                {
                }
            }

            foreach (InlineAttachment inlineAttachment in _selectedMessage.InlineAttachments)
            {
                try
                {
                    string path = Path.Combine(Application.StartupPath, "tmp", _selectedMessage.MessageId.MD5(),
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
                _selectedMessage.Attachments.Where(_ => !string.IsNullOrWhiteSpace(_.FileName)).OrderBy(_ => _.FileName)
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
            Message msg = _messages[lsvMails.SelectedItems[0].Index];
            Attachment attachment =
                msg.Attachments.Where(_ => !string.IsNullOrWhiteSpace(_.FileName)).Skip(
                    lsvAttachments.SelectedItems[0].Index).Take(1).FirstOrDefault();
            Process.Start(Path.Combine(Application.StartupPath, "tmp", msg.MessageId.MD5(), attachment.FileName));
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lsvAttachments.SelectedItems.Count == 0) return;
            Message msg = _messages[lsvMails.SelectedItems[0].Index];
            Attachment attachment =
                msg.Attachments.Where(_ => !string.IsNullOrWhiteSpace(_.FileName)).Skip(
                    lsvAttachments.SelectedItems[0].Index).Take(1).FirstOrDefault();
            sfdMain.FileName = attachment.FileName;

            if (sfdMain.ShowDialog() != DialogResult.OK) return;

            File.Copy(Path.Combine(Application.StartupPath, "tmp", msg.MessageId.MD5(), attachment.FileName),
                      sfdMain.FileName);
        }

        private void mnuMessages_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = lsvMails.SelectedItems.Count == 0;
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
            lsvMails.Items.Clear();
            pnlInfo.Visible = wbrMain.Visible = pnlAttachments.Visible = false;
            pgbFetchMails.Visible = true;

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
                lsvMails.Items.AddRange(
                    _messages.Select(
                        _ =>
                        new ListViewItem(string.IsNullOrWhiteSpace(_.Subject)
                                             ? "[No subject]"
                                             : _.Subject.Replace("\n", "").Replace("\t", ""))).ToArray());
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
                _messages.RemoveAt(lsvMails.SelectedItems[0].Index);
                lsvMails.Items.RemoveAt(lsvMails.SelectedItems[0].Index);
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
                _messages.RemoveAt(lsvMails.SelectedItems[0].Index);
                lsvMails.Items.RemoveAt(lsvMails.SelectedItems[0].Index);
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
                    istAttachments.Images.Add(file.Split('.').Last(), NativeMethods.GetSystemIcon(file));
            }
        }

        #endregion
    }
}