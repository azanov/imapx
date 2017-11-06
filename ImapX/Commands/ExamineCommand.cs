using ImapX.Extensions;

namespace ImapX.Commands
{
    public class ExamineCommand : FolderCommand
    {
        public ExamineCommand(ImapClient client, long id, Folder folder) 
            : base(client, id, folder, string.Format("EXAMINE {0}\r\n", folder.Path.Quote()))
        {
        }
    }
}
