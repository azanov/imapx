using ImapX.Enums;

namespace ImapX
{
    public class FolderStatus
    {
        public FolderStatus()
        {
            AppendLimit = -1;
        }

        public FolderStatusType Type { get; set; }

        /// <summary>
        /// The number of messages in the folder.
        /// </summary>
        public long TotalMessageCount { get; set; }

        /// <summary>
        /// The number of messages with the \Recent flag set.
        /// </summary>
        public long RecentMessageCount { get; set; }

        /// <summary>
        /// The number of messages which do not have the \Seen flag set.
        /// </summary>
        public long UnseenMessagesCount { get; set; }

        /// <summary>
        /// The next unique identifier value of the mailbox.
        /// </summary>
        public long UIdNext { get; set; }

        /// <summary>
        ///  The unique identifier validity value of the mailbox.
        /// </summary>
        public long UIdValidity { get; set; }

        /// <summary>
        ///  Maximum message size in octets that the server will accept. 
        ///  An APPENDLIMIT number of 0 indicates the server will not accept any APPEND commands 
        ///  at all for the affected mailboxes
        /// </summary>
        public long AppendLimit { get; set; }
    }
}
