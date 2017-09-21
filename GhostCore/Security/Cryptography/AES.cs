using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.Security.Cryptography
{
    public class AES
    {
        #region Static Methods and Properties

        public static byte[] GenerateIV()
        {
            return new AesCryptoServiceProvider().IV;
        }

        public static byte[] GenerateKey()
        {
            byte[] data = new byte[16];
            using (RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider())
                cryptoServiceProvider.GetBytes(data);
            return data;
        }

        #endregion

        #region Fields

        private byte[] _iv;
        private byte[] _key;

        #endregion

        #region Properties

        public byte[] IV
        {
            get { return _iv; }
            set { _iv = value; }
        }
        public byte[] Key
        {
            get { return _key; }
            set { _key = value; }
        }

        #endregion

        #region Constructors and initialization

        public AES()
        {
            _iv = GenerateIV();
            _key = GenerateKey();
        }

        public AES(byte[] key, byte[] iv)
        {
            _key = key;
            _iv = iv;
        }

        #endregion

        #region Encryption Methods

        public byte[] Encrypt(byte[] input)
        {
            byte[] bytes;
            using (AesCryptoServiceProvider cryptoServiceProvider = new AesCryptoServiceProvider())
            {
                cryptoServiceProvider.Key = _key;
                cryptoServiceProvider.IV = IV;
                cryptoServiceProvider.Padding = PaddingMode.PKCS7;
                ICryptoTransform encryptor = ((SymmetricAlgorithm)cryptoServiceProvider).CreateEncryptor(cryptoServiceProvider.Key, cryptoServiceProvider.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                            streamWriter.Write(Encoding.Default.GetString(input));
                        bytes = memoryStream.ToArray();
                    }
                }
            }
            return bytes;
        }
        public byte[] Encrypt(string input)
        {
            using (AesCryptoServiceProvider cryptoServiceProvider = new AesCryptoServiceProvider())
            {
                cryptoServiceProvider.Key = _key;
                cryptoServiceProvider.IV = IV;
                cryptoServiceProvider.Padding = PaddingMode.PKCS7;
                ICryptoTransform encryptor = ((SymmetricAlgorithm)cryptoServiceProvider).CreateEncryptor(cryptoServiceProvider.Key, cryptoServiceProvider.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                            streamWriter.Write(input);
                        return memoryStream.ToArray();
                    }
                }
            }
        }
        public string EncryptToString(byte[] input)
        {
            return Encoding.Default.GetString(Encrypt(input));
        }
        public string EncryptToString(string input, bool safeString = false)
        {
            if (safeString)
                return Convert.ToBase64String(Encrypt(input));

            return Encoding.Default.GetString(Encrypt(input));
        }

        #endregion

        #region Decryption Methods

        public byte[] Decrypt(byte[] input)
        {
            string s;
            using (AesCryptoServiceProvider cryptoServiceProvider = new AesCryptoServiceProvider())
            {
                cryptoServiceProvider.Key = _key;
                cryptoServiceProvider.IV = IV;
                cryptoServiceProvider.Padding = PaddingMode.PKCS7;
                ICryptoTransform decryptor = ((SymmetricAlgorithm)cryptoServiceProvider).CreateDecryptor(cryptoServiceProvider.Key, cryptoServiceProvider.IV);
                using (MemoryStream memoryStream = new MemoryStream(input))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            s = streamReader.ReadToEnd();
                    }
                }
            }
            return Encoding.Default.GetBytes(s);
        }
        public byte[] Decrypt(string input, bool safeString = false)
        {
            string s;
            using (AesCryptoServiceProvider cryptoServiceProvider = new AesCryptoServiceProvider())
            {
                cryptoServiceProvider.Key = _key;
                cryptoServiceProvider.IV = IV;
                cryptoServiceProvider.Padding = PaddingMode.PKCS7;
                ICryptoTransform decryptor = ((SymmetricAlgorithm)cryptoServiceProvider).CreateDecryptor(cryptoServiceProvider.Key, cryptoServiceProvider.IV);

                byte[] bytes = null;
                if (safeString)
                    bytes = Convert.FromBase64String(input);
                else
                    bytes = Encoding.Default.GetBytes(input);

                using (MemoryStream memoryStream = new MemoryStream(bytes))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            s = streamReader.ReadToEnd();
                    }
                }
            }
            return Encoding.Default.GetBytes(s);
        }
        public string DecryptToString(byte[] input)
        {
            throw new NotImplementedException();
        }
        public string DecryptToString(string input, bool safeString = false)
        {
            using (AesCryptoServiceProvider cryptoServiceProvider = new AesCryptoServiceProvider())
            {
                cryptoServiceProvider.Key = _key;
                cryptoServiceProvider.IV = IV;
                cryptoServiceProvider.Padding = PaddingMode.PKCS7;
                ICryptoTransform decryptor = ((SymmetricAlgorithm)cryptoServiceProvider).CreateDecryptor(cryptoServiceProvider.Key, cryptoServiceProvider.IV);

                byte[] bytes = null;
                if (safeString)
                    bytes = Convert.FromBase64String(input);
                else
                    bytes = Encoding.Default.GetBytes(input);

                using (MemoryStream memoryStream = new MemoryStream(bytes))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            return streamReader.ReadToEnd();
                    }
                }
            }
        }

        #endregion

    }
}
