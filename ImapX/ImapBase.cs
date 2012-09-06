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
        protected int CommandCount;
        protected TcpClient ImapServer;
        protected NetworkStream ImapStream;
        protected SslStream ImapSslStream;
        
        protected StreamReader ImapStreamReader;
        protected StreamReader ImapSslStreamReader;
        protected string ImapHost = "localhost";
        protected int ImapPort = 146;
        protected bool UseSSL;
        protected SslProtocols SSLProtocols;
        internal bool IsConnected;
        internal bool IsLogged;
        protected string _userLogin;
        protected string _userPassword;
        internal string SelectedFolder;

        public bool IsDebug
        {
            get
            {
                return isDebug;
            }
            set
            {
                isDebug = value;
            }
        }

        public string Host
        {
            get
            {
                return ImapHost;
            }
            set
            {
                ImapHost = value;
            }
        }

        public int Port
        {
            get
            {
                return ImapPort;
            }
            set
            {
                ImapPort = value;
            }
        }

        public bool UseSsl
        {
            get
            {
                return UseSSL;
            }
            set
            {
                UseSSL = value;
            }
        }

        public string UserLogin
        {
            get
            {
                return _userLogin;
            }
            set
            {
                _userLogin = value;
            }
        }

        public string UserPassword
        {
            get
            {
                return _userPassword;
            }
            set
            {
                _userPassword = value;
            }
        }

        public bool LogIn()
        {
            return LogIn(_userLogin, _userPassword);
        }

        public bool LogIn(string login, string password)
        {
            if (!IsConnected)
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
            if (SendAndReceive(command, ref arrayList))
            {
                IsLogged = true;
                _userLogin = login;
                _userPassword = password;
                return true;
            }
            return false;
        }

        public bool LogOut()
        {
            if (!IsLogged)
            {
                return true;
            }
            var arrayList = new ArrayList();
            const string command = "LOGOUT\r\n";
            if (!SendAndReceive(command, ref arrayList))
            {
                return false;
            }
            IsLogged = false;
            return true;
        }

        public bool Connect()
        {
            return Connect(ImapHost, ImapPort, UseSSL);
        }

        public bool Connect(string sHost, int nPort, bool useSSL)
        {
            UseSSL = useSSL;
            bool result = true;
            CommandCount = 0;
            try
            {
                ImapServer = new TcpClient(sHost, nPort);
               
                ImapStream = ImapServer.GetStream();
                ImapStreamReader = new StreamReader(ImapServer.GetStream());
                if (useSSL)
                {
                    ImapSslStream = new SslStream(ImapServer.GetStream(), false, ValidateServerCertificate, null);
                    try
                    {
                        ImapSslStream.AuthenticateAsClient(sHost, null, SSLProtocols, false);
                    }
                    catch (AuthenticationException)
                    {
                        result = false;
                        ImapServer.Close();
                    }
                    ImapSslStreamReader = new StreamReader(ImapSslStream);
                }
                string text = useSSL ? ImapSslStreamReader.ReadLine() : ImapStreamReader.ReadLine();
                if (text != null && text.StartsWith("* OK"))
                {
                    Capability();
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
            ImapHost = sHost;
            ImapPort = nPort;
            IsConnected = true;
            return result;
        }

        public bool Disconnect()
        {
            CommandCount = 0;
            if (IsConnected)
            {
                if (ImapSslStream != null)
                {
                    ImapSslStream.Close();
                }
                if (ImapStream != null)
                {
                    ImapStream.Close();
                }
                if (ImapServer != null)
                {
                    ImapServer.Close();
                }
            }
            IsConnected = false;
            return true;
        }

        public bool Capability()
        {
            var arrayList = new ArrayList();
            const string command = "CAPABILITY\r\n";
            return SendAndReceive(command, ref arrayList);
        }

        public bool SendData(string data)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(data.ToCharArray());
            try
            {
                if (UseSSL)
                {
                    ImapSslStream.Write(bytes, 0, data.Length);
                }
                else
                {
                    ImapStream.Write(bytes, 0, data.Length);
                }
                bool flag = true;
                while (flag)
                {
                    string value = UseSSL ? ImapSslStreamReader.ReadLine() : ImapStreamReader.ReadLine();
                    if (isDebug)
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
                if (UseSSL)
                {
                    ImapSslStream.Write(data, 0, data.Length);
                }
                else
                {
                    ImapStream.Write(data, 0, data.Length);
                }
                bool flag = true;
                var messageCollection = new MessageCollection();
                while (flag)
                {
                    string text = UseSSL ? ImapSslStreamReader.ReadLine() : ImapStreamReader.ReadLine();
                    if (isDebug)
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
            CommandCount++;
            string text = string.Concat(new object[]
			{
				"IMAP00", 
				CommandCount, 
				" ", 
				command
			});
            byte[] bytes = Encoding.ASCII.GetBytes(text.ToCharArray());
            if (isDebug)
            {
                Console.WriteLine(text);
            }
            try
            {
                if (UseSSL)
                {
                    ImapSslStream.Write(bytes, 0, bytes.Length);
                }
                else
                {
                    ImapStream.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception)
            {
            }
        }

        public bool SendAndReceive(string command, ref ArrayList sResultArray)
        {
            bool result = true;
            CommandCount++;
            string text = string.Concat(new object[]
			{
				"IMAP00", 
				CommandCount, 
				" ", 
				command
			});
            byte[] bytes = Encoding.ASCII.GetBytes(text.ToCharArray());
            if (isDebug)
            {
                Console.WriteLine(text);
            }
            try
            {
                if (UseSSL)
                {
                    
                    ImapSslStream.Write(bytes, 0, bytes.Length);
                }
                else
                {
                    ImapStream.Write(bytes, 0, bytes.Length);
                }
                bool flag = true;
                while (flag)
                {
                    string text2 = UseSSL ? ImapSslStreamReader.ReadLine() : ImapStreamReader.ReadLine();
                    if (isDebug)
                    {
                        Console.WriteLine(text2);
                    }
                	if (text2 != null)
                	{
                		sResultArray.Add(text2);
                		if (text2.StartsWith("IMAP00" + CommandCount + " OK"))
                		{
                			flag = false;
                		}
                		else
                		{
                			if (text2.StartsWith("IMAP00" + CommandCount + " NO"))
                			{
                				flag = false;
                				result = false;
                			}
                			else
                			{
                				if (text2.StartsWith("IMAP00" + CommandCount + " BAD"))
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
