using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImapX.Authentication;
using ImapX.Extensions;
using System.IO;
using ImapX.EncodingHelpers;

namespace ImapX.Commands
{
    public class AuthenticateOAuth2Command : AutheticateCommand
    {
        public AuthenticateOAuth2Command(ImapBase imapBase, long id, OAuth2Credentials credentials) : base(imapBase, id, "XOAUTH2")
        {
            var userData = Encoding.UTF8.GetBytes("user=" + credentials.Login);
            var tokenLabelData = Encoding.UTF8.GetBytes("auth=Bearer ");
            var vendorData = string.IsNullOrEmpty(credentials.Vendor) ? null : Encoding.UTF8.GetBytes("vendor=" + credentials.Vendor + "\n");
            var tokenData = Encoding.UTF8.GetBytes(credentials.AuthToken + (vendorData == null ? "\n" : ""));

            using (var stream = new MemoryStream())
            {
                stream.Write(userData, 0, userData.Length);
                stream.WriteByte(1);
                stream.Write(tokenLabelData, 0, tokenLabelData.Length);
                stream.Write(tokenData, 0, tokenData.Length);
                stream.WriteByte(1);
                if (vendorData != null)
                {
                    stream.Write(vendorData, 0, vendorData.Length);
                    stream.WriteByte(1);
                }
                stream.WriteByte(1);
                Parts.Add(Base64.ToBase64(stream.ToArray()) + "\r\n");
            }            
        }

        public override void Continue(string serverResponse)
        {
            base.Continue(serverResponse);
        }
    }
}
