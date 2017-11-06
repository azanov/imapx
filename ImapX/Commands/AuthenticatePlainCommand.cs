using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImapX.Authentication;
using ImapX.Extensions;
using System.IO;
using ImapX.EncodingHelpers;

namespace ImapX.Commands
{
    public class AuthenticatePlainCommand : AutheticateCommand
    {
        public AuthenticatePlainCommand(ImapBase imapBase, long id, PlainCredentials credentials) : base(imapBase, id, "PLAIN")
        {
            using(var stream = new MemoryStream())
            {
                var login = Encoding.UTF8.GetBytes(credentials.Login);
                var password = Encoding.UTF8.GetBytes(credentials.Password);
                stream.WriteByte(0);
                stream.Write(login, 0, login.Length);
                stream.WriteByte(0);
                stream.Write(password, 0, password.Length);
                Parts.Add(Base64.ToBase64(stream.ToArray()) + "\r\n");
            }
            
        }

        public override void Continue(string serverResponse)
        {
            base.Continue(serverResponse);
        }
    }
}
