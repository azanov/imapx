using System;
using System.Collections.Generic;

namespace ImapX
{
    public class IdleEventArgs : EventArgs
    {
        public ImapClient Client { get; set; }
        public Folder Folder { get; set; }
        public IEnumerable<Message> Messages { get; set; }
    }
}
