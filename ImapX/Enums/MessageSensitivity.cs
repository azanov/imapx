using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ImapX.Enums
{
    [DefaultValue(None)]
    public enum MessageSensitivity
    {
        None,
        Personal,
        Private,
        CompanyConfidential
    }
}
