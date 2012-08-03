using System;
using System.Collections;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
namespace ImapX
{
    public class ImapBase
    {
        protected bool isDebug;
        protected int _commandCount;
        protected TcpClient _imapServer;
        protected NetworkStream _imapStream;
        protected SslStream _imapSslStream;
        protected StreamReader _imapStreamReader;
        protected StreamReader _imapSslStreamReader;
        protected string _imapHost = "localhost";
        protected int _imapPort = 146;
        protected bool _useSSL;
        internal bool _isConnected;
        internal bool _isLogged;
        protected string _userLogin;
        protected string _userPassword;
        internal string _selectedFolder;

        public bool IsDebug
        {
            get
            {
                return this.isDebug;
            }
            set
            {
                this.isDebug = value;
            }
        }

        public string Host
        {
            get
            {
                return this._imapHost;
            }
            set
            {
                this._imapHost = value;
            }
        }

        public int Port
        {
            get
            {
                return this._imapPort;
            }
            set
            {
                this._imapPort = value;
            }
        }

        public bool UseSsl
        {
            get
            {
                return this._useSSL;
            }
            set
            {
                this._useSSL = value;
            }
        }

        public string UserLogin
        {
            get
            {
                return this._userLogin;
            }
            set
            {
                this._userLogin = value;
            }
        }

        public string UserPassword
        {
            get
            {
                return this._userPassword;
            }
            set
            {
                this._userPassword = value;
            }
        }

        public bool LogIn()
        {
            return this.LogIn(this._userLogin, this._userPassword);
        }

        public bool LogIn(string login, string password)
        {
            if (!this._isConnected)
            {
                throw new ImapException("Not Connection");
            }
            if (string.IsNullOrEmpty(login))
            {
                throw new ImapException("Login is null or empty");
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ImapException("Password is null or empty");
            }
            var arrayList = new ArrayList();
            string command = string.Concat(new[]
			{
				"LOGIN \"", 
				login, 
				"\" \"", 
				password, 
				"\"\r\n"
			});
            if (this.SendAndReceive(command, ref arrayList))
            {
                this._isLogged = true;
                this._userLogin = login;
                this._userPassword = password;
                return true;
            }
            return false;
        }

        public bool LogOut()
        {
            if (!this._isLogged)
            {
                return true;
            }
            var arrayList = new ArrayList();
            const string command = "LOGOUT\r\n";
            if (!this.SendAndReceive(command, ref arrayList))
            {
                return false;
            }
            this._isLogged = false;
            return true;
        }

        public bool Connect()
        {
            return this.Connect(this._imapHost, this._imapPort, this._useSSL);
        }

        public bool Connect(string sHost, int nPort, bool useSSL)
        {
            this._useSSL = useSSL;
            bool result = true;
            this._commandCount = 0;
            try
            {
                this._imapServer = new TcpClient(sHost, nPort);
                this._imapStream = this._imapServer.GetStream();
                this._imapStreamReader = new StreamReader(this._imapServer.GetStream());
                if (useSSL)
                {
                    this._imapSslStream = new SslStream(this._imapServer.GetStream(), false, this.ValidateServerCertificate, null);
                    try
                    {
                        this._imapSslStream.AuthenticateAsClient(sHost);
                    }
                    catch (AuthenticationException)
                    {
                        result = false;
                        this._imapServer.Close();
                    }
                    this._imapSslStreamReader = new StreamReader(this._imapSslStream);
                }
                string text = useSSL ? this._imapSslStreamReader.ReadLine() : this._imapStreamReader.ReadLine();
                if (text != null && text.StartsWith("* OK"))
                {
                    this.Capability();
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                throw new ImapException("dont connect");
            }
            this._imapHost = sHost;
            this._imapPort = nPort;
            this._isConnected = true;
            return result;
        }

        public bool Disconnect()
        {
            this._commandCount = 0;
            if (this._isConnected)
            {
                if (this._imapSslStream != null)
                {
                    this._imapSslStream.Close();
                }
                if (this._imapStream != null)
                {
                    this._imapStream.Close();
                }
                if (this._imapServer != null)
                {
                    this._imapServer.Close();
                }
            }
            this._isConnected = false;
            return true;
        }

        public bool Capability()
        {
            var arrayList = new ArrayList();
            const string command = "CAPABILITY\r\n";
            return this.SendAndReceive(command, ref arrayList);
        }

        public bool SendData(string data)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(data.ToCharArray());
            try
            {
                if (this._useSSL)
                {
                    this._imapSslStream.Write(bytes, 0, data.Length);
                }
                else
                {
                    this._imapStream.Write(bytes, 0, data.Length);
                }
                bool flag = true;
                while (flag)
                {
                    string value = this._useSSL ? this._imapSslStreamReader.ReadLine() : this._imapStreamReader.ReadLine();
                    if (this.isDebug)
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

        public MessageCollection SearchUtf8Data(byte[] data)
        {
            MessageCollection result;
            try
            {
                if (this._useSSL)
                {
                    this._imapSslStream.Write(data, 0, data.Length);
                }
                else
                {
                    this._imapStream.Write(data, 0, data.Length);
                }
                bool flag = true;
                var messageCollection = new MessageCollection();
                while (flag)
                {
                    string text = this._useSSL ? this._imapSslStreamReader.ReadLine() : this._imapStreamReader.ReadLine();
                    if (this.isDebug)
                    {
                        Console.WriteLine(text);
                    }
                	if (text != null)
                	{
                		string[] array = text.Split(new[]
                		                            	{
                		                            		' '
                		                            	});
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
                	}
                	flag = false;
                }
                result = messageCollection;
            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }

        public void SendCommand(string command)
        {
            this._commandCount++;
            string text = string.Concat(new object[]
			{
				"IMAP00", 
				this._commandCount, 
				" ", 
				command
			});
            byte[] bytes = Encoding.ASCII.GetBytes(text.ToCharArray());
            if (this.isDebug)
            {
                Console.WriteLine(text);
            }
            try
            {
                if (this._useSSL)
                {
                    this._imapSslStream.Write(bytes, 0, bytes.Length);
                }
                else
                {
                    this._imapStream.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception)
            {
            }
        }

        public bool SendAndReceive(string command, ref ArrayList sResultArray)
        {
            bool result = true;
            this._commandCount++;
            string text = string.Concat(new object[]
			{
				"IMAP00", 
				this._commandCount, 
				" ", 
				command
			});
            byte[] bytes = Encoding.ASCII.GetBytes(text.ToCharArray());
            if (this.isDebug)
            {
                Console.WriteLine(text);
            }
            try
            {
                if (this._useSSL)
                {
                    this._imapSslStream.Write(bytes, 0, bytes.Length);
                }
                else
                {
                    this._imapStream.Write(bytes, 0, bytes.Length);
                }
                bool flag = true;
                while (flag)
                {
                    string text2 = this._useSSL ? this._imapSslStreamReader.ReadLine() : this._imapStreamReader.ReadLine();
                    if (this.isDebug)
                    {
                        Console.WriteLine(text2);
                    }
                	if (text2 != null)
                	{
                		sResultArray.Add(text2);
                		if (text2.StartsWith("IMAP00" + this._commandCount + " OK"))
                		{
                			flag = false;
                		}
                		else
                		{
                			if (text2.StartsWith("IMAP00" + this._commandCount + " NO"))
                			{
                				flag = false;
                				result = false;
                			}
                			else
                			{
                				if (text2.StartsWith("IMAP00" + this._commandCount + " BAD"))
                				{
                					flag = false;
                					result = false;
                				}
                				else
                				{
                					if (text2.StartsWith("+ "))
                					{
                						flag = false;
                					}
                				}
                			}
                		}
                	}
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }
            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);
            return false;
        }
    }
}
