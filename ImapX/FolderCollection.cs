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
                foreach (Folder current in this)
                {
                    if (current.Name.Equals(name))
                    {
                        current.Examine();
                        return current;
                    }
                }
                return null;
            }
        }
    }
}
