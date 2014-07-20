using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using ImapX.Collections;
using ImapX.Constants;
using ImapX.Enums;
using ImapX.Exceptions;
using ImapX.Parsing;

#if !NETFX_CORE
using System.Security.Cryptography.X509Certificates;
using ThreadState = System.Threading.ThreadState;
using Timer = System.Timers.Timer;
#endif
#if WINDOWS_PHONE || NETFX_CORE
using SocketEx;
#else
using System.Net.Security;

#endif

namespace ImapX
{
    public class ImapBase : IDisposable
    {
        public const int DefaultImapPort = 143;
        public const int DefaultImapSslPort = 993;
        protected static readonly Regex NewLineRex = new Regex(@"\r\n");

        protected TcpClient _client;
        protected long _counter;

        protected string _host;
        protected Stream _ioStream;
        protected object _lock = 0;
        protected int _port = DefaultImapPort;

        protected SslProtocols _sslProtocol = SslProtocols.None;
        protected StreamReader _streamReader;
        protected bool _validateServerCertificate = true;
        protected DateTime _lastActivity;

        /// <summary>
        ///     Basic client behavior settings like folder browse mode and message download mode
        /// </summary>
        public ClientBehavior Behavior { get; protected set; }

        /// <summary>
        ///     Gets whether the client is authenticated
        /// </summary>
        public bool IsAuthenticated { get; protected set; }

        /// <summary>
        ///     Gets whether the client is connected to the server
        /// </summary>
        public bool IsConnected
        {
            get { return _client != null && _client.Connected; }
        }

        public bool IsDebug { get; set; }

        /// <summary>
        ///     The server address to connect to
        /// </summary>
        /// <exception cref="Exceptions.InvalidStateException">On set, if the client is connected.</exception>
        public string Host
        {
            get { return _host; }
            set
            {
                if (IsConnected)
                    throw new InvalidStateException(
                        "The host cannot be changed after the connection has been established. Please disconnect first.");
                _host = value;
            }
        }

        /// <summary>
        ///     The server port used. 143 by default
        /// </summary>
        /// <exception cref="Exceptions.InvalidStateException">On set, if the client is connected.</exception>
        public int Port
        {
            get { return _port; }
            set
            {
                if (IsConnected)
                    throw new InvalidStateException(
                        "The port cannot be changed after the connection has been established. Please disconnect first.");
                _port = value;
            }
        }

        internal Folder SelectedFolder { get; set; }

        /// <summary>
        ///     The SSL protocol used. <code>SslProtocols.None</code> by default
        /// </summary>
        /// <exception cref="Exceptions.InvalidStateException">On set, if the client is connected.</exception>
        public SslProtocols SslProtocol
        {
            get { return _sslProtocol; }
            set
            {
                if (IsConnected)
                    throw new InvalidStateException(
                        "The SSL protocol cannot be changed after the connection has been established. Please disconnect first.");
                _sslProtocol = value;
            }
        }

        /// <summary>
        ///     Get or set if SSL should be used. <code>false</code> by default.  If set to <code>true</code>, the
        ///     <code>SslProtocol</code> will be set to <code>SslProtocols.Default</code>
        /// </summary>
        /// <exception cref="Exceptions.InvalidStateException">On set, if the client is connected.</exception>
        public bool UseSsl
        {
            get { return _sslProtocol != SslProtocols.None; }
            set { _sslProtocol = value ? SslProtocols.Default : SslProtocols.None; }
        }

        /// <summary>
        ///     Get or set whether the server certificate should be validated when using SSL. <code>true</code> by default
        /// </summary>
        /// <exception cref="Exceptions.InvalidStateException">On set, if the client is connected.</exception>
        public bool ValidateServerCertificate
        {
            get { return _validateServerCertificate; }
            set
            {
                if (IsConnected)
                    throw new InvalidStateException(
                        "The certificate validation mode cannot be changed after the connection has been established. Please disconnect first.");

                _validateServerCertificate = value;
            }
        }

        /// <summary>
        ///     The server capabilities
        /// </summary>
        public Capability Capabilities { get; protected set; }

        /// <summary>
        ///     Disconnects from server and disposes the objects
        /// </summary>
        public void Dispose()
        {
            CleanUp();
        }

        /// <summary>
        ///     Connect using set values.
        /// </summary>
        /// <returns><code>true</code> if the connection was successful</returns>
        /// <exception cref="Exceptions.InvalidStateException">If the client is already connected.</exception>
        public bool Connect()
        {
            return Connect(_host, _port, _sslProtocol, _validateServerCertificate);
        }

        /// <summary>
        ///     Connects to an IMAP server on the default port (143; 993 if SSL is used)
        /// </summary>
        /// <param name="host">Server address</param>
        /// <param name="useSsl">Defines whether SSL should be used.</param>
        /// <param name="validateServerCertificate">Defines whether the server certificate should be validated when SSL is used</param>
        /// <returns><code>true</code> if the connection was successful</returns>
        /// <exception cref="Exceptions.InvalidStateException">If the client is already connected.</exception>
        public bool Connect(string host, bool useSsl = false, bool validateServerCertificate = true)
        {
            return Connect(host,
                useSsl ? DefaultImapSslPort : DefaultImapPort,
                useSsl ? SslProtocols.Default : SslProtocols.None,
                validateServerCertificate);
        }

        /// <summary>
        ///     Connects to an IMAP server on the specified port
        /// </summary>
        /// <param name="host">Server address</param>
        /// <param name="port">Server port</param>
        /// <param name="useSsl">Defines whether SSL should be used</param>
        /// <param name="validateServerCertificate">Defines whether the server certificate should be validated when SSL is used</param>
        /// <returns><code>true</code> if the connection was successful</returns>
        /// <exception cref="Exceptions.InvalidStateException">If the client is already connected.</exception>
        public bool Connect(string host, int port, bool useSsl = false, bool validateServerCertificate = true)
        {
            return Connect(host,
                port,
                useSsl ? SslProtocols.Default : SslProtocols.None,
                validateServerCertificate);
        }

        /// <summary>
        ///     Connects to an IMAP server on the specified port
        /// </summary>
        /// <param name="host">Server address</param>
        /// <param name="port">Server port</param>
        /// <param name="sslProtocol">SSL protocol to use, <code>SslProtocols.None</code> by default</param>
        /// <param name="validateServerCertificate">Defines whether the server certificate should be validated when SSL is used</param>
        /// <returns><code>true</code> if the connection was successful</returns>
        /// <exception cref="Exceptions.InvalidStateException">If the client is already connected.</exception>
        public bool Connect(string host, int port, SslProtocols sslProtocol = SslProtocols.None,
            bool validateServerCertificate = true)
        {
            _host = host;
            _port = port;
            _sslProtocol = sslProtocol;
            _validateServerCertificate = validateServerCertificate;

            if (IsConnected)
                throw new InvalidStateException("The client is already connected. Please disconnect first.");

            try
            {
#if !WINDOWS_PHONE && !NETFX_CORE
                _client = new TcpClient(_host, _port);

                if (_sslProtocol == SslProtocols.None)
                {
                    _ioStream = _client.GetStream();
                    _streamReader = new StreamReader(_ioStream);
                }
                else
                {
                    _ioStream = new SslStream(_client.GetStream(), false, CertificateValidationCallback, null);
                    (_ioStream as SslStream).AuthenticateAsClient(_host, null, _sslProtocol, false);
                    _streamReader = new StreamReader(_ioStream);
                }
#else
    //TODO: Add support for Tls
                _client = _sslProtocol == SslProtocols.None ? new TcpClient(_host, _port) : new SecureTcpClient(_host, _port);
                _ioStream = _client.GetStream();
                _streamReader = new StreamReader(_ioStream);
#endif

                string result = _streamReader.ReadLine();

                _lastActivity = DateTime.Now;

                if (result != null && result.StartsWith(ResponseType.ServerOk))
                {
                    Capability();
                    return true;
                }
                else if (result != null && result.StartsWith(ResponseType.ServerPreAuth))
                {
                    IsAuthenticated = true;
                    Capability();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (!IsConnected)
                    CleanUp();
            }
        }


        /// <summary>
        ///     Disconnects from server
        /// </summary>
        public void Disconnect()
        {
            //if (_idleState != IdleState.Off)
            //    PauseIdling();
            CleanUp();
        }

        /// <summary>
        ///     Disconnects from server and disposes the objects
        /// </summary>
        protected void CleanUp()
        {
            _counter = 0;

            if (!IsConnected)
                return;

#if !NETFX_CORE
            StopIdling();
#endif
            if (_streamReader != null)
                _streamReader.Dispose();

            if (_ioStream != null)
                _ioStream.Dispose();

            if (_client != null)
#if !WINDOWS_PHONE && !NETFX_CORE
                _client.Close();
#else
                _client.Dispose();
#endif
        }


#if !WINDOWS_PHONE && !NETFX_CORE

        /// <summary>
        ///     The certificate validation callback
        /// </summary>
        protected bool CertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return sslPolicyErrors == SslPolicyErrors.None || !_validateServerCertificate;
        }

#endif

        protected void Capability()
        {
            IList<string> data = new List<string>();
            if (SendAndReceive(ImapCommands.Capability, ref data) && data.Count > 0)
                Capabilities = new Capability(data[0]);
        }

        public bool SendAndReceive(string command, ref IList<string> data, CommandProcessor processor = null,
            Encoding encoding = null, bool pushResultToDatadespiteProcessor = false)
        {
#if !NETFX_CORE
            if (_idleState == IdleState.On)
                PauseIdling();
#endif
            lock (_lock)
            {
                if (_client == null || !_client.Connected)
                    throw new SocketException((int) SocketError.NotConnected);

                const string tmpl = "IMAPX{0} {1}";
                _counter++;

                StreamReader reader = encoding == null || Equals(encoding, Encoding.UTF8)
                    ? _streamReader
                    : new StreamReader(_ioStream, encoding);

                var parts = new Queue<string>(NewLineRex.Split(command));

                string text = string.Format(tmpl, _counter, parts.Dequeue().Trim()) + "\r\n";
                byte[] bytes = Encoding.UTF8.GetBytes(text.ToCharArray());

                if (IsDebug)
                    Debug.WriteLine(text);

                _ioStream.Write(bytes, 0, bytes.Length);

                _lastActivity = DateTime.Now;

                while (true)
                {
                    string tmp = reader.ReadLine();

                    if (tmp == null)
                    {
#if !NETFX_CORE
                        if (_idleState == IdleState.Paused)
                            StartIdling();
#endif
                        return false;
                    }

                    if (IsDebug)
                        Debug.WriteLine(tmp);

                    if (processor == null || pushResultToDatadespiteProcessor)
                        data.Add(tmp);

                    if (processor != null)
                        processor.ProcessCommandResult(tmp);

                    if (tmp.StartsWith("+ ") && (parts.Count > 0 || (processor != null && processor.TwoWayProcessing)))
                    {
                        if (parts.Count > 0)
                        {
                            text = parts.Dequeue().Trim() + "\r\n";

                            if (IsDebug)
                                Debug.WriteLine(text);

                            bytes = Encoding.UTF8.GetBytes(text.ToCharArray());
                        }
                        else if (processor != null)
                            bytes = processor.AppendCommandData(tmp);

                        _ioStream.Write(bytes, 0, bytes.Length);
                        continue;
                    }

                    if (tmp.StartsWith(string.Format(tmpl, _counter, ResponseType.Ok)))
                    {
#if !NETFX_CORE
                        if (_idleState == IdleState.Paused)
                            StartIdling();
#endif
                        return true;
                    }

                    if (tmp.StartsWith(string.Format(tmpl, _counter, ResponseType.PreAuth)))
                    {
#if !NETFX_CORE
                        if (_idleState == IdleState.Paused)
                            StartIdling();
#endif
                        return true;
                    }

                    if (tmp.StartsWith(string.Format(tmpl, _counter, ResponseType.No)) ||
                        tmp.StartsWith(string.Format(tmpl, _counter, ResponseType.Bad)))
                    {
#if !NETFX_CORE
                        if (_idleState == IdleState.Paused)
                            StartIdling();
#endif
                        Match serverAlertMatch = Expressions.ServerAlertRex.Match(tmp);
                        if (serverAlertMatch.Success && tmp.Contains("IMAP") && tmp.Contains("abled"))
                            throw new ServerAlertException(serverAlertMatch.Groups[1].Value);
                        return false;
                    }
                }
            }
        }

#if !NETFX_CORE

        #region Idle support

        private IdleState _idleState;

        internal IdleState IdleState
        {
            get { return _idleState; }
        }

        private Thread _idleLoopThread;
        private Thread _idleProcessThread;
        private long _lastIdleUId;
        private readonly ThreadSafeQueue<string> _idleEvents = new ThreadSafeQueue<string>();
        private readonly AutoResetEvent _idleEventsResetEvent = new AutoResetEvent(false);
        private Timer _maintainIdleConnectionTimer;

        internal bool StartIdling()
        {
            if (SelectedFolder == null)
                return false;

            switch (_idleState)
            {
                case IdleState.Off:

                    //if (SelectedFolder.UidNext == 0)
                    //    SelectedFolder.Status(new[] { FolderStatusFields.UIdNext });
                    _lastIdleUId = SelectedFolder.UidNext;
                    break;
                case IdleState.On:
                    return true;

                case IdleState.Paused:

                    //if (SelectedFolder.UidNext == 0)
                    //    SelectedFolder.Status(new[] { FolderStatusFields.UIdNext });
                    _lastIdleUId = SelectedFolder.UidNext;

                    break;
            }

            lock (_lock)
            {
                const string tmpl = "IMAPX{0} {1}";
                _counter++;
                string text = string.Format(tmpl, _counter, "IDLE") + "\r\n";
                byte[] bytes = Encoding.UTF8.GetBytes(text.ToCharArray());
                if (IsDebug)
                    Debug.WriteLine(text);

                _ioStream.Write(bytes, 0, bytes.Length);
                if (_ioStream.ReadByte() != '+')
                    return false;
                var line = _streamReader.ReadLine();

                if (IsDebug && !string.IsNullOrEmpty(line))
                    Debug.WriteLine(line);
            }

            _idleState = IdleState.On;

            _idleLoopThread = new Thread(WaitForIdleServerEvents) {IsBackground = true};
            _idleLoopThread.Start();

            if (OnIdleStarted != null)
                OnIdleStarted(SelectedFolder, new IdleEventArgs
                {
                    Client = SelectedFolder.Client,
                    Folder = SelectedFolder
                });

            return true;
        }

        private void MaintainIdleConnection()
        {
            if (_maintainIdleConnectionTimer == null)
            {

                _maintainIdleConnectionTimer = new Timer(Behavior.NoopIssueTimeout*1000) {AutoReset = true};
                _maintainIdleConnectionTimer.Elapsed += (sender, e) =>
                {
                    if (_idleState != IdleState.On) return;

                    IList<string> data = new List<string>();
                    if (!SendAndReceive(ImapCommands.Noop, ref data))
                    {
                        // that should not be the final solution. we need a fallback
                        _maintainIdleConnectionTimer.Enabled = false;
                    }
                };
            }
            _maintainIdleConnectionTimer.Enabled = true;
        }

        private void ProcessIdleServerEvents()
        {
            while (true)
            {
                string tmp;
                if (!_idleEvents.TryDequeue(out tmp))
                {
                    if (_idleState == IdleState.On)
                    {
                        // instead of an instant retry: wait for a signal that will be emitted after a new value was put into the queue
                        _idleEventsResetEvent.WaitOne();
                        continue;
                    }
                    return;
                }
                

                Match match = Expressions.IdleResponseRex.Match(tmp);

                if (!match.Success)
                    continue;

                if (match.Groups[2].Value == "EXISTS")
                {
                    SelectedFolder.Status(new[] {FolderStatusFields.UIdNext});

                    if (_lastIdleUId != SelectedFolder.UidNext)
                    {
                        var msgs =
                            SelectedFolder.Search(string.Format("UID {0}:{1}", _lastIdleUId, SelectedFolder.UidNext));
                        var args = new IdleEventArgs
                        {
                            Folder = SelectedFolder,
                            Messages = msgs
                        };
                        if (OnNewMessagesArrived != null)
                            OnNewMessagesArrived(SelectedFolder, args);
                        SelectedFolder.RaiseNewMessagesArrived(args);
                        _lastIdleUId = SelectedFolder.UidNext;
                    }
                }
            }
        }

        private void WaitForIdleServerEvents()
        {
            if (_idleProcessThread == null)
            {
                _idleProcessThread = new Thread(ProcessIdleServerEvents) {IsBackground = true};
                _idleProcessThread.Start();
            }

            // a silent voice tells me to call that function from outside, but who cares
            MaintainIdleConnection();

            while (_idleState == IdleState.On)
            {
                if (_ioStream.ReadByte() != -1)
                {
                    string tmp = _streamReader.ReadLine();

                    if (tmp == null)
                        continue;

                    if (IsDebug)
                        Debug.WriteLine(tmp);

                    if (tmp.ToUpper().Contains("OK"))
                    {
                        _idleState = IdleState.Off;
                        // unblock the processing thread, it will end itself because _idleState is Off
                        _idleEventsResetEvent.Set();
                        return;
                    }
                    _idleEvents.Enqueue(tmp);
                    // signal the waiting event processing thread to continue
                    _idleEventsResetEvent.Set();

                    if (_idleProcessThread != null && _idleProcessThread.ThreadState == ThreadState.Stopped)
                        _idleProcessThread = null;

                    if (_idleProcessThread == null)
                    {
                        _idleProcessThread = new Thread(ProcessIdleServerEvents) {IsBackground = true};
                        _idleProcessThread.Start();
                    }
                   
                }

                //Thread.Sleep(5000);
            }
        }

        internal void PauseIdling()
        {
            if (_idleState != IdleState.On)
                return;
            StopIdling(true);
            _idleState = IdleState.Paused;

            if (OnIdlePaused != null)
                OnIdlePaused(SelectedFolder, new IdleEventArgs
                {
                    Client = SelectedFolder.Client,
                    Folder = SelectedFolder
                });
        }

        internal void StopIdling(bool pausing = false)
        {
            if (_idleState == IdleState.Off)
                return;

            _counter++;
            const string text = "DONE" + "\r\n";
            byte[] bytes = Encoding.UTF8.GetBytes(text.ToCharArray());
            if (IsDebug)
                Debug.WriteLine(text);

            _ioStream.Write(bytes, 0, bytes.Length);

            if (_maintainIdleConnectionTimer != null)
            {
                // we could call Stop() alternatively (according to MSDN)
                _maintainIdleConnectionTimer.Enabled = false;
            }

            if (!pausing && _maintainIdleConnectionTimer != null)
            {
                // Close() is like Dispose()... but closing a connection sounds better
                _maintainIdleConnectionTimer.Close();
                _maintainIdleConnectionTimer = null;
            }

            if (_idleLoopThread != null)
            {
                _idleLoopThread.Join();
                _idleLoopThread = null;
            }

            if (!pausing && OnIdleStopped != null)
                OnIdleStopped(SelectedFolder, new IdleEventArgs
                {
                    Client = SelectedFolder.Client,
                    Folder = SelectedFolder
                });
        }

        public event EventHandler<IdleEventArgs> OnNewMessagesArrived;
        public event EventHandler<IdleEventArgs> OnIdleStarted;
        public event EventHandler<IdleEventArgs> OnIdlePaused;
        public event EventHandler<IdleEventArgs> OnIdleStopped;

        #endregion

#endif
    }
}