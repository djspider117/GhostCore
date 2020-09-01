using System;
using System.Collections.Generic;
using System.Text;

namespace GhostCore.Foundation
{
    public interface ISafeTaskResult
    {
        bool IsFaulted { get; }
        string FailReason { get; }
        int HResult { get; }

        Exception DetailedException { get; }
    }

    public interface ISafeTaskResult<out T> : ISafeTaskResult
    {
        T ResultValue { get; }
        ISafeTaskResult<K> Cast<K>();
    }

    public class SafeTaskResult : ISafeTaskResult
    {
        public static ISafeTaskResult Ok { get; private set; } = new SafeTaskResult();
        public static ISafeTaskResult FailWithNoReason { get; private set; } = new SafeTaskResult(failReason: "No Reason");

        public virtual bool IsFaulted { get; protected set; }
        public virtual string FailReason { get; protected set; }
        public virtual int HResult { get; protected set; }
        public virtual Exception DetailedException { get; protected set; }

        public SafeTaskResult()
        {
            IsFaulted = false;
        }

        public SafeTaskResult(string failReason)
        {
            IsFaulted = true;
            FailReason = failReason;
        }

        public SafeTaskResult(string failReason, Exception ex, int hresult)
        {
            IsFaulted = true;
            FailReason = failReason;
            DetailedException = ex;
            HResult = hresult;
        }
        public SafeTaskResult(string failReason, Exception ex)
        {
            IsFaulted = true;
            FailReason = failReason;
            DetailedException = ex;
        }

        public SafeTaskResult(string failReason, int hresult)
        {
            IsFaulted = true;
            FailReason = failReason;
            HResult = hresult;
        }
    }

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
    }

}
