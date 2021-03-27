using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GhostCore.Win32
{
    public static class NTDLL
    {
        [DllImport("ntdll.dll", SetLastError = false)]
        internal static extern IntPtr NtSuspendProcess(IntPtr ProcessHandle);


        [DllImport("ntdll.dll", SetLastError = true)]
        internal static extern IntPtr NtResumeProcess(IntPtr ProcessHandle);
    }
}
