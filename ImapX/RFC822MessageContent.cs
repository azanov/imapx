namespace ImapX
{
    public class RFC822MessageContent : TextMessageContent
    {
        public RFC822MessageContent(ImapClient client, Message message) : base(client, message)
        {
        }

        internal RFC822MessageContent()
        {
        }

        public Envelope Envelope { get; set; }
    }
}
