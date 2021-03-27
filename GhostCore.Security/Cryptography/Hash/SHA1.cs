using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GhostCore.Security.Cryptography.Hash.Abstract;

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
