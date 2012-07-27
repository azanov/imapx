using System;
namespace EmailParser
{
    public class ParseError
    {
        private string _item;
        private Exception _exception;
        public string ItemString
        {
            get
            {
                return this._item;
            }
            set
            {
                this._item = value;
            }
        }
        public Exception ThrowedException
        {
            get
            {
                return this._exception;
            }
            set
            {
                this._exception = value;
            }
        }
        public ParseError(string item, Exception e)
        {
            this._item = item;
            this._exception = e;
        }
    }
}
