using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace ImapX
{
    public class MessageContent
    {
        private string _contentStream;

    	public string BoundaryName { get; set; }

    	public Dictionary<string, string> PartHeaders { get; set; }

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

    	public string ContentDescription { get; set; }

    	public string MIMEVersion { get; set; }

    	public string ContentFilename { get; set; }

    	public string ContentDisposition { get; set; }

    	public string ContentId { get; set; }

    	public string PartID { get; set; }

    	public string TextData { get; set; }

    	public byte[] BinaryData { get; set; }

    	public string ContentType { get; set; }

    	public string ContentTransferEncoding { get; set; }

    	public int ContentSize { get; set; }

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
            }

            return attachment;
        }
    }
}
