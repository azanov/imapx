using System;
using System.Linq;
using System.Text;
using ImapX.Constants;
using ImapX.EncodingHelpers;

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
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                throw new ArgumentException("Login and password cannot be empty");

            Login = login;
            Password = password;
        }

        public override string ToCommand(Capability capabilities)
        {
            if (!IsSupported(capabilities))
                throw new NotSupportedException("The selected authentication mechanism is not supported");

            return capabilities.LoginDisabled ? string.Format(ImapCommands.Authenticate + "\n{1}\n{2}", "PLAIN", Base64.ToBase64(Encoding.UTF8.GetBytes(Login)), Base64.ToBase64(Encoding.UTF8.GetBytes(Password))) : string.Format(ImapCommands.Login, Login, Password);
        }

        public override bool IsSupported(Capability capabilities)
        {
            return capabilities != null && (!capabilities.LoginDisabled || capabilities.AuthenticationMechanisms.Contains("PLAIN"));
        }

        public override void ProcessCommandResult(string data)
        {
            
        }

        public override byte[] AppendCommandData(string serverResponse)
        {
            return Encoding.UTF8.GetBytes(Environment.NewLine);
        }
    }
}