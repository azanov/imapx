using System;
using System.IO;

namespace ImapX
{
    [Serializable]
    public class Attachment
    {
        private byte[] _fileData;
        private string _fileName;

        public int FileSize
        {
            get { return _fileData == null ? 0 : _fileData.Length; }
        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName =string.IsNullOrEmpty(value) ? "Unnamed" : value; }
        }

        public string FileEncoding { get; set; }

        public string FileType { get; set; }

        public byte[] FileData
        {
            get { return _fileData; }
            set { _fileData = value; }
        }

        public void SaveFile(string folderPath)
        {
            using (var fileStream = new FileStream(Path.Combine(folderPath, FileName), FileMode.Create, FileAccess.Write))
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
            return Base64.ToBase64(array);
        }
    }
}