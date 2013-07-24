using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImapX.Enums;

namespace ImapX
{
    public class ClientBehavior
    {

        public ClientBehavior()
        {
            FolderTreeBrowseMode = FolderTreeBrowseMode.Lazy;
            MessageFetchMode = MessageFetchMode.HeadersOnly;
            ExamineFolders = true;
            FolderDelimeter = '\0';
            SpecialUseMetadataPath = "/private/specialuse";
        }

        /// <summary>
        /// Get or set the folder tree loading mode
        /// </summary>
        public FolderTreeBrowseMode FolderTreeBrowseMode { get; set; }

        /// <summary>
        /// Gets or sets the mode how messages should be downloaded when fetched automatically
        /// </summary>
        public MessageFetchMode MessageFetchMode { get; set; }

        /// <summary>
        /// Gets or sets whether the folders should be examined when requested first
        /// </summary>
        public bool ExamineFolders { get; set; }

        /// <summary>
        /// A char used as delimeter for folders
        /// </summary>
        internal char FolderDelimeter { get; set; }

        /// <summary>
        /// The path where the special use metadata information for folders is stored
        /// </summary>
        public string SpecialUseMetadataPath { get; set; }
    }
}
