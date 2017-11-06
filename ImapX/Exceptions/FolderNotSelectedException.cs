using System;

namespace ImapX.Exceptions
{
    public class FolderNotSelectedException : Exception
    {
        public FolderNotSelectedException() { }
        public FolderNotSelectedException(string message) : base(message) { }
        public FolderNotSelectedException(string format, params object[] args) : base(string.Format(format, args)) { }
    }
}
