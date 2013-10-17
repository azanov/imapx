using System.Collections.Generic;

namespace System.Net.Mime
{
    public class ContentDisposition
    {
        private readonly string _disposition;
        private string _dispositionType;

        private Dictionary<string, string> _parameters;


        public ContentDisposition()
        {
            _dispositionType = "attachment";
            _disposition = _dispositionType;
        }


        public ContentDisposition(string disposition)
        {
            if (disposition == null)
            {
                throw new ArgumentNullException("disposition");
            }
            _disposition = disposition;
            ParseValue();
        }


        public string DispositionType
        {
            get { return _dispositionType; }
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
                _dispositionType = value;
            }
        }


        public Dictionary<string, string> Parameters
        {
            get { return _parameters ?? (_parameters = new Dictionary<string, string>()); }
        }


        public string FileName
        {
            get { return Parameters["filename"]; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    Parameters.Remove("filename");
                    return;
                }
                Parameters["filename"] = value;
            }
        }


        public DateTime CreationDate
        {
            set
            {
                var value2 = new SmtpDateTime(value);
                if (Parameters.ContainsKey("creation-date"))
                    Parameters["creation-date"] = value2.ToString();
                else
                    Parameters.Add("creation-date", value2.ToString());
            }
        }


        public DateTime ModificationDate
        {
            set
            {
                var value2 = new SmtpDateTime(value);
                if (Parameters.ContainsKey("modification-date"))
                    Parameters["modification-date"] = value2.ToString();
                else
                    Parameters.Add("modification-date", value2.ToString());
            }
        }


        public bool Inline
        {
            get { return _dispositionType == "inline"; }
            set
            {
                if (value)
                {
                    _dispositionType = "inline";
                    return;
                }
                _dispositionType = "attachment";
            }
        }


        public DateTime ReadDate
        {
            set
            {
                var value2 = new SmtpDateTime(value);
                if (Parameters.ContainsKey("read-date"))
                    Parameters["read-date"] = value2.ToString();
                else
                    Parameters.Add("read-date", value2.ToString());
            }
        }


        public long Size
        {
            get
            {
                object obj = Parameters.ContainsKey("size") ? Parameters["size"] : null;
                if (obj == null)
                {
                    return -1L;
                }
                return (long) obj;
            }
            set
            {
                if (Parameters.ContainsKey("size"))
                    Parameters["size"] = value.ToString();
                else
                    Parameters.Add("size", value.ToString());
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
            try
            {
                _dispositionType = MailBnfHelper.ReadToken(_disposition, ref num, null);
                if (string.IsNullOrEmpty(_dispositionType))
                {
                    throw new FormatException();
                }
                if (_parameters == null)
                {
                    _parameters = new Dictionary<string, string>();
                }
                else
                {
                    _parameters.Clear();
                }
                while (MailBnfHelper.SkipCfWs(_disposition, ref num))
                {
                    if (_disposition[num++] != ';')
                    {
                        throw new FormatException();
                    }
                    if (!MailBnfHelper.SkipCfWs(_disposition, ref num))
                    {
                        break;
                    }
                    string text = MailBnfHelper.ReadParameterAttribute(_disposition, ref num, null);
                    if (_disposition[num++] != '=')
                    {
                        throw new FormatException();
                    }
                    if (!MailBnfHelper.SkipCfWs(_disposition, ref num))
                    {
                        throw new FormatException();
                    }
                    string value = _disposition[num] == '"' ? MailBnfHelper.ReadQuotedString(_disposition, ref num, null) : MailBnfHelper.ReadToken(_disposition, ref num, null);
                    if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(value))
                    {
                        throw new FormatException();
                    }
                    Parameters.Add(text, value);
                }
            }
            catch (FormatException innerException)
            {
                throw new FormatException();
            }
        }
    }
}