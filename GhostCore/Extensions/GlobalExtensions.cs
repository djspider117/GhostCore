using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GhostCore;

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
            Debug.WriteLine(ex);
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
            Debug.WriteLine(ex);
        }
    }

    public static void Retry(this object any, Func<bool> action, int retryCount = 3)
    {
        int currentRetryCount = retryCount;
        bool willRetry = true;
        do
        {
            int curRetry = (int)(retryCount - currentRetryCount + 1);
            string methodName = action.GetMethodInfo().Name;
            Debug.WriteLine("Retry count: " + curRetry + ". Calling method : " + methodName);
            willRetry = action();
            Debug.WriteLine("Finished Retry count: " + curRetry + ". Calling method was : " + methodName);
        }
        while (--currentRetryCount > 0 && willRetry);
    }
    public static async Task RetryAsync(this object any, Func<Task<bool>> action, int retryCount = 3)
    {
        int currentRetryCount = retryCount;
        bool willRetry = true;
        do
        {
            int curRetry = (int)(retryCount - currentRetryCount + 1);
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