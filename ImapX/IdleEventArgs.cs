using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImapX
{
    public class IdleEventArgs : EventArgs
    {
        public ImapClient Client { get; set; }
        public Folder Folder { get; set; }
        public Message[] Messages { get; set; }
    }
}
