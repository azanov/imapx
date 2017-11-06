using System.ComponentModel;

namespace ImapX.Enums
{
    [DefaultValue(Idle)]
    public enum ImapBaseState
    {
        Idle,
        CommandInProgress
    }
}
