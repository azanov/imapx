using System;
using System.ComponentModel;

namespace ImapX.Enums
{
    [Flags, DefaultValue(None)]
    public enum FolderStatusType : int
    {
        None = 0,

        /// <summary>
        /// The number of messages in the folder.
        /// </summary>
        TotalMessageCount = 1,

        /// <summary>
        /// The number of messages with the \Recent flag set.
        /// </summary>
        RecentMessageCount = 2,

        /// <summary>
        /// The number of messages which do not have the \Seen flag set.
        /// </summary>
        UnseenMessagesCount = 4,

        /// <summary>
        /// The next unique identifier value of the mailbox.
        /// </summary>
        UIdNext = 8,

        /// <summary>
        ///  The unique identifier validity value of the mailbox.
        /// </summary>
        UIdValidity = 16,

        /// <summary>
        ///  Maximum message size in octets that the server will accept. 
        ///  An APPENDLIMIT number of 0 indicates the server will not accept any APPEND commands 
        ///  at all for the affected mailboxes
        /// </summary>
        AppendLimit = 32,

        RecentUnseenTotalCount = TotalMessageCount | RecentMessageCount | UnseenMessagesCount,

        /// <summary>
        /// The next unique identifier and its validity value of the mailbox.
        /// </summary>
        UId = UIdNext | UIdValidity,

        All = RecentUnseenTotalCount | UId | AppendLimit
    }
}
