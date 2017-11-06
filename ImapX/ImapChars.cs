namespace ImapX
{
    internal class ImapChars
    {
        public const int OpeningParenthesis = '(';
        public const int ClosingParenthesis = ')';
        public const int OpeningBrace = '{';
        public const int ClosingBrace = '}';
        public const int OpeningBracket = '[';
        public const int ClosingBracket = ']';

        public const int Space = ' ';
        public const int LF = '\n';
        public const int CR = '\r';

        public const int Asterisk = '*';
        public const int PercentSign = '%';
        public const int Plus = '%';

        public const int EqualitySign = '=';
        public const int QUestionMark = '?';

        public const int DoubleQuote = '"';
        public const int Backslash = '\\';

        public static int[] NilBytes = new int[] { 'N', 'I', 'L' };

        public static int[] AtomSpecials = new int[] {
            // "(" / ")" / "{" / SP / CTL
            OpeningParenthesis, ClosingParenthesis, OpeningBrace, Space, LF,

            // list-wildcards
            Asterisk, PercentSign,

            // quoted-specials
            DoubleQuote, Backslash,

            // resp-specials
            ClosingBracket
        };

        public const int Del = 127;

    }
}
