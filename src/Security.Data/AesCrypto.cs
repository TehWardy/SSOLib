using Security.Data.Interfaces;
using System.Text;
using System.Text.Json;

namespace Security.Data
{
    public class AesCrypto<T> : ISymmetricCrypto<T>
    {
        readonly string decryptionKey;
        readonly AesThenHmac crypto = new();

        public AesCrypto(string key) => decryptionKey = key;

        public string Encrypt(T source, string key)
        {
            Encoding e = Encoding.UTF8;
            byte[] rawData = e.GetBytes(JsonSerializer.Serialize(source));
            byte[] cipherData = crypto.SimpleEncryptWithPassword(rawData, key, null);
            return Convert.ToBase64String(cipherData);
        }

        public string Encrypt(T source)
        {
            if (decryptionKey == null)
            {
                throw new InvalidOperationException("Decryption key not set.");
            }

            Encoding e = Encoding.UTF8;
            byte[] rawData = e.GetBytes(System.Text.Json.JsonSerializer.Serialize(source));
            byte[] cipherData = crypto.SimpleEncryptWithPassword(rawData, decryptionKey, null);
            return Convert.ToBase64String(cipherData);
        }

        public T Decrypt(string source, string key)
        {
            Encoding e = Encoding.UTF8;
            byte[] decryptedBytes = crypto.SimpleDecryptWithPassword(Convert.FromBase64String(source), key);
            return JsonSerializer.Deserialize<T>(e.GetString(decryptedBytes));
        }

        public T Decrypt(string source)
        {
            if (decryptionKey == null)
            {
                throw new InvalidOperationException("Decryption key not set.");
            }

            Encoding e = Encoding.UTF8;
            byte[] decryptedBytes = crypto.SimpleDecryptWithPassword(Convert.FromBase64String(source), decryptionKey);
            return JsonSerializer.Deserialize<T>(e.GetString(decryptedBytes));
        }
    }
}