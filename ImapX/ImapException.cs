using System;

namespace ImapX
{
    public class ImapException : Exception
    {
        private readonly string _message;

        public ImapException(string message) : base(message)
        {
            _message = message;
        }

        public override string Message
        {
            get { return _message; }
        }
    }
}