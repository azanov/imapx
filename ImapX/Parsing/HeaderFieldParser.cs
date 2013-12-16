﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using ImapX.EncodingHelpers;
using System.Net.Mime;
using System.Text;

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

        internal static readonly string[] ValidDateTimeFormats =
        {
            "ddd, dd MMM yyyy HH:mm:ss",
            "dd MMM yyyy HH:mm:ss",
            "ddd, dd MMM yyyy HH:mm",
            "dd MMM yyyy HH:mm",
            "ddd, d MMM yyyy HH:mm:ss",
            "ddd, d MMM yyyy HH:mm"
        };

        public static DateTime? ParseDate(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;

            var num = value.IndexOf(':');

            if (num != -1)
            {
                var num2 = value.IndexOfAny(new[] { ' ', '\t' }, num);
                if (num2 == -1)
                    return null;
                value = value.Substring(0, num2);
            }

            value = value.Replace('-', ' ');

            DateTime dateTime;
            if (
                !DateTime.TryParseExact(value, ValidDateTimeFormats, DateTimeFormatInfo.InvariantInfo,
                    DateTimeStyles.AllowWhiteSpaces, out dateTime))
                return null;



            return dateTime;
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
                else if (v.IndexOf("/") != -1)
                    type = v;
                else if (v.IndexOf('=') != -1)
                    sb.Append(v + (i < tmp.Length - 1 ? "; " : ""));
                else if (tmp[i].Trim().ToLower().StartsWith("iso-"))
                    sb.Append("charset=" + tmp[i].Trim().ToLower() + (i < tmp.Length - 1 ? "; " : ""));
            }
            try
            {
                return new ContentType(type + (sb.Length == 0 ? "" : "; " + sb.ToString()));
            }
            catch
            {
                return null;
            }
        }


    }
}