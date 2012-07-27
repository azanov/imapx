﻿using System;
namespace ImapX
{
    public class ImapClient
    {
        public Imap _client;
        private FolderCollection _folders;
        public string SelectedFolder
        {
            get
            {
                return this._client._selectedFolder;
            }
        }
        public bool IsConnected
        {
            get
            {
                return this._client._isConnected;
            }
        }
        public bool IsDebug
        {
            get
            {
                return this._client.IsDebug;
            }
            set
            {
                this._client.IsDebug = value;
            }
        }
        public bool IsLogined
        {
            get
            {
                return this._client._isLogged;
            }
        }
        public FolderCollection Folders
        {
            get
            {
                if (this._folders == null)
                {
                    this._folders = this.GetFolders();
                }
                return this._folders;
            }
            set
            {
                this._folders = value;
            }
        }
        public FolderCollection GetFolders()
        {
            FolderCollection folders = this._client.GetFolders("");
            foreach (Folder current in folders)
            {
                current._client = this._client;
            }
            return folders;
        }
        public ImapClient(string host, int port, bool useSsl)
        {
            this._client = new Imap(host, port, useSsl);
        }
        public bool Connection()
        {
            return this._client.Connect();
        }
        public bool LogIn(string login, string password)
        {
            return this._client.LogIn(login, password);
        }
        public bool Disconnect()
        {
            return this._client.Disconnect();
        }
        public bool LogOut()
        {
            return this._client.LogOut();
        }
        public void AppendMessageForYahoo(string Folder, Message msg, string flags)
        {
        }
        public MessageCollection EndUtf8Searsh(byte[] b)
        {
            MessageCollection messageCollection = this._client.SearchUtf8Data(b);
            if (messageCollection != null)
            {
                foreach (Message current in messageCollection)
                {
                    current._client = this._client;
                }
            }
            return messageCollection;
        }
        public bool CreateFolder(string name)
        {
            return this._client.CreateFolder(name);
        }
    }
}