using System;
using System.Runtime.InteropServices;

namespace GhostCore.Win32
{
    public static class User32
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr hWnd, string text, string caption, int options);
    }
}
