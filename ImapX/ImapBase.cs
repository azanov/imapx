using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using ImapX.Constants;
using ImapX.Exceptions;
using System.Collections;
using ImapX.Parsing;
#if WINDOWS_PHONE
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
        private static readonly Regex NewLineRex = new Regex(@"\r\n");

        private TcpClient _client;
        private bool _connected;
        private long _counter;

        private string _host;
        private Stream _ioStream;
        private int _port = DefaultImapPort;

        private SslProtocols _sslProtocol = SslProtocols.None;
        private StreamReader _streamReader;
        private bool _validateServerCertificate = true;

        /// <summary>
        ///     Gets whether the client is authenticated
        /// </summary>
        public bool IsAuthenticated { get; internal set; }

        /// <summary>
        ///     Gets whether the client is connected to the server
        /// </summary>
        public bool IsConnected
        {
            get { return _connected; }
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
                if (_connected)
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
                if (_connected)
                    throw new InvalidStateException(
                        "The port cannot be changed after the connection has been established. Please disconnect first.");
                _port = value;
            }
        }

        /// <summary>
        ///     The SSL protocol used. <code>SslProtocols.None</code> by default
        /// </summary>
        /// <exception cref="Exceptions.InvalidStateException">On set, if the client is connected.</exception>
        public SslProtocols SslProtocol
        {
            get { return _sslProtocol; }
            set
            {
                if (_connected)
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
                if (_connected)
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

            if (_connected)
                throw new InvalidStateException("The client is already connected. Please disconnect first.");

            try
            {
#if !WINDOWS_PHONE
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

                if (result != null && result.StartsWith(ResponseType.ServerOk))
                {
                    _connected = true;
                    Capability();
                    return true;
                }
                else if (result != null && result.StartsWith(ResponseType.ServerPreAuth))
                {
                    _connected = true;
                    IsAuthenticated = true;
                    Capability();
                    return true;
                }
                else
                    return false;
            }
            catch(Exception ex)
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
            CleanUp();
        }

        /// <summary>
        ///     Disconnects from server and disposes the objects
        /// </summary>
        private void CleanUp()
        {
            _counter = 0;

            if (!_connected)
                return;

            if (_streamReader != null)
                _streamReader.Dispose();

            if (_ioStream != null)
                _ioStream.Dispose();

            if (_client != null)
#if !WINDOWS_PHONE
                _client.Close();
#else
                _client.Dispose();
#endif
            _connected = false;
        }


#if !WINDOWS_PHONE

        /// <summary>
        ///     The certificate validation callback
        /// </summary>
        private bool CertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return sslPolicyErrors == SslPolicyErrors.None || !_validateServerCertificate;
        }

#endif

        internal void Capability()
        {
            IList<string> data = new List<string>();
            if (SendAndReceive(ImapCommands.Capability, ref data) && data.Count > 0)
                Capabilities = new Capability(data[0]);
        }

        public bool SendAndReceive(string command, ref IList<string> data, CommandProcessor processor = null,
            Encoding encoding = null)
        {
            if (_client == null || !_client.Connected)
                throw new SocketException((int)SocketError.NotConnected);

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

            while (true)
            {
                string tmp = reader.ReadLine();

                if (tmp == null)
                {
                    return false;
                }

                if (IsDebug)
                    Debug.WriteLine(tmp);

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
                    return true;

                if (tmp.StartsWith(string.Format(tmpl, _counter, ResponseType.PreAuth)))
                    return true;

                if (tmp.StartsWith(string.Format(tmpl, _counter, ResponseType.No)) ||
                    tmp.StartsWith(string.Format(tmpl, _counter, ResponseType.Bad)))
                {
                    var serverAlertMatch = Expressions.ServerAlertRex.Match(tmp);
                    if(serverAlertMatch.Success && tmp.Contains("IMAP") && tmp.Contains("abled"))
                        throw new ServerAlertException(serverAlertMatch.Groups[1].Value);
                    return false;
                }
            }
        }

        #region Obsolete

        [Obsolete("SendCommand is obsolete", true)]
        public void SendCommand(string command)
        {
        }

        [Obsolete("SendData", true)]
        public bool SendData(string data)
        {
            throw new NotImplementedException();
        }

        [Obsolete("SendAndReceiveMessage", true)]
        public bool SendAndReceiveMessage(string command, ref List<string> data, Message msg)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}