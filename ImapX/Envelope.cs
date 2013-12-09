using System;
using System.Collections.Generic;

namespace ImapX
{
    public class Envelope
    {
        public string MessageId { get; set; }
    
        public MailAddress From { get; set; }
        public MailAddress Sender { get; set; }
        public List<MailAddress> To { get; set; }
        public List<MailAddress> Cc { get; set; }
        public List<MailAddress> Bcc { get; set; }
        public List<MailAddress> ReplyTo { get; set; }
        public string InReplyTo { get; set; }
        public DateTime? Date { get; set; }

        public string Subject { get; set; }
    }
}