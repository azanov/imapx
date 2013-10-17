using System;

namespace ImapX.Exceptions
{
    public class OperationFailedException : Exception
    {
        public OperationFailedException() { }
        public OperationFailedException(string message) : base(message) { }
    }
}