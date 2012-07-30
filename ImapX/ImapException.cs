using System;
namespace ImapX
{
    public class ImapException : Exception
    {
        private string _message;

        public override string Message
        {
            get
            {
                return this._message;
            }
        }

        public ImapException(string message) : base(message)
        {
            this._message = message;
        }
    }
}
