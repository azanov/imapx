using System;
using System.IO;
#if !NETFX_CORE
using System.Security.Cryptography;
#endif
using System.Text;
using System.Text.RegularExpressions;

namespace ImapX.EncodingHelpers
{
    /// <summary>
    ///     Transforms text to and from base64 encoding using streams.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The built in System.Convert.ToBase64String and FromBase64String methods are prone
    ///         to error with OutOfMemoryException when used with larger strings or byte arrays.
    ///     </para>
    ///     <para>
    ///         This class remedies the problem by using classes from the System.Security.Cryptography
    ///         namespace to do the byte conversion with streams and buffered output.
    ///     </para>
    /// </remarks>
    /// <see
    ///     cref="http://www.tribridge.com/Blog/crm/default/2011-04-29/Solving-OutOfMemoryException-errors-when-attempting-to-attach-large-Base64-encoded-content-into-CRM-annotations.aspx" />
    public static class Base64
    {
        /// <summary>
        ///     Converts a byte array to a base64 string one block at a time.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string ToBase64(byte[] data)
        {

#if WINDOWS_PHONE || NETFX_CORE
            return Convert.ToBase64String(data);
#else
            var builder = new StringBuilder();
            using (var writer = new StringWriter(builder))
            {
                using (var transformation = new ToBase64Transform())
                {
                    // Transform the data in chunks the size of InputBlockSize.

                    var bufferedOutputBytes = new byte[transformation.OutputBlockSize];

                    int i = 0;

                    int inputBlockSize = transformation.InputBlockSize;

                    while (data.Length - i > inputBlockSize)
                    {
                        transformation.TransformBlock(data, i, data.Length - i, bufferedOutputBytes, 0);


                        i += inputBlockSize;


                        writer.Write(Encoding.UTF8.GetString(bufferedOutputBytes, 0, bufferedOutputBytes.Length));
                    }

                    // Transform the final block of data.

                    bufferedOutputBytes = transformation.TransformFinalBlock(data, i, data.Length - i);

                    writer.Write(Encoding.UTF8.GetString(bufferedOutputBytes, 0, bufferedOutputBytes.Length));

                    // Free up any used resources.

                    transformation.Clear();
                }

                writer.Close();
            }
            return builder.ToString();
#endif
        }

        /// <summary>
        ///     Converts a base64 string to a byte array.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static byte[] FromBase64(string s)
        {
#if WINDOWS_PHONE || NETFX_CORE
            return Convert.FromBase64String(s);
#else
            byte[] bytes;

            //s = Regex.Replace(Regex.Replace(s, @"\r\n?|\n", string.Empty), @"--.*", string.Empty);
            using (var writer = new MemoryStream())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(s);

                using (var transformation = new FromBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces))
                {
                    var bufferedOutputBytes = new byte[transformation.OutputBlockSize];

                    // Transform the data in chunks the size of InputBlockSize.

                    int i = 0;

                    while (inputBytes.Length - i > 4)
                    {
                        transformation.TransformBlock(inputBytes, i, 4, bufferedOutputBytes, 0);


                        i += 4;


                        writer.Write(bufferedOutputBytes, 0, transformation.OutputBlockSize);
                    }

                    // Transform the final block of data.

                    bufferedOutputBytes = transformation.TransformFinalBlock(inputBytes, i, inputBytes.Length - i);

                    writer.Write(bufferedOutputBytes, 0, bufferedOutputBytes.Length);

                    // Free up any used resources.

                    transformation.Clear();
                }

                writer.Position = 0;

                bytes = writer.ToArray();

                writer.Close();
            }
            return bytes;
#endif
        }
    }
}