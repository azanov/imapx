using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImapX.Exceptions
{
    public class ServerAlertException : Exception
    {
        public ServerAlertException() { }
        public ServerAlertException(string message) : base(message) { }
    }
}
