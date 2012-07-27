using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace ImapX.Helpers
{
    internal class DecodeHelper
    {

        internal static string DecodeSubject(string _subject)
        {
            if (string.IsNullOrWhiteSpace(_subject))
                return string.Empty;

            try
            {

                var regex = new Regex(@"=\?(?<charset>.*?)\?(?<encoding>[qQbB])\?(?<value>.*?)\?=");

                var decodedString = string.Empty;

                while (_subject.Length > 0)
                {
                    var match = regex.Match(_subject);
                    if (match.Success)
                    {
                        // If the match isn't at the start of the string, copy the initial few chars to the output
                        decodedString += _subject.Substring(0, match.Index);

                        var charset = match.Groups["charset"].Value;
                        var encoding = match.Groups["encoding"].Value.ToUpper();
                        var value = match.Groups["value"].Value;

                        if (encoding.Equals("B"))
                        {
                            decodedString += DecodeBase64(value, Encoding.GetEncoding(charset));
                        }
                        else if (encoding.Equals("Q"))
                        {
                            decodedString += DecodeQuotedPrintable(value, Encoding.GetEncoding(charset));
                        }
                        else
                        {
                            // Encoded value not known, return original string
                            // (Match should not be successful in this case, so this code may never get hit)
                            decodedString += _subject;
                            break;
                        }

                        // Trim off up to and including the match, then we'll loop and try matching again.
                        _subject = _subject.Substring(match.Index + match.Length);
                    }
                    else
                    {
                        // No match, not encoded, return original string
                        decodedString += _subject;
                        break;
                    }
                }

                return decodedString;

            }
            catch
            {
                return _subject;
            }
        }

        internal static string DecodeBase64(string value, Encoding encoding)
        {
            if (encoding == null)
                encoding = System.Text.Encoding.Default;

            if (string.IsNullOrWhiteSpace(value))
                return "";

            var bytes = Convert.FromBase64String(value);
            return encoding.GetString(bytes);

        }

        internal static string DecodeQuotedPrintable(string value, Encoding encoding)
        {
            if (encoding == null)
                encoding = Encoding.Default;


            if (value.IndexOf('_') > -1 && value.IndexOf(' ') == -1)
                value = value.Replace('_', ' ');

            var data = System.Text.Encoding.ASCII.GetBytes(value);
            var eq = Convert.ToByte('=');
            var n = 0;
            for (int i = 0; i < data.Length; i++)
            {
                var b = data[i];

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

        internal static Encoding ParseContentType(string value, out string contentType)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                contentType = null; return null;
            }
            value = value.ToLower().Trim();
            var rex = (new Regex("(.*);.*charset=(.*)[;]?"));

            value = value.Replace("\"", "").Replace("\'", "").Replace("\n", "").Replace("\t", "");
            var tmp = rex.Match(value);

            if (!tmp.Success)
            {

                contentType = (new Regex(@"(.*)\/(.*)[;]?").IsMatch(value)) ? value.Replace(";", "").TrimEnd() : string.Empty;
                return Encoding.Default;
            }

            contentType = tmp.Groups[tmp.Groups.Count - 2].Value.Split(new[] { ';' })[0].Trim();

            return Encoding.GetEncoding(tmp.Groups[tmp.Groups.Count - 1].Value.Split(new[] { ';' })[0].Trim());
        }
    }
}
