using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ImapX.EncodingHelpers
{
    public class ImapUTF7
    {
        public static string Encode(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            value = value.Replace("&", "&-");

            var rex = new Regex("[^ -~]*");
            var encoding = new UnicodeEncoding(true, false);

            IEnumerable<Match> toEncode =
                rex.Matches(value).Cast<Match>().Where(_ => !string.IsNullOrEmpty(_.Value));

            foreach (Match match in toEncode)
            {
                string b64 = "&" + Base64.ToBase64(encoding.GetBytes(match.Value)).TrimEnd('=').Replace("/", ",") +
                             "-";
                value = value.Replace(match.Value, b64);
            }

            return value;
        }

        public static string Decode(string name)
        {
            string result = name.Replace("&-", "&");
            var encoding = new UnicodeEncoding(true, false);
            var rex = new Regex(@"&[\w|,|\+]*-");

            MatchCollection matches = rex.Matches(name);

            foreach (Match m in matches)
            {
                string v = m.Value.TrimStart('&').TrimEnd('-').Replace(",", "/");
                v += v.Length%4 == 2 ? "==" : (v.Length%4 == 3 ? "=" : "");
                var data = Base64.FromBase64(v);
                result = result.Replace(m.Value, encoding.GetString(data, 0, data.Length).Replace("\0", ""));
            }

            return result;
        }
    }
}