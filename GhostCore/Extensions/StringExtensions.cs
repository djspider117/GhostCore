using System;
using System.Collections.Generic;
using System.Text;

namespace GhostCore.Extensions
{
    public static class StringExtensions
    {
        public static string ToTitleCase(this string value)
        {
            if (value == null)
                return null;
            if (value.Length == 0)
                return value;

            StringBuilder result = new StringBuilder(value);
            result[0] = char.ToUpper(result[0]);
            for (int i = 1; i < result.Length; ++i)
            {
                if (char.IsWhiteSpace(result[i - 1]))
                    result[i] = char.ToUpper(result[i]);
                else
                    result[i] = char.ToLower(result[i]);
            }
            return result.ToString();
        }

        public static bool IsInteger(this string value, out long val)
        {
            return long.TryParse(value, out val);
        }
        public static bool IsInteger(this string value)
        {
            return long.TryParse(value, out long dummy);
        }

        public static bool IsReal(this string value, out double val)
        {
            return double.TryParse(value, out val);
        }
        public static bool IsReal(this string value)
        {
            return double.TryParse(value, out double dummy);
        }
    }
}
