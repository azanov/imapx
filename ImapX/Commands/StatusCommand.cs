using ImapX.Enums;
using ImapX.Extensions;

namespace ImapX.Commands
{
    public class StatusCommand : FolderCommand<FolderStatus>
    {
        public StatusCommand(ImapClient client, long id, Folder folder, FolderStatusType type) 
            : base(client, id, folder, string.Format("STATUS {0} {1}\r\n", folder.Path.Quote(), type.ToCommandParameter()))
        {
        }
        
        public override bool HandleSpecificUntaggedResponse(ImapParser io, ImapToken responseToken)
        {
            if (responseToken.Value != "STATUS")
                base.HandleSpecificUntaggedResponse(io, responseToken);

            Response = new FolderStatus();

            ImapToken token;
            io.ReadString();

            io.ConsumeOpeningParenthesis();

            while((token = io.PeekToken()).Type == TokenType.Atom)
            {
                var fieldName = io.ReadToken().Value;
                var value = io.ReadLong();

                switch(fieldName)
                {
                    case "RECENT":
                        Folder.Recent = value;
                        Response.RecentMessageCount = value;
                        Response.Type |= FolderStatusType.RecentMessageCount;
                        break;

                    case "MESSAGES":
                        Response.TotalMessageCount = value;
                        Response.Type |= FolderStatusType.TotalMessageCount;
                        break;

                    case "UIDNEXT":
                        Folder.UidNext = value;
                        Response.RecentMessageCount = value;
                        Response.Type |= FolderStatusType.RecentMessageCount;
                        break;

                    case "UIDVALIDITY":
                        Folder.UidValidity = value;
                        Response.UIdValidity = value;
                        Response.Type |= FolderStatusType.UIdValidity;
                        break;

                    case "UNSEEN":
                        Folder.Unseen = value;
                        Response.UnseenMessagesCount = value;
                        Response.Type |= FolderStatusType.UnseenMessagesCount;
                        break;

                    case "APPENDLIMIT":
                        Folder.AppendLimit = value;
                        Response.AppendLimit = value;
                        Response.Type |= FolderStatusType.AppendLimit;
                        break;
                } 
            }

            io.ConsumeClosingParenthesis();

            return true;
        }
    }
}
