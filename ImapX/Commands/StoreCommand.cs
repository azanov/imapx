using ImapX.Enums;
using System.Collections.Generic;

namespace ImapX.Commands
{
    public class StoreCommand : FetchCommand
    {
        public StoreCommand(ImapClient client, long id, Message message, StoreAction action, IEnumerable<string> flags) 
            : base(client, id, message, string.Empty)
        {
            var prefix = string.Empty;
            switch (action)
            {
                case StoreAction.Add:
                    prefix = "+"; break;

                case StoreAction.Delete:
                    prefix = "-"; break;
            }

            Parts[0] += string.Format("UID STORE {0} {1}FLAGS ({2})\r\n", message.UId, prefix, string.Join(" ", flags));
        }
    }
}
