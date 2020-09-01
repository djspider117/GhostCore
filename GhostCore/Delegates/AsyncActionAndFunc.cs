using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.Delegates
{
    public delegate Task AsyncAction();
    public delegate Task AsyncAction<T>(T arg);
    public delegate Task AsyncAction<T1, T2>(T1 arg1, T2 arg2);
    public delegate Task AsyncAction<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg);

    public delegate Task<TReturn> AsyncFunc<TReturn>();
    public delegate Task<TReturn> AsyncFunc<T, TReturn>(T arg);
    public delegate Task<TReturn> AsyncFunc<T1, T2, TReturn>(T1 arg1, T2 arg2);
    public delegate Task<TReturn> AsyncFunc<T1, T2, T3, TReturn>(T1 arg1, T2 arg2, T3 arg);
}
