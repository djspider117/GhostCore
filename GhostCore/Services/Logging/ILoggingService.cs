using System;

namespace GhostCore.Services.Logging
{

    /// <summary>
    /// Represents the destination of log messages.
    /// </summary>
    public interface ILoggingService : IDisposable
    {
        /// <summary>
        /// Logs a message to the current log channel.
        /// </summary>
        /// <param name="msg">The message to log.</param>
        /// <param name="loggingLevel">The logging level.</param>
        void LogMessage(string msg, LoggingLevel loggingLevel = LoggingLevel.Information);

        /// <summary>
        /// Logs a critical message to the current log channel.
        /// </summary> 
        /// /// <param name="msg">The message to log.</param>
        void LogCritical(string msg);

        /// <summary>
        /// Logs a error message to the current log channel.
        /// </summary> 
        /// /// <param name="msg">The message to log.</param>
        void LogError(string msg);

        /// <summary>
        /// Logs a warning message to the current log channel.
        /// </summary> 
        /// /// <param name="msg">The message to log.</param>
        void LogWarning(string msg);

        /// <summary>
        /// Logs an info message to the current log channel.
        /// </summary> 
        /// /// <param name="msg">The message to log.</param>
        void LogInfo(string msg);

        /// <summary>
        /// Logs a verbose message to the current log channel.
        /// </summary> 
        /// /// <param name="msg">The message to log.</param>
        void LogVerbose(string msg);
    }


    public enum LoggingLevel : byte
    {
        //
        // Summary:
        //     Log all messages.
        Verbose = 0,
        //
        // Summary:
        //     Log messages of information level and higher.
        Information = 1,
        //
        // Summary:
        //     Log messages of warning level and higher.
        Warning = 2,
        //
        // Summary:
        //     Log messages of error level and higher.
        Error = 3,
        //
        // Summary:
        //     Log only critical messages.
        Critical = 4
    }
}
