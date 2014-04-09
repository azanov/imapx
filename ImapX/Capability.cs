using System.Collections.Generic;
using System.Linq;

namespace ImapX
{
    public class Capability
    {
        public Capability(string commandResult)
        {
            Update(commandResult);
        }

        /// <summary>
        ///     Gets whether the server supports the ACL extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc4314" />
        public bool Acl { get; private set; }

        /// <summary>
        ///     Contains a list of all capabilities supported by the server
        /// </summary>
        public string[] All { get; private set; }

        /// <summary>
        ///     A list of additional authentication mechanisms the server supports
        /// </summary>
        public string[] AuthenticationMechanisms { get; private set; }

        /// <summary>
        ///     Gets whether the server supports the BINARY extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc3516" />
        public bool Binary { get; private set; }

        /// <summary>
        ///     Gets whether the server supports the CATENATE extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc4469" />
        public bool Catenate { get; private set; }

        /// <summary>
        ///     Gets whether the server supports the CHILD extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc3348" />
        public bool Children { get; private set; }

        /// <summary>
        ///     Contains a list of compression mechanisms supported by the server
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc4978" />
        public string[] CompressionMechanisms { get; private set; }

        /// <summary>
        ///     Gets whether the server supports the CONDSTORE extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc4551" />
        public bool CondStore { get; private set; }

        /// <summary>
        ///     Contains a list of contexts supported by the server
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc5267" />
        public string[] Contexts { get; private set; }

        /// <summary>
        ///     Gets whether the server supports the CONVERT extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc5259" />
        public bool Convert { get; private set; }

        /// <summary>
        ///     Gets whether the server supports the CREATE-SPECIAL-USE extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc6154" />
        public bool CreateSpecialUse { get; private set; }

        /// <summary>
        ///     Gets whether the server supports the ENABLE extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc5161" />
        public bool Enable { get; private set; }

        /// <summary>
        ///     Gets whether the server supports the ESEARCH extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc4731" />
        public bool ESearch { get; private set; }

        /// <summary>
        ///     Gets whether the server supports the ESORT extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc5267" />
        public bool ESort { get; private set; }

        /// <summary>
        ///     Gets whether the server supports the FILTERS extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc5466" />
        public bool Filters { get; private set; }

        /// <summary>
        ///     Gets whether the server supports the ID extension
        /// </summary>
        public bool Id { get; private set; }

        /// <summary>
        ///     Gets whether the server supports the IDLE extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc2177" />
        public bool Idle { get; private set; }

        /// <summary>
        ///     Gets whether the LOGIN command is disabled on the server
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc2595" />
        public bool LoginDisabled { get; private set; }

        /// <summary>
        ///     Gets whether the METADATA extension is supported
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc5464" />
        public bool Metadata { get; private set; }

        /// <summary>
        ///     Gets whether the server supports the NAMESPACE extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc2342" />
        public bool Namespace { get; private set; }

        /// <summary>
        ///     Gets whether the server supports authentication through OAuth
        /// </summary>
        public bool XoAuth { get; private set; }

        /// <summary>
        ///     Gets whether the server supports authentication through OAuth2
        /// </summary>
        public bool XoAuth2 { get; private set; }

        /// <summary>
        ///     Gets whether the server supports the QUOTA extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc2087" />
        public bool Quota { get; private set; }

        /// <summary>
        ///     Gets whether the server supports the UNSELECT extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc3691" />
        public bool Unselect { get; private set; }

        public bool XList { get; private set; }

        public bool XGMExt1 { get; private set; }

        internal void Update(string commandResult)
        {
            if (string.IsNullOrEmpty(commandResult))
                return;

            commandResult = commandResult.Replace("* CAPABILITY IMAP4rev1 ", "");

            All = (All ?? new string[0]).Concat(commandResult.Split(' ').Where(_ => !string.IsNullOrEmpty(_.Trim()))).Distinct().ToArray();

            AuthenticationMechanisms = (AuthenticationMechanisms ?? new string[0]).Concat(All.Where(_ => _.StartsWith("AUTH="))
                .Select(_ => _.Substring(5, _.Length - 5))).Distinct().ToArray();

            CompressionMechanisms = (CompressionMechanisms ?? new string[0]).Concat(All.Where(_ => _.StartsWith("COMPRESS="))
                .Select(_ => _.Substring(9, _.Length - 9))).Distinct().ToArray();

            Contexts = (Contexts ?? new string[0]).Concat(All.Where(_ => _.StartsWith("CONTEXT="))
                .Select(_ => _.Substring(8, _.Length - 8))).Distinct().ToArray();

            foreach (string s in All)
            {
                switch (s)
                {
                    case "X-GM-EXT-1":
                        XGMExt1 = true;
                        break;
                    case "XLIST":
                        XList = true;
                        break;
                    case "UNSELECT":
                        Unselect = true;
                        break;
                    case "QUOTA":
                        Quota = true;
                        break;
                    case "AUTH=XOAUTH2":
                        XoAuth2 = true;
                        break;
                    case "AUTH=XOAUTH":
                        XoAuth = true;
                        break;
                    case "NAMESPACE":
                        Namespace = true;
                        break;
                    case "METADATA":
                        Metadata = true;
                        break;
                    case "LOGINDISABLED":
                        LoginDisabled = true;
                        break;
                    case "IDLE":
                        Idle = true;
                        break;
                    case "ID":
                        Id = true;
                        break;
                    case "FILTERS":
                        Filters = true;
                        break;
                    case "ESORT":
                        ESort = true;
                        break;
                    case "ESEARCH":
                        ESearch = true;
                        break;
                    case "ENABLE":
                        Enable = true;
                        break;
                    case "CREATE-SPECIAL-USE":
                        CreateSpecialUse = true;
                        break;
                    case "CONVERT":
                        Convert = true;
                        break;
                    case "CONDSTORE":
                        CondStore = true;
                        break;
                    case "CHILDREN":
                        Children = true;
                        break;
                    case "CATENATE":
                        Catenate = true;
                        break;
                    case "BINARY":
                        Binary = true;
                        break;
                    case "ACL":
                        Acl = true;
                        break;
                }
            }
        }
    }
}