using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImapX.Constants
{
    public sealed class ImapCommands
    {

        public const string Store = "UID STORE {0} {1} ({2})";

        public const string Capability = "CAPABILITY";

        public const string Login = "LOGIN \"{0}\" \"{1}\"";

        public const string Logout = "LOGOUT";

        public const string Authenticate = "AUTHENTICATE {0}";

        public const string List = "LIST \"{0}\" {1}";

        public const string XList = "XLIST \"{0}\" {1}";

        public const string Rename = "RENAME \"{0}\" \"{1}\"";

        public const string Delete = "DELETE \"{0}\"";

        public const string Examine = "EXAMINE \"{0}\"";

        public const string Status = "STATUS \"{0}\" ({1})";

        public const string Expunge = "EXPUNGE";

        public const string Create = "CREATE \"{0}\"";

        public const string Search = "UID SEARCH {0}";

        public const string Copy = "UID COPY {0} \"{1}\"";

        public const string Fetch = "UID FETCH {0} ({1})";

        public const string Select = "SELECT \"{0}\"";

        public const string SetMetaData = "SETMETADATA \"{0}\" ({1} {2})";

        public const string GetQuotaRoot = "GETQUOTAROOT {0}";

        public const string Append = "APPEND \"{0}\"";

        public const string Compress = "COMPRESS \"{0}\"";

        public const string Noop = "NOOP";
    }
}
