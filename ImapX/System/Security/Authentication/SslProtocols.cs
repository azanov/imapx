using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Security.Authentication
{
    [Flags]
    public enum SslProtocols
    {
        None = 0,
        Ssl2 = 12,
        Ssl3 = 48,
        Tls = 192,
        Default = 240,
    }
}
