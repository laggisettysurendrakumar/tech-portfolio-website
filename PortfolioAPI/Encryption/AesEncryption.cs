using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Buffers.Text;

public static class AesEncryption
{
    public static string DecryptClientKey(string encryptedBase64, string keyBase64, string ivBase64)
    {
        try
        {
            byte[] cipherText = Convert.FromBase64String(encryptedBase64);
            byte[] key = Convert.FromBase64String(keyBase64);
            byte[] iv = Convert.FromBase64String(ivBase64);

            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = key;
            aes.IV = iv;

            using var ms = new MemoryStream(cipherText);
            using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using var reader = new StreamReader(cs);
            return reader.ReadToEnd();
        }
        catch (Exception ex)
        {
            // Handle any error (e.g., decryption failure)
            throw new InvalidOperationException("Error decrypting client key", ex);
        }
    }

    public static string Encrypt(string plainText, string masterKey)
    {
        using var aes = Aes.Create();
        aes.Key = GetKeyBytes(masterKey);
        aes.GenerateIV();

        using var ms = new MemoryStream();
        ms.Write(aes.IV, 0, aes.IV.Length); // prepend IV

        using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var writer = new StreamWriter(cs))
        {
            writer.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    public static string Decrypt(string cipherText, string masterKey)
    {
        var fullCipher = Convert.FromBase64String(cipherText);
        using var aes = Aes.Create();
        aes.Key = GetKeyBytes(masterKey);

        var iv = new byte[16];
        Array.Copy(fullCipher, 0, iv, 0, iv.Length);
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(fullCipher, 16, fullCipher.Length - 16);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var reader = new StreamReader(cs);

        return reader.ReadToEnd();
    }

    private static byte[] GetKeyBytes(string key)
    {
        using var sha = SHA256.Create();
        return sha.ComputeHash(Encoding.UTF8.GetBytes(key));
    }

}