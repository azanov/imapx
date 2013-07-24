using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImapX.Exceptions
{
    public class InvalidStateException : Exception
    {
        public InvalidStateException() { }
        public InvalidStateException(string message) : base(message) { }
    }
}
