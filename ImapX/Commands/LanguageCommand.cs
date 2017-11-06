namespace ImapX.Commands
{
    public class LanguageCommand : ImapCommand<string[]>
    {
        public LanguageCommand(ImapBase imapBase, long id, string language = null) 
            : base(imapBase, id, string.Format("LANGUAGE{0}\r\n", string.IsNullOrWhiteSpace(language) ? "" : " " + language))
        {
        }

        public override bool HandleSpecificUntaggedResponse(ImapParser io, ImapToken responseToken)
        {
            if (responseToken.Value != "LANGUAGE")
                return base.HandleSpecificUntaggedResponse(io, responseToken);

            io.ReadStringList();

            return true;
        }
    }
}
