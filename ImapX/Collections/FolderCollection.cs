using System;
using System.Linq;
using System.Collections.Generic;
using ImapX.Enums;
using ImapX.EncodingHelpers;

namespace ImapX.Collections
{
    public class FolderCollection : ImapObjectCollection<Folder>
    {
        internal Folder Parent { get; set; }

        internal FolderCollection(ImapClient client, Folder parent, IEnumerable<Folder> items) : base(client, items)
        {
            Parent = parent;
        }

        public Folder this[string path]
        {
            get
            {
                var buffer = path;
                Folder folder = null;

                while(buffer.Length > 0)
                {
                    folder = this.FirstOrDefault(_ => _.Path == buffer);
                    if (folder == null)
                        buffer = path.Substring(0, Math.Max(0, path.LastIndexOf(Client.Behavior.FolderDelimeter)));
                    else
                        break;
                }

                return folder != null && buffer.Length < path.Length ? folder.SubFolders[path] : folder;
            }
        }

        /// <summary>
        /// Creates a new folder with the given name
        /// </summary>
        /// <param name="folderName">The folder name</param>
        /// <returns>The new folder if it could be created, otherwise null</returns>
        /// <exception cref="System.ArgumentException">If the folder name is empty</exception>
        public Folder Add(string folderName)
        {
            return Client.CreateFolder(folderName, Parent);
        }
    }
}
