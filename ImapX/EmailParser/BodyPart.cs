using System.Collections.Generic;

namespace ImapX.EmailParser
{
    public class BodyPart
    {
    	private List<int> _body = new List<int>();
        private Dictionary<string, string> _headers = new Dictionary<string, string>();
        public Dictionary<string, string> Headers
        {
            get
            {
                return this._headers;
            }
            set
            {
                this._headers = value;
            }
        }

    	public string Boundary { get; set; }

    	public List<int> BodyIndexes
        {
            get
            {
                return this._body;
            }
            set
            {
                this._body = value;
            }
        }
    }
}