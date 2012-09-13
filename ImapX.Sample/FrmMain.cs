using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ImapX.Sample.Native;

namespace ImapX.Sample
{
    public partial class FrmMain : Form
    {
        private IEnumerable<Message> _messages;
        private string _result;

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
                    lstFolders.DataSource = Program.ImapClient.Folders.Select(_ => _.Name).ToArray();
                }
                else
                    Application.Exit();
            }
        }

        private void bgwMain_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                e.Result = false;
                var folderIndex = (int) e.Argument;
                _messages = Program.ImapClient.Folders[folderIndex].Search("ALL", true).OrderByDescending(_ => _.Date);
                e.Result = true;
            }
            catch (Exception ex)
            {
                _result = ex.ToString();
            }
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


        private void bgwMain_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (InvokeRequired)
                Invoke(new RunWorkerCompletedEventHandler(bgwMain_RunWorkerCompleted), new {sender, e});
            else
            {
                if ((bool) e.Result)
                {
                    lstMails.Items.AddRange(
                        _messages.Select(
                            _ =>
                            string.IsNullOrWhiteSpace(_.Subject)
                                ? "[No subject]"
                                : _.Subject.Replace("\n", "").Replace("\t", "")).ToArray
                            ());
                }
                else
                {
                    MessageBox.Show(_result, "Error", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
                pgbFetchMails.Visible = false;
                lstFolders.Enabled = lstMails.Enabled = true;
            }
        }

        private void lstFolders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFolders.SelectedIndex == -1) return;
            lstFolders.Enabled = lstMails.Enabled = false;
            lstMails.Items.Clear();
            pnlInfo.Visible = wbrMain.Visible = pnlAttachments.Visible = false;
            pgbFetchMails.Visible = true;
            bgwMain.RunWorkerAsync(lstFolders.SelectedIndex);
        }

        private void lsvMails_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstMails.SelectedItems.Count == 0) return;
            Message msg = _messages.ElementAt(lstMails.SelectedIndex);

            lblSubject.Text = string.IsNullOrWhiteSpace(msg.Subject)
                                  ? "No subject"
                                  : msg.Subject.Replace("\n", "").Replace("\t", "");

            lblTime.Text = msg.Date.ToString();
            lblFrom.Text = string.Join("; ", msg.From.Select(_ => _.ToString()));
            lblTo.Text = string.Join("; ", msg.To.Select(_ => _.ToString()));
            bool isHtml;
            string body = msg.GetDecodedBody(out isHtml);
            wbrMain.Document.OpenNew(true);


            lsvAttachments.Items.Clear();


            Directory.CreateDirectory(Path.Combine(Application.StartupPath, "tmp"));
            Directory.CreateDirectory(Path.Combine(Application.StartupPath, "tmp", msg.MessageId.MD5()));

            var files = new List<string>();

            foreach (Attachment attachment in msg.Attachments)
            {
                try
                {
                    string path = Path.Combine(Application.StartupPath, "tmp", msg.MessageId.MD5(), attachment.FileName);
                    files.Add(path);
                    File.WriteAllBytes(path, attachment.FileData);
                }
                catch (Exception)
                {
                }
            }

            foreach (InlineAttachment inlineAttachment in msg.InlineAttachments)
            {
                try
                {
                    string path = Path.Combine(Application.StartupPath, "tmp", msg.MessageId.MD5(),
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
                msg.Attachments.Where(_ => !string.IsNullOrWhiteSpace(_.FileName)).OrderBy(_ => _.FileName).Select(
                    _ => new ListViewItem(_.FileName)
                             {
                                 ImageKey = _.FileName.Split('.').Last()
                             }).ToArray());

            pnlInfo.Visible = true;
            wbrMain.Visible = true;
            pnlAttachments.Visible = msg.Attachments.Any();
        }

        private void lsvAttachments_MouseClick(object sender, MouseEventArgs e)
        {
            if (lsvAttachments.SelectedItems.Count == 0 || e.Button != MouseButtons.Right) return;
            mnuAttachment.Show(lsvAttachments, e.Location);
        }

        private void lsvAttachments_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lsvAttachments.SelectedItems.Count == 0 || e.Button != MouseButtons.Left) return;
            openToolStripMenuItem_Click(null, null);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lsvAttachments.SelectedItems.Count == 0) return;
            Message msg = _messages.ElementAt(lstMails.SelectedIndex);
            Attachment attachment =
                msg.Attachments.Where(_ => !string.IsNullOrWhiteSpace(_.FileName)).Skip(
                    lsvAttachments.SelectedItems[0].Index).Take(1).FirstOrDefault();
            Process.Start(Path.Combine(Application.StartupPath, "tmp", msg.MessageId.MD5(), attachment.FileName));
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lsvAttachments.SelectedItems.Count == 0) return;
            Message msg = _messages.ElementAt(lstMails.SelectedIndex);
            Attachment attachment =
                msg.Attachments.Where(_ => !string.IsNullOrWhiteSpace(_.FileName)).Skip(
                    lsvAttachments.SelectedItems[0].Index).Take(1).FirstOrDefault();
            sfdMain.FileName = attachment.FileName;

            if (sfdMain.ShowDialog() != DialogResult.OK) return;

            File.Copy(Path.Combine(Application.StartupPath, "tmp", msg.MessageId.MD5(), attachment.FileName),
                      sfdMain.FileName);
        }
    }
}