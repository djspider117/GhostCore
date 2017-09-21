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
    }
}
