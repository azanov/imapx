using System.Collections.Generic;
using System.Linq;

namespace ImapX.Commands
{
    public class SearchCommand : FolderCommand<List<Message>>
    {
        public SearchCommand(ImapClient client, long id, Folder folder, string query) : base(client, id, folder, "")
        {
            Parts[0] += string.Format("{0}SEARCH {1}\r\n", client.Capabilities.UIdPlus ? "UID " : "", query);
        }

        public override bool HandleSpecificUntaggedResponse(ImapParser io, ImapToken responseToken)
        {
            if (responseToken.Value != "SEARCH")
                return base.HandleSpecificUntaggedResponse(io, responseToken);

            Response = new List<Message>();
            ImapToken token;
            while ((token = io.PeekToken()).Type == TokenType.Number)
            {
                var uIdOrSeqNumber = io.ReadLong();

                var msg = Folder.Messages.FirstOrDefault(_ => Client.Capabilities.UIdPlus ? _.UId == uIdOrSeqNumber : _.SequenceNumber == uIdOrSeqNumber);

                if (msg == null)
                {
                    msg = new Message(uIdOrSeqNumber, Client, Folder);
                    Folder.Messages.AddInternal(msg);
                }

                Response.Add(msg);
            }

            return true;
        }
    }
}
