namespace ImapX.Commands
{
    public class LogoutCommand : ImapCommand
    {
        public LogoutCommand(ImapBase imapBase, long id) : base(imapBase, id, "LOGOUT\r\n")
        {
        }
    }
}
