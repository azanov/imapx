using System.ComponentModel;

namespace ImapX.Enums
{
    [DefaultValue(Off)]
    public enum IdleState
    {
        Off = 0,
        On = 1,
        Paused = 2,
        Stopping = 4
    }
}
