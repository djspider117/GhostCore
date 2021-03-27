using System;

namespace GhostCore
{
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
}
