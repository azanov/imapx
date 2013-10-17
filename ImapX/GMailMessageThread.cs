using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImapX.Collections;
using ImapX.Enums;

namespace ImapX
{
    public class GMailMessageThread
    {

        internal GMailMessageThread()
        {
        }

        internal GMailMessageThread(ImapClient client, Folder folder, long threadId)
        {
            Id = threadId;
            Messages = new MessageCollection(client, folder);
        }

        /// <summary>
        /// The Id of the current thread
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// Messages associated with the current thread. The collection is being populated when new messages are downloaded. To fetch all messages use <code>FetchAssocicatedMessages</code>
        /// </summary>
        public MessageCollection Messages { get; set; }

        /// <summary>
        /// Downloads all messages associated with the current thread
        /// </summary>
        /// <param name="mode">The message fetch mode, allows to select which parts of the message will be requested.</param>
        /// <param name="count">
        ///     The maximum number of messages that will be requested. Set <code>count</code> to <code>-1</code>
        ///     will request all messages which match the given query.
        /// </param>
        public void FetchAssocicatedMessages(MessageFetchMode mode = MessageFetchMode.ClientDefault,
            int count = -1)
        {
            Messages.Download("X-GM-THRID " + Id, mode, count);
        }

    }
}
