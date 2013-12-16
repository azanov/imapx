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
                    _fileName = string.IsNullOrEmpty(_content.ContentDisposition.FileName)
                        ? "unnamed"
                        : StringDecoder.Decode(_content.ContentDisposition.FileName, true);
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

        #region Obsolete

        [Obsolete("GetStream is obsolete, please use GetTextData instead", true)]
        public string GetStream()
        {
            return _content.ContentStream;
        }

        [Obsolete("SaveFile is obsolete. Please use Save instead.", true)]
        public void SaveFile(string folderPath)
        {
        }

        [Obsolete("FileType is obsolete. Please use ContentType instead.", true)]
        public string FileType
        {
            get { return null; }
        }

        [Obsolete("FileEncoding is obsolete. Please use ContentType.Charset and ContentTransferEncoding instead.", true)]
        public string FileEncoding
        {
            get { return null; }
        }

        #endregion
    }
}