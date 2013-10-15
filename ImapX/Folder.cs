using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ImapX.Collections;
using ImapX.EncodingHelpers;
using ImapX.Exceptions;
using ImapX.Flags;
using System.Collections;

namespace ImapX
{

    public class Folder
    {
        private readonly ImapClient _client;

        private readonly Folder _parent;
        private int _exists;
        private FolderFlagCollection _flags;
        private string _folderPath;
        private MessageCollection _messages;
        private string _name;
        private int _recents;
        private FolderCollection _subFolders;

        private int _uidNext;
        private string _uidValidity;
        private int _unseen;

        internal Folder(string path, IEnumerable<string> flags, ref Folder parent, ImapClient client)
        {
            _folderPath = path;
            _name = ImapUTF7.Decode(_folderPath.Split(client.Behavior.FolderDelimeter).Last());
            UpdateFlags(flags);
            _parent = parent;
            _client = client;
        }


        public FolderFlagCollection Flags
        {
            get { return _flags; }
        }

        public IEnumerable<string> AllowedPermanentFlags { get; set; }

        public bool Selectable { get; private set; }

        public int LastUpdateMessagesCount { get; private set; }

        public bool HasChildren { get; internal set; }

        public int Unseen
        {
            get { return _unseen; }
        }

        public int Recents
        {
            get { return _recents; }
            set { _recents = value; }
        }

        public int Exists
        {
            get { return _exists; }
        }

        public int UidNext
        {
            get { return _uidNext; }
        }

        public string UidValidity
        {
            get { return _uidValidity; }
        }

        public MessageCollection Messages
        {
            get { return _messages ?? (_messages = SetMessage()); }
            set
            {
                if (_messages == null)
                    _messages = SetMessage();
                _messages = value;
            }
        }

        public string FolderPath
        {
            get { return _folderPath; }
            internal set { _folderPath = value; }
        }

        [Obsolete("SubFolder is obsolete, please use SubFolders")]
        public FolderCollection SubFolder
        {
            get { return SubFolders; }
        }

        public FolderCollection SubFolders
        {
            get
            {
                return _subFolders ??
                       (_subFolders =
                           HasChildren
                               ? _client.GetFolders(_folderPath + _client.Behavior.FolderDelimeter, _client.Folders,
                                   this)
                               : new FolderCollection(_client, this));
            }
            internal set { _subFolders = value; }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (!Rename(value))
                    throw new OperationFailedException("Failed to rename folder");
            }
        }

        /// <summary>
        ///     Ranames the folder
        /// </summary>
        /// <param name="name">the name to set</param>
        /// <returns></returns>
        internal bool Rename(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Folder name cannot be empty");

            List<string> data = new List<string>();

            string encodedName = ImapUTF7.Encode(name);

            int i = _folderPath.LastIndexOf(_client.Behavior.FolderDelimeter);

            string newPath = i < 1 ? encodedName : _folderPath.Substring(0, i + 1) + encodedName;

            if (_client.SendAndReceive(string.Format(ImapCommands.RENAME, _folderPath, newPath), ref data))
            {
                _name = name;
                _folderPath = newPath;

                if (HasChildren && _subFolders != null)
                {
                    foreach (Folder folder in SubFolders)
                        folder.UpdatePath(_folderPath);
                }

                return true;
            }

            return false;
        }

        internal void UpdatePath(string parentPath)
        {
            int i = _folderPath.LastIndexOf(_client.Behavior.FolderDelimeter);
            _folderPath = parentPath + _folderPath.Substring(i, _folderPath.Length - i);
        }

        internal void UpdateFlags(string flags)
        {
            UpdateFlags(flags.Split(' '));
        }

        /// <summary>
        ///     Updates the private list of flags, sets properties like HasChildren and Selectable
        /// </summary>
        /// <param name="flags"></param>
        internal void UpdateFlags(IEnumerable<string> flags)
        {
            _flags = new FolderFlagCollection((flags ?? new string[0]).Where(_ => !string.IsNullOrEmpty(_)), _client,
                this);
            Selectable = !flags.Contains(FolderFlags.NoSelect);
            HasChildren = flags.Contains(FolderFlags.HasChildren);
        }

        public override string ToString()
        {
            return Name;
        }

        public MessageCollection CheckNewMessage(bool processMessages)
        {
            var messageCollection = new MessageCollection();
            MessageCollection messageCollection2 = _client.SearchMessage("all");
            int result = _messages.Select(current => current.MessageUid).Concat(new[] {-1}).Max();
            var list = messageCollection2.Where(m => m.MessageUid > result);
            if (list.Count() > 0)
            {
                LastUpdateMessagesCount = list.Count();
                foreach (Message current2 in list)
                {
                    current2.Client = _client;
                    if (processMessages)
                    {
                        current2.Process();
                    }
                }
                _messages.AddRange(list);
                messageCollection.AddRange(list);
            }
            else
            {
                LastUpdateMessagesCount = 0;
            }
            return messageCollection;
        }

        private MessageCollection SetMessage()
        {
            Select();
            MessageCollection messageCollection = _client.SearchMessage("all");
            foreach (Message current in messageCollection)
            {
                current.Client = _client;
                current.Folder = this;
            }
            return messageCollection;
        }

        internal static Folder Parse(string commandResult, ref Folder parent, ImapClient client)
        {
            var rex = new Regex(@".*\((\\.*)+\)\s[""]?(.|[NIL]{3})[""]?\s[""]?([^""]*)[""]?", RegexOptions.IgnoreCase);
            Match match = rex.Match(commandResult);

            if (match.Success && match.Groups.Count == 4)
            {
                string[] flags = match.Groups[1].Value.Split(' ');

                string path = match.Groups[3].Value;

                if (client.Behavior.FolderDelimeter == '\0' && !(string.IsNullOrEmpty(match.Groups[2].Value) || match.Groups[2].Value.ToUpper().Equals("NIL")))
                    client.Behavior.FolderDelimeter = match.Groups[2].Value.ToCharArray()[0];

                return new Folder(path, flags, ref parent, client);
            }

            return null;
        }

        public bool Examine()
        {
            if (_client == null || !_client.IsConnected)
            {
                throw new ImapException("Dont Connect");
            }
            List<string> arrayList = new List<string>();
            string command = "EXAMINE \"" + FolderPath + "\"\r\n";
            if (!_client.SendAndReceive(command, ref arrayList))
            {
                return false;
            }
            foreach (string line in arrayList)
            {
                if (!ParseHelper.Exists(line, ref _exists) && !ParseHelper.Recent(line, ref _recents) &&
                    !ParseHelper.UidNext(line, ref _uidNext) && !ParseHelper.Unseen(line, ref _unseen))
                {
                    ParseHelper.UidValidity(line, ref _uidValidity);
                }
            }
            return true;
        }

        public MessageCollection Search(string path, bool makeProcess)
        {
            if (_client == null || !_client.IsConnected)
            {
                throw new ImapException("Dont Connect");
            }
            string selectedFolder = _client.SelectedFolder;
            Select();
            MessageCollection messageCollection = _client.SearchMessage(path);
            foreach (Message current in messageCollection)
            {
                current.Client = _client;
                current.Folder = this; // [5/10/13] Fix by axlns
                if (makeProcess)
                {
                    current.Process();
                }
            }
            _client.SelectFolder(selectedFolder);
            return messageCollection;
        }

        public void Select()
        {
            _client.SelectFolder(FolderPath);
        }

        public bool EmptyFolder()
        {
            if (_client == null || !_client.IsConnected)
            {
                throw new ImapException("Dont Connect");
            }
            string text = "UID STORE {0}:{1} +FLAGS (\\Deleted)\r\n"; // [21.12.12] Fix by Yaroslav T, added UID command
            List<string> arrayList = new List<string>();
            if (Messages.Count == 0)
            {
                return true;
            }
            int messageUid = Messages[0].MessageUid;
            int messageUid2 = Messages[Messages.Count - 1].MessageUid;
            Select();
            if (_client.SendAndReceive(string.Format(text, messageUid, messageUid2), ref arrayList))
            {
                text = "EXPUNGE\r\n";
                if (_client.SendAndReceive(text, ref arrayList))
                {
                    _messages.Clear();
                    Examine();
                    return true;
                }
            }
            return false;
        }

        public bool CreateFolder(string name)
        {
            return SubFolders.Add(name);
        }

        [Obsolete("DeleteFolder is obsolete, please use Remove instead")]
        public bool DeleteFolder()
        {
            return Remove();
        }

        /// <summary>
        ///     Removes the folder
        /// </summary>
        /// <returns><code>true</code> if the folder could be removed</returns>
        public bool Remove()
        {
            if (!Selectable)
                throw new InvalidOperationException(
                    "A non-selectable folder cannot be deleted. This error may occur if the folder has subfolders.");

            List<string> data = new List<string>();
            if (!_client.SendAndReceive(string.Format(ImapCommands.DELETE, _folderPath), ref data))
                return false;

            if (_parent != null)
                _parent._subFolders.RemoveInternal(this);
            else
                _client.Folders.RemoveInternal(this);

            return true;
        }

        public bool CopyMessageToFolder(Message msg, Folder folder)
        {
            if (_client == null || !_client.IsConnected)
            {
                throw new ImapException("Dont Connect");
            }
            if (msg == null)
            {
                throw new ImapException("Message is null");
            }
            if (folder == null)
            {
                throw new ImapException("Folder is null");
            }
            string selectedFolder = _client.SelectedFolder;
            Select();
            string text = "UID COPY {0} \"{1}\"\r\n"; // [21.12.12] Fix by Yaroslav T, added UID command
            List<string> arrayList = new List<string>();
            if (!_client.SendAndReceive(string.Format(text, msg.MessageUid, folder.FolderPath), ref arrayList))
            {
                _client.SelectFolder(selectedFolder);
                return false;
            }
            text = "EXPUNGE\r\n";
            if (!_client.SendAndReceive(text, ref arrayList))
            {
                return false;
            }
            _client.SelectFolder(selectedFolder);
            return true;
        }

        public bool DeleteMessage(Message msg)
        {
            if (_client == null || !_client.IsConnected)
            {
                throw new ImapException("Dont Connect");
            }
            if (msg == null)
            {
                throw new ImapException("Message is null");
            }
            string selectedFolder = _client.SelectedFolder;
            _client.SelectFolder(FolderPath);
            string text = "UID STORE {0} +FLAGS (\\Deleted)\r\n"; // [21.12.12] Fix by Yaroslav T, added UID command
            List<string> arrayList = new List<string>();
            Select();
            if (_client.SendAndReceive(string.Format(text, msg.MessageUid), ref arrayList))
            {
                text = "EXPUNGE\r\n";
                if (_client.SendAndReceive(text, ref arrayList))
                {
                    Messages.Remove(msg);
                    Examine();
                }
                _client.SelectFolder(selectedFolder);
                return true;
            }
            _client.SelectFolder(selectedFolder);
            return false;
        }

        public bool MoveMessageToFolder(Message msg, Folder folder)
        {
            if (_client == null || !_client.IsConnected)
            {
                throw new ImapException("Dont Connect");
            }
            return CopyMessageToFolder(msg, folder) && DeleteMessage(msg);
        }

        public bool AppendMessage(Message msg, string flag)
        {
            if (_client == null || !_client.IsConnected)
            {
                throw new ImapException("Dont Connect");
            }
            if (msg == null)
            {
                throw new ImapException("Message is null");
            }
            string selectedFolder = _client.SelectedFolder;
            Select();
            var arrayList = new List<string>();
            string text = msg.MessageBuilder();
            int length = text.Length;
            if (string.IsNullOrEmpty(flag))
            {
                flag = "\\draft";
            }
            string command = string.Concat(new object[]
                                           {
                                               "APPEND \"",
                                               FolderPath,
                                               "\" (",
                                               flag,
                                               ") {",
                                               length - 2,
                                               "}\r\n"
                                           });
            if (_client.SendAndReceiveMessage(command, ref arrayList, msg))
            {
                _client.SelectFolder(selectedFolder);
                return true;
            }
            _client.SelectFolder(selectedFolder);
            return false;

        }
    }
}