using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ImapX.EncodingHelpers;
using ImapX.Parsing;

namespace ImapX
{
    public static class ParseHelper
    {
        
        public static string ExtractFileType(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            var rex = new Regex(@"(.*\/.*)");
            string[] tmp = value.Split(';');
            foreach (string s in tmp)
            {
                Match m = rex.Match(s.Trim());
                if (m.Success)
                    return m.Value;
            }
            return value;
        }

        public static Encoding ParseContentType(string value, out string contentType)
        {
            if (string.IsNullOrEmpty(value))
            {
                contentType = null;
                return null;
            }
            value = value.ToLower().Trim();
            var rex = (new Regex("(.*);.*charset=(.*)[;]?"));
            value = value.Replace("\"", "").Replace("\'", "").Replace("\n", "").Replace("\t", "");
            Match tmp = rex.Match(value);
            if (!tmp.Success)
            {
                contentType = (new Regex(@"(.*)\/(.*)[;]?").IsMatch(value))
                    ? value.Replace(";", "").TrimEnd()
                    : string.Empty;
                return Encoding.UTF8;
            }
            contentType = tmp.Groups[tmp.Groups.Count - 2].Value.Split(new[] {';'})[0].Trim();
            return StringDecoder.TryGetEncoding(tmp.Groups[tmp.Groups.Count - 1].Value.Split(new[] {';'})[0].Trim(), Encoding.UTF8);
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

        internal static string ExtractFileName(string p)
        {
            if (string.IsNullOrEmpty(p)) return string.Empty;

            var rex = new Regex(@"([^:|^=]*)[:|=][\s]?(.*)[;]?");
            foreach (Match match in p.Split(';').Select(part => rex.Match(part)))
            {
                if (!match.Success)
                    continue;


                string field = match.Groups[1].Value.ToLower().Trim();
                string value = match.Groups[2].Value.Trim().Trim('"').TrimEnd(';');

                switch (field)
                {
                    case "name":
                    case "filename":
                        return StringDecoder.Decode(value.Trim('"').Trim('\''));
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

                s = StringDecoder.Decode(s);

                s = s.Trim();
                s = s + ".eml";
            }
            else
            {
                s = "ATT.eml";
                int i = 0;
                while (File.Exists(s))
                {
                    s = "ATT" + i + ".eml";
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