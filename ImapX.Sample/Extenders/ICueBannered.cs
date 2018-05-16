namespace ImapX.Sample.Extenders
{
    public interface ICueBannered
    {
        void SetCueText(string text, bool showOnFocus);
        string GetCueText();
    }
}