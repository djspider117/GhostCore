using System;
using System.Collections.Generic;

namespace GhostCore.Logging
{
    public sealed class CompositeLoggingService : ILoggingService
    {
        public List<ILoggingService> LoggingServices { get; set; }

        public CompositeLoggingService()
        {
            LoggingServices = new List<ILoggingService>();
        }

        public CompositeLoggingService(IEnumerable<ILoggingService> services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            LoggingServices = new List<ILoggingService>(services);
        }

        public void LogMessage(string msg, LoggingLevel loggingLevel = LoggingLevel.Information)
        {
            var now = DateTime.UtcNow;
            for (int i = 0; i < LoggingServices.Count; i++)
            {
                LoggingServices[i].LogMessage(msg, loggingLevel);
            }
        }

        public void LogCritical(string msg) => LogMessage(msg, LoggingLevel.Critical);
        public void LogError(string msg) => LogMessage(msg, LoggingLevel.Error);
        public void LogWarning(string msg) => LogMessage(msg, LoggingLevel.Warning);
        public void LogInfo(string msg) => LogMessage(msg, LoggingLevel.Information);
        public void LogVerbose(string msg) => LogMessage(msg, LoggingLevel.Verbose);

        public void Dispose()
        {
            for (int i = 0; i < LoggingServices.Count; i++)
            {
                LoggingServices[i].Dispose();
            }

            LoggingServices.Clear();
            LoggingServices = null;
        }
    }

}