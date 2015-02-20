using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImapX.Sample.Yahoo
{
    public class YahooAccessToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public string xoauth_yahoo_guid { get; set; }
    }
}
