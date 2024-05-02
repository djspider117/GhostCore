using GhostCore.Security.Cryptography.Hash.Abstract;
using System.Security.Cryptography;

namespace GhostCore.Security.Cryptography.Hash
{
    public class SHA1 : HashStrategyBase
    {
        static SHA1()
        {
            cryptoServiceProvider = new SHA1CryptoServiceProvider();
        }
    }
}
