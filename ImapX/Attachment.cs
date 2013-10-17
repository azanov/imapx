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

        internal Attachment()
        {
        }

        internal Attachment(MessageContent content)
        {
            _content = content;
            _content.PropertyChanged += ContentOnPropertyChanged;
            ContentOnPropertyChanged(null, null);
        }

        private void ContentOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args == null || args.PropertyName == "ContentId")
                ContentId = _content.ContentId;

            if (args == null || args.PropertyName == "ContentTransferEncoding")
                ContentTransferEncoding = _content.ContentTransferEncoding;

            if (args == null || args.PropertyName == "ContentDisposition")
                FileName = string.IsNullOrEmpty(_content.ContentDisposition.FileName) ? "unnamed" : StringDecoder.Decode(_content.ContentDisposition.FileName);

            if (args != null && args.PropertyName != "ContentTransferEncoding") return;

            ContentTransferEncoding = _content.ContentTransferEncoding;

            if (!_content.Downloaded || _content.ContentStream == null)
                FileData = new byte[0];
            else
                switch (ContentTransferEncoding)
                {
                    case ContentTransferEncoding.Base64:
                        FileData = Base64.FromBase64(_content.ContentStream);
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
                        FileData = encoding.GetBytes(_content.ContentStream);
                        break;
                }
        }

        public string ContentId { set; get; }

        public byte[] FileData { get; set; }

        public string FileName { get; set; }

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

        public ContentType ContentType { get; set; }

        public ContentTransferEncoding ContentTransferEncoding { get; set; }

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

        #region Obsolete

        [Obsolete("GetStream might be removed in future releases")]
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