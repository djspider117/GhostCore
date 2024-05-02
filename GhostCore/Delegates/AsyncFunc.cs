using System.Threading.Tasks;

namespace GhostCore
{
    public delegate Task<TReturn> AsyncFunc<TReturn>();
    public delegate Task<TReturn> AsyncFunc<T, TReturn>(T arg);
    public delegate Task<TReturn> AsyncFunc<T1, T2, TReturn>(T1 arg1, T2 arg2);
    public delegate Task<TReturn> AsyncFunc<T1, T2, T3, TReturn>(T1 arg1, T2 arg2, T3 arg);
}
