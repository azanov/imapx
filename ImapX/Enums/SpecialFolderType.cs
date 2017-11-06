using System.ComponentModel;

namespace ImapX.Enums
{
    [DefaultValue(None)]
    public enum SpecialFolderType
    {
        None,
        All,
        Archive,
        Inbox,
        Drafts,
        Important,
        Flagged,
        Junk,
        Sent,
        Trash,
    }
}
