using System;
using System.Collections.Generic;
using System.Globalization;
using ImapX.EncodingHelpers;
using System.Net.Mime;
using System.Text;
using ImapX.Extensions;

namespace ImapX.Parsing
{
    public class HeaderFieldParser
    {
        public static List<MailAddress> ParseMailAddressCollection(string value)
        {
            var list = new List<MailAddress>();
            string[] array = value.Trim().Split(
                value.IndexOf('>') != -1 ? new[] { ">,", "> ," } : new[] { "," }, StringSplitOptions.None);

            foreach (string line in array)
            {
                try
                {
                    list.Add(ParseMailAddress(line));
                }
                catch
                {
                }
            }
            

            return list;

        }

        public static MailAddress ParseMailAddress(string value)
        {
            int num = value.LastIndexOf("<", StringComparison.Ordinal);
            if (num < 0)
                return new MailAddress(string.Empty, value.Trim());

            string address = value.Substring(num).Trim().TrimStart('<').TrimEnd('>');

            if (string.IsNullOrEmpty(address)) return null;

            string displayName = "";
            if (num >= 1)
                displayName = value.Substring(0, num - 1).Trim();

            return new MailAddress(StringDecoder.Decode(displayName, true).Trim().Trim(new[] { ' ', '<', '>', '\r', '\n' }), address);
        }

        public static DateTime? ParseDate(string value)
        {
            return DateTimeExtensions.ParseDate(value);
        }

        public static ContentType ParseContentType(string value)
        {
            var tmp = value.ToLower().Replace(Environment.NewLine, "").Replace("\t", "").Split(';');
            var type = "";
            var sb = new StringBuilder();
            for (var i = 0; i < tmp.Length; i++)
            {
                var v = tmp[i].Trim();

                if (v.StartsWith("multipart"))
                    continue;

                if (v.IndexOf('/') != -1 && v.IndexOf('=') == -1)
                    type = v;
                else if (v.IndexOf('=') != -1)
                    sb.Append(v + (i < tmp.Length - 1 ? "; " : ""));
                else if (v.StartsWith("iso-"))
                    sb.Append("charset=" + v + (i < tmp.Length - 1 ? "; " : ""));
            }

            if (string.IsNullOrEmpty(type))
                return null;
            try
            {
                return new ContentType(type + (sb.Length == 0 ? "" : "; " + sb));
            }
            catch
            {
                return null;
            }
        }


    }
}