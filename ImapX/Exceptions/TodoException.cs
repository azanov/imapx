using System;

namespace ImapX.Exceptions
{
    public class TodoException : Exception
    {
        public TodoException() { }
        public TodoException(string message) : base(message) { }
        public TodoException(string format, params object[] args) : base(string.Format(format, args)) { }
    }
}
