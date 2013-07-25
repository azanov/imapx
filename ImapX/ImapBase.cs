﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using ImapX.Exceptions;

namespace ImapX
{
    public class ImapBase
    {
        public const int DEFAULT_IMAP_PORT = 143;
        public const int DEFAULT_IMAP_SSL_PORT = 993;
        internal string SelectedFolder;

        private TcpClient _client;
        private bool _connected;
        private long _counter;

        private string _host;
        private Stream _ioStream;
        private bool _isDebug;
        private int _port = DEFAULT_IMAP_PORT;

        private SslProtocols _sslProtocol = SslProtocols.None;
        private StreamReader _streamReader;
        private bool _validateServerCertificate = true;

        /// <summary>
        ///     Gets whether the client is authenticated
        /// </summary>
        public bool IsAuthenticated { get; internal set; }

        [Obsolete("IsLogin is obsolete, please use IsAuthenticated instead", true)]
        public bool IsLogged
        {
            get { return IsAuthenticated; }
            internal set { IsAuthenticated = value; }
        }

        /// <summary>
        ///     Gets whether the client is connected to the server
        /// </summary>
        public bool IsConnected
        {
            get { return _connected; }
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

        public bool IsDebug
        {
            get { return _isDebug; }
            set { _isDebug = value; }
        }

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
        ///     The server capabilities
        /// </summary>
        public Capability Capabilities { get; private set; }


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
        public bool Connect(string host, bool useSsl = false, bool validateServerCertificate = false)
        {
            return Connect(host,
                useSsl ? DEFAULT_IMAP_SSL_PORT : DEFAULT_IMAP_PORT,
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
        public bool Connect(string host, int port, bool useSsl = false, bool validateServerCertificate = false)
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
            bool validateServerCertificate = false)
        {
            _host = host;
            _port = port;
            _sslProtocol = sslProtocol;
            _validateServerCertificate = validateServerCertificate;

            if (_connected)
                throw new InvalidStateException("The client is already connected. Please disconnect first.");

            try
            {
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

                string result = _streamReader.ReadLine();

                if (result != null && result.StartsWith(ResponseType.SERVER_OK))
                {
                    _connected = true;
                    Capability();
                    return true;
                }
                else if (result != null && result.StartsWith(ResponseType.SERVER_PREAUTH))
                {
                    _connected = true;
                    IsAuthenticated = true;
                    Capability();
                    return true;
                }
                else
                    return false;
            }
            catch
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
                _streamReader.Close();

            if (_ioStream != null)
                _ioStream.Close();

            if (_client != null)
                _client.Close();

            _connected = false;
        }

        internal void Capability()
        {
            IList<string> data = new List<string>();
            if (SendAndReceive(ImapCommands.CAPABILITY, ref data) && data.Count > 0)
                Capabilities = new Capability(data[0]);
        }

        public bool SendData(string data)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(data.ToCharArray());
            try
            {
                _ioStream.Write(bytes, 0, data.Length);

                bool flag = true;
                while (flag)
                {
                    string value = _streamReader.ReadLine();
                    if (_isDebug)
                    {
                        Console.WriteLine(value);
                    }
                    flag = false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public void SendCommand(string command)
        {
            _counter++;
            string text = string.Concat(new object[]
            {
                "IMAP00",
                _counter,
                " ",
                command
            });
            byte[] bytes = Encoding.ASCII.GetBytes(text.ToCharArray());
            if (_isDebug)
            {
                Console.WriteLine(text);
            }
            try
            {
                _ioStream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception)
            {
            }
        }

        public bool SendAndReceive(string command, ref IList<string> data)
        {
            const string tmpl = "IMAPX{0} {1}";
            _counter++;

            var parts = new Queue<string>(new Regex(@"\r\n").Split(command).Where(_ => !string.IsNullOrEmpty(_)));

            string text = string.Format(tmpl, _counter, parts.Dequeue().Trim()) + "\r\n";
            byte[] bytes = Encoding.UTF8.GetBytes(text.ToCharArray());

            try
            {
                _ioStream.Write(bytes, 0, bytes.Length);

                while (true)
                {
                    string tmp = _streamReader.ReadLine();

                    //if (string.IsNullOrEmpty(tmp))
                    //    return false;

                    data.Add(tmp);

                    if (tmp.StartsWith("+ ") && parts.Count > 0)
                    {
                        bytes = Encoding.UTF8.GetBytes((parts.Dequeue().Trim() + "\r\n").ToCharArray());
                        _ioStream.Write(bytes, 0, bytes.Length);
                        continue;
                    }

                    if (tmp.StartsWith(string.Format(tmpl, _counter, ResponseType.OK)))
                        return true;

                    if (tmp.StartsWith(string.Format(tmpl, _counter, ResponseType.PREAUTH)))
                        return true;

                    if (tmp.StartsWith(string.Format(tmpl, _counter, ResponseType.NO)) || tmp.StartsWith(string.Format(tmpl, _counter, ResponseType.BAD)))
                        return false;

                }

            }
            catch (AuthenticationException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return false;
        }

        /// <summary>
        ///     The certificate validation callback
        /// </summary>
        private bool CertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return sslPolicyErrors == SslPolicyErrors.None || !_validateServerCertificate;
        }
    }
}