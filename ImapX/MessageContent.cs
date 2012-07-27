using System;
using System.Collections.Generic;
namespace ImapX
{
    public class MessageContent
    {
        private string _contentStream;
        private string _partID;
        private string _textData;
        private byte[] _binaryData;
        private string _contentType;
        private string _contentTransferEncoding;
        private int _contentSize;
        private string _contentID;
        private string _contentDisposition;
        private string _contentFilename;
        private string _mimeVersion;
        private string _contentDescription;
        private string _boundary;
        private Dictionary<string, string> _partHeaders;
        public string BoundaryName
        {
            get
            {
                return this._boundary;
            }
            set
            {
                this._boundary = value;
            }
        }
        public Dictionary<string, string> PartHeaders
        {
            get
            {
                return this._partHeaders;
            }
            set
            {
                this._partHeaders = value;
            }
        }
        public string ContentStream
        {
            get
            {
                return this._contentStream;
            }
            set
            {
                this._contentStream = value;
            }
        }
        public string ContentDescription
        {
            get
            {
                return this._contentDescription;
            }
            set
            {
                this._contentDescription = value;
            }
        }
        public string MIMEVersion
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
        public string ContentFilename
        {
            get
            {
                return this._contentFilename;
            }
            set
            {
                this._contentFilename = value;
            }
        }
        public string ContentDisposition
        {
            get
            {
                return this._contentDisposition;
            }
            set
            {
                this._contentDisposition = value;
            }
        }
        public string ContentId
        {
            get
            {
                return this._contentID;
            }
            set
            {
                this._contentID = value;
            }
        }
        public string PartID
        {
            get
            {
                return this._partID;
            }
            set
            {
                this._partID = value;
            }
        }
        public string TextData
        {
            get
            {
                return this._textData;
            }
            set
            {
                this._textData = value;
            }
        }
        public byte[] BinaryData
        {
            get
            {
                return this._binaryData;
            }
            set
            {
                this._binaryData = value;
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
        public int ContentSize
        {
            get
            {
                return this._contentSize;
            }
            set
            {
                this._contentSize = value;
            }
        }
        public MessageContent()
        {
            this._contentStream = string.Empty;
        }
    }
}
