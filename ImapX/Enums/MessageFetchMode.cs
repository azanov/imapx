using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ImapX.Enums
{
    [DefaultValue(MessageFetchMode.HeadersOnly)]
    public enum MessageFetchMode
    {
        Full,
        HeadersOnly,
        NoAttachments
    }
}
