using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysDebug = System.Diagnostics.Debug;

namespace GhostCore.DebugUtils
{
    internal class StandardDebugOutput : IDebugOutput
    {
        public void Log(object item)
        {
            SysDebug.WriteLine(item);
        }

        public void WriteLine(string message)
        {
            SysDebug.WriteLine(message);
        }

        public void Write(object item)
        {
            SysDebug.Write(item);
        }
    }
}
