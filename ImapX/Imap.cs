using System;
using System.Collections;
namespace ImapX
{
    public class Imap : ImapBase
    {
        private FolderCollection _folders;
        internal char _delimiter = '/';

        internal Imap _imap
        {
            get
            {
                return this;
            }
        }

        public Imap()
        {
        }

        public Imap(string host, int port, bool useSsl)
        {
            this._imapHost = host;
            this._imapPort = port;
            this._useSSL = useSsl;
            this._folders = new FolderCollection();
        }

        public Imap(string host, int port, bool useSsl, string userLogin, string userPassword)
        {
            this._imapHost = host;
            this._imapPort = port;
            this._useSSL = useSsl;
            this._userLogin = userLogin;
            this._userPassword = userPassword;
            this._folders = new FolderCollection();
        }

        public bool SelectFolder(string folderName)
        {
            if (this._imap == null && !this._imap._isConnected)
            {
                throw new ImapException("Not Connect");
            }
            if (string.IsNullOrEmpty(folderName))
            {
                return false;
            }
            ArrayList arrayList = new ArrayList();
            string command = "SELECT \"" + folderName + "\"\r\n";
            if (!base.SendAndReceive(command, ref arrayList))
            {
                return false;
            }
            this._selectedFolder = folderName;
            return true;
        }

        public MessageCollection SearchMessage(string path)
        {
            if (this._imap == null && !this._imap._isConnected)
            {
                throw new ImapException("Not Connect");
            }
            if (path == null)
            {
                throw new ImapException("Not Set Search Path");
            }
            ArrayList arrayList = new ArrayList();
            string command = "SEARCH " + path + "\r\n";
            if (!base.SendAndReceive(command, ref arrayList))
            {
                throw new ImapException("Bad or not correct search query");
            }
            string[] array = arrayList[0].ToString().Split(new char[]
			{
				' '
			});
            MessageCollection messageCollection = new MessageCollection();
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string s = array2[i];
                int messageUid;
                if (int.TryParse(s, out messageUid))
                {
                    messageCollection.Add(new Message
                    {
                        MessageUid = messageUid
                    });
                }
            }
            return messageCollection;
        }

        public FolderCollection GetFolders(string parent)
        {
            if (this._imap == null && !this._imap._isConnected)
            {
                throw new ImapException("Not Connect");
            }
            FolderCollection folderCollection = new FolderCollection();
            ArrayList arrayList = new ArrayList();
            string command = "LIST \"" + parent + "\" *\r\n";
            if (!base.SendAndReceive(command, ref arrayList))
            {
                throw new ImapException("Bad or not correct Path");
            }
            if (arrayList[0].ToString().StartsWith("* "))
            {
                this._delimiter = arrayList[0].ToString()[arrayList[0].ToString().IndexOf("\"") + 1];
                if (arrayList[0].ToString().Contains("NIL"))
                {
                    this._delimiter = '"';
                }
            }
            foreach (string text in arrayList)
            {
                if (text.StartsWith("* "))
                {
                    string[] array = text.Split(new char[]
					{
						this._delimiter
					});
                    if (this._delimiter == '"')
                    {
                        array = new string[]
						{
							array[0], 
							array[1]
						};
                    }
                    if (array.Length == 2)
                    {
                        Folder folder = new Folder(array[array.Length - 1].Replace("\"", "").Trim());
                        if (this._delimiter != '"')
                        {
                            folder.FolderPath = text.Substring(text.IndexOf("\"" + this._delimiter + "\"")).Replace("\"" + this._delimiter + "\"", "").Replace("\"", "").Trim();
                        }
                        else
                        {
                            folder.FolderPath = text.Substring(text.IndexOf(this._delimiter)).Replace("\"", "");
                        }
                        if (text.Contains("\\HasChildren"))
                        {
                            folder._hasChildren = true;
                            folder._client = this._imap;
                            folder.GetSubFolders();
                        }
                        folderCollection.Add(folder);
                    }
                    else
                    {
                        if (array.Length == 3 & !parent.Equals("\"\""))
                        {
                            string folderName = array[array.Length - 1].Replace("\"", "").Trim();
                            Folder folder2 = new Folder(folderName);
                            folder2.FolderPath = text.Substring(text.IndexOf("\"" + this._delimiter + "\"")).Replace("\"" + this._delimiter + "\"", "").Replace("\"", "").Trim();
                            if (text.Contains("\\HasChildren"))
                            {
                                folder2._hasChildren = true;
                                folder2._client = this._imap;
                                folder2.GetSubFolders();
                            }
                            folderCollection.Add(folder2);
                        }
                    }
                }
            }
            return folderCollection;
        }

        public bool CreateFolder(string name)
        {
            if (this._imap == null && !this._imap._isConnected)
            {
                throw new ImapException("Not Connect");
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new ImapException("Not Set Folder Name");
            }
            string format = "CREATE \"{0}\"\r\n";
            ArrayList arrayList = new ArrayList();
            if (base.SendAndReceive(string.Format(format, name), ref arrayList))
            {
                Folder folder = new Folder(name);
                folder.FolderPath = name;
                return true;
            }
            return false;
        }
    }
}
