using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GhostCore;
using GhostCore.DebugUtils;

public static class GlobalExtensions
{
    public static void TryOrSkip(this object any, Action callback)
    {
        try
        {
            callback();
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
    public static async Task TryOrSkipAsync(this object any, Func<Task> callback)
    {
        try
        {
            await callback();
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    public static void Retry(this object any, Func<bool> action)
    {
        int currentRetryCount = CoreConfiguration.RETRY_COUNT;
        bool willRetry = true;
        do
        {
            int curRetry = (int)(CoreConfiguration.RETRY_COUNT - currentRetryCount + 1);
            string methodName = action.GetMethodInfo().Name;
            Debug.Log("Retry count: " + curRetry + ". Calling method : " + methodName);
            willRetry = action();
            Debug.Log("Finished Retry count: " + curRetry + ". Calling method was : " + methodName);
        }
        while (--currentRetryCount > 0 && willRetry);
    }
    public static async Task RetryAsync(this object any, Func<Task<bool>> action)
    {
        int currentRetryCount = CoreConfiguration.RETRY_COUNT;
        bool willRetry = true;
        do
        {
            int curRetry = (int)(CoreConfiguration.RETRY_COUNT - currentRetryCount + 1);
            string methodName = action.GetMethodInfo().Name;
            System.Diagnostics.Debug.WriteLine("Retry count: " + curRetry + ". Calling method : " + methodName);
            willRetry = await action();
            System.Diagnostics.Debug.WriteLine("Finished Retry count: " + curRetry + ". Calling method was : " + methodName);
        }
        while (--currentRetryCount > 0 && willRetry);
    }

    public static string FormatException(this Exception ex)
    {
#if DEBUG
        return string.Format("{0}\r\nDetails:\r\n{1}", ex.Message, ex.ToString());
#else
         return ex.Message;
#endif
    }
}


public static class Debug
{
    public static void Log(object item)
    {
#if DEBUG

        DebugLogger.Default.Log(item);
#endif
    }
}
