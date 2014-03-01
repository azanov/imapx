using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace System.Net.Mime
{
    internal class SmtpDateTime
    {
        internal const string UnknownTimeZoneDefaultOffset = "-0000";
        internal const string UtcDefaultTimeZoneOffset = "+0000";
        internal const int OffsetLength = 5;
        internal const int MaxMinuteValue = 59;
        internal const string DateFormatWithDayOfWeek = "ddd, dd MMM yyyy HH:mm:ss";
        internal const string DateFormatWithoutDayOfWeek = "dd MMM yyyy HH:mm:ss";
        internal const string DateFormatWithDayOfWeekAndNoSeconds = "ddd, dd MMM yyyy HH:mm";
        internal const string DateFormatWithoutDayOfWeekAndNoSeconds = "dd MMM yyyy HH:mm";

        internal static readonly string[] ValidDateTimeFormats =
        {
            "ddd, dd MMM yyyy HH:mm:ss",
            "dd MMM yyyy HH:mm:ss",
            "ddd, dd MMM yyyy HH:mm",
            "dd MMM yyyy HH:mm"
        };

        internal static readonly char[] AllowedWhiteSpaceChars =
        {
            ' ',
            '\t'
        };

        internal static readonly IDictionary<string, TimeSpan> TimeZoneOffsetLookup = InitializeShortHandLookups();
        internal static readonly long TimeSpanMaxTicks = 3599400000000L;
        internal static readonly int OffsetMaxValue = 9959;
        private readonly DateTime _date;
        private readonly TimeSpan _timeZone;
        private readonly bool _unknownTimeZone;

        internal SmtpDateTime(DateTime value)
        {
            _date = value;
            switch (value.Kind)
            {
                case DateTimeKind.Unspecified:
                    _unknownTimeZone = true;
                    return;
                case DateTimeKind.Utc:
                    _timeZone = TimeSpan.Zero;
                    return;
                case DateTimeKind.Local:
                {
                    TimeSpan utcOffset = TimeZoneInfo.Local.GetUtcOffset(value);
                    _timeZone = ValidateAndGetSanitizedTimeSpan(utcOffset);
                    return;
                }
                default:
                    return;
            }
        }

        internal SmtpDateTime(string value)
        {
            string timeZoneString;
            _date = ParseValue(value, out timeZoneString);
            if (!TryParseTimeZoneString(timeZoneString, out _timeZone))
            {
                _unknownTimeZone = true;
            }
        }

        internal DateTime Date
        {
            get
            {
                if (_unknownTimeZone)
                {
                    return DateTime.SpecifyKind(_date, DateTimeKind.Unspecified);
                }
                var dateTimeOffset = new DateTimeOffset(_date, _timeZone);
                return dateTimeOffset.LocalDateTime;
            }
        }

        internal static IDictionary<string, TimeSpan> InitializeShortHandLookups()
        {
            return new Dictionary<string, TimeSpan>
            {
                {
                    "UT",
                    TimeSpan.Zero
                },

                {
                    "GMT",
                    TimeSpan.Zero
                },

                {
                    "EDT",
                    new TimeSpan(-4, 0, 0)
                },

                {
                    "EST",
                    new TimeSpan(-5, 0, 0)
                },

                {
                    "CDT",
                    new TimeSpan(-5, 0, 0)
                },

                {
                    "CST",
                    new TimeSpan(-6, 0, 0)
                },

                {
                    "MDT",
                    new TimeSpan(-6, 0, 0)
                },

                {
                    "MST",
                    new TimeSpan(-7, 0, 0)
                },

                {
                    "PDT",
                    new TimeSpan(-7, 0, 0)
                },

                {
                    "PST",
                    new TimeSpan(-8, 0, 0)
                }
            };
        }

        public override string ToString()
        {
            if (_unknownTimeZone)
            {
                return string.Format("{0} {1}", FormatDate(_date), "-0000");
            }
            return string.Format("{0} {1}", FormatDate(_date), TimeSpanToOffset(_timeZone));
        }

        internal void ValidateAndGetTimeZoneOffsetValues(string offset, out bool positive, out int hours,
            out int minutes)
        {
            if (offset.Length != 5)
            {
                throw new FormatException();
            }
            positive = offset.StartsWith("+");
            if (!int.TryParse(offset.Substring(1, 2), NumberStyles.None, CultureInfo.InvariantCulture, out hours))
            {
                throw new FormatException();
            }
            if (!int.TryParse(offset.Substring(3, 2), NumberStyles.None, CultureInfo.InvariantCulture, out minutes))
            {
                throw new FormatException();
            }
            if (minutes > 59)
            {
                throw new FormatException();
            }
        }

        internal void ValidateTimeZoneShortHandValue(string value)
        {
            if (value.ToCharArray().Where((t, i) => !char.IsLetter(value, i)).Any())
            {
                throw new FormatException();
            }
        }

        internal string FormatDate(DateTime value)
        {
            return value.ToString("ddd, dd MMM yyyy H:mm:ss", CultureInfo.InvariantCulture);
        }

        internal DateTime ParseValue(string data, out string timeZone)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new FormatException();
            }
            int num = data.IndexOf(':');
            if (num == -1)
            {
                throw new FormatException();
            }
            int num2 = data.IndexOfAny(AllowedWhiteSpaceChars, num);
            if (num2 == -1)
            {
                throw new FormatException();
            }
            string s = data.Substring(0, num2).Trim();
            DateTime result;
            if (
                !DateTime.TryParseExact(s, ValidDateTimeFormats, CultureInfo.InvariantCulture,
                    DateTimeStyles.AllowWhiteSpaces, out result))
            {
                throw new FormatException();
            }
            string text = data.Substring(num2).Trim();
            int num3 = text.IndexOfAny(AllowedWhiteSpaceChars);
            if (num3 != -1)
            {
                text = text.Substring(0, num3);
            }
            if (string.IsNullOrEmpty(text))
            {
                throw new FormatException();
            }
            timeZone = text;
            return result;
        }

        internal bool TryParseTimeZoneString(string timeZoneString, out TimeSpan timeZone)
        {
            timeZone = TimeSpan.Zero;
            if (timeZoneString == "-0000")
            {
                return false;
            }
            if (timeZoneString[0] == '+' || timeZoneString[0] == '-')
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
                    else
                    {
                        if (num2 != 0)
                        {
                            num2 *= -1;
                        }
                    }
                }
                timeZone = new TimeSpan(num, num2, 0);
                return true;
            }
            ValidateTimeZoneShortHandValue(timeZoneString);
            if (TimeZoneOffsetLookup.ContainsKey(timeZoneString))
            {
                timeZone = TimeZoneOffsetLookup[timeZoneString];
                return true;
            }
            return false;
        }

        internal TimeSpan ValidateAndGetSanitizedTimeSpan(TimeSpan span)
        {
            var result = new TimeSpan(span.Days, span.Hours, span.Minutes, 0, 0);
            if (Math.Abs(result.Ticks) > TimeSpanMaxTicks)
            {
                throw new FormatException();
            }
            return result;
        }

        internal string TimeSpanToOffset(TimeSpan span)
        {
            if (span.Ticks == 0L)
            {
                return "+0000";
            }
            var num = (uint) Math.Abs(Math.Floor(span.TotalHours));
            var num2 = (uint) Math.Abs(span.Minutes);
            string str = (span.Ticks > 0L) ? "+" : "-";
            if (num < 10u)
            {
                str += "0";
            }
            str += num.ToString();
            if (num2 < 10u)
            {
                str += "0";
            }
            return str + num2;
        }
    }
}