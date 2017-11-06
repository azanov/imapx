using ImapX.Commands;
using ImapX.Exceptions;
using System;
using System.IO;
using System.Text;

namespace ImapX.Authentication
{
    /// <summary>
    /// Credentials used for OAuth2 authentication
    /// </summary>
    public class OAuth2Credentials : ImapCredentials
    {
        public OAuth2Credentials(string login, string authToken, string vendor = null)
        {
            Login = login;
            AuthToken = authToken;
            Vendor = vendor;
        }

        /// <summary>
        ///     The login name
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        ///     The auth token
        /// </summary>
        public string AuthToken { get; set; }

        /// <summary>
        /// The vendor
        /// </summary>
        public string Vendor { get; set; }

        public override bool IsSupported(Capability capabilities)
        {
            return capabilities.AuthenticationMechanisms.Contains("XOAUTH2");
        }

        public override ImapCommand ToCommand(ImapBase imapBase, long id, Capability capabilities)
        {
            return new AuthenticateOAuth2Command(imapBase, id, this);
        }
        
    }

}