Imports System.Runtime.InteropServices

Public Class NativeMethods
    ' Methods
    <DllImport("User32.dll")> _
    Public Shared Function DestroyIcon(ByVal hIcon As IntPtr) As Integer
    End Function

    Public Shared Function GetSystemIcon(ByVal sFilename As String) As Icon
        Dim shinfo As New SHFILEINFO
        NativeMethods.SHGetFileInfo(sFilename, 0, shinfo, Marshal.SizeOf(shinfo), &H101)
        Dim myIcon As Icon = DirectCast(Icon.FromHandle(shinfo.hIcon).Clone, Icon)
        NativeMethods.DestroyIcon(shinfo.hIcon)
        Return myIcon
    End Function

    Public Shared Function GetVisibleScrollbars(ByVal ctl As Control) As ScrollBars
        Dim wndStyle As Integer = NativeMethods.GetWindowLong(ctl.Handle, -16)
        Dim hsVisible As Boolean = ((wndStyle And &H100000) <> 0)
        Dim vsVisible As Boolean = ((wndStyle And &H200000) <> 0)
        If hsVisible Then
            Return If(vsVisible, ScrollBars.Both, ScrollBars.Horizontal)
        End If
        Return If(vsVisible, ScrollBars.Vertical, ScrollBars.None)
    End Function

    <DllImport("user32.dll", SetLastError:=True)> _
    Public Shared Function GetWindowLong(ByVal hWnd As IntPtr, ByVal nIndex As Integer) As Integer
    End Function

    <DllImport("shell32.dll", CharSet:=CharSet.Unicode)> _
    Public Shared Function SHGetFileInfo(ByVal pszPath As String, ByVal dwFileAttributes As UInt32, ByRef psfi As SHFILEINFO, ByVal cbSizeFileInfo As UInt32, ByVal uFlags As UInt32) As IntPtr
    End Function


    ' Fields
    Public Const GWL_STYLE As Integer = -16
    Public Const SHGFI_ICON As UInt32 = &H100
    Public Const SHGFI_LARGEICON As UInt32 = 0
    Public Const SHGFI_SMALLICON As UInt32 = 1
    Public Const WS_HSCROLL As Integer = &H100000
    Public Const WS_VSCROLL As Integer = &H200000
End Class



