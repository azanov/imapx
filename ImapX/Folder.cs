using ImapX.Collections;
using ImapX.EncodingHelpers;
using ImapX.Enums;
using ImapX.Exceptions;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ImapX
{
    public class Folder
    {
        protected string _name;
        protected FolderCollection _subFolders;
        protected MessageCollection _messages;


        /// <summary>
        /// The ImapClient used to communicate with the server to perform folder specific actions
        /// </summary>
        internal ImapClient Client { get; set; }

        /// <summary>
        ///     The number of messages in the mailbox.
        /// </summary>
        public long Exists { get; internal set; }

        /// <summary>
        ///     The number of messages with the \Recent flag set.
        /// </summary>
        public long Recent { get; internal set; }

        /// <summary>
        ///     The message sequence number of the first unseen message in the mailbox.
        /// </summary>
        public long FirstUnseen { get; internal set; }

        /// <summary>
        ///     The number of messages which do not have the \Seen flag set.
        /// </summary>
        public long Unseen { get; internal set; }

        /// <summary>
        ///     The next unique identifier value.
        /// </summary>
        public long UidNext { get; internal set; }

        /// <summary>
        ///     The unique identifier validity value.
        /// </summary>
        public long UidValidity { get; internal set; }

        /// <summary>
        /// The path of the folder on the server
        /// </summary>
        public string Path { get; internal set; }

        /// <summary>
        ///     Gets or sets the name of the folder. Setting this property will rename the folder.
        /// </summary>
        public string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_name) && !string.IsNullOrWhiteSpace(Path))
                {
                    if (Client.EncodingMode == ImapEncodingMode.UTF8)
                        _name = Path;
                    else
                        _name = ImapUTF7.Decode(Path);
                }

                return _name;
            }
            set
            {
                throw new TodoException();
            }
        }

        /// <summary>
        /// Folder flags
        /// </summary>
        public FolderFlagCollection Flags { get; internal set; }

        /// <summary>
        ///     A list of message flags that the client can change permanently.  If this is missing, the client should assume that
        ///     all flags can be changed permanently.
        /// </summary>
        public IEnumerable<string> AllowedPermanentFlags { get; internal set; }

        /// <summary>
        /// The special folder type (e.g. Inbox, Junk, Archive etc.)
        /// </summary>
        public SpecialFolderType Type { get; internal set; }

        /// <summary>
        /// Indicates whether the folder accepts modifications. Is true for all folders except the selected one
        /// </summary>
        public bool ReadOnly { get; internal set; }

        /// <summary>
        /// Indicates whether the folder can be selected
        /// </summary>
        public bool Selectable { get; internal set; }

        /// <summary>
        /// Indicates whether the folder has subfolders
        /// </summary>
        public bool HasChildren { get; internal set; }

        /// <summary>
        /// Indicates whether the folder can have subfolders
        /// </summary>
        public bool CanHaveChildren { get; internal set; }

        /// <summary>
        /// Indicates whether this folder can be removed
        /// </summary>
        public bool CanBeDeleted
        {
            get
            {
                return (Selectable || !HasChildren) && Type != SpecialFolderType.Inbox;
            }
        }

        /// <summary>
        /// Indicates whether the server has marked the folder as "interesting"
        /// </summary>
        public bool Marked { get; internal set; }

        /// <summary>
        /// Highest mod-sequence value of all messages in the folder
        /// </summary>
        /// <see cref="https://tools.ietf.org/html/rfc4551"/>
        public long HighestModSeq { get; internal set; }

        public Folder ParentFolder { get; internal set; }

        /// <summary>
        ///     Subfolders of the current folder
        /// </summary>
        public FolderCollection SubFolders
        {
            get
            {
                return _subFolders ??
                       (_subFolders = new FolderCollection(Client, this,
                           HasChildren
                               ? Client.GetFolders(this)
                               : new Folder[0]));
            }
            internal set { _subFolders = value; }
        }

        /// <summary>
        ///     Messages stored in this folder
        /// </summary>
        public MessageCollection Messages
        {
            get
            {
                return _messages;
            }
            internal set { _messages = value; }
        }

        public long AppendLimit { get; internal set; }

        internal Folder(ImapClient client, Folder parent = null)
        {
            Client = client;
            Flags = new FolderFlagCollection(client, this);
            Selectable = true;
            CanHaveChildren = true;
            AllowedPermanentFlags = new string[0];
            ReadOnly = true;
            ParentFolder = parent;
            _subFolders = new FolderCollection(Client, parent, new Folder[0]);
            _messages = new MessageCollection(Client, this);
            AppendLimit = -1;
        }

        internal Folder(ImapClient client, string path, IEnumerable<string> flags = null) : this(client)
        {
            Initialize(path, flags);
        }

        internal void Initialize(string path, IEnumerable<string> flags = null)
        {
            Path = path;
            if (flags != null)
                Flags = new FolderFlagCollection(flags, Client, this);
        }

        /// <summary>
        ///     Selects the folder, making it available for modifications and messages available for queries
        /// </summary>
        public SafeResult Select()
        {
            return Client.SelectFolder(this);
        }

        /// <summary>
        ///     Unselects the folder
        /// </summary>
        public SafeResult Unselect()
        {
            return Client.UnselectCurrentFolder();
        }

        /// <summary>
        ///     Selects the folder, making messages available for queries, no modifications allowed
        /// </summary>
        public SafeResult Examine()
        {
            return Client.ExamineFolder(this);
        }

        /// <summary>
        ///     Deselects the folder and removes all messages marked with the \Deleted flag
        /// </summary>
        /// <returns></returns>
        public SafeResult Close()
        {
            return Client.CloseFolder(this);
        }

        /// <summary>
        /// Removes all messages marked with the \Deleted flag from the folder.
        /// </summary>
        /// <returns></returns>
        public SafeResult Expunge()
        {
            return Client.ExpungeFolder(this);
        }

        /// <summary>
        ///     Removes the folder
        /// </summary>
        [Obsolete("Folder.Remove is obsolete. Use Folder.Delete instead")]
        public SafeResult Remove()
        {
            return Delete();
        }

        /// <summary>
        ///     Deletes the folder
        /// </summary>
        public SafeResult Delete()
        {
            return Client.DeleteFolder(this);
        }

        public FolderStatus Status(FolderStatusType type = FolderStatusType.All)
        {
            return Client.GetFolderStatus(this, type);
        }
        
        public IEnumerable<Message> Search(string query = "ALL", MessageFetchMode mode = MessageFetchMode.ClientDefault,
            int count = -1)
        {
            var messages = Client.SearchFolder(this, query).ToArray();
            
            foreach(var msg in messages)
                Client.FetchMessage(msg, mode, false);

            return messages;
        }


        /// <summary>
        /// Schedule the examine command. Used to automatically examine folders after list/xlist if <code>ImapClient.Behavior.ExamineFolders = true</code>
        /// </summary>
        internal void ScheduleExamine()
        {
            Client.ScheduleExamine(this);
        }

        internal void RemoveExpungedMessages()
        {
            // TODO: remove messages with the \Deleted flag
        }

        internal void RemoveExpungedMessage(int id)
        {
            // TODO: remove the given message
        }
        
    }
}
