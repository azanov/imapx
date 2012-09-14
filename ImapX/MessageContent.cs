using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace ImapX
{
    public class MessageContent
    {
        public string BoundaryName { get; set; }

    	public Dictionary<string, string> PartHeaders { get; set; }

        public string ContentStream { get; set; }

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
            ContentStream = string.Empty;
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
            
            var rex = new Regex(@"([^:|^=]*)[:|=][\s]?(.*)[;]?");
            var tmp = ContentStream.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var attachment = new Attachment();
            var bodyPart = string.Empty;

            try
            {
               

               

                for (var i = 0; i < tmp.Length && string.IsNullOrWhiteSpace(bodyPart); i++)
                {

                    if (tmp[i].StartsWith("--"))
                        continue;

                    var line = tmp[i].Trim('\t').Trim().TrimEnd(';');

                    var parts = line.Contains(';') ? line.Split(';') : new[] {line};

                    foreach (var match in parts.Select(part => rex.Match(part)))
                    {
                        if (!match.Success && parts.Length == 1)
                        {
                            bodyPart = string.Join("\r\n", tmp.Skip(i));
                            break;
                        }
                        if (!match.Success)
                            continue;


                        var field = match.Groups[1].Value.ToLower().Trim();
                        var value = match.Groups[2].Value.Trim().Trim('"').TrimEnd(';');

                        switch (field)
                        {
                            case MessageProperty.CONTENT_TYPE:
                                attachment.FileType = ParseHelper.ExtractFileType(value.ToLower());
                                break;
                            case "name":
                            case "filename":
                                attachment.FileName = ParseHelper.DecodeName(value.Trim('"').Trim('\''));
                                break;
                            case MessageProperty.CONTENT_TRANSFER_ENCODING:
                                attachment.FileEncoding = value.ToLower();
                                break;
                        }
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
            catch(FormatException ex)
            {
                var str = new StringBuilder();
                str.Append("Error parsing attachment");
                str.Append(Environment.NewLine);
                str.AppendFormat("Attachment filename: \"{0}\"", attachment.FileName);
                str.Append(Environment.NewLine);
                str.AppendFormat("Part body: \"{0}\"", bodyPart);
                throw new Exception(str.ToString(), ex);
            }
        }

        internal InlineAttachment ToInlineAttachment()
        {
            var rex = new Regex(@"([^:|^=]*)[:|=][\s]?(.*)[;]?");
            var inlineAttachment = new InlineAttachment();

            inlineAttachment.FileEncoding = ContentTransferEncoding;

            inlineAttachment.FileType = ParseHelper.ExtractFileType(ContentType);

            inlineAttachment.FileName =
                PartHeaders.FirstOrDefault(_ => _.Key.ToLower() == "content-id" || _.Key.ToLower() == "x-attachment-id").Value.Replace("<", "").Replace(">", "");
            
            switch (ContentTransferEncoding)
            {
                case "base64":
                    inlineAttachment.FileData = Convert.FromBase64String(ContentStream);
                    
                    break;
            }

            return inlineAttachment;
        }
    }
}
