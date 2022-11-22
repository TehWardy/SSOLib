using Security.Data.Interfaces;

namespace Security.Data.Brokers.Encryption
{
    public class PasswordEncryptionBroker : IPasswordEncryptionBroker
    {
        private readonly ICrypto<string> crypto;

        public PasswordEncryptionBroker(ICrypto<string> crypto)
        {
            this.crypto = crypto;
        }

        public string Encrypt(string password)
            => crypto.Encrypt(password);

        public bool EncryptedAndPlainTextAreEqual(string encrypted, string plainText)
            => crypto.Decrypt(encrypted) == plainText;
    }
}
