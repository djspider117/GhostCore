using System;
using System.Collections.Generic;
using System.Text;

namespace GhostCore
{
    public interface ISafeTaskResult
    {
        bool IsFaulted { get; }
        string FailReason { get; }
        int HResult { get; }

        Exception DetailedException { get; }
    }
}
