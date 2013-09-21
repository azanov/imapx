namespace System.Net.Mail
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
            Address = addr;
            DisplayName = display;
        }

        public string DisplayName { get; set; }

        public string Address { get; set; }

        public override string ToString()
        {
            return DisplayName != string.Empty ? string.Format("{0} <{1}>", DisplayName, Address) : Address;
        }
    }
}