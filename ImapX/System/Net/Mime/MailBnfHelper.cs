using System.Linq;
using System.Text;

namespace System.Net.Mime
{
    public class MailBnfHelper
    {
        internal static bool[] Ttext;
        internal static bool[] Qtext;
        internal static readonly char Cr;
        internal static readonly char Lf;
        internal static readonly char Space;
        internal static readonly char Tab;
        internal static readonly int Ascii7BitMaxValue;

        static MailBnfHelper()
        {
            Qtext = new bool[128];
            Ttext = new bool[128];
            Ascii7BitMaxValue = 127;
            Space = ' ';
            Tab = '\t';
            Cr = '\r';
            Lf = '\n';

            for (int l = 1; l <= 9; l++)
            {
                Qtext[l] = true;
            }
            Qtext[11] = true;
            Qtext[12] = true;
            for (int m = 14; m <= 33; m++)
            {
                Qtext[m] = true;
            }
            for (int n = 35; n <= 91; n++)
            {
                Qtext[n] = true;
            }
            for (int num = 93; num <= 127; num++)
            {
                Qtext[num] = true;
            }
            for (int num8 = 33; num8 <= 126; num8++)
            {
                Ttext[num8] = true;
            }
            Ttext[40] = false;
            Ttext[41] = false;
            Ttext[60] = false;
            Ttext[62] = false;
            Ttext[64] = false;
            Ttext[44] = false;
            Ttext[59] = false;
            Ttext[58] = false;
            Ttext[92] = false;
            Ttext[34] = false;
            Ttext[47] = false;
            Ttext[91] = false;
            Ttext[93] = false;
            Ttext[63] = false;
            Ttext[61] = false;
        }

        internal static bool HasCrOrLf(string data)
        {
            return data.ToCharArray().Any(t => t == '\r' || t == '\n');
        }

        internal static bool SkipCfWs(string data, ref int offset)
        {
            int num = 0;
            while (offset < data.Length)
            {
                if (data[offset] > '\u007f')
                {
                    throw new FormatException();
                }
                if (data[offset] == '\\' && num > 0)
                {
                    offset += 2;
                }
                else
                {
                    if (data[offset] == '(')
                    {
                        num++;
                    }
                    else
                    {
                        if (data[offset] == ')')
                        {
                            num--;
                        }
                        else
                        {
                            if (data[offset] != ' ' && data[offset] != '\t' && num == 0)
                            {
                                return true;
                            }
                        }
                    }
                }
                if (num < 0)
                {
                    throw new FormatException();
                }
                offset++;
            }
            return false;
        }

        internal static string ReadParameterAttribute(string data, ref int offset, StringBuilder builder)
        {
            if (!SkipCfWs(data, ref offset))
            {
                return null;
            }
            return ReadToken(data, ref offset, null);
        }

        internal static string ReadToken(string data, ref int offset, StringBuilder builder)
        {
            int num = offset;
            while (offset < data.Length)
            {
                if (data[offset] > Ascii7BitMaxValue)
                {
                    throw new FormatException();
                }
                if (!Ttext[data[offset]])
                {
                    break;
                }
                offset++;
            }
            if (num == offset)
            {
                throw new FormatException();
            }
            return data.Substring(num, offset - num);
        }

        internal static string ReadQuotedString(string data, ref int offset, StringBuilder builder)
        {
            return ReadQuotedString(data, ref offset, builder, false, false);
        }

        internal static string ReadQuotedString(string data, ref int offset, StringBuilder builder,
            bool doesntRequireQuotes, bool permitUnicodeInDisplayName)
        {
            if (!doesntRequireQuotes)
            {
                offset++;
            }
            int num = offset;
            StringBuilder stringBuilder = builder ?? new StringBuilder();
            while (offset < data.Length)
            {
                if (data[offset] == '\\')
                {
                    stringBuilder.Append(data, num, offset - num);
                    num = ++offset;
                }
                else
                {
                    if (data[offset] == '"')
                    {
                        stringBuilder.Append(data, num, offset - num);
                        offset++;
                        if (builder == null)
                        {
                            return stringBuilder.ToString();
                        }
                        return null;
                    }
                    if (data[offset] == '=' && data.Length > offset + 3 && data[offset + 1] == '\r' &&
                        data[offset + 2] == '\n' && (data[offset + 3] == ' ' || data[offset + 3] == '\t'))
                    {
                        offset += 3;
                    }
                    else
                    {
                        if (permitUnicodeInDisplayName)
                        {
                            if (data[offset] <= Ascii7BitMaxValue && !Qtext[data[offset]])
                            {
                                throw new FormatException();
                            }
                        }
                        else
                        {
                            if (data[offset] > Ascii7BitMaxValue || !Qtext[data[offset]])
                            {
                                throw new FormatException();
                            }
                        }
                    }
                }
                offset++;
            }
            if (!doesntRequireQuotes)
            {
                throw new FormatException();
            }
            stringBuilder.Append(data, num, offset - num);
            if (builder == null)
            {
                return stringBuilder.ToString();
            }
            return null;
        }

        internal static bool IsFwsAt(string data, int index)
        {
            return data[index] == Cr && index + 2 < data.Length && data[index + 1] == Lf &&
                   (data[index + 2] == Space || data[index + 2] == Tab);
        }

        internal static void GetTokenOrQuotedString(string data, StringBuilder builder, bool allowUnicode)
        {
            int i = 0;
            int num = 0;
            while (i < data.Length)
            {
                if (!CheckForUnicode(data[i], allowUnicode) && (!Ttext[data[i]] || data[i] == ' '))
                {
                    builder.Append('"');
                    while (i < data.Length)
                    {
                        if (!CheckForUnicode(data[i], allowUnicode))
                        {
                            if (IsFwsAt(data, i))
                            {
                                i++;
                                i++;
                            }
                            else
                            {
                                if (!Qtext[data[i]])
                                {
                                    builder.Append(data, num, i - num);
                                    builder.Append('\\');
                                    num = i;
                                }
                            }
                        }
                        i++;
                    }
                    builder.Append(data, num, i - num);
                    builder.Append('"');
                    return;
                }
                i++;
            }
            if (data.Length == 0)
            {
                builder.Append("\"\"");
            }
            builder.Append(data);
        }

        private static bool CheckForUnicode(char ch, bool allowUnicode)
        {
            if (ch < Ascii7BitMaxValue)
            {
                return false;
            }
            if (!allowUnicode)
            {
                throw new FormatException();
            }
            return true;
        }
    }
}