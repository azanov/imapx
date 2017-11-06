using ImapX.Enums;

namespace ImapX.Commands
{
    public class XListCommand : ListCommand
    {
        public XListCommand(ImapClient client, long id, Folder parentFolder = null, FolderTreeBrowseMode mode = FolderTreeBrowseMode.Lazy)
            : base(client, id, parentFolder, mode, "XLIST")
        {
        }

        internal override string ResponseToken => "XLIST";
    }
}
