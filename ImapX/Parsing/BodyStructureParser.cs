using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using ImapX.EncodingHelpers;
using ImapX.Extensions;

namespace ImapX.Parsing
{
    internal class BodyStructureParser : IDisposable
    {

        private readonly ImapClient _client;
        private readonly Message _message;
        private readonly StringReader _reader;
        private readonly char[] _spaces = { ' ', '\t' };

        public BodyStructureParser(string data, ImapClient client, Message message)
        {
            _reader = new StringReader(data);
            _client = client;
            _message = message;
        }

        public MessageContent[] Parse(int level = 0)
        {
            var parts = new List<MessageContent>();
            int partNumber = 1;

            while (_reader.Peek() == '(')
            {
                _reader.Read();

                if (_reader.Peek() == '(')
                {
                    parts.AddRange(Parse(level + 1));
                    DropExtensionData();
                }
                else
                    parts.Add(ParsePart(partNumber, level));

                

                partNumber++;
            }
            return parts.ToArray();
        }

        public MessageContent ParsePart(int number, int level)
        {
            
            var part = new MessageContent(_client, _message);

            part.ContentNumber = level < 2 ? number.ToString() : level - 1 + "." + number;

            var contentType = ReadString().ToLower();
            var contentSubType = ReadString().ToLower();

            if (string.IsNullOrEmpty(contentType))
            {
                // Set content type to application/octet-stream in case server returned NIL
                contentType = "application";
                contentSubType = "octet-stream";
            }

            part.ContentType = HeaderFieldParser.ParseContentType(contentType + "/" + contentSubType);

            part.Parameters = ReadParameterList();

            if (part.Parameters.ContainsKey("content-type"))
            {
                try
                {
                    if (part.ContentType == null)
                        part.ContentType = HeaderFieldParser.ParseContentType(part.Parameters["content-type"]);
                } catch{}

            }

            if (part.Parameters.ContainsKey("charset"))
            {
                if (part.ContentType == null)
                    part.ContentType = new ContentType();
                part.ContentType.CharSet = part.Parameters["charset"];
            }

            if (part.Parameters.ContainsKey("name") || part.Parameters.ContainsKey("filename"))
            {
                var value =
                    StringDecoder.Decode(part.Parameters.ContainsKey("name")
                        ? part.Parameters["name"]
                        : part.Parameters["filename"]);

                if (part.ContentType == null)
                    part.ContentType = new ContentType();

                if (part.ContentDisposition == null)
                    part.ContentDisposition = new ContentDisposition();

                part.ContentDisposition.FileName = value;

                if (string.IsNullOrEmpty(part.ContentDisposition.DispositionType))
                    part.ContentDisposition.DispositionType = DispositionTypeNames.Attachment;

                part.ContentType.Name = value;
            }

            if (part.Parameters.ContainsKey("content-id"))
            {

                if (part.ContentDisposition == null)
                    part.ContentDisposition = new ContentDisposition();

                part.ContentDisposition.DispositionType = DispositionTypeNames.Inline;

                part.ContentId = part.Parameters["content-id"].Trim(' ', '<', '>');
            }

            part.ContentId = ReadString();
            part.Description = ReadString();
            part.ContentTransferEncoding = ReadString().ToContentTransferEncoding();
            part.Size = ReadLong();
            
            if (contentType == "text")
            {
                part.Lines = ReadLong();
            }
            else if (contentType == "message" && contentSubType == "rfc822")
            {
                SkipSpaces();

                if (_reader.Peek() == '(')
                {
                    part.Envelope = ReadEnvelope();
                    _reader.Read(); // Read ')' of envelope

                    DropExtensionData();

                    part.Lines = ReadLong();
                }
                else
                {
                    // Some servers, e.g GMail, don't return 
                    // envelope & body strcuture for message/rfc822 attachments

                    if (part.ContentDisposition == null)
                        part.ContentDisposition = new ContentDisposition();

                    part.ContentDisposition.FileName = "unnamed.eml";

                    if (string.IsNullOrEmpty(part.ContentDisposition.DispositionType))
                        part.ContentDisposition.DispositionType = DispositionTypeNames.Attachment;

                }
            }

            SkipSpaces();

            if (_reader.Peek() == ')')
            {
                _reader.Read();
                return part;
            }
            part.Md5 = ReadString();

            if (_reader.Peek() == ')')
            {
                _reader.Read();
                return part;
            }


            part.ContentDisposition = ReadDisposition() ?? part.ContentDisposition;

            if (!string.IsNullOrEmpty(part.ContentId) && part.ContentDisposition != null)
                part.ContentDisposition.DispositionType = DispositionTypeNames.Inline;

            if (part.ContentDisposition != null && string.IsNullOrEmpty(part.ContentDisposition.FileName))
                part.ContentDisposition.FileName = string.Format("unnamed-{0}.{1}", part.ContentNumber,
                    contentType == "message" ? "eml" : "dat");

            if (_reader.Peek() == ')')
            {
                _reader.Read();
                return part;
            }

            part.Language = ReadLanguage();

            DropExtensionData();

            return part;
        }

        private void DropExtensionData()
        {
            var braces = 0;

            while (true)
            {
                var tmp = _reader.Read();
                if (tmp == '(') 
                    braces++;
                else if (tmp == ')')
                {
                    braces--;
                    if (braces <= 0)
                        break;
                    
                }
                else if (tmp == -1)
                    break;
            }
        }

        private string[] ReadLanguage()
        {
            SkipSpaces();
            if (_reader.Peek() == ')')
                return new string[0];
            switch (_reader.Peek())
            {
                case '(':
                {
                    _reader.Read();

                    var sb = new StringBuilder();

                    while (_reader.Peek() != ')')
                        sb.Append(_reader.Read());

                    return sb.Replace("\"", "").ToString().Split(' ');

                }
                default:
                    return new[] { ReadString() };
            }
        }

        #region Reading strings

        private string ReadString(bool trimSpaces = true)
        {
            if (trimSpaces)
                SkipSpaces();
            return _reader.Peek() == '{' ? ReadUnquotedString(trimSpaces) : ReadQuotedString(trimSpaces);
        }

        private string ReadUnquotedString(bool trimSpaces = true)
        {
            if (trimSpaces)
                SkipSpaces();

            _reader.Read(); // read '{'

            var sb = new StringBuilder();
            char currentChar;

            while ((currentChar = (char)_reader.Read()) != '}')
                sb.Append(currentChar);

            var charCount = int.Parse(sb.ToString());

            _reader.Read();

            sb = new StringBuilder();

            for (var i = 0; i < charCount; i++)
                sb.Append((char)_reader.Read());

            if (trimSpaces)
                SkipSpaces();

            return sb.ToString();
        }

        private string ReadQuotedString(bool trimSpaces = true)
        {
            if (trimSpaces)
                SkipSpaces();

            char prevChar = '\0', currentChar;

            var sb = new StringBuilder();

            while ((currentChar = (char)_reader.Read()) != '"')
            {
                sb.Append(currentChar);
                if (sb.ToString() == "NIL")
                    return string.Empty;
            }

            while ((currentChar = (char)_reader.Read()) != '"' || prevChar == '\\')
            {
                prevChar = currentChar;
                sb.Append(currentChar);
            }

            if (trimSpaces)
                SkipSpaces();

            return sb.ToString();
        }

        #endregion

        private Dictionary<string, string> ReadParameterList()
        {
            var result = new Dictionary<string, string>();

            if (_reader.Peek() == 'N')
            {
                ReadString();
                return result;
            }
            if (_reader.Peek() != '(')
                return result;

            _reader.Read();

            while (_reader.Peek() != ')')
                result.Add(ReadString().ToLower(), ReadString());

            _reader.Read();

            return result;
        }

        private long ReadLong(bool trimSpaces = true)
        {
            if (trimSpaces)
                SkipSpaces();

            char currentChar;
            var chars = new List<char>();

            while (char.IsDigit(currentChar = (char)_reader.Peek()))
            {
                _reader.Read();
                chars.Add(currentChar);
            }

            long result;

            long.TryParse(new string(chars.ToArray()), out result);

            if (trimSpaces)
                SkipSpaces();

            return result;
        }

        private ContentDisposition ReadDisposition()
        {
            SkipSpaces();
            var sb = new StringBuilder();
            char currentChar;

            while ((currentChar = (char)_reader.Read()) != '(')
            {
                sb.Append(currentChar);
                if (sb.ToString() == "NIL")
                    return null;
            }
            var type = ReadString().ToLower();
            var paramaters = ReadParameterList();
            var disposition = new ContentDisposition(type);

            foreach (var paramater in paramaters)
            {
                switch (paramater.Key)
                {
                    case "filename":
                    case "name":
                        disposition.FileName = StringDecoder.Decode(paramater.Value);
                        break;
                }
            }

            SkipSpaces();
            _reader.Read(); // read ')'
            SkipSpaces();
            return disposition;
        }

        private Envelope ReadEnvelope()
        {
            var date = ReadString();
            var subject = ReadString();
            var from = ReadMailAddressCollection();
            var sender = ReadMailAddressCollection();
            var replyTo = ReadMailAddressCollection();
            var to = ReadMailAddressCollection();
            var cc = ReadMailAddressCollection();
            var bcc = ReadMailAddressCollection();
            var inReplyTo = ReadString();
            var messageId = ReadString();

            return new Envelope
            {
                Bcc = bcc,
                Cc = cc,
                Date = HeaderFieldParser.ParseDate(date),
                From = from.FirstOrDefault(),
                InReplyTo = inReplyTo,
                MessageId = messageId,
                ReplyTo = replyTo,
                Sender = sender.FirstOrDefault(),
                Subject = subject,
                To = to
            };

        }

        #region Reading mail addresses

        private List<MailAddress> ReadMailAddressCollection()
        {
            SkipSpaces();

            if (_reader.Peek() == 'N')
            {
                ReadString();
                return new List<MailAddress>();
            }
            _reader.Read(); // read '(' indicating collection

            var result = new List<MailAddress>();

            while (_reader.Peek() != ')')
                try
                {
                    result.Add(ReadMailAddress());
                }
                catch
                {
                }

            _reader.Read(); // read ')' indicating end of collection

            SkipSpaces();

            return result;
        }

        private MailAddress ReadMailAddress()
        {
            SkipSpaces();
            _reader.Read(); // Read '(' indicating start of mail address

            var name = ReadString();
            var sourceRoute = ReadString();
            var mailBox = ReadString();
            var host = ReadString();

            var result = new MailAddress(string.Format("{0}@{1}", mailBox, host), name);

            _reader.Read(); // Read ')' indicating end of mail address
            SkipSpaces();

            return result;
        }

        #endregion

        private void SkipSpaces()
        {
            int data;
            while ((data = _reader.Peek()) != -1 && _spaces.Contains((char) data))
                _reader.Read();
        }

        public void Dispose()
        {
            if(_reader != null)
                _reader.Dispose();
        }
    }
}