using System;
using System.Collections;
using System.Linq;

namespace ImapX
{
    [Serializable]
    public class Folder
    {
        public Imap Client;
        private int _exists;
        private string _friendlyFolderName;
        private MessageCollection _messages;
        private int _recents;
        private FolderCollection _subFolders;
        private int _uidNext;
        private string _uidValidity;
        private int _unseen;

        public Folder(string folderName)
        {
            ImapUtf7FolderName = folderName;
            _subFolders = new FolderCollection();
        }

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

        public string FolderPath { get; set; }

        public FolderCollection SubFolder
        {
            get
            {
                SubfolderInit();
                return _subFolders;
            }
            set { _subFolders = value; }
        }

        public string Name
        {
            get
            {
                return string.IsNullOrEmpty(_friendlyFolderName)
                           ? (_friendlyFolderName = ImapUTF7.Decode(ImapUtf7FolderName))
                           : _friendlyFolderName;
            }
        }

        internal string ImapUtf7FolderName { get; set; }

        private void SubfolderInit()
        {
            foreach (var current in _subFolders)
                current.Client = Client;
        }

        public override string ToString()
        {
            return Name;
        }

        public MessageCollection CheckNewMessage(bool processMessages)
        {
            var messageCollection = new MessageCollection();
            var messageCollection2 = Client.SearchMessage("all");
            var result = _messages.Select(current => current.MessageUid).Concat(new[] {-1}).Max();
            var list = messageCollection2.FindAll(m => m.MessageUid > result);
            if (list.Count > 0)
            {
                LastUpdateMessagesCount = list.Count;
                foreach (var current2 in list)
                {
                    current2.Client = Client;
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
            var messageCollection = Client.SearchMessage("all");
            foreach (var current in messageCollection)
            {
                current.Client = Client;
                current.Folder = this;
            }
            return messageCollection;
        }

        public bool GetSubFolders()
        {
            bool result;
            try
            {
                _subFolders = Client.GetFolders(FolderPath + Client.Delimiter);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public bool Examine()
        {
            if (Client == null || !Client.IsConnected)
            {
                throw new ImapException("Dont Connect");
            }
            var arrayList = new ArrayList();
            string command = "EXAMINE \"" + FolderPath + "\"\r\n";
            if (!Client.SendAndReceive(command, ref arrayList))
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
            if (Client == null || !Client.IsConnected)
            {
                throw new ImapException("Dont Connect");
            }
            var selectedFolder = Client.SelectedFolder;
            Select();
            var messageCollection = Client.SearchMessage(path);
            foreach (var current in messageCollection)
            {
                current.Client = Client;
                current.Folder = this; // [5/10/13] Fix by axlns
                if (makeProcess)
                {
                    current.Process();
                }
            }
            Client.SelectFolder(selectedFolder);
            return messageCollection;
        }

        public void Select()
        {
            Client.SelectFolder(FolderPath);
        }

        public bool EmptyFolder()
        {
            if (Client == null || !Client.IsConnected)
            {
                throw new ImapException("Dont Connect");
            }
            string text = "UID STORE {0}:{1} +FLAGS (\\Deleted)\r\n"; // [21.12.12] Fix by Yaroslav T, added UID command
            var arrayList = new ArrayList();
            if (Messages.Count == 0)
            {
                return true;
            }
            int messageUid = Messages[0].MessageUid;
            int messageUid2 = Messages[Messages.Count - 1].MessageUid;
            Select();
            if (Client.SendAndReceive(string.Format(text, messageUid, messageUid2), ref arrayList))
            {
                text = "EXPUNGE\r\n";
                if (Client.SendAndReceive(text, ref arrayList))
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
            if (Client == null || !Client.IsConnected)
            {
                throw new ImapException("Dont Connect");
            }
            const string format = "CREATE \"{0}\"\r\n";
            var arrayList = new ArrayList();
            string text = string.Format("{0}{1}{2}", FolderPath, Client.Delimiter, name);
            if (Client.SendAndReceive(string.Format(format, text), ref arrayList))
            {
                var folder = new Folder(name) {FolderPath = text, Client = Client};
                _subFolders.Add(folder);
                _subFolders[name].Examine();
                return true;
            }
            return false;
        }

        public bool DeleteFolder()
        {
            if (Client == null || !Client.IsConnected)
            {
                throw new ImapException("Dont Connect");
            }
            var arrayList = new ArrayList();
            string text = "CLOSE \"" + FolderPath + "\"\r\n";
            Client.SendAndReceive(text, ref arrayList);
            text = "DELETE \"{0}\"\r\n";
            return Client.SendAndReceive(string.Format(text, FolderPath), ref arrayList);
        }

        public bool CopyMessageToFolder(Message msg, Folder folder)
        {
            if (Client == null || !Client.IsConnected)
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
            string selectedFolder = Client.SelectedFolder;
            Select();
            string text = "UID COPY {0} \"{1}\"\r\n";// [21.12.12] Fix by Yaroslav T, added UID command
            var arrayList = new ArrayList();
            if (!Client.SendAndReceive(string.Format(text, msg.MessageUid, folder.FolderPath), ref arrayList))
            {
                Client.SelectFolder(selectedFolder);
                return false;
            }
            text = "EXPUNGE\r\n";
            if (!Client.SendAndReceive(text, ref arrayList))
            {
                return false;
            }
            Client.SelectFolder(selectedFolder);
            return true;
        }

        public bool DeleteMessage(Message msg)
        {
            if (Client == null || !Client.IsConnected)
            {
                throw new ImapException("Dont Connect");
            }
            if (msg == null)
            {
                throw new ImapException("Message is null");
            }
            string selectedFolder = Client.SelectedFolder;
            Client.SelectFolder(FolderPath);
            string text = "UID STORE {0} +FLAGS (\\Deleted)\r\n";// [21.12.12] Fix by Yaroslav T, added UID command
            var arrayList = new ArrayList();
            Select();
            if (Client._imap.SendAndReceive(string.Format(text, msg.MessageUid), ref arrayList))
            {
                text = "EXPUNGE\r\n";
                if (Client.SendAndReceive(text, ref arrayList))
                {
                    Messages.Remove(msg);
                    Examine();
                }
                Client.SelectFolder(selectedFolder);
                return true;
            }
            Client.SelectFolder(selectedFolder);
            return false;
        }

        public bool MoveMessageToFolder(Message msg, Folder folder)
        {
            if (Client == null || !Client.IsConnected)
            {
                throw new ImapException("Dont Connect");
            }
            return CopyMessageToFolder(msg, folder) && DeleteMessage(msg);
        }

        public bool AppendMessage(Message msg, string flag)
        {
            if (Client == null || !Client.IsConnected)
            {
                throw new ImapException("Dont Connect");
            }
            if (msg == null)
            {
                throw new ImapException("Message is null");
            }
            string selectedFolder = Client.SelectedFolder;
            Select();
            var arrayList = new ArrayList();
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
            if (Client.SendAndReceive(command, ref arrayList) && Client.SendData(text))
            {
                Client.SelectFolder(selectedFolder);
                return true;
            }
            Client.SelectFolder(selectedFolder);
            return false;
        }
    }
}