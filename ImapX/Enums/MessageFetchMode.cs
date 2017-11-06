using System;
using System.ComponentModel;

namespace ImapX.Enums
{
    [DefaultValue(None), Flags]
    public enum MessageFetchMode
    {
        None = -2,
        ClientDefault = -1,

        Initial = 0,

        /// <summary>
        /// Request message flags
        /// </summary>
        Flags = 1,

        ContentType = 2,
        InternalDate = 4,
        Size = 8,
        Headers = 16,

        Envelope = 32,

        BodyStructure = 64,
        Body = BodyStructure | 128,
        Attachments = BodyStructure | 256,

        GMailMessageId = 512,
        GMailThreads = 1024,
        GMailLabels = 2048,

        BodyPart = 4096,
        BodyText = 8192,

        GMailExtendedData = GMailMessageId | GMailLabels | GMailThreads,

        /// <summary>
        /// Request flags, envelope and the body structure
        /// </summary>
        Tiny = Flags | Envelope | BodyStructure,

        /// <summary>
        /// Request flags, enevelope, body structure, size and internal date
        /// </summary>
        Minimal = Tiny | Size | InternalDate,

        /// <summary>
        /// Request flags, headers, body structure and body, size and internal date
        /// </summary>
        Basic = Minimal | Headers | Body,

        /// <summary>
        /// Request flags, headers, body and attachments, size and internal date
        /// </summary>
        Full = Basic | Headers | Attachments

    }
}