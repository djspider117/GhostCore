using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GhostCore.Win32
{
    public static class Kernel32
    {
        [DllImport("kernel32.dll")]
        public static extern void RtlMoveMemory(IntPtr dest, IntPtr source, int Length);

        [DllImport("Kernel32")]
        public static extern void AllocConsole();

        [DllImport("Kernel32")]
        public static extern void FreeConsole();
    }
}
