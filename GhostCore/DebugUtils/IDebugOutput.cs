using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.DebugUtils
{
    public interface IDebugOutput
    {
        void Log(object item);
        void WriteLine(string message);
        void Write(object item);
    }
}
