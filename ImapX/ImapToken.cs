namespace ImapX
{
    public class ImapToken
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }
        public byte[] Data { get; set; }

        public ImapToken(TokenType type, string value = null, byte[] data = null)
        {
            Type = type;
            Value = value;
            Data = data;
        }
    }
}
