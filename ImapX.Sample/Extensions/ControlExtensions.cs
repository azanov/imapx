using Imapx.Sample.Native;
using ImapX.Sample.Extenders;
using ImapX.Sample.Native;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ImapX.Sample.Extensions
{
    public static class ControlExtensions
    {
        public static void NotifyMovable(this Form form)
        {
            User32.ReleaseCapture();
            User32.SendMessage(form.Handle, User32.WM_NCLBUTTONDOWN, User32.HT_CAPTION, 0);
        }

        public static void SetCueText(this Control control, string text, bool showOnFocus)
        {
            if (control is ICueBannered)
                ((ICueBannered)control).SetCueText(text, showOnFocus);
            else if (control is ComboBox)
            {
                var info = new COMBOBOXINFO();
                info.cbSize = Marshal.SizeOf(info);
                User32.GetComboBoxInfo(control.Handle, ref info);
                User32.SendMessage(info.hwndItem, User32.CB_SETCUEBANNER, false, text);
            }
            else
                User32.SendMessage(control.Handle, User32.EM_SETCUEBANNER, showOnFocus, text);
        }

        public static string GetCueText(this Control control)
        {
            return GetCueText(control.Handle, control is ComboBox);
        }

        internal static string GetCueText(IntPtr handle, bool comboBox)
        {
            var builder = new StringBuilder();
            if (comboBox)
            {
                var info = new COMBOBOXINFO();
                info.cbSize = Marshal.SizeOf(info);
                User32.GetComboBoxInfo(handle, ref info);
                User32.SendMessage(info.hwndItem, User32.CB_GETCUEBANNER, 0, builder);
            }
            else
                User32.SendMessage(handle, User32.EM_GETCUEBANNER, 0, builder);
            return builder.ToString();
        }

    }
}