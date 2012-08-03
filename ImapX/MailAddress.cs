namespace ImapX
{
    public class MailAddress
    {
    	public string DisplayName { get; set; }

    	public string Address { get; set; }

    	public MailAddress()
        {
            this.DisplayName = string.Empty;
            this.Address = string.Empty;
        }

        public MailAddress(string display, string addr)
        {
            this.Address = addr;
            this.DisplayName = display;
        }

        public override string ToString()
        {
            if (this.DisplayName != string.Empty)
            {
                return string.Format("{0} <{1}>", this.DisplayName, this.Address);
            }
            return this.Address;
        }
    }
}
