using System.ComponentModel;

namespace ImapX.Enums
{
    [DefaultValue(HeadersOnly)]
    public enum MessageFetchMode
    {
        Full,
        HeadersOnly,
        NoAttachments
    }
}