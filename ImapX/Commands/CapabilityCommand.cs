using ImapX.Constants;
using ImapX.Enums;
using ImapX.Exceptions;
using System;

namespace ImapX.Commands
{
    public class CapabilityCommand : ImapCommand
    {
        public CapabilityCommand(ImapBase imapBase, long id) : this(imapBase, id, "CAPABILITY\r\n")
        {
        }

        public CapabilityCommand(ImapBase imapBase, long id, string code) : base(imapBase, id, code)
        {
        }
        
        public override bool HandleSpecificUntaggedResponse(ImapParser io, ImapToken responseToken)
        {
            if (responseToken.Value == "CAPABILITY")
            {
                ImapToken token;
                Base.Capabilities = new Capability();

                while ((token = io.PeekToken()).Type == TokenType.Atom)
                {
                    io.ReadToken();
                    Base.Capabilities.Add(token.Value);
                }

                return true;
            }

            return base.HandleSpecificUntaggedResponse(io, responseToken);
        }

        public override void OnCommandComplete()
        {
            if (Base.Capabilities.UTF8Accept)
                Base.Enable("UTF8=ACCEPT");
            else
                Base.EncodingMode = ImapEncodingMode.ImapUTF7;
        }
    }
}
