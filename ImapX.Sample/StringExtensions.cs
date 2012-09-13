using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace System
{
    public static class StringExtensions
    {
        public static string MD5(this string input)
        {
            byte[] result =
                (CryptoConfig.CreateFromName("MD5") as HashAlgorithm).ComputeHash(Encoding.UTF8.GetBytes(input));
            var output = new StringBuilder();
            foreach (byte t in result)
            {
                output.Append(t.ToString("x2", CultureInfo.InvariantCulture));
            }
            return output.ToString();
        }
    }
}