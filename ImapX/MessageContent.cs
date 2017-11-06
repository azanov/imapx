using ImapX.Constants;
using ImapX.EncodingHelpers;
using ImapX.Enums;
using ImapX.Extensions;
using ImapX.Parsing;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;

namespace ImapX
{

    public class MessageContent
    {
        protected bool _downloading;

        internal ImapClient Client;
        internal Message Message;

        internal MessageContent() { }

        public MessageContent(ImapClient client, Message message)
        {
            Client = client;
            Message = message;
            Parameters = new Dictionary<string, string>();
            Headers = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Headers { get; set; }

        private string _contentId;
        public string ContentId
        {
            get
            {
                return _contentId;
            }
            set
            {
                _contentId = value?.Trim('<', '>', ' ');
            }
        }

        public string ContentNumber { get; set; }

        public ContentType ContentType { get; set; }

        public ContentTransferEncoding ContentTransferEncoding { get; set; }

        public ContentDisposition ContentDisposition { get; set; }

        public string Description { get; set; }

        public Dictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// The size of the message part
        /// </summary>
        public long Size { get; set; }

        public string Md5 { get; set; }
        public string[] Language { get; set; }

        public bool Downloaded { get; internal set; }

        public void Download()
        {
            if (!Downloaded && !_downloading)
            {
                _downloading = true;
                Client.FetchMessage(Message, MessageFetchMode.BodyPart, bodyPartNumber: ContentNumber);
                _downloading = false;
            }
        }

        public bool Binary
        {
            get
            {
                return Downloaded && _contentStream == null;
            }
        }

        protected byte[] _data;
        public byte[] BinaryData
        {
            get
            {
                if (!Downloaded) Download();
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        protected string _contentStream;
        public string ContentStream
        {
            get
            {
                if (!Downloaded) Download();
                if (_contentStream == null && BinaryData != null)
                    _contentStream = Encoding.UTF8.GetString(BinaryData);
                return _contentStream;
            }
            internal set
            {
                _contentStream = value;
            }
        }

        internal void AddHeaderInternal(string headerName, string headerValue)
        {
            Headers[headerName] = headerValue;

            switch (headerName)
            {
                case MessageHeader.ContentType:
                    ContentType = HeaderFieldParser.ParseContentType(headerValue);
                    break;
                case MessageHeader.ContentTransferEncoding:
                    ContentTransferEncoding = headerValue.ToContentTransferEncoding();
                    break;
                case MessageHeader.ContentId:
                    ContentId = headerValue;
                    break;
            }
        }

    }
}