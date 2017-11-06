using ImapX.Extensions;

namespace ImapX.Commands
{
    public class SelectCommand : FolderCommand
    {
        public SelectCommand(ImapClient client, long id, Folder folder)
            : base(client, id, folder, string.Format("SELECT {0}\r\n", folder.Path.Quote()))
        {
        }

        public override void OnCommandComplete()
        {
            Client.SelectedFolder = Folder;
        }
    }
}
