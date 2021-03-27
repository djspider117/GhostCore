using GhostCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

public static class SafeGlobalExtensions
{
    public static ISafeTaskResult SafeTry(this object any, Func<ISafeTaskResult> executor)
    {
        try
        {
            return executor();
        }
        catch (Exception ex)
        {
            return new SafeTaskResult(ex.Message, ex);
        }
    }

    public static async Task<ISafeTaskResult> SafeTry(this object any, Func<Task<ISafeTaskResult>> executor)
    {
        try
        {
            return await executor();
        }
        catch (Exception ex)
        {
            return new SafeTaskResult(ex.Message, ex);
        }
    }
}
