using System;
using System.Collections.Generic;
using System.Linq;
using ImapX.Constants;
using ImapX.EncodingHelpers;

namespace ImapX.Collections
{
    public class FolderCollection : ImapObjectCollection<Folder>
    {

        Folder _parentFolder;

        public FolderCollection(ImapClient client, Folder parentFolder = null)
            : base(client)
        {
            _parentFolder = parentFolder;
        }

        public FolderCollection(IEnumerable<Folder> items, ImapClient client, Folder parentFolder = null)
            : base(client, items)
        {
            _parentFolder = parentFolder;
        }

        public Folder this[string name]
        {
            get
            {
                var result = List.FirstOrDefault(_ => _.Name.Equals(name));
                return result;
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
            if (string.IsNullOrEmpty(folderName))
                throw new ArgumentException("The folder name cannot be empty");

            folderName = ImapUTF7.Encode(folderName);

            var path = _parentFolder == null ? folderName : _parentFolder.Path + Client.Behavior.FolderDelimeter + folderName;

            IList<string> data = new List<string>();

            if (!Client.SendAndReceive(string.Format(ImapCommands.Create, path), ref data)) return null;
            
            var folder = new Folder(path, new string[0], ref _parentFolder, Client);

            if (Client.Behavior.ExamineFolders)
                folder.Examine();

            AddInternal(folder);

            return folder;
        }

        /// <summary>
        /// Removes a folder
        /// </summary>
        /// <param name="item">The folder to remove</param>
        /// <returns><code>true</code> if the folder could be removed</returns>
        public bool Remove(Folder item)
        {
            return item.Remove();
        }

        /// <summary>
        /// Removes a folder at the specified index
        /// </summary>
        /// <returns><code>true</code> if the folder could be removed</returns>
        public bool RemoveAt(int index)
        {
            return Remove(List[index]);
        }

        public Folder Find(string path)
        {
            foreach (var folder in this)
            {
                if (folder.Path == path)
                    return folder;

                if (!folder.HasChildren) continue;

                var result = folder.SubFolders.Find(path);
                if (result != null)
                    return result;
            }
            return null;
        }

    }
}