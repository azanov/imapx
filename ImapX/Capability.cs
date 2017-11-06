using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ImapX
{
    public class Capability
    {

        internal bool Loaded { get; set; }

        /// <summary>
        ///     Contains a list of all capabilities supported by the server
        /// </summary>
        public List<string> All { get; private set; }

        public bool IMAP4 { get; set; }

        public bool IMAP4rev1 { get; set; }

        /// <summary>
        ///     A list of additional authentication mechanisms the server supports
        /// </summary>
        public List<string> AuthenticationMechanisms { get; internal set; }

        /// <summary>
        ///     Gets whether the LOGIN command is disabled on the server
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc2595" />
        public bool LoginDisabled { get; internal set; }
        
        public bool StartTLS { get; internal set; }
        public bool Id { get; internal set; }
        public bool XList { get; private set; }

        public bool UTF8Accept { get; internal set; }

        public bool Enable { get; internal set; }

        public bool UIdPlus { get; set; }

        public bool CompressDeflate { get; set; }

        public bool ListStatus { get; set; }

        public bool SpecialUse { get; set; }

        public bool Language { get; set; }

        public long AppendLimit { get; set; }

        public bool Idle { get; set; }

        internal Capability()
        {
            All = new List<string>();
            AuthenticationMechanisms = new List<string>();
            AppendLimit = -1;
        }

        internal void Add(string value)
        {
            All.Add(value);

            if (value.StartsWith("AUTH="))
                AuthenticationMechanisms.Add(value.Substring(5));
            else if (value.StartsWith("APPENDLIMIT="))
            {
                long.TryParse(value.Substring(12), out long limit);
                AppendLimit = limit;
            }
            else
            {
                switch (value)
                {
                    case "IMAP4":
                        IMAP4 = true;
                        break;
                    case "IMAP4rev1":
                        IMAP4rev1 = true;
                        break;
                    case "LOGINDISABLED":
                        LoginDisabled = true;
                        break;
                    case "STARTTLS":
                        StartTLS = true;
                        break;
                    case "ID":
                        Id = true;
                        break;
                    case "XLIST":
                        XList = true;
                        break;
                    case "UTF8=ACCEPT":
                        UTF8Accept = true;
                        break;
                    case "ENABLE":
                        Enable = true;
                        break;
                    case "UIDPLUS":
                        UIdPlus = true;
                        break;
                    case "COMPRESS=DEFLATE":
                        CompressDeflate = true;
                        break;
                    case "LIST-STATUS":
                        ListStatus = true;
                        break;
                    case "SPECIAL-USE":
                        SpecialUse = true;
                        break;
                    case "IDLE":
                        Idle = true;
                        break;
                }
            }
        }
    }
}