using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace ImapX
{
    public class Folder
    {
        public Imap _client;
        private string _folderName;
        private string _folderPath;
        private FolderCollection _subFolders;
        private MessageCollection _messages;
        private int _unseen;
        private int _recents;
        private int _exists;
        private string _uidValidity;
        private int _uidNext;
        private int _lastMessageUpdateCount;
        internal bool _hasChildren;

        public int LastUpdateMessagesCount
        {
            get
            {
                return this._lastMessageUpdateCount;
            }
        }

        public bool HasChildren
        {
            get
            {
                return this._hasChildren;
            }
        }

        public int Unseen
        {
            get
            {
                return this._unseen;
            }
        }

        public int Recents
        {
            get
            {
                return this._recents;
            }
            set
            {
                this._recents = value;
            }
        }

        public int Exists
        {
            get
            {
                return this._exists;
            }
        }

        public int UidNext
        {
            get
            {
                return this._uidNext;
            }
        }

        public string UidValidity
        {
            get
            {
                return this._uidValidity;
            }
        }

        public MessageCollection Messages
        {
            get
            {
                if (this._messages == null)
                {
                    this._messages = this.setMessage();
                }
                return this._messages;
            }
            set
            {
                if (this._messages == null)
                {
                    this._messages = this.setMessage();
                }
                this._messages = value;
            }
        }

        public string FolderPath
        {
            get
            {
                return this._folderPath;
            }
            set
            {
                this._folderPath = value;
            }
        }

        public FolderCollection SubFolder
        {
            get
            {
                this.SubfolderInit();
                return this._subFolders;
            }
            set
            {
                this._subFolders = value;
            }
        }

        public string Name
        {
            get
            {
                return this._folderName;
            }
            set
            {
                this._folderName = value;
            }
        }

        /// <summary>
        /// Decodes the folder name using the specified encoding and 
        /// returns a friendly string
        /// </summary>
        /// <param name="encoding">The encoding to use. If null, 1201 (russian) is used by default</param>
        /// <remarks>
        /// [27.07.2012] Fix by Pavel Azanov (coder13)
        /// </remarks>
        public string GetDecodedName(Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(this._folderName))
                return string.Empty;

            try
            {

                if (!this._folderName.StartsWith("&") && !this._folderName.EndsWith("-"))
                    return this._folderName;

                var sb = new StringBuilder();

                this._folderName.Split(' ').All(delegate(string part)
                {

                    part = part.Replace(",", "/").TrimStart('&').TrimEnd('-');

                    while (part.Length % 4 != 0)
                        part += "=";


                    sb.Append(ParseHelper.DecodeBase64(part, encoding));
                    sb.Append(" ");

                    return true;
                });

                return sb.ToString().Trim();
            }
            catch
            {
                return _folderName ?? string.Empty;
            }
        }

        private void SubfolderInit()
        {
            foreach (Folder current in this._subFolders)
            {
                current._client = this._client;
            }
        }

        public Folder(string folderName)
        {
            this._folderName = folderName;
            this._subFolders = new FolderCollection();
        }

        public override string ToString()
        {
            return this.Name;
        }

        public MessageCollection CheckNewMessage(bool processMessages)
        {
            MessageCollection messageCollection = new MessageCollection();
            MessageCollection messageCollection2 = new MessageCollection();
            messageCollection2 = this._client.SearchMessage("all");
            int result = -1;
            foreach (Message current in this._messages)
            {
                result = Math.Max(result, current.MessageUid);
            }
            List<Message> list = messageCollection2.FindAll((Message m) => m.MessageUid > result);
            if (list != null && list.Count > 0)
            {
                this._lastMessageUpdateCount = list.Count;
                foreach (Message current2 in list)
                {
                    current2._client = this._client;
                    if (processMessages)
                    {
                        current2.Process();
                    }
                }
                this._messages.AddRange(list);
                messageCollection.AddRange(list);
            }
            else
            {
                this._lastMessageUpdateCount = 0;
            }
            return messageCollection;
        }

        private MessageCollection setMessage()
        {
            this.Select();
            MessageCollection messageCollection = new MessageCollection();
            messageCollection = this._client.SearchMessage("all");
            foreach (Message current in messageCollection)
            {
                current._client = this._client;
            }
            return messageCollection;
        }

        public bool GetSubFolders()
        {
            bool result;
            try
            {
                this._subFolders = this._client.GetFolders(this._folderPath + this._client._delimiter);
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
            if (this._client == null && !this._client._isConnected)
            {
                throw new ImapException("Dont Connect");
            }
            ArrayList arrayList = new ArrayList();
            string command = "EXAMINE \"" + this.FolderPath + "\"\r\n";
            if (!this._client.SendAndReceive(command, ref arrayList))
            {
                return false;
            }
            foreach (string line in arrayList)
            {
                if (!ParseHelper.Exists(line, ref this._exists) && !ParseHelper.Recent(line, ref this._recents) && !ParseHelper.UidNext(line, ref this._uidNext) && !ParseHelper.Unseen(line, ref this._unseen))
                {
                    ParseHelper.UidValidity(line, ref this._uidValidity);
                }
            }
            return true;
        }

        public MessageCollection Search(string path, bool makeProcess)
        {
            if (this._client == null && !this._client._isConnected)
            {
                throw new ImapException("Dont Connect");
            }
            string selectedFolder = this._client._selectedFolder;
            this.Select();
            MessageCollection messageCollection = this._client.SearchMessage(path);
            foreach (Message current in messageCollection)
            {
                current._client = this._client;
                if (makeProcess)
                {
                    current.Process();
                }
            }
            this._client.SelectFolder(selectedFolder);
            return messageCollection;
        }

        public void Select()
        {
            this._client.SelectFolder(this._folderPath);
        }

        public bool EmptyFolder()
        {
            if (this._client == null && !this._client._isConnected)
            {
                throw new ImapException("Dont Connect");
            }
            string text = "STORE {0}:{1} +FLAGS (\\Deleted)\r\n";
            ArrayList arrayList = new ArrayList();
            if (this.Messages.Count == 0)
            {
                return true;
            }
            int messageUid = this.Messages[0].MessageUid;
            int messageUid2 = this.Messages[this.Messages.Count - 1].MessageUid;
            this.Select();
            if (this._client.SendAndReceive(string.Format(text, messageUid, messageUid2), ref arrayList))
            {
                text = "EXPUNGE\r\n";
                if (this._client.SendAndReceive(text, ref arrayList))
                {
                    this._messages.Clear();
                    this.Examine();
                    return true;
                }
            }
            return false;
        }

        public bool CreateFolder(string name)
        {
            if (this._client == null && !this._client._isConnected)
            {
                throw new ImapException("Dont Connect");
            }
            string format = "CREATE \"{0}\"\r\n";
            ArrayList arrayList = new ArrayList();
            string text = string.Format("{0}{1}{2}", this._folderPath, this._client._delimiter, name);
            if (this._client.SendAndReceive(string.Format(format, text), ref arrayList))
            {
                Folder folder = new Folder(name);
                folder.FolderPath = text;
                folder._client = this._client;
                this._subFolders.Add(folder);
                this._subFolders[name].Examine();
                return true;
            }
            return false;
        }

        public bool DeleteFolder()
        {
            if (this._client == null && !this._client._isConnected)
            {
                throw new ImapException("Dont Connect");
            }
            ArrayList arrayList = new ArrayList();
            string text = "CLOSE \"" + this._folderPath + "\"\r\n";
            this._client.SendAndReceive(text, ref arrayList);
            text = "DELETE \"{0}\"\r\n";
            return this._client.SendAndReceive(string.Format(text, this.FolderPath), ref arrayList);
        }

        public bool CopyMessageToFolder(Message msg, Folder folder)
        {
            if (this._client == null && !this._client._isConnected)
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
            string selectedFolder = this._client._selectedFolder;
            this.Select();
            string text = "COPY {0} \"{1}\"\r\n";
            ArrayList arrayList = new ArrayList();
            if (!this._client.SendAndReceive(string.Format(text, msg.MessageUid, folder.FolderPath), ref arrayList))
            {
                this._client.SelectFolder(selectedFolder);
                return false;
            }
            text = "EXPUNGE\r\n";
            if (!this._client.SendAndReceive(text, ref arrayList))
            {
                return false;
            }
            this._client.SelectFolder(selectedFolder);
            return true;
        }

        public bool DeleteMessage(Message msg)
        {
            if (this._client == null && !this._client._isConnected)
            {
                throw new ImapException("Dont Connect");
            }
            if (msg == null)
            {
                throw new ImapException("Message is null");
            }
            string selectedFolder = this._client._selectedFolder;
            this._client.SelectFolder(this._folderPath);
            string text = "STORE {0} +FLAGS (\\Deleted)\r\n";
            ArrayList arrayList = new ArrayList();
            this.Select();
            if (this._client._imap.SendAndReceive(string.Format(text, msg.MessageUid), ref arrayList))
            {
                text = "EXPUNGE\r\n";
                if (this._client.SendAndReceive(text, ref arrayList))
                {
                    this.Examine();
                }
                this._client.SelectFolder(selectedFolder);
                return true;
            }
            this._client.SelectFolder(selectedFolder);
            return false;
        }

        public bool MoveMessageToFolder(Message msg, Folder folder)
        {
            if (this._client == null && !this._client._isConnected)
            {
                throw new ImapException("Dont Connect");
            }
            return this.CopyMessageToFolder(msg, folder) && this.DeleteMessage(msg);
        }

        public bool AppendMessage(Message msg, string flag)
        {
            if (this._client == null && !this._client._isConnected)
            {
                throw new ImapException("Dont Connect");
            }
            if (msg == null)
            {
                throw new ImapException("Message is null");
            }
            string selectedFolder = this._client._selectedFolder;
            this.Select();
            ArrayList arrayList = new ArrayList();
            string text = msg.messageBuilder();
            int length = text.Length;
            if (string.IsNullOrEmpty(flag))
            {
                flag = "\\draft";
            }
            string command = string.Concat(new object[]
			{
				"APPEND \"", 
				this.FolderPath, 
				"\" (", 
				flag, 
				") {", 
				length - 2, 
				"}\r\n"
			});
            if (this._client.SendAndReceive(command, ref arrayList) && this._client.SendData(text))
            {
                this._client.SelectFolder(selectedFolder);
                return true;
            }
            this._client.SelectFolder(selectedFolder);
            return false;
        }
    }
}
