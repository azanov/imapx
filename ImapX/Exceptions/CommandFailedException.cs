using System;

namespace ImapX.Exceptions
{
    public class CommandFailedException : Exception
    {
        public CommandFailedException() { }
        public CommandFailedException(string message) : base(message) { }
        public CommandFailedException(string format, params object[] args) : base(string.Format(format, args)) { }
    }
}
