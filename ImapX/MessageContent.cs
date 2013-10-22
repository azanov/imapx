using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public class MessageContent : CommandProcessor, INotifyPropertyChanged
    {
        private readonly ImapClient _client;
        private readonly Message _message;
        private MessageFetchState _fetchState;
        private MessageFetchState _fetchProgress;
        private ContentType _contentType;
        private ContentTransferEncoding _contentTransferEncoding;
        private string _contentStream;
        private string _contentId;
        private ContentDisposition _contentDisposition;

        private static readonly Regex MimeRex = new Regex(@".*BODY\[[\d\.]+MIME\] \{\d+\}");
        private static readonly Regex BodyRex = new Regex(@".*BODY\[[\d\.]+\] \{\d+\}");
        private static readonly Regex CommandEndRex = new Regex(@"IMAPX\d+ OK");

        internal MessageContent(){}

        public MessageContent(ImapClient client, Message message)
        {
            _client = client;
            _message = message;
            Parameters = new Dictionary<string, string>();
        }

        public string ContentId
        {
            get { return _contentId; }
            set
            {
                _contentId = value;
                OnPropertyChanged("ContentId");
            }
        }

        public string ContentNumber { get; set; }

        public ContentType ContentType
        {
            get { return _contentType; }
            set
            {
                _contentType = value;
                OnPropertyChanged("ContentType");
            }
        }

        public ContentTransferEncoding ContentTransferEncoding
        {
            get { return _contentTransferEncoding; }
            set
            {
                _contentTransferEncoding = value;
                OnPropertyChanged("ContentTransferEncoding");
            }
        }

        public ContentDisposition ContentDisposition
        {
            get { return _contentDisposition; }
            set
            {
                _contentDisposition = value;
                OnPropertyChanged("ContentDisposition");
            }
        }

        public string Description { get; set; }

        public Dictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// The size of the message part
        /// </summary>
        public long Size { get; set; }

        public string Md5 { get; set; }
        public string Language { get; set; }

        public string ContentStream
        {
            get { return _contentStream; }
            set
            {
                _contentStream = value;
                OnPropertyChanged("ContentStream");
            }
        }

        public bool Downloaded
        {
            get { return _fetchProgress.HasFlag(MessageFetchState.Headers | MessageFetchState.Body); }
        }

        public override void ProcessCommandResult(string data)
        {



            if (MimeRex.IsMatch(data))
            {
                data = MimeRex.Replace(data, "").Trim();
                //_writer.Write(data);

                ContentStream += ContentTransferEncoding == ContentTransferEncoding.QuotedPrintable
                    ? data.TrimEnd(new[] { ' ', '=' })
                    : data;
                _fetchState = MessageFetchState.Headers;
                _fetchProgress = _fetchProgress | MessageFetchState.Headers;
                return;
            }

            if (BodyRex.IsMatch(data))
            {
                data = BodyRex.Replace(data, "").Trim();

                ContentStream += ContentTransferEncoding == ContentTransferEncoding.QuotedPrintable
                    ? data.TrimEnd(new[] { ' ', '=' })
                    : data;

                _fetchState = MessageFetchState.Body;
                _fetchProgress = _fetchProgress | MessageFetchState.Body;
                return;
            }

            


            if (_fetchState == MessageFetchState.Headers)
            {

                Match headerMatch = Expressions.HeaderParseRex.Match(data);
                if (!headerMatch.Success) return;

                string key = headerMatch.Groups[1].Value.ToLower();
                string value = headerMatch.Groups[2].Value;

                if (Parameters.ContainsKey(key))
                    Parameters[key] = value;
                else
                    Parameters.Add(key, value);

                switch (key)
                {
                    case "content-type":
                        if (ContentType == null)
                            ContentType = new ContentType(value);
                        break;
                    case "charset":
                        if (ContentType == null)
                            ContentType = new ContentType();
                        ContentType.CharSet = value;
                        break;
                    case "filename":
                    case "name":

                        value = StringDecoder.Decode(value);

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
            else if (CommandEndRex.IsMatch(data))
            {
                if(ContentStream.EndsWith(")"))
                    ContentStream = ContentStream.Substring(0, ContentStream.Length - 1);
            }
            else
            {
                data = ContentTransferEncoding == ContentTransferEncoding.QuotedPrintable
                    ? data.TrimEnd(new[] { ' ', '=' })
                    : data;
                ContentStream += data.Length == 0 ? Environment.NewLine : data;
            }
        }

        public bool Download()
        {
            //ContentStream = new MemoryStream();
            // _writer = new StreamWriter(ContentStream);

            Encoding encoding = null;
            try
            {
                encoding = Encoding.GetEncoding(ContentType.CharSet);
            }
            catch
            {
            }

            IList<string> data = new List<string>();
            bool result =
                _client.SendAndReceive(
                    string.Format(ImapCommands.Fetch, _message.UId,
                        string.Format("BODY.PEEK[{0}.MIME] BODY.PEEK[{0}]", ContentNumber)), ref data,
                    this, encoding);

            //_writer.Flush();

            OnPropertyChanged("all");

            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
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