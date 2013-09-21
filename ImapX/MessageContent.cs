using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ImapX.EncodingHelpers;
using ImapX.Parsing;

namespace ImapX
{

    public class MessageContent
    {
        public MessageContent()
        {
            ContentStream = string.Empty;
        }

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

        /// <summary>
        ///     Some messages contain attachments that are not described in the ContentDisposition,
        ///     but in the ContentStream directly. This method is used to convert a body part to
        ///     an attachment
        /// </summary>
        /// <remarks>
        ///     [27.07.2012] Fix by Pavel Azanov (coder13)
        /// </remarks>
        internal Attachment ToAttachment()
        {
            var rex = new Regex(@"([^:|^=]*)[:|=][\s]?(.*)[;]?");
            string[] tmp = ContentStream.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var attachment = new Attachment();
            string bodyPart = string.Empty;

            try
            {
                if (ContentType != null && ContentType.ToLower().Contains("message/rfc822"))
                    // [2013-04-24] naudelb(Len Naude) - Added
                {
                    // This part is an email attachment in mime(text) format that will be atached as an "eml" file
                    // The name of the file will be derived from the attachment's "Subject" line
                    // Remove leading blank line
                    ContentStream = ContentStream.TrimStart("\r\n".ToCharArray());
                    attachment.FileName = ParseHelper.GetRFC822FileName(ContentStream);
                    attachment.FileData = Encoding.UTF8.GetBytes(ContentStream);
                }
                else if (ContentType != null && ContentType.ToLower().Contains("message/delivery-status"))
                    // [2013-04-24] naudelb(Len Naude) - Added
                {
                    // Delivery failed notice atachment in mime(text) format
                    // Name will be hardcoded as "details.txt" as this is what outlook does
                    attachment.FileName = "details.txt";
                    attachment.FileData = Encoding.UTF8.GetBytes(ContentStream);
                }
                else
                {
                    for (int i = 0; i < tmp.Length && string.IsNullOrEmpty(bodyPart); i++)
                    {
                        if (tmp[i].StartsWith("--"))
                            continue;

                        string line = tmp[i].Trim('\t').Trim('\"').Trim().TrimEnd(';'); // [05/28/13] Fix by Woozer 

                        string[] parts = line.Contains(';') ? line.Split(';') : new[] {line};

                        foreach (Match match in parts.Select(part => rex.Match(part)))
                        {
                            if (!match.Success && parts.Length == 1)
                            {
                                bodyPart = string.Join("\r\n", tmp.Skip(i).ToArray());
                                break;
                            }
                            if (!match.Success)
                                continue;


                            string field = match.Groups[1].Value.ToLower().Trim();
                            string value = match.Groups[2].Value.Trim().Trim('"').TrimEnd(';');

                            switch (field)
                            {
                                case MessageProperty.CONTENT_TYPE:
                                    attachment.FileType = ParseHelper.ExtractFileType(value.ToLower());
                                    break;
                                case "name":
                                case "filename":
                                    attachment.FileName = StringDecoder.Decode(value.Trim('"').Trim('\''));
                                    break;
                                case MessageProperty.CONTENT_TRANSFER_ENCODING:
                                    attachment.FileEncoding = value.ToLower();
                                    break;
                            }
                        }
                    }


                    // [2013-04-24] naudelb(Len Naude) - The value might be mixed case
                    //switch (attachment.FileEncoding)
                    switch (
                        string.IsNullOrEmpty(attachment.FileEncoding) ? "7bit" : attachment.FileEncoding.ToLower())
                    {
                        case "base64":
                            attachment.FileData = Base64.FromBase64(bodyPart);
                            break;
                        case "7bit":
                            attachment.FileData = Encoding.UTF8.GetBytes(bodyPart);
                            break;
                        case "quoted-printable":
                            attachment.FileData =
                                Encoding.UTF8.GetBytes(StringDecoder.DecodeQuotedPrintable(bodyPart, Encoding.UTF8));
                            break;
                        default:
                            attachment.FileData = Encoding.UTF8.GetBytes(bodyPart);
                            break;
                    }
                }


                return attachment;
            }
            catch (FormatException ex)
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

            // [2013-04-24] naudelb(Len Naude) - Catch Exceptions
            try
            {
                inlineAttachment.FileEncoding = ContentTransferEncoding;
                inlineAttachment.FileType = ParseHelper.ExtractFileType(ContentType);
                inlineAttachment.FileName =
                    PartHeaders.FirstOrDefault(
                        _ => _.Key.ToLower() == "content-id" || _.Key.ToLower() == "x-attachment-id")
                        .Value.Replace("<", "")
                        .Replace(">", "");

                // [2013-04-24] naudelb(Len Naude) - Cater for mixed case and different encodings
                //switch (ContentTransferEncoding)
                //{
                //    case "base64":
                //        inlineAttachment.FileData = Base64.FromBase64(ContentStream);
                //        break;
                //}
                switch (
                    string.IsNullOrEmpty(inlineAttachment.FileEncoding)
                        ? "7bit"
                        : inlineAttachment.FileEncoding.ToLower())
                {
                    case "base64":
                        inlineAttachment.FileData = Base64.FromBase64(ContentStream);
                        break;
                    case "7bit":
                        inlineAttachment.FileData = Encoding.UTF8.GetBytes(ContentStream);
                        break;
                    case "quoted-printable":
                        inlineAttachment.FileData =
                            Encoding.UTF8.GetBytes(StringDecoder.DecodeQuotedPrintable(ContentStream, Encoding.UTF8));
                        break;
                    default:
                        inlineAttachment.FileData = Encoding.UTF8.GetBytes(ContentStream);
                        break;
                }
            }
            catch (Exception ex)
            {
                var str = new StringBuilder();
                str.Append("Error parsing inlineAttachment");
                str.Append(Environment.NewLine);
                str.AppendFormat("inlineAttachment filename: \"{0}\"", inlineAttachment.FileName);
                str.Append(Environment.NewLine);
                str.AppendFormat("Part body: \"{0}\"", ContentStream);
                throw new Exception(str.ToString(), ex);
            }

            return inlineAttachment;
        }
    }
}