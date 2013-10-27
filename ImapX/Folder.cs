﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ImapX.Collections;
using ImapX.Constants;
using ImapX.EncodingHelpers;
using ImapX.Enums;
using ImapX.Exceptions;
using ImapX.Extensions;
using ImapX.Flags;
using System.Collections;
using ImapX.Parsing;

namespace ImapX
{

    public class Folder
    {
        private readonly ImapClient _client;

        private readonly Folder _parent;
        private FolderFlagCollection _flags;
        private MessageCollection _messages;
        private string _name;
        private string _path;
        private FolderCollection _subFolders;

        internal Folder() { }

        internal Folder(string path, IEnumerable<string> flags, ref Folder parent, ImapClient client)
        {
            _path = path;
            _name = ImapUTF7.Decode(_path.Split(client.Behavior.FolderDelimeter).Last());
            UpdateFlags(flags);
            _parent = parent;
            _client = client;
            GMailThreads = new GMailThreadCollection();
        }

        /// <summary>
        ///     The number of messages in the mailbox.
        /// </summary>
        public long Exists { get; private set; }

        /// <summary>
        ///     The number of messages with the \Recent flag set.
        /// </summary>
        public long Recent { get; private set; }

        [Obsolete("Recents is obsolete, please use Recent instead", true)]
        public long Recents { get; private set; }

        /// <summary>
        ///     The message sequence number of the first unseen message in the mailbox.
        /// </summary>
        public long Unseen { get; private set; }

        /// <summary>
        ///     Subfolders of the current folder
        /// </summary>
        public FolderCollection SubFolders
        {
            get
            {
                return _subFolders ??
                       (_subFolders =
                           HasChildren
                               ? _client.GetFolders(_path + _client.Behavior.FolderDelimeter, _client.Folders, this)
                               : new FolderCollection(_client, this));
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
                if (_messages != null) return _messages;
                _messages = new MessageCollection(_client, this);
                if (_client.Behavior.AutoPopulateFolderMessages)
                    _messages.Download();
                return _messages;
            }
            internal set { _messages = value; }
        }

        /// <summary>
        ///     The collection of GMail message threads in this folder. The collection is populated when messages are requested.
        /// </summary>
        public GMailThreadCollection GMailThreads { get; internal set; }

        /// <summary>
        ///     Gets whether the current folder has subfolders
        /// </summary>
        public bool HasChildren { get; internal set; }

        /// <summary>
        ///     Gets or sets the name of the folder. Setting this property will rename the folder.
        /// </summary>
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
        ///     Gets whether the folder can be selected (messages can be searched)
        /// </summary>
        public bool Selectable { get; private set; }

        /// <summary>
        ///     Folder path on the server
        /// </summary>
        public string Path
        {
            get { return _path; }
            internal set { _path = value; }
        }

        [Obsolete("FolderPath is obsolete, please use Path instead")]
        public string FolderPath
        {
            get { return _path; }
            internal set { _path = value; }
        }

        /// <summary>
        ///     Flags of current folder. Determine the type of the folder.
        /// </summary>
        /// <see cref="ImapY.Flags.FolderFlags" />
        public FolderFlagCollection Flags
        {
            get { return _flags; }
        }

        /// <summary>
        ///     A list of message flags that the client can change permanently.  If this is missing, the client should assume that
        ///     all flags can be changed permanently.
        /// </summary>
        public IEnumerable<string> AllowedPermanentFlags { get; internal set; }

        /// <summary>
        ///     The next unique identifier value.
        /// </summary>
        public long UidNext { get; private set; }

        /// <summary>
        ///     The unique identifier validity value.
        /// </summary>
        public string UidValidity { get; private set; }

        internal static Folder Parse(string commandResult, ref Folder parent, ImapClient client)
        {
            Match match = Expressions.FolderParseRex.Match(commandResult);

            if (match.Success && match.Groups.Count == 4)
            {
                string[] flags = match.Groups[1].Value.Split(' ');

                string path = match.Groups[3].Value;

                if (client.Behavior.FolderDelimeter == '\0')
                    client.Behavior.FolderDelimeter = string.IsNullOrEmpty(match.Groups[2].Value)
                        ? '"'
                        : match.Groups[2].Value.ToCharArray()[0];

                return new Folder(path, flags, ref parent, client);
            }

            return null;
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

        internal void UpdatePath(string parentPath)
        {
            int i = _path.LastIndexOf(_client.Behavior.FolderDelimeter);
            _path = parentPath + _path.Substring(i, _path.Length - i);
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

            IList<string> data = new List<string>();

            string encodedName = ImapUTF7.Encode(name);

            int i = _path.LastIndexOf(_client.Behavior.FolderDelimeter);

            string newPath = i < 1 ? encodedName : _path.Substring(0, i + 1) + encodedName;

            if (!_client.SendAndReceive(string.Format(ImapCommands.Rename, _path, newPath), ref data)) return false;

            _name = name;
            _path = newPath;

            if (!HasChildren || _subFolders == null) return true;
            foreach (var folder in SubFolders)
                folder.UpdatePath(_path);

            return true;
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

            IList<string> data = new List<string>();
            if (!_client.SendAndReceive(string.Format(ImapCommands.Delete, _path), ref data))
                return false;

            if (_parent != null)
                _parent._subFolders.RemoveInternal(this);
            else
                _client.Folders.RemoveInternal(this);

            return true;
        }

        /// <summary>
        ///     Permanently removes all messages that have the \Deleted flag set from the current folder.
        /// </summary>
        /// <returns></returns>
        public bool Expunge()
        {
            Folder selectedFolder = _client.SelectedFolder;

            if (!Equals(selectedFolder))
                Select();

            IList<string> data = new List<string>();
            bool result = _client.SendAndReceive(ImapCommands.Expunge, ref data);

            if (!Equals(selectedFolder))
                selectedFolder.Select();

            return result;
        }

        private void ProcessSelectOrExamineResult(IList<string> data)
        {
            if (data == null)
                return;

            for (int i = 0; i < data.Count - 1; i++)
            {
                Match m = Expressions.ExistsRex.Match(data[i]);

                if (m.Success)
                {
                    Exists = long.Parse(m.Groups[1].Value);
                    continue;
                }

                m = Expressions.RecentRex.Match(data[i]);

                if (m.Success)
                {
                    Recent = long.Parse(m.Groups[1].Value);
                    continue;
                }

                m = Expressions.UnseenRex.Match(data[i]);

                if (m.Success)
                {
                    Unseen = long.Parse(m.Groups[1].Value);
                    continue;
                }

                m = Expressions.UIdValidityRex.Match(data[i]);

                if (m.Success)
                {
                    UidValidity = m.Groups[1].Value;
                    continue;
                }

                m = Expressions.UIdNextRex.Match(data[i]);

                if (m.Success)
                {
                    UidNext = long.Parse(m.Groups[1].Value);
                    continue;
                }

                m = Expressions.PermanentFlagsRex.Match(data[i]);

                if (m.Success)
                    AllowedPermanentFlags = m.Groups[1].Value.Split(' ').Where(_ => !string.IsNullOrEmpty(_));
                //else
                //{
                //    m = Expressions.FlagsRex.Match(data[i]);

                //    if (m.Success)
                //        UpdateFlags(m.Groups[1].Value.Split(' '));
                //}
            }
        }

        public bool Examine()
        {
            IList<string> data = new List<string>();
            if (!_client.SendAndReceive(string.Format(ImapCommands.Examine, _path), ref data))
                return false;

            ProcessSelectOrExamineResult(data);

            return true;
        }

        /// <summary>
        ///     Selects the folder, making messages available for queries
        /// </summary>
        /// <returns></returns>
        public bool Select()
        {
            if (!Selectable)
                throw new InvalidOperationException("A non-selectable folder cannot be selected.");

            IList<string> data = new List<string>();
            if (!_client.SendAndReceive(string.Format(ImapCommands.Select, _path), ref data))
                return false;

            ProcessSelectOrExamineResult(data);

            _client.SelectedFolder = this;

            return true;
        }

        internal Message[] Fetch(IEnumerable<long> uIds, MessageFetchMode mode = MessageFetchMode.ClientDefault)
        {
            var result = new List<Message>();

            foreach (
                Message msg in
                    uIds.Select(uId => Messages.FirstOrDefault(_ => _.UId == uId) ?? new Message(uId, _client, this))
                )
            {
                msg.Download(mode);

                if (!Messages.Contains(msg))
                    Messages.AddInternal(msg);

                if (!result.Contains(msg))
                    result.Add(msg);
            }
            return result.ToArray();
        }

        internal long[] SearchMessageIds(string query = "ALL", int count = -1)
        {
            if (_client.SelectedFolder != this && !Select())
                throw new OperationFailedException("The folder couldn't be selected for search.");

            IList<string> data = new List<string>();
            if (!_client.SendAndReceive(string.Format(ImapCommands.Search, query), ref data))
                throw new ArgumentException("The search query couldn't be processed");

            var result = Expressions.SearchRex.Match(data.FirstOrDefault(Expressions.SearchRex.IsMatch) ?? "");

            if (!result.Success)
                //throw new OperationFailedException("The data returned from the server doesn't match the requirements");
                return new long[0];

            return count < 0
                ? result.Groups[1].Value.Trim().Split(' ').Select(long.Parse).ToArray()
                : result.Groups[1].Value.Trim().Split(' ').OrderBy(_ => _).Take(count).Select(long.Parse).ToArray();
        }

        /// <summary>
        ///     Downloads messages from server using default or given mode.
        /// </summary>
        /// <param name="query">The search query to filter messages. <code>ALL</code> by default</param>
        /// <param name="mode">The message fetch mode, allows to select which parts of the message will be requested.</param>
        /// <param name="count">
        ///     The maximum number of messages that will be requested. Set <code>count</code> to <code>-1</code>
        ///     will request all messages which match the given query.
        /// </param>
        public Message[] Search(string query = "ALL", MessageFetchMode mode = MessageFetchMode.ClientDefault,
            int count = -1)
        {
            return Fetch(SearchMessageIds(query, count), mode);
        }

         [Obsolete("This Search overload is obsolete, please use another instead", true)]
        public MessageCollection Search(string path, bool makeProcess)
        {
            throw new NotImplementedException();
        }

        [Obsolete("CreateFolder is obsolete, please use SubFolders.Add instead", true)]
        public bool CreateFolder(string name)
        {
            return SubFolders.Add(name) != null;
        }

        [Obsolete("CopyMessageToFolder is obsolete, please use Message.CopyTo instead", true)]
        public bool CopyMessageToFolder(Message msg, Folder folder)
        {
            return msg.CopyTo(folder);
        }

        [Obsolete("DeleteMessage is obsolete, please use Message.Remove instead", true)]
        public bool DeleteMessage(Message msg)
        {
            return msg.Remove();
        }

        [Obsolete("DeleteFolder is obsolete, please use Remove instead")]
        public bool DeleteFolder()
        {
            return Remove();
        }


        [Obsolete("MoveMessageToFolder is obsolete, please use Message.MoveTo instead", true)]
        public bool MoveMessageToFolder(Message msg, Folder folder)
        {
            return msg.MoveTo(folder);
        }

         [Obsolete("SubFolder is obsolete, please use SubFolders instead", true)]
         public FolderCollection SubFolder { get; internal set; }

         [Obsolete("CheckNewMessage is obsolete", true)]
        public MessageCollection CheckNewMessage(bool processMessages)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Appends a new message to the end of specified folder
        /// </summary>
        /// <param name="eml">The eml data of the message to append</param>
        /// <param name="flags">The flags to be set for the message. If empty, the server will add the \Recent flag automatically</param>
        /// <param name="date">The internal date of the message to be set</param>
        /// <returns><code>true</code> if the message was appended, otherwise <code>false</code></returns>
        public bool AppendMessage(string eml, IEnumerable<string> flags = null, DateTime? date = null)
        {
            if(string.IsNullOrEmpty(eml))
                throw new ArgumentException("eml cannot be empty");

            IList<string> data = new List<string>();

            var msgUploader = new MessageUploader(eml);

            var dateStr = date.HasValue ? date.Value.ToString("dd-MM-yyyy") : "";

            if (dateStr.StartsWith("0"))
                dateStr = " " + dateStr.Substring(1, dateStr.Length - 1);

            return _client.SendAndReceive(
                string.Format(ImapCommands.Append,
                _path) + " {" + eml.Length + "}",
                ref data, msgUploader);
        }

#if !WINDOWS_PHONE

        public bool AppendMessage(System.Net.Mail.MailMessage mailMessage)
        {
            if (mailMessage == null)
                throw new ArgumentNullException("mailMessage", "mailMessage cannot be null");

            try
            {
                return AppendMessage(mailMessage.ToEml());
            }
            catch {
                return false;
            }

        }

#endif

        [Obsolete("AppendMessage(Message) is obsolete", true)]
        public bool AppendMessage(Message msg)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Removes all messages from current folder
        /// </summary>
        /// <returns></returns>
        public bool EmptyFolder()
        {
            IEnumerable<string> ids = SearchMessageIds().GroupUIdSequences();

            if (!ids.Any())
                return true;

            foreach (string group in ids)
            {
                IList<string> data = new List<string>();
                if (
                    !_client.SendAndReceive(
                        string.Format(ImapCommands.Store, group, "+FLAGS",
                            MessageFlags.Deleted), ref data)) 
                    return false;
            }

            if (!Expunge())
                return false;

            Messages.ClearInternal();

            Examine();

            return true;
        }
    }
}