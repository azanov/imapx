using System;

namespace ImapX.Exceptions
{
    public class InvalidStateException : Exception
    {
        public InvalidStateException() { }
        public InvalidStateException(string message) : base(message) { }
    }
}