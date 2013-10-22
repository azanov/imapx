using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImapX.Constants;
using ImapX.Parsing;

namespace ImapX
{
    internal class MessageUploader : CommandProcessor
    {

        private string _eml;

        public MessageUploader(string eml)
        {
            _eml = eml;
            TwoWayProcessing = true;
        }

        public MessageUploader(Message msg) : this(msg.ToEml())
        {
            
        }

        public override void ProcessCommandResult(string data)
        {
            
        }

        public override byte[] AppendCommandData(string serverResponse)
        {
            return Encoding.UTF8.GetBytes((_eml ?? "") + "\r\n");
        }
    }
}
