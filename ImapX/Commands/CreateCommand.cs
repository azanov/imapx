using ImapX.EncodingHelpers;
using ImapX.Enums;
using ImapX.Extensions;

namespace ImapX.Commands
{
    public class CreateCommand : FolderCommand<Folder>
    {

        protected string _name;
        protected string _path;

        public CreateCommand(ImapClient client, long id, string folderName, Folder parent) 
            : base(client, id, parent)
        {
            _name = folderName;
            _path = (parent == null ? "" : parent.Path + client.Behavior.FolderDelimeter) +
                (client.EncodingMode == ImapEncodingMode.UTF8 ? folderName : ImapUTF7.Encode(folderName)) + client.Behavior.FolderDelimeter;

            Parts.Add(string.Format("CREATE {0}\r\n", _path.Quote()));
        }

        public override void OnCommandComplete()
        {
            Response = new Folder(Client, Folder);
            if (Folder != null)
                Folder.SubFolders.AddInternal(Response);
            else
                Client.Folders.AddInternal(Response);

            Client.ScheduleExamine(Response);
        }
    }
}
