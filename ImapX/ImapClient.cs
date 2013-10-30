﻿using System;
using System.Collections.Generic;

using System.Globalization;
using System.Linq;
using System.Security.Authentication;
using ImapX.Authentication;
using ImapX.Collections;
using ImapX.Constants;
using ImapX.Enums;

namespace ImapX
{
    public class ImapClient : ImapBase
    {

        private CommonFolderCollection _folders;

        /// <summary>
        /// Basic client behavior settings like folder browse mode and message download mode
        /// </summary>
        public ClientBehavior Behavior { get; private set; }

        /// <summary>
        /// The folder structure
        /// </summary>
        public CommonFolderCollection Folders
        {
            get
            {
                return _folders ?? (_folders = GetFolders());
            }
        }

        /// <summary>
        /// Get or set the credentials used to authenticate
        /// </summary>
        public ImapCredentials Credentials { get; set; }

        internal Folder SelectedFolder { get; set; }

        /// <summary>
        /// Creates a new IMAP client
        /// </summary>
        public ImapClient()
        {
            Behavior = new ClientBehavior();
        }

        /// <summary>
        /// Creates a new IMAP client, specifies the server to connect to. The default port is used (143; 993 if SSL is used)
        /// </summary>
        public ImapClient(string host, bool useSsl = false, bool validateServerCertificate = true)
            : this(host, useSsl ? DefaultImapSslPort : DefaultImapPort, useSsl ? SslProtocols.Default : SslProtocols.None, validateServerCertificate)
        {

        }

        /// <summary>
        /// Creates a new IMAP client, specifies the server and the port to connect to. 
        /// </summary>
        public ImapClient(string host, int port, bool useSsl = false, bool validateServerCertificate = true)
            : this(host, port, useSsl ? SslProtocols.Default : SslProtocols.None, validateServerCertificate)
        {

        }

        /// <summary>
        /// Creates a new IMAP client, specifies the server and the port to connect to. 
        /// </summary>
        public ImapClient(string host, int port, SslProtocols sslProtocol = SslProtocols.None, bool validateServerCertificate = true)
            : this()
        {
            Host = host;
            Port = port;
            SslProtocol = sslProtocol;
            ValidateServerCertificate = validateServerCertificate;
        }

        [Obsolete("Please use another constructor. This will be removed in future releases.", true)]
        public ImapClient(string host, int port, bool useSsl = false, SslProtocols sslProtocols = SslProtocols.Default, bool validateServerCertificate = true)
            : this(host, port, useSsl ? sslProtocols : SslProtocols.None, validateServerCertificate)
        {

        }

        /// <summary>
        /// Authenticate using credentials set through the <code>Credentials</code> property
        /// </summary>
        /// <returns><code>true</code> if the authentication was successful</returns>
        public bool Login()
        {
            if(Credentials == null)
                throw new ArgumentNullException("The credentials cannot be null");

            return Login(Credentials);
        }

        /// <summary>
        /// Authenticate using a login and password
        /// </summary>
        /// <returns><code>true</code> if the authentication was successful</returns>
        public bool Login(string login, string password)
        {
            return Login(new PlainCredentials(login, password));
        }

        /// <summary>
        /// Authenticate using given credentials
        /// </summary>
        /// <returns><code>true</code> if the authentication was successful</returns>
        public bool Login(ImapCredentials credentials)
        {
            Credentials = credentials;
            IList<string> data = new List<string>();
            IsAuthenticated = SendAndReceive(credentials.ToCommand(Capabilities), ref data, credentials);

            var capabilities = data.FirstOrDefault(_ => _.StartsWith("* CAPABILITY"));

            if (Capabilities == null)
                Capabilities = new Capability(capabilities);
            else
                Capabilities.Update(capabilities);

            if (IsAuthenticated && Host.ToLower() == "imap.qq.com")
                Behavior.SearchAllNotSupported = true;

            return IsAuthenticated;
        }

        /// <summary>
        /// Logout from server
        /// </summary>
        /// <returns><code>true</code> if the logout was successful</returns>
        public bool Logout()
        {
            IList<string> data = new List<string>();
            if (SendAndReceive(ImapCommands.Logout, ref data))
            {
                IsAuthenticated = false;
                Behavior.FolderDelimeter = '\0';
                _folders = null;
            }
            return !IsAuthenticated;
        }

        /// <summary>
        /// Requests the top-level folder structure
        /// </summary>
        /// <returns></returns>
        internal CommonFolderCollection GetFolders()
        {
            var folders = new CommonFolderCollection(this);
            folders.AddRangeInternal(GetFolders("", folders, null, true));
            return folders;
        }

        /// <summary>
        /// Request the folder structure for a specific path
        /// </summary>
        /// <param name="path">The path to search</param>
        /// <param name="commonFolders">The list of common folders to update</param>
        /// <param name="parent">The parent folder</param>
        /// <param name="isFirstLevel">if <code>true</code>, will request the subfolders of all folders found. Thsi settign depends on the current FolderTreeBrowseMode</param>
        /// <returns>A list of folders</returns>
        internal FolderCollection GetFolders(string path, CommonFolderCollection commonFolders, Folder parent = null, bool isFirstLevel = false)
        {
            var result = new FolderCollection(this, parent);
            var cmd = string.Format(Capabilities.XList && !Capabilities.XGMExt1 ? ImapCommands.XList : ImapCommands.List, path, Behavior.FolderTreeBrowseMode == FolderTreeBrowseMode.Full ? "*" : "%");
            IList<string> data = new List<string>();
            if (!SendAndReceive(cmd, ref data)) return result;

            for (var i = 0; i < data.Count - 1; i++)
            {
                var folder = Folder.Parse(data[i], ref parent, this);
                commonFolders.TryBind(ref folder);

                if (Behavior.ExamineFolders)
                    folder.Examine();

                if (folder.HasChildren && (isFirstLevel || Behavior.FolderTreeBrowseMode == FolderTreeBrowseMode.Full))
                    folder.SubFolders = GetFolders(folder.Path + Behavior.FolderDelimeter, commonFolders, folder);

                result.AddInternal(folder);

            }

            return result;

        }

        #region Obsolete

        [Obsolete("SelectFolder is obsolete", true)]
        public bool SelectFolder(string folderName)
        {
            throw new NotImplementedException();
        }

        [Obsolete("OAuth2LogIn is obsolete, please use Login(IImapCredentials) with OAuth2Credentials instead", true)]
        public bool OAuth2LogIn(string login, string authToken)
        {
            return Login(new OAuth2Credentials(login, authToken));
        }

        [Obsolete("Connection is obsolete, please use Connect instead", true)]
        public bool Connection()
        {
            return Connect();
        }

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


        #endregion

    }
}