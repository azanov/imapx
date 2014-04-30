using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ImapX.Enums
{
    [DefaultValue(Off)]
    public enum IdleState
    {
        Off = 0,
        On = 1,
        Paused = 2,
        Starting = 4
    }
}
