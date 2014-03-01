﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Security.Authentication
{

    public class AuthenticationException : Exception
    {
        public AuthenticationException() : base() { }
        public AuthenticationException(string message) : base(message) { }
        public AuthenticationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
