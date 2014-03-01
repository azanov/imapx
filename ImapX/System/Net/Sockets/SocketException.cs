using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Net.Sockets
{
    
    public class SocketException : Exception
    {
    
        public SocketError SocketErrorCode { get; set; }
        public SocketException(){}

        public SocketException(int errorCode)
        {
            SocketErrorCode = (SocketError) errorCode;
        }

        internal SocketException(SocketError socketError)
            : this((int)socketError)
        {
        }
    }
}
