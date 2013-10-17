using System;
using System.ComponentModel;

namespace ImapX.Enums
{
    [DefaultValue(None), Flags]
    public enum MessageFetchMode
    {
        None = -2,
        ClientDefault = -1,

        /// <summary>
        /// Request message flags
        /// </summary>
        Flags = 1,

        InternalDate = 2,
        Size = 4,
        Headers = 8,

        BodyStructure = 16,
        Body = BodyStructure | 32,
        Attachments = BodyStructure | 64,

        GMailMessageId = 128,
        GMailThreads = 256,
        GMailLabels = 512,

        GMailExtendedData = GMailMessageId | GMailLabels | GMailThreads,

        /// <summary>
        /// Request flags, headers and the body structure
        /// </summary>
        Tiny = Flags | Headers | BodyStructure,

        /// <summary>
        /// Request flags, headers, body structure, size and internal date
        /// </summary>
        Minimal = Tiny | Size | InternalDate,

        /// <summary>
        /// Request flags, headers, body structure and body, size and internal date
        /// </summary>
        Basic = Minimal | Body,

        /// <summary>
        /// Request flags, headers, body and attachments, size and internal date
        /// </summary>
        Full = Basic | Attachments
        
    }
}