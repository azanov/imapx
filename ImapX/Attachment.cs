using ImapX.EncodingHelpers;
using ImapX.Enums;
using ImapX.Extensions;
using System.IO;
using System.Net.Mime;

namespace ImapX
{
    public class Attachment
    {
        private readonly MessageContent _content;
        private string _fileName = null;

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
                return _content.BinaryData;

            }
        }

        public string FileName
        {
            get
            {
                if (_fileName == null)
                {
                    if (_content.ContentDisposition != null)
                        if (!string.IsNullOrWhiteSpace(_content.ContentDisposition.FileName))
                            _fileName = _content.ContentDisposition.FileName;

                    if (string.IsNullOrWhiteSpace(_fileName) && _content.ContentType!=null)
                    {
                        _fileName = _content.ContentType.Name;

                        if (string.IsNullOrWhiteSpace(_fileName) && _content.ContentType.Parameters != null)
                        {
                            if (string.IsNullOrWhiteSpace(_fileName) && _content.ContentType.Parameters.ContainsKey("name"))
                                _fileName = _content.ContentType.Parameters["name"];

                            if (string.IsNullOrWhiteSpace(_fileName) && _content.ContentType.Parameters.ContainsKey("filename"))
                                _fileName = _content.ContentType.Parameters["filename"];
                        }
                    }

                    if (string.IsNullOrWhiteSpace(_fileName) && _content.Parameters != null)
                    {
                        if (string.IsNullOrWhiteSpace(_fileName) && _content.Parameters.ContainsKey("name"))
                            _fileName = _content.Parameters["name"];

                        if (string.IsNullOrWhiteSpace(_fileName) && _content.Parameters.ContainsKey("filename"))
                            _fileName = _content.Parameters["filename"];

                    }

                    if (string.IsNullOrWhiteSpace(_fileName))
                        _fileName = (_content.ContentId ?? "").Replace(Path.GetInvalidFileNameChars(), "");

                    if (string.IsNullOrWhiteSpace(_fileName))
                        _fileName = "unnamed";

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

    }
}