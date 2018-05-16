using ImapX.Sample.Controls;

namespace ImapX.Sample
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lnkArchive = new ImapX.Sample.Controls.FolderLinkLabel();
            this.lnkAllMails = new ImapX.Sample.Controls.FolderLinkLabel();
            this.lnkTrash = new ImapX.Sample.Controls.FolderLinkLabel();
            this.lnkJunk = new ImapX.Sample.Controls.FolderLinkLabel();
            this.lnkFlagged = new ImapX.Sample.Controls.FolderLinkLabel();
            this.lnkImportant = new ImapX.Sample.Controls.FolderLinkLabel();
            this.lnkDrafts = new ImapX.Sample.Controls.FolderLinkLabel();
            this.lnkSent = new ImapX.Sample.Controls.FolderLinkLabel();
            this.lnkInbox = new ImapX.Sample.Controls.FolderLinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(5, 5);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackgroundImage = global::ImapX.Sample.Properties.Resources.pattern8;
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(829, 552);
            this.splitContainer1.SplitterDistance = 200;
            this.splitContainer1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.lnkArchive);
            this.panel1.Controls.Add(this.lnkAllMails);
            this.panel1.Controls.Add(this.lnkTrash);
            this.panel1.Controls.Add(this.lnkJunk);
            this.panel1.Controls.Add(this.lnkFlagged);
            this.panel1.Controls.Add(this.lnkImportant);
            this.panel1.Controls.Add(this.lnkDrafts);
            this.panel1.Controls.Add(this.lnkSent);
            this.panel1.Controls.Add(this.lnkInbox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(15);
            this.panel1.Size = new System.Drawing.Size(200, 275);
            this.panel1.TabIndex = 0;
            // 
            // lnkArchive
            // 
            this.lnkArchive.Dock = System.Windows.Forms.DockStyle.Top;
            this.lnkArchive.Folder = null;
            this.lnkArchive.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkArchive.ForeColor = System.Drawing.Color.White;
            this.lnkArchive.Image = global::ImapX.Sample.Properties.Resources.archive;
            this.lnkArchive.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkArchive.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkArchive.LinkColor = System.Drawing.Color.White;
            this.lnkArchive.Location = new System.Drawing.Point(15, 231);
            this.lnkArchive.Name = "lnkArchive";
            this.lnkArchive.Padding = new System.Windows.Forms.Padding(24, 4, 5, 6);
            this.lnkArchive.Size = new System.Drawing.Size(170, 27);
            this.lnkArchive.TabIndex = 23;
            this.lnkArchive.TabStop = true;
            this.lnkArchive.Text = "lnkArchive";
            this.lnkArchive.Visible = false;
            this.lnkArchive.Click += new System.EventHandler(this.lnkFolder_Click);
            // 
            // lnkAllMails
            // 
            this.lnkAllMails.Dock = System.Windows.Forms.DockStyle.Top;
            this.lnkAllMails.Folder = null;
            this.lnkAllMails.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkAllMails.ForeColor = System.Drawing.Color.White;
            this.lnkAllMails.Image = global::ImapX.Sample.Properties.Resources.mails;
            this.lnkAllMails.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkAllMails.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkAllMails.LinkColor = System.Drawing.Color.White;
            this.lnkAllMails.Location = new System.Drawing.Point(15, 204);
            this.lnkAllMails.Name = "lnkAllMails";
            this.lnkAllMails.Padding = new System.Windows.Forms.Padding(24, 4, 5, 6);
            this.lnkAllMails.Size = new System.Drawing.Size(170, 27);
            this.lnkAllMails.TabIndex = 22;
            this.lnkAllMails.TabStop = true;
            this.lnkAllMails.Text = "lnkAllMails";
            this.lnkAllMails.Visible = false;
            this.lnkAllMails.Click += new System.EventHandler(this.lnkFolder_Click);
            // 
            // lnkTrash
            // 
            this.lnkTrash.Dock = System.Windows.Forms.DockStyle.Top;
            this.lnkTrash.Folder = null;
            this.lnkTrash.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkTrash.ForeColor = System.Drawing.Color.White;
            this.lnkTrash.Image = global::ImapX.Sample.Properties.Resources.empty_trash;
            this.lnkTrash.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkTrash.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkTrash.LinkColor = System.Drawing.Color.White;
            this.lnkTrash.Location = new System.Drawing.Point(15, 177);
            this.lnkTrash.Name = "lnkTrash";
            this.lnkTrash.Padding = new System.Windows.Forms.Padding(24, 4, 5, 6);
            this.lnkTrash.Size = new System.Drawing.Size(170, 27);
            this.lnkTrash.TabIndex = 21;
            this.lnkTrash.TabStop = true;
            this.lnkTrash.Text = "lnkTrash";
            this.lnkTrash.Visible = false;
            this.lnkTrash.Click += new System.EventHandler(this.lnkFolder_Click);
            // 
            // lnkJunk
            // 
            this.lnkJunk.Dock = System.Windows.Forms.DockStyle.Top;
            this.lnkJunk.Folder = null;
            this.lnkJunk.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkJunk.ForeColor = System.Drawing.Color.White;
            this.lnkJunk.Image = global::ImapX.Sample.Properties.Resources.junk;
            this.lnkJunk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkJunk.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkJunk.LinkColor = System.Drawing.Color.White;
            this.lnkJunk.Location = new System.Drawing.Point(15, 150);
            this.lnkJunk.Name = "lnkJunk";
            this.lnkJunk.Padding = new System.Windows.Forms.Padding(24, 4, 5, 6);
            this.lnkJunk.Size = new System.Drawing.Size(170, 27);
            this.lnkJunk.TabIndex = 20;
            this.lnkJunk.TabStop = true;
            this.lnkJunk.Text = "lnkJunk";
            this.lnkJunk.Visible = false;
            this.lnkJunk.Click += new System.EventHandler(this.lnkFolder_Click);
            // 
            // lnkFlagged
            // 
            this.lnkFlagged.Dock = System.Windows.Forms.DockStyle.Top;
            this.lnkFlagged.Folder = null;
            this.lnkFlagged.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkFlagged.ForeColor = System.Drawing.Color.White;
            this.lnkFlagged.Image = global::ImapX.Sample.Properties.Resources.flag;
            this.lnkFlagged.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkFlagged.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkFlagged.LinkColor = System.Drawing.Color.White;
            this.lnkFlagged.Location = new System.Drawing.Point(15, 123);
            this.lnkFlagged.Name = "lnkFlagged";
            this.lnkFlagged.Padding = new System.Windows.Forms.Padding(24, 4, 5, 6);
            this.lnkFlagged.Size = new System.Drawing.Size(170, 27);
            this.lnkFlagged.TabIndex = 19;
            this.lnkFlagged.TabStop = true;
            this.lnkFlagged.Text = "lnkFlagged";
            this.lnkFlagged.Visible = false;
            this.lnkFlagged.Click += new System.EventHandler(this.lnkFolder_Click);
            // 
            // lnkImportant
            // 
            this.lnkImportant.Dock = System.Windows.Forms.DockStyle.Top;
            this.lnkImportant.Folder = null;
            this.lnkImportant.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkImportant.ForeColor = System.Drawing.Color.White;
            this.lnkImportant.Image = global::ImapX.Sample.Properties.Resources.important;
            this.lnkImportant.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkImportant.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkImportant.LinkColor = System.Drawing.Color.White;
            this.lnkImportant.Location = new System.Drawing.Point(15, 96);
            this.lnkImportant.Name = "lnkImportant";
            this.lnkImportant.Padding = new System.Windows.Forms.Padding(24, 4, 5, 6);
            this.lnkImportant.Size = new System.Drawing.Size(170, 27);
            this.lnkImportant.TabIndex = 18;
            this.lnkImportant.TabStop = true;
            this.lnkImportant.Text = "lnkImportant";
            this.lnkImportant.Visible = false;
            this.lnkImportant.Click += new System.EventHandler(this.lnkFolder_Click);
            // 
            // lnkDrafts
            // 
            this.lnkDrafts.Dock = System.Windows.Forms.DockStyle.Top;
            this.lnkDrafts.Folder = null;
            this.lnkDrafts.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkDrafts.ForeColor = System.Drawing.Color.White;
            this.lnkDrafts.Image = global::ImapX.Sample.Properties.Resources.pencil;
            this.lnkDrafts.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkDrafts.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkDrafts.LinkColor = System.Drawing.Color.White;
            this.lnkDrafts.Location = new System.Drawing.Point(15, 69);
            this.lnkDrafts.Name = "lnkDrafts";
            this.lnkDrafts.Padding = new System.Windows.Forms.Padding(24, 4, 5, 6);
            this.lnkDrafts.Size = new System.Drawing.Size(170, 27);
            this.lnkDrafts.TabIndex = 17;
            this.lnkDrafts.TabStop = true;
            this.lnkDrafts.Text = "lnkDrafts";
            this.lnkDrafts.Visible = false;
            this.lnkDrafts.Click += new System.EventHandler(this.lnkFolder_Click);
            // 
            // lnkSent
            // 
            this.lnkSent.Dock = System.Windows.Forms.DockStyle.Top;
            this.lnkSent.Folder = null;
            this.lnkSent.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkSent.ForeColor = System.Drawing.Color.White;
            this.lnkSent.Image = global::ImapX.Sample.Properties.Resources.paper_plane;
            this.lnkSent.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkSent.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkSent.LinkColor = System.Drawing.Color.White;
            this.lnkSent.Location = new System.Drawing.Point(15, 42);
            this.lnkSent.Name = "lnkSent";
            this.lnkSent.Padding = new System.Windows.Forms.Padding(24, 4, 5, 6);
            this.lnkSent.Size = new System.Drawing.Size(170, 27);
            this.lnkSent.TabIndex = 16;
            this.lnkSent.TabStop = true;
            this.lnkSent.Text = "lnkSent";
            this.lnkSent.Visible = false;
            this.lnkSent.Click += new System.EventHandler(this.lnkFolder_Click);
            // 
            // lnkInbox
            // 
            this.lnkInbox.Dock = System.Windows.Forms.DockStyle.Top;
            this.lnkInbox.Folder = null;
            this.lnkInbox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkInbox.ForeColor = System.Drawing.Color.White;
            this.lnkInbox.Image = global::ImapX.Sample.Properties.Resources.inbox;
            this.lnkInbox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkInbox.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkInbox.LinkColor = System.Drawing.Color.White;
            this.lnkInbox.Location = new System.Drawing.Point(15, 15);
            this.lnkInbox.Name = "lnkInbox";
            this.lnkInbox.Padding = new System.Windows.Forms.Padding(24, 4, 5, 6);
            this.lnkInbox.Size = new System.Drawing.Size(170, 27);
            this.lnkInbox.TabIndex = 15;
            this.lnkInbox.TabStop = true;
            this.lnkInbox.Text = "lnkInbox";
            this.lnkInbox.Visible = false;
            this.lnkInbox.Click += new System.EventHandler(this.lnkFolder_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::ImapX.Sample.Properties.Resources.pattern8;
            this.ClientSize = new System.Drawing.Size(839, 562);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmMain";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.Shown += new System.EventHandler(this.FrmMain_Shown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private FolderLinkLabel lnkTrash;
        private FolderLinkLabel lnkJunk;
        private FolderLinkLabel lnkFlagged;
        private FolderLinkLabel lnkImportant;
        private FolderLinkLabel lnkDrafts;
        private FolderLinkLabel lnkSent;
        private FolderLinkLabel lnkInbox;
        private FolderLinkLabel lnkArchive;
        private FolderLinkLabel lnkAllMails;
    }
}

