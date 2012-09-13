using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ImapX.Sample.Native
{
    public class NativeMethods
    {
        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0; // 'Large icon 
        public const uint SHGFI_SMALLICON = 0x1; // 'Small icon 

        [DllImport("shell32.dll")]
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
    }
}