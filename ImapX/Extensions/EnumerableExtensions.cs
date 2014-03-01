using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace ImapX.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<string> GroupUIdSequences(this IEnumerable<long> data)
        {
            long? previous = null;
            int index = 0;
            IEnumerable<string> tmp = data.OrderBy(_ => _).Select(value =>
            {
                if (previous.HasValue)
                    index = (value - previous == 1) ? index : index + 1;
                previous = value;
                return new KeyValuePair<int, long>(index, value);
            }).GroupBy(_ => _.Key).Select(_ => _.First().Value + ":" + _.Last().Value);
            return tmp;
        }

#if !WINDOWS_PHONE && !NETFX_CORE

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this NameValueCollection col)
        {
            var dict = new Dictionary<TKey, TValue>();
            TypeConverter keyConverter = TypeDescriptor.GetConverter(typeof(TKey));
            TypeConverter valueConverter = TypeDescriptor.GetConverter(typeof(TValue));

            foreach (string name in col)
            {
                var key = (TKey)keyConverter.ConvertFromString(name);
                var value = (TValue)valueConverter.ConvertFromString(col[name]);
                dict.Add(key, value);
            }

            return dict;
        }

#endif

        public static List<MailAddress> ToMailAddressList(this MailAddressCollection col)
        {
            return col.Select(address => new MailAddress(address.DisplayName, address.Address)).ToList();
        }
    }
}
