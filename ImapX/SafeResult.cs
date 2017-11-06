using System;

namespace ImapX
{
    public class SafeResult
    {

        public static SafeResult True = new SafeResult(true);
        public static SafeResult False = new SafeResult();

        public bool Success { get; set; }
        public Exception Exception { get; set; }

        public SafeResult(bool value = false, Exception exception = null)
        {
            Success = value;
            Exception = exception;
        }

        public static implicit operator SafeResult(bool value)
        {
            return value ? True : False;
        }

        public static explicit operator bool(SafeResult x)
        {
            return x.Success;
        }

        public static bool operator true(SafeResult value)
        {
            return value.Success;
        }

        public static bool operator false(SafeResult value)
        {
            return !value.Success;
        }
    }
}
