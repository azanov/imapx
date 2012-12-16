using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ImapX.EmailParser;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ImapX
{
    [Serializable]
    public class Message : ISerializable
    {
        internal Imap Client;
        private string _subject;
        private DateTime _date;
        private EmailParser.EmailParser _emailParser;

        public Dictionary<string, string> Headers { get; private set; }

        public List<string> Flags { get; private set; }

        public List<Attachment> Attachments { get; set; }
        public List<InlineAttachment> InlineAttachments { get; set; }

        public MessageContent TextBody { get; set; }

        public MessageContent HtmlBody { get; set; }

        public string MimeVersion { get; set; }

        public string Organization { get; set; }

        public string Priority { get; set; }

        public string Received { get; set; }

        public string References { get; set; }

        public string ReplyTo { get; set; }

        public string XMailer { get; set; }

        public string MessageId { get; set; }

        public string ContentType { get; set; }

        public string ContentTransferEncoding { get; set; }

        public List<MessageContent> BodyParts { get; set; }

        public List<MailAddress> To { get; set; }

        public List<MailAddress> From { get; set; }

        public string Cc { get; set; }

        public string Bcc { get; set; }

        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
            }
        }

        public int MessageUid { get; set; }

        public string Subject
        {
            get
            {
                return _subject ?? string.Empty;
            }
            set
            {
                _subject = value;
            }
        }

        public Message()
        {
            Headers = new Dictionary<string, string>();
            Flags = new List<string>();
            Attachments = new List<Attachment>();
            InlineAttachments = new List<InlineAttachment>();
            BodyParts = new List<MessageContent>();
            To = new List<MailAddress>();
            From = new List<MailAddress>();
            TextBody = new MessageContent();
            HtmlBody = new MessageContent();
        }

        public bool SetFlag(string status)
        {
            bool result;
            var arrayList = new ArrayList();
            string command = string.Concat(new object[]
			{
				"STORE ", 
				MessageUid, 
				" +FLAGS (", 
				status, 
				")\r\n"
			});
            try
            {
                result = Client.SendAndReceive(command, ref arrayList);
            }
            catch
            {
                result = false;
            }
            GetFlags();
            return result;
        }

        public void ProcessHeader()
        {
            GetMessage("body[HEADER]", false);
        }

        public void ProcessFlags()
        {
            GetFlags();
        }

        /// <remarks>
        /// [27.07.2012] Fix by Pavel Azanov (coder13)
        ///              Some messages contain attachments that are not described in the ContentDisposition,
        ///              but in the ContentStream directly.
        /// </remarks>
        public bool Process()
        {
            GetFlags();
            GetMessage("BODY.PEEK[]", true);
            foreach (MessageContent current in BodyParts)
            {
                if (current.ContentType != null && current.ContentType.ToLower().Contains("text/plain"))
                {
                    TextBody = current;
                    TextBody.TextData = TextBody.ContentStream;
                }
            }
            foreach (MessageContent current2 in BodyParts)
            {
                if (current2.ContentType != null && current2.ContentType.ToLower().Contains("text/html"))
                {
                    HtmlBody = current2;
                    HtmlBody.TextData = HtmlBody.ContentStream;
                }
            }
            foreach (MessageContent current3 in BodyParts)
            {

                if (current3.ContentDisposition != null && current3.ContentDisposition.ToLower().Contains("attachment"))
                {
                    var attachment = new Attachment
                                         {
                                             FileName = ParseHelper.DecodeName(string.IsNullOrEmpty(current3.ContentFilename) ? ParseHelper.ExtractFileName(current3.ContentType) : current3.ContentFilename),
                                             FileType = ParseHelper.ExtractFileType(current3.ContentType),
                                             FileEncoding = current3.ContentTransferEncoding,
                                             FileData = Base64.FromBase64(current3.ContentStream)
                                         };
                    Attachments.Add(attachment);
                }
                else if (current3.ContentStream.ToLower().Replace(" ", "").Replace("\"", "").Contains("n=attachment") || current3.ContentStream.ToLower().Replace(" ", "").Replace("\"", "").Contains("n:attachment")) // [27.07.2012]
                    Attachments.Add(current3.ToAttachment());               // [27.07.2012]
                else if (current3.PartHeaders.Any(_ => _.Key.ToLower().Contains("attachment")))
                {
                    InlineAttachments.Add(current3.ToInlineAttachment());
                }
            }
            return true;
        }

        private void GetFlags()
        {
            bool flag;
            var arrayList = new ArrayList();
            string command = "FETCH " + MessageUid + " (FLAGS)\r\n";
            try
            {
                flag = Client.SendAndReceive(command, ref arrayList);
            }
            catch
            {
                flag = false;
            }
            if (!flag) return;
            string text = arrayList[0].ToString();
            Flags.Clear();
            if (text.Contains("\\Answered"))
            {
                Flags.Add("\\Answered");
            }
            if (text.Contains("\\Seen"))
            {
                Flags.Add("\\Seen");
            }
            if (text.Contains("\\Recent"))
            {
                Flags.Add("\\Recent");
            }
            if (text.Contains("\\Draft"))
            {
                Flags.Add("\\Draft");
            }
            if (text.Contains("\\Deleted"))
            {
                Flags.Add("\\Deleted");
            }
        }

        public string GetDecodedBody(out bool isHtml)
        {
            var body = "";
            var transferEncoding = "";
            var contentType = "";
            var encoding = Encoding.Default;

            if (HtmlBody != null && !string.IsNullOrEmpty(HtmlBody.ContentStream))
            {
                body = HtmlBody.ContentStream;
                encoding = ParseHelper.ParseContentType(HtmlBody.ContentType, out contentType);
                transferEncoding = string.IsNullOrEmpty(HtmlBody.ContentTransferEncoding) ? ContentTransferEncoding : HtmlBody.ContentTransferEncoding;
            }
            else if (TextBody != null && !string.IsNullOrEmpty(TextBody.ContentStream) && !(TextBody.ContentDisposition != null && TextBody.ContentDisposition.ToLower().Contains("attachment")))
            {
                body = TextBody.ContentStream;
                encoding = ParseHelper.ParseContentType(TextBody.ContentType, out contentType);
                transferEncoding = string.IsNullOrEmpty(TextBody.ContentTransferEncoding) ? ContentTransferEncoding : TextBody.ContentTransferEncoding;
            }
            else if (BodyParts.Count > 0)
            {
                var part = BodyParts.FirstOrDefault(_ => !(_.ContentDisposition != null && _.ContentDisposition.ToLower().Contains("attachment")));
                if (part == null)
                {
                    isHtml = false;
                    return string.Empty;
                }
                body = part.ContentStream;
                encoding = ParseHelper.ParseContentType(part.ContentType, out contentType);
                transferEncoding = string.IsNullOrEmpty(part.ContentTransferEncoding) ? ContentTransferEncoding : part.ContentTransferEncoding;
            }

            if (encoding == null)
            {
                var rex = new Regex("^(.*):\\s(.*)[\r\n]?");
                var tmp = (new Regex("\r\n")).Split(body).ToArray();

                for (var i = 0; i < tmp.Length; i++)
                {
                    if (string.IsNullOrEmpty(tmp[i])) continue;
                    var m = rex.Match(tmp[i]);
                    if (!m.Success)
                    {
                        //all headers passed
                        body = string.Join("\r\n", tmp.Skip(i).ToArray());
                        break;
                    }
                    if (m.Groups[1].Value.ToLower().Trim() == "content-transfer-encoding")
                        transferEncoding = m.Groups[2].Value;
                    else if (m.Groups[1].Value.ToLower().Trim() == "content-type")
                        encoding = ParseHelper.ParseContentType(m.Groups[2].Value, out contentType);
                }

            }

            transferEncoding = (transferEncoding ?? string.Empty).ToLower().Trim();

            switch (transferEncoding)
            {
                case "base64":
                    body = ParseHelper.DecodeBase64(body, encoding);
                    break;
                case "quoted-printable":
                    body = ParseHelper.DecodeQuotedPrintable(body, encoding);
                    break;
            }



            isHtml = contentType == "text/html";

            return body;
        }

        /// <remarks>
        /// [27.07.2012] Fix by Pavel Azanov (coder13)
        ///              Added automated decoding of mail subject
        /// [30.07.2012] Replaced weird if-clauses used for header
        ///              parsing with an easy to read switch-case
        /// </remarks>
        private void GetMessage(string path, bool processBody)
        {
            var arrayList = new ArrayList();
            string command = string.Concat(new object[]
			{
				"FETCH ", 
				MessageUid, 
				" ", 
				path, 
				"\r\n"
			});

            Client.SendAndReceive(command, ref arrayList);
            try
            {
                _emailParser = new EmailParser.EmailParser(arrayList.ToArray(typeof(string)) as string[]);
            }
            catch
            { }

            _emailParser.InitializeIndexes();
            _emailParser.ParseHeaders();



            foreach (KeyValuePair<string, string> current in _emailParser._headersCollection)
            {
                var key = current.Key.ToLower().Trim();

                switch (key)
                {
                    case MessageProperty.TO:
                        To = ParseHelper.AddressCollection(current.Value);
                        break;
                    case MessageProperty.FROM:
                        From = ParseHelper.AddressCollection(current.Value);
                        break;
                    case MessageProperty.DATE:
                        DateTime.TryParse((new Regex(@"\(.*\)").Replace(current.Value.Trim(), "").Trim()), out _date);
                        break;
                    case MessageProperty.CONTENT_TRANSFER_ENCODING:
                        ContentTransferEncoding = current.Value;
                        break;
                    case MessageProperty.CONTENT_TYPE:
                        ContentType = current.Value;
                        break;
                    case MessageProperty.MESSAGE_ID:
                        MessageId = current.Value;
                        break;
                    case MessageProperty.MIME_VERSION:
                        MimeVersion = current.Value;
                        break;
                    case MessageProperty.ORGANIZATION:
                        Organization = current.Value;
                        break;
                    case MessageProperty.PRIORITY:
                        Priority = current.Value;
                        break;
                    case MessageProperty.RECEIVED:
                        Received = current.Value;
                        break;
                    case MessageProperty.REFERENCES:
                        References = current.Value;
                        break;
                    case MessageProperty.REPLY_TO:
                        ReplyTo = ParseHelper.DecodeName(current.Value);
                        break;
                    case MessageProperty.X_MAILER:
                        XMailer = current.Value;
                        break;
                    case MessageProperty.CC:
                        Cc = ParseHelper.DecodeName(current.Value);
                        break;
                    case MessageProperty.BCC:
                        Bcc = ParseHelper.DecodeName(current.Value);
                        break;
                    case MessageProperty.SUBJECT:
                        _subject = ParseHelper.DecodeName(current.Value);
                        break;

                }

            }
            Headers = _emailParser._headersCollection;
            if (!processBody) return;
            _emailParser.ParseBody();
            foreach (BodyPart current2 in _emailParser._parts)
            {
                var messageContent = new MessageContent();
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
                                // Fix provided by Henkes
                                // For reference see http://imapx.codeplex.com/workitem/1423
                                string contentFilename = current3.Value.Split(new[]
                                                                                  {
                                                                                      "filename="
                                                                                  }, StringSplitOptions.None)[1].Split(';')[0].Trim(new[]
                                                                                                                          {
                                                                                                                              '"'
                                                                                                                          }).Replace("\n","");
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
                            if (current3.Key.ToLower().Equals(MessageProperty.CONTENT_TRANSFER_ENCODING))
                            {
                                messageContent.ContentTransferEncoding = current3.Value;
                            }
                        }
                    }
                }
                if (current2.Boundary == null)
                {
                    messageContent.ContentType = ContentType;
                }
                messageContent.ContentStream = _emailParser.GetPart(current2);
                messageContent.PartHeaders = current2.Headers;
                messageContent.BoundaryName = current2.Boundary;
                BodyParts.Add(messageContent);
            }
        }

        private StringBuilder GetEml()
        {
            var stringBuilder = new StringBuilder();
            if (_emailParser != null && _emailParser._emailItems != null && _emailParser._emailItems.Length > 0)
            {
                for (int i = 1; i <= _emailParser._emailItems.Length - 2; i++)
                {
                    if (i == _emailParser._emailItems.Length - 2 && _emailParser._emailItems[i].Length > 0)
                    {
                        stringBuilder.AppendFormat("{0}{1}", _emailParser._emailItems[i].TrimEnd(new[]
						{
							')'
						}), "\r\n");
                    }
                    else
                    {
                        stringBuilder.AppendFormat("{0}{1}", _emailParser._emailItems[i], "\r\n");
                    }
                }
            }
            else
            {
                stringBuilder.Append(MessageBuilder());
            }
            return stringBuilder;
        }

        public string GetAsString()
        {
            return GetEml().ToString();
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
            StringBuilder eml = GetEml();
            using (var fileStream = new FileStream(path + filename + ".eml", FileMode.Create, FileAccess.Write))
            {
                using (TextWriter textWriter = new StreamWriter(fileStream, Encoding.ASCII))
                {
                    textWriter.Write(eml.ToString());
                }
            }
        }

        public string MessageBuilder()
        {
            var stringBuilder = new StringBuilder();
            DateTime now = DateTime.Now;
            stringBuilder.AppendFormat("Date: {0}{1}", now.ToString("dd-MM-yyyy hh:mm:ss", CultureInfo.CreateSpecificCulture("en-US")), "\r\n");
            if (From.Count > 0)
            {
                stringBuilder.Append("From: ");
                foreach (MailAddress current in From)
                {
                    stringBuilder.AppendFormat("{0}, ", current);
                }
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
                stringBuilder.Append("\r\n");
            }
            if (To.Count > 0)
            {
                stringBuilder.Append("To: ");
                foreach (MailAddress current2 in To)
                {
                    stringBuilder.AppendFormat("{0}, ", current2);
                }
                stringBuilder.Remove(stringBuilder.Length - 2, 1);
                stringBuilder.Append("\r\n");
            }
            if (!string.IsNullOrEmpty(_subject))
            {
                stringBuilder.AppendFormat("Subject: {0}{1}", _subject, "\r\n");
            }
            if (string.IsNullOrEmpty(MessageId))
            {
                stringBuilder.AppendFormat("Message-Id: {0}@ImapX{1}", Guid.NewGuid(), "\r\n");
            }
            else
            {
                stringBuilder.AppendFormat("Message-Id: {0}{1}", MessageId, "\r\n");
            }
            if (!string.IsNullOrEmpty(XMailer))
            {
                stringBuilder.AppendFormat("X-Mailer: {0}{1}", XMailer, "\r\n");
            }
            if (!string.IsNullOrEmpty(ContentTransferEncoding))
            {
                stringBuilder.AppendFormat("Content-Transfer-Encoding: {0}{1}", ContentTransferEncoding, "\r\n");
            }
            if (string.IsNullOrEmpty(MimeVersion))
            {
                stringBuilder.AppendFormat("MIME-Version: 1.0{0}", "\r\n");
            }
            else
            {
                stringBuilder.AppendFormat("MIME-Version: {0}{1}", MimeVersion, "\r\n");
            }
            if (TextBody == null && Attachments.Count <= 0)
            {
                stringBuilder.AppendFormat("Content-Type: text/html; charset=utf-8{0}", "\r\n");
                stringBuilder.AppendFormat("Content-Transfer-Encoding: base64{0}", "\r\n");
                stringBuilder.Append("\r\n");
                if (HtmlBody != null && HtmlBody.TextData != null)
                {
                    stringBuilder.AppendFormat("{0}{1}", Base64.ToBase64(Encoding.UTF8.GetBytes(HtmlBody.TextData)), Environment.NewLine);
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
                if (TextBody != null && TextBody.TextData != null)
                {
                    stringBuilder.AppendFormat("{0}{1}", Base64.ToBase64(Encoding.UTF8.GetBytes(TextBody.TextData)), Environment.NewLine);
                }
                else
                {
                    stringBuilder.AppendFormat(Environment.NewLine, new object[0]);
                }
                stringBuilder.AppendFormat("--part000{0}", "\r\n");
                stringBuilder.AppendFormat("Content-Type: text/html; charset=utf-8{0}", "\r\n");
                stringBuilder.AppendFormat("Content-Transfer-Encoding: base64{0}", "\r\n");
                stringBuilder.Append("\r\n");
                if (HtmlBody != null && HtmlBody.TextData != null)
                {
                    stringBuilder.AppendFormat("{0}{1}", Base64.ToBase64(Encoding.UTF8.GetBytes(HtmlBody.TextData)), Environment.NewLine);
                }
                else
                {
                    stringBuilder.AppendFormat(Environment.NewLine, new object[0]);
                }
                foreach (Attachment current3 in Attachments)
                {
                    stringBuilder.AppendFormat("--part000{0}", Environment.NewLine);
                    stringBuilder.AppendFormat("Content-Type: {0}; name=\"{1}\"{2}", current3.FileType, current3.FileName.Substring(current3.FileName.IndexOf('/')).Trim(new[]
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
                    stringBuilder.AppendFormat("Content-Disposition: attachment; filename=\"{0}\"{1}", current3.FileName.Substring(current3.FileName.LastIndexOf('/')).Trim(new[]
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

        public Message(SerializationInfo info, StreamingContext ctxt)
        {
            this.BodyParts = (List<MessageContent>)info.GetValue("Parts", typeof(List<MessageContent>));
            this.ContentTransferEncoding = (string)info.GetValue("Encoding", typeof(string));
            this.ContentType = (string)info.GetValue("Type", typeof(string));
            this.Subject = (string)info.GetValue("Subject", typeof(string));
            this.XMailer = (string)info.GetValue("XMailer", typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Encoding", this.ContentTransferEncoding);
            info.AddValue("Type", this.ContentType);
            info.AddValue("Subject", this.Subject);
            info.AddValue("XMailer", this.XMailer);
            info.AddValue("Parts", this.BodyParts);
        }

        public void ExportForReport(string fileName)
        {
            using (Stream stream = File.Open(fileName, FileMode.Create))
            {
                BinaryFormatter bFormatter = new BinaryFormatter();
                bFormatter.Serialize(stream, this);
                stream.Close();
            }
        }

        public static Message FromReport(string fileName)
        {
            Message objectToSerialize;
            using (Stream stream = File.Open(fileName, FileMode.Open))
            {
                BinaryFormatter bFormatter = new BinaryFormatter();
                objectToSerialize = (Message)bFormatter.Deserialize(stream);
                stream.Close();
            }
            return objectToSerialize;

        }

    }
}
