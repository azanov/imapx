using ImapX.Commands;
using ImapX.Enums;
using ImapX.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ImapX
{
    public class ImapBase
    {
        public const int DefaultImapPort = 143;
        public const int DefaultImapSslPort = 993;

        protected ImapBaseState _state = ImapBaseState.Idle;

        protected string _host;
        protected int _port;
        protected ImapConnectionSecurity _connectionSecurity = ImapConnectionSecurity.None;
        protected bool _validateServerCertificate = true;

        protected TcpClient _client;
        protected Stream _ioStream;
        protected ImapParser _io;

        protected long _cmdCount = -1;
        protected string[] _supportedLanguages;

        protected List<ImapCommand> _commandQueue;

        public object Lock = 0;

        /// <summary>
        ///     Gets whether the client is connected to the server
        /// </summary>
        public bool IsConnected
        {
            get { return _client != null && _client.Connected; }
        }

        /// <summary>
        ///     Basic client behavior settings like folder browse mode and message download mode
        /// </summary>
        public ClientBehavior Behavior { get; protected set; }

        /// <summary>
        ///     Gets whether the client is authenticated
        /// </summary>
        public bool IsAuthenticated { get; internal set; }

        /// <summary>
        ///     The server capabilities
        /// </summary>
        public Capability Capabilities { get; internal set; }

        public bool IsDebug { get; set; }

        public ImapEncodingMode EncodingMode { get; internal set; }

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

        /// <summary>
        ///     The SSL protocol used. <code>SslProtocols.None</code> by default
        /// </summary>
        /// <exception cref="Exceptions.InvalidStateException">On set, if the client is connected.</exception>
        public ImapConnectionSecurity ConnectionSecurity
        {
            get { return _connectionSecurity; }
            set
            {
                if (IsConnected)
                    throw new InvalidStateException(
                        "The SSL protocol cannot be changed after the connection has been established. Please disconnect first.");
                _connectionSecurity = value;
            }
        }


        /// <summary>
        ///     Get or set if SSL should be used. <code>false</code> by default.  If set to <code>true</code>, the
        ///     <code>SslProtocol</code> will be set to <code>SslProtocols.Default</code>
        /// </summary>
        [Obsolete("Use the ConnectionSecurity property instead", true)]
        public bool UseSsl
        {
            get { return _connectionSecurity != ImapConnectionSecurity.None; }
            set { _connectionSecurity = value ? ImapConnectionSecurity.SSL : ImapConnectionSecurity.None; }
        }


        public string[] SupportedLanguages
        {
            get
            {
                if (_supportedLanguages == null)
                    _supportedLanguages = GetSupportedLanguages();
                return _supportedLanguages;
            }
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

        public ImapBase()
        {
            _commandQueue = new List<ImapCommand>();
            Capabilities = new Capability();
        }

        /// <summary>
        ///     The certificate validation callback
        /// </summary>
        protected bool CertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return sslPolicyErrors == SslPolicyErrors.None || !_validateServerCertificate;
        }

        /// <summary>
        ///     Connect using set values.
        /// </summary>
        public SafeResult Connect()
        {
            return Connect(_host, _port, _connectionSecurity, _validateServerCertificate);
        }

        /// <summary>
        ///     Connect using set values.
        /// </summary>
        public Task<SafeResult> ConnectAsync()
        {
            return ConnectAsync(_host, _port, _connectionSecurity, _validateServerCertificate);
        }


        /// <summary>
        /// Connects to an IMAP server on port 993 using SSL
        /// </summary>
        /// <param name="host">Server address</param>
        public SafeResult Connect(string host)
        {
            return Connect(host, DefaultImapSslPort, ImapConnectionSecurity.SSL, ValidateServerCertificate);
        }

        /// <summary>
        /// Connects to an IMAP server on port 993 using SSL
        /// </summary>
        /// <param name="host">Server address</param>
        public Task<SafeResult> ConnectAsync(string host)
        {
            return ConnectAsync(host, DefaultImapSslPort, ImapConnectionSecurity.SSL, ValidateServerCertificate);
        }

        /// <summary>
        ///     Connects to an IMAP server on the specified port
        /// </summary>
        /// <param name="host">Server address</param>
        /// <param name="port">Server port</param>
        /// <param name="connectionSecuity">SSL protocol to use, <code>SslProtocols.None</code> by default</param>
        /// <param name="validateServerCertificate">Defines whether the server certificate should be validated when SSL is used</param>
        public SafeResult Connect(string host, int port, ImapConnectionSecurity connectionSecuity = ImapConnectionSecurity.None,
            bool validateServerCertificate = true)
        {
            _host = host;
            _port = port;
            _connectionSecurity = connectionSecuity;
            _validateServerCertificate = validateServerCertificate;
            {
                if (IsConnected)
                    return new SafeResult(exception: new InvalidStateException("The client is already connected. Please disconnect first."));

                try
                {
                    _client = new TcpClient(_host, _port);
                    _ioStream = _client.GetStream();

                    if (_connectionSecurity == ImapConnectionSecurity.SSL)
                    {
                        _ioStream = new SslStream(_ioStream, false, CertificateValidationCallback, null);
                        (_ioStream as SslStream).AuthenticateAsClient(_host, null, SslProtocols.Tls12, false);
                    }

                    _io = new ImapParser(this, _ioStream);

                    RunCommand(new ConnectCommand(this, GetNextCommandId()));

                    if (!Capabilities.Loaded)
                        Capability();

                    if (
                        connectionSecuity == ImapConnectionSecurity.StartTLS ||
                        (Capabilities.LoginDisabled && connectionSecuity == ImapConnectionSecurity.StartTLSIfLoginDisabled))
                    {
                        StartTLS();

                        _ioStream = new SslStream(_ioStream, false, CertificateValidationCallback, null);
                        (_ioStream as SslStream).AuthenticateAsClient(_host, null, SslProtocols.Tls12, false);

                        _io.BindStream(_ioStream);

                        Capability();
                    }

                    return SafeResult.True;
                }
                catch (Exception ex)
                {
                    Disconnect();
                    return new SafeResult(exception: ex);
                }
                finally
                {

                }
            }
        }

        /// <summary>
        ///     Connects to an IMAP server on the specified port
        /// </summary>
        /// <param name="host">Server address</param>
        /// <param name="port">Server port</param>
        /// <param name="connectionSecuity">SSL protocol to use, <code>SslProtocols.None</code> by default</param>
        /// <param name="validateServerCertificate">Defines whether the server certificate should be validated when SSL is used</param>
        public Task<SafeResult> ConnectAsync(string host, int port, ImapConnectionSecurity connectionSecuity = ImapConnectionSecurity.None,
            bool validateServerCertificate = true)
        {
            return Task.Run(() => Connect(host, port, connectionSecuity, validateServerCertificate));
        }

        public void Disconnect()
        {
            try
            {
                if (_client != null)
                    _client.Close();

                _cmdCount = -1;

                if (_commandQueue.Count > 0)
                    _commandQueue.Clear();

                if (_ioStream != null)
                    _ioStream.Dispose();

                _client = null;
                _ioStream = null;
            }
            catch { }
        }

        public void Capability()
        {
            var cmd = RunCommand(new CapabilityCommand(this, GetNextCommandId()));

            if (cmd.State != CommandState.Ok)
                throw new InvalidOperationException();
        }

        public Task CapabilityAsync()
        {
            return Task.Run(() => Capability());
        }

        protected void StartTLS()
        {
            if (!Capabilities.StartTLS)
                throw new UnsupportedCapabilityException();

            var cmd = RunCommand(new StartTLSCommand(this, GetNextCommandId()));

            if (cmd.State != CommandState.Ok)
                throw new CommandFailedException("Failed to negotiate STARTTLS. Details: {0}", cmd.StateDetails);
        }

        public SafeResult Enable(string capability)
        {
            if (!Capabilities.Enable)
                return new SafeResult(exception: new UnsupportedCapabilityException());

            var cmd = RunCommand(new EnableCommand(this, GetNextCommandId(), capability));

            if (cmd.State != CommandState.Ok)
            {
                EncodingMode = ImapEncodingMode.ImapUTF7;
                return new SafeResult(exception: new CommandFailedException("Failed to enable the capability '{0}'. Details: {1}", capability, cmd.StateDetails));
            }

            EncodingMode = ImapEncodingMode.UTF8;

            return true;
        }

        public Task<SafeResult> EnableAsync(string capability)
        {
            return Task.Run(() => Enable(capability));
        }

        public virtual void ScheduleCommand(ImapCommand cmd)
        {
            _commandQueue.Add(cmd);
        }

        public ImapCommand RunCommand(ImapCommand cmd)
        {
            ScheduleCommand(cmd);
            while (ProcessNextCommand() != cmd.Id) ;
            return cmd;
        }

        public ImapCommand<T> RunCommand<T>(ImapCommand<T> cmd)
        {
            ScheduleCommand(cmd);
            while (ProcessNextCommand() != cmd.Id) ;
            return cmd;
        }

        public ClientImapCommand RunCommand(ClientImapCommand cmd)
        {
            ScheduleCommand(cmd);
            while (ProcessNextCommand() != cmd.Id) ;
            return cmd;
        }

        public ClientImapCommand<T> RunCommand<T>(ClientImapCommand<T> cmd)
        {
            ScheduleCommand(cmd);
            while (ProcessNextCommand() != cmd.Id) ;
            return cmd;
        }

        internal long ProcessNextCommand()
        {
            if (_state == ImapBaseState.CommandInProgress)
                return -1;

            lock (Lock)
            {

                _state = ImapBaseState.CommandInProgress;

                var cmd = _commandQueue.FirstOrDefault();
                if (cmd == null)
                    return -1;

                cmd.State = CommandState.Active;

                string commandPart;

                try
                {
                    while ((commandPart = cmd.GetNextPart()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(commandPart))
                            _io.Write(Encoding.UTF8.GetBytes(commandPart));

                        while (true)
                        {
                            var token = _io.ReadToken();

                            if (token.Type == TokenType.Eos)
                                break;
                            else if (token.Type == TokenType.Atom)
                            {
                                if (token.Value == "+")
                                {
                                    cmd.Continue(_io.ReadLine());
                                    break;
                                }
                                else if (cmd.MatchesTag(token.Value))
                                {
                                    cmd.HandleTaggedResponse(_io);
                                    break;
                                }
                            }
                            else if (token.Type == TokenType.Asterisk)
                            {
                                cmd.HandleUntaggedResponse(_io);
                                if (cmd.BreakAfterUntagged)
                                    break;
                            }

                        }
                    }
                }
                catch (BadCommandException)
                {
                    cmd.State = CommandState.Bad;
                }
                catch (Exception ex)
                {
                    cmd.State = CommandState.CriticalFailure;
                    cmd.StateDetails = ex.ToString();
                }

                _state = ImapBaseState.Idle;
                _commandQueue.RemoveAt(0);

                if (cmd.State == CommandState.Ok)
                    cmd.OnCommandComplete();

                return cmd.Id;

            }
        }

        internal void HandleCapabilityResponse(ImapParser io)
        {
            ImapToken token;
            Capabilities = new Capability();

            while ((token = io.PeekToken()).Type == TokenType.Atom)
            {
                io.ReadToken();
                Capabilities.Add(token.Value);
            }
        }

        internal void HandlePreAuthenticate()
        {
            IsAuthenticated = true;
        }

        internal void HandleAlert(string message)
        {
            // TODO
        }

        internal string[] GetSupportedLanguages()
        {
            if (!Capabilities.Language)
                return new[] { "i-default" };

            var cmd = RunCommand(new LanguageCommand(this, GetNextCommandId()));

            if (cmd.State != CommandState.Ok)
                throw new OperationFailedException("Failed to get the supported languages. Details: {0}", cmd.StateDetails);

            return cmd.Response;
        }

        public Task<string[]> GetSupportedLanguagesAsync()
        {
            return Task.Run(() => {
                _supportedLanguages = GetSupportedLanguages();
                return _supportedLanguages;
            });
        }

        public SafeResult SetLanguage(string language)
        {
            if (string.IsNullOrWhiteSpace(language))
                return new SafeResult(exception: new ArgumentException("A valid language is required", nameof(language)));

            if (Capabilities.Language)
            {

                var cmd = RunCommand(new LanguageCommand(this, GetNextCommandId(), language));

                return new SafeResult(cmd.State == CommandState.Ok, cmd.State != CommandState.Ok
                    ? new OperationFailedException("Failed to get the supported languages. Details: {0}", cmd.StateDetails)
                    : null);
            }
            else if (language == "i-default")
                return true;

            return new SafeResult(exception: new NotSupportedException("The server does not support any languages except i-default"));
        }

        public long GetNextCommandId()
        {
            return _cmdCount++;
        }
    }
}
