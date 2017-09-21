using System;

namespace GhostCore.Extensions
{
    public static class DateTimeExtensions
    {
        private static DateTime _jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long GetCurrentTimeMillis(this DateTime d)
        {
            return (long)((DateTime.UtcNow - _jan1st1970).TotalMilliseconds);
        }

        public static long CurrentTimeMillis
        {
            get { return GetCurrentTimeMillis(DateTime.Now); }
        }
    }
}
