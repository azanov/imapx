using System;
using System.Collections.Generic;

namespace ImapX
{
    [Serializable]
    public class FolderCollection : List<Folder>
    {
        public Folder this[string name]
        {
            get
            {
                var result = Find(_ => _.Name.Equals(name) || _.ImapUtf7FolderName.Equals(name));
                if (result != null)
                    result.Examine();
                return result;
            }
        }
    }
}