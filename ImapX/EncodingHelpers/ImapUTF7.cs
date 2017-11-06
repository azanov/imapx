using System.Text;

namespace ImapX.EncodingHelpers
{
    public class ImapUTF7
    {
        public static string Encode(string value)
        {
            var sb = new StringBuilder();

            for(var i = 0; i < value.Length; i++)
            {
                var currentChar = value[i];
                if (currentChar == '&')
                    sb.Append("&-");
                else if (currentChar >= '\x20' && currentChar <= '\x7e')
                    sb.Append(currentChar);
                else
                {
                    var toEncode = new StringBuilder();
                    while (i < value.Length && ((currentChar = value[i]) < '\x20' || currentChar > '\x7e'))
                    {
                        toEncode.Append(currentChar); i++;
                    }
                    i--;
                    sb.Append(Encoding.ASCII.GetString(Encoding.UTF7.GetBytes(toEncode.ToString())).Replace('/', ',').Replace('+', '&'));
                }
            }
            return sb.ToString();
        }

        public static string Decode(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var sb = new StringBuilder();

            for(var i = 0; i < value.Length; i++)
            {
                var currentChar = value[i];

                if (currentChar != '&')
                    sb.Append(currentChar);
                else
                {
                    var length = value.IndexOf('-', i) - i;
                    if (length > 1)
                    {
                        var part = "+" + value.Substring(i + 1, length);
                        var v = Encoding.UTF7.GetString(Encoding.ASCII.GetBytes(part.Replace(',', '/')));
                        sb.Append(v);
                        i += length;
                    }
                    else
                    {
                        sb.Append('&'); if (length == 1) i++;
                    }
                }
            }

            return sb.ToString();
            
        }
    }
}
