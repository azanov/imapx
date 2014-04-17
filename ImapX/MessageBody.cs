using System;
using ImapX.EncodingHelpers;
using ImapX.Enums;
using ImapX.Parsing;
using ImapX.Extensions;

namespace ImapX
{
    public class MessageBody
    {
        private readonly ImapClient _client;
        private readonly MessageContent _htmlContent;
        private readonly MessageContent _textContent;

        private string _decodedHtml;
        private string _decodedText;

        internal MessageBody()
        {
        }

        internal MessageBody(ImapClient client, MessageContent textContent, MessageContent htmlContent)
        {
            _client = client;
            _textContent = textContent;
            _htmlContent = htmlContent;
        }

        public BodyType Downloaded
        {
            get
            {
                var r = BodyType.None;
                if (HasHtml && _htmlContent.Downloaded)
                    r |= BodyType.Html;
                if (HasText && _textContent.Downloaded)
                    r |= BodyType.Html;
                return r;
            }
        }

        public bool HasHtml
        {
            get { return _htmlContent != null; }
        }

        public bool HasText
        {
            get { return _textContent != null; }
        }

        public string Html
        {
            get
            {
                if (_htmlContent == null && _textContent == null)
                    return string.Empty;

                if (_client.Behavior.AutoGenerateMissingBody && HasText)
                {
                    _decodedHtml = Text.Replace(Environment.NewLine, "<br />");
                    return _decodedHtml;
                }

                if (_htmlContent == null)
                    return string.Empty;

                if (!_htmlContent.Downloaded && _client.Behavior.AutoDownloadBodyOnAccess)
                    _htmlContent.Download();

                _decodedHtml = _htmlContent.ContentTransferEncoding == ContentTransferEncoding.QuotedPrintable ? _htmlContent.ContentStream : BodyDecoder.DecodeMessageContent(_htmlContent);

                return _decodedHtml ?? string.Empty;
            }
        }

        public string Text
        {
            get
            {
                if (_textContent == null && _htmlContent == null)
                    return string.Empty;

                if (_client.Behavior.AutoGenerateMissingBody && HasHtml)
                {
                    _decodedText =
                        Expressions.HtmlTagFilterRex.Replace(
                            Expressions.BrTagFilterRex.Replace(Html, Environment.NewLine), string.Empty);
                    return _decodedText;
                }
                
                if (_textContent == null)
                    return string.Empty;

                if (!_textContent.Downloaded && _client.Behavior.AutoDownloadBodyOnAccess)
                    _textContent.Download();

                _decodedText = _textContent.ContentTransferEncoding == ContentTransferEncoding.QuotedPrintable ? _textContent.ContentStream : BodyDecoder.DecodeMessageContent(_textContent);

                return _decodedText ?? string.Empty;
            }
        }

        public void Download(BodyType type = BodyType.TextAndHtml)
        {
            if (HasHtml && type.HasFlag(BodyType.Html))
                _htmlContent.Download();

            if (HasText && type.HasFlag(BodyType.Text))
                _textContent.Download();
        }
    }
}