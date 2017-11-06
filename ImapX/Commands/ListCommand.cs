using ImapX.Enums;
using ImapX.Exceptions;
using ImapX.Extensions;
using ImapX.Flags;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ImapX.Commands
{
    public class ListCommand : FolderCommand<List<Folder>>
    {
        protected FolderTreeBrowseMode _mode;

        internal virtual string ResponseToken { get { return "LIST"; } }

        public ListCommand (ImapClient client, long id, Folder parentFolder, FolderTreeBrowseMode mode = FolderTreeBrowseMode.Lazy, string command = "LIST") 
            : base(client, id, parentFolder, string.Empty)
        {
            _mode = mode;
            Response = new List<Folder>();

            var specialUse = client.Capabilities.SpecialUse ? "SPECIAL-USE" : null;
            var listStatus = client.Capabilities.ListStatus && client.Behavior.ListFolderStatusType != FolderStatusType.None
                ? string.Format("STATUS {0}", client.Behavior.ListFolderStatusType.ToCommandParameter())
                      : "";

            Parts[0] += string.Format("{0} {1} {2}{3}\r\n",
                      command, (parentFolder == null ? "" : parentFolder.Path + client.Behavior.FolderDelimeter).Quote(),
                      (char)mode,
                      string.IsNullOrWhiteSpace(specialUse) && string.IsNullOrWhiteSpace(listStatus)
                        ? string.Empty : string.Format(" RETURN ({0})", string.Join(" ", specialUse, listStatus).Trim())
                  );

        }
        
        public override bool HandleSpecificUntaggedResponse(ImapParser io, ImapToken responseToken)
        {
            if (responseToken.Value != ResponseToken)
                return base.HandleSpecificUntaggedResponse(io, responseToken);
            
            ImapToken token;
            io.ConsumeToken(TokenType.OpeningParenthesis);

            var folder = new Folder(Client) {
                ParentFolder = Folder,
            };

            while ((token = io.ReadToken()).Type == TokenType.Flag)
            {
                switch (token.Value)
                {
                    case FolderFlags.All:
                    case FolderFlags.XAllMail:
                        folder.Type = SpecialFolderType.All; break;

                    case FolderFlags.Archive:
                        folder.Type = SpecialFolderType.Archive; break;

                    case FolderFlags.Drafts:
                        folder.Type = SpecialFolderType.Drafts; break;

                    case FolderFlags.Flagged:
                    case FolderFlags.XStarred:
                        folder.Type = SpecialFolderType.Flagged; break;

                    case FolderFlags.HasChildren:
                        folder.HasChildren = true; break;

                    case FolderFlags.HasNoChildren:
                        folder.HasChildren = false; break;

                    case FolderFlags.Junk:
                    case FolderFlags.XSpam:
                        folder.Type = SpecialFolderType.Junk; break;

                    case FolderFlags.Marked:
                        folder.Marked = true;
                        break;

                    case FolderFlags.NoInferiors:
                        folder.CanHaveChildren = false; break;

                    case FolderFlags.NoSelect:
                        folder.Selectable = false; break;

                    case FolderFlags.Sent:
                        folder.Type = SpecialFolderType.Sent; break;

                    case FolderFlags.Trash:
                        folder.Type = SpecialFolderType.Trash; break;

                    case FolderFlags.Unmarked:
                        // Folders are not marked by default
                        break;

                    case FolderFlags.XInbox:
                        folder.Type = SpecialFolderType.Inbox; break;

                }
                folder.Flags.AddInternal(token.Value);
            }

            if (token.Type != TokenType.ClosingParenthesis)
                throw new UnexpectedTokenException();

            // read folder delimiter
            Base.Behavior.FolderDelimeter = io.ReadNullableString();

            // read folder path
            folder.Path = io.ReadString();

            if (folder.Path == "INBOX")
                folder.Type = SpecialFolderType.Inbox;
            else if (folder.Type == SpecialFolderType.Inbox)
                folder.Path = "INBOX";

            // TODO: Build tree structure if the mode == *
            
            Response.Add(folder);
            Folder = folder;

            if (Client.Behavior.ExamineFolders && folder.Selectable)
                folder.ScheduleExamine();

            return true;
        }
    }
}
