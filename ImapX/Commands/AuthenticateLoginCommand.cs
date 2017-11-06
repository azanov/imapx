using ImapX.Authentication;
using ImapX.Extensions;

namespace ImapX.Commands
{
    public class AuthenticateLoginCommand : AutheticateCommand
    {
        public AuthenticateLoginCommand(ImapBase imapBase, long id, PlainCredentials credentials) : base(imapBase, id, "LOGIN")
        {
            Parts.Add(credentials.Login.ToBase64String() + "\r\n");
            Parts.Add(credentials.Password.ToBase64String() + "\r\n");
        }
    }
}
