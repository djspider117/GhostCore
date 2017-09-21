using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.Security.Cryptography.Hash.Abstract
{
    public class HashStrategyBase : IHashStrategy
    {
        protected static HashAlgorithm cryptoServiceProvider;

        public byte[] Hash(byte[] input)
        {
            return cryptoServiceProvider.ComputeHash(input);
        }
        public byte[] Hash(string input)
        {
            return Hash(Encoding.Default.GetBytes(input));
        }
        public string HashToString(byte[] input, bool safeString = false)
        {
            if (safeString)
                return Convert.ToBase64String(input);
            return Encoding.Default.GetString(Hash(input));
        }
        public string HashToString(string input, bool safeString = false)
        {
            return HashToString(Encoding.Default.GetBytes(input), safeString);
        }

        ~HashStrategyBase()
        {
            if (cryptoServiceProvider != null)
                cryptoServiceProvider.Dispose();
        }
    }
}
