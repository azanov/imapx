using System.Collections.Generic;
using System.Text;
using ImapX.EncodingHelpers;

namespace System.Net.Mime
{
    public class ContentType
    {
        internal static readonly string Default = "application/octet-stream";
        private readonly string _type;
        private string _mediaType;
        private Dictionary<string, string> _parameters;
        private string _subType;


        public ContentType()
            : this(Default)
        {
        }


        public ContentType(string contentType)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException("contentType");
            }
            if (contentType == string.Empty)
            {
                throw new ArgumentException();
            }
            _type = contentType;
            ParseValue();
        }


        public string Boundary
        {
            get { return Parameters["boundary"]; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    Parameters.Remove("boundary");
                    return;
                }
                Parameters["boundary"] = value;
            }
        }


        public string CharSet
        {
            get { return Parameters["charset"]; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    Parameters.Remove("charset");
                    return;
                }
                Parameters["charset"] = value;
            }
        }


        public string MediaType
        {
            get { return _mediaType + "/" + _subType; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (value == string.Empty)
                {
                    throw new ArgumentException();
                }
                int num = 0;
                _mediaType = MailBnfHelper.ReadToken(value, ref num, null);
                if (_mediaType.Length == 0 || num >= value.Length || value[num++] != '/')
                {
                    throw new FormatException();
                }
                _subType = MailBnfHelper.ReadToken(value, ref num, null);
                if (_subType.Length == 0 || num < value.Length)
                {
                    throw new FormatException();
                }
            }
        }


        public string Name
        {
            get
            {
                string text = Parameters["name"];
                Encoding encoding = MimeBasePart.DecodeEncoding(text);
                if (encoding != null)
                {
                    text = StringDecoder.Decode(text, true);
                }
                return text;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    Parameters.Remove("name");
                    return;
                }
                Parameters["name"] = value;
            }
        }


        public Dictionary<string, string> Parameters
        {
            get
            {
                if (_parameters == null && _type == null)
                {
                    _parameters = new Dictionary<string, string>();
                }
                return _parameters;
            }
        }


        public override bool Equals(object rparam)
        {
            return rparam != null &&
                   string.Compare(ToString(), rparam.ToString(), StringComparison.OrdinalIgnoreCase) == 0;
        }


        public override int GetHashCode()
        {
            return ToString().ToLowerInvariant().GetHashCode();
        }


        private void ParseValue()
        {
            int num = 0;
            Exception ex = null;
            _parameters = new Dictionary<string, string>();
            try
            {
                _mediaType = MailBnfHelper.ReadToken(_type, ref num, null);
                if (string.IsNullOrEmpty(_mediaType) || num >= _type.Length || _type[num++] != '/')
                {
                    ex = new FormatException();
                }
                if (ex == null)
                {
                    _subType = MailBnfHelper.ReadToken(_type, ref num, null);
                    if (string.IsNullOrEmpty(_subType))
                    {
                        ex = new FormatException();
                    }
                }
                if (ex == null)
                {
                    while (MailBnfHelper.SkipCfWs(_type, ref num))
                    {
                        if (_type[num++] != ';')
                        {
                            ex = new FormatException();
                            break;
                        }
                        if (!MailBnfHelper.SkipCfWs(_type, ref num))
                        {
                            break;
                        }
                        string text = MailBnfHelper.ReadParameterAttribute(_type, ref num, null);
                        if (string.IsNullOrEmpty(text))
                        {
                            ex = new FormatException();
                            break;
                        }
                        if (num >= _type.Length || _type[num++] != '=')
                        {
                            ex = new FormatException();
                            break;
                        }
                        if (!MailBnfHelper.SkipCfWs(_type, ref num))
                        {
                            ex = new FormatException();
                            break;
                        }
                        string text2 = _type[num] == '"' ? MailBnfHelper.ReadQuotedString(_type, ref num, null) : MailBnfHelper.ReadToken(_type, ref num, null);
                        if (text2 == null)
                        {
                            ex = new FormatException();
                            break;
                        }
                        _parameters.Add(text, text2);
                    }
                }
            }
            catch (FormatException)
            {
                throw new FormatException();
            }
            if (ex != null)
            {
                throw new FormatException();
            }
        }
    }
}