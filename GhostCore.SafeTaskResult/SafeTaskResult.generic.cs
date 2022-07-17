using System;

namespace GhostCore
{
    public class SafeTaskResult<T> : SafeTaskResult, ISafeTaskResult<T>
    {
        public T ResultValue { get; private set; }

        public SafeTaskResult(T value)
        {
            ResultValue = value;
        }

        public SafeTaskResult(string failReason) : base(failReason)
        {
        }

        public SafeTaskResult(string failReason, Exception ex) : base(failReason, ex)
        {
        }

        public SafeTaskResult(string failReason, int hresult) : base(failReason, hresult)
        {
        }

        public SafeTaskResult(string failReason, Exception ex, int hresult) : base(failReason, ex, hresult)
        {
        }

        public ISafeTaskResult<K> Cast<K>()
        {
            return this as ISafeTaskResult<K>;
        }

        public ISafeTaskResult<K> Convert<K>()
        {
            if (!IsFaulted)
                throw new InvalidOperationException("Convert should only be used when the task is faulted");

            return new SafeTaskResult<K>(FailReason, DetailedException, HResult);
        }
    }

    public static class STRFactory
    {
        public static SafeTaskResult<T> Make<T>(T result)
        {
            return new SafeTaskResult<T>(result);
        }
    }
}
