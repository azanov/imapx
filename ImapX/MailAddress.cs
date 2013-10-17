using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImapX
{
    public class MailAddress
    {
        public MailAddress()
        {
            DisplayName = string.Empty;
            Address = string.Empty;
        }

        public MailAddress(string display, string addr)
        {
            Address = (addr ?? "").Trim(' ', '\n', '\t');
            DisplayName = (display ?? "").Trim(' ', '\n', '\t');
        }

        public string DisplayName { get; set; }

        public string Address { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(DisplayName) ? Address : string.Format("{0} <{1}>", DisplayName, Address);
        }

        public static explicit operator MailAddress(System.Net.Mail.MailAddress address)
        {
            return new MailAddress(address.DisplayName, address.Address);
        }

    }
}
