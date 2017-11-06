using ImapX.Commands;
using ImapX.Extensions;
using System;
using System.Linq;
using System.Text;

namespace ImapX.Authentication
{
    /// <summary>
    /// Credentials used for PLAIN authentication or the LOGIN command
    /// </summary>
    public class PlainCredentials : ImapCredentials
    {
        /// <summary>
        /// The login name
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// The password
        /// </summary>
        public string Password { get; set; }

        public PlainCredentials(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public override bool IsSupported(Capability capabilities)
        {
            return capabilities.AuthenticationMechanisms.Contains("PLAIN") || 
                capabilities.AuthenticationMechanisms.Contains("LOGIN") || !capabilities.LoginDisabled;
        }

        public override ImapCommand ToCommand(ImapBase imapBase, long id, Capability capabilities)
        {
            if (capabilities.AuthenticationMechanisms.Contains("PLAIN"))
                return new AuthenticatePlainCommand(imapBase, id, this);

            if (capabilities.AuthenticationMechanisms.Contains("LOGIN"))
                return new AuthenticateLoginCommand(imapBase, id, this);

            return new LoginCommand(imapBase, id, Login, Password);
        }
    }
}