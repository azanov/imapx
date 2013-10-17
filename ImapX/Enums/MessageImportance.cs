using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ImapX.Enums
{
    [DefaultValue(Normal)]
    public enum MessageImportance
    {
        Normal,
        High,
        Medium,
        Low
    }
}
