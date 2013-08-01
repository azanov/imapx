using System;

namespace ImapX.EmailParser
{
    public class ParseError
    {
        public ParseError(string item, Exception e)
        {
            ItemString = item;
            ThrowedException = e;
        }

        public string ItemString { get; set; }

        public Exception ThrowedException { get; set; }
    }
}