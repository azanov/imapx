using System;

namespace ImapX
{
    [Serializable]
    public class MailAddress
    {
        public MailAddress()
        {
            DisplayName = string.Empty;
            Address = string.Empty;
        }

        public MailAddress(string display, string addr)
        {
            Address = addr;
            DisplayName = display;
        }

        public string DisplayName { get; set; }

        public string Address { get; set; }

        public override string ToString()
        {
            if (DisplayName != string.Empty)
            {
                return string.Format("{0} <{1}>", DisplayName, Address);
            }
            return Address;
        }
    }
}