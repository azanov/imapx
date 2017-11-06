namespace ImapX.Commands
{
    public class EnableCommand : ImapCommand
    {
        public EnableCommand(ImapBase imapBase, long id, string capability) : base(imapBase, id, string.Format("ENABLE {0}\r\n", capability))
        {
        }

        public override bool HandleSpecificUntaggedResponse(ImapParser io, ImapToken responseToken)
        {
            if (responseToken.Value == "ENABLED")
            {
                io.ReadAtom();
                return true;
            }

            return base.HandleSpecificUntaggedResponse(io, responseToken);

        }
    }
}
