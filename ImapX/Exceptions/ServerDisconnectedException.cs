using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImapX.Exceptions
{
    public class ServerDisconnectedException : Exception
    {
        public ServerDisconnectedException() { }
        public ServerDisconnectedException(string message) : base(message) { }
    }
}
