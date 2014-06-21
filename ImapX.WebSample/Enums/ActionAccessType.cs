using System;
using System.ComponentModel;

namespace ImapX.WebSample.Enums
{
    [Flags, DefaultValue(Public)]
    public enum ActionAccessType : int
    {
        Public = 1,
        NotAuthenticatedOnly = 2,
        Authenticated = 4
    }
}
