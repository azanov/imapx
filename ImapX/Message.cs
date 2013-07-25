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
using ImapX.Collections;

namespace ImapX
{
    [Serializable]
    public class Message : ISerializable
    {
        internal ImapClient Client;
        internal Folder Folder;  // [5/10/13] Fix by axlns
        private string _subject;
        private DateTime _date;
        private EmailParser.EmailParser _emailParser;

        public Dictionary<string, string> Headers { get; private set; }

        public MessageFlagCollection Flags { get; private set; }

        public GMailMessageLabelCollection Labels { get; private set; }

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

        public MailAddress From { get; set; }

        public List<MailAddress> Cc { get; set; }

        public List<MailAddress> Bcc { get; set; }

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

        public Message(ImapClient client)
        {
            Client = client;
            Headers = new Dictionary<string, string>();
            Flags = new MessageFlagCollection(Client, this);
            Labels = new GMailMessageLabelCollection(Client, this);
            Attachments = new List<Attachment>();
            InlineAttachments = new List<InlineAttachment>();
            BodyParts = new List<MessageContent>();
            To = new List<MailAddress>();
            TextBody = new MessageContent();
            HtmlBody = new MessageContent();
        }

        [Obsolete("AddFlag is obsolete, please use Flags.Add instead")]
        public bool AddFlag(string flag)
        {
            return Flags.Add(flag);
        }

        [Obsolete("RemoveFlag is obsolete, please use Flags.Remove instead")]
        public bool RemoveFlag(string flag)
        {
            return Flags.Remove(flag);
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
        /// [5/10/13] Fix by axlns
        /// </remarks>
        public bool Process()
        {
            // [5/10/13] Fix by axlns

            string returnToFolder = "";

            if (Client.SelectedFolder != Folder.FolderPath)
            {
                returnToFolder = Client.SelectedFolder;
                Client.SelectFolder(Folder.FolderPath);
            }


            try
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
                    else if (current.ContentType != null && current.ContentType.ToLower().Contains("text/html"))
                    {
                        HtmlBody = current;
                        HtmlBody.TextData = HtmlBody.ContentStream;
                    }
                    else if (current.ContentType != null && current.ContentType.ToLower().Contains("message/rfc822")) // [2013-04-24] naudelb(Len Naude) - Added
                    {
                        // This part is an email attachment in mime(text) format that will be atached as an "eml" file
                        // The name of the file will be derived from the attachment's "Subject" line
                        Attachments.Add(current.ToAttachment());
                    }
                    else if (current.ContentType != null && current.ContentType.ToLower().Contains("message/delivery-status")) // [2013-04-24] naudelb(Len Naude) - Added
                    {
                        // Delivery failed notice atachment in mime(text) format
                        // Name will be hardcoded as "details.txt" as this is what outlook does
                        Attachments.Add(current.ToAttachment());
                    }
                    else if (current.ContentDisposition != null && current.ContentDisposition.ToLower().Contains("attachment") ||
                             !string.IsNullOrEmpty(current.ContentType) && current.ContentType.Replace(" ", "").ToLower().Contains("name=")) //Mails sent from powershell do not have attachments marked as attachments.. Recognize them by containing a filename in ContentType
                    {

                        // [2013-04-24] naudelb(Len Naude) - Embedded Image/Inline Attachment if the Content-ID is present and not explicitly specified as attachment
                        if (string.IsNullOrEmpty(current.ContentId) || (current.ContentDisposition != null && current.ContentDisposition.ToLower().Contains("attachment")))
                        {

                            var attachment = new Attachment
                            {
                                FileName = ParseHelper.DecodeName(string.IsNullOrEmpty(current.ContentFilename) ? ParseHelper.ExtractFileName(current.ContentType) : current.ContentFilename),
                                FileType = ParseHelper.ExtractFileType(current.ContentType),
                                FileEncoding = current.ContentTransferEncoding
                            };

                            // [2013-04-24] naudelb(Len Naude) - Clean File Name 
                            attachment.FileName = attachment.FileName.Replace(":", "_").Replace("\\", "_");
                            current.ContentStream = current.ContentStream.TrimStart("\r\n".ToCharArray());
                            // [2013-04-24] naudelb(Len Naude) - The value might be mixed case
                            //switch (attachment.FileEncoding)
                            switch (string.IsNullOrEmpty(attachment.FileEncoding) ? "7bit" : attachment.FileEncoding.ToLower())
                            {
                                case "base64":
                                    attachment.FileData = Base64.FromBase64(current.ContentStream);
                                    break;
                                case "7bit":
                                    attachment.FileData = Encoding.ASCII.GetBytes(current.ContentStream);
                                    break;
                                case "quoted-printable":
                                    attachment.FileData = Encoding.UTF8.GetBytes(ParseHelper.DecodeQuotedPrintable(current.ContentStream, Encoding.UTF8));
                                    break;
                                default:
                                    attachment.FileData = Encoding.UTF8.GetBytes(current.ContentStream);
                                    break;
                            }
                            Attachments.Add(attachment);

                        }
                        else
                        {
                            InlineAttachments.Add(current.ToInlineAttachment());
                        }

                    }
                    else if (current.ContentStream.ToLower().Replace(" ", "").Replace("\"", "").Contains("n=attachment") || current.ContentStream.ToLower().Replace(" ", "").Replace("\"", "").Contains("n:attachment")) // [27.07.2012]
                        Attachments.Add(current.ToAttachment());               // [27.07.2012]
                    else if (current.PartHeaders.Any(_ => _.Key.ToLower().Contains("attachment")))
                    {
                        InlineAttachments.Add(current.ToInlineAttachment());
                    }


                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (!string.IsNullOrEmpty(returnToFolder))
                {
                    Client.SelectFolder(returnToFolder);
                }
            }
        }

        private void GetFlags()
        {

            var flagRex = new Regex(@"FLAGS \((.*?)\)");
            var labelsRex = new Regex(@"X-GM-LABELS \((.*?)\)");

            IList<string> data = new List<string>();
            string command = "UID FETCH " + MessageUid + " (FLAGS)\r\n"; // [21.12.12] Fix by Yaroslav T, added UID command

            if(Client.SendAndReceive(command, ref data))
                Flags.AddRangeInternal(flagRex.Match(data[0]).Groups[1].Value.Split(' ').Where(_=>!string.IsNullOrEmpty(_)));

            if (!Client.Capabilities.XGMExt1) return;

            command = "UID FETCH " + MessageUid + " (X-GM-LABELS)\r\n";
            data.Clear();

            if(Client.SendAndReceive(command, ref data))
                Labels.AddRangeInternal(labelsRex.Match(data[0]).Groups[1].Value.Split(' ').Where(_ => !string.IsNullOrEmpty(_)));
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
            IList<string> arrayList = new List<string>();
            string command = string.Concat(new object[]
			{
				"UID FETCH ", // [21.12.12] Fix by Yaroslav T, added UID command
				MessageUid, 
				" ", 
				path, 
				"\r\n"
			});

            Client.SendAndReceive(command, ref arrayList);
            try
            {
                _emailParser = new EmailParser.EmailParser(arrayList.ToArray());
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
                        From = ParseHelper.Address(current.Value);
                        break;
                    case MessageProperty.DATE:
                        DateTime.TryParse((new Regex(@"\(.*\)").Replace(current.Value.Trim(), "").Trim()), CultureInfo.InvariantCulture, DateTimeStyles.None, out _date);
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
                        Cc = ParseHelper.AddressCollection(current.Value);
                        break;
                    case MessageProperty.BCC:
                        Bcc = ParseHelper.AddressCollection(current.Value);
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

                        // [2013-04-24] naudelb(Len Naude) - Keep the Content-ID as reference to the embedded iamge (Inline Attachment)
                        if (current3.Key.ToLower().Equals("content-id"))
                        {
                            messageContent.ContentId = current3.Value;
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
            if (From != null)
            {
                stringBuilder.Append("From: ");
                stringBuilder.AppendFormat("{0}, ", From);
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
