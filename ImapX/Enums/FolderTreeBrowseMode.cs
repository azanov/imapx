using System.ComponentModel;

namespace ImapX.Enums
{
    [DefaultValue(FolderTreeBrowseMode.Lazy)]
    public enum FolderTreeBrowseMode
    {
        /// <summary>
        /// The subfolder list is only loaded when it is being needed
        /// </summary>
        Lazy,

        /// <summary>
        /// Full folder structure is loaded.
        /// WARNING: Will lead to infinite loops if the folder structure is circular!
        /// </summary>
        Full
    }
}