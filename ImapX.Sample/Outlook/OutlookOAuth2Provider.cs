using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ImapX.Sample.Outlook
{
    public class OutlookOAuth2Provider
    {
        const string AUTH_URI = "https://login.live.com/oauth20_authorize.srf";
        public const string REDIRECT_URI = "https://login.live.com/oauth20_desktop.srf";

        const string GET_ACCESS_TOKEN_URI = "https://login.live.com/oauth20_token.srf";
        //const string GET_USER_PROFILE_URI = "https://www.googleapis.com/oauth2/v1/userinfo";

        const string CLIENT_ID = "0000000048141A43";
        const string CLIENT_SECRET = "4v0QVVOujbR7omyk3FJBgwft2hi1Oer-";


        const string SCOPES = "wl.basic,wl.emails,wl.imap,wl.offline_access";
        const string RESPONSE_TYPE_CODE = "code";

        public static Uri BuildAuthenticationUri()
        {
            return new Uri(string.Format("{0}?client_id={1}&scope={2}&response_type={3}&redirect_uri={4}",
                                            AUTH_URI,
                                            CLIENT_ID,
                                            SCOPES,
                                            RESPONSE_TYPE_CODE,
                                            REDIRECT_URI));
        }

        public static OutlookAccessToken GetAccessToken(string code)
        {
            string postData = string.Format("client_id={0}&redirect_uri={1}&client_secret={2}&code={3}&grant_type=authorization_code", CLIENT_ID, REDIRECT_URI, CLIENT_SECRET, code);


            HttpWebRequest request = (HttpWebRequest)
            WebRequest.Create(GET_ACCESS_TOKEN_URI); request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = "POST";


            byte[] postBytes = Encoding.UTF8.GetBytes(postData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postBytes.Length;
            Stream requestStream = request.GetRequestStream();

            requestStream.Write(postBytes, 0, postBytes.Length);
            requestStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (var responseReader = new StreamReader(response.GetResponseStream()))
            {
                var tmp = responseReader.ReadToEnd();
                return JsonHelper.From<OutlookAccessToken>(tmp);
            }
        }

        public static OutlookProfile GetUserProfile(string accessToken)
        {
            var w = new WebClient();

            string url = "https://apis.live.net/v5.0/me/?access_token=" + accessToken;

            w.Encoding = Encoding.UTF8;
            string profile = w.DownloadString(url);

            return JsonHelper.From<OutlookProfile>(profile);
        }
    }
}
