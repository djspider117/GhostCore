using System;
using System.Collections.Generic;

namespace GhostCore.Extensions
{
    public static class CollectionExtensions
    {
        public static void Apply<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }
    }
}
