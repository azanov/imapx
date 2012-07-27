using EmailParser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using ImapX.Helpers;
using System.Linq;
using System.Text.RegularExpressions;
namespace ImapX
{
	public class Message
	{
		private int _msgUID;
		internal Imap _client;
		private string _subject;
		private List<MailAddress> _to;
		private List<MailAddress> _from;
		private List<Attachment> _attachments;
		private string _cc;
		private string _bcc;
		private DateTime _date;
		private Dictionary<string, string> _headers = new Dictionary<string, string>();
		private List<string> _flags = new List<string>();
		private string _mimeVersion;
		private string _organization;
		private string _priority;
		private string _received;
		private string _references;
		private string _replyTo;
		private string _xMailer;
		private string _messageId;
		private string _contentType;
		private string _contentTransferEncoding;
        private EmailParser.EmailParser _emailParser;
		private MessageContent _textBody;
		private MessageContent _htmlBody;
		private List<MessageContent> _bodyParts;
		public Dictionary<string, string> Headers
		{
			get
			{
				return this._headers;
			}
		}
		public List<string> Flags
		{
			get
			{
				return this._flags;
			}
		}
		public List<Attachment> Attachments
		{
			get
			{
				return this._attachments;
			}
			set
			{
				this._attachments = value;
			}
		}
		public MessageContent TextBody
		{
			get
			{
				return this._textBody;
			}
			set
			{
				this._textBody = value;
			}
		}
		public MessageContent HtmlBody
		{
			get
			{
				return this._htmlBody;
			}
			set
			{
				this._htmlBody = value;
			}
		}
		public string MimeVersion
		{
			get
			{
				return this._mimeVersion;
			}
			set
			{
				this._mimeVersion = value;
			}
		}
		public string Organization
		{
			get
			{
				return this._organization;
			}
			set
			{
				this._organization = value;
			}
		}
		public string Priority
		{
			get
			{
				return this._priority;
			}
			set
			{
				this._priority = value;
			}
		}
		public string Received
		{
			get
			{
				return this._received;
			}
			set
			{
				this._received = value;
			}
		}
		public string References
		{
			get
			{
				return this._references;
			}
			set
			{
				this._references = value;
			}
		}
		public string ReplyTo
		{
			get
			{
				return this._replyTo;
			}
			set
			{
				this._replyTo = value;
			}
		}
		public string XMailer
		{
			get
			{
				return this._xMailer;
			}
			set
			{
				this._xMailer = value;
			}
		}
		public string MessageId
		{
			get
			{
				return this._messageId;
			}
			set
			{
				this._messageId = value;
			}
		}
		public string ContentType
		{
			get
			{
				return this._contentType;
			}
			set
			{
				this._contentType = value;
			}
		}
		public string ContentTransferEncoding
		{
			get
			{
				return this._contentTransferEncoding;
			}
			set
			{
				this._contentTransferEncoding = value;
			}
		}
		public List<MessageContent> BodyParts
		{
			get
			{
				return this._bodyParts;
			}
			set
			{
				this._bodyParts = value;
			}
		}
		public List<MailAddress> To
		{
			get
			{
				return this._to;
			}
			set
			{
				this._to = value;
			}
		}
		public List<MailAddress> From
		{
			get
			{
				return this._from;
			}
			set
			{
				this._from = value;
			}
		}
		public string Cc
		{
			get
			{
				return this._cc;
			}
			set
			{
				this._cc = value;
			}
		}
		public string Bcc
		{
			get
			{
				return this._bcc;
			}
			set
			{
				this._bcc = value;
			}
		}
		public DateTime Date
		{
			get
			{
				return this._date;
			}
			set
			{
				this._date = value;
			}
		}
		public int MessageUid
		{
			get
			{
				return this._msgUID;
			}
			set
			{
				this._msgUID = value;
			}
		}
		public string Subject
		{
			get
			{
				return this._subject;
			}
			set
			{
				this._subject = value;
			}
		}
		public Message()
		{
			this._attachments = new List<Attachment>();
			this._bodyParts = new List<MessageContent>();
			this._to = new List<MailAddress>();
			this._from = new List<MailAddress>();
			this._textBody = new MessageContent();
			this._htmlBody = new MessageContent();
		}
		public bool SetFlag(string status)
		{
			bool result = true;
			ArrayList arrayList = new ArrayList();
			string command = string.Concat(new object[]
			{
				"STORE ", 
				this._msgUID, 
				" +FLAGS (", 
				status, 
				")\r\n"
			});
			try
			{
				result = this._client.SendAndReceive(command, ref arrayList);
			}
			catch
			{
				result = false;
			}
			this.getFlags();
			return result;
		}
		public void ProcessHeader()
		{
			this.getMessage("body[HEADER]", false);
		}

        /* Method not used

        /// <remarks>
        /// [27.07.2012] Fix by Pavel Azanov (coder13)
        ///              Some messages contain attachments that are not described in the ContentDisposition,
        ///              but in the ContentStream directly.
        /// </remarks>
		public void ProcessBody()
		{
			this.getMessage("BODY.PEEK[]", true);
			foreach (MessageContent current in this._bodyParts)
			{
				if (current.ContentType != null && current.ContentType.ToLower().Contains("text/plain"))
				{
					this._textBody = current;
					this._textBody.TextData = this._textBody.ContentStream;
				}
			}
			foreach (MessageContent current2 in this._bodyParts)
			{
				if (current2.ContentType != null && current2.ContentType.ToLower().Contains("text/html"))
				{
					this._htmlBody = current2;
					this._htmlBody.TextData = this._htmlBody.ContentStream;
				}
			}
			foreach (MessageContent current3 in this._bodyParts)
			{
                if (current3.ContentDisposition == null)
                    continue;
                else if (current3.ContentDisposition.ToLower().Contains("attachment"))
                {
                    Attachment attachment = new Attachment();
                    attachment.FileName = current3.ContentFilename;
                    attachment.FileType = current3.ContentType;
                    attachment.FileEncoding = current3.ContentTransferEncoding;
                    attachment.FileData = Convert.FromBase64String(current3.ContentStream);
                    this._attachments.Add(attachment);
                }
                else if (current3.ContentStream.ToLower().Contains("attachment")) // [27.07.2012]
                    this._attachments.Add(current3.ToAttachment());               // [27.07.2012]
			}

        }
        
        */

		public void ProcessFlags()
		{
			this.getFlags();
		}

        /// <remarks>
        /// [27.07.2012] Fix by Pavel Azanov (coder13)
        ///              Some messages contain attachments that are not described in the ContentDisposition,
        ///              but in the ContentStream directly.
        /// </remarks>
		public bool Process()
		{
			this.getFlags();
			this.getMessage("BODY.PEEK[]", true);
			foreach (MessageContent current in this._bodyParts)
			{
				if (current.ContentType != null && current.ContentType.ToLower().Contains("text/plain"))
				{
					this._textBody = current;
					this._textBody.TextData = this._textBody.ContentStream;
				}
			}
			foreach (MessageContent current2 in this._bodyParts)
			{
				if (current2.ContentType != null && current2.ContentType.ToLower().Contains("text/html"))
				{
					this._htmlBody = current2;
					this._htmlBody.TextData = this._htmlBody.ContentStream;
				}
			}
			foreach (MessageContent current3 in this._bodyParts)
			{
                if (current3.ContentDisposition == null)
                    continue;
                else if (current3.ContentDisposition.ToLower().Contains("attachment"))
                {
                    Attachment attachment = new Attachment();
                    attachment.FileName = current3.ContentFilename;
                    attachment.FileType = current3.ContentType;
                    attachment.FileEncoding = current3.ContentTransferEncoding;
                    attachment.FileData = Convert.FromBase64String(current3.ContentStream);
                    this._attachments.Add(attachment);
                }
                else if (current3.ContentStream.ToLower().Contains("attachment")) // [27.07.2012]
                    this._attachments.Add(current3.ToAttachment());               // [27.07.2012]
			}
			return true;
		}
		private bool getFlags()
		{
			bool flag = true;
			ArrayList arrayList = new ArrayList();
			string command = "FETCH " + this._msgUID + " (FLAGS)\r\n";
			try
			{
				flag = this._client.SendAndReceive(command, ref arrayList);
			}
			catch
			{
				flag = false;
			}
			if (flag)
			{
				string text = arrayList[0].ToString();
				this._flags.Clear();
				if (text.Contains("\\Answered"))
				{
					this._flags.Add("\\Answered");
				}
				if (text.Contains("\\Seen"))
				{
					this._flags.Add("\\Seen");
				}
				if (text.Contains("\\Recent"))
				{
					this._flags.Add("\\Recent");
				}
				if (text.Contains("\\Draft"))
				{
					this._flags.Add("\\Draft");
				}
				if (text.Contains("\\Deleted"))
				{
					this._flags.Add("\\Deleted");
				}
			}
			return flag;
		}

        public string GetDecodedBody(out bool isHtml)
        {
            var body = "";
            var transferEncoding = "";
            var contentType = "";
            var encoding = Encoding.Default;

            if (this._htmlBody != null && !string.IsNullOrWhiteSpace(this._htmlBody.ContentStream))
            {
                body = this._htmlBody.ContentStream;
                encoding = DecodeHelper.ParseContentType(this._htmlBody.ContentType, out contentType);
                transferEncoding = string.IsNullOrWhiteSpace(this._htmlBody.ContentTransferEncoding) ? _contentTransferEncoding : this._htmlBody.ContentTransferEncoding;
            }
            else if (this._textBody != null && !string.IsNullOrWhiteSpace(this._textBody.ContentStream) && !(this._textBody.ContentDisposition != null && this._textBody.ContentDisposition.ToLower().Contains("attachment")))
            {
                body = this._textBody.ContentStream;
                encoding = DecodeHelper.ParseContentType(this._textBody.ContentType, out contentType);
                transferEncoding = string.IsNullOrWhiteSpace(this._textBody.ContentTransferEncoding) ? _contentTransferEncoding : this._textBody.ContentTransferEncoding;
            }
            else if (_bodyParts.Count > 0)
            {
                var part =_bodyParts.Where(_ => !(_.ContentDisposition != null && _.ContentDisposition.ToLower().Contains("attachment"))).FirstOrDefault();
                if (part == null)
                {
                    isHtml = false;
                    return string.Empty;
                }
                body = part.ContentStream;
                encoding = DecodeHelper.ParseContentType(part.ContentType, out contentType);
                transferEncoding = string.IsNullOrWhiteSpace(part.ContentTransferEncoding) ? _contentTransferEncoding : part.ContentTransferEncoding;
            }

            if (encoding == null)
            {
                var rex = new Regex("^(.*):\\s(.*)[\r\n]?");
                var tmp = (new Regex("\r\n")).Split(body).ToArray();

                for (var i = 0; i < tmp.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(tmp[i])) continue;
                    var m = rex.Match(tmp[i]);
                    if (m == null || !m.Success)
                    {
                        //all headers passed
                        body = string.Join("\r\n", tmp.Skip(i));
                        break;
                    }
                    else if (m.Groups[1].Value.ToLower().Trim() == "content-transfer-encoding")
                        transferEncoding = m.Groups[2].Value;
                    else if (m.Groups[1].Value.ToLower().Trim() == "content-type")
                        encoding = DecodeHelper.ParseContentType(m.Groups[2].Value, out contentType);
                }

            }

            transferEncoding = (transferEncoding ?? string.Empty).ToLower().Trim();

            switch (transferEncoding)
            {
                case "base64":
                    body = DecodeHelper.DecodeBase64(body, encoding);
                    break;
                case "quoted-printable":
                    body = DecodeHelper.DecodeQuotedPrintable(body, encoding);
                    break;
                default:
                    break;
            }



            isHtml = contentType == "text/html";

            return body;
        }

        /// <remarks>
        /// [27.07.2012] Fix by Pavel Azanov (coder13)
        ///              Added automated decoding of mail subject
        /// </remarks>
		private void getMessage(string path, bool processBody)
		{
			ArrayList arrayList = new ArrayList();
			string command = string.Concat(new object[]
			{
				"FETCH ", 
				this._msgUID, 
				" ", 
				path, 
				"\r\n"
			});
			this._client.SendAndReceive(command, ref arrayList);
			try
			{
                this._emailParser = new EmailParser.EmailParser(arrayList.ToArray(typeof(string)) as string[]);
			}
			catch (Exception)
			{
			}
			this._emailParser.InitializeIndexes();
			this._emailParser.ParseHeaders();
			foreach (KeyValuePair<string, string> current in this._emailParser._headersCollection)
			{
				if (current.Key.ToLower().Trim().Equals("to"))
				{
					this._to = ParseHelper.AddressCollection(current.Value);
				}
				else
				{
					if (current.Key.ToLower().Trim().Equals("from"))
					{
						this._from = ParseHelper.AddressCollection(current.Value);
					}
					else
					{
						if (current.Key.ToLower().Trim().Equals("date"))
						{
							string s = current.Value.Trim();
							DateTime.TryParse(s, out this._date);
						}
						else
						{
							if (!ParseHelper.MessageProperty(current.Key, current.Value, "content-transfer-encoding", ref this._contentTransferEncoding) && !ParseHelper.MessageProperty(current.Key, current.Value, "content-type", ref this._contentType) && !ParseHelper.MessageProperty(current.Key, current.Value, "message-id", ref this._messageId) && !ParseHelper.MessageProperty(current.Key, current.Value, "mime-version", ref this._mimeVersion) && !ParseHelper.MessageProperty(current.Key, current.Value, "organization", ref this._organization) && !ParseHelper.MessageProperty(current.Key, current.Value, "priority", ref this._priority) && !ParseHelper.MessageProperty(current.Key, current.Value, "received", ref this._received) && !ParseHelper.MessageProperty(current.Key, current.Value, "references", ref this._references) && !ParseHelper.MessageProperty(current.Key, current.Value, "reply-to", ref this._replyTo) && !ParseHelper.MessageProperty(current.Key, current.Value, "x-mailer", ref this._xMailer) && !ParseHelper.MessageProperty(current.Key, current.Value, "cc", ref this._cc) && !ParseHelper.MessageProperty(current.Key, current.Value, "bcc", ref this._bcc))
							{
								ParseHelper.MessageProperty(current.Key, current.Value, "subject", ref this._subject);

                                this._subject = DecodeHelper.DecodeSubject(this._subject); // [27.07.2012]

							}
						}
					}
				}
			}
			this._headers = this._emailParser._headersCollection;
			if (processBody)
			{
				this._emailParser.ParseBody();
				foreach (BodyPart current2 in this._emailParser._parts)
				{
					MessageContent messageContent = new MessageContent();
					foreach (KeyValuePair<string, string> current3 in current2.Headers)
					{
						if (current3.Key.ToLower().Equals("content-type"))
						{
							messageContent.ContentType = current3.Value;
						}
						else
						{
							if (current3.Key.ToLower().Equals("content-disposition"))
							{
								messageContent.ContentDisposition = current3.Value;
								if (current3.Value.ToLower().Contains("filename="))
								{
									string contentFilename = current3.Value.Split(new string[]
									{
										"filename="
									}, StringSplitOptions.None)[1].Trim(new char[]
									{
										'"'
									});
									messageContent.ContentFilename = contentFilename;
									continue;
								}
							}
							if (current3.Key.ToLower().Equals("content-description"))
							{
								messageContent.ContentDescription = current3.Value;
							}
							else
							{
								if (current3.Key.ToLower().Equals("content-transfer-encoding"))
								{
									messageContent.ContentTransferEncoding = current3.Value;
								}
							}
						}
					}
					if (current2.Boundary == null)
					{
						messageContent.ContentType = this._contentType;
					}
					messageContent.ContentStream = this._emailParser.GetPart(current2);
					messageContent.PartHeaders = current2.Headers;
					messageContent.BoundaryName = current2.Boundary;
					this._bodyParts.Add(messageContent);
				}
			}
		}
		private StringBuilder getEml()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this._emailParser != null && this._emailParser._emailItems != null && this._emailParser._emailItems.Length > 0)
			{
				for (int i = 1; i <= this._emailParser._emailItems.Length - 2; i++)
				{
					if (i == this._emailParser._emailItems.Length - 2 && this._emailParser._emailItems[i].Length > 0)
					{
						stringBuilder.AppendFormat("{0}{1}", this._emailParser._emailItems[i].TrimEnd(new char[]
						{
							')'
						}), "\r\n");
					}
					else
					{
						stringBuilder.AppendFormat("{0}{1}", this._emailParser._emailItems[i], "\r\n");
					}
				}
			}
			else
			{
				stringBuilder.Append(this.messageBuilder());
			}
			return stringBuilder;
		}
		public string GetAsString()
		{
			return this.getEml().ToString();
		}
		public void SaveAsEmlToFile(string path, string filename)
		{
			if (!Directory.Exists(path))
			{
				throw new ImapException("Directory not Exists");
			}
			if (filename == null)
			{
				filename = Guid.NewGuid().ToString();
			}
			StringBuilder eml = this.getEml();
			using (FileStream fileStream = new FileStream(path + filename + ".eml", FileMode.Create, FileAccess.Write))
			{
				using (TextWriter textWriter = new StreamWriter(fileStream, Encoding.ASCII))
				{
					textWriter.Write(eml.ToString());
				}
			}
		}
		public string messageBuilder()
		{
			StringBuilder stringBuilder = new StringBuilder();
			DateTime now = DateTime.Now;
			now = DateTime.Now;
			stringBuilder.AppendFormat("Date: {0}{1}", now.ToString("dd-MM-yyyy hh:mm:ss", CultureInfo.CreateSpecificCulture("en-US")), "\r\n");
			if (this._from.Count > 0)
			{
				stringBuilder.Append("From: ");
				foreach (MailAddress current in this._from)
				{
					stringBuilder.AppendFormat("{0}, ", current.ToString());
				}
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
				stringBuilder.Append("\r\n");
			}
			if (this._to.Count > 0)
			{
				stringBuilder.Append("To: ");
				foreach (MailAddress current2 in this._to)
				{
					stringBuilder.AppendFormat("{0}, ", current2.ToString());
				}
				stringBuilder.Remove(stringBuilder.Length - 2, 1);
				stringBuilder.Append("\r\n");
			}
			if (!string.IsNullOrEmpty(this._subject))
			{
				stringBuilder.AppendFormat("Subject: {0}{1}", this._subject, "\r\n");
			}
			if (string.IsNullOrEmpty(this._messageId))
			{
				stringBuilder.AppendFormat("Message-Id: {0}@ImapX{1}", Guid.NewGuid(), "\r\n");
			}
			else
			{
				stringBuilder.AppendFormat("Message-Id: {0}{1}", this._messageId, "\r\n");
			}
			if (!string.IsNullOrEmpty(this.XMailer))
			{
				stringBuilder.AppendFormat("X-Mailer: {0}{1}", this._xMailer, "\r\n");
			}
			if (!string.IsNullOrEmpty(this._contentTransferEncoding))
			{
				stringBuilder.AppendFormat("Content-Transfer-Encoding: {0}{1}", this.ContentTransferEncoding, "\r\n");
			}
			if (string.IsNullOrEmpty(this.MimeVersion))
			{
				stringBuilder.AppendFormat("MIME-Version: 1.0{0}", "\r\n");
			}
			else
			{
				stringBuilder.AppendFormat("MIME-Version: {0}{1}", this._mimeVersion, "\r\n");
			}
			if (this._textBody == null && this._attachments.Count <= 0)
			{
				stringBuilder.AppendFormat("Content-Type: text/html; charset=utf-8{0}", "\r\n");
				stringBuilder.AppendFormat("Content-Transfer-Encoding: base64{0}", "\r\n");
				stringBuilder.Append("\r\n");
				if (this._htmlBody != null && this._htmlBody.TextData != null)
				{
					stringBuilder.AppendFormat("{0}{1}", Convert.ToBase64String(Encoding.UTF8.GetBytes(this._htmlBody.TextData)), Environment.NewLine);
				}
				else
				{
					stringBuilder.AppendFormat(Environment.NewLine, new object[0]);
				}
			}
			else
			{
				stringBuilder.Append("Content-Type: multipart/mixed;");
				stringBuilder.AppendFormat("boundary=\"part000\"{0}", "\r\n");
				stringBuilder.AppendFormat("--part000{0}", "\r\n");
				stringBuilder.AppendFormat("Content-Type: text/plain; charset=utf-8{0}", "\r\n");
				stringBuilder.AppendFormat("Content-Transfer-Encoding: base64{0}", "\r\n");
				stringBuilder.Append("\r\n");
				if (this._textBody != null && this._textBody.TextData != null)
				{
					stringBuilder.AppendFormat("{0}{1}", Convert.ToBase64String(Encoding.UTF8.GetBytes(this._textBody.TextData)), Environment.NewLine);
				}
				else
				{
					stringBuilder.AppendFormat(Environment.NewLine, new object[0]);
				}
				stringBuilder.AppendFormat("--part000{0}", "\r\n");
				stringBuilder.AppendFormat("Content-Type: text/html; charset=utf-8{0}", "\r\n");
				stringBuilder.AppendFormat("Content-Transfer-Encoding: base64{0}", "\r\n");
				stringBuilder.Append("\r\n");
				if (this._htmlBody != null && this._htmlBody.TextData != null)
				{
					stringBuilder.AppendFormat("{0}{1}", Convert.ToBase64String(Encoding.UTF8.GetBytes(this._htmlBody.TextData)), Environment.NewLine);
				}
				else
				{
					stringBuilder.AppendFormat(Environment.NewLine, new object[0]);
				}
				foreach (Attachment current3 in this._attachments)
				{
					stringBuilder.AppendFormat("--part000{0}", Environment.NewLine);
					stringBuilder.AppendFormat("Content-Type: {0}; name=\"{1}\"{2}", current3.FileType, current3.FileName.Substring(current3.FileName.IndexOf('/')).Trim(new char[]
					{
						'/'
					}), Environment.NewLine);
					if (string.IsNullOrEmpty(current3.FileEncoding))
					{
						stringBuilder.AppendFormat("Content-Transfer-Encoding: base64{0}", Environment.NewLine);
					}
					else
					{
						stringBuilder.AppendFormat("Content-Transfer-Encoding: {0}{1}", current3.FileEncoding, Environment.NewLine);
					}
					stringBuilder.AppendFormat("Content-Disposition: attachment; filename=\"{0}\"{1}", current3.FileName.Substring(current3.FileName.LastIndexOf('/')).Trim(new char[]
					{
						'/'
					}), Environment.NewLine);
					stringBuilder.Append(Environment.NewLine);
					stringBuilder.Append(current3.GetStream());
					stringBuilder.Append(Environment.NewLine);
				}
				stringBuilder.AppendFormat("--part000--{0}", Environment.NewLine);
			}
			return stringBuilder.ToString();
		}
	}
}
