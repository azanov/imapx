namespace ImapX
{
    public enum TokenType
    {
        Eos = -1,
        Atom,
        Quoted,
        Literal,
        Nil,
        Flag,
        Number,

        Asterisk = ImapChars.Asterisk,
        OpeningBracket = ImapChars.OpeningBracket,
        ClosingBracket = ImapChars.ClosingBracket,

        OpeningParenthesis = ImapChars.OpeningParenthesis,
        ClosingParenthesis = ImapChars.ClosingParenthesis,

        OpeningBrace = ImapChars.OpeningBrace,
        ClosingBrace = ImapChars.ClosingBrace,

        Eol = ImapChars.LF,
    }
}
