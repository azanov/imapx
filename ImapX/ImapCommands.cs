namespace ImapX
{
    public sealed class ImapCommands
    {
        public const string STORE = "UID STORE {0} {1} ({2})";

        public const string CAPABILITY = "CAPABILITY";

        public const string LOGIN = "LOGIN \"{0}\" \"{1}\"";

        public const string LOGOUT = "LOGOUT";

        public const string AUTHENTICATE = "AUTHENTICATE {0}";

        public const string LIST = "LIST \"{0}\" {1}";

        public const string X_LIST = "XLIST \"{0}\" {1}";

        public const string RENAME = "RENAME \"{0}\" \"{1}\"";

        public const string DELETE = "DELETE \"{0}\"";

        public const string EXAMINE = "EXAMINE \"{0}\"";

        public const string CREATE = "CREATE \"{0}\"";

        public const string SEARCH = "UID SEARCH {0}";

        public const string SELECT = "SELECT \"{0}\"";

        public const string SET_META_DATA = "SETMETADATA \"{0}\" ({1} {2})";
    }
}