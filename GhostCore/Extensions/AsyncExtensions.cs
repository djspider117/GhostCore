﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GhostCore.Extensions
{
    /// <summary>
    /// Async-related extension methods.
    /// </summary>
    public static class AsyncExtensions
    {
        /// <summary>
        /// Runs a specific task in parallel from a list of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to operate on.</typeparam>
        /// <param name="items">The source items.</param>
        /// <param name="func">Returns the task to run in parallel from <typeparamref name="T"/>.</param>
        /// <returns>A task representing the completion of all tasks.</returns>
        public static Task InParallel<T>(this IEnumerable<T> items, Func<T, Task> func)
        {
            var tasks = items.Select(func);

            return Task.WhenAll(tasks);
        }

        /// <summary>
        /// Runs a specific task in parallel from a list of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to operate on.</typeparam>
        /// <typeparam name="T2">The return type.</typeparam>
        /// <param name="items">The source items.</param>
        /// <param name="func">Returns the task to run in parallel from <typeparamref name="T"/>.</param>
        /// <returns>A <see cref="Task"/> representing the completion of all tasks. The result is an array of all the returned values.</returns>
        public static Task<T2[]> InParallel<T, T2>(this IEnumerable<T> items, Func<T, Task<T2>> func)
        {
            var tasks = items.Select(func);

            return Task.WhenAll(tasks);
        }
    }
}
