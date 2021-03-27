namespace GhostCore.Services.Logging
{
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
