using ImapX.Authentication;
using ImapX.Collections;
using ImapX.Commands;
using ImapX.Enums;
using ImapX.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ImapX
{
    public class ImapClient : ImapBase
    {
        protected CommonFolderCollection _folders;
        protected ImapIdentity _serverIdentity;
        protected Folder _selectedFolder;
        protected IdleState _idleState;
        protected CancellationTokenSource _idleCancellationTokenSource = new CancellationTokenSource();

        public ImapClient()
        {
            Behavior = new ClientBehavior();
        }

        /// <summary>
        /// Get or set the credentials used to authenticate
        /// </summary>
        public ImapCredentials Credentials { get; set; }

        public ImapIdentity ClientIdentity { get; set; }

        public ImapIdentity ServerIdentity
        {
            get
            {
                if (!Capabilities.Id)
                    return null;
                if (_serverIdentity == null)
                    _serverIdentity = Identity();

                return _serverIdentity;

            }
            protected set
            {
                _serverIdentity = value;
            }
        }
        
        /// <summary>
        /// The folder structure
        /// </summary>
        public CommonFolderCollection Folders
        {
            get
            {
                if (_folders == null)
                    _folders = new CommonFolderCollection(this, GetFolders());

                return _folders;
            }
        }

        internal Folder SelectedFolder
        {
            get
            {
                return _selectedFolder;
            }
            set
            {
                if (_selectedFolder != null)
                    _selectedFolder.ReadOnly = true;
                _selectedFolder = value;
            }
        }
        
        /// <summary>
        /// Authenticate using credentials set through the <code>Credentials</code> property
        /// </summary>
        /// <returns><code>true</code> if the authentication was successful</returns>
        public SafeResult Login()
        {
            if (Credentials == null)
                throw new ArgumentNullException("The credentials cannot be null");

            return Login(Credentials);
        }

        public Task<SafeResult> LoginAsync()
        {
            return Task.Run(() => Login());
        }

        /// <summary>
        /// Authenticate using a login and password
        /// </summary>
        /// <returns><code>true</code> if the authentication was successful</returns>
        public SafeResult Login(string login, string password)
        {
            return Login(new PlainCredentials(login, password));
        }

        public Task<SafeResult> LoginAsync(string login, string password)
        {
            return Task.Run(() => Login(login, password));
        }

        /// <summary>
        /// Authenticate using given credentials
        /// </summary>
        /// <returns><code>true</code> if the authentication was successful</returns>
        public SafeResult Login(ImapCredentials credentials)
        {
            Credentials = credentials;

            if (credentials.IsSupported(Capabilities))
            {
                var cmd = RunCommand(Credentials.ToCommand(this, GetNextCommandId(), Capabilities));
                
                if (cmd.State == CommandState.Ok)
                    IsAuthenticated = true;
                else
                    return new SafeResult(exception: new OperationFailedException("Failed to autheticate. Details: {0}", cmd.StateDetails));
            }
            
            return IsAuthenticated;
        }

        public Task<SafeResult> LoginAsync(ImapCredentials credentials)
        {
            return Task.Run(() => Login(credentials));
        }

        /// <summary>
        /// Logout from server
        /// </summary>
        /// <returns><code>true</code> if the logout was successful</returns>
        public SafeResult Logout()
        {
            if (!IsAuthenticated)
                return new SafeResult(exception: new InvalidStateException("You have to be authenticated to logout"));

            var cmd = RunCommand(new LogoutCommand(this, GetNextCommandId()));

            if (cmd.State == CommandState.Ok)
                IsAuthenticated = false;
            else
                return new SafeResult(exception: new OperationFailedException("Failed to autheticate. Details: {0}", cmd.StateDetails));

            return true;
        }

        public Task<SafeResult> LogoutAsync()
        {
            return Task.Run(() => Logout());
        }

        /// <summary>
        /// Sends the client identity to the server and returns the server identity.
        /// Sets the <code>ClientIdentity</code> property if the <code>clientIdentity</code> argument us not null.
        /// Sets the <code>ServerIdentity</code> property. 
        /// </summary>
        /// <param name="clientIdentity">The client identity to send to the server. If <code>null</code>, value from the <code>ClientIdentity</code> property is used.</param>
        /// <returns></returns>
        public ImapIdentity Identity(ImapIdentity clientIdentity = null)
        {
            if (clientIdentity != null)
                ClientIdentity = clientIdentity;

            if (!Capabilities.Id)
                throw new UnsupportedCapabilityException();
            
            var cmd = RunCommand(new IDCommand(this, GetNextCommandId(), clientIdentity ?? ClientIdentity));
            
            if (cmd.State != CommandState.Ok)
                throw new InvalidOperationException();

            ServerIdentity = cmd.Response;

            return cmd.Response;
        }

        internal SafeResult SelectFolder(Folder folder)
        {
            if (!folder.Selectable)
                return new SafeResult(false, new InvalidOperationException("The folder is not selectable"));

            if (_selectedFolder == folder)
                return true;

            var cmd = RunCommand(new SelectCommand(this, GetNextCommandId(), folder));
            
            if (cmd.State != CommandState.Ok)
                return new SafeResult(exception: new OperationFailedException("Failed to select folder. Details: {0}", cmd.StateDetails));
            
            return true;
        }

        internal Task<SafeResult> SelectFolderAsync(Folder folder)
        {
            return Task.Run(() => SelectFolder(folder));
        }

        internal void ScheduleExamine(Folder folder)
        {
            if (!folder.Selectable)
                throw new InvalidOperationException("The folder is not selectable");

            ScheduleCommand(new ExamineCommand(this, GetNextCommandId(), folder));
        }

        internal SafeResult ExamineFolder(Folder folder)
        {
            if (!folder.Selectable)
                return new SafeResult(false, new InvalidOperationException("The folder is not selectable"));

            var cmd = RunCommand(new ExamineCommand(this, GetNextCommandId(), folder));

            if (cmd.State != CommandState.Ok)
                return new SafeResult(exception: new OperationFailedException("Failed to examine folder. Details: {0}", cmd.StateDetails));
            
            return true;
        }

        internal SafeResult ExpungeFolder(Folder folder)
        {
            if (!folder.Selectable)
                return new SafeResult(false, new InvalidOperationException("The folder is not selectable"));

            var cmd = RunCommand(new ExpungeCommand(this, GetNextCommandId(), folder));

            if (cmd.State != CommandState.Ok)
                return new SafeResult(exception: new OperationFailedException("Failed to expunge folder. Details: {0}", cmd.StateDetails));

            return true;
        }

        internal SafeResult DeleteFolder(Folder folder)
        {
            if (!folder.CanBeDeleted)
                return new SafeResult(false, new InvalidOperationException("The folder cannot be removed"));

            var cmd = RunCommand(new DeleteCommand(this, GetNextCommandId(), folder));

            if (cmd.State != CommandState.Ok)
                return new SafeResult(exception: new OperationFailedException("Failed to expunge folder. Details: {0}", cmd.StateDetails));

            return true;
        }

        internal IEnumerable<Message> SearchFolder(Folder folder, string query)
        {
            SelectFolder(folder);

            var cmd = RunCommand(new SearchCommand(this, GetNextCommandId(), folder, query));

            if (cmd.State != CommandState.Ok)
                throw new OperationFailedException("Failed to search the messages. Details: {0}", cmd.StateDetails);

            return cmd.Response;
        }

        internal void FetchMessage(Message message, MessageFetchMode mode = MessageFetchMode.ClientDefault, 
            bool reloadHeaders = false, string bodyPartNumber = null)
        {
            if (mode == MessageFetchMode.ClientDefault)
                mode = Behavior.MessageFetchMode;

            mode = mode &= ~message.DownloadProgress;

            mode &= ~MessageFetchMode.GMailExtendedData;

            if (mode == MessageFetchMode.None || mode == MessageFetchMode.Initial)
                return;

            var cmd = RunCommand(new FetchCommand(this, GetNextCommandId(), message, mode, reloadHeaders, bodyPartNumber));

            if (cmd.State != CommandState.Ok)
                throw new OperationFailedException("Failed to fetch the message. Details: {0}", cmd.StateDetails);

            if (mode.HasFlag(MessageFetchMode.Body))
                message.Body.Download();
        }

        internal SafeResult Store(Message message, StoreAction action, IEnumerable<string> flags)
        {
            var cmd = RunCommand(new StoreCommand(this, GetNextCommandId(), message, action, flags));
            return cmd.State == CommandState.Ok;
        }

        internal void ScheduleFetch(Message message, MessageFetchMode mode = MessageFetchMode.ClientDefault, bool reloadHeaders = false)
        {
            mode = mode &= ~message.DownloadProgress;

            if (mode == MessageFetchMode.None)
                return;

            ScheduleCommand(new FetchCommand(this, GetNextCommandId(), message, mode, reloadHeaders));
        }

        internal FolderStatus GetFolderStatus(Folder folder, FolderStatusType type)
        {
            // Do not get the append limit if it is not supported
            if (Capabilities.AppendLimit < 0)
                type &= ~FolderStatusType.AppendLimit;

            var cmd = RunCommand(new StatusCommand(this, GetNextCommandId(), folder, type));

            if (cmd.State != CommandState.Ok)
                throw new OperationFailedException("Failed to request the folder status. Details: {0}", cmd.StateDetails);

            return cmd.Response;
        }

        internal SafeResult CloseFolder(Folder folder)
        {
            if (SelectedFolder != folder)
                return new SafeResult(exception: new InvalidStateException("The folder cannot be closed as it's not the selected folder"));

            var cmd = RunCommand(new CloseCommand(this, GetNextCommandId(), folder));

            if (cmd.State != CommandState.Ok)
                return new SafeResult(exception: new OperationFailedException("Failed to close folder. Details: {0}", cmd.StateDetails));

            return true;
        }

        internal Folder CreateFolder(string folderName, Folder parent)
        {
            if (string.IsNullOrEmpty(folderName))
                throw new ArgumentException("The folder name cannot be empty");
            
            var cmd = RunCommand(new CreateCommand(this, GetNextCommandId(), folderName, parent));

            if (cmd.State != CommandState.Ok)
                throw new OperationFailedException("Failed to create folder. Details: {0}", cmd.StateDetails);

            return cmd.Response;
        }

        internal IEnumerable<Folder> GetFolders(Folder parentFolder = null)
        {
            return GetFolders(parentFolder, Behavior.FolderTreeBrowseMode);
        }

        internal IEnumerable<Folder> GetFolders(Folder parentFolder, FolderTreeBrowseMode mode)
        {
            var cmd = RunCommand(
                (!Capabilities.SpecialUse || (Behavior.ListFolderStatusType != FolderStatusType.None && Capabilities.ListStatus)) &&
                Capabilities.XList ? 
                    new XListCommand(this, GetNextCommandId(), parentFolder, mode) { Base = this } : 
                    new ListCommand(this, GetNextCommandId(), parentFolder, mode) { Base = this }
                );
            
            if (cmd.State != CommandState.Ok)
                throw new InvalidOperationException();

            if (parentFolder != null)
                Folders.BindRangeInternal(cmd.Response);

            return cmd.Response;
        }

        public SafeResult UnselectCurrentFolder()
        {
            if (SelectedFolder == null)
                return new SafeResult(exception: new InvalidStateException("The folder cannot be unselected as no folder is selected"));

            var cmd = RunCommand(new UnselectCommand(this, GetNextCommandId()));

            if (cmd.State != CommandState.Ok)
                return new SafeResult(exception: new OperationFailedException("Failed to unselect the current folder. Details: {0}", cmd.StateDetails));

            return true;
        }
    }
}
