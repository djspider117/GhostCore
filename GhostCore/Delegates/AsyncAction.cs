using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore
{
    public delegate Task AsyncAction();
    public delegate Task AsyncAction<T>(T arg);
    public delegate Task AsyncAction<T1, T2>(T1 arg1, T2 arg2);
    public delegate Task AsyncAction<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg);
}
