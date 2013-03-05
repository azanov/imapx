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
        public string[] _emailItems;
        private bool isEmailValid = true;
        public Dictionary<string, string> _headersCollection;
        private string _headerLastKey;
        public List<ParseError> _badParseItems = new List<ParseError>();
        public List<BodyPart> _parts = new List<BodyPart>();
        private int _headerStartIndex;
        private int _headerEndIndex;
        private int _bodyStartIndex;
        private int _bodyEndIndex;
        public List<string> _boundaryCollection;
        public EmailParser(string[] email)
        {
            this._headersCollection = new Dictionary<string, string>();
            this._boundaryCollection = new List<string>();
            this._emailItems = email;
        }
        public void InitializeIndexes()
        {
            for (int i = 0; i < this._emailItems.Length; i++)
            {
                if (string.IsNullOrEmpty(this._emailItems[i]))
                {
                    this._headerStartIndex = 0;
                    this._headerEndIndex = i;
                    this._bodyStartIndex = i + 1;
                    this._bodyEndIndex = this._emailItems.Length - 2;
                    return;
                }
            }
        }
        public void ParseHeaders()
        {
            for (int i = this._headerStartIndex; i < this._headerEndIndex; i++)
            {
                if (this._emailItems[i].Contains("boundary="))
                {
                    try
                    {
                        string text = this._emailItems[i].Split(new[]
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
                        this._boundaryCollection.Add(text);
                    }
                    catch (Exception e)
                    {
                        this._badParseItems.Add(new ParseError(this._emailItems[i], e));
                    }
                }
                int num = this._emailItems[i].IndexOf(':');

                // Fix provided by iamwill 12/12/12
                // For reference see http://imapx.codeplex.com/workitem/1424

                // Fix provided by itreims  03/05/13
                // For reference see http://imapx.codeplex.com/workitem/1523
                try 
                { 
                    if (num > 0 & !this._emailItems[i].StartsWith('\t'.ToString(CultureInfo.InvariantCulture)) & !this._emailItems[i].StartsWith(' '.ToString(CultureInfo.InvariantCulture))) 
                    { 
                        string text2 = this._emailItems[i].Substring(0, num);
                        string text3 = String.Empty;
                        if (this._emailItems[i].Length > num + 1)
                        {
                            text3 = this._emailItems[i].Substring(num + 2);
                        }
                        this._headerLastKey = text2; 
                        var trimmedText2 = text2.Trim(new[] { ' ' }); 
                        var trimmedText3 = text3.Trim(new[] { ' ' }); 
                        
                        if (!this._headersCollection.ContainsKey(trimmedText2)) 
                        { 
                            this._headersCollection.Add(trimmedText2, trimmedText3); 
                        } 
                    } 
                    else
                    { 
                        Dictionary<string, string> headersCollection; string headerLastKey; 
                        if (this._headerLastKey != null && this._headersCollection.ContainsKey(this._headerLastKey))
                        {
                            (headersCollection = this._headersCollection)[headerLastKey = this._headerLastKey] = headersCollection[headerLastKey] + "\n" + this._emailItems[i];
                        } 
                    } 
                }
                catch (Exception e2) 
                { 
                    this._badParseItems.Add(new ParseError(this._emailItems[i], e2)); 
                }
            }
        }
        public string GetPart(BodyPart p)
        {
            var stringBuilder = new StringBuilder();
            bool flag = true;
            foreach (int current in p.BodyIndexes)
            {
                if (current < 0) continue;
                if (flag)
                {
                    stringBuilder.Append(this._emailItems[current >= _emailItems.Length ? _emailItems.Length - 1 : current]);
                    
                    flag = false;
                }
                else
                {
                    stringBuilder.Append("\r\n" + this._emailItems[current >= _emailItems.Length ? _emailItems.Length - 1 : current]);
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
        	if (this._boundaryCollection.Count < 1)
            {
                for (int i = this._bodyStartIndex; i <= this._bodyEndIndex; i++)
                {
                    bodyPart.BodyIndexes.Add(i);
                }
            }
            else
            {
                int num = this._bodyStartIndex;
                while (num < this._bodyEndIndex && (this._boundaryCollection.Count <= 0 || !this._emailItems[num].Contains(this._boundaryCollection[0] + "--")))
                {
                    foreach (string current in this._boundaryCollection)
                    {
                        if (this._emailItems[num].Contains(current))
                        {
                            if (bodyPart.Boundary != null)
                            {
                                this._parts.Add(bodyPart);
                            }
                        	bodyPart = new BodyPart {Boundary = current};
                        	num++;
                            while (this._bodyEndIndex >= num && this._emailItems[num] != string.Empty)
                            {
                                if (this._emailItems[num].Contains("boundary="))
                                {
                                    try
                                    {
                                        string text = this._emailItems[num].Split(new[]
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
                                        this._boundaryCollection.Add(text);
                                    }
                                    catch (Exception e)
                                    {
                                        this._badParseItems.Add(new ParseError(this._emailItems[num], e));
                                    }
                                }
                                int num2 = this._emailItems[num].IndexOf(':');
                                try
                                {
                                    if (num2 > 0 & !this._emailItems[num].StartsWith('\t'
										.ToString(CultureInfo.InvariantCulture)) & !this._emailItems[num].StartsWith(' '.ToString(CultureInfo.InvariantCulture)))
                                    {
                                        string text2 = this._emailItems[num].Substring(0, num2);
                                        string text3 = this._emailItems[num].Substring(num2 + 2);
                                        this._headerLastKey = text2;
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
                                        Dictionary<string, string> headers;
                                        string headerLastKey;
                                        (headers = bodyPart.Headers)[headerLastKey = this._headerLastKey] = headers[headerLastKey] + "\n" + this._emailItems[num];
                                    }
                                }
                                catch (Exception e2)
                                {
                                    this._badParseItems.Add(new ParseError(this._emailItems[num], e2));
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
            this._parts.Add(bodyPart);
        }
    }
}
