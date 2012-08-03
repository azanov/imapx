using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace ImapX
{
    internal static class ParseHelper
    {
        internal static string DecodeSubject(string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
                return string.Empty;
            try
            {
                var regex = new Regex(@"=\?(?<charset>.*?)\?(?<encoding>[qQbB])\?(?<value>.*?)\?=");
                var decodedString = string.Empty;
                while (subject.Length > 0)
                {
                    var match = regex.Match(subject);
                    if (match.Success)
                    {
                        // If the match isn't at the start of the string, copy the initial few chars to the output
                        decodedString += subject.Substring(0, match.Index);
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
                            decodedString += subject;
                            break;
                        }
                        // Trim off up to and including the match, then we'll loop and try matching again.
                        subject = subject.Substring(match.Index + match.Length);
                    }
                    else
                    {
                        // No match, not encoded, return original string
                        decodedString += subject;
                        break;
                    }
                }
                return decodedString;
            }
            catch
            {
                return subject;
            }
        }

        internal static string DecodeBase64(string value, Encoding encoding)
        {
            if (encoding == null)
                encoding = Encoding.Default;
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
            var data = Encoding.ASCII.GetBytes(value);
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

        public static bool Exists(string line, ref int property)
        {
            if (line.Contains("EXISTS"))
            {
                int num;
                if (int.TryParse(line.Split(new[]
				{
					' '
				})[1], out num))
                {
                    property = num;
                }
                return true;
            }
            return false;
        }

        public static bool Recent(string line, ref int property)
        {
            if (line.Contains("RECENT"))
            {
                int num;
                if (int.TryParse(line.Split(new[]
				{
					' '
				})[1], out num))
                {
                    property = num;
                }
                return true;
            }
            return false;
        }

        public static bool Unseen(string line, ref int property)
        {
            if (line.Contains("UNSEEN"))
            {
                int num;
                if (int.TryParse(line.Split(new[]
				{
					' '
				})[3].Replace("]", ""), out num))
                {
                    property = num;
                }
                return true;
            }
            return false;
        }

        public static bool UidValidity(string line, ref string property)
        {
            if (line.Contains("UIDVALIDITY"))
            {
                string text = line.Split(new[]
				{
					' '
				})[3].Replace("]", "");
                property = text;
                return true;
            }
            return false;
        }

        public static bool UidNext(string line, ref int property)
        {
            if (line.Contains("UIDNEXT"))
            {
                int num;
                if (int.TryParse(line.Split(new[]
				{
					' '
				})[3].Replace("]", ""), out num))
                {
                    property = num;
                }
                return true;
            }
            return false;
        }

        public static bool MessageProperty(string key, string value, string header, ref string property)
        {
            if (key.ToLower().Trim().Equals(header))
            {
                property = value;
                return true;
            }
            return false;
        }

        public static MailAddress Address(string line)
        {
            int num = line.LastIndexOf("<", StringComparison.Ordinal);
            if (num < 0)
            {
                return new MailAddress(null, line.Trim());
            }
            string addr = line.Substring(num).Trim().TrimStart(new[]
			{
				'<'
			}).TrimEnd(new[]
			{
				'>'
			});
            string display = "";
            if (num >= 1)
            {
                display = line.Substring(0, num - 1).Trim();
            }
            return new MailAddress(display, addr);
        }

        public static List<MailAddress> AddressCollection(string value)
        {
            var list = new List<MailAddress>();
            string[] array = value.Trim().Split(new[]
			{
				">,", 
				"> ,"
			}, StringSplitOptions.None);
            string[] array2 = array;
            foreach (string line in array2)
            {
            	try
            	{
            		list.Add(Address(line));
            	}
            	catch (Exception)
            	{
            		throw new Exception("Not correct mail address");
            	}
            }
            return list;
        }
    }
}