using ImapX.Extensions;
using ImapX.Flags;

namespace ImapX.Commands
{
    public class DeleteCommand : FolderCommand
    {
        public DeleteCommand(ImapClient client, long id, Folder folder) : base(client, id, folder, string.Format("DELETE {0}", folder.Path.Quote()))
        {
        }

        public override void OnCommandComplete()
        {
            if (Folder.HasChildren)
            {
                /*
                      It is permitted to delete a name that has inferior hierarchical
                      names and does not have the \Noselect mailbox name attribute.  In
                      this case, all messages in that mailbox are removed, and the name
                      will acquire the \Noselect mailbox name attribute. 
                */

                Folder.Selectable = false;
                Folder.Flags.AddInternal(FolderFlags.NoSelect);
                
                Folder.Messages.ClearInternal();
            }
            else if (Folder.ParentFolder != null)
                Folder.ParentFolder.SubFolders.RemoveInternal(Folder);
            else
                Client.Folders.RemoveInternal(Folder);
        }
    }
}
