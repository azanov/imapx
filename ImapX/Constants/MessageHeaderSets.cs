using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImapX.Constants
{
    public sealed class MessageHeaderSets
    {

        public static readonly string[] Minimal =
        {
            MessageHeader.From,
            MessageHeader.To,
            MessageHeader.Date,
            MessageHeader.Subject,
            MessageHeader.Cc,
            MessageHeader.ContentType,
            MessageHeader.MessageId
        };

        public static readonly string[] Envelope =
        {
            MessageHeader.From,
            MessageHeader.To,
            MessageHeader.Date,
            MessageHeader.Subject,
            MessageHeader.Cc,
            MessageHeader.MessageId,
            MessageHeader.Bcc,
            MessageHeader.ReplyTo,
            MessageHeader.InReplyTo,
            MessageHeader.Sender
        };

    }
}
