using GhostCore.Security.Cryptography.Hash.Abstract;
using System.Security.Cryptography;

namespace GhostCore.Security.Cryptography.Hash
{
    public class MD5 : HashStrategyBase
    {
        static MD5()
        {
            cryptoServiceProvider = new MD5CryptoServiceProvider();
        }
    }
}
