﻿using System.IO;
using System.Threading.Tasks;

namespace GhostCore.Extensions
{
    /// <summary>
    /// Extensions that can be used with a <see cref="Stream"/>
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Converts the <paramref name="input"/> <see cref="Stream"/> to a byte array.
        /// </summary>
        /// <remarks>
        /// The method will keep reading (and copying into a MemoryStream) until it runs out of data. It then asks the MemoryStream to return a copy of the data in an array.
        /// </remarks>
        public static byte[] ToBytes(this Stream input)
        {
            using var memStream = new MemoryStream();
            input.CopyTo(memStream);
            return memStream.ToArray();
        }

        public static async Task<byte[]> ToBytesAsync(this Stream input)
        {
            using var memStream = new MemoryStream();
            await input.CopyToAsync(memStream);
            return memStream.ToArray();
        }
    }
}