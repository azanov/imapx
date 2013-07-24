using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Authentication;
using ImapX.Authentication;

namespace ImapX
{
    public class ImapClient : ImapBase
    {
        private FolderCollection _folders;

        /// <summary>
        ///     Creates a new IMAP client
        /// </summary>
        public ImapClient()
        {
        }

        /// <summary>
        ///     Creates a new IMAP client, specifies the server to connect to. The default port is used (143; 993 if SSL is used)
        /// </summary>
        public ImapClient(string host, bool useSsl = false, bool validateServerCertificate = false)
            : this(
                host, useSsl ? DEFAULT_IMAP_SSL_PORT : DEFAULT_IMAP_PORT,
                useSsl ? SslProtocols.Default : SslProtocols.None, validateServerCertificate)
        {
        }

        /// <summary>
        ///     Creates a new IMAP client, specifies the server and the port to connect to.
        /// </summary>
        public ImapClient(string host, int port, bool useSsl = false, bool validateServerCertificate = false)
            : this(host, port, useSsl ? SslProtocols.Default : SslProtocols.None, validateServerCertificate)
        {
        }

        /// <summary>
        ///     Creates a new IMAP client, specifies the server and the port to connect to.
        /// </summary>
        public ImapClient(string host, int port, SslProtocols sslProtocol = SslProtocols.None,
            bool validateServerCertificate = false)
            : this()
        {
            Host = host;
            Port = port;
            SslProtocol = sslProtocol;
            ValidateServerCertificate = validateServerCertificate;
        }

        [Obsolete("Please use another constructor. This will be removed in future releases.", true)]
        public ImapClient(string host, int port, bool useSsl = false, SslProtocols sslProtocols = SslProtocols.Default,
            bool validateServerCertificate = true)
            : this(host, port, useSsl ? sslProtocols : SslProtocols.None, validateServerCertificate)
        {
        }

        /// <summary>
        ///     A char used as delimeter for folders
        /// </summary>
        internal char FolderDelimeter { get; set; }

        /// <summary>
        ///     Get or set the credentials used to authenticate
        /// </summary>
        public IImapCredentials Credentials { get; set; }

        [Obsolete("UserLogin is obsolete, please use Credentials together with Authentication.PlainCredentials instead",
            true)]
        public string UserLogin
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        [Obsolete(
            "UserPassword is obsolete, please use Credentials together with Authentication.PlainCredentials instead",
            true)]
        public string UserPassword
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        [Obsolete("IsLogined is obsolete, please use IsAuthenticated instead", true)]
        public bool IsLogined
        {
            get { return IsAuthenticated; }
        }

        public FolderCollection Folders
        {
            get { return _folders ?? (_folders = GetFolders()); }
            set { _folders = value; }
        }

        internal FolderCollection GetFolders()
        {
            FolderCollection folders = GetFolders("");
            foreach (Folder current in folders)
            {
                current.Client = this;
            }
            return folders;
        }

        internal FolderCollection GetFolders(string parent)
        {
            if (!IsConnected)
            {
                throw new ImapException("Not Connect");
            }
            var folderCollection = new FolderCollection();
            IList<string> data = new List<string>();
            string command = "LIST \"" + parent + "\" %\r\n";
            if (!SendAndReceive(command, ref data))
            {
                throw new ImapException("Bad or not correct Path");
            }
            if (data[0].StartsWith("* "))
            {
                FolderDelimeter = data[0][data[0].IndexOf("\"", StringComparison.Ordinal) + 1];
                if (data[0].Contains("NIL"))
                {
                    FolderDelimeter = '"';
                }
            }
            foreach (string text in data)
            {
                if (text.StartsWith("* "))
                {
                    string[] array = text.Split(new[]
                    {
                        FolderDelimeter
                    });
                    if (FolderDelimeter == '"')
                    {
                        array = new[]
                        {
                            array[0],
                            array[1]
                        };
                    }
                    if (array.Length == 2)
                    {
                        var folder = new Folder(array[array.Length - 1].Replace("\"", "").Trim())
                        {
                            FolderPath = FolderDelimeter != '"'
                                ? text.Substring(text.IndexOf("\"" + FolderDelimeter + "\"", StringComparison.Ordinal)).
                                    Replace("\""
                                            + FolderDelimeter + "\"", "").Replace("\"", "").Trim()
                                : text.Substring(text.IndexOf(FolderDelimeter)).Replace("\"", "")
                        };
                        if (text.Contains("\\HasChildren"))
                        {
                            folder.HasChildren = true;
                            folder.Client = this;
                            folder.GetSubFolders();
                        }
                        folderCollection.Add(folder);
                    }
                    else
                    {
                        if (array.Length == 3 & !parent.Equals("\"\""))
                        {
                            string folderName = array[array.Length - 1].Replace("\"", "").Trim();
                            var folder2 = new Folder(folderName)
                            {
                                FolderPath =
                                    text.Substring(text.IndexOf("\"" + FolderDelimeter + "\"", StringComparison.Ordinal))
                                        .Replace(
                                            "\"" + FolderDelimeter + "\"", "").Replace("\"", "").Trim()
                            };
                            if (text.Contains("\\HasChildren"))
                            {
                                folder2.HasChildren = true;
                                folder2.Client = this;
                                folder2.GetSubFolders();
                            }
                            folderCollection.Add(folder2);
                        }
                    }
                }
            }
            return folderCollection;
        }

        internal MessageCollection SearchMessage(string path)
        {
            if (!IsConnected)
            {
                throw new ImapException("Not Connect");
            }
            if (path == null)
            {
                throw new ImapException("Not Set Search Path");
            }
            IList<string> data = new List<string>();


            string command = "UID SEARCH " + path + "\r\n"; // [21.12.12] Fix by Yaroslav T, added UID command
            if (!SendAndReceive(command, ref data))
            {
                throw new ImapException("Bad or not correct search query");
            }
            string[] array = (data.FirstOrDefault(_=>_.StartsWith("* SEARCH", true, CultureInfo.InvariantCulture)) ?? "").Split(new[]
            {
                ' '
            });
            var messageCollection = new MessageCollection();
            string[] array2 = array;
            foreach (string s in array2)
            {
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

        [Obsolete("Connection is obsolete, please use Connect instead")]
        public bool Connection()
        {
            return Connect();
        }

        /// <summary>
        ///     Authenticate using a login and password
        /// </summary>
        /// <returns><code>true</code> if the authentication was successful</returns>
        public bool Login(string login, string password)
        {
            return Login(new PlainCredentials(login, password));
        }

        /// <summary>
        ///     Authenticate using given credentials
        /// </summary>
        /// <returns><code>true</code> if the authentication was successful</returns>
        public bool Login(IImapCredentials credentials)
        {
            Credentials = credentials;
            IList<string> data = new List<string>();
            IsAuthenticated = SendAndReceive(credentials.ToCommand(Capabilities), ref data);
            return IsAuthenticated;
        }

        public bool OAuth2LogIn(string login, string authToken)
        {
            return Login(new OAuth2Credentials(login, authToken));
        }

        /// <summary>
        ///     Logout from server
        /// </summary>
        /// <returns><code>true</code> if the logout was successful</returns>
        public bool Logout()
        {
            IList<string> data = new List<string>();
            if (SendAndReceive(ImapCommands.LOGOUT, ref data))
            {
                IsAuthenticated = false;
                FolderDelimeter = '\0';
                _folders = null;
            }
            return !IsAuthenticated;
        }

        //public void AppendMessageForYahoo(string Folder, Message msg, string flags)
        //{
        //}

        public MessageCollection EndUtf8Searsh(byte[] b)
        {
            MessageCollection messageCollection = SearchUtf8Data(b);
            if (messageCollection != null)
            {
                foreach (Message current in messageCollection)
                {
                    current.Client = this;
                }
            }
            return messageCollection;
        }

        public bool SelectFolder(string folderName)
        {
            if (!IsConnected)
            {
                throw new ImapException("Not Connect");
            }
            if (string.IsNullOrEmpty(folderName))
            {
                return false;
            }
            IList<string> arrayList = new List<string>();
            string command = "SELECT \"" + folderName + "\"\r\n";
            if (!SendAndReceive(command, ref arrayList))
            {
                return false;
            }
            SelectedFolder = folderName;
            return true;
        }

    }
}