namespace ImapX
{
    public class TextMessageContent : MessageContent
    {
        public TextMessageContent(ImapClient client, Message message) : base(client, message)
        {
        }

        internal TextMessageContent()
        {
        }

        public long Lines { get; set; }
    }
}
