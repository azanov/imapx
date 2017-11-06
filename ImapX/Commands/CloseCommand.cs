using ImapX.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImapX.Commands
{
    public class CloseCommand : FolderCommand
    {
        public CloseCommand(ImapClient client, long id, Folder folder) : base(client, id, folder, string.Format("CLOSE {0}\r\n", folder.Path.Quote()))
        {
        }

        public override void OnCommandComplete()
        {
            Folder.RemoveExpungedMessages();
        }
    }
}
