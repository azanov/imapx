using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using ImapX.Constants;
using ImapX.EncodingHelpers;
using ImapX.Enums;
using ImapX.Extensions;
using ImapX.Parsing;

namespace ImapX
{

    public class MessageContent : CommandProcessor
    {
        private readonly ImapClient _client;
        private readonly Message _message;
        private MessageFetchState _fetchState;
        private MessageFetchState _fetchProgress;

        private static readonly Regex CommandEndRex = new Regex(@"IMAPX\d+ OK");
        private static readonly Regex CommandJunkUID = new Regex(@"UID \d+");
        private static readonly Regex CommandStartRex = new Regex(@"^\* \d+ FETCH \((UID \d+)?");

        internal MessageContent() { }

        public MessageContent(ImapClient client, Message message)
        {
            _client = client;
            _message = message;
            Parameters = new Dictionary<string, string>();
        }

        public string ContentId { get; set; }

        public string ContentNumber { get; set; }

        public ContentType ContentType { get; set; }

        public ContentTransferEncoding ContentTransferEncoding { get; set; }

        public ContentDisposition ContentDisposition { get; set; }

        public string Description { get; set; }

        public Dictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// The size of the message part
        /// </summary>
        public long Size { get; set; }

        public long Lines { get; set; }

        public string Md5 { get; set; }
        public string[] Language { get; set; }

        public string ContentStream { get; set; }

        private StringBuilder _contentBuilder;

        public Envelope Envelope { get; set; }

        public bool Downloaded
        {
            get { return _fetchProgress.HasFlag(MessageFetchState.Headers | MessageFetchState.Body); }
        }

        private void AppendDataToContentStream(string data)
        {
            switch (ContentTransferEncoding)
            {
                case ContentTransferEncoding.QuotedPrintable:
                    // Fix by danbert2000 - 5/8/14
                    if (data.EndsWith("="))
                    {
                        // Fix by kirchik
                        //_contentBuilder.Append(data.TrimEnd(new[] {' ', '='}));
                        _contentBuilder.Append(data.TrimEnd('='));
                    }
                    else if (string.IsNullOrEmpty(data))
                    {
                        _contentBuilder.Append("\r\n");
                    }
                    else
                    {
                        _contentBuilder.Append(data + "\r\n");
                    }
                    break;
                case ContentTransferEncoding.EightBit:
                case ContentTransferEncoding.SevenBit:
                    _contentBuilder.AppendLine(data);
                    //if (string.IsNullOrEmpty(data))
                    //    _contentBuilder.Append(Environment.NewLine);
                    //else
                    //    _contentBuilder.Append(data + " ");
                    break;
                default:
                    _contentBuilder.Append(data);
                    break;
            }
        }

        private static string CleanData(string value, int startIndex)
        {
            var endIndex = value.IndexOf('}', startIndex);

            return endIndex == -1 ? value : value.Remove(startIndex, endIndex - startIndex + 1);
        }

        public override void ProcessCommandResult(string data)
        {

            int index, index2;

            if ((index = data.IndexOf("BODY[" + ContentNumber + ".MIME]", StringComparison.Ordinal)) != -1)
            {
                if ((index2 = data.IndexOf("BODY[" + ContentNumber + ".MIME] NIL", StringComparison.Ordinal)) != -1)
                {
                    data = data.Replace("BODY[" + ContentNumber + ".MIME] NIL", "");
                }
                else
                {
                    data = CleanData(data, index).Trim();

                    if (data.StartsWith("*") && data.Contains("FETCH"))
                        data = CommandStartRex.Replace(data, "");

                    if (!string.IsNullOrEmpty(data))
                        AppendDataToContentStream(data);
                    _fetchState = MessageFetchState.Headers;
                    _fetchProgress = _fetchProgress | MessageFetchState.Headers;
                    return;
                }
            }

            if ((index = data.IndexOf("BODY[" + ContentNumber + "]", StringComparison.Ordinal)) != -1)
            {

                data = CleanData(data, index).Trim();

                if (data.StartsWith("*") && (data.Contains("UID") || data.Contains("FETCH")))
                    data = CommandStartRex.Replace(data, "");

                if (!string.IsNullOrEmpty(data))
                    AppendDataToContentStream(data);

                _fetchState = MessageFetchState.Body;
                _fetchProgress = _fetchProgress | MessageFetchState.Body;
                return;
            }




            if (_fetchState == MessageFetchState.Headers)
            {

                try
                {
                    Match headerMatch = Expressions.HeaderParseRex.Match(data);
                    if (!headerMatch.Success) return;

                    string key = headerMatch.Groups[1].Value.ToLower();
                    string value = headerMatch.Groups[2].Value;

                    if (this.ContentType != null && this.ContentType.MediaType == "message/rfc822")
                        _contentBuilder.AppendLine(data);

                    if (Parameters.ContainsKey(key))
                        Parameters[key] = value;
                    else
                        Parameters.Add(key, value);

                    switch (key)
                    {
                        case "content-type":
                            if (ContentType == null)
                                ContentType = HeaderFieldParser.ParseContentType(value);

                            if (!string.IsNullOrEmpty(ContentType.Name))
                            {
                                ContentType.Name = StringDecoder.Decode(ContentType.Name, true);
                                if (ContentDisposition == null)
                                    ContentDisposition = new ContentDisposition
                                    {
                                        DispositionType = DispositionTypeNames.Attachment
                                    };
                                ContentDisposition.FileName = ContentType.Name;

                            }

                            break;
                        case "charset":
                            if (ContentType == null)
                                ContentType = new ContentType();
                            ContentType.CharSet = value;
                            break;
                        case "filename":
                        case "name":

                            value = StringDecoder.Decode(value, true);

                            if (ContentType == null)
                                ContentType = new ContentType();

                            if (ContentDisposition == null)
                                ContentDisposition = new ContentDisposition();

                            ContentDisposition.FileName = value;

                            if (string.IsNullOrEmpty(ContentDisposition.DispositionType) && string.IsNullOrEmpty(ContentId))
                                ContentDisposition.DispositionType = DispositionTypeNames.Attachment;

                            ContentType.Name = value;
                            break;
                        case "content-id":
                            if (ContentDisposition == null)
                                ContentDisposition = new ContentDisposition();

                            ContentDisposition.DispositionType = DispositionTypeNames.Inline;

                            ContentId = value.Trim(' ', '<', '>');
                            break;
                        case "content-disposition":
                            if (ContentDisposition == null)
                                ContentDisposition = new ContentDisposition(value);

                            if (!string.IsNullOrEmpty(ContentId))
                                ContentDisposition.DispositionType = DispositionTypeNames.Inline;

                            break;
                        case "content-transfer-encoding":
                            ContentTransferEncoding = value.ToContentTransferEncoding();
                            break;
                    }
                }
                catch
                {

                }
            }
            else if (CommandEndRex.IsMatch(data))
            {
                for (var i = _contentBuilder.Length - 1; i >= 0; i--)
                {
                    if (_contentBuilder[i] == ')')
                    {
                        _contentBuilder.Remove(i, _contentBuilder.Length - i);
                        return;
                    }
                }
            }
            else if ((index = data.IndexOf("UID")) != -1)
            {
                data = CommandJunkUID.Replace(data, "");
                AppendDataToContentStream(data);
                return;
            }
            else
                AppendDataToContentStream(data);
        }

        public bool Download()
        {
            if (Downloaded)
                return true;

            //ContentStream = new MemoryStream();
            // _writer = new StreamWriter(ContentStream);

            Encoding encoding = null;
            if (ContentType != null && ContentType.CharSet != null)
                try
                {
                    encoding = Encoding.GetEncoding(ContentType.CharSet);
                }
                catch
                {
                }
            else if (_message.ContentType != null && _message.ContentType.CharSet != null)
            {
                try
                {
                    encoding = Encoding.GetEncoding(_message.ContentType.CharSet);
                }
                catch
                {
                }
            }

            IList<string> data = new List<string>();

            _contentBuilder = new StringBuilder();

            bool result =
                _client.SendAndReceive(
                    string.Format(ImapCommands.Fetch, _message.UId,
                        string.Format("BODY.PEEK[{0}.MIME] BODY.PEEK[{0}]", ContentNumber)), ref data,
                    this, encoding);

            //_writer.Flush();

            _fetchProgress = _fetchProgress | MessageFetchState.Body | MessageFetchState.Headers;

            ContentStream = _contentBuilder.ToString();

            if (ContentTransferEncoding == ContentTransferEncoding.QuotedPrintable && !string.IsNullOrEmpty(ContentStream))
                ContentStream = StringDecoder.DecodeQuotedPrintable(ContentStream, encoding);

            return result;
        }

        internal void AppendEml(ref StringBuilder sb, bool addHeaders)
        {
            if (addHeaders)
            {
                foreach (var header in Parameters)
                    sb.AppendLine(string.Format("{0}: {1}", header.Key, header.Value));
            }

            sb.AppendLine();

            sb.Append(ContentStream);

            sb.AppendLine();
        }

    }
}