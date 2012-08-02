using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImapX.Extensions
{
    public static class StringExtensions
    {
        public static string AsGMailFolderName(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var sb = new StringBuilder();
            var encoding = Encoding.GetEncoding(1201);

            value.Split(' ').All(delegate(string part)
            {

                sb.Append("&");

                sb.Append(Convert.ToBase64String(encoding.GetBytes(part)).TrimEnd('=').Replace("/", ","));

                sb.Append("- ");

                return true;

            });

            return sb.ToString().TrimEnd();

        } 
    }
}
