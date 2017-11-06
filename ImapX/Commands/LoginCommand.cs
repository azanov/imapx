namespace ImapX.Commands
{
    public class LoginCommand : ImapCommand
    {
        public LoginCommand(ImapBase imapBase, long id, string userName, string password) : base(imapBase, id, string.Format("LOGIN \"{0}\" \"{1}\"\r\n", userName, password))
        {
        }
    }
}
