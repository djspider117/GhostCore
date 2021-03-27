using System.Threading;

namespace Internal
{
    internal static class __Incrementor
    {
        private static int _internalValue = 0;

        public static int GetIncrementedValue() => Interlocked.Increment(ref _internalValue);
    }
}
