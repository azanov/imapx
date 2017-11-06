using ImapX.EncodingHelpers;
using ImapX.Enums;
using ImapX.Exceptions;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace ImapX
{
    public class ImapParser
    {
        private int _pickedByte = -2;
        private int _currentByte = -1;
        private Stream _in;
        private Stream _out;
        private ImapBase _base;
        private ImapToken _pickedToken;

        public CancellationToken CancellationToken { get; set; }

        public ImapParser() { }

        public ImapParser(ImapBase imapBase, Stream stream)
        {
            _base = imapBase;
            BindStream(stream);
        }

        public void BindStream(Stream stream)
        {
            _in = stream; _out = stream;
        }

        #region core parsing

        protected ImapToken ParseQuotedString(bool skipDecoding = false)
        {
            var escaped = false;
            var endOfString = false;
            var buffer = new List<byte>();
            //var rfc2047 = false;
            //var rfc2047Encoding = Encoding.UTF8;
            //var rfc2047TransferEncoding = ContentTransferEncoding.Unknown;

            ReadByte(); // read leading "

            while (!endOfString)
            {
                if ((_currentByte = ReadByte(true)) == -1)
                    break;

                switch (_currentByte)
                {
                    case ImapChars.DoubleQuote:
                        if (escaped)
                        {
                            buffer.Add((byte)_currentByte); escaped = false;
                        }
                        else if (CanFollowQuotedString(PeekByte()))
                        {
                            endOfString = true;
                        }
                        else
                            buffer.Add((byte)_currentByte);
                        break;

                    case ImapChars.Backslash:
                        if (escaped)
                        {
                            buffer.Add((byte)_currentByte); escaped = false;
                        }
                        escaped = true;
                        break;

                    default:
                        if (escaped)
                        {
                            // in case the \ is followed by some trash instead of " or \
                            buffer.Add(ImapChars.Backslash);
                            escaped = false;
                        }

                        buffer.Add((byte)_currentByte);
                        break;
                }
            }

            ;

            var result = skipDecoding ? null : Encoding.UTF8.GetString(buffer.ToArray());

            if (_base.IsDebug)
                Debug.Write(result + "\"");

            return new ImapToken(TokenType.Quoted, result, skipDecoding ? buffer.ToArray() : null);
        }

        protected ImapToken ParseLiteralToken(bool skipDecoding = false)
        {
            var data = ReadLiteralData();
            var result = new ImapToken(TokenType.Literal,
               skipDecoding ? null : Encoding.UTF8.GetString(data), skipDecoding ? data : null);
            return result;
        }

        protected ImapToken ParseAtomToken()
        {
            var nilCandidate = true;
            var nilBytesCount = 0;
            var allDigits = true;

            StringBuilder sb = new StringBuilder();

            while (IsAtom(_currentByte = PeekByte()))
            {
                ReadByte();

                if (nilCandidate && ImapChars.NilBytes[nilBytesCount++] != _currentByte)
                    nilCandidate = false;

                if (allDigits)
                    allDigits = allDigits && _currentByte > 47 && _currentByte < 58;

                sb.Append((char)_currentByte);
            }

            if (nilCandidate)
                return new ImapToken(TokenType.Nil);

            if (allDigits)
                return new ImapToken(TokenType.Number, sb.ToString());

            return new ImapToken(TokenType.Atom, sb.ToString());
        }

        protected ImapToken ParseFlag()
        {
            return new ImapToken(TokenType.Flag, ((char)ReadByte()).ToString() + ReadToken().Value);
        }

        #endregion

        #region reading

        protected void SkipToToken()
        {
            _currentByte = -1;
            while ((_currentByte = PeekByte()) > ImapChars.LF && (_currentByte == ImapChars.Space || _currentByte == ImapChars.CR))
                ReadByte();
        }

        public ImapToken ReadToken(bool skipDecoding = false)
        {
            if (_pickedToken != null)
            {
                var token = _pickedToken;
                _pickedToken = null;
                return token;
            }

            SkipToToken();

            if (_currentByte == ImapChars.DoubleQuote)
                return ParseQuotedString(skipDecoding);
            else if (_currentByte == ImapChars.OpeningBrace)
                return ParseLiteralToken(skipDecoding);
            else if (_currentByte == ImapChars.Backslash)
                return ParseFlag();
            else if (IsAtom(_currentByte))
                return ParseAtomToken();
            else
                ReadByte();

            return new ImapToken((TokenType)_currentByte);
        }

        protected int PeekByte()
        {
            if (_pickedByte == -2)
                _pickedByte = ReadByte(true);

            return _pickedByte;
        }

        protected int ReadByte(bool lowLevelTraceDisabled = false)
        {
            var result = _pickedByte;
            if (result > -2)
                _pickedByte = -2;
            else
            {
                result = _in.ReadByte();
            }

            if (_base.IsDebug && !lowLevelTraceDisabled)
                Debug.Write(((char)result).ToString());

            return result;
        }

        protected byte[] ReadBytes(int count, bool lowLevelTraceDisabled = false)
        {
            var result = new byte[count];
            int b;
            for (var i = 0; i < result.Length; i++)
            {
                b = ReadByte(lowLevelTraceDisabled);
                result[i] = (byte)(b > 0 ? b : 0);
            }

            return result;
        }

        public ImapToken PeekToken()
        {
            if (_pickedToken == null)
                _pickedToken = ReadToken();
            return _pickedToken;
        }

        public ImapToken ReadAtom()
        {
            var token = ReadToken();
            if (token.Type != TokenType.Atom)
                throw new UnexpectedTokenException();
            return token;
        }

        public long ReadLong()
        {
            var token = ReadToken();
            if (token.Type != TokenType.Number)
                throw new UnexpectedTokenException();
            return long.Parse(token.Value);
        }

        public int ReadInt()
        {
            var token = ReadToken();
            if (token.Type != TokenType.Number)
                throw new UnexpectedTokenException();
            return int.Parse(token.Value);
        }

        public string ReadString()
        {
            var token = ReadToken();
            if (!(token.Type == TokenType.Quoted || token.Type == TokenType.Literal || token.Type == TokenType.Atom))
                throw new UnexpectedTokenException();
            return token.Value;
        }

        public ImapToken ReadStringToken(bool skipDecoding = false)
        {
            var token = ReadToken(skipDecoding);
            if (!(token.Type == TokenType.Quoted || token.Type == TokenType.Literal || token.Type == TokenType.Atom))
                throw new UnexpectedTokenException();
            return token;
        }

        protected byte[] ReadLiteralData()
        {
            ReadByte();
            var length = ReadInt();
            ConsumeToken(TokenType.ClosingBrace);
            ConsumeToken(TokenType.Eol);
            return ReadBytes(length);
        }

        public MailAddress ReadAddress()
        {
            ConsumeOpeningParenthesis();

            var name = ReadNullableString();
            var adl = ReadNullableString();
            var mailbox = ReadNullableString();
            var host = ReadNullableString();

            ConsumeClosingParenthesis();

            return new MailAddress(StringDecoder.Decode(name, true), mailbox + "@" + host);
        }

        public KeyValuePair<string, string>? ReadKeyValuePair(bool nullable, bool nullableValue = true)
        {
            var token = PeekToken();
            if (token.Type == TokenType.Nil)
            {
                ReadToken();
                if (nullable) return null;
                throw new UnexpectedTokenException();
            }
            return new KeyValuePair<string, string>(ReadString(), nullableValue ? ReadNullableString() : ReadString());
        }

        public Dictionary<string, string> ReadDictionary(bool nullable = true, bool nullableFieldValues = true, Dictionary<string, string> result = null)
        {
            var token = ReadToken();
            if (token.Type == TokenType.OpeningParenthesis)
            {
                if (result == null)
                    result = new Dictionary<string, string>();

                while ((token = PeekToken()).Type != TokenType.ClosingParenthesis)
                {
                    var item = ReadKeyValuePair(false, nullableFieldValues);
                    result.Add(item.Value.Key, item.Value.Value);
                }

                ConsumeClosingParenthesis();

                return result;
            }
            else if (token.Type == TokenType.Nil && nullable)
                return null;
            else
                throw new UnexpectedTokenException();
        }

        public List<string> ReadStringList(bool nullable = true)
        {
            var token = PeekToken();

            if (token.Type == TokenType.OpeningParenthesis)
            {
                ConsumeOpeningParenthesis();
                var result = new List<string>();

                while ((token = PeekToken()).Type != TokenType.ClosingParenthesis)
                    result.Add(token.Value);

                ConsumeClosingParenthesis();

                return result;
            }
            else if (token.Type == TokenType.Nil && nullable)
            {
                ReadToken();
                return null;
            }
            else
                throw new UnexpectedTokenException();
        }

        public List<MailAddress> ReadAddressList()
        {
            var result = new List<MailAddress>();

            var token = PeekToken();

            if (token.Type == TokenType.Nil)
            {
                ReadToken();
                return result;
            }

            ConsumeOpeningParenthesis();

            while (PeekToken().Type == TokenType.OpeningParenthesis)
                result.Add(ReadAddress());

            ConsumeClosingParenthesis();
            return result;
        }

        public string ReadNullableString()
        {
            var token = ReadToken();
            if (!(token.Type == TokenType.Quoted || token.Type == TokenType.Literal || token.Type == TokenType.Atom || token.Type == TokenType.Nil))
                throw new UnexpectedTokenException();
            return token.Value;
        }

        public ImapToken ReadNullableStringToken(bool skipDecoding = false)
        {
            var token = ReadToken(skipDecoding);
            if (!(token.Type == TokenType.Quoted || token.Type == TokenType.Literal || token.Type == TokenType.Atom || token.Type == TokenType.Nil))
                throw new UnexpectedTokenException();
            return token;
        }

        public string ReadLine()
        {
            if (_pickedToken != null && _pickedToken.Type == TokenType.Eol)
            {
                _pickedToken = null;
                return string.Empty;
            }

            _pickedToken = null;
            var buffer = new List<byte>();
            while ((_currentByte = ReadByte(true)) > -1 && _currentByte != ImapChars.LF)
                if (_currentByte != ImapChars.CR) buffer.Add((byte)_currentByte);

            var result = Encoding.UTF8.GetString(buffer.ToArray());

            if (_base.IsDebug)
                Debug.WriteLine(result);

            return result;
        }

        #endregion

        #region consumers

        public void ConsumeToken(TokenType type)
        {
            var token = ReadToken();
            if (token.Type != type)
                throw new UnexpectedTokenException();
        }

        public void ConsumeAsterisk()
        {
            ConsumeToken(TokenType.Asterisk);
        }

        public void ConsumeClosingParenthesis()
        {
            ConsumeToken(TokenType.ClosingParenthesis);
        }

        public void ConsumeOpeningParenthesis()
        {
            ConsumeToken(TokenType.OpeningParenthesis);
        }

        public void ConsumeOpeningBracket()
        {
            ConsumeToken(TokenType.OpeningBracket);
        }

        public void ConsumeClosingBracket()
        {
            ConsumeToken(TokenType.ClosingBracket);
        }

        public void ConsumeTillEol()
        {
            while ((_currentByte = ReadByte()) > -1 && _currentByte != ImapChars.LF) ;
        }

        public void ConsumeAtom()
        {
            ConsumeToken(TokenType.Atom);
        }

        #endregion

        public void Write(byte[] buffer)
        {
            if (_base.IsDebug)
                Debug.Write(Encoding.UTF8.GetString(buffer));

            _out.Write(buffer, 0, buffer.Length);

        }

        #region tools

        public bool CanFollowQuotedString(int value)
        {
            switch (value)
            {
                case ImapChars.CR:
                case ImapChars.LF:
                case ImapChars.ClosingParenthesis:
                case ImapChars.ClosingBracket:
                case ImapChars.Space:
                    return true;
                default:
                    return false;
            }
        }

        public bool IsAtom(int value)
        {
            //return !ImapChars.AtomSpecials.Contains(value) && value > ImapChars.Space && value < ImapChars.Del;

            if (value <= ImapChars.Space || value >= ImapChars.Del)
                return false;

            switch (value)
            {
                case ImapChars.OpeningParenthesis:
                case ImapChars.ClosingParenthesis:
                case ImapChars.OpeningBrace:
                case ImapChars.Space:
                case ImapChars.LF:

                // list-wildcards
                case ImapChars.Asterisk:
                case ImapChars.PercentSign:

                // quoted-specials
                case ImapChars.DoubleQuote:
                case ImapChars.Backslash:

                // resp-specials
                case ImapChars.ClosingBracket:

                case ImapChars.OpeningBracket:
                case ImapChars.ClosingBrace:
                    return false;

            }

            return true;

        }

        #endregion


    }
}
