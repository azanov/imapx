using System;
using System.Collections.Generic;
namespace EmailParser
{
    public class BodyPart
    {
        private string _boundary;
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
        public string Boundary
        {
            get
            {
                return this._boundary;
            }
            set
            {
                this._boundary = value;
            }
        }
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
