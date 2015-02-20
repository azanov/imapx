using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using ImapX.EncodingHelpers;

namespace ImapX.Sample.Yahoo
{
    public class YahooOAuth2Provider
    {
        const string AUTH_URI = "https://api.login.yahoo.com/oauth2/request_auth";
        public const string REDIRECT_URI = "http://imapx.org/oauth2.html";

        const string GET_ACCESS_TOKEN_URI = "https://api.login.yahoo.com/oauth2/get_token";
        //const string GET_USER_PROFILE_URI = "https://www.googleapis.com/oauth2/v1/userinfo";

        const string CLIENT_ID = "dj0yJmk9YWFoWEFpNXY0M0djJmQ9WVdrOVRqZFlTVWhWTXpJbWNHbzlNQS0tJnM9Y29uc3VtZXJzZWNyZXQmeD1iMQ--";
        const string CLIENT_SECRET = "76725cc471481ff590cbc0fb2c23622af28c7e24";

        const string RESPONSE_TYPE_CODE = "code";

        public static Uri BuildAuthenticationUri()
        {
            return new Uri(string.Format("{0}?client_id={1}&response_type={2}&redirect_uri={3}&language=en-us",
                                            AUTH_URI,
                                            CLIENT_ID,
                                            RESPONSE_TYPE_CODE,
                                            REDIRECT_URI));
        }

        public static YahooAccessToken GetAccessToken(string code)
        {
            string postData = string.Format("client_id={0}&redirect_uri={1}&client_secret={2}&code={3}&grant_type=authorization_code", CLIENT_ID, REDIRECT_URI, CLIENT_SECRET, code);


            HttpWebRequest request = (HttpWebRequest)
            WebRequest.Create(GET_ACCESS_TOKEN_URI); request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = "POST";
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(CLIENT_ID + ":" + CLIENT_SECRET)));

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
                return JsonHelper.From<YahooAccessToken>(tmp);
            }
        }

        //public static OutlookProfile GetUserProfile(string accessToken)
        //{
        //    var w = new WebClient();

        //    string url = "https://apis.live.net/v5.0/me/?access_token=" + accessToken;

        //    w.Encoding = Encoding.UTF8;
        //    string profile = w.DownloadString(url);

        //    return JsonHelper.From<OutlookProfile>(profile);
        //}
    }
}
