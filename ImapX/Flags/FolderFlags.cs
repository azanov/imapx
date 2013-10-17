namespace ImapX.Flags
{
    /// <summary>
    /// Predefined system folder flags and additional flags returned by the XLIST command
    /// </summary>
    public sealed class FolderFlags
    {

        public const string All = @"\All";

        public const string Archive = @"\Archive";

        public const string Drafts = @"\Drafts";

        public const string Flagged = @"\Flagged";

        public const string HasChildren = @"\HasChildren";

        public const string HasNoChildren = @"\HasNoChildren";

        public const string Junk = @"\Junk";

        public const string Marked = @"\Marked";

        public const string NoInferiors = @"\Noinferiors";

        public const string NoSelect = @"\Noselect";

        public const string Sent = @"\Sent";

        public const string Trash = @"\Trash";

        public const string Unmarked = @"\Unmarked";



        public const string XAllMail = @"\AllMail";

        public const string XImportant = @"\Important";

        public const string XInbox = @"\Inbox";

        public const string XSpam = @"\Spam";

        public const string XStarred = @"\Starred";


    }
}