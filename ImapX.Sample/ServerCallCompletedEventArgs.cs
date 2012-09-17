using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImapX.Sample
{
    public class ServerCallCompletedEventArgs : EventArgs
    {
        public bool Result { get; set; }
        public Exception Exception { get; set; }

        public  ServerCallCompletedEventArgs(bool result = true, Exception exception = null)
        {
            Result = result;
            Exception = exception;
        }
    }
}
