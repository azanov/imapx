using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImapX.Exceptions
{
    public class OperationFailedException : Exception
    {
        public OperationFailedException() { }
        public OperationFailedException(string message) : base(message) { }
    }
}
