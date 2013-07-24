using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ImapX.Authentication
{
    public class OAuth2Credentials : IImapCredentials
    {

        /// <summary>
        /// The login name
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// The auth token
        /// </summary>
        public string AuthToken { get; set; }

        public OAuth2Credentials(string login, string authToken)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(authToken))
                throw new ArgumentException("Login and auth token cannot be empty");

            Login = login;
            AuthToken = authToken;

        }

        public string ToCommand(Capability capabilities)
        {
            if (!IsSupported(capabilities))
                throw new NotSupportedException("The selected authentication mechanism is not supported");

            return string.Format(ImapCommands.AUTHENTICATE + " \"{1}\"", "XOAUTH2", PrepareOAuthCredentials(Login, AuthToken));

        }

        private string PrepareOAuthCredentials(string login, string token)
        {

            var userData = Encoding.UTF8.GetBytes("user=" + login);
            var tokenLabelData = Encoding.UTF8.GetBytes("auth=Bearer ");
            var tokenData = Encoding.UTF8.GetBytes(token + "\n");

            using (var stream = new MemoryStream())
            {
                stream.Write(userData, 0, userData.Length);
                stream.WriteByte(1);
                stream.Write(tokenLabelData, 0, tokenLabelData.Length);
                stream.Write(tokenData, 0, tokenData.Length);
                stream.WriteByte(1);
                stream.WriteByte(1);
                return Convert.ToBase64String(stream.ToArray());
            }

        }

        public bool IsMultiCommand()
        {
            return false;
        }

        public bool ProcessAnswers()
        {
            return true;
        }

        public bool IsSupported(Capability capabilities)
        {
            return capabilities.XOAuth2;
        }
    }
}
