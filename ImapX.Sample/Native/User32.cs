using Imapx.Sample.Native;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ImapX.Sample.Native
{
    public class User32
    {
        // ReSharper disable InconsistentNaming

        public const int CBN_DROPDOWN = 7;

        public const int EM_SETCUEBANNER = 0x1501;
        public const int EM_GETCUEBANNER = 0x1502;
        public const int CB_SETCUEBANNER = 0x1703;
        public const int CB_GETCUEBANNER = 0x1704;
         

        public const int ES_WANTRETURN = 0x1000;

        public const int GWL_WNDPROC = -4;

        public const int HT_CAPTION = 0x2;

        public const int LB_ITEMFROMPOINT = 425;

        public const int LVS_EX_BORDERSELECT = 0x00008000;
        public const int LVS_EX_DOUBLEBUFFER = 0x00010000;

        public const int WM_COMMAND = 0x111;
        public const int WM_CREATE = 0x0001;
        public const int WM_KEYDOWN = 0x100;
        public const int WM_KILLFOCUS = 0x0008;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int WM_MOUSEMOVE = 0x200;
        public const int WM_PARENTNOTIFY = 0x210;
        public const int WM_REFLECT = WM_USER + 0x1C00;
        public const int WM_SETFONT = 0x30;
        public const int WM_USER = 0x0400;
        public const int WM_VSCROLL = 0x115;
        public const int WM_HSCROLL = 0x114;
        public const int WM_MOUSEWHEEL = 0x020A;

        public const int WS_CHILD = 0x40000000;
        public const int WS_VISIBLE = 0x10000000;

        // ReSharper enable InconsistentNaming

        public delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool InvalidateRect(IntPtr hWnd, ref RECT rect, bool bErase);

        [DllImport("user32.dll")]
        public static extern bool GetComboBoxInfo(IntPtr hwnd, ref COMBOBOXINFO pcbi);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, [MarshalAs(UnmanagedType.Bool)] bool wParam,
            [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int msg, int wParam, StringBuilder lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr CreateWindowEx(int dwExStyle, string lpClassName, string lpWindowName, int dwStyle,
            int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool DestroyWindow(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetWindowText(IntPtr hwnd, String lpString);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr newWndProc);

        [DllImport("user32.dll")]
        public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
    }
}