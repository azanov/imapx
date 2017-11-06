namespace ImapX.Commands
{
    public class StartTLSCommand : ImapCommand
    {
        public StartTLSCommand(ImapBase imapBase, long id) : base(imapBase, id, "STARTTLS\r\n")
        {
        }
    }
}
