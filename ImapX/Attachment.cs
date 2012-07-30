using System;
using System.IO;

namespace ImapX
{
    public class Attachment
    {
        private byte[] _fileData;
        private string _fileName;
        private int _fileSize;
        private string _fileEncoding;
        private string _fileType;

        public int FileSize
        {
            get
            {
                return this._fileData == null ? 0 : this._fileData.Length;
            }
        }

        public string FileName
        {
            get
            {
                return this._fileName;
            }
            set
            {
                this._fileName = value;
            }
        }

        public string FileEncoding
        {
            get
            {
                return this._fileEncoding;
            }
            set
            {
                this._fileEncoding = value;
            }
        }

        public string FileType
        {
            get
            {
                return this._fileType;
            }
            set
            {
                this._fileType = value;
            }
        }

        public byte[] FileData
        {
            get
            {
                return this._fileData;
            }
            set
            {
                this._fileData = value;
            }
        }

        public void SaveFile(string downloadLocation)
        {
            FileStream fileStream = new FileStream(downloadLocation + this._fileName, FileMode.Create, FileAccess.Write);
            fileStream.Write(this._fileData, 0, this._fileData.Length);
            fileStream.Close();
        }

        public string GetStream()
        {
            byte[] array;
            using (FileStream fileStream = new FileStream(this._fileName, FileMode.Open, FileAccess.Read))
            {
                array = new byte[(int)((object)((IntPtr)fileStream.Length))];
                fileStream.Read(array, 0, (int)fileStream.Length);
            }
            return Convert.ToBase64String(array);
        }
    }
}
