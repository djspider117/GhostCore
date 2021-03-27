using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.Security.Protocols
{
    public class RandomNumberGenerator
    {
        public static byte[] GenerateStrongRandom()
        {
            byte[] data = new byte[16];
            using (RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider())
                cryptoServiceProvider.GetBytes(data);
            return data;
        }

        public static byte[] GenerateStrongRandom(int bytes)
        {
            byte[] data = new byte[bytes];
            using (RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider())
                cryptoServiceProvider.GetBytes(data);
            return data;
        }
    }
}
