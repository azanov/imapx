namespace ImapX.Commands
{
    public class UnselectCommand : ClientImapCommand
    {
        public UnselectCommand(ImapClient client, long id) : base(client, id, "UNSELECT\r\n")
        {
        }
    }
}
