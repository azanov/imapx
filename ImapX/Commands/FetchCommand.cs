using ImapX.Constants;
using ImapX.EncodingHelpers;
using ImapX.Enums;
using ImapX.Exceptions;
using ImapX.Extensions;
using ImapX.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;

namespace ImapX.Commands
{
    public class FetchCommand : MessageCommand
    {
        public FetchCommand(ImapClient client, long id, Message message,
            MessageFetchMode mode = MessageFetchMode.ClientDefault,
            bool reloadHeaders = false, string bodyPartNumber = null) : base(client, id, message, string.Empty)
        {
            if (mode == MessageFetchMode.ClientDefault)
                mode = Client.Behavior.MessageFetchMode;

            var sb = new StringBuilder();

            if (Client.Capabilities.UIdPlus)
                sb.AppendFormat("UID FETCH {0} ", message.UId);
            else
                sb.AppendFormat("UID FETCH {0} ", message.SequenceNumber);

            sb.Append("(");

            if (mode.HasFlag(MessageFetchMode.Flags))
                sb.Append("FLAGS ");

            if (mode.HasFlag(MessageFetchMode.InternalDate))
                sb.Append("INTERNALDATE ");

            if (mode.HasFlag(MessageFetchMode.Size))
                sb.Append("RFC822.SIZE ");

            if (mode.HasFlag(MessageFetchMode.Headers))
            {
                if (Client.Behavior.RequestedHeaders == null || Client.Behavior.RequestedHeaders.Length == 0)
                    sb.Append("BODY.PEEK[HEADER] ");
                else
                {
                    var headers = message.DownloadProgress.HasFlag(MessageFetchMode.Envelope) || mode.HasFlag(MessageFetchMode.Envelope) ?
                        Client.Behavior.RequestedHeaders.Except(MessageHeaderSets.Envelope).ToArray() : Client.Behavior.RequestedHeaders;

                    if (headers.Length > 0)
                        sb.Append("BODY.PEEK[HEADER.FIELDS (" + string.Join(" ", headers.Select(_ => _.ToUpper())) + ")] ");
                }
            }

            if (mode.HasFlag(MessageFetchMode.Envelope))
                sb.Append("ENVELOPE ");

            if (mode.HasFlag(MessageFetchMode.BodyStructure))
                sb.Append("BODYSTRUCTURE ");

            if (mode.HasFlag(MessageFetchMode.BodyPart))
            {
                sb.AppendFormat("BODY.PEEK[{0}.MIME] BODY.PEEK[{0}]", bodyPartNumber);
            }

            if (mode.HasFlag(MessageFetchMode.BodyText))
            {
                sb.AppendFormat("BODY.PEEK[TEXT]");
            }

            Parts[0] += sb.ToString().Trim() + ")\r\n";

            Message.InProgress = mode;
        }

        internal FetchCommand(ImapClient client, long id, Message message, string code) : base(client, id, message, code)
        {
            
        }

        private void HandleUId(ImapParser io)
        {
            var uid = io.ReadLong();
        }

        private void HandleFlags(ImapParser io)
        {
            ImapToken token;

            Message.Flags.ClearInternal();

            io.ConsumeOpeningParenthesis();

            while ((token = io.ReadToken()).Type == TokenType.Flag || token.Type == TokenType.Atom)
                Message.Flags.AddInternal(token.Value);

            if (token.Type != TokenType.ClosingParenthesis)
                throw new UnexpectedTokenException();

            Message.DownloadProgress |= MessageFetchMode.Flags;
        }

        private void HandleInternalDate(ImapParser io)
        {
            Message.InternalDate = DateTimeExtensions.ParseDate(io.ReadString());
            Message.DownloadProgress |= MessageFetchMode.InternalDate;
        }

        private void HandleSize(ImapParser io)
        {
            Message.Size = io.ReadLong();
            Message.DownloadProgress |= MessageFetchMode.Size;
        }

        private Envelope HandleEnvelope(ImapParser io)
        {
            var result = new Envelope();

            io.ConsumeOpeningParenthesis();

            result.Date = DateTimeExtensions.ParseDate(io.ReadNullableString());
            result.Subject = StringDecoder.Decode(io.ReadNullableString(), true);
            result.From = io.ReadAddressList().FirstOrDefault();
            result.Sender = io.ReadAddressList().FirstOrDefault();
            result.ReplyTo = io.ReadAddressList();
            result.To = io.ReadAddressList();
            result.Cc = io.ReadAddressList();
            result.Bcc = io.ReadAddressList();
            result.InReplyTo = io.ReadNullableString();
            result.MessageId = io.ReadNullableString();

            io.ConsumeClosingParenthesis();

            return result;
        }

        private void HandleHeaderBuffer(string[] headerBuffer, string sectionPart = null)
        {
            MessageContent bodyPart = null;
            string headerName = "", headerValue = "";
            if (sectionPart != null)
            {
                bodyPart = Message.BodyParts.FirstOrDefault(_ => _.ContentNumber == sectionPart);
                if (bodyPart == null)
                    return; // there's no body part with the given number, skip processing for now..
            }

            foreach (var value in headerBuffer)
            {
                var i = value.IndexOf(':');
                if (i == -1)
                    headerValue += value;
                else
                {
                    if (!string.IsNullOrEmpty(headerName))
                    {
                        if (bodyPart != null)
                            bodyPart.AddHeaderInternal(headerName.ToLower(), headerValue);
                        else
                            Message.AddHeaderInternal(headerName.ToLower(), headerValue);
                    }
                    headerName = value.Substring(0, i);
                    headerValue = value.Substring(i + 1);
                }
            }
        }

        private void HandleHeaders(ImapParser io, string sectionPart = null)
        {
            if (sectionPart != null)
                throw new TodoException("Fetching of headers for specific parts needs to be implemented");

            ImapToken token = io.PeekToken();
            if (token.Type == TokenType.Atom)
                io.ConsumeAtom();

            // header-list
            io.ConsumeOpeningParenthesis();
            while (io.PeekToken().Type == TokenType.Atom)
                io.ReadToken();
            io.ConsumeClosingParenthesis();

            io.ConsumeClosingBracket();

            var headerBuffer = (io.ReadNullableString() ?? string.Empty).Trim('\r', '\n', ' ').Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            HandleHeaderBuffer(headerBuffer, sectionPart);

            if (string.IsNullOrWhiteSpace(sectionPart))
                Message.DownloadProgress |= MessageFetchMode.Headers;
        }

        private void HandleBodySection(ImapParser io)
        {
            /* "BODY" section ["<" number ">"] SP nstring
                         
                    section = "[" [section-spec] "]"

                        section-spec    = section-msgtext / (section-part ["." section-text])
                        section-msgtext = "HEADER" / "HEADER.FIELDS" [".NOT"] SP header-list / "TEXT"

                            section-part    = nz-number *("." nz-number)

            */


            io.ConsumeOpeningBracket();

            ImapToken token = io.ReadToken();

            var isPart = char.IsDigit(token.Value[0]);
            var hasText = !char.IsDigit(token.Value.Last());
            var sectionPart = isPart ? (hasText ? token.Value.Substring(0, token.Value.LastIndexOf('.')) : token.Value) : null;
            var sectionText = isPart ? (hasText ?
                token.Value.Substring(token.Value.LastIndexOf('.') + 1) : "") : token.Value;

            switch (sectionText)
            {
                case "HEADER":
                case "HEADER.FIELDS":
                case "HEADER.FIELDS.NOT":
                    HandleHeaders(io, sectionPart);
                    break;
                case "TEXT":
                    throw new TodoException("Fetching of TEXT for the whole message or for a specific part needs to be implemented");
                case "MIME":
                    io.ConsumeClosingBracket();
                    var headerBuffer = (io.ReadNullableString() ?? string.Empty).Trim('\r', '\n', ' ').Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    HandleHeaderBuffer(headerBuffer, sectionPart);
                    break;
                default:
                    io.ConsumeClosingBracket();
                    var bodyPart = Message.BodyParts.FirstOrDefault(_ => _.ContentNumber == sectionPart);

                    var data = io.ReadNullableStringToken(
                        bodyPart.ContentDisposition != null || 
                        !string.IsNullOrWhiteSpace(bodyPart.ContentId) ||
                        bodyPart.ContentTransferEncoding == ContentTransferEncoding.Base64
                    );
                    
                    if (bodyPart != null)
                    {
                        bodyPart.ContentStream =
                            BodyDecoder.DecodeMessageContent(data.Value, bodyPart.ContentTransferEncoding, bodyPart.ContentType?.CharSet);

                        switch(bodyPart.ContentTransferEncoding)
                        {
                            case ContentTransferEncoding.Base64:
                                if (data.Data != null)
                                    bodyPart.BinaryData = Base64.FromBase64(data.Data);
                                if (bodyPart.ContentDisposition == null && string.IsNullOrWhiteSpace(bodyPart.ContentStream))
                                    bodyPart.ContentStream = Encoding.UTF8.GetString(bodyPart.BinaryData);
                                break;

                            case ContentTransferEncoding.QuotedPrintable:
                                if (data.Data != null && data.Data.Length > 0)
                                {
                                    bodyPart.ContentStream = BodyDecoder.DecodeMessageContent(Encoding.UTF8.GetString(data.Data), 
                                        ContentTransferEncoding.QuotedPrintable, bodyPart.ContentType?.CharSet);
                                    bodyPart.BinaryData = Encoding.UTF8.GetBytes(bodyPart.ContentStream);
                                }
                                break;

                            default:
                                bodyPart.BinaryData = data.Data;
                                break;
                        }
                        
                        bodyPart.Downloaded = true;
                    }
                    break;
            }

        }

        private List<MessageContent> HandleMultiPartBody(ImapParser io, string contentNumber = "")
        {
            /*
                body-type-mpart = 1*body SP media-subtype [SP body-ext-mpart]

                    body-ext-mpart  = body-fld-param [SP body-fld-dsp [SP body-fld-lang [SP body-fld-loc *(SP body-extension)]]]
            */

            var result = new List<MessageContent>();

            if (!string.IsNullOrWhiteSpace(contentNumber))
                contentNumber += ".";

            ImapToken token;
            var contentIndex = 1;
            while ((token = io.PeekToken()).Type == TokenType.OpeningParenthesis)
            {
                result.AddRange(HandleBody(io, contentNumber + contentIndex));
                contentIndex++;
            }

            var mediaSubType = io.ReadString();

            token = io.PeekToken();

            if (token.Type != TokenType.ClosingParenthesis)
            {
                var parameters = io.ReadDictionary(true, false);
                token = io.PeekToken();

                if (token.Type != TokenType.ClosingParenthesis)
                {
                    // body-fld-dsp
                    if (token.Type == TokenType.OpeningParenthesis)
                    {
                        io.ConsumeOpeningParenthesis();
                        io.ReadString();
                        io.ReadDictionary(true, false);
                        io.ConsumeClosingParenthesis();
                    }
                    else if (token.Type == TokenType.Nil)
                        io.ConsumeToken(TokenType.Nil);

                    token = io.PeekToken();
                    if (token.Type != TokenType.ClosingParenthesis)
                    {
                        // body-fld-lang
                        if (token.Type == TokenType.OpeningParenthesis)
                            io.ReadStringList();
                        else
                            io.ReadNullableString();

                        token = io.PeekToken();
                        if (token.Type != TokenType.ClosingParenthesis)
                        {
                            // body-fld-loc
                            io.ReadNullableString();

                            token = io.PeekToken();
                            if (token.Type != TokenType.ClosingParenthesis)
                                HandleBodyExtension(io);
                        }
                    }
                }
            }

            return result;
        }

        private MessageContent HandleBodyFields(MessageContent content, ImapParser io)
        {
            /*
                body-fields     = body-fld-param SP body-fld-id SP body-fld-desc SP body-fld-enc SP body-fld-octets

                    body-fld-param  = "(" string SP string *(SP string SP string) ")" / nil
                    body-fld-id     = nstring
                    body-fld-desc   = nstring
                    body-fld-enc    = (DQUOTE ("7BIT" / "8BIT" / "BINARY" / "BASE64"/ "QUOTED-PRINTABLE") DQUOTE) / string
                    body-fld-octets = number
             */

            content.Parameters = io.ReadDictionary(true, false);
            content.ContentId = io.ReadNullableString();
            content.Description = io.ReadNullableString();
            content.ContentTransferEncoding = io.ReadNullableString().ToContentTransferEncoding();
            content.Size = io.ReadLong();

            return content;
        }

        private void HandleBodyExtension(ImapParser io)
        {
            // body-extension  = nstring / number / "(" body-extension *(SP body-extension) ")"

            var token = io.PeekToken();
            if (token.Type == TokenType.OpeningParenthesis)
            {
                io.ReadToken();
                while ((token = io.PeekToken()).Type != TokenType.ClosingParenthesis)
                    HandleBodyExtension(io);
            }
            else
                io.ReadToken();
        }

        private MessageContent HandleOnePartBody(ImapParser io, string contentNumber)
        {
            /*
                body-type-1part = (body-type-basic / body-type-msg / body-type-text) [SP body-ext-1part]

                    body-type-basic = media-basic SP body-fields
                        media-basic     = ((DQUOTE ("APPLICATION" / "AUDIO" / "IMAGE" / "MESSAGE" / "VIDEO") DQUOTE) / string) SP media-subtype
                            media-subtype   = string

                    body-type-msg   = media-message SP body-fields SP envelope SP body SP body-fld-lines
                        media-message   = DQUOTE "MESSAGE" DQUOTE SP DQUOTE "RFC822" DQUOTE

                    body-type-text  = media-text SP body-fields SP body-fld-lines
                        DQUOTE "TEXT" DQUOTE SP media-subtype
                            media-subtype   = string
            */

            var type = io.ReadString().ToLower();
            var mediaSubType = io.ReadString().ToLower();
            var bodyFldLines = 0;

            var result = HandleBodyFields(
                type == "text" ?
                    new TextMessageContent(Client, Message) :
                    type == "message" && mediaSubType == "rfc822" ?
                        new RFC822MessageContent(Client, Message) :
                        new MessageContent(Client, Message)
            , io);

            if (type == "message" && mediaSubType == "rfc822")
            {
                // body-type-msg
                ((RFC822MessageContent)result).Envelope = HandleEnvelope(io);
                HandleBody(io, contentNumber);
            }

            if (mediaSubType == "rfc822" || type == "text")
                ((TextMessageContent)result).Lines = bodyFldLines = io.ReadInt();

            result.ContentType = HeaderFieldParser.ParseContentType(type + "/" + mediaSubType);

            var token = io.PeekToken();
            if (token.Type != TokenType.ClosingParenthesis)
            {
                /*  
                    body-ext-1part  = body-fld-md5 [SP body-fld-dsp [SP body-fld-lang [SP body-fld-loc *(SP body-extension)]]]
                        body-fld-md5    = nstring
                        body-fld-dsp    = "(" string SP body-fld-param ")" / nil
                        body-fld-lang   = nstring / "(" string *(SP string) ")"
                        body-fld-loc    = nstring
                        body-extension  = nstring / number / "(" body-extension *(SP body-extension) ")"
                */
                result.Md5 = io.ReadNullableString();

                token = io.PeekToken();

                if (token.Type != TokenType.ClosingParenthesis)
                {
                    // body-fld-dsp
                    if (token.Type == TokenType.OpeningParenthesis)
                    {
                        io.ConsumeOpeningParenthesis();
                        result.ContentDisposition = new ContentDisposition(io.ReadString().ToLower());
                        result.Parameters = io.ReadDictionary(true, false, result.Parameters);
                        io.ConsumeClosingParenthesis();
                    }
                    else if (token.Type == TokenType.Nil)
                        io.ConsumeToken(TokenType.Nil);

                    token = io.PeekToken();
                    if (token.Type != TokenType.ClosingParenthesis)
                    {
                        // body-fld-lang
                        if (token.Type == TokenType.OpeningParenthesis)
                            result.Language = io.ReadStringList().ToArray();
                        else
                            result.Language = new List<string>(new string[] { io.ReadNullableString() }.Where(_ => !string.IsNullOrEmpty(_))).ToArray();

                        token = io.PeekToken();
                        if (token.Type != TokenType.ClosingParenthesis)
                        {
                            // body-fld-loc
                            io.ReadNullableString();

                            token = io.PeekToken();
                            if (token.Type != TokenType.ClosingParenthesis)
                                HandleBodyExtension(io);
                        }
                    }
                }
            }

            result.ContentNumber = contentNumber;

            return result;
        }

        private List<MessageContent> HandleBody(ImapParser io, string contentNumber = "")
        {
            // body            = "(" (body-type-1part / body-type-mpart) ")"
            var result = new List<MessageContent>();

            io.ConsumeOpeningParenthesis();

            ImapToken token = io.PeekToken();

            if (token.Type == TokenType.OpeningParenthesis)
                result.AddRange(HandleMultiPartBody(io, contentNumber));
            else
                result.Add(HandleOnePartBody(io, string.IsNullOrWhiteSpace(contentNumber) ? "1" : contentNumber));

            io.ConsumeClosingParenthesis();

            return result;
        }

        public override bool HandleSpecificUntaggedResponse(ImapParser io, ImapToken responseToken)
        {
            var token = io.PeekToken();
            if (responseToken.Type != TokenType.Number || token.Value != "FETCH")
                return base.HandleSpecificUntaggedResponse(io, responseToken);

            // responseToken = UID / SEQNUMBER
            io.ReadAtom(); // 'FETCH'

            io.ConsumeOpeningParenthesis();

            while ((token = io.PeekToken()).Type == TokenType.Atom)
            {
                io.ReadToken();

                switch (token.Value)
                {
                    case "UID":
                        HandleUId(io);
                        break;

                    case "FLAGS":
                        HandleFlags(io);
                        Message.DownloadProgress |= MessageFetchMode.Flags;
                        break;

                    case "INTERNALDATE":
                        HandleInternalDate(io);
                        Message.DownloadProgress |= MessageFetchMode.InternalDate;
                        break;

                    case "RFC822.SIZE":
                        HandleSize(io);
                        Message.DownloadProgress |= MessageFetchMode.Size;
                        break;

                    case "ENVELOPE":
                        Message.ApplyEnvelope(HandleEnvelope(io));
                        Message.DownloadProgress |= MessageFetchMode.Envelope;
                        break;

                    case "BODYSTRUCTURE":
                    case "BODY":
                        token = io.PeekToken();
                        if (token.Type == TokenType.OpeningBracket)
                            HandleBodySection(io);
                        else if (token.Type == TokenType.OpeningParenthesis)
                        {
                            Message.BodyParts = HandleBody(io).ToArray();
                            Message.DownloadProgress |= MessageFetchMode.BodyStructure;
                            if (token.Value == "BODY")
                                Message.DownloadProgress |= MessageFetchMode.Body;
                        }
                        else
                            throw new UnexpectedTokenException();

                        break;
                }
            }

            io.ConsumeClosingParenthesis();

            Message.InProgress = MessageFetchMode.None;

            return true;
        }
    }
}
