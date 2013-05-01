using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ImapX
{
    public static class ParseHelper
    {

        public static Encoding TryGetEncoding(string name, Encoding defaultEncoding = null)
        {
            try
            {
                return Encoding.GetEncoding(name);
            }
            catch {
                return defaultEncoding;
            }
        }

        public static string DecodeName(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            try
            {
                text = text.Replace("\t", "");
                var regex = new Regex(@"[=]?\?(?<charset>.*?)\?(?<encoding>[qQbB])\?(?<value>.*?)\?=");
                var decodedString = string.Empty;
                while (text.Length > 0)
                {
                    var match = regex.Match(text);
                    if (match.Success)
                    {
                        // If the match isn't at the start of the string, copy the initial few chars to the output
                        decodedString += text.Substring(0, match.Index);
                        var charset = match.Groups["charset"].Value;
                        var encoding = match.Groups["encoding"].Value.ToUpper();
                        var value = match.Groups["value"].Value;
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

        public static string DecodeBase64(string value, Encoding encoding)
        {
            if (encoding == null)
                encoding = Encoding.Default;
            if (string.IsNullOrEmpty(value))
                return "";
            var bytes = Base64.FromBase64(value);
            return encoding.GetString(bytes);
        }

        public static string DecodeQuotedPrintable(string value, Encoding encoding)
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

        public static string ExtractFileType(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            var rex = new Regex(@"(.*\/.*)");
            var tmp = value.Split(';');
            foreach (var s in tmp)
            {
                var m = rex.Match(s.Trim());
                if (m.Success)
                    return m.Value;
            }
            return value;
        }

        public static Encoding ParseContentType(string value, out string contentType)
        {
            if (string.IsNullOrEmpty(value))
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
            return TryGetEncoding(tmp.Groups[tmp.Groups.Count - 1].Value.Split(new[] { ';' })[0].Trim(), Encoding.UTF8);
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
            return new MailAddress(DecodeName(display), addr);
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

        internal static string ExtractFileName(string p)
        {
            if (string.IsNullOrEmpty(p)) return string.Empty;

            var rex = new Regex(@"([^:|^=]*)[:|=][\s]?(.*)[;]?");
            foreach (var match in p.Split(';').Select(part => rex.Match(part)))
            {
                
                if (!match.Success)
                    continue;


                var field = match.Groups[1].Value.ToLower().Trim();
                var value = match.Groups[2].Value.Trim().Trim('"').TrimEnd(';');

                switch (field)
                {
                    
                    case "name":
                    case "filename":
                        return DecodeName(value.Trim('"').Trim('\''));
                        
                }
            }
            return string.Empty;
        }

        // [2013-04-24] naudelb(Len Naude) - Added
        // Gets the file name from the attaches eml's subject line
        public static string GetRFC822FileName(string textData)
        {
            if (string.IsNullOrEmpty(textData)) return string.Empty;

            string s = string.Empty;

            if (textData.IndexOf("Subject:") > 0)
            {
                s = textData.Substring(textData.IndexOf("Subject:"));
                s = s.Substring(8, s.IndexOf(Environment.NewLine) - 8);

                s = RemoveIllegalFileNameChars(s);

                s = s.Trim();
                s = s + ".eml";
            }
            else
            {
                s = "ATT.eml";
                int i = 0;
                while (System.IO.File.Exists(s))
                {
                    s = "ATT" + i.ToString() + ".eml";
                    i++;
                }
            }

            return s;
        }
        // [2013-04-24] naudelb(Len Naude) - Added
        public static string RemoveIllegalFileNameChars(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;

            char spc = Convert.ToChar(" ");
            s = s.Replace('\b', spc);
            s = s.Replace('\r', spc);
            s = s.Replace("\r\n", " ");
            s = s.Replace('\f', spc);
            s = s.Replace('\n', spc);
            s = s.Replace('\0', spc);
            s = s.Replace('"', spc);
            s = s.Replace('\t', spc);
            s = s.Replace('\v', spc);
            s = s.Replace("\\", " ");
            s = s.Replace("/", " ");
            s = s.Replace(":", " ");
            s = s.Replace("*", " ");
            s = s.Replace("?", " ");
            s = s.Replace("\"", " ");
            s = s.Replace("<", " ");
            s = s.Replace(">", " ");
            s = s.Replace("|", " ");
            return s;
        }
    }
}