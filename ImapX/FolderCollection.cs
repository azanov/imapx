using System;
using System.Collections.Generic;

namespace ImapX
{
    public class FolderCollection : List<Folder>
    {
        public Folder this[string name]
        {
            get
            {
                var result = this.Find(_ => _.Name.Equals(name));
                if (result != null)
                    result.Examine();
                return result;
            }
        }
    }
}
