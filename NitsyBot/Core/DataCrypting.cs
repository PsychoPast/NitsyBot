using System;
using System.Security.Cryptography;
using System.Text;

namespace NitsyBot.Core.Crypt
{
    public static class DataCrypting
    {
        public static string GenerateKey(int keysize)
        {
            using RijndaelManaged aesEncryption = new RijndaelManaged
            {
                KeySize = keysize,
                BlockSize = 128,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };
            aesEncryption.GenerateIV();
            string ivStr = Convert.ToBase64String(aesEncryption.IV);
            aesEncryption.GenerateKey();
            string keyStr = Convert.ToBase64String(aesEncryption.Key);
            string completeKey = ivStr + "," + keyStr;
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(completeKey));
        }

        public static string[] Encrypt(string enemail, string enpass, string enusername, string key, int keysize)
        {
            using RijndaelManaged aesEncryption = new RijndaelManaged
            {
                KeySize = keysize,
                BlockSize = 128,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                IV = Convert.FromBase64String(Encoding.UTF8.GetString(Convert.FromBase64String(key)).Split(',')[0]),
                Key = Convert.FromBase64String(Encoding.UTF8.GetString(Convert.FromBase64String(key)).Split(',')[1])
            };
            byte[] emailen = Encoding.UTF8.GetBytes(enemail);
            byte[] passen = Encoding.UTF8.GetBytes(enpass);
            byte[] usernameen = Encoding.UTF8.GetBytes(enusername);
            ICryptoTransform crypto = aesEncryption.CreateEncryptor();
            byte[] email = crypto.TransformFinalBlock(emailen, 0, emailen.Length);
            byte[] pass = crypto.TransformFinalBlock(passen, 0, passen.Length);
            byte[] username = crypto.TransformFinalBlock(usernameen, 0, usernameen.Length);
            string finalemail = Convert.ToBase64String(email);
            string finalpass = Convert.ToBase64String(pass);
            string finalusername = Convert.ToBase64String(username);
            string test = $"{finalemail} {finalpass} {finalusername}";
            string[] testt = test.Split(" ");
            return testt;
        }

        public static string Decrypt(string encrypted, string key, int keysize)
        {
            using RijndaelManaged aesEncryption = new RijndaelManaged
            {
                KeySize = keysize,
                BlockSize = 128,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                IV = Convert.FromBase64String(Encoding.UTF8.GetString(Convert.FromBase64String(key)).Split(',')[0]),
                Key = Convert.FromBase64String(Encoding.UTF8.GetString(Convert.FromBase64String(key)).Split(',')[1])
            };
            ICryptoTransform decrypto = aesEncryption.CreateDecryptor();
            byte[] encryptedBytes = Convert.FromBase64CharArray(encrypted.ToCharArray(), 0, encrypted.Length);
            return Encoding.UTF8.GetString(decrypto.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length));
        }
    }
}