using System;

namespace ImapX.Exceptions
{
    public class UnsupportedResponseCodeException : Exception
    {
        public UnsupportedResponseCodeException() { }
        public UnsupportedResponseCodeException(string message) : base(message) { }
        public UnsupportedResponseCodeException(string format, params object[] args) : base(string.Format(format, args)) { }
    }
}
