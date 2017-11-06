using ImapX.Enums;
using System.Net.Mime;
using System.Text;

namespace ImapX.EncodingHelpers
{
    internal class BodyDecoder
    {
        public static string DecodeMessageContent(string value, ContentTransferEncoding contentTransferEncoding, string charset)
        {
            Encoding encoding = Encoding.UTF8;
            if (!string.IsNullOrWhiteSpace(charset))
            {
                try
                {
                    encoding = Encoding.GetEncoding(charset);
                }
                catch
                {
                }
            }

            switch (contentTransferEncoding)
            {
                case ContentTransferEncoding.Base64:
                    return StringDecoder.DecodeBase64(value, encoding);

                case ContentTransferEncoding.QuotedPrintable:
                    return StringDecoder.DecodeQuotedPrintable(value, encoding);

                default:
                    return value;
            }
        }
        
    }
}
