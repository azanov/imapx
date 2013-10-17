using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImapX.Extensions
{
    public static class StringExtensions
    {
        public static string Break(this string value, int chunkSize)
        {
            var sb = new StringBuilder();

            value.Break(ref sb, chunkSize);

            return sb.ToString();
        }

        public static void Break(this string value, ref StringBuilder sb, int chunkSize)
        {

            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("value cannot be empty", "value");

            if (chunkSize < 1)
                throw new ArgumentException("chunkSize cannot be less than 1", "chunkSize");

            if (value.Length <= chunkSize)
            {
                sb.Append(value);
                return;
            }

            for (var i = 0; i < value.Length; i += chunkSize)
                sb.AppendLine(value.Substring(i, i + chunkSize > value.Length ? (value.Length - i) : chunkSize));


        }
    }
}
