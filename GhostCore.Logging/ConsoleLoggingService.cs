using System;

namespace GhostCore.Services.Logging
{
    public sealed class ConsoleLoggingService : ILoggingService
    {
        private static object _syncRoot = new object();

        public void LogMessage(string msg, LoggingLevel loggingLevel = LoggingLevel.Information)
        {
            lock (_syncRoot)
            {
                var now = DateTime.UtcNow;
                var color = GetConsoleColor(loggingLevel);

                if (loggingLevel == LoggingLevel.Critical)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = color;
                }
                else
                {
                    Console.ForegroundColor = color;
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                Console.WriteLine($"[{now.Year}.{now.Month:D2}.{now.Day:D2}][{loggingLevel}]");

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }

        public void LogCritical(string msg) => LogMessage(msg, LoggingLevel.Critical);
        public void LogError(string msg) => LogMessage(msg, LoggingLevel.Error);
        public void LogWarning(string msg) => LogMessage(msg, LoggingLevel.Warning);
        public void LogInfo(string msg) => LogMessage(msg, LoggingLevel.Information);
        public void LogVerbose(string msg) => LogMessage(msg, LoggingLevel.Verbose);

        public void Dispose()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        private ConsoleColor GetConsoleColor(LoggingLevel level)
        {
            switch (level)
            {
                case LoggingLevel.Verbose:
                    return ConsoleColor.DarkGray;
                case LoggingLevel.Information:
                    return ConsoleColor.Gray;
                case LoggingLevel.Warning:
                    return ConsoleColor.Yellow;
                case LoggingLevel.Error:
                case LoggingLevel.Critical:
                    return ConsoleColor.Red;
                default:
                    return ConsoleColor.Gray;
            }
        }
    }

}