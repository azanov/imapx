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
        
        
    }
}
