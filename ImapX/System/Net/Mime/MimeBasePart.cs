using System.Text;

namespace System.Net.Mime
{
    internal class MimeBasePart
    {
        internal static Encoding DecodeEncoding(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            string[] array = value.Split(new[]
            {
                '?',
                '\r',
                '\n'
            });
            if (array.Length < 5 || array[0] != "=" || array[4] != "=")
            {
                return null;
            }
            string name = array[1];
            return Encoding.GetEncoding(name);
        }
    }
}