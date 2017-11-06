using ImapX.Exceptions;
using ImapX.Extensions;
using System.Text;

namespace ImapX.Commands
{
    public class IDCommand : ImapCommand<ImapIdentity>
    {
        public IDCommand(ImapClient client, long id, ImapIdentity identity = null) : base(client, id, "ID")
        {
            var sb = new StringBuilder("");
            if (identity == null || identity.Count == 0)
                sb.Append(" NIL");
            else
            {
                sb.Append(ImapChars.OpeningParenthesis);
                foreach (var pair in identity)
                    sb.AppendFormat(" \"{0}\" \"{1}\"", pair.Key.Quote(), pair.Value.Quote());
                sb.Append(ImapChars.ClosingParenthesis);
            }

            sb.Append("\r\n");

            Parts[0] = Parts[0] + sb.ToString();
        }
        
        public override bool HandleSpecificUntaggedResponse(ImapParser io, ImapToken responseToken)
        {
            if (responseToken.Value != "ID")
                return base.HandleSpecificUntaggedResponse(io, responseToken);

            Response = new ImapIdentity();
            
            ImapToken token = io.ReadToken();
            if (token.Type == TokenType.OpeningParenthesis)
            {
                while (true)
                {
                    if (io.PeekToken().Type == TokenType.ClosingParenthesis)
                        break;
                    Response[io.ReadString()] = io.ReadNullableString();
                }
                io.ConsumeClosingParenthesis();
            }
            else if (token.Type != TokenType.Nil)
                throw new UnexpectedTokenException();

            //io.ConsumeTillEol();
            return true;
        }
    }
}
