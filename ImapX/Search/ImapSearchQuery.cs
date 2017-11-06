using System;

namespace ImapX.Search
{
    public abstract class ImapSearchQuery
    {
        public abstract string ToArgument();

        public override string ToString()
        {
            return ToArgument();
        }
    }
    
    public class OrExpression : ImapSearchQuery
    {
        public ImapSearchQuery LeftExpression { get; set; }
        public ImapSearchQuery RightExpression { get; set; }

        public OrExpression(ImapSearchQuery left, ImapSearchQuery right)
        {
            LeftExpression = left;
            RightExpression = right;
        }

        public override string ToArgument()
        {
            return string.Format("OR ({0}) ({1})", LeftExpression.ToArgument(), RightExpression.ToArgument());
        }
    }

    public class AndExpression : ImapSearchQuery
    {
        public ImapSearchQuery LeftExpression { get; set; }
        public ImapSearchQuery RightExpression { get; set; }

        public AndExpression(ImapSearchQuery left, ImapSearchQuery right)
        {
            LeftExpression = left;
            RightExpression = right;
        }

        public override string ToArgument()
        {
            return string.Format("{0} {1}", LeftExpression, RightExpression);
        }
    }

    public class NotExpression : ImapSearchQuery
    {
        public ImapSearchQuery Expression { get; set; }

        public NotExpression(ImapSearchQuery expr)
        {
            Expression = expr;
        }

        public override string ToArgument()
        {
            throw new NotImplementedException();
        }
    }
}
