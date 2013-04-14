using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ImapX.Sample.Google
{
    public class GoogleOAuth2Provider
    {

        const string AUTH_URI = "https://accounts.google.com/o/oauth2/auth";
        const string REDIRECT_URI = "urn:ietf:wg:oauth:2.0:oob";
        const string GET_ACCESS_TOKEN_URI = "https://accounts.google.com/o/oauth2/token";
        const string GET_USER_PROFILE_URI = "https://www.googleapis.com/oauth2/v1/userinfo";

        const string CLIENT_ID = "819410764762.apps.googleusercontent.com";
        const string CLIENT_SECRET = "vz1VGPT2meoSJ5RXBco-56aR";

       
        const string SCOPE_USER_EMAIL = "https://www.googleapis.com/auth/userinfo.email";
        const string SCOPE_GMAIL = "https://mail.google.com/";
        const string RESPONSE_TYPE_CODE = "code";

        public static Uri BuildAuthenticationUri()
        {
            return new Uri(string.Format("{0}?response_type={1}&client_id={2}&redirect_uri={3}&scope={4}%20{5}", 
                                            AUTH_URI, 
                                            RESPONSE_TYPE_CODE, 
                                            CLIENT_ID, 
                                            REDIRECT_URI, 
                                            SCOPE_USER_EMAIL,
                                            SCOPE_GMAIL));
        }
        
        public static GoogleAccessToken GetAccessToken(string code)
        {
            string postData = string.Format("client_id={0}&client_secret={1}&grant_type=authorization_code&code={2}&redirect_uri={3}", CLIENT_ID, CLIENT_SECRET, code, REDIRECT_URI);


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
            using(var responseReader = new StreamReader(response.GetResponseStream()))
            {
                var tmp = responseReader.ReadToEnd();
                return JsonHelper.From<GoogleAccessToken>(tmp);
            }
        }

        public static GoogleProfile GetUserProfile(GoogleAccessToken accessToken)
        {
            using (var w = new WebClient())
            {
                var url = string.Format("{0}?access_token=", GET_USER_PROFILE_URI, accessToken.access_token);

                w.Headers.Add("Authorization", string.Format("Bearer  {0}", accessToken.access_token));

                w.Encoding = Encoding.UTF8;
                var tmp = w.DownloadString(url);

                return JsonHelper.From<GoogleProfile>(tmp);
            }
        }

    }
}
