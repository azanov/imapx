using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImapX.Commands
{
    public class CompressCommand : ImapCommand
    {
        public CompressCommand(ImapBase imapBase, long id) : base(imapBase, id, "COMPRESS DEFLATE\r\n")
        {
        }
    }
}
