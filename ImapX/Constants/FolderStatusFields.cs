using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImapX.Constants
{
    public static class FolderStatusFields
    {
        /// <summary>
        /// The number of messages in the mailbox.
        /// </summary>
        public const string Messages = "MESSAGES";

        /// <summary>
        /// The number of messages with the \Recent flag set.
        /// </summary>
        public const string Recent = "RECENT";

        /// <summary>
        /// The next unique identifier value of the mailbox.
        /// </summary>
        public const string UIdNext = "UIDNEXT";

        /// <summary>
        ///  The unique identifier validity value of the mailbox.
        /// </summary>
        public const string UIdValidity = "UIDVALIDITY";

        /// <summary>
        /// The number of messages which do not have the \Seen flag set.
        /// </summary>
        public const string Unseen = "Unseen";

    }
}
