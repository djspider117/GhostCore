using GhostCore;
using GhostCore.Services;
using System.Diagnostics;

namespace GhostCore.Services.Logging
{
    public static class Logger
    {
        private static ILoggingService _svc;

        public static void Configure(ILoggingService svc)
        {
            _svc = svc;
        }

        public static void LogMessage(string msg, LoggingLevel loggingLevel = LoggingLevel.Information) => _svc?.LogMessage(msg, loggingLevel);

        public static void LogCritical(string msg) => LogMessage(msg, LoggingLevel.Critical);
        public static void LogError(string msg) => LogMessage(msg, LoggingLevel.Error);
        public static void LogWarning(string msg) => LogMessage(msg, LoggingLevel.Warning);
        public static void LogInfo(string msg) => LogMessage(msg, LoggingLevel.Information);
        public static void LogVerbose(string msg) => LogMessage(msg, LoggingLevel.Verbose);
    }

}