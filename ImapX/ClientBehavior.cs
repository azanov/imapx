using System;
using ImapX.Constants;
using ImapX.Enums;

namespace ImapX
{
    /// <summary>
    /// Defines basic client behavior for browsing folders, downloading messages and other
    /// </summary>
    public class ClientBehavior
    {
        private MessageFetchMode _messageFetchMode;

        public ClientBehavior()
        {
            FolderTreeBrowseMode = FolderTreeBrowseMode.Lazy;
            MessageFetchMode = MessageFetchMode.Basic;
            ExamineFolders = true;
            AutoPopulateFolderMessages = false;
            FolderDelimeter = '\0';
            SpecialUseMetadataPath = "/private/specialuse";
            AutoDownloadBodyOnAccess = true;
            RequestedHeaders = MessageHeaderSets.Minimal;
            AutoGenerateMissingBody = false;
            SearchAllNotSupported = false;
            NoopIssueTimeout = 840;
        }

        /// <summary>
        ///     Get or set the folder tree loading mode
        /// </summary>
        public FolderTreeBrowseMode FolderTreeBrowseMode { get; set; }

        /// <summary>
        ///     Gets or sets the mode how messages should be downloaded when fetched automatically
        /// </summary>
        public MessageFetchMode MessageFetchMode
        {
            get { return _messageFetchMode; }
            set
            {
                if (value == MessageFetchMode.ClientDefault)
                    throw new ArgumentException("The default fetch mode cannot be set to ClientDefault!");
                _messageFetchMode = value;
            }
        }

        /// <summary>
        ///     A list of message headers that will be requested. Set it to <code>null</code> to request all headers
        /// </summary>
        public string[] RequestedHeaders { get; set; }

        /// <summary>
        ///     Gets or sets whether the client should automatically populate Folder.Messages or it is done manually by calling
        ///     Folder.Messages.Download
        /// </summary>
        public bool AutoPopulateFolderMessages { get; set; }

        /// <summary>
        ///     Gets or sets whether the client should automatically download the message body (text & html) when the
        ///     Message.Body.Html/Text properties are accessed or it is done manually by calling Folder.Messages.Download
        /// </summary>
        public bool AutoDownloadBodyOnAccess { get; set; }

        /// <summary>
        ///     Gets or sets whether the folders should be examined when requested first
        /// </summary>
        public bool ExamineFolders { get; set; }

        /// <summary>
        ///     A char used as delimeter for folders
        /// </summary>
        internal char FolderDelimeter { get; set; }

        /// <summary>
        ///     The path where the special use metadata information for folders is stored
        /// </summary>
        public string SpecialUseMetadataPath { get; set; }

        /// <summary>
        ///     In case a message has only plain text or only html body available, the other view will be generated automatically if this property is set to <code>true</code>, 
        /// </summary>
        public bool AutoGenerateMissingBody { get; set; }

        /// <summary>
        ///     Some servers (e.g) imap.qq.com seem not give any response on SEARCH ALL, set this property to true to automatically replace this call by SEARCH SINCE 0000-00-00 in order to fix the issue
        /// </summary>
        public bool SearchAllNotSupported { get; set; }

        /// <summary>
        ///     Some servers (e.g) imap.qq.com seem not give any response on LIST "<folder-name>" %
        /// </summary>
        public bool LazyFolderBrowsingNotSupported { get; set; }

        /// <summary>
        /// Number of seconds after which a NOOP command is sent to the server if there is no activity
        /// </summary>
        public int NoopIssueTimeout { get; set; }
    }
}