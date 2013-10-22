using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using ImapX.Enums;

namespace ImapX.Extensions
{
    public static class EnumExtensions
    {
        public static MessageImportance ToMessageImportance(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return MessageImportance.Normal;

            switch (value.ToLower())
            {
                case "high":
                    return MessageImportance.High;
                case "medium":
                    return MessageImportance.Medium;
                case "low":
                    return MessageImportance.Low;
                default:
                    return MessageImportance.Normal;
            }
        }

        public static MessageSensitivity ToMessageSensitivity(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return MessageSensitivity.None;

            switch (value.ToLower())
            {
                case "personal":
                    return MessageSensitivity.Personal;
                case "private":
                    return MessageSensitivity.Private;
                case "company confidential":
                    return MessageSensitivity.CompanyConfidential;
                default:
                    return MessageSensitivity.None;
            }
        }

        public static ContentTransferEncoding ToContentTransferEncoding(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return ContentTransferEncoding.Unknown;

            switch (value.ToLower())
            {
                case "7bit":
                    return ContentTransferEncoding.SevenBit;
                case "8bit":
                    return ContentTransferEncoding.EightBit;
                case "binary":
                    return ContentTransferEncoding.Binary;
                case "quoted-printable":
                    return ContentTransferEncoding.QuotedPrintable;
                case "base64":
                    return ContentTransferEncoding.Base64;
                default:
                    return ContentTransferEncoding.Unknown;
            }
        }

        public static TransferEncoding ToMimeTransferEncoding(this ContentTransferEncoding value)
        {
            switch (value)
            {
                case ContentTransferEncoding.Base64:
                    return TransferEncoding.Base64;
                case ContentTransferEncoding.QuotedPrintable:
                    return TransferEncoding.QuotedPrintable;
                case ContentTransferEncoding.SevenBit:
                    return TransferEncoding.SevenBit;
                default:
                    return TransferEncoding.Unknown;
            }
        }



        public static bool HasFlag(this Enum value, Enum flag)
        {
            ulong keysVal = Convert.ToUInt64(value);
            ulong flagVal = Convert.ToUInt64(flag);

            return (keysVal & flagVal) == flagVal;

        }



    }
}
