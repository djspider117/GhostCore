using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GhostCore.Win32
{
    public static class GDI32
    {
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
    }
}
