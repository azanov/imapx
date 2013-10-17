using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using ImapX.Parsing;

namespace ImapX.EncodingHelpers
{
    internal class StringDecoder
    {
        private static Encoding TryGetEncoding(string name, Encoding defaultEncoding = null)
        {
            try
            {
                return Encoding.GetEncoding(name);
            }
            catch
            {
                return defaultEncoding;
            }
        }

        internal static string DecodeBase64(string value, Encoding encoding)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            if (string.IsNullOrEmpty(value))
                return "";
            byte[] bytes = Base64.FromBase64(value);
            return encoding.GetString(bytes, 0, bytes.Length);
        }

        internal static string DecodeQuotedPrintable(string value, Encoding encoding)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            if (encoding == null)
                encoding = Encoding.UTF8;

            if (value.IndexOf('_') > -1 && value.IndexOf(' ') == -1)
                value = value.Replace('_', ' ');
            byte[] data = Encoding.UTF8.GetBytes(value);
            byte eq = Convert.ToByte('=');
            int n = 0;
            for (int i = 0; i < data.Length; i++)
            {
                byte b = data[i];

                if ((b == eq) && ((i + 1) < data.Length))
                {
                    byte b1 = data[i + 1], b2 = data[i + 2];
                    if (b1 == 10 || b1 == 13)
                    {
                        i++;
                        if (b2 == 10 || b2 == 13)
                        {
                            i++;
                        }
                        continue;
                    }
                    data[n] = (byte)int.Parse(value.Substring(i + 1, 2), NumberStyles.HexNumber);
                    n++;
                    i += 2;
                }
                else
                {
                    data[n] = b;
                    n++;
                }
            }
            value = encoding.GetString(data, 0, n);
            return value;

        }


        public static string Decode(string text)
        {
            text = (text ?? "").Trim();

            if (string.IsNullOrEmpty(text))
                return string.Empty;

            if (text.IndexOf("=?", StringComparison.Ordinal) == -1)
                return text;

            try
            {
                text = text.Replace("\t", "");

                string decodedString = string.Empty;
                while (text.Length > 0)
                {
                    Match match = Expressions.StringEncodingRex.Match(text);
                    if (match.Success)
                    {
                        // If the match isn't at the start of the string, copy the initial few chars to the output
                        decodedString += text.Substring(0, match.Index);
                        string charset = match.Groups["charset"].Value;
                        string encoding = match.Groups["encoding"].Value.ToUpper();
                        string value = match.Groups["value"].Value;
                        if (encoding.Equals("B"))
                        {
                            decodedString += DecodeBase64(value, TryGetEncoding(charset, Encoding.UTF8));
                        }
                        else if (encoding.Equals("Q"))
                        {
                            decodedString += DecodeQuotedPrintable(value, TryGetEncoding(charset, Encoding.UTF8));
                        }
                        else
                        {
                            // Encoded value not known, return original string
                            // (Match should not be successful in this case, so this code may never get hit)
                            decodedString += text;
                            break;
                        }
                        // Trim off up to and including the match, then we'll loop and try matching again.
                        text = text.Substring(match.Index + match.Length);
                    }
                    else
                    {
                        // No match, not encoded, return original string
                        decodedString += text;
                        break;
                    }
                }
                return decodedString;
            }
            catch
            {
                return text;
            }
        }
    }
}