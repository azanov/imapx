using System;
using System.ComponentModel;
using System.IO;
using System.Net.Mime;
using System.Text;
using ImapX.EncodingHelpers;
using ImapX.Enums;

namespace ImapX
{

    public class Attachment
    {
        private readonly MessageContent _content;
        private string _fileName = null;
        private byte[] _fileData = null;

        public Attachment()
        {

        }

        internal Attachment(MessageContent content)
        {
            _content = content;
        }

        public string ContentId
        {
            get
            {
                return _content.ContentId;
            }
        }

        public byte[] FileData
        {
            get
            {
                if (_fileData == null)
                {
                    if (_content.Downloaded && _content.ContentStream != null)
                    {
                        switch (ContentTransferEncoding)
                        {
                            case ContentTransferEncoding.Base64:
                                _fileData = Base64.FromBase64(_content.ContentStream);
                                break;
                            default:
                                Encoding encoding = Encoding.UTF8;
                                try
                                {
                                    encoding = Encoding.GetEncoding(ContentType.CharSet);
                                }
                                catch
                                {
                                }

                                _fileData = encoding.GetBytes(_content.ContentStream);
                                break;
                        }
                    }
                }
                return _fileData;
            }
        }

        public string FileName
        {
            get
            {
                if (_fileName == null)
                {
                    if (string.IsNullOrEmpty(_content.ContentDisposition.FileName))
                    {
                        if (string.IsNullOrEmpty(_content.ContentType.Name))
                            _fileName = "unnamed";
                        else if (_content.ContentType.Parameters.ContainsKey("name"))
                            _fileName = _content.ContentType.Name;
                    }
                    else
                        _fileName = _content.ContentDisposition.FileName;

                    _fileName = StringDecoder.Decode(_fileName, true);
                }
                return _fileName;
            }
        }

        public bool Downloaded
        {
            get
            {
                return _content.Downloaded;
            }
        }

        public void Download()
        {
            if (!Downloaded)
                _content.Download();
        }

        public ContentType ContentType
        {
            get
            {
                return _content.ContentType;
            }
        }

        public ContentTransferEncoding ContentTransferEncoding
        {
            get
            {
                return _content.ContentTransferEncoding;
            }
        }

#if NETFX_CORE

        public void Save(string folder, string fileName = null)
        {
            //TODO
        }

#else
        public void Save(string folder, string fileName = null)
        {
            string path = Path.Combine(folder,
                string.IsNullOrEmpty(fileName) ? (string.IsNullOrEmpty(FileName) ? "unnamed.dat" : FileName) : fileName);

            using (
                var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                fileStream.Write(FileData, 0, FileData.Length);
                fileStream.Close();
            }

        }

#endif

        public long FileSize
        {
            get
            {
                return _content.Downloaded ? FileData.Length : _content.Size;
            }
        }

        public string GetTextData()
        {
            return _content.ContentStream;
        }

    }
}