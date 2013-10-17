using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImapX.Constants
{
    public sealed class MessageHeader
    {
        public const string MimeVersion = "mime-version";
        public const string Subject = "subject";
        public const string Sender = "sender";
        public const string To = "to";
        public const string From = "from";
        public const string Cc = "cc";
        public const string Bcc = "bcc";
        public const string Organisation = "organisation";
        public const string Organization = "organization";
        public const string Date = "date";
        public const string Priority = "priority";
        public const string XPriority = "x-priority";
        public const string XMsMailPriority = "x-msmail-priority";
        public const string Importance = "importance";
        public const string ContentType = "content-type";
        public const string ContentTransferEncoding = "content-transfer-encoding";
        public const string MessageId = "message-id";
        public const string XMailer = "x-mailer";
        public const string Mailer = "mailer";
        public const string ReplyTo = "reply-to";
        public const string Received = "received";
        public const string References = "references";
        public const string Sensitivity = "sensitivity";
        public const string ReturnPath = "return-path";
        public const string DeliveredTo = "delivered-to";
        public const string ContentLanguage = "content-language";
        public const string Language = "language";
        public const string InReplyTo = "in-reply-to";
        public const string Comments = "comments";
    }
}
