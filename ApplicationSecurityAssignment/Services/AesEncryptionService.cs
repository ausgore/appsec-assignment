using System.Security.Cryptography;
using System.Text;

namespace ApplicationSecurityAssignment.Services
{
    public class AesEncryptionService : IEncryptionService
    {
        private readonly byte[] key;
        private readonly byte[] iv;

        public AesEncryptionService(IConfiguration configuration)
        {
            key = Convert.FromBase64String(configuration["EncryptionSettings:AesKey"]);
            iv = Convert.FromBase64String(configuration["EncryptionSettings:AesIV"]);
        }

        public string Encrypt(string text)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(text);
            }
            return Convert.ToBase64String(msEncrypt.ToArray());
        }
        
        public string Decrypt(string cipher)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var msDecrypt = new MemoryStream(Convert.FromBase64String(cipher));
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            return srDecrypt.ReadToEnd();
        }
    }
}
