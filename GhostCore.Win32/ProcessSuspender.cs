using System;
using System.Diagnostics;
using System.Linq;

namespace GhostCore.Win32
{
    public static class ProcessSuspender
    {
        public static void SuspendProcess(IntPtr handle)
        {
            NTDLL.NtSuspendProcess(handle);
        }

        public static void SuspendProcess(string procName)
        {
            var proc = Process.GetProcessesByName(procName)?.FirstOrDefault();
            if (proc == null)
                return;

            NTDLL.NtSuspendProcess(proc.Handle);
        }

        public static void ResumeProcess(IntPtr handle)
        {
            NTDLL.NtResumeProcess(handle);
        }

        public static void ResumeProcess(string procName)
        {
            var proc = Process.GetProcessesByName(procName)?.FirstOrDefault();
            if (proc == null)
                return;

            NTDLL.NtResumeProcess(proc.Handle);
        }
    }
}
