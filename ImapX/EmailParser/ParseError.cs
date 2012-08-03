using System;

namespace ImapX.EmailParser
{
    public class ParseError
    {
    	public string ItemString { get; set; }

    	public Exception ThrowedException { get; set; }

    	public ParseError(string item, Exception e)
        {
            this.ItemString = item;
            this.ThrowedException = e;
        }
    }
}
