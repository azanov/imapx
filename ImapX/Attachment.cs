using System;
using System.IO;

namespace ImapX
{

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
            set { _fileName = string.IsNullOrEmpty(value) ? "Unnamed" : value; }
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
            // [2013-04-26] naudelb(Len Naude) - Will throw an exception if the file name is null
            // This happens in the case of delivery failed notification i.e. "Mail delivery failed: returning message to sender"
            // and the body of containt the word "attachment"
            if (!string.IsNullOrEmpty(FileName))
            {
                using (
                    var fileStream = new FileStream(Path.Combine(folderPath, FileName), FileMode.Create,
                        FileAccess.Write))
                {
                    fileStream.Write(_fileData, 0, _fileData.Length);
                    fileStream.Close();
                }
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