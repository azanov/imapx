using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ImapX.Extensions
{
    public static class DateTimeExtensions
    {

        internal static readonly char[] allowedWhiteSpaceChars;
        internal static readonly string[] validDateTimeFormats;
        internal static readonly IDictionary<string, TimeSpan> timeZoneOffsetLookup;

        static DateTimeExtensions()
        {
            allowedWhiteSpaceChars = new char[] { ' ', '\t' };
            
            validDateTimeFormats = new string[]
            {
                "ddd, dd MMM yyyy HH:mm:ss",
                "dd MMM yyyy HH:mm:ss",
                "ddd, dd MMM yyyy HH:mm",
                "dd MMM yyyy HH:mm",
                "ddd, d MMM yyyy HH:mm:ss",
                "ddd, d MMM yyyy HH:mm",
                "d MMM yyyy HH:mm:ss", // ^ new date format added by danbert2000 5/8/14
                "ddd, dd-MMM-yyyy HH:mm:ss",
                "dd-MMM-yyyy HH:mm:ss",
                "ddd, dd-MMM-yyyy HH:mm",
                "dd-MMM-yyyy HH:mm",
                "ddd, d-MMM-yyyy HH:mm:ss",
                "ddd, d-MMM-yyyy HH:mm",
                "d-MMM-yyyy HH:mm:ss"
            };

            timeZoneOffsetLookup = new Dictionary<string, TimeSpan>();
            timeZoneOffsetLookup.Add("UT", TimeSpan.Zero);
            timeZoneOffsetLookup.Add("GMT", TimeSpan.Zero);
            timeZoneOffsetLookup.Add("EDT", new TimeSpan(-4, 0, 0));
            timeZoneOffsetLookup.Add("EST", new TimeSpan(-5, 0, 0));
            timeZoneOffsetLookup.Add("CDT", new TimeSpan(-5, 0, 0));
            timeZoneOffsetLookup.Add("CST", new TimeSpan(-6, 0, 0));
            timeZoneOffsetLookup.Add("MDT", new TimeSpan(-6, 0, 0));
            timeZoneOffsetLookup.Add("MST", new TimeSpan(-7, 0, 0));
            timeZoneOffsetLookup.Add("PDT", new TimeSpan(-7, 0, 0));
            timeZoneOffsetLookup.Add("PST", new TimeSpan(-8, 0, 0));

        }

        public static string ToImapDate(this DateTime date)
        {
            if (date == null)
                throw new ArgumentException("date cannot be null");

            return date.ToString("dd-MMM-yyyy", new CultureInfo("en-US"));
        }

        public static string ToImapInternalDate(this DateTime date)
        {
            var dateTime = date.ToString("dd-MMM-yyyy hh:mm:ss", new CultureInfo("en-US"));
            var timeZone = date.ToString("zzz").Replace(":", "");

            if (dateTime[0] == '0')
                dateTime = " " + dateTime.Substring(1);

            return dateTime + " " + timeZone;
        }

        public static DateTime? ParseDate(string value)
        {

            

            string timeZoneString = null;
            TimeSpan timeZone;
            var date = ParseValue(value, out timeZoneString);

            if (!date.HasValue)
                return null;

            if (TryParseTimeZoneString(timeZoneString, out timeZone))
            {
                DateTimeOffset offset = new DateTimeOffset(date.Value, timeZone);
                return offset.LocalDateTime;
            }
            else {
                return DateTime.SpecifyKind(date.Value, DateTimeKind.Unspecified);

            }
        }

        private static DateTime? ParseValue(string value, out string timeZone)
        {
            DateTime time; timeZone = null;

            if (string.IsNullOrEmpty(value))
                return null;

            int index = value.IndexOf(':');

            if (index == -1)
                return null;

            int length = value.IndexOfAny(allowedWhiteSpaceChars, index);

            if (length == -1)
                return null;

            if (!DateTime.TryParseExact(value.Substring(0, length).Trim(), validDateTimeFormats, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out time))
                return null;

            string str2 = value.Substring(length).Trim();

            int num3 = str2.IndexOfAny(allowedWhiteSpaceChars);

            if (num3 != -1)
            {
                str2 = str2.Substring(0, num3);
            }

            if (string.IsNullOrEmpty(str2))
                return null;

            timeZone = str2;
            return time;

        }

        private static bool TryParseTimeZoneString(string timeZoneString, out TimeSpan timeZone)
        {
            timeZone = TimeSpan.Zero;

            try
            {
                
                if (timeZoneString != "-0000")
                {
                    if ((timeZoneString[0] == '+') || (timeZoneString[0] == '-'))
                    {
                        bool flag;
                        int num;
                        int num2;
                        ValidateAndGetTimeZoneOffsetValues(timeZoneString, out flag, out num, out num2);
                        if (!flag)
                        {
                            if (num != 0)
                            {
                                num *= -1;
                            }
                            else if (num2 != 0)
                            {
                                num2 *= -1;
                            }
                        }
                        timeZone = new TimeSpan(num, num2, 0);
                        return true;
                    }
                    ValidateTimeZoneShortHandValue(timeZoneString);
                    if (timeZoneOffsetLookup.ContainsKey(timeZoneString))
                    {
                        timeZone = timeZoneOffsetLookup[timeZoneString];
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        private static void ValidateTimeZoneShortHandValue(string value)
        {
            for (int i = 0; i < value.Length; i++)
                if (!char.IsLetter(value, i))
                    throw new FormatException();
        }

        private static void ValidateAndGetTimeZoneOffsetValues(string offset, out bool positive, out int hours, out int minutes)
        {
            if (offset.Length != 5) 
                throw new FormatException();

            positive = offset.StartsWith("+");
            if (!int.TryParse(offset.Substring(1, 2), NumberStyles.None, CultureInfo.InvariantCulture, out hours))
                throw new FormatException();

            if (!int.TryParse(offset.Substring(3, 2), NumberStyles.None, CultureInfo.InvariantCulture, out minutes))
                throw new FormatException();

            if (minutes > 0x3b)
                throw new FormatException();
        }

 

 


 



    }
}
