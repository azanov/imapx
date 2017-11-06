using ImapX.Collections;
using ImapX.Constants;
using ImapX.EncodingHelpers;
using ImapX.Enums;
using ImapX.Extensions;
using ImapX.Flags;
using ImapX.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace ImapX
{
    public class Message
    {
        internal MessageFetchMode DownloadProgress;
        internal MessageFetchMode InProgress = MessageFetchMode.None;

        public ImapClient Client { get; internal set; }

        public Folder Folder { get; internal set; }

        /// <summary>
        ///     A relative position from 1 to the number of messages in the mailbox.
        /// </summary>
        public long SequenceNumber { get; set; }

        /// <summary>
        ///     Size of the message in bytes
        /// </summary>
        public long Size { get; internal set; }

        public DateTime? InternalDate { get; internal set; }

        /// <summary>
        ///     The time when the message was written (or submitted)
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        ///     All headers associated with the message
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        ///     An indicator that this message is formatted according to the MIME standard, and an indication of which version of
        ///     MIME is utilized.
        /// </summary>
        public string MimeVersion { get; set; }

        /// <summary>
        ///     Text that provides a summary, or indicates the nature, of the message
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        ///     Data type and format of content
        /// </summary>
        public ContentType ContentType { get; set; }

        /// <summary>
        ///     Coding method used in a MIME message body
        /// </summary>
        public string ContentTransferEncoding { get; set; }

        /// <summary>
        ///     Author or person taking responsibility for the message.
        /// </summary>
        public MailAddress From { get; set; }

        /// <summary>
        ///     The person or agent submitting the message to the network, if other than shown by the From header
        /// </summary>
        public MailAddress Sender { get; set; }

        /// <summary>
        ///     Primary recipient(s)
        /// </summary>
        public List<MailAddress> To { get; set; }

        /// <summary>
        ///     Secondary, informational recipients. (cc = Carbon Copy)
        /// </summary>
        public List<MailAddress> Cc { get; set; }

        /// <summary>
        ///     Recipient(s) not to be disclosed to other recipients ("blind carbon copy")
        /// </summary>
        public List<MailAddress> Bcc { get; set; }

        public MailAddress ReturnPath { get; set; }

        /// <summary>
        ///     The organization to which the sender belongs, or to which the machine belongs
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        ///     A hint from the sender to the recipients about how important a message is
        /// </summary>
        public MessageImportance Importance { get; set; }

        /// <summary>
        ///     How sensitive it is to disclose this message to other people than the specified recipients
        /// </summary>
        public MessageSensitivity Sensitivity { get; set; }

        /// <summary>
        ///     Unique ID of this message.
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        ///     Client software used by the sender
        /// </summary>
        /// <remarks>Parsed from Mailer or X-Mailer header.</remarks>
        public string Mailer { get; set; }

        /// <summary>
        ///     Suggested E-mail address(es) for replies
        /// </summary>
        public List<MailAddress> ReplyTo { get; set; }

        /// <summary>
        ///     Code for natural language used in the message.
        /// </summary>
        /// <remarks>Parsed from Language or Content-Language header.</remarks>
        public string Language { get; set; }

        /// <summary>
        ///     Reference to message which this message is a reply to.
        /// </summary>
        public string InReplyTo { get; set; }

        /// <summary>
        ///     Text comments added to the message
        /// </summary>
        public string Comments { get; set; }

        private MessageContent[] _bodyParts;
        public MessageContent[] BodyParts
        {
            get
            {
                if (!DownloadProgress.HasFlag(MessageFetchMode.BodyStructure) && !InProgress.HasFlag(MessageFetchMode.BodyStructure))
                    Download(MessageFetchMode.BodyStructure);

                return _bodyParts;
            }
            internal set
            {
                _bodyParts = value;
            }
        }

        private Attachment[] _attachments;
        public Attachment[] Attachments
        {
            get
            {
                if (_attachments == null)
                    _attachments = BodyParts
                    .Where(_ => _.ContentDisposition != null && _.ContentDisposition.DispositionType == "attachment")
                    .Select(_ => new Attachment(_))
                    .ToArray();

                return _attachments;
            }
        }

        private Attachment[] _embeddedResources;
        public Attachment[] EmbeddedResources
        {
            get
            {
                if (_embeddedResources == null)
                    _embeddedResources = BodyParts
                    .Where(_ => (_.ContentDisposition != null && _.ContentDisposition.Inline) || !string.IsNullOrWhiteSpace(_.ContentId))
                    .Select(_ => new Attachment(_))
                    .ToArray();

                return _embeddedResources;
            }
        }

        private MessageBody _body;
        public MessageBody Body
        {
            get
            {
                if (_body == null)
                {
                    _body = new MessageBody(Client,
                           BodyParts.FirstOrDefault(_ => _.ContentType != null && _.ContentType.MediaType == "text/plain"),
                           BodyParts.FirstOrDefault(_ => _.ContentType != null && _.ContentType.MediaType == "text/html"));
                }
                return _body;
            }
            internal set
            {
                _body = value;
            }
        }

        /// <summary>
        ///     A unique identifier of the message.
        /// </summary>
        public long UId { get; private set; }

        /// <summary>
        ///     A list of zero or more named tokens associated with the message.
        ///     Can contain system flags (<code>Flags.MessageFlags</code>) or keywords defined by the server.
        /// </summary>
        /// <see cref="ImapX.Flags.MessageFlags" />
        public MessageFlagCollection Flags
        {
            get
            {
                if (!DownloadProgress.HasFlag(MessageFetchMode.Flags) && !InProgress.HasFlag(MessageFetchMode.Flags))
                    Download(MessageFetchMode.Flags);
                return _flags;
            }
            internal set
            {
                _flags = value;
            }
        }
        private MessageFlagCollection _flags;

        /// <summary>
        ///     Gets or sets whether the message has been read/seen
        /// </summary>
        public bool Seen
        {
            get { return Flags.Contains(MessageFlags.Seen); }
            set
            {
                if (value)
                    Flags.Add(MessageFlags.Seen);
                else
                    Flags.Remove(MessageFlags.Seen);
            }
        }

        public Message()
        {
            Headers = new Dictionary<string, string>();
            BodyParts = new MessageContent[0];
            Flags = new MessageFlagCollection();
            To = new List<MailAddress>();
            Cc = new List<MailAddress>();
            Bcc = new List<MailAddress>();
        }

        internal Message(ImapClient client, Folder folder)
            : this()
        {
            Flags = new MessageFlagCollection(client, this);
            //Labels = new GMailMessageLabelCollection(client, this);

            Client = client;
            Folder = folder;
        }

        internal Message(long uIdOrSequenceNumber, ImapClient client, Folder folder)
            : this(client, folder)
        {
            if (client.Capabilities.UIdPlus)
            {
                UId = uIdOrSequenceNumber;
                SequenceNumber = -1;
            }
            else
            {
                SequenceNumber = uIdOrSequenceNumber;
                UId = -1;
            }
        }

        public void Download(MessageFetchMode mode = MessageFetchMode.ClientDefault, bool reloadHeaders = false)
        {
            Client.FetchMessage(this, mode, reloadHeaders);
        }

        internal void AddHeaderInternal(string headerName, string headerValue)
        {
            Headers[headerName] = headerValue;

            switch (headerName)
            {
                case MessageHeader.MimeVersion:
                    MimeVersion = headerValue;
                    break;
                case MessageHeader.Sender:
                    Sender = HeaderFieldParser.ParseMailAddress(headerValue);
                    break;
                case MessageHeader.Subject:
                    Subject = StringDecoder.Decode(headerValue, true);
                    break;
                case MessageHeader.To:
                    if (To.Count == 0)
                        To = HeaderFieldParser.ParseMailAddressCollection(headerValue);
                    else
                        foreach (MailAddress addr in HeaderFieldParser.ParseMailAddressCollection(headerValue))
                            To.Add(addr);
                    break;
                case MessageHeader.DeliveredTo:
                    To.Add(HeaderFieldParser.ParseMailAddress(headerValue));
                    break;
                case MessageHeader.From:
                    From = HeaderFieldParser.ParseMailAddress(headerValue);
                    break;
                case MessageHeader.Cc:
                    Cc = HeaderFieldParser.ParseMailAddressCollection(headerValue);
                    break;
                case MessageHeader.Bcc:
                    Bcc = HeaderFieldParser.ParseMailAddressCollection(headerValue);
                    break;
                case MessageHeader.Organisation:
                case MessageHeader.Organization:
                    Organization = StringDecoder.Decode(headerValue, true);
                    break;
                case MessageHeader.Date:
                    Date = HeaderFieldParser.ParseDate(headerValue);
                    break;
                case MessageHeader.Importance:
                    Importance = headerValue.ToMessageImportance();
                    break;
                case MessageHeader.ContentType:
                    ContentType = HeaderFieldParser.ParseContentType(headerValue);
                    break;
                case MessageHeader.ContentTransferEncoding:
                    ContentTransferEncoding = headerValue;
                    break;
                case MessageHeader.MessageId:
                    MessageId = headerValue;
                    break;
                case MessageHeader.Mailer:
                case MessageHeader.XMailer:
                    Mailer = headerValue;
                    break;
                case MessageHeader.ReplyTo:
                    ReplyTo = HeaderFieldParser.ParseMailAddressCollection(headerValue);
                    break;
                case MessageHeader.Sensitivity:
                    Sensitivity = headerValue.ToMessageSensitivity();
                    break;
                case MessageHeader.ReturnPath:
                    ReturnPath =
                        HeaderFieldParser.ParseMailAddress(
                            headerValue.Split(new[] { '\r', '\n' }, StringSplitOptions.None)
                                .Distinct()
                                .FirstOrDefault());
                    break;
                case MessageHeader.ContentLanguage:
                case MessageHeader.Language:
                    Language = headerValue;
                    break;
                case MessageHeader.InReplyTo:
                    InReplyTo = headerValue;
                    break;
                case MessageHeader.Comments:
                    Comments = headerValue;
                    break;
            }
        }

        internal void ApplyEnvelope(Envelope envelope)
        {
            Date = envelope.Date;
            Subject = envelope.Subject;
            From = envelope.From;
            Sender = envelope.Sender;
            ReplyTo = envelope.ReplyTo;
            To = envelope.To;
            Cc = envelope.Cc;
            Bcc = envelope.Bcc;
            InReplyTo = envelope.InReplyTo;
            MessageId = envelope.MessageId;
        }
    }
}
