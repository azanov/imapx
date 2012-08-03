namespace ImapX
{
    public class Command
    {
        public const string CLOSE = "CLOSE";
        public const string APPEND = "APPEND";
        public const string COPY = "COPY";
        public const string DELETE = "DELETE";
        public const string CREATE = "CREATE";
        public const string UID = "UID";
        public const string EXPUNGE = "EXPUNGE";
        public const string EXAMINE = "EXAMINE";
        public const string STORE = "STORE";
        public const string FETCH = "FETCH";
        public const string SEARCH = "SEARCH";
        public const string LIST = "LIST";
        public const string SELECT = "SELECT";
        public const string LOGIN = "LOGIN";
        public const string LOGOUT = "LOGOUT";
        public const string IDENTIFIER = "IMAP00";
        public const string CAPABILITY = "CAPABILITY";
        public const string COMMAND_ENTER = "\r\n";
    }
}
