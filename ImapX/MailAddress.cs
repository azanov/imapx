using System;
namespace ImapX
{
    public class MailAddress
    {
        private string _address;
        private string _displayName;
        public string DisplayName
        {
            get
            {
                return this._displayName;
            }
            set
            {
                this._displayName = value;
            }
        }
        public string Address
        {
            get
            {
                return this._address;
            }
            set
            {
                this._address = value;
            }
        }
        public MailAddress()
        {
            this._displayName = string.Empty;
            this._address = string.Empty;
        }
        public MailAddress(string display, string addr)
        {
            this._address = addr;
            this._displayName = display;
        }
        public override string ToString()
        {
            if (!(this.DisplayName == string.Empty))
            {
                return string.Format("{0} <{1}>", this.DisplayName, this.Address);
            }
            return this.Address;
        }
    }
}
