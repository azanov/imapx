using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ImapX.Sample.Native
{
    public class NativeMethods
    {
        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0; // 'Large icon 
        public const uint SHGFI_SMALLICON = 0x1; // 'Small icon 

        public const int GWL_STYLE = -16;

        public const int WS_VSCROLL = 0x00200000;
        public const int WS_HSCROLL = 0x00100000;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi,
                                                  uint cbSizeFileInfo, uint uFlags);

        [DllImport("User32.dll")]
        public static extern int DestroyIcon(IntPtr hIcon);

        public static Icon GetSystemIcon(string sFilename)
        {
            var shinfo = new SHFILEINFO();
            SHGetFileInfo(sFilename, 0, ref shinfo, (uint) Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_SMALLICON);

            var myIcon = (Icon) Icon.FromHandle(shinfo.hIcon).Clone();
            DestroyIcon(shinfo.hIcon); // Cleanup 
            return myIcon;
        }

        public static ScrollBars GetVisibleScrollbars(Control ctl)
        {
            int wndStyle = GetWindowLong(ctl.Handle, GWL_STYLE);
            bool hsVisible = (wndStyle & WS_HSCROLL) != 0;
            bool vsVisible = (wndStyle & WS_VSCROLL) != 0;

            if (hsVisible)
                return vsVisible ? ScrollBars.Both : ScrollBars.Horizontal;
            return vsVisible ? ScrollBars.Vertical : ScrollBars.None;
        }

    }
}