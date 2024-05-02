using System.Security.Cryptography;

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
