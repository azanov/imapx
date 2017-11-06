namespace ImapX.Commands
{
    public class MessageCommand : ClientImapCommand
    {
        public Message Message { get; set; }

        public MessageCommand(ImapClient client, long id, Message message) : base(client, id, message.Folder)
        {
            Message = message;
        }

        public MessageCommand(ImapClient client, long id, Message message, string code) : base(client, id, code, message.Folder)
        {
            Message = message;
        }
    }

    public class MessageCommand<T> : ClientImapCommand<T>
    {
        public Message Message { get; set; }

        public MessageCommand(ImapClient client, long id, Message message) : base(client, id, message.Folder)
        {
            Message = message;
        }

        public MessageCommand(ImapClient client, long id, Message message, string code) : base(client, id, code, message.Folder)
        {
            Message = message;
        }
    }
}
