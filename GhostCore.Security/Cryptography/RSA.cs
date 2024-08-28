using System.Security.Cryptography;

namespace GhostCore.Security.Cryptography
{
    public class RSA
    {
        public static byte[] Sign(byte[] data, byte[] key)
        {
            RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
            cryptoServiceProvider.ImportCspBlob(key);
            return cryptoServiceProvider.SignData(data, "RSA");
        }

        public static bool VerifySignature(byte[] data, byte[] key, byte[] sig)
        {
            RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
            cryptoServiceProvider.ImportCspBlob(key);
            return cryptoServiceProvider.VerifyData(data, "RSA", sig);
        }
    }
}
