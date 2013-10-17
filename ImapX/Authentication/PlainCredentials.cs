using System;
using System.Linq;
using ImapX.Constants;

namespace ImapX.Authentication
{
    /// <summary>
    /// Credentials used for PLAIN authentication or the LOGIN command
    /// </summary>
    public class PlainCredentials : IImapCredentials
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

        public string ToCommand(Capability capabilities)
        {
            if (!IsSupported(capabilities))
                throw new NotSupportedException("The selected authentication mechanism is not supported");

            return capabilities.LoginDisabled ? string.Format(ImapCommands.Authenticate + " \"{1}\" \"{2}\"", "PLAIN", Login, Password) : string.Format(ImapCommands.Login, Login, Password);
        }

        public bool IsSupported(Capability capabilities)
        {
            return capabilities != null && (!capabilities.LoginDisabled || capabilities.AuthenticationMechanisms.Contains("PLAIN"));
        }

        public bool ProcessAnswers()
        {
            return true;
        }

        public bool IsMultiCommand()
        {
            return false;
        }
    }
}