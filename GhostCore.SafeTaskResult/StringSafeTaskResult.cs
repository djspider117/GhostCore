using System;

namespace GhostCore
{
    public class StringSafeTaskResult : SafeTaskResult, ISafeTaskResult<string>
    {
        public string ResultValue { get; }

        public StringSafeTaskResult(string resultValue)
        {
            ResultValue = resultValue;
        }

        public StringSafeTaskResult(bool isError, string failReason) : base(failReason)
        {
        }

        public StringSafeTaskResult(bool isError, string failReason, Exception ex) : base(failReason, ex)
        {
        }

        public StringSafeTaskResult(bool isError, string failReason, int hresult) : base(failReason, hresult)
        {
        }

        public StringSafeTaskResult(bool isError, string failReason, Exception ex, int hresult) : base(failReason, ex, hresult)
        {
        }

        public ISafeTaskResult<K> Cast<K>()
        {
            return this as ISafeTaskResult<K>;
        }
    }
}
