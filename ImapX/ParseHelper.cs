using System;
using System.Collections.Generic;
namespace ImapX
{
    internal static class ParseHelper
    {
        public static bool Exists(string line, ref int property)
        {
            if (line.Contains("EXISTS"))
            {
                int num;
                if (int.TryParse(line.Split(new char[]
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
                if (int.TryParse(line.Split(new char[]
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
                if (int.TryParse(line.Split(new char[]
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
                string text = line.Split(new char[]
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
                if (int.TryParse(line.Split(new char[]
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
            int num = line.LastIndexOf("<");
            if (num < 0)
            {
                return new MailAddress(null, line.Trim());
            }
            string addr = line.Substring(num).Trim().TrimStart(new char[]
			{
				'<'
			}).TrimEnd(new char[]
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
            List<MailAddress> list = new List<MailAddress>();
            string[] array = value.Trim().Split(new string[]
			{
				">,", 
				"> ,"
			}, StringSplitOptions.None);
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string line = array2[i];
                try
                {
                    list.Add(ParseHelper.Address(line));
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
