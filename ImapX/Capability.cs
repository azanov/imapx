using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImapX
{
    public class Capability
    {

        /// <summary>
        /// Gets whether the server supports the ACL extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc4314"/>
        public bool Acl
        {
            get
            {
                return All.Contains("ACL");
            }
        }

        /// <summary>
        /// Contains a list of all capabilities supported by the server
        /// </summary>
        public IEnumerable<string> All { get; private set; }

        /// <summary>
        /// A list of additional authentication mechanisms the server supports
        /// </summary>
        public IEnumerable<string> AuthenticationMechanisms { get; private set; }

        /// <summary>
        /// Gets whether the server supports the BINARY extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc3516"/>
        public bool Binary
        {
            get
            {
                return All.Contains("BINARY");
            }
        }

        /// <summary>
        /// Gets whether the server supports the CATENATE extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc4469"/>
        public bool Catenate
        {
            get
            {
                return All.Contains("CATENATE");
            }
        }

        /// <summary>
        /// Gets whether the server supports the CHILD extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc3348"/>
        public bool Children
        {
            get
            {
                return All.Contains("CHILDREN");
            }
        }

        /// <summary>
        /// Contains a list of compression mechanisms supported by the server
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc4978"/>
        public IEnumerable<string> CompressionMechanisms { get; private set; }

        /// <summary>
        /// Gets whether the server supports the CONDSTORE extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc4551"/>
        public bool CondStore
        {
            get
            {
                return All.Contains("CONDSTORE");
            }
        }

        /// <summary>
        /// Contains a list of contexts supported by the server
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc5267"/>
        public IEnumerable<string> Contexts { get; private set; }

        /// <summary>
        /// Gets whether the server supports the CONVERT extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc5259"/>
        public bool Convert
        {
            get
            {
                return All.Contains("CONVERT");
            }
        }

        /// <summary>
        /// Gets whether the server supports the CREATE-SPECIAL-USE extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc6154"/>
        public bool CreateSpecialUse
        {
            get
            {
                return All.Contains("CREATE-SPECIAL-USE");
            }
        }

        /// <summary>
        /// Gets whether the server supports the ENABLE extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc5161"/>
        public bool Enable
        {
            get
            {
                return All.Contains("ENABLE");
            }
        }

        /// <summary>
        /// Gets whether the server supports the ESEARCH extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc4731"/>
        public bool ESearch
        {
            get
            {
                return All.Contains("ESEARCH");
            }
        }

        /// <summary>
        /// Gets whether the server supports the ESORT extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc5267"/>
        public bool ESort
        {
            get
            {
                return All.Contains("ESORT");
            }
        }

        /// <summary>
        /// Gets whether the server supports the FILTERS extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc5466"/>
        public bool Filters
        {
            get
            {
                return All.Contains("FILTERS");
            }
        }

        /// <summary>
        /// Gets whether the server supports the I18NLEVEL extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc5255#section-4"/>
        public int? I18NLevel
        {
            get
            {
                try
                {
                    var tmp = All.FirstOrDefault(_ => _.StartsWith("I18NLEVEL="));
                    return tmp == null ? null : (int?)int.Parse(tmp.Substring(10, tmp.Length - 10));
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets whether the server supports the ID extension
        /// </summary>
        public bool Id
        {
            get
            {
                return All.Contains("ID");
            }
        }

        /// <summary>
        /// Gets whether the server supports the IDLE extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc2177"/>
        public bool Idle
        {
            get
            {
                return All.Contains("IDLE");
            }
        }

        /// <summary>
        /// Gets whether the LOGIN command is disabled on the server
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc2595"/>
        public bool LoginDisabled
        {
            get
            {
                return All.Contains("LOGINDISABLED");
            }
        }

        /// <summary>
        /// Gets whether the METADATA extension is supported
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc5464"/>
        public bool Metadata
        {
            get
            {
                return All.Contains("METADATA");
            }
        }

        /// <summary>
        /// Gets whether the server supports the NAMESPACE extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc2342"/>
        public bool Namespace
        {
            get
            {
                return All.Contains("NAMESPACE");
            }
        }

        /// <summary>
        /// Gets whether the server supports authentication through OAuth
        /// </summary>
        public bool XOAuth
        {
            get
            {
                return AuthenticationMechanisms.Contains("XOAUTH");
            }
        }

        /// <summary>
        /// Gets whether the server supports authentication through OAuth2
        /// </summary>
        public bool XOAuth2
        {
            get
            {
                return AuthenticationMechanisms.Contains("XOAUTH2");
            }
        }

        /// <summary>
        /// Gets whether the server supports the QUOTA extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc2087"/>
        public bool Quota
        {
            get
            {
                return All.Contains("QUOTA");
            }
        }

        /// <summary>
        /// Gets whether the server supports the UNSELECT extension
        /// </summary>
        /// <see cref="http://tools.ietf.org/html/rfc3691"/>
        public bool Unselect
        {
            get
            {
                return All.Contains("UNSELECT");
            }
        }

        public bool XList
        {
            get
            {
                return All.Contains("XLIST");
            }
        }

        public bool XGMExt1
        {
            get
            {
                return All.Contains("X-GM-EXT-1");
            }
        }

        // TODO: update capabilities after login

        public Capability(string commandResult)
        {
            commandResult = commandResult.Replace("* CAPABILITY IMAP4rev1 ", "");

            All = commandResult.Split(' ').Where(_ => !string.IsNullOrEmpty(_.Trim()));

            AuthenticationMechanisms = All.Where(_ => _.StartsWith("AUTH="))
                                          .Select(_ => _.Substring(5, _.Length - 5));

            CompressionMechanisms = All.Where(_ => _.StartsWith("COMPRESS="))
                                          .Select(_ => _.Substring(10, _.Length - 10));

            Contexts = All.Where(_ => _.StartsWith("CONTEXT="))
                                          .Select(_ => _.Substring(8, _.Length - 8));


        }

    }
}
