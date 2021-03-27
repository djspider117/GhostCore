using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GhostCore.Security.Cryptography.Hash.Abstract;

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
