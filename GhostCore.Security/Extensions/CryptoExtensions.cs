using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GhostCore.Security.Cryptography;

namespace GhostCore.Security.Extensions
{
    public static class CryptoExtensions
    {
        private static AES _aes;

        public static AES AESReference { get { return _aes; } }

        static CryptoExtensions()
        {
            _aes = new AES();
        }

        public static byte[] Encrypt(this byte[] input)
        {
            return _aes.Encrypt(input);
        }
        public static string EncryptToString(this byte[] input)
        {
            return _aes.EncryptToString(input);
        }
        public static byte[] Encrypt(this string input)
        {
            return _aes.Encrypt(input);
        }
        public static string EncryptToString(this string input)
        {
            return _aes.EncryptToString(input);
        }

        public static byte[] Sign(this byte[] data, byte[] key)
        {
            return RSA.Sign(data, key);
        }
        public static bool VerifySignature(this byte[] data, byte[] key, byte[] sig)
        {
            return RSA.VerifySignature(data, key, sig);
        }
    }
}
