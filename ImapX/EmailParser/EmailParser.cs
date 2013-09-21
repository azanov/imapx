using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ImapX.EmailParser
{
    public class EmailParser
    {
        public const string CRLN = "\r\n";
        public const char SPACE = ' ';
        public const char TAB = '\t';
        public const string BOUNDARY = "boundary=";
        public const string BOUNDARY_END_SYMBOLS = "--";
        public List<ParseError> BadParseItems = new List<ParseError>();
        private int _bodyEndIndex;
        private int _bodyStartIndex;
        public List<string> BoundaryCollection;
        public string[] EmailItems;
        private int _headerEndIndex;
        private string _headerLastKey;
        private int _headerStartIndex;
        public Dictionary<string, string> HeadersCollection;
        public List<BodyPart> Parts = new List<BodyPart>();

        public EmailParser(string[] email)
        {
            HeadersCollection = new Dictionary<string, string>();
            BoundaryCollection = new List<string>();
            EmailItems = email;
        }

        public void InitializeIndexes()
        {
            for (int i = 0; i < EmailItems.Length; i++)
            {
                if (string.IsNullOrEmpty(EmailItems[i]))
                {
                    _headerStartIndex = 0;
                    _headerEndIndex = i;
                    _bodyStartIndex = i + 1;
                    _bodyEndIndex = EmailItems.Length - 2;
                    return;
                }
            }
        }

        public void ParseHeaders()
        {
            for (int i = _headerStartIndex; i < _headerEndIndex; i++)
            {
                if (EmailItems[i].ToLower().Contains("boundary="))
                {
                    try
                    {
                        string text = EmailItems[i].Split(new[]
                        {
                            "boundary="
                        }, StringSplitOptions.None)[1];
                        if (text.Contains(";"))
                        {
                            text = text.Substring(0, text.IndexOf(';'));
                        }
                        if (text.Contains("\""))
                        {
                            text = text.Replace("\"", "");
                        }
                        BoundaryCollection.Add(text);
                    }
                    catch (Exception e)
                    {
                        BadParseItems.Add(new ParseError(EmailItems[i], e));
                    }
                }
                int num = EmailItems[i].IndexOf(':');

                // Fix provided by iamwill 12/12/12
                // For reference see http://imapx.codeplex.com/workitem/1424

                // Fix provided by itreims  03/05/13
                // For reference see http://imapx.codeplex.com/workitem/1523
                try
                {
                    if (num > 0 & !EmailItems[i].ToLower().StartsWith('\t'.ToString(CultureInfo.InvariantCulture)) &
                        !EmailItems[i].ToLower().StartsWith(' '.ToString(CultureInfo.InvariantCulture)))
                    {
                        string text2 = EmailItems[i].Substring(0, num);
                        string text3 = String.Empty;
                        if (EmailItems[i].Length > num + 1)
                        {
                            text3 = EmailItems[i].Substring(num + 2);
                        }
                        _headerLastKey = text2;
                        string trimmedText2 = text2.Trim(new[] {' '});
                        string trimmedText3 = text3.Trim(new[] {' '});

                        if (!HeadersCollection.ContainsKey(trimmedText2))
                        {
                            HeadersCollection.Add(trimmedText2, trimmedText3);
                        }
                        else
                        {
                            HeadersCollection[trimmedText2] += "\r\n" + trimmedText3;
                        }
                    }
                    else
                    {
                        if (_headerLastKey != null && HeadersCollection.ContainsKey(_headerLastKey))
                        {
                            Dictionary<string, string> headersCollection = HeadersCollection;
                            string headerLastKey = _headerLastKey;
                            headersCollection[headerLastKey] =
                                headersCollection[headerLastKey] + "\n" + EmailItems[i];
                        }
                    }
                }
                catch (Exception e2)
                {
                    BadParseItems.Add(new ParseError(EmailItems[i], e2));
                }
            }
        }

        public string GetPart(BodyPart p)
        {
            if (EmailItems == null || EmailItems.Length == 0)
                return string.Empty;

            var stringBuilder = new StringBuilder();
            bool flag = true;
            foreach (int current in p.BodyIndexes)
            {
                if (current < 0) continue;
                if (flag)
                {
                    stringBuilder.Append(EmailItems[current >= EmailItems.Length ? EmailItems.Length - 1 : current]);

                    flag = false;
                }
                else
                {
                    stringBuilder.Append("\r\n" +
                                         EmailItems[current >= EmailItems.Length ? EmailItems.Length - 1 : current]);
                }
            }
            if (p.Boundary == null && stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1].Equals(')'))
            {
                stringBuilder = stringBuilder.Remove(stringBuilder.Length - 1, 1);
            }
            return stringBuilder.ToString();
        }

        public void ParseBody()
        {
            var bodyPart = new BodyPart {Boundary = null};
            if (BoundaryCollection.Count < 1)
            {
                for (int i = _bodyStartIndex; i <= _bodyEndIndex; i++)
                {
                    bodyPart.BodyIndexes.Add(i);
                }
            }
            else
            {
                int num = _bodyStartIndex;
                while (num < _bodyEndIndex &&
                       (BoundaryCollection.Count <= 0 || !EmailItems[num].Contains(BoundaryCollection[0] + "--")))
                {
                    foreach (string current in BoundaryCollection)
                    {
                        if (EmailItems[num].Contains(current))
                        {
                            if (bodyPart.Boundary != null)
                            {
                                Parts.Add(bodyPart);
                            }
                            bodyPart = new BodyPart {Boundary = current};
                            num++;
                            while (_bodyEndIndex >= num && EmailItems[num] != string.Empty)
                            {
                                if (EmailItems[num].ToLower().Contains("boundary="))
                                {
                                    try
                                    {
                                        string text = EmailItems[num].Split(new[]
                                        {
                                            "boundary="
                                        }, StringSplitOptions.None)[1];
                                        if (text.Contains(";"))
                                        {
                                            text = text.Substring(0, text.IndexOf(';'));
                                        }
                                        if (text.Contains("\""))
                                        {
                                            text = text.Replace("\"", "");
                                        }
                                        BoundaryCollection.Add(text);
                                    }
                                    catch (Exception e)
                                    {
                                        BadParseItems.Add(new ParseError(EmailItems[num], e));
                                    }
                                }
                                int num2 = EmailItems[num].IndexOf(':');
                                try
                                {
                                    if (num2 > 0 & !EmailItems[num].StartsWith('\t'
                                        .ToString(CultureInfo.InvariantCulture)) &
                                        !EmailItems[num].ToLower()
                                            .StartsWith(' '.ToString(CultureInfo.InvariantCulture)))
                                    {
                                        string text2 = EmailItems[num].Substring(0, num2);
                                        string text3 = EmailItems[num].Substring(num2 + 2);
                                        _headerLastKey = text2;
                                        bodyPart.Headers.Add(text2.Trim(new[]
                                        {
                                            ' '
                                        }), text3.Trim(new[]
                                        {
                                            ' '
                                        }));
                                    }
                                    else
                                    {
                                        Dictionary<string, string> headers = bodyPart.Headers;
                                        string headerLastKey =  _headerLastKey;
                                        headers[headerLastKey] =
                                            headers[headerLastKey] + "\n" + EmailItems[num];
                                    }
                                }
                                catch (Exception e2)
                                {
                                    BadParseItems.Add(new ParseError(EmailItems[num], e2));
                                }
                                num++;
                            }
                            num++;
                            break;
                        }
                    }
                    bodyPart.BodyIndexes.Add(num);
                    num++;
                }
            }
            Parts.Add(bodyPart);
        }
    }
}