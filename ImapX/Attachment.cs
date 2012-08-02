using System;
using System.IO;

namespace ImapX
{
    public class Attachment
    {
        private byte[] _fileData;

        public int FileSize
        {
            get { return _fileData == null ? 0 : _fileData.Length; }
        }

        public string FileName { get; set; }

        public string FileEncoding { get; set; }

        public string FileType { get; set; }

        public byte[] FileData
        {
            get { return _fileData; }
            set { _fileData = value; }
        }

        public void SaveFile(string downloadLocation)
        {
            using (var fileStream = new FileStream(downloadLocation + FileName, FileMode.Create, FileAccess.Write))
            {
                fileStream.Write(_fileData, 0, _fileData.Length);
                fileStream.Close();
            }
        }

        public string GetStream()
        {
            byte[] array;
            using (var fileStream = new FileStream(FileName, FileMode.Open, FileAccess.Read))
            {
                array = new byte[(int) ((object) ((IntPtr) fileStream.Length))];
                fileStream.Read(array, 0, (int) fileStream.Length);
            }
            return Convert.ToBase64String(array);
        }
    }
}