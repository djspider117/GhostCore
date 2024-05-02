using System;
using System.Runtime.InteropServices;

namespace GhostCore.Win32
{
    public static class GDI32
    {
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
    }
}
