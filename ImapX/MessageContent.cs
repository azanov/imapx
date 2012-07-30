using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

        public string DecodedContentStream
        {
            get
            {
                return this._contentStream;
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

        /// <summary>
        /// Some messages contain attachments that are not described in the ContentDisposition,
        /// but in the ContentStream directly. This method is used to convert a body part to 
        /// an attachment
        /// </summary>
        /// <remarks>
        /// [27.07.2012] Fix by Pavel Azanov (coder13)
        /// </remarks>
        internal Attachment ToAttachment()
        {
            var rex = new Regex(@"(.*)[:|=][\s]?(.*)[;]?");
            var tmp = ContentStream.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var attachment = new Attachment();

            var bodyPart = string.Empty;

            for (var i = 0; i < tmp.Length; i++)
            {

                if (tmp[i].StartsWith("---"))
                    continue;

                var line = tmp[i].Trim('\t');
                var match = rex.Match(line);

                if (!match.Success)
                {
                    bodyPart = string.Join("\r\n", tmp.Skip(i));
                    break;
                }

                var field = match.Groups[1].Value.ToLower().Trim();
                var value = match.Groups[2].Value.Trim().TrimEnd(';');

                switch (field)
                {
                    case "content-type":
                        attachment.FileType = value.ToLower();
                        break;
                    case "name":
                    case "filename":
                        attachment.FileName = value.Trim('"').Trim('\'');
                        break;
                    case "content-transfer-encoding":
                        attachment.FileEncoding = value.ToLower();
                        break;
                }

            }

            switch (attachment.FileEncoding)
            {
                case "base64":
                    attachment.FileData = Convert.FromBase64String(bodyPart);
                    break;
                default:
                    break;
            }

            return attachment;
        }
    }
}
