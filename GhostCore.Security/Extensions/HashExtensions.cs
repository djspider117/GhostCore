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

        public static long QuickHash(this string value)
        {
            const long p = 31;
            const long m = (long)(1e9 + 9);
            long hashValue = 0;
            long p_pow = 1;
            foreach (var c in value)
            {
                hashValue = (hashValue + (c - 'a' + 1) * p_pow) % m;
                p_pow = (p_pow * p) % m;
            }
            return hashValue;
        }
    }
}
