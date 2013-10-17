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
            MessageHeader.ContentType
        };

    }
}
