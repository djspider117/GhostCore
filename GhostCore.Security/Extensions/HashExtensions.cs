using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GhostCore.Security.Cryptography.Hash;

namespace GhostCore.Security.Extensions
{
    public static class HashExtensions
    {
        private static SHA1 _sha1;
        private static MD5 _md5;

        static HashExtensions()
        {
            _sha1 = new SHA1();
            _md5 = new MD5();
        }

        public static string SHA1_StringToString(this string value)
        {
            return _sha1.HashToString(value, true);
        }

        public static byte[] SHA1_StrintToBytes(this string value)
        {
            return _sha1.Hash(value);
        }

        public static string SHA1_BytesToString(this byte[] value)
        {
            return _sha1.HashToString(value, true);
        }

        public static byte[] SHA1_BytesToBytes(this byte[] value)
        {
            return _sha1.Hash(value);
        }
    }
}
