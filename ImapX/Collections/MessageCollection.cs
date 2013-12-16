using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImapX.Enums;

namespace ImapX.Collections
{
    public class MessageCollection : ImapObjectCollection<Message>
    {
        private readonly Folder _folder;

        public MessageCollection(ImapClient client, Folder folder)
            : base(client)
        {
            _folder = folder;
        }

        /// <summary>
        ///     Downloads messages from server using default or given mode.
        /// </summary>
        /// <param name="query">The search query to filter messages. <code>ALL</code> by default</param>
        /// <param name="mode">The message fetch mode, allows to select which parts of the message will be requested.</param>
        /// <param name="count">
        ///     The maximum number of messages that will be requested. Set <code>count</code> to <code>-1</code>
        ///     will request all messages which match the given query.
        /// </param>
        public void Download(string query = "ALL", MessageFetchMode mode = MessageFetchMode.ClientDefault,
            int count = -1)
        {
            _folder.Search(query, mode, count);
        }

        /// <summary>
        ///     Downloads messages by their UIds from server using default or given mode.
        /// </summary>
        /// <param name="uIds">The uIds of the messages to download.</param>
        /// <param name="mode">The message fetch mode, allows to select which parts of the message will be requested.</param>
        public void Download(long[] uIds, MessageFetchMode mode = MessageFetchMode.ClientDefault)
        {
            _folder.Search(uIds, mode);
        }
    }
}
