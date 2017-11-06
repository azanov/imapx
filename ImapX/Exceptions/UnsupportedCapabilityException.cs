using System;

namespace ImapX.Exceptions
{
    public class UnsupportedCapabilityException : Exception
    {
        public UnsupportedCapabilityException() { }
        public UnsupportedCapabilityException(string message) : base(message) { }
        public UnsupportedCapabilityException(string format, params object[] args) : base(string.Format(format, args)) { }
    }
}
