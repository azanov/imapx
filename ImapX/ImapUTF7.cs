using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ImapX
{
    public class ImapUTF7
    {
        public static string Encode(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            value = value.Replace("&", "&-");

            var rex = new Regex("[^ -~]*");
            var encoding = new UnicodeEncoding(true, false);

            var toEncode =
                rex.Matches(value).Cast<Match>().Where(_ => !string.IsNullOrWhiteSpace(_.Value));

            foreach (var match in toEncode)
            {
                var b64 = "&" + Base64.ToBase64(encoding.GetBytes(match.Value)).TrimEnd('=').Replace("/", ",") +
                             "-";
                value = value.Replace(match.Value, b64);
            }

            return value;
        }

        public static string Decode(string name)
        {
            var result = name.Replace("&-", "&");
            var encoding = new UnicodeEncoding(true, false);
            var rex = new Regex(@"&[\w|,]*-");

            var matches = rex.Matches(name);

            foreach (Match m in matches)
            {
                string v = m.Value.TrimStart('&').TrimEnd('-').Replace(",", "/");
                v += v.Length%4 == 2 ? "==" : (v.Length%4 == 3 ? "=" : "");
                result = result.Replace(m.Value, encoding.GetString(Base64.FromBase64(v)).Replace("\0", ""));
            }

            return result;
        }
    }
}