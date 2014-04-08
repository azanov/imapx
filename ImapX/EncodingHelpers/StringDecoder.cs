using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using ImapX.Parsing;
using System.Linq;
using System.Collections.Generic;

namespace ImapX.EncodingHelpers
{
    public class StringDecoder
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

        internal static string DecodeQuotedPrintable(string input, Encoding encoding)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            if (encoding == null)
                encoding = Encoding.UTF8;

            var sb = new StringBuilder();
            var validHex = new[]
            {
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'
            };

    
            var start = 0;
            var end = input.Length;
            while (start < end)
            {
                int pos = input.IndexOf('=', start);
                if (pos < 0)
                {
                    sb.Append(input.Substring(start, end - start));
                    break;
                }
                sb.Append(input.Substring(start, pos - start));
                start = pos;

                var buffer = new List<byte>();

                while (pos < end - 2 && input[pos] == '=' && validHex.Contains(char.ToUpper(input[pos + 1])) && validHex.Contains(char.ToUpper(input[pos + 2])))
                {
                    string hex = input.Substring(pos + 1, 2);
                    byte b = byte.Parse(hex, NumberStyles.HexNumber);
                    buffer.Add(b);
                    start += 3;
                    pos += 3;
                }

                if (buffer.Count > 0)
                {
                    sb.Append(encoding.GetChars(buffer.ToArray()));
                    buffer.Clear();
                }

                if(pos < input.Length && input[pos] == '=')
                {
                    sb.Append("=");
                    start++;
                }
            }
            input = sb.ToString().Replace("?=", "").Replace("=\r\n", Environment.NewLine);

            return input;


        }


        public static string Decode(string text, bool dropLines)
        {
            text = (text ?? "").Trim();

            if (string.IsNullOrEmpty(text))
                return string.Empty;

            if (dropLines)
                text = text.Replace(Environment.NewLine, "");

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
                            decodedString += DecodeQuotedPrintable(value, TryGetEncoding(charset, Encoding.UTF8)).Replace("_", " ");
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