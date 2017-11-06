using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImapX.Exceptions
{
    public class LoginFailedException : Exception
    {
        public LoginFailedException() { }
        public LoginFailedException(string message) : base(message) { }
        public LoginFailedException(string format, params object[] args) : base(string.Format(format, args)) { }
    }
}
