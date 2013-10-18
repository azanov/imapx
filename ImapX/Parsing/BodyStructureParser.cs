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
            _reader = new StringReader(Normalize(data));
            _client = client;
            _message = message;
        }

        public void Dispose()
        {
            if (_reader == null) return;
            _reader.Dispose();
        }

        private string Normalize(string data)
        {
            int parenthesis = 0;
            for (int i = 0; i < data.Length; i++)
            {
                switch (data[i])
                {
                    case '(':
                        parenthesis += parenthesis == -1 ? 2 : 1;
                        break;
                    case ')':
                        parenthesis--;
                        break;
                }

                if (parenthesis == 0 || parenthesis == -2)
                    return data.Substring(0, i + 1);
            }

            return data;
        }

        public MessageContent[] Parse()
        {
            _reader.Read();
            return _reader.Peek() != '(' ? new[] { ParsePart("1") } : ParseBlock();
        }

        public MessageContent[] ParseBlock(string number = "")
        {
            var parts = new List<MessageContent>();

            int blockPartCount = 1;

            while (_reader.Peek() == '(')
            {
                _reader.Read();

                if (_reader.Peek() == '(')
                    parts.AddRange(ParseBlock(number + blockPartCount));
                else
                    parts.Add(ParsePart(number + (string.IsNullOrEmpty(number) ? "" : ".") + blockPartCount));

                blockPartCount++;
            }
            SkipOver('(');
            SkipUntil('(');
            return parts.ToArray();
        }

        public MessageContent ParsePart(string partNumber)
        {
            var part = new MessageContent(_client, _message);

            string contentType = ReadString().ToLower();
            string contentSubType = ReadString().ToLower();

            part.ContentNumber = partNumber;
            part.ContentType = string.IsNullOrEmpty(contentType) || string.IsNullOrEmpty(contentSubType)
                ? null
                : new ContentType(contentType.ToLower() + "/" + contentSubType.ToLower());

            part.Parameters = ReadParameterList();

            if (part.Parameters.ContainsKey("content-type"))
            {
                if (part.ContentType == null)
                {
                    part.ContentType = new ContentType(part.Parameters["content-type"]);

                    if (!string.IsNullOrEmpty(part.ContentType.Name))
                        part.ContentType.Name = StringDecoder.Decode(part.ContentType.Name);
                }

            }

            if (part.Parameters.ContainsKey("charset"))
            {
                if (part.ContentType == null)
                    part.ContentType = new ContentType();
                part.ContentType.CharSet = part.Parameters["charset"];
            }

            if (part.Parameters.ContainsKey("name") || part.Parameters.ContainsKey("filename"))
            {
                var value = StringDecoder.Decode(part.Parameters.ContainsKey("name") ? part.Parameters["name"] : part.Parameters["filename"]);

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

                part.ContentId = part.Parameters["content-id"].Trim(' ','<', '>');
            }

            part.ContentId = ReadString().Trim(' ', '<', '>');

            if (!string.IsNullOrEmpty(part.ContentId))
            {
                if (part.ContentDisposition == null)
                    part.ContentDisposition = new ContentDisposition();
                part.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
            }

            part.Description = ReadString();
            part.ContentTransferEncoding = ReadString().ToContentTransferEncoding();
            part.Size = ReadLong();

            switch (contentType)
            {
                
                case "message":

                    // When a message is being attached (e.g) through outlook
                    // it won't have a filename

                    if(part.ContentType == null)
                        part.ContentType = new ContentType();
                    part.ContentType.Name = "unnamed.eml";
                    if(part.ContentDisposition == null)
                        part.ContentDisposition = new ContentDisposition();
                    part.ContentDisposition.DispositionType = DispositionTypeNames.Attachment;
                    part.ContentDisposition.Size = part.Size;
                    part.ContentDisposition.FileName = "unnamed.eml";
                    return part;
                case "text":
                    ReadLong(); // lines
                    break;
            }

            Skip(_spaces);
            if (_reader.Peek() == ')')
            {
                _reader.Read();
                return part;
            }
            part.Md5 = ReadString();

            Skip(_spaces);
            if (_reader.Peek() == ')')
            {
                SkipOver(')');
                Skip(_spaces);
                return part;
            }
            var disposition = ReadDisposition();

            part.ContentDisposition = disposition;

            if (!string.IsNullOrEmpty(part.ContentId))
            {
                if (part.ContentDisposition == null)
                    part.ContentDisposition = new ContentDisposition();
                part.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
            }

            Skip(_spaces);
            if (_reader.Peek() == ')')
            {
                SkipOver(')');
                Skip(_spaces);
                return part;
            }
            part.Language = ReadString();

            Skip(_spaces);
            if (_reader.Peek() == ')')
            {
                SkipOver(')');
                Skip(_spaces);
                return part;
            }
            string uri = ReadString();

            SkipOver(')');
            Skip(_spaces);


            return part;
        }

        public ContentDisposition ReadDisposition()
        {
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

            Skip(_spaces);
            _reader.Read(); // read ')'

            return disposition;
        }


        private void Skip(char[] chars)
        {
            while (chars.Contains((char)_reader.Peek()))
                _reader.Read();
        }

        private void SkipOver(char @char)
        {
            while ((_reader.Peek()) != -1)
                if (@char == (char)_reader.Read()) return;
        }

        private void SkipUntil(char @char)
        {
            while ((_reader.Peek()) != -1)
                if (@char == (char)_reader.Peek()) return;
                else _reader.Read();
        }


        private string ReadString(bool trimSpaces = true)
        {
            if (trimSpaces)
                Skip(_spaces);

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
                Skip(_spaces);

            return sb.ToString();
        }

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
                Skip(_spaces);

            char currentChar;
            var chars = new List<char>();

            while (char.IsDigit(currentChar = (char) _reader.Peek()))
            {
                _reader.Read();
                chars.Add(currentChar);
            }

            long result;

            long.TryParse(new string(chars.ToArray()), out result);

            if (trimSpaces)
                Skip(_spaces);

            return result;
        }
    }
}
