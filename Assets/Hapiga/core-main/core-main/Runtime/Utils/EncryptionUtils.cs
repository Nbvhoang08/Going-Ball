using System;
using System.IO;
using System.Security.Cryptography;

namespace Hapiga.Core.Runtime.Utils
{
    public static class EncryptionUtils
    {
        private static byte[] auxVector;
        private static string auxString;

        public static byte[] EncryptString(string toEncrypt, byte[] aesKey, byte[] aesIV)
        {
            if(string.IsNullOrEmpty(toEncrypt))
            { UnityEngine.Debug.LogError("String to encrypt is null or empty!"); throw new ArgumentException("String to encrypt is null or empty!"); }
            if(aesKey == null || aesKey.Length != 32)
            { UnityEngine.Debug.LogError("Encryption key is null or has illegal length!"); throw new ArgumentException("Encryption key is null or has illegal length!"); }
            if(aesIV == null || aesIV.Length != 16)
            { UnityEngine.Debug.LogError("Encryption IV is null or has illegal length!"); throw new ArgumentException("Encryption IV is null or has illegal length!"); }

            auxVector = null;

            using(Aes enAes = Aes.Create())
            {
                enAes.Key = aesKey;
                enAes.IV = aesIV;

                ICryptoTransform encryptor = enAes.CreateEncryptor(enAes.Key, enAes.IV);

                using(MemoryStream msEncrypt = new MemoryStream())
                {
                    using(CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using(StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(toEncrypt);
                        }
                        auxVector = msEncrypt.ToArray();
                    }
                }
            }
            return auxVector;
        }

        public static string DecryptBytes(byte[] toDecrypt, byte[] aesKey, byte[] aesIV)
        {
            if(toDecrypt == null || toDecrypt.Length <= 0)
            { UnityEngine.Debug.LogError("Byte[] to decrypt is null or empty!"); throw new ArgumentException("Byte[] to decrypt is null or empty!"); }
            if(aesKey == null || aesKey.Length != 32)
            { UnityEngine.Debug.LogError("Encryption key is null or has illegal length!"); throw new ArgumentException("Encryption key is null or has illegal length!"); }
            if(aesIV == null || aesIV.Length != 16)
            { UnityEngine.Debug.LogError("Encryption IV is null or has illegal length!"); throw new ArgumentException("Encryption IV is null or has illegal length!"); }

            auxString = string.Empty;

            using(Aes decAes = Aes.Create())
            {
                decAes.Key = aesKey;
                decAes.IV = aesIV;

                ICryptoTransform decryptor = decAes.CreateDecryptor(decAes.Key, decAes.IV);

                using(MemoryStream msDecrypt = new MemoryStream(toDecrypt))
                {
                    using(CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using(StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            auxString = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return auxString;
        }

        public static void GenerateAesKeyAndIV(out byte[] aesKey, out byte[] aesIV)
        {
            using(Aes aes = Aes.Create())
            {
                aesKey = aes.Key;
                aesIV = aes.IV;
            }
        }
    }
}
