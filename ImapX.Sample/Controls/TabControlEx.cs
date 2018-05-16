using Imapx.Sample.Native;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ImapX.Sample.Controls
{
    public class TabControlEx : TabControl
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        public bool RemoveGap { get; set; }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == 0x1300 + 40 && RemoveGap)
            {
                var rc = (RECT)m.GetLParam(typeof(RECT));
                rc.Left -= 7;
                rc.Right += 7;
                rc.Top -= 7;
                rc.Bottom += 7;
                Marshal.StructureToPtr(rc, m.LParam, true);
            }
            base.WndProc(ref m);
        }
    }
}