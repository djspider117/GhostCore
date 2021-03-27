using System;
using System.Diagnostics;

namespace GhostCore.Logging
{
    public sealed class DebugLoggingService : ILoggingService
    {
        public void LogMessage(string msg, LoggingLevel loggingLevel = LoggingLevel.Information)
        {
            var now = DateTime.UtcNow;
            Debug.WriteLine($"[{now.Year}.{now.Month:D2}.{now.Day:D2}][{loggingLevel}]");
        }

        public void LogCritical(string msg) => LogMessage(msg, LoggingLevel.Critical);
        public void LogError(string msg) => LogMessage(msg, LoggingLevel.Error);
        public void LogWarning(string msg) => LogMessage(msg, LoggingLevel.Warning);
        public void LogInfo(string msg) => LogMessage(msg, LoggingLevel.Information);
        public void LogVerbose(string msg) => LogMessage(msg, LoggingLevel.Verbose);

        public void Dispose() => Debug.Close();
    }

}