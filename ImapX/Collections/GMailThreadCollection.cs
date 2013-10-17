using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImapX.Collections
{
    public class GMailThreadCollection : ImapObjectCollection<GMailMessageThread>
    {
        public GMailThreadCollection()
            : base(null)
        {

        }
    }
}
