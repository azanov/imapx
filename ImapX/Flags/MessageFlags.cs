using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImapX.Flags
{
    /// <summary>
    /// Predefined system Message flags
    /// </summary>
    public sealed class MessageFlags
    {
        /// <summary>
        /// Message has been read.
        /// </summary>
        public const string Seen = @"\Seen";

        /// <summary>
        /// Message has been answered.
        /// </summary>
        public const string Answered = @"\Answered";

        /// <summary>
        /// Message is "flagged" for urgent/special attention.
        /// </summary>
        public const string Flagged = @"\Flagged";

        /// <summary>
        /// Message is "deleted" for removal by later EXPUNGE.
        /// </summary>
        public const string Deleted = @"\Deleted";

        /// <summary>
        /// Message has not completed composition (marked as a draft).
        /// </summary>
        public const string Draft = @"\Draft";

        /// <summary>
        /// Message is "recently" arrived in this mailbox. 
        /// WARNING: This flag can not be altered by the client.
        /// </summary>
        public const string Recent = @"\Recent";

    }
}
