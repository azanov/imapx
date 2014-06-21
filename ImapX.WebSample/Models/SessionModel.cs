using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ImapX.WebSample.Models
{
    public class SessionModel
    {

        public ImapClient Client
        {
            get
            {
                return Get<ImapClient>("ImapClient");
            }
            set
            {
                Set("ImapClient", value);
            }
        }

        #region Session management

        public static HttpSessionState Current
        {
            get
            {
                return HttpContext.Current.Session;
            }
        }

        public static void Set(string key, dynamic value)
        {
            Current[key] = value;
        }

        public static T Get<T>(string key)
        {
            return (T)(Current[key] ?? default(T));
        }

        #endregion

    }
}