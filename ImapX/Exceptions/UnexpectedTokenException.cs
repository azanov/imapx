using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImapX.Exceptions
{
    public class UnexpectedTokenException : Exception
    {
        public UnexpectedTokenException() { }
        public UnexpectedTokenException(string message) : base(message) { }
    }
}
