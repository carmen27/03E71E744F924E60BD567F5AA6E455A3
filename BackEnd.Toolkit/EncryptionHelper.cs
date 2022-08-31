using System.Security.Cryptography;
using System.Text;

namespace BackEnd.Toolkit
{
    public static class EncryptionHelper
    {
        public static string Decrypt(string seed, byte[] cipher)
        {
            using var md5 = MD5.Create();
            using var tdes = TripleDES.Create();
            tdes.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(seed));
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            using var transform = tdes.CreateDecryptor();
            var bytes = transform.TransformFinalBlock(cipher, 0, cipher.Length);
            return Encoding.UTF8.GetString(bytes);
        }

        public static byte[] Encrypt(string seed, string text)
        {
            using var md5 = MD5.Create();
            using var tdes = TripleDES.Create();
            tdes.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(seed));
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            using var transform = tdes.CreateEncryptor();
            var textBytes = Encoding.UTF8.GetBytes(text);
            var bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
            return bytes;
        }
    }
}