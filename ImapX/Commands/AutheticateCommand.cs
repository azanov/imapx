using ImapX.Authentication;

namespace ImapX.Commands
{
    public abstract class AutheticateCommand : CapabilityCommand
    {
        public AutheticateCommand(ImapBase imapBase, long id, string mechanism) : base(imapBase, id, "AUTHENTICATE " + mechanism + "\r\n")
        {
        }

        
    }
}
