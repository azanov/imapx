using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImapX.Enums
{
    [Flags]
    public enum MessageFetchState : int
    {
        None = 1,
        Headers = 2,
        Body = 4
    }
}
