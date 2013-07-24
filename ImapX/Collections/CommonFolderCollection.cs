using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImapX.Flags;

namespace ImapX.Collections
{
    public class CommonFolderCollection : FolderCollection
    {

        public Folder All { get; private set; }
        public Folder Archive { get; private set; }
        public Folder Inbox { get; private set; }
        public Folder Drafts { get; private set; }
        public Folder Important { get; private set; }
        public Folder Flagged { get; private set; }
        public Folder Junk { get; private set; }
        public Folder Sent { get; private set; }
        public Folder Trash { get; private set; }

        public CommonFolderCollection(ImapClient client) : base(client) { }

        internal void TryBind(ref Folder folder)
        {
            if (folder.Flags.Contains(FolderFlags.All) || folder.Flags.Contains(FolderFlags.XAllMail))
                All = folder;
            else if (folder.Flags.Contains(FolderFlags.Archive))
                Archive = folder;
            else if (folder.Name.ToUpper() == "INBOX" || folder.Flags.Contains(FolderFlags.XInbox))
                Inbox = folder;
            else if (folder.Flags.Contains(FolderFlags.Drafts))
                Drafts = folder;
            else if (folder.Flags.Contains(FolderFlags.XImportant))
                Important = folder;
            else if (folder.Flags.Contains(FolderFlags.Flagged) || folder.Flags.Contains(FolderFlags.XStarred))
                Flagged = folder;
            else if (folder.Flags.Contains(FolderFlags.Junk) || folder.Flags.Contains(FolderFlags.XSpam))
                Junk = folder;
            else if (folder.Flags.Contains(FolderFlags.Sent))
                Sent = folder;
            else if (folder.Flags.Contains(FolderFlags.Trash))
                Trash = folder;
        }



    }
}
