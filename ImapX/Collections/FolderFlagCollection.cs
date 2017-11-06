using System.Collections.Generic;

namespace ImapX.Collections
{
    public class FolderFlagCollection : ImapObjectCollection<string>
    {
        protected Folder _folder;

        public FolderFlagCollection(ImapClient client, Folder folder)
            : base(client)
        {
            _folder = folder;
        }

        public FolderFlagCollection(IEnumerable<string> items, ImapClient client, Folder folder)
            : base(client, items)
        {
            _folder = folder;
        }
    }
}
