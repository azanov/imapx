using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImapX.Constants
{
    public class ResponseCode
    {
        public const string Alert = "ALERT";
        public const string BadCharset = "BADCHARSET";
        public const string Capability = "CAPABILITY";
        public const string Parse = "PARSE";
        public const string PermanentFlags = "PERMANENTFLAGS";
        public const string ReadOnly = "READ-ONLY";
        public const string ReadWrite = "READ-WRITE";
        public const string TryCreate = "TRYCREATE";
        public const string UIdNext = "UIDNEXT";
        public const string UIdValidity = "UIDVALIDITY";
        public const string Unseen = "UNSEEN";
        public const string CompressionActive = "COMPRESSIONACTIVE";
        public const string AuthenticationFailed = "AUTHENTICATIONFAILED";

        public const string HighestModSeq = "HIGHESTMODSEQ";
    }
}
