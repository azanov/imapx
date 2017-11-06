using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImapX.Commands
{
    /// <summary>
    /// Dummy command to handle the connect response
    /// </summary>
    public class ConnectCommand : ImapCommand
    {
        public ConnectCommand(ImapBase imapBase, long id) : base(imapBase, id, string.Empty)
        {
            Parts[0] = string.Empty;
            BreakAfterUntagged = true;
        }
    }
}
