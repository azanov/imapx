using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImapX.Constants
{
    public sealed class ResponseType
    {
        public const string Ok = "OK";
        public const string No = "NO";
        public const string Bad = "BAD";
        public const string PreAuth = "PREAUTH";

        public const string ServerOk = "* OK";
        public const string ServerBad = "* BAD";
        public const string ServerNo = "* NO";
        public const string ServerPreAuth = "* PREAUTH";
        public const string Prefix = "*";
    }
}
