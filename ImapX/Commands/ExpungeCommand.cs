using System.Collections.Generic;

namespace ImapX.Commands
{
    public class ExpungeCommand : FolderCommand<List<long>>
    {
        public ExpungeCommand(ImapClient client, long id, Folder folder) : base(client, id, folder, "EXPUNGE\r\n")
        {
        }
        
        public override bool HandleSpecificUntaggedResponse(ImapParser io, ImapToken responseToken)
        {
            var token = io.PeekToken();
            if (responseToken.Type == TokenType.Number && token.Value == "EXPUNGE")
            {
                var id = int.Parse(responseToken.Value);
                Folder.RemoveExpungedMessage(id);
                io.ReadToken();
                return true;
            }
            return base.HandleSpecificUntaggedResponse(io, responseToken);
        }
    }
}
