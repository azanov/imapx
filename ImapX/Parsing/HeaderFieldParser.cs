using System;
using System.Net.Mail;

namespace ImapX.Parsing
{
    public class HeaderFieldParser
    {
        public static MailAddressCollection ParseMailAddressCollection(string value)
        {
            var list = new MailAddressCollection();
            string[] array = value.Trim().Split(new[]
            {
                ">,",
                "> ,"
            }, StringSplitOptions.None);

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
                return new MailAddress(value.Trim(), string.Empty);

            string address = value.Substring(num).Trim().TrimStart('<').TrimEnd('>');

            string displayName = "";
            if (num >= 1)
                displayName = value.Substring(0, num - 1).Trim();

            return new MailAddress(address,
                StringDecoder.Decode(displayName).Trim().Trim(new[] {' ', '<', '>', '\r', '\n'}));
        }
    }
}