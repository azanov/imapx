using System;

namespace ImapX.Exceptions
{
    public class BadCommandException : Exception
    {
        public BadCommandException() { }
        public BadCommandException(string message) : base(message) { }
        public BadCommandException(string format, params object[] args) : base(string.Format(format, args)) { }
    }
}
