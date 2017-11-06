using ImapX.EncodingHelpers;
using System;
using System.Text;

namespace ImapX.Extensions
{
    public static class StringExtensions
    {
        public static string Quote(this string value)
        {
            value = value.Replace("\"", "\\\"");
            value = value.Replace("\\", "\\\\");
            return "\"" + value + "\"";
        }

        public static string ToBase64String(this string value)
        {
            return Base64.ToBase64(Encoding.UTF8.GetBytes(value));
        }

        public static string Replace(this string s, char[] separators, string newVal)
        {
            string[] temp;

            temp = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return String.Join(newVal, temp);
        }
    }
}
